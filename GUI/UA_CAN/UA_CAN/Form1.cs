using System.IO.Ports;
using System.Timers;
using System.Windows.Forms;



namespace UA_CAN
{
    public partial class Form1 : Form
    {
        USB2CAN Serial = new USB2CAN();

        private bool isConnected = false;
        private bool timerFlag = false;
        int count = 0;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            var devices = Serial.GetUSBDevices();
            foreach (var device in devices)
            {
                portBox.Items.Add($"[{device.Key}] {device.Value}");
            }
            portBox.SelectedIndex = 0;

            foreach (int rate in Serial.baudRates)
            {
                baudBox.Items.Add(rate.ToString());
            }
            baudBox.SelectedIndex = 4;



            rtbConsole.Multiline = true;
            rtbConsole.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbConsole.SelectionAlignment = HorizontalAlignment.Left;


            portBox.DropDown += PortBox_DropDown;
            

            //Serial.autoConnect();   // 
        }

        private void PortBox_DropDown(object? sender, EventArgs e)
        {
            portBox.Items.Clear();
            var devices = Serial.GetUSBDevices();
            foreach (var device in devices)
            {
                portBox.Items.Add($"[{device.Key}] {device.Value}");
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var devices = Serial.GetUSBDevices();
            string port = devices.ElementAt(portBox.SelectedIndex).Key;

            if (baudBox.SelectedItem != null && int.TryParse(baudBox.SelectedItem.ToString(), out int baudrate))
            {
                if (!isConnected)
                {

                    if (Serial.begin(port, baudrate))
                    {
                        Log($"[O] Connected to {port} @ {baudBox.SelectedItem} baud");
                        isConnected = true;

                        btnConnect.Text = "Disconnect";
                    }
                    else
                    {
                        Log($"[X] Failed to connect {port}");
                    }
                }
                else 
                {
                    Serial.close();
                    isConnected= false;
                    btnConnect.Text = "Connect";
                    Log($"[ ] Disconnected from {port}");
                }
            }
            else
            {
                Log($"Please set port or baudrate");
            }

           
        }

        private void btnTimer_Click(object sender, EventArgs e)
        {

            timer1.Interval = 1000;

            if (!timerFlag)
            {
                
                timer1.Tick += Timer_Tick;
                timer1.Start();
                timerFlag = true;
            }
            else 
            {
                timer1.Tick -= Timer_Tick;
                timer1.Stop();
                timerFlag = false;
            }


        }

        private void Timer_Tick(object? sender, EventArgs e)
        {



            //Log($"{count++}");
            if (Serial.isUSBConnected())
            {
                Log($" {count++} {Serial.sp.PortName} {Serial.sp.IsOpen} - {Serial.lastPort}");
            }
            else
            {
                Log($" {count--} {Serial.sp.PortName} {Serial.sp.IsOpen} - {Serial.lastPort}");
                isConnected = false;
                btnConnect.Text = "Connect";
            }


        }

        private void Log(string message)
        {

            string time = DateTime.Now.ToString("HH:mm::ss");


            rtbConsole.AppendText($"{time}->");

            if (rtbConsole.InvokeRequired)
            {
                rtbConsole.Invoke(new Action(() =>
                {
                    rtbConsole.AppendText(message + Environment.NewLine);
                }));
            }
            else
            {
                rtbConsole.AppendText(message + Environment.NewLine);
            }
            rtbConsole.ScrollToCaret();
        }




    }
}
