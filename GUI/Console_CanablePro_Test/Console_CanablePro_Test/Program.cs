using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

public class CAN
{
    private SerialPort serialPort;
    private string comPort;
    private int baudRate;
    private int slcanBitrateIndex;
    private Thread readThread;
    private bool keepReading = true;

    public CAN(string comPort, int baudRate = 115200, int slcanBitrateIndex = 6)
    {
        this.comPort = comPort;
        this.baudRate = baudRate;
        this.slcanBitrateIndex = slcanBitrateIndex; // 0~8 corresponding to 10k~1M
    }

    public void Open()
    {
        serialPort = new SerialPort(comPort, baudRate, Parity.None, 8, StopBits.One);
        serialPort.NewLine = "\r";
        serialPort.ReadTimeout = 500;
        serialPort.WriteTimeout = 500;
        serialPort.Open();

        // Set bitrate with 'Sx\r' where x is 0~8 (before opening the channel)
        string bitrateCmd = $"S{slcanBitrateIndex}\r";
        serialPort.Write(bitrateCmd);
        Thread.Sleep(100);

        // Send SLCAN 'Open channel' command
        serialPort.Write("O\r");

        keepReading = true;
        readThread = new Thread(ReadLoop);
        readThread.Start();

        Console.WriteLine($"Opened {comPort} at {baudRate} baud with CAN bitrate index S{slcanBitrateIndex}.");
    }

    public void WriteCAN(uint id, byte[] data)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            string frameType = id > 0x7FF ? "T" : "t";
            string idStr = id > 0x7FF ? id.ToString("X8") : id.ToString("X3");
            string lenStr = data.Length.ToString("X1");
            string dataStr = BitConverter.ToString(data).Replace("-", "");

            string slcanFrame = $"{frameType}{idStr}{lenStr}{dataStr}\r";
            serialPort.Write(slcanFrame);
            Console.WriteLine($"Sent: {slcanFrame.Trim()} (ID: 0x{id:X}, Data: {BitConverter.ToString(data)})");
        }
    }

    private void ReadLoop()
    {
        while (keepReading)
        {
            try
            {
                string response = serialPort.ReadLine();
                Console.WriteLine($"Received: {response}");
            }
            catch (TimeoutException) { }
        }
    }

    public void Close()
    {
        keepReading = false;
        readThread?.Join();
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Write("C\r"); // Close SLCAN channel
            serialPort.Close();
            Console.WriteLine($"Closed {comPort}.");
        }
    }

    public static void Main()
    {
        var can = new CAN("COM7", 115200, 6); // S6 = 500k
        can.Open();

        byte[] data1 = { 0, 25, 0, 1, 3, 1, 4, 1 };
        byte[] data2 = { 1, 2, 3, 4, 5, 6, 7, 8 };

        while (true)
        {
            can.WriteCAN(0xC0FFEE, data1); // Extended ID
            can.WriteCAN(0x123, data2);    // Standard ID
            Thread.Sleep(1000);
        }
    }
}