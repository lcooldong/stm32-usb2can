using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gripper_V4
{
    public partial class Form1 : Form
    {
        // 내부 모니터링 루프가 포함된 Gripper 클래스 사용
        Gripper gripper = new Gripper(new USB2CAN());

        // UI 갱신을 위한 전용 타이머 (데이터 수신은 Gripper 내부 Task가 담당)
        private System.Windows.Forms.Timer uiRefreshTimer = new System.Windows.Forms.Timer();
        private bool isConnected = false;

        public Form1()
        {
            InitializeComponent();

            // Gripper 클래스에서 올라오는 이벤트를 rtb_Console에 연결
            gripper.OnMessageReceived += (msg) => {
                Log("[GRIPPER]:"+ msg + "\r\n", true);
            };

            // UI 갱신 타이머 설정
            uiRefreshTimer.Interval = 50;
            uiRefreshTimer.Tick += (s, e) => UpdateUIStatus();
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

                // gripper.Connect 내부에서 자동으로 StartMonitoring()이 호출됨
                if (gripper.Connect(port, baud))
                {
                    isConnected = true;
                    btn_Connect.Text = "Disconnect";
                    btn_Connect.BackColor = Color.LightGreen;

                    // UI 갱신 타이머 시작
                    uiRefreshTimer.Start();
                    Log($"[O] Connected to {port}\r\n", true);
                }
            }
            else
            {
                // UI 타이머 중지 및 장치 해제 (Disconnect 내부에서 Task 종료 처리함)
                uiRefreshTimer.Stop();
                gripper.Disconnect();

                isConnected = false;
                btn_Connect.Text = "Connect";
                btn_Connect.BackColor = SystemColors.Control;
                lbl_Status.Text = "연결 끊김";
                Log("[ ] Disconnected\r\n", true);
            }
        }

        private void UpdateUIStatus()
        {
            // Gripper 내부 Task에 의해 실시간 갱신된 값을 화면에 뿌려주기만 함
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

        // --- 모터 제어 버튼 (이전과 동일) ---
        private void btn_Gripper_Move_Click(object sender, EventArgs e)
        {
            if (ushort.TryParse(txtb_Gripper_target.Text, out ushort pos))
                gripper.MoveGripper(pos, 100);
        }

        private void btn_Pusher_Move_Click(object sender, EventArgs e)
        {
            if (ushort.TryParse(txtb_Pusher_target.Text, out ushort pos))
                gripper.MovePusher(pos, 200);
        }

        private void btn_Gripper_Click(object sender, EventArgs e)
        {
            if (btn_Gripper.Text == "Close") { gripper.MoveGripper(Gripper.gripper_close_limit, 100); btn_Gripper.Text = "Open"; }
            else { gripper.MoveGripper(Gripper.gripper_open_limit, 100); btn_Gripper.Text = "Close"; }
        }

        private void btn_Pusher_Click(object sender, EventArgs e)
        {
            if (btn_Pusher.Text == "Push") { gripper.MovePusher(Gripper.pusher_push_limit, 200); btn_Pusher.Text = "Release"; }
            else { gripper.MovePusher(Gripper.pusher_release_limit, 200); btn_Pusher.Text = "Push"; }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 폼 종료 시 시리얼 포트 및 내부 Task 안전 종료
            uiRefreshTimer.Stop();
            if (isConnected)
            {
                gripper.Disconnect();
            }
        }

        private void btn_Receive_Click(object sender, EventArgs e)
        {
            if (gripper.IsConnected)
            {
                // 1. PC에서 나가는 요청 패킷 로그 (0x10 = GET_STATE)
                Log("[TX] Request State (CMD: 0x10)\r\n", true);

                // 2. 아두이노에 요청
                gripper.RequestState();
            }
            else
            {
                Log("[X] Not Connected\r\n", true);
            }
        }

        //private void btn_CAN_Test_Click(object sender, EventArgs e)
        //{



        //}
    }
}