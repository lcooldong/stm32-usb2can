using System.IO.Ports;
using System.Timers;
using System.Windows.Forms;


namespace UA_CAN
{
    public partial class Form1 : Form
    {
        Gripper gripper = new Gripper();
        Solenoid solenoid = new Solenoid();
        Gripper_V4 gripperV4 = new Gripper_V4(new USB2CAN());

        System.Timers.Timer timer = new System.Timers.Timer();
        private bool isConnected = false;
        private bool hallFlag = false;
        private bool lockFlag = false;
        private bool pushFlag = false;
        private bool timerFlag = false;

        private bool solPushFlag = false;
        private bool solPhotoFlag = false;

        int count = 0;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {


            var devices = gripper.GetUSBDevices();
            foreach (var device in devices)
            {
                portBox.Items.Add($"[{device.Key}] {device.Value}");
            }
            portBox.SelectedIndex = 0;


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

            // 솔레이노이드
            solenoid.begin("COM8");
            solenoid.receivingPacket(); // Solenoid 상시로 읽어오기
            
            txbSolLED.Text = "0";
            txbRed.Text = "0";
            txbGreen.Text = "0";
            txbBlue.Text = "0";
            txbBrightness.Text = "0";

            txbDXL.Text = "2000";
            txbLSV.Text = "0";
        }

        private void PortBox_DropDown(object? sender, EventArgs e)
        {
            portBox.Items.Clear();

            var devices = gripper.GetUSBDevices();
            foreach (var device in devices)
            {
                portBox.Items.Add($"[{device.Key}] {device.Value}");
            }
        }

        // USB 시작
        private void btnConnect_Click(object sender, EventArgs e)
        {

            var devices = gripper.GetUSBDevices();
            string port = devices.ElementAt(portBox.SelectedIndex).Key;

            if (baudBox.SelectedItem != null && int.TryParse(baudBox.SelectedItem.ToString(), out int baudrate))
            {
                if (!isConnected)
                {

                    if (gripper.begin(port, baudrate))
                    {
                        Log($"[O] Connected to {port} @ {baudBox.SelectedItem} baud\r\n", true);
                        btnConnect_Open();
                    }
                    else
                    {
                        Log($"[X] Failed to connect {port}\r\n", true);
                    }
                }
                else
                {
                    stopTimer();
                    Thread.Sleep(10);  // 주의
                    gripper.portClose();
                    btnConnect_Close();
                    Log($"[ ] Disconnected from {port}\r\n", true);

                }
            }
            else
            {
                Log($"Please set port or baudrate\r\n", true);
            }
        }


        // CAN 읽어오기
        private void btnTimer_Click(object sender, EventArgs e)
        {

            timer.Interval = 50;

            if (!timerFlag)
            {
                timer.Elapsed += Timer_Elapsed;
                timer.Start();

                gripper.receivingPacket();
                gripper.canStart();

                btnTimer.Text = "STOP";
                Log("Start Reading\r\n", true);
                timerFlag = true;
            }
            else
            {
                stopTimer();
            }

        }


        private void stopTimer()
        {
            timer.Elapsed -= Timer_Elapsed;
            timer.Stop();

            gripper.canStop();
            gripper.receivingStop();

            btnTimer.Text = "READ";
            Log("Stop Reading\r\n", true);
            timerFlag = false;
        }

        // -> Task 로 만들어서 사용 데이터 출력 부분
        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {

            if (gripper.isUSBConnected())
            {


                if (gripper.isUSBOpen())
                {
                    btnConnect_Open();

                }
                count++;

                byte[] last = gripper.getData();    // 마지막 값 가져오기

                if (txbHall.InvokeRequired)
                {
                    txbHall.Invoke(new Action(() =>
                    {
                        txbHall.Text = gripper.recvPacket.hallSensor.raw.ToString();
                    }));
                }
                if (txbDXL_Read.InvokeRequired)
                {
                    txbDXL_Read.Invoke(new Action(() =>
                    {
                        txbDXL_Read.Text = gripper.recvPacket.dxl.position.ToString();
                    }));
                }
                if (txbLSV_Read.InvokeRequired)
                {
                    txbLSV_Read.Invoke(new Action(() =>
                    {
                        txbLSV_Read.Text = gripper.recvPacket.lsv.position.ToString();
                    }));
                }
                if (txbCount.InvokeRequired)
                {
                    txbCount.Invoke(new Action(() =>
                    {
                        txbCount.Text = gripper.count.ToString();
                    }));
                }

                

                //txbHall.Text = gripper.recvPacket.hallSensor.raw.ToString();
                //txbDXL_Read.Text = gripper.recvPacket.dxl.position.ToString();
                //txbLSV_Read.Text = gripper.recvPacket.lsv.position.ToString();
                //txbCount.Text = gripper.count.ToString();

            }
            else
            {

                btnConnect_Close();
                Log($" {count--} {gripper.portName} {gripper.isUSBOpen()} - {gripper.lastPort}\r\n", true);
            }


            if (solenoid.isUSBOpen())
            {
                byte[] solLast = solenoid.getData();

                if (txbSolState.InvokeRequired)
                {
                    txbSolState.Invoke(new Action(() =>
                    {
                        txbSolState.Text = solenoid.recvPacket.SOLENOID_STATE.ToString();
                    }));
                }

                if (txbPhoto.InvokeRequired)
                {
                    txbPhoto.Invoke(new Action(() =>
                    {
                        txbPhoto.Text = solenoid.recvPacket.PHOTO_TRIGGER.ToString();
                    }));
                }
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


        // 고정회전용 값 변경은 Gripper.cs 
        private void btnLock_Click(object sender, EventArgs e)
        {

            if (lockFlag)
            {
                gripper.rotateDXL(false);
                lockFlag = false;
                btnLock.Text = "Lock";
            }
            else
            {
                gripper.rotateDXL(true);
                lockFlag = true;
                btnLock.Text = "Unlock";
            }
        }

        private void btnFixedPush_Click(object sender, EventArgs e)
        {
            if (pushFlag)
            {
                gripper.releaseLinear();
                pushFlag = false;
                btnFixedPush.Text = "Push";
            }
            else
            {

                gripper.pushLinear();
                pushFlag = true;
                btnFixedPush.Text = "Release";
            }
        }


        // 캔데이터 참고용
        private void btnCANTest_Click(object sender, EventArgs e)
        {
            byte[] testData = { 0xA1, 0xB2, 0xC3, 0xD4, 0xE5, 0xF6 };
            byte[] testFD = new byte[24];

            for (int i = 0; i < testFD.Length; i++)
            {
                testFD[i] = (byte)i;
            }


            gripper.sendCANPacket(CAN_TYPE.CAN_FD, 0x124, CAN_DLC.FDCAN_DLC_BYTE_24, testFD);




            Log("0x{0:X2} DLC:{1:D2} Length:{2} =>", true, gripper.lastSendPacket?.id, gripper.lastSendPacket?.dlc, gripper.lastSendPacket?.data.Count);


            for (int i = 0; i < gripper.lastSendPacket?.data.Count; i++)
            {

                Log($"|{gripper.lastSendPacket?.data[i]:X2}", false);
            }
            Log("\r\n", false);

        }

        // Hall 센서 제어
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
            gripper.rotate(ushort.Parse(txbDXL.Text));  // 모터 회전 권장값 2000 => 3400
        }

        private void btnPush_Click(object sender, EventArgs e)
        {
            gripper.push(ushort.Parse(txbLSV.Text));    // 리니어 모터 회전 권장값 0 => 1000
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

        private void btnSolLED_Click(object sender, EventArgs e)
        {
            solenoid.setLED(int.Parse(txbSolLED.Text));
        }

        private void btnSol_Click(object sender, EventArgs e)
        {
            if (solPushFlag)
            {
                solenoid.release();
                btnSol.Text = "Push";
                solPushFlag = false;
            }
            else 
            {
                solenoid.push();
                btnSol.Text = "Release";
                solPushFlag = true;
            }
        }

        private void btnPhoto_Click(object sender, EventArgs e)
        {
            if (solPhotoFlag)
            {
                solenoid.stopPhotoSensing();
                btnPhoto.Text = "PhotoOff";
                solPhotoFlag = false;
            }
            else
            {
                solenoid.startPhotoSensing();
                btnPhoto.Text = "PhotoOn";
                solPhotoFlag = true;
            }
        }
    }
}
