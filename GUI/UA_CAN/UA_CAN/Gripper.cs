using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UA_CAN
{
    internal enum Hall
    {
        OFF = 0,
        ON  = 1,
    }

    internal class Gripper
    {
        internal USB2CAN _serial = new USB2CAN();
        internal CANablePro _can;

        private CancellationTokenSource _cts_request = new CancellationTokenSource();
        private CancellationTokenSource _cts_response = new CancellationTokenSource();
        public GripperPacket recvPacket = new GripperPacket();
        private GripperPacket sendPacket = new GripperPacket();  
        public readonly int[] baudrates;
        public string? portName;
        public string? lastPort;
        public packet_t? lastSendPacket;
        public byte count = 0;

        private CAN_TYPE myType = CAN_TYPE.CAN_FD;
        private int CAN_ID = 0x124;
        private int DXL_ID = 0x00;
        private ushort DXLInitPosition = 2000;
        private ushort DXLTargetPosition = 3200;
        

        public Gripper() 
        {
            _can = new CANablePro(_serial);
            baudrates = _serial.baudRates.ToArray();    // Deep Copy
            
        }

        enum CmdState
        {
            REQUEST = 0x01,
            RESPONSE = 0x02,
            MOTOR_DXL_RUN = 0x03,
            MOTOR_DXL_ARRIVED = 0x04,
            MOTOR_LSV_RUN = 0x05,
            MOTOR_LSV_ARRIVED = 0x06,
            HALL_SENSOR_RUN = 0x07,
            HALL_SENSOR_STOP = 0x08,
            LED_ON = 0x09,
            LED_OFF = 0x0A,
            CAN_START = 0x1F,
            CAN_STOP = 0x0F
        }

        enum GripperState 
        {
            DISCONNECTED = 0x00,
            CONNECTED    = 0x01,
            DXL_INIT     = 0x02,
            DXL_SUB      = 0x03,
            DXL_TARGET   = 0x04,
        }
        

        // 9
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct dxl_t
        {
            public byte   id;
            public UInt16 position;
            public UInt16 velocity;
            public byte   status;   // bool to byte
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct lsv_t
        {
            public byte id;
            public UInt16 position;
            public UInt16 velocity;
            public byte status;
        }


        //14
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ads_t
        {
            public byte toggleSwitch;   // bool to byte
            public Int16 raw;
            public byte status;         // bool to byte
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct color_t
        {
            public byte red;
            public byte green;
            public byte blue;
            public byte brightness;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct led_t
        {
            public byte ledSwitch;
            public color_t colors;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct cmd_t
        {
            public byte count;
            public byte command;
        }

        // 32 bytes
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct GripperPacket
        {
            public cmd_t cmd;
            public dxl_t dxl;
            public lsv_t lsv;
            public ads_t hallSensor;
            public led_t led;
            public byte crc;
        }

        public Dictionary<string, string> GetUSBDevices()
        {
            return _serial.GetUSBDevices();
        }


        public bool begin(string port, int baudrate = 115200)
        {
            _can.packetQueue.Clear();

            bool ret = _serial.begin(port, baudrate);
            if (ret)
            {
                portName = _serial.sp.PortName;
                lastPort = _serial.lastPort;

                _can.openChannel();
            }

            return ret;
        }

        public void portClose()
        {
            _can.closeChannel();
            _serial.close();
        }

        public packet_t? GetLast()
        {
            return _can?.GetLastPacket();
        }

        public void receivingPacket()
        {   

            _can.read();
        }

        public void receivingStop()
        {
            _can.stopRead();
        }

        public bool isUSBConnected()
        {
            return _serial.isUSBConnected();
        }

        public bool isUSBOpen()
        {
            return _serial.sp.IsOpen;
        }




        public void request() 
        {
            GripperPacket tempPacket = new GripperPacket();

            tempPacket.cmd.count = count++;
            tempPacket.cmd.command = (byte)CmdState.REQUEST;
            tempPacket.dxl.position = DXLInitPosition;

            canSend(0x126, tempPacket);
            //Thread.Sleep(100);
        }

        public void canStart() 
        {
            GripperPacket tempPacket = new GripperPacket();

            tempPacket.cmd.command = (byte)CmdState.CAN_START;

            request();
            
            //getData();

            //if(recvPacket.cmd.command == (byte)CmdState.RESPONSE) 
            //{
            //    Console.WriteLine("Ready to can Start");
            //    canSend(0x123, tempPacket);
            //}

            //recvPacket.cmd.command = 0;
            Console.WriteLine("Ready to can Start");
            canSend(0x123, tempPacket);
            //push(1000);

        }

        public void canStop() 
        {
            GripperPacket tempPacket = new GripperPacket();
            tempPacket.cmd.command = (byte)CmdState.CAN_STOP;
            Console.WriteLine("CAN Reading Stop");
            canSend(0x123, tempPacket);

        }

        public void switchHall(Hall hall)
        {
            GripperPacket tempPacket = new GripperPacket();

            tempPacket.dxl.position = recvPacket.dxl.position;
            tempPacket.lsv.position = recvPacket.lsv.position;
            if (hall == Hall.ON)
            {
                tempPacket.cmd.command = (byte)CmdState.HALL_SENSOR_RUN;
                tempPacket.hallSensor.toggleSwitch = 0x01;
                Console.WriteLine("Ready to HALL Start");
            }
            else
            {
                tempPacket.cmd.command = (byte)CmdState.HALL_SENSOR_STOP;
                tempPacket.hallSensor.toggleSwitch = 0x00;
                Console.WriteLine("Stop reading HALL");
            }
         
            canSend(0x123, tempPacket);

        }

        public void rotate(ushort value) 
        {
            GripperPacket tempPacket = new GripperPacket();

            tempPacket.cmd.command = (byte)CmdState.MOTOR_DXL_RUN;
            tempPacket.dxl.position = value;
            canSend(0x123, tempPacket);

        }

        public void push(ushort value) 
        {
            GripperPacket tempPacket = new GripperPacket();

            tempPacket.cmd.command = (byte)CmdState.MOTOR_LSV_RUN;
            tempPacket.lsv.position = value;
            Console.WriteLine("Push LSV");
            canSend(0x123, tempPacket);
        }





        public void canSend(int id, GripperPacket gripper) 
        {
            byte[] packet = preparePacekt(gripper);
            sendCANPacket(myType, id, CAN_DLC.FDCAN_DLC_BYTE_24, packet);
        }
        

        public void sendCANPacket(CAN_TYPE type, int id, CAN_DLC dlc, byte[] packet) 
        {
            _can.write(type, id, dlc, packet);
            lastSendPacket = _can._sendPacekt;

            //int[] sizes = new int[5];
            //sizes[0] = Marshal.SizeOf(typeof(cmd_t));
            //sizes[1] = Marshal.SizeOf(typeof(dxl_t));
            //sizes[2] = Marshal.SizeOf(typeof(lsv_t));
            //sizes[3] = Marshal.SizeOf(typeof(ads_t));
            //sizes[4] = Marshal.SizeOf(typeof(led_t));
            


            //for (int i = 0; i < sizes.Length; i++)
            //{
            //    Console.Write($"{sizes[i]} ");
            //}


            //Console.Write(" Length:{0} |", packet.Length);
            //for (int i = 0; i < packet.Length; i++)
            //{
            //    Console.Write($" {packet[i]:X2}");
            //}
            //Console.WriteLine();
        }


        public byte[] getData()
        {   

            var last = this.GetLast();

            byte[] data = new byte[ _can.CAN_LEN[(int)CAN_DLC.FDCAN_DLC_BYTE_24]];

            if (last != null) 
            {
                //data = new byte[last.data.Count];

                recvPacket.cmd.count = last.data[0];
                recvPacket.cmd.command = last.data[1];

                recvPacket.dxl.id = last.data[2];
                recvPacket.dxl.position = (ushort)(last.data[4] << 8 | last.data[3]);
                recvPacket.dxl.velocity = (ushort)(last.data[6] << 8 | last.data[5]);
                recvPacket.dxl.status = last.data[7];

                recvPacket.lsv.id = last.data[8];
                recvPacket.lsv.position = (ushort)(last.data[10] << 8 | last.data[9]);
                recvPacket.lsv.velocity = (ushort)(last.data[12] << 8 | last.data[11]);
                recvPacket.lsv.status = last.data[13];

                recvPacket.hallSensor.toggleSwitch = last.data[14];
                //byte[] voltageArray = { last.data[19], last.data[20], last.data[21], last.data[22] };
                //recvPacket.hallSensor.voltage = BitConverter.ToSingle(voltageArray, 0);
                recvPacket.hallSensor.raw = (Int16)(last.data[15] | last.data[16] << 8);
                recvPacket.hallSensor.status = last.data[17];

                recvPacket.led.ledSwitch = last.data[18];
                recvPacket.led.colors.red = last.data[19];
                recvPacket.led.colors.green = last.data[20];
                recvPacket.led.colors.blue = last.data[21];
                recvPacket.led.colors.brightness = last.data[22];

                recvPacket.crc = last.data[23];

                Console.Write($"{recvPacket.cmd.command:X2}=>");
                Console.Write("DXL POS :" + recvPacket.dxl.position);
                Console.Write(" LSV POS    " + recvPacket.lsv.position);
                Console.Write(" HALLSENSOR {0} ",recvPacket.hallSensor.raw);
                Console.WriteLine("GET LED COLOR  {0} {1} {2} {3}" , 
                    recvPacket.led.colors.red,
                    recvPacket.led.colors.green,
                    recvPacket.led.colors.blue,
                    recvPacket.led.colors.brightness);


                data = preparePacekt(recvPacket);
            }

            return data;
        }


        public void rotateDXL(bool isLock) 
        {
            if (isLock) 
            {
                rotate(DXLTargetPosition);
            }
            else
            {
                rotate(DXLInitPosition);
            }
        }

        public void pushLinear() 
        {
            push(800);
        }

        public void releaseLinear() 
        {
            push(300);
        }

     

        public void switchLED() 
        {
                           
        }

        public void setLEDColor()
        {
            
        }

        public byte[] preparePacekt(GripperPacket packet) 
        {
            byte[] byteArray;

            int length = Marshal.SizeOf(packet);


            
            byteArray = StructToBytes(packet);



            return byteArray;
        }


        private byte[] StructToBytes(object obj)
        {

            int structLength = Marshal.SizeOf(obj);
            //Console.WriteLine("Struct[{0}] <<", structLength);

            byte[] arr = new byte[structLength];
            IntPtr ptr = Marshal.AllocHGlobal(structLength);

            Marshal.StructureToPtr(obj, ptr, false);
            Marshal.Copy(ptr, arr, 0, structLength);

            Marshal.FreeHGlobal(ptr);

            return arr;
        }








        //public void start(int interval)
        //{
        //    _cts_request.Cancel();
        //    _cts_request = new CancellationTokenSource();

        //    Task.Run(async () =>
        //    {
        //        await requestTask(_cts_request, interval);
        //    });
        //}

        //// Request to board
        //private async Task requestTask(CancellationTokenSource cts, int delay)
        //{
        //    while (!cts.IsCancellationRequested)
        //    {


        //        await Task.Delay(delay);
        //    }

        //}

        //// Receive data continuously
        //public void getStatus()
        //{
        //    _cts_response.Cancel();
        //    _cts_response = new CancellationTokenSource();

        //    Task.Run(async () =>
        //    {
        //        await responseTask(_cts_request);
        //    });
        //}

        //private async Task responseTask(CancellationTokenSource cts)
        //{
        //    while (!_cts_response.IsCancellationRequested)
        //    {

        //        await Task.Delay(1);
        //    }

        //}


    }
}
