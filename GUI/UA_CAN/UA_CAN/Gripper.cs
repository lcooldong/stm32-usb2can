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
        private USB2CAN _serial;

        private CancellationTokenSource _cts_request = new CancellationTokenSource();
        private CancellationTokenSource _cts_response = new CancellationTokenSource();

        public Gripper(USB2CAN serial) 
        {
            _serial = serial;        
        }



        enum GripperState
        {

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

        }


        // 32 bytes
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct canPacket
        {

        }



        public void sendPacket()
        {
            //byte[] testData = new byte[32];

            //for (int i = 0; i < 32; i++) 
            //{
            //    testData[i] = (byte)i;
            //    _serial.write(testData, 1);
            //}
            byte[] data = new byte[1];
            data[0] = 0x49;
            _serial.write(data, 1);
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
