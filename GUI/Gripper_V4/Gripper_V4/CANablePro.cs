using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Gripper_V4
{
    // 구조체 대신에 클래스로
    public class packet_t
    {
        public int id;
        public int dlc;
        public List<byte> data = new List<byte>();
    }
    public enum CAN_TYPE
    {
        CAN_FD = 0,
        CAN_CLASSIC = 1
    }



    public enum CAN_DLC
    {
        FDCAN_DLC_BYTE_0,
        FDCAN_DLC_BYTE_1,
        FDCAN_DLC_BYTE_2,
        FDCAN_DLC_BYTE_3,
        FDCAN_DLC_BYTE_4,
        FDCAN_DLC_BYTE_5,
        FDCAN_DLC_BYTE_6,
        FDCAN_DLC_BYTE_7,
        FDCAN_DLC_BYTE_8,
        FDCAN_DLC_BYTE_12,
        FDCAN_DLC_BYTE_16,
        FDCAN_DLC_BYTE_20,
        FDCAN_DLC_BYTE_24,
        FDCAN_DLC_BYTE_32,
        FDCAN_DLC_BYTE_48,
        FDCAN_DLC_BYTE_64,
    }

    public enum CAN_BITRATE
    {
        CAN_100K,
        CAN_125K,
        CAN_250K,
        CAN_500K,
        CAN_1M,
        CAN_2M,
        CAN_4M,
        CAN_5M,
    }



    internal class CANablePro
    {
        private USB2CAN _serial;
        private CAN_BITRATE _canBitrate;
        private CAN_BITRATE _canfdBitrate;
        public packet_t _sendPacekt = new packet_t();

        

        public int _targetId = 0x314;    // ID
        public string _raw = "";

        private CancellationTokenSource _cts_read = new CancellationTokenSource();
        public CircularQueue<packet_t> packetQueue = new CircularQueue<packet_t>(256);

        //CAN_TYPE canType = new CAN_TYPE();

        public readonly int[] CAN_LEN = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 12, 16, 20, 24, 32, 48, 64 };



        public CANablePro(USB2CAN serial, CAN_BITRATE canBitrate = CAN_BITRATE.CAN_500K, CAN_BITRATE canfdBitrate = CAN_BITRATE.CAN_2M)
        {
            _serial = serial;
            _canBitrate = canBitrate;
            _canfdBitrate = canfdBitrate;

        }


        public void openChannel() 
        {
            string s_canBitrate = string.Format("S{0:D}\r",_canBitrate + 3);
            string s_canfdBitrate = string.Format("Y{0:D}\r", _canfdBitrate - 3);


            //_serial.sp.Write("S6\r");
   


            _serial.sp.Write(s_canBitrate);

            // Set CAN FD Bitrate (2M = f4)
            _serial.sp.Write(s_canfdBitrate);
            //Console.WriteLine("=> {0}", s_canBitrate);
            //Console.WriteLine("=> {0}", s_canfdBitrate);
            // Enable CAN FD
            //_serial.sp.Write("F1\r");

            // Open CAN Channel
            _serial.sp.Write("O\r");

            //Thread.Sleep(10);
        }

        public void closeChannel() 
        {
            _serial.sp.Write("C\r");
        }


        public void write(CAN_TYPE type, int id, CAN_DLC dlc, byte[] packet) 
        {
            _sendPacekt.data.Clear();

            var localPacket = new packet_t
            {
                id = id,
                dlc = (int)dlc,
                data = packet.Take(CAN_LEN[(int)dlc]).ToList()

            };
            _sendPacekt = localPacket;

            Task.Run(() => {

                
                
   
                WriteTask(type, localPacket);
            });
        }


        private Task WriteTask(CAN_TYPE type, packet_t pacekt) 
        {

            string toSend = type == CAN_TYPE.CAN_FD ? "d" : "t";
            toSend += pacekt.id.ToString("X");
            toSend += pacekt.dlc.ToString("X");

            for (int i = 0; i< pacekt.data.Count; i++)
            {
                toSend += pacekt.data[i].ToString("X2");
                
            }
            toSend += "\r";

            //if (toSend.Length > 0)
            //{
                
            //    Console.WriteLine($"OK -> {toSend}");
            //}
            //else 
            //{
            //    Console.WriteLine("Nothing To Send");    
            //}

            //await _serial.sp.BaseStream.WriteAsync(Encoding.ASCII.GetBytes(toSend)).ConfigureAwait(false);
            _serial.sp.BaseStream.Write(Encoding.ASCII.GetBytes(toSend));


            return Task.CompletedTask;
        }

        public void read()
        {
            _cts_read.Cancel();
            _cts_read = new CancellationTokenSource();

            Task.Run( async () =>
            {
                await ReadTask();
            });
        }

        public void stopRead() 
        {
            _cts_read?.Cancel();
        }

        private async Task ReadTask()
        {
            string response = "";
            while (!_cts_read.IsCancellationRequested)
            {   
                response = await ReadExistsStream(_serial.sp).ConfigureAwait(false);  // under 64 bytes (under DLC32 -> 8, 12, 16, 20, 24)
                //response = await ReadStreamBlockAsync(_serial.sp, 70, 1).ConfigureAwait(false); // 32 bytes
                Console.WriteLine(response.Length);
                if (response.Length > 0)
                {
                    _raw = response;
                    //Console.WriteLine(response);

                    if (response.StartsWith("d") || response.StartsWith("t"))
                    {
                        byte[] byteStr = Encoding.Default.GetBytes(response);

                        GetPacket(byteStr);

                        //var last = packetQueue.GetLast();

                        //if (_targetId == last?.id && last != null)
                        //{
                        //    Console.WriteLine("String ->Byte Value ID : 0x{0:X} [{1}]", last.id, last.dlc);

                        //    for (int i = 0; i < last.dlc; i++)
                        //    {
                        //        Console.WriteLine("{0:X2}", last.data[i]);
                        //    }
                        //}

                    }
                    
                }
            }
        }


        // read under 64 bytes
        private async Task<string> ReadExistsStream(SerialPort _sp)
        {
            int length = _sp.BytesToRead;
            //if (length > 0)
            //{
            //    Console.WriteLine("LENG " + length);
            //}
            if (length == 0) return "";

            byte[] buffer = new byte[length];
            await _sp.BaseStream.ReadAsync(buffer).ConfigureAwait(false);
            return Encoding.UTF8.GetString(buffer.ToArray());
        }


        private async Task<string> ReadStreamBlockAsync(SerialPort _sp, int maxBytes = 1024, int readDelayMs = 10)
        {
            List<byte> bufferList = new List<byte>();
            byte[] tempBuffer = new byte[256]; // read in 256-byte chunks

            var stream = _sp.BaseStream;

            int attempts = 0;
            while (_sp.BytesToRead > 0 || attempts < 3)
            {
                int bytesRead = await stream.ReadAsync(tempBuffer, 0, tempBuffer.Length).ConfigureAwait(false);
                if (bytesRead > 0)
                {
                    bufferList.AddRange(tempBuffer.Take(bytesRead));
                    attempts = 0; // reset on successful read
                }
                else
                {
                    attempts++;
                    await Task.Delay(readDelayMs);
                }

                if (bufferList.Count >= maxBytes) break;
            }

            return Encoding.UTF8.GetString(bufferList.ToArray());
        }

        private void GetPacket(byte[] packet) 
        {
            int id = 0, hex;
            int high, low;
            

            for (int i = 1; i <= 3; i++)
            {
                hex = packet[i] > '9' ? packet[i] - 'A' + 10 : packet[i] - '0';
                id += hex << (4 * (3 - i)); // hex 1개씩
            }
            int dlc = packet[4] > '9' ? packet[4] - 'A' + 10 : packet[4] - '0'; // D -> 13

            packet_t pkt = new packet_t() { id = id, dlc = dlc };


            int[] hexArray = new int[CAN_LEN[pkt.dlc]];  // 13 -> 32 bytes
            

            if (hexArray.Length > 0) 
            {
                for (int i = 5; i < 5 + hexArray.Length * 2; i += 2)
                {
                    high = packet[i] > '9' ? packet[i] - 'A' + 10 : packet[i] - '0';
                    low = packet[i + 1] > '9' ? packet[i + 1] - 'A' + 10 : packet[i + 1] - '0';

                    hexArray[(i - 5) / 2] = (high << 4) | low;  // dlc = 8 -> 0~7 range

                    //Console.WriteLine( $"{high} | {low}");
                    pkt.data.Add((byte)hexArray[(i - 5) / 2]);
                }

                packetQueue.Enqueue(pkt);
            }
                   
        }

        public packet_t? GetLastPacket()
        {
            return packetQueue.GetLast();
        }

        public void clearPacket() 
        {
            packetQueue.Clear();
        }

        // TODO: Read 에서 사용할 예정
        private void ConvertLittlrEndian(byte[] packet) 
        {
            int high, low;

            int[] hexArray = new int[32];
            if (hexArray.Length > 0)
            {
                for (int i = 5; i < 5 + hexArray.Length * 2; i += 2)
                {
                    high = packet[i] > '9' ? packet[i] - 'A' + 10 : packet[i] - '0';
                    low = packet[i + 1] > '9' ? packet[i + 1] - 'A' + 10 : packet[i + 1] - '0';

                    hexArray[(i - 5) / 2] = (high << 4) | low;  // dlc = 8 -> 0~7 range

                    //Console.WriteLine( $"{high} | {low}");
                    //pkt.data.Add((byte)hexArray[(i - 5) / 2]);
                }

                //packetQueue.Enqueue(pkt);
            }

            return;


        }





    }
}
