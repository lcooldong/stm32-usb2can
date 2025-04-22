using System.IO.Ports;

namespace UA_CAN
{
    public partial class Form1 : Form
    {
        USB2CAN Serial = new USB2CAN();
        string port = "COM23";

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            var devices = Serial.GetUSBDevices();
            foreach (var device in devices)
            {
                portBox.Items.Add(device);
            }
            
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Serial.begin(port);
        }



        

    }
}
