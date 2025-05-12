using System.IO.Ports;
using System.Timers;
using System.Windows.Forms;



namespace UA_CAN
{
    public partial class Form1 : Form
    {
        static USB2CAN Serial = new USB2CAN();
        Gripper gripper = new Gripper(Serial);
        CANablePro can0 = new CANablePro(Serial);

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
            

            //Serial.autoConnect();   // Connect to last COM Port.
            
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
                        btnConnect_Open();
                        can0.read();    // read CAN After UART Open
                    }
                    else
                    {
                        Log($"[X] Failed to connect {port}");
                    }
                }
                else 
                {
                    Serial.close();
                    btnConnect_Close();
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
                
                if (Serial.sp.IsOpen) 
                {
                    btnConnect_Open();


                }
                Log($" {count++} {Serial.sp.PortName} {Serial.sp.IsOpen} - {Serial.lastPort}");

                Log($"{can0._packet.id} | {can0._packet.dlc}");
                for (int i = 0; i < can0._packet.dlc; i++)
                {
                    Log($"{can0._packet.data[i]} ", false);
                }
                Log("");
                //Log($"{can0._raw}");


                //gripper.sendPacket();
            }
            else
            {

                btnConnect_Close();
                Log($" {count--} {Serial.sp.PortName} {Serial.sp.IsOpen} - {Serial.lastPort}");
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

        private void Log(string format, params object[] args)
        {
            string message = string.Format(format, args);
            string time = DateTime.Now.ToString("HH:mm::ss");


            rtbConsole.AppendText($"{time}->");

            if (rtbConsole.InvokeRequired)
            {
                rtbConsole.Invoke(new Action(() =>
                {
                    rtbConsole.AppendText(message);
                }));
            }
            else
            {
                rtbConsole.AppendText(message);
            }
            rtbConsole.ScrollToCaret();
        }




    }
}
