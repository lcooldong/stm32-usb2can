using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gripper_V4
{
    public partial class Form1 : Form
    {
        Gripper gripper = new Gripper(new USB2CAN());

        // 백그라운드 작업을 제어하기 위한 토큰
        private CancellationTokenSource _cts_receive;
        private bool isReceiving = false;
        private bool isConnected = false;

        public Form1()
        {
            InitializeComponent();

            // Gripper 클래스의 이벤트를 Form의 Log 메서드와 연결
            gripper.OnMessageReceived += (msg) => {
                Log(msg + "\r\n", true);
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdatePortList();
            string[] baudrates = { "9600", "19200", "38400", "57600", "115200", "230400", "460800", "921600" };
            cb_Baudrate.Items.AddRange(baudrates);
            cb_Baudrate.SelectedIndex = 4;

            txtb_Gripper_target.Text = "0";
            txtb_Pusher_target.Text = "0";
            cb_PortName.DropDown += (s, ev) => UpdatePortList();
        }


        private void Log(string message, bool showTime)
        {
            if (rtb_Console.InvokeRequired)
            {
                rtb_Console.BeginInvoke(new Action(() => Log(message, showTime)));
                return;
            }

            if (showTime)
            {
                rtb_Console.AppendText($"[{DateTime.Now:HH:mm:ss}] ");
            }
            rtb_Console.AppendText(message);
            rtb_Console.ScrollToCaret();
        }


        private void UpdatePortList()
        {
            cb_PortName.Items.Clear();
            var devices = gripper.GetUSBDevices();
            foreach (var device in devices) cb_PortName.Items.Add($"[{device.Key}] {device.Value}");
            if (cb_PortName.Items.Count > 0) cb_PortName.SelectedIndex = 0;
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                if (cb_PortName.SelectedItem == null) return;
                string port = cb_PortName.SelectedItem.ToString().Split('[', ']')[1];
                int baud = int.Parse(cb_Baudrate.SelectedItem.ToString());

                if (gripper.Connect(port, baud))
                {
                    isConnected = true;
                    btn_Connect.Text = "Disconnect";
                    btn_Connect.BackColor = Color.LightGreen;

                    // 연결 시 자동으로 수신 시작을 원하면 아래 주석 해제
                    // btn_Receive_Click(null, null);
                }
            }
            else
            {
                btn_Receive_Stop(); // 수신 중지 우선
                gripper.Disconnect();
                isConnected = false;
                btn_Connect.Text = "Connect";
                btn_Connect.BackColor = SystemColors.Control;
                lbl_Status.Text = "연결 끊김";
            }
        }

        // --- Task.Run 기반 실시간 수신 버튼 ---
        private void btn_Receive_Click(object sender, EventArgs e)
        {
            if (!isConnected) return;

            if (!isReceiving)
            {
                // 수신 시작
                _cts_receive = new CancellationTokenSource();
                isReceiving = true;
                btn_Receive.Text = "Stop Recv";
                btn_Receive.BackColor = Color.Orange;

                // 백그라운드 태스크 시작
                Task.Run(() => ReceiveLoop(_cts_receive.Token));
            }
            else
            {
                btn_Receive_Stop();
            }
        }

        private void btn_Receive_Stop()
        {
            _cts_receive?.Cancel();
            isReceiving = false;
            btn_Receive.Text = "Start Recv";
            btn_Receive.BackColor = SystemColors.Control;
        }


        // 현재 데이터 가져오는 함수 (필수) - UI 쪽만 제거하면 됨, 위쪽 Task.Run() 사용
        private async Task ReceiveLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    // 1. 데이터 해석 (백그라운드 스레드에서 실행)
                    gripper.ParseIncomingData();

                    // 2. UI 갱신 (UI 스레드로 대리 호출)
                    this.Invoke(new Action(() =>
                    {
                        UpdateUIStatus();
                    }));

                    // 3. 약 50ms 대기 (타이머 역할)
                    await Task.Delay(50, token);
                }
            }
            catch (TaskCanceledException) { /* 정상 종료 */ }
            catch (Exception ex)
            {
                this.Invoke(new Action(() =>
                {
                    lbl_Status.Text = "에러: " + ex.Message;
                }));
            }
        }

        private void UpdateUIStatus()
        {
            txtb_Gripper_pos.Text = gripper.CurrentGripperPos.ToString();
            txtb_Pusher_pos.Text = gripper.CurrentPusherPos.ToString();

            if (gripper.ConnectionStatus == 0x03)
            {
                lbl_Status.Text = "정상 연결 (0x03)";
                lbl_Status.ForeColor = Color.Green;
            }
            else
            {
                lbl_Status.Text = $"연결 확인 ({gripper.ConnectionStatus:X2})";
                lbl_Status.ForeColor = Color.Red;
            }
        }

        // --- 기존 제어 버튼 로직 ---
        private void btn_Gripper_Move_Click(object sender, EventArgs e)
        {
            if (ushort.TryParse(txtb_Gripper_target.Text, out ushort pos))
                gripper.MoveGripper(pos, 100);
        }

        private void btn_Pusher_Move_Click(object sender, EventArgs e)
        {
            if (ushort.TryParse(txtb_Pusher_target.Text, out ushort pos))
                gripper.MovePusher(pos, 100);
        }

        private void btn_Gripper_Click(object sender, EventArgs e)
        {
            if (btn_Gripper.Text == "Close") { gripper.MoveGripper(3400, 100); btn_Gripper.Text = "Open"; }
            else { gripper.MoveGripper(2000, 100); btn_Gripper.Text = "Close"; }
        }

        private void btn_Pusher_Click(object sender, EventArgs e)
        {
            if (btn_Pusher.Text == "Push") { gripper.MovePusher(1000, 100); btn_Pusher.Text = "Release"; }
            else { gripper.MovePusher(0, 100); btn_Pusher.Text = "Push"; }
        }

        private void btn_CAN_Test_Click(object sender, EventArgs e)
        {



        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isReceiving)
            {
                _cts_receive?.Cancel();
                // 비동기 작업이 정리될 시간을 아주 짧게 부여
                Thread.Sleep(50);
            }

            // 2. 시리얼 포트 및 CAN 채널 닫기
            if (isConnected)
            {
                gripper.Disconnect();
                isConnected = false;
                Console.WriteLine("System Safely Terminated.");
            }
        }
    }
}