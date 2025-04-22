using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UA_CAN
{
    internal class USB2CAN
    {
        public SerialPort sp = new SerialPort();

        private CancellationTokenSource _cts_request = new CancellationTokenSource();
        private CancellationTokenSource _cts_response = new CancellationTokenSource();

        public Dictionary<string, string> usbDevices;

        enum GripperState 
        {
            
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct canPacket 
        {
            
        }


        public void begin(string port, int baudrate = 115200) 
        {
            sp.PortName = port;
            sp.BaudRate = baudrate;
            sp.DataBits = 8;
            sp.Parity = Parity.None;
            sp.StopBits = StopBits.One;
            sp.Handshake =Handshake.None;

            if (!sp.IsOpen)
            {

                sp.Close();
                sp.Open();
                Console.WriteLine("[Port Re-Opened] : " + sp.PortName);
            }
            else
            {
                sp.Open();
                Console.WriteLine("[Port Opened] : " + sp.PortName);
            }
        }


        private void initPacket() 
        {
        
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

        public List<string> GetUSBDevices()
        {
            List<string> usbDevices = new List<string>();

            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity where Name like '%(COM%)'"))
            {
                foreach (var device in searcher.Get())
                {
                    string? name = device["Name"]?.ToString();
                    string? deviceId = device["DeviceID"]?.ToString();
                    usbDevices.Add($"{name}  →  {deviceId}");
                }
            }

            return usbDevices;
        }




    }
}
