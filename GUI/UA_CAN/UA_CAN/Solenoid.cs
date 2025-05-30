using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static UA_CAN.Gripper;

namespace UA_CAN
{
    internal class Solenoid
    {
        private USB2CAN _serial = new USB2CAN();
        private CANablePro _can;
        private CAN_TYPE myType = CAN_TYPE.CAN_CLASSIC;

        public SolenoidPacket recvPacket = new SolenoidPacket();
        public packet_t? lastSendPacket;
        public string? portName;
        public string? lastPort;

        public int sol_id = 0x300;

        public readonly byte PACKET_STX = 0x02;
        public readonly byte PACKET_ETX = 0xFF;

        enum SolRequest
        {
            LED_TURN_OFF = 0xF0,
            LED_TURN_ON = 0xF1,
            SOL_PUSH = 0xF2,
            SOL_RELEASE = 0xF3,
            PHOTO_START = 0xF4,
            PHOTO_STOP = 0xF5,
            PHOTO_TRIG = 0xF6,
            PHOTO_READING = 0xF7,
            REQUEST_DATA = 0xF8,
            RETURN_DATA = 0xF9
        }

        enum SolState
        {
            ESP_OTA_MODE = 0x55,
            ESP_NORMAL_MODE = 0xAA,
            PHOTO_OFF = 0x00,
            PHOTO_ON = 0x01,
            LED_OFF = 0x00,
            LED_ON = 0x01,
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SolenoidPacket
        {
            public byte STX;
            public byte REQUEST;
            public byte ESP_STATE;
            public byte LED_BRIGHTNESS;
            public byte SOLENOID_STATE;
            public byte PHOTO_SWITCH;
            public byte PHOTO_TRIGGER;
            public byte ETX;
        }




        public Solenoid()
        {
            _can = new CANablePro(_serial);
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


        public void setLED(int brightness) 
        {
            SolenoidPacket tempPacket = new SolenoidPacket();
            tempPacket.STX = PACKET_STX;
            tempPacket.REQUEST = (byte)SolRequest.LED_TURN_ON;
            tempPacket.LED_BRIGHTNESS = (byte)brightness;
            tempPacket.ETX = PACKET_ETX;

            CanSend(sol_id, tempPacket);
        }

        public void push() 
        {
            SolenoidPacket tempPacket = new SolenoidPacket();
            tempPacket.STX = PACKET_STX;
            tempPacket.REQUEST = (byte)SolRequest.SOL_PUSH;
            tempPacket.ETX = PACKET_ETX;

            CanSend(sol_id, tempPacket);
        }

        public void release() 
        {
            SolenoidPacket tempPacket = new SolenoidPacket();
            tempPacket.STX = PACKET_STX;
            tempPacket.REQUEST = (byte)SolRequest.SOL_RELEASE;
            tempPacket.ETX = PACKET_ETX;

            CanSend(sol_id, tempPacket);
        }

        public void startPhotoSensing()
        {
            SolenoidPacket tempPacket = new SolenoidPacket();
            tempPacket.STX = PACKET_STX;
            tempPacket.REQUEST = (byte)SolRequest.PHOTO_START;
            tempPacket.ETX = PACKET_ETX;

            CanSend(sol_id, tempPacket);
        }

        public void stopPhotoSensing()
        {
            SolenoidPacket tempPacket = new SolenoidPacket();
            tempPacket.STX = PACKET_STX;
            tempPacket.REQUEST = (byte)SolRequest.PHOTO_STOP;
            tempPacket.ETX = PACKET_ETX;

            CanSend(sol_id, tempPacket);
        }




        public void CanSend(int id, SolenoidPacket sol) 
        {
            byte[] packet = preparePacekt(sol);
            sendCANPacket(myType, id, CAN_DLC.FDCAN_DLC_BYTE_8, packet);

        }
        public void sendCANPacket(CAN_TYPE type, int id, CAN_DLC dlc, byte[] packet)
        {
            _can.write(type, id, dlc, packet);
            lastSendPacket = _can._sendPacekt;
        }



        public byte[] getData()
        {
            var last = this.GetLast();
            byte[] data = new byte[_can.CAN_LEN[(int)CAN_DLC.FDCAN_DLC_BYTE_8]];

            if (last != null)
            {
                recvPacket.STX            = last.data[0];
                recvPacket.REQUEST        = last.data[1];
                recvPacket.ESP_STATE      = last.data[2];
                recvPacket.LED_BRIGHTNESS = last.data[3];
                recvPacket.SOLENOID_STATE = last.data[4];
                recvPacket.PHOTO_SWITCH   = last.data[5];
                recvPacket.PHOTO_TRIGGER  = last.data[6];
                recvPacket.ETX            = last.data[7];

                data = preparePacekt(recvPacket);
            }

            return data;
        }

        public byte[] preparePacekt(SolenoidPacket packet)
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


    }
}
