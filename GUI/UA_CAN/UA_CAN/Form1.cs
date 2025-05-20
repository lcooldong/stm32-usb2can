using System.IO.Ports;
using System.Timers;
using System.Windows.Forms;


namespace UA_CAN
{
    public partial class Form1 : Form
    {
        Gripper gripper = new Gripper();

        static USB2CAN Serial = new USB2CAN();
        CANablePro can0 = new CANablePro(Serial);

        System.Timers.Timer timer = new System.Timers.Timer();
        private bool isConnected = false;
        private bool hallFlag = false;
        private bool timerFlag = false;
        int count = 0;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            //var devices = Serial.GetUSBDevices();
            //var devices = gripper._serial.GetUSBDevices();
            var devices = gripper.GetUSBDevices();
            foreach (var device in devices)
            {
                portBox.Items.Add($"[{device.Key}] {device.Value}");
            }
            portBox.SelectedIndex = 0;

            //foreach (int rate in Serial.baudRates)
            //foreach (int rate in gripper._serial.baudRates)
            foreach (int rate in gripper.baudrates)
            {
                baudBox.Items.Add(rate.ToString());
            }
            baudBox.SelectedIndex = 4;



            rtbConsole.Multiline = true;
            rtbConsole.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbConsole.SelectionAlignment = HorizontalAlignment.Left;

            this.ActiveControl = null;


            portBox.DropDown += PortBox_DropDown;


            //Serial.autoConnect();   // Connect to last COM Port.

        }

