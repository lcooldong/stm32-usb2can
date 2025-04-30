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
        public bool startFlag = false;
        public string lastPort = "";

        private CancellationTokenSource _cts_request = new CancellationTokenSource();
        private CancellationTokenSource _cts_response = new CancellationTokenSource();
        private CancellationTokenSource _cts_connection = new CancellationTokenSource();

        //public Dictionary<string, string> usbDevices;

        public int[] baudRates = new int[] { 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600 };


        enum GripperState 
        {
            
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct canPacket 
        {
            
        }


        public bool begin(string port, int baudrate = 115200) 
        {
       

            try
            {
                if (sp.IsOpen) 
                {
                    Console.WriteLine($"[Already Connected] : {sp.PortName}");
                    return true;
                }

                sp.PortName = port;     // Default COM1
                sp.BaudRate = baudrate;
                sp.DataBits = 8;
                sp.Parity = Parity.None;
                sp.StopBits = StopBits.One;
                sp.Handshake = Handshake.None;

                lastPort = sp.PortName;
                startFlag = true;

                sp.Open();
                Console.WriteLine($"[Connected] : {sp.PortName} @ {sp.BaudRate}");
                
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"[Access Denied] : {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] : {ex.Message}");
                return false;
            }
        }

        public void close() 
        {
            sp.Close();
            Console.WriteLine("[Disconnected]");
        }

        public void reset() 
        {

        }

        public void autoConnect() 
        {
            _cts_connection.Cancel();
            _cts_connection = new CancellationTokenSource();

            Task.Run(async() =>
            {
                await checkConnectionTask();
            });
        }

        private async Task checkConnectionTask() 
        {
            while (!_cts_connection.IsCancellationRequested) 
            {
                // Connected
                if (isUSBConnected())
                {
                    var devices = GetUSBDevices();

                    foreach (var device in devices)
                    {
                        if (lastPort == device.Key)
                        {
                            begin(lastPort);
                            break;
                        }
                    }
                }


                await Task.Delay(1000);
            }
        }


        // 연결했던 USB 존재 여부 확인
        public bool isUSBConnected() 
        {
            try
            {
                bool ret = false;

                var devices = GetUSBDevices();

                foreach (var device in devices)
                {
                    if (sp.PortName == device.Key)
                    {
                        ret = true;
                        break;
                    }
                    else 
                    {
                        ret = false;
                        sp.PortName = "COM1";
                    }
                }

                
                
                return ret;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"[Access Denied] : {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] : {ex.Message}");
                return false;
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

        public List<string> GetUSBDevicesList()
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

        public Dictionary<string, string> GetUSBDevices() 
        {
            var portDict = new Dictionary<string, string>();

            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Name LIKE '%(COM%)'"))
            {
                foreach (var obj in searcher.Get())
                {
                    string? fullName = obj["Name"]?.ToString();
                    if (!string.IsNullOrEmpty(fullName))
                    {
                        int start = fullName.LastIndexOf("(COM");
                        if (start >= 0)
                        {
                            string port = fullName.Substring(start + 1).Replace(")", ""); // e.g., COM3
                            string deviceName = fullName.Substring(0, start).Trim();
                            portDict[port] = deviceName;
                        }
                    }
                }
            }

            return portDict;
        }




    }
}
