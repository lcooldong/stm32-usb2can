namespace UA_CAN
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnConnect = new Button();
            portBox = new ComboBox();
            baudBox = new ComboBox();
            lblPort = new Label();
            lblBaud = new Label();
            rtbConsole = new RichTextBox();
            btnTimer = new Button();
            lblMotorLock = new Label();
            lblPusher = new Label();
            lblHall = new Label();
            btnCANTest = new Button();
            btnHall = new Button();
            btnLED = new Button();
            txbRed = new TextBox();
            txbGreen = new TextBox();
            txbBlue = new TextBox();
            txbBrightness = new TextBox();
            lblRed = new Label();
            lblGreen = new Label();
            lblBlue = new Label();
            lblBrightness = new Label();
            btnPkt = new Button();
            SuspendLayout();
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(635, 11);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(75, 23);
            btnConnect.TabIndex = 0;
            btnConnect.Text = "connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // portBox
            // 
            portBox.FormattingEnabled = true;
            portBox.Location = new Point(92, 11);
            portBox.Name = "portBox";
            portBox.Size = new Size(308, 23);
            portBox.TabIndex = 1;
            // 
            // baudBox
            // 
            baudBox.FormattingEnabled = true;
            baudBox.Location = new Point(482, 11);
            baudBox.Name = "baudBox";
            baudBox.Size = new Size(121, 23);
            baudBox.TabIndex = 2;
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(57, 15);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(29, 15);
            lblPort.TabIndex = 3;
            lblPort.Text = "Port";
            // 
            // lblBaud
            // 
            lblBaud.AutoSize = true;
            lblBaud.Location = new Point(442, 15);
            lblBaud.Name = "lblBaud";
            lblBaud.Size = new Size(34, 15);
            lblBaud.TabIndex = 4;
            lblBaud.Text = "Baud";
            // 
            // rtbConsole
            // 
            rtbConsole.Location = new Point(57, 127);
            rtbConsole.Name = "rtbConsole";
            rtbConsole.Size = new Size(954, 225);
            rtbConsole.TabIndex = 6;
            rtbConsole.Text = "";
            // 
            // btnTimer
            // 
            btnTimer.Location = new Point(57, 72);
            btnTimer.Name = "btnTimer";
            btnTimer.Size = new Size(75, 23);
            btnTimer.TabIndex = 7;
            btnTimer.Text = "Timer";
            btnTimer.UseVisualStyleBackColor = true;
            btnTimer.Click += btnTimer_Click;
            // 
            // lblMotorLock
            // 
            lblMotorLock.AutoSize = true;
            lblMotorLock.Location = new Point(57, 396);
            lblMotorLock.Name = "lblMotorLock";
            lblMotorLock.Size = new Size(32, 15);
            lblMotorLock.TabIndex = 8;
            lblMotorLock.Text = "Lock";
            // 
            // lblPusher
            // 
            lblPusher.AutoSize = true;
            lblPusher.Location = new Point(56, 428);
            lblPusher.Name = "lblPusher";
            lblPusher.Size = new Size(33, 15);
            lblPusher.TabIndex = 9;
            lblPusher.Text = "Push";
            // 
            // lblHall
            // 
            lblHall.AutoSize = true;
            lblHall.Location = new Point(58, 460);
            lblHall.Name = "lblHall";
            lblHall.Size = new Size(28, 15);
            lblHall.TabIndex = 10;
            lblHall.Text = "Hall";
            // 
            // btnCANTest
            // 
            btnCANTest.Location = new Point(57, 501);
            btnCANTest.Name = "btnCANTest";
            btnCANTest.Size = new Size(75, 23);
            btnCANTest.TabIndex = 11;
            btnCANTest.Text = "CANTest";
            btnCANTest.UseVisualStyleBackColor = true;
            btnCANTest.Click += btnCANTest_Click;
            // 
            // btnHall
            // 
            btnHall.Location = new Point(160, 501);
            btnHall.Name = "btnHall";
            btnHall.Size = new Size(75, 23);
            btnHall.TabIndex = 12;
            btnHall.Text = "HALL";
            btnHall.UseVisualStyleBackColor = true;
            btnHall.Click += btnHall_Click;
            // 
            // btnLED
            // 
            btnLED.Location = new Point(684, 522);
            btnLED.Name = "btnLED";
            btnLED.Size = new Size(171, 23);
            btnLED.TabIndex = 13;
            btnLED.Text = "ChangeColor";
            btnLED.UseVisualStyleBackColor = true;
            btnLED.Click += btnLED_Click;
            // 
            // txbRed
            // 
            txbRed.Location = new Point(755, 397);
            txbRed.Name = "txbRed";
            txbRed.Size = new Size(100, 23);
            txbRed.TabIndex = 14;
            txbRed.TextChanged += txb_Changed;
            txbRed.KeyPress += txb_KeyPress;
            // 
            // txbGreen
            // 
            txbGreen.Location = new Point(755, 426);
            txbGreen.Name = "txbGreen";
            txbGreen.Size = new Size(100, 23);
            txbGreen.TabIndex = 15;
            txbGreen.TextChanged += txb_Changed;
            txbGreen.KeyPress += txb_KeyPress;
            // 
            // txbBlue
            // 
            txbBlue.Location = new Point(755, 455);
            txbBlue.Name = "txbBlue";
            txbBlue.Size = new Size(100, 23);
            txbBlue.TabIndex = 16;
            txbBlue.TextChanged += txb_Changed;
            txbBlue.KeyPress += txb_KeyPress;
            // 
            // txbBrightness
            // 
            txbBrightness.Location = new Point(755, 484);
            txbBrightness.Name = "txbBrightness";
            txbBrightness.Size = new Size(100, 23);
            txbBrightness.TabIndex = 17;
            txbBrightness.TextChanged += txb_Changed;
            txbBrightness.KeyPress += txb_KeyPress;
            // 
            // lblRed
            // 
            lblRed.AutoSize = true;
            lblRed.Location = new Point(684, 400);
            lblRed.Name = "lblRed";
            lblRed.Size = new Size(27, 15);
            lblRed.TabIndex = 18;
            lblRed.Text = "Red";
            // 
            // lblGreen
            // 
            lblGreen.AutoSize = true;
            lblGreen.Location = new Point(684, 429);
            lblGreen.Name = "lblGreen";
            lblGreen.Size = new Size(38, 15);
            lblGreen.TabIndex = 19;
            lblGreen.Text = "Green";
            // 
            // lblBlue
            // 
            lblBlue.AutoSize = true;
            lblBlue.Location = new Point(684, 459);
            lblBlue.Name = "lblBlue";
            lblBlue.Size = new Size(30, 15);
            lblBlue.TabIndex = 20;
            lblBlue.Text = "Blue";
            // 
            // lblBrightness
            // 
            lblBrightness.AutoSize = true;
            lblBrightness.Location = new Point(684, 490);
            lblBrightness.Name = "lblBrightness";
            lblBrightness.Size = new Size(62, 15);
            lblBrightness.TabIndex = 21;
            lblBrightness.Text = "Brightness";
            // 
            // btnPkt
            // 
            btnPkt.Location = new Point(56, 543);
            btnPkt.Name = "btnPkt";
            btnPkt.Size = new Size(75, 23);
            btnPkt.TabIndex = 22;
            btnPkt.Text = "RecvPacket";
            btnPkt.UseVisualStyleBackColor = true;
            btnPkt.Click += btnPkt_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1122, 609);
            Controls.Add(btnPkt);
            Controls.Add(lblBrightness);
            Controls.Add(lblBlue);
            Controls.Add(lblGreen);
            Controls.Add(lblRed);
            Controls.Add(txbBrightness);
            Controls.Add(txbBlue);
            Controls.Add(txbGreen);
            Controls.Add(txbRed);
            Controls.Add(btnLED);
            Controls.Add(btnHall);
            Controls.Add(btnCANTest);
            Controls.Add(lblHall);
            Controls.Add(lblPusher);
            Controls.Add(lblMotorLock);
            Controls.Add(btnTimer);
            Controls.Add(rtbConsole);
            Controls.Add(lblBaud);
            Controls.Add(lblPort);
            Controls.Add(baudBox);
            Controls.Add(portBox);
            Controls.Add(btnConnect);
            Name = "Form1";
            Text = "F";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnConnect;
        private ComboBox portBox;
        private ComboBox baudBox;
        private Label lblPort;
        private Label lblBaud;
        private RichTextBox rtbConsole;
        private Button btnTimer;
        private Label lblMotorLock;
        private Label lblPusher;
        private Label lblHall;
        private Button btnCANTest;
        private Button btnHall;
        private Button btnLED;
        private TextBox txbRed;
        private TextBox txbGreen;
        private TextBox txbBlue;
        private TextBox txbBrightness;
        private Label lblRed;
        private Label lblGreen;
        private Label lblBlue;
        private Label lblBrightness;
        private Button btnPkt;
    }
}
