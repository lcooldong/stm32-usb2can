using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using System.Globalization;





namespace ConsoleApp_COM_CANable_V2
{
    public class Program
    {


        public struct DataPacket
        {

        }

        // CAN FD 구현
        static async Task Main(string[] args)
        {
            Console.WriteLine("CANable V2");

            SerialPort sp = new SerialPort();

            Begin(sp, "COM8");

            int targetId = 0x314;    // 11 bit 
            int dlc = 0;
            string response = "";

            Console.WriteLine("Target ID : 0x{0} | DEC => {1}", targetId.ToString("X3"), targetId);

            while (true)
            {
                response = await ReadExistsStream(sp).ConfigureAwait(false);
                
                if (response.Length > 0) Console.WriteLine($"{response}");

                // t 로 시작 -> can2.0
                if (response.StartsWith("d"))
                {

                    byte[] byteStr = Encoding.Default.GetBytes(response);
                    Console.WriteLine($"Byte Length : {byteStr.Length}");   // 22 

                    if (byteStr[byteStr.Length - 1] == '\r')
                    {
                        Console.WriteLine("Carriage Return");
                    }

                    // ID
                    int id = 0, hex;
                    for (int i = 1; i <= 3; i++)
                    {
                        hex = byteStr[i] > '9' ? byteStr[i] - 'A' + 10 : byteStr[i] - '0';
                        //Console.WriteLine(hex);
                        id += hex << (4 * (3 - i)); // hex 1개씩
                    }

                    // DLC
                    dlc = byteStr[4] > '9' ? byteStr[4] - 'A' + 10 : byteStr[4] - '0';


                    // Data
                    int high, low;
                    int[] hexArray = new int[dlc];

                    for (int i = 5; i < 5 + dlc * 2; i += 2)
                    {
                        high = byteStr[i] > '9' ? byteStr[i] - 'A' + 10 : byteStr[i] - '0';
                        low = byteStr[i + 1] > '9' ? byteStr[i + 1] - 'A' + 10 : byteStr[i + 1] - '0';

                        hexArray[(i - 5) / 2] = (high << 4) | low;

                        //Console.WriteLine( $"{high} | {low}");
                    }


                    // ID 같을 경우만 받음
                    if (targetId == id)
                    {
                        Console.WriteLine("String ->Byte Value ID : 0x{0:X} [{1}]", id, dlc);

                        for (int i = 0; i < dlc; i++)
                        {
                            Console.WriteLine("{0:X2}", hexArray[i]);
                        }
                    }

                    string sendText = "t12380700012A03040F06\r";    // 한번에 보낼 때


                    byte[] byteToSend = new byte[byteStr.Length];   // 보낼 데이터 배열

                    int idToSend = 0x124;   // ID 

                    byte[] dataToSend = { 00, 01, 02, 03, 04, 05, 06 }; // Packet Data

                    dataToSend[3] = 0x05;

                    int dataToSendLength = dataToSend.Length;       // 길이
                    int dlcToSend = dataToSendLength + 1;



                    //string ToSend = "t";
                    string ToSend = "d";

                    ToSend += idToSend.ToString("X");
                    ToSend += dlcToSend.ToString("X");
                    ToSend += dataToSendLength.ToString("X2");

                    for (int i = 0; i < dataToSend.Length; i++)
                    {
                        ToSend += dataToSend[i].ToString("X2");
                    }

                    ToSend += "\r";

                    Console.WriteLine(ToSend);


                    //await sp.BaseStream.WriteAsync(byteStr, 0, byteStr.Length).ConfigureAwait(false);

                    //await sp.BaseStream.WriteAsync(Encoding.ASCII.GetBytes(sendText)).ConfigureAwait(false);

                    // sendPacket
                    await sp.BaseStream.WriteAsync(Encoding.ASCII.GetBytes(ToSend)).ConfigureAwait(false);
                }
                //else if(response.StartsWith('d')) // FDCAN
                //{
                    
                //}

            }

            while (true)
            {
                Console.Write("Send char to Serial : ");
                var command = Console.ReadLine();
                if (command.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
                    break;

                response = await ReadExistsStream(sp).ConfigureAwait(false);
                Console.WriteLine($"Read Exist chars = {response}");

                await sp.BaseStream.WriteAsync(Encoding.ASCII.GetBytes(command)).ConfigureAwait(false);
                response = await ReadExistsStream(sp).ConfigureAwait(false);
                Console.WriteLine($"Read chars exact after write = {response}");

                Thread.Sleep(10);

                //response = await ReadExistsStream(sp).ConfigureAwait(false);
                //Console.WriteLine($"Read chars exact after write = {response}");
            }

            sp.Close();
        }


        public static void Begin(SerialPort _sp, string _port, int _baudrate = 115200)
        {
            _sp.PortName = _port;
            _sp.BaudRate = _baudrate;
            _sp.DataBits = 8;
            _sp.Parity = Parity.None;
            _sp.StopBits = StopBits.One;

            //_sp.Open();

            try
            {
                _sp.Close();
                _sp.Open();
                Console.WriteLine($"IsOpen -> {_sp.IsOpen}");
                //if (_sp.IsOpen)
                //{
                //    _sp.Close();
                //    _sp.Open();
                //    Console.WriteLine("[Port Re-Opened] : " + _sp.PortName);
                //}
                //{
                //    _sp.Open();
                //    Console.WriteLine("[Port Opened] : " + _sp.PortName);
                //}
            }
            catch (Exception)
            {
                Console.WriteLine("==> Please Check Connectivity !");
            }
        }

        public static async Task<string> ReadExistsStream(SerialPort _sp)
        {
            int length = _sp.BytesToRead;
            if (length == 0) return "";

            byte[] buffer = new byte[length];
            await _sp.BaseStream.ReadAsync(buffer).ConfigureAwait(false);
            return Encoding.UTF8.GetString(buffer.ToArray());
        }

        public static void setPacket() 
        {
        
        }

        public byte[] StructToBytes(Object obj)
        {
            int structLength = Marshal.SizeOf(obj);
            byte[] arr = new byte[structLength];

            IntPtr ptr = Marshal.AllocHGlobal(structLength);

            Marshal.StructureToPtr(obj, ptr, false);
            Marshal.Copy(ptr, arr, 0, structLength);

            Marshal.FreeHGlobal(ptr);

            return arr;
        }


    }
}



