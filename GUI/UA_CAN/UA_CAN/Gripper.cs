using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UA_CAN
{
    internal class Gripper
    {
        internal USB2CAN _serial = new USB2CAN();
        internal CANablePro _can;

        private CancellationTokenSource _cts_request = new CancellationTokenSource();
        private CancellationTokenSource _cts_response = new CancellationTokenSource();
        public GripperPacket gripperPacket = new GripperPacket();
        public int[] baudrates;
        public string? portName;
        public string? lastPort;
        public packet_t? lastSendPacket;

        public Gripper() 
        {
            _can = new CANablePro(_serial);
            baudrates = _serial.baudRates.ToArray();    // Deep Copy
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
            }

            return ret;
        }

        public void portClose() 
        {
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



        enum GripperState
        {
            REQUEST           = 0x01, 
            RESPONSE          = 0x02,
            MOTOR_DXL_RUN     = 0x03,
            MOTOR_DXL_ARRIVED = 0x04,
            STOP              = 0x05
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct dxl_t
        {
            byte   id;
            UInt16 position;
            UInt16 velocity;
            bool   status;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct lsv_t
        {
            byte id;
            UInt16 position;
            UInt16 velocity;
            int status;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ads_t
        {
            bool status;
            bool toggleSwitch;
            float voltage;
            Int16 raw;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct color_t
        {
            byte red;
            byte green;
            byte blue;
            byte brightness;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct led_t
        {
            byte ledSwitch;
            color_t colors;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct cmd_t
        {
            byte count;
            byte command;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct crc_t
        {
            byte h_crc;
            byte l_crc;
        }


        // 32 bytes
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct GripperPacket
        {
            cmd_t cmd;
            dxl_t dxl;
            lsv_t lsv;
            ads_t hallSensor;
            led_t led;
            crc_t crc;
        }



        public void sendPacket(CAN_TYPE type, int id, CAN_DLC dlc, byte[] packet)
        {
            _can.write(type, id, dlc, packet);
            lastSendPacket = _can._sendPacekt;

        }

        public void start(int interval)
        {
            _cts_request.Cancel();
            _cts_request = new CancellationTokenSource();

            Task.Run(async () =>
            {
                await requestTask(_cts_request, interval);
            });
        }

        // Request to board
        private async Task requestTask(CancellationTokenSource cts, int delay)
        {
            while (!cts.IsCancellationRequested)
            {


                await Task.Delay(delay);
            }

        }

        // Receive data continuously
        public void getStatus()
        {
            _cts_response.Cancel();
            _cts_response = new CancellationTokenSource();

            Task.Run(async () =>
            {
                await responseTask(_cts_request);
            });
        }

        private async Task responseTask(CancellationTokenSource cts)
        {
            while (!_cts_response.IsCancellationRequested)
            {

                await Task.Delay(1);
            }

        }


    }
}
