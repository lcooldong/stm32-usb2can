using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UA_CAN
{
    

    internal class CANablePro
    {
        private USB2CAN _serial;
        private CancellationTokenSource _cts_read = new CancellationTokenSource();

        public packet_t _packet;
        public int _targetId = 0x314;    // ID
        public string _raw = "";

        public CANablePro(USB2CAN serial) 
        {
            _serial = serial;
            _packet.data = new List<int>();
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct packet_t
        {
            public int id;
            public int dlc;
            public List<int> data;
        }

        public void read()
        {
            _cts_read.Cancel();
            _cts_read = new CancellationTokenSource();

            Task.Run( async () =>
            {
                await readTask();
            });
        }

        private async Task readTask() 
        {
            string response = "";
            //while (!_cts_read.IsCancellationRequested)
            while (true) 
            {
                
                response = await ReadExistsStream(_serial.sp).ConfigureAwait(false);
                if (response.Length > 0)
                {
                    _raw = response;

                    if (response.StartsWith("d") )
                    {
                        byte[] byteStr = Encoding.Default.GetBytes(response);

                        GetPacket(byteStr);

                        //if (_targetId == _packet.id) 
                        //{
                        //    Console.WriteLine("String ->Byte Value ID : 0x{0:X} [{1}]", _packet.id, _packet.dlc);

                        //    for (int i = 0; i < _packet.dlc; i++)
                        //    {
                        //        Console.WriteLine("{0:X2}", _packet.data[i]);
                        //    }
                        //} 

                    }
                    else
                    {

                    }
                }
            }
            

        }

        private async Task<string> ReadExistsStream(SerialPort _sp)
        {
            int length = _sp.BytesToRead;
            if (length == 0) return "";

            byte[] buffer = new byte[length];
            await _sp.BaseStream.ReadAsync(buffer).ConfigureAwait(false);
            return Encoding.UTF8.GetString(buffer.ToArray());
        }

        private void GetPacket(byte[] packet) 
        {
            int id = 0, hex;
            int high, low;

            for (int i = 1; i <= 3; i++)
            {
                hex = packet[i] > '9' ? packet[i] - 'A' + 10 : packet[i] - '0';
                //Console.WriteLine(hex);
                id += hex << (4 * (3 - i)); // hex 1개씩
            }

            _packet.id = id;
            _packet.dlc = packet[4] > '9' ? packet[4] - 'A' + 10 : packet[4] - '0';

            int[] hexArray = new int[_packet.dlc];       

            for (int i = 5; i < 5 + _packet.dlc * 2; i += 2)
            {
                high = packet[i] > '9' ? packet[i] - 'A' + 10 : packet[i] - '0';
                low = packet[i + 1] > '9' ? packet[i + 1] - 'A' + 10 : packet[i + 1] - '0';

                hexArray[(i - 5) / 2] = (high << 4) | low;
                
                //Console.WriteLine( $"{high} | {low}");
            }

            for (int i = 0; i < _packet.dlc; i++)
            {
                _packet.data[i] = hexArray[i];
            }

            
        }


    }
}
