using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UA_CAN
{
    internal class USB2CAN
    {
        public SerialPort sp = new SerialPort();

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

    }
}