        private void PortBox_DropDown(object? sender, EventArgs e)
        {
            portBox.Items.Clear();
            //var devices = Serial.GetUSBDevices();
            //var devices = gripper._serial.GetUSBDevices();
            var devices = gripper.GetUSBDevices();
            foreach (var device in devices)
            {
                portBox.Items.Add($"[{device.Key}] {device.Value}");
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //var devices = Serial.GetUSBDevices();
            //var devices = gripper._serial.GetUSBDevices();
            var devices = gripper.GetUSBDevices();
            string port = devices.ElementAt(portBox.SelectedIndex).Key;

            if (baudBox.SelectedItem != null && int.TryParse(baudBox.SelectedItem.ToString(), out int baudrate))
            {
                if (!isConnected)
                {

                    //if (Serial.begin(port, baudrate))
                    //if (gripper._serial.begin(port, baudrate))
                    if (gripper.begin(port, baudrate))
                    {
                        Log($"[O] Connected to {port} @ {baudBox.SelectedItem} baud\r\n", true);
                        btnConnect_Open();

                        //can0.clearPacket();

                    }
                    else
                    {
                        Log($"[X] Failed to connect {port}\r\n", true);
                    }
                }
                else
                {
                    //Serial.close();
                    //gripper._serial.close();
                    gripper.portClose();
                    btnConnect_Close();
                    Log($"[ ] Disconnected from {port}\r\n", true);

                    timer.Elapsed -= Timer_Elapsed;
                    timer.Stop();
                    gripper.receivingStop();
                }
            }
            else
            {
                Log($"Please set port or baudrate\r\n", true);
            }
        }



        private void btnTimer_Click(object sender, EventArgs e)
        {

            timer.Interval = 50;

            if (!timerFlag)
            {
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
                //can0.read();     // read CAN After UART Open
                //gripper._can.read();     // read CAN After UART Open


                gripper.receivingPacket();
                gripper.canStart();
               
                btnTimer.Text = "STOP";
                Log("Start Reading\r\n", true);
                timerFlag = true;
            }
            else
            {
                timer.Elapsed -= Timer_Elapsed;
                timer.Stop();
                //can0.stopRead();
                //gripper._can.stopRead();

                gripper.canStop();
                gripper.receivingStop();

                btnTimer.Text = "READ";
                Log("Stop Reading\r\n", true);
                timerFlag = false;
            }

        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            //if (Serial.isUSBConnected())
            //if (gripper._serial.isUSBConnected())
            if (gripper.isUSBConnected())
            {

                //if (Serial.sp.IsOpen)
                //if (gripper._serial.sp.IsOpen)
                if (gripper.isUSBOpen())
                {
                    btnConnect_Open();

                }
                count++;
                //Log(" {0:D2} {1} {2} - {3} |", true, count, Serial.sp.PortName, Serial.sp.IsOpen, Serial.lastPort);
                //Log(" {0:D2} {1} {2} - {3} |", true, count, gripper._serial.sp.PortName, gripper._serial.sp.IsOpen, gripper._serial.lastPort);
                //Log(" {0:D3} {1} {2} - {3} |", true, count, gripper.portName, gripper.isUSBOpen(), gripper.lastPort);

                //var last = can0.packetQueue.GetLast();
                //var last = gripper._can.packetQueue.GetLast();
                byte[] last = gripper.getData();
                txbHall.Text = gripper.recvPacket.hallSensor.raw.ToString();
                txbDXL_Read.Text = gripper.recvPacket.dxl.position.ToString();
                txbLSV_Read.Text = gripper.recvPacket.lsv.position.ToString();
                txbCount.Text = gripper.count.ToString();

                //var last = gripper.GetLast();
                //if (last != null)
                //{
                //    string hex = string.Join(" ", last.data.Select(b => b.ToString("X2")));
                //    Log("0x{0:X2} {1:X} {2}", false, last.id, last.dlc, hex);
                //}
                //if (last != null)
                //{
                //    Log("{0:X2} ", false, gripper.recvPacket.cmd.command);
                //}


                //Log("\r\n", false);


                //Log($"{can0._raw}", false);
                //gripper.sendPacket();
            }
            else
            {

                btnConnect_Close();
                //Log($" {count--} {Serial.sp.PortName} {Serial.sp.IsOpen} - {Serial.lastPort}\r\n", true);
                //Log($" {count--} {gripper._serial.sp.PortName} {gripper._serial.sp.IsOpen} - {gripper._serial.lastPort}\r\n", true);
                Log($" {count--} {gripper.portName} {gripper.isUSBOpen()} - {gripper.lastPort}\r\n", true);
            }
        }


        private void btnConnect_Open()
        {
            isConnected = true;
            btnConnect.Text = "Disconnect";
        }

        private void btnConnect_Close()
        {
            isConnected = false;
            btnConnect.Text = "Connect";
        }



        private void Log(string format, bool tFlag, params object?[] args)
        {
            this.Invoke(new Action(() =>
            {
                string message = string.Format(format, args);
                if (tFlag)
                {
                    string time = DateTime.Now.ToString("HH:mm::ss");
                    rtbConsole.AppendText($"{time}->");
                }


                if (rtbConsole.InvokeRequired)
                {
                    rtbConsole.Invoke(new Action(() =>
                    {
                        rtbConsole.AppendText(message);
                    }));
                }
                else
                {
                    rtbConsole.AppendText(message); // Work on here
                }
                rtbConsole.ScrollToCaret();
            }));

        }

        private void btnCANTest_Click(object sender, EventArgs e)
        {
            byte[] testData = { 0xA1, 0xB2, 0xC3, 0xD4, 0xE5, 0xF6 };
            byte[] testFD = new byte[24];

            for (int i = 0; i < testFD.Length; i++)
            {
                testFD[i] = (byte)i;
            }

            //can0.write(CAN_TYPE.CAN_FD, 0x124, CAN_DLC.FDCAN_DLC_BYTE_24, testFD);  
            //Log("0x{0:X2} DLC:{1:D2} Length:{2} =>", true, can0._sendPacekt.id, can0._sendPacekt.dlc, can0._sendPacekt.data.Count);
            //gripper._can.write(CAN_TYPE.CAN_FD, 0x124, CAN_DLC.FDCAN_DLC_BYTE_32, testFD);  
            gripper.sendCANPacket(CAN_TYPE.CAN_FD, 0x124, CAN_DLC.FDCAN_DLC_BYTE_24, testFD);



            //Log("0x{0:X2} DLC:{1:D2} Length:{2} =>", true, gripper._can._sendPacekt.id, gripper._can._sendPacekt.dlc, gripper._can._sendPacekt.data.Count);
            Log("0x{0:X2} DLC:{1:D2} Length:{2} =>", true, gripper.lastSendPacket?.id, gripper.lastSendPacket?.dlc, gripper.lastSendPacket?.data.Count);

            //for (int i = 0; i < can0._sendPacekt.data.Count; i++)
            //for (int i = 0; i < gripper._can._sendPacekt.data.Count; i++)
            for (int i = 0; i < gripper.lastSendPacket?.data.Count; i++)
            {
                //Log($"|{can0._sendPacekt.data[i]:X2}", false);
                //Log($"|{gripper._can._sendPacekt.data[i]:X2}", false);
                Log($"|{gripper.lastSendPacket?.data[i]:X2}", false);
            }
            Log("\r\n", false);

        }

        private void btnHall_Click(object sender, EventArgs e)
        {
            if (hallFlag)
            {
                gripper.switchHall(Hall.OFF);
                hallFlag = false;
                Log("Stop reading HallSensor\r\n", true);

            }
            else
            {
                gripper.switchHall(Hall.ON);
                hallFlag = true;
                Log("Start reading HallSensor\r\n", true);
            }


        }

        private void btnLED_Click(object sender, EventArgs e)
        {
            
        }


        private void txb_KeyPress(object sender, KeyPressEventArgs e)
        {


            txbRed.MaxLength = 3;
            txbGreen.MaxLength = 3;
            txbBlue.MaxLength = 3;
            txbBrightness.MaxLength = 3;

            if (!('0' <= e.KeyChar && e.KeyChar <= '9') && e.KeyChar != (char)8) e.Handled = true;
        }

        private void txb_Changed(object sender, EventArgs e)
        {
            if (int.TryParse(txbRed.Text, out int vRed) && vRed >= 255)
            {
                txbRed.Text = "255";
            }
            if (int.TryParse(txbGreen.Text, out int vGreen) && vGreen >= 255)
            {
                txbGreen.Text = "255";
            }
            if (int.TryParse(txbBlue.Text, out int vBlue) && vBlue >= 255)
            {
                txbBlue.Text = "255";
            }
            if (int.TryParse(txbBrightness.Text, out int vBrightness) && vBrightness >= 255)
            {
                txbBrightness.Text = "255";
            }
        }

        private void btnPkt_Click(object sender, EventArgs e)
        {
            gripper.getData();
        }

        private void btnReq_Click(object sender, EventArgs e)
        {
            gripper.request();
        }

        private void motorValue_KeyPressed(object sender, KeyPressEventArgs e)
        {
            txbLSV.MaxLength = 4;
            txbDXL.MaxLength = 4;

            if (!('0' <= e.KeyChar && e.KeyChar <= '9') && e.KeyChar != (char)8) e.Handled = true;
        }

        private void motorValue_Changed(object sender, EventArgs e)
        {

            if (int.TryParse(txbDXL.Text, out int dxlValue) && dxlValue >= 4095)
            {
                txbDXL.Text = "4095";
            }

            if (int.TryParse(txbLSV.Text, out int lsvValue) && lsvValue >= 4095)
            {
                txbLSV.Text = "4095";
            }
        }

        private void btnRotate_Click(object sender, EventArgs e)
        {
            gripper.rotate(ushort.Parse(txbDXL.Text));
        }

        private void btnPush_Click(object sender, EventArgs e)
        {
            gripper.push(ushort.Parse(txbLSV.Text));
        }
    }
}
