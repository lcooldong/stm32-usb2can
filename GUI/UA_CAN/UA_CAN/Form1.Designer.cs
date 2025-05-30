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
            lblDXL = new Label();
            lblLSV = new Label();
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
            btnReq = new Button();
            txbDXL = new TextBox();
            txbLSV = new TextBox();
            txbHall = new TextBox();
            txbLSV_Read = new TextBox();
            txbDXL_Read = new TextBox();
            btnPush = new Button();
            btnRotate = new Button();
            txbCount = new TextBox();
            btnLock = new Button();
            btnFixedPush = new Button();
            btnSolLED = new Button();
            txbSolLED = new TextBox();
            btnSol = new Button();
            btnPhoto = new Button();
            txbSolState = new TextBox();
            txbPhoto = new TextBox();
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
            btnTimer.Text = "READ";
            btnTimer.UseVisualStyleBackColor = true;
            btnTimer.Click += btnTimer_Click;
            // 
            // lblDXL
            // 
            lblDXL.AutoSize = true;
            lblDXL.Location = new Point(58, 410);
            lblDXL.Name = "lblDXL";
            lblDXL.Size = new Size(42, 15);
            lblDXL.TabIndex = 8;
            lblDXL.Text = "Locker";
            // 
            // lblLSV
            // 
            lblLSV.AutoSize = true;
            lblLSV.Location = new Point(58, 444);
            lblLSV.Name = "lblLSV";
            lblLSV.Size = new Size(43, 15);
            lblLSV.TabIndex = 9;
            lblLSV.Text = "Pusher";
            // 
            // lblHall
            // 
            lblHall.AutoSize = true;
            lblHall.Location = new Point(58, 473);
            lblHall.Name = "lblHall";
            lblHall.Size = new Size(28, 15);
            lblHall.TabIndex = 10;
            lblHall.Text = "Hall";
            // 
            // btnCANTest
            // 
            btnCANTest.Location = new Point(221, 357);
            btnCANTest.Name = "btnCANTest";
            btnCANTest.Size = new Size(75, 23);
            btnCANTest.TabIndex = 11;
            btnCANTest.Text = "CANTest";
            btnCANTest.UseVisualStyleBackColor = true;
            btnCANTest.Click += btnCANTest_Click;
            // 
            // btnHall
            // 
            btnHall.Location = new Point(378, 473);
            btnHall.Name = "btnHall";
            btnHall.Size = new Size(75, 23);
            btnHall.TabIndex = 12;
            btnHall.Text = "HALL";
            btnHall.UseVisualStyleBackColor = true;
            btnHall.Click += btnHall_Click;
            // 
            // btnLED
            // 
            btnLED.Location = new Point(505, 530);
            btnLED.Name = "btnLED";
            btnLED.Size = new Size(171, 23);
            btnLED.TabIndex = 13;
            btnLED.Text = "ChangeColor";
            btnLED.UseVisualStyleBackColor = true;
            btnLED.Click += btnLED_Click;
            // 
            // txbRed
            // 
            txbRed.Location = new Point(576, 397);
            txbRed.Name = "txbRed";
            txbRed.Size = new Size(100, 23);
            txbRed.TabIndex = 14;
            txbRed.TextChanged += txb_Changed;
            txbRed.KeyPress += txb_KeyPress;
            // 
            // txbGreen
            // 
            txbGreen.Location = new Point(576, 426);
            txbGreen.Name = "txbGreen";
            txbGreen.Size = new Size(100, 23);
            txbGreen.TabIndex = 15;
            txbGreen.TextChanged += txb_Changed;
            txbGreen.KeyPress += txb_KeyPress;
            // 
            // txbBlue
            // 
            txbBlue.Location = new Point(576, 455);
            txbBlue.Name = "txbBlue";
            txbBlue.Size = new Size(100, 23);
            txbBlue.TabIndex = 16;
            txbBlue.TextChanged += txb_Changed;
            txbBlue.KeyPress += txb_KeyPress;
            // 
            // txbBrightness
            // 
            txbBrightness.Location = new Point(576, 484);
            txbBrightness.Name = "txbBrightness";
            txbBrightness.Size = new Size(100, 23);
            txbBrightness.TabIndex = 17;
            txbBrightness.TextChanged += txb_Changed;
            txbBrightness.KeyPress += txb_KeyPress;
            // 
            // lblRed
            // 
            lblRed.AutoSize = true;
            lblRed.Location = new Point(505, 400);
            lblRed.Name = "lblRed";
            lblRed.Size = new Size(27, 15);
            lblRed.TabIndex = 18;
            lblRed.Text = "Red";
            // 
            // lblGreen
            // 
            lblGreen.AutoSize = true;
            lblGreen.Location = new Point(505, 429);
            lblGreen.Name = "lblGreen";
            lblGreen.Size = new Size(38, 15);
            lblGreen.TabIndex = 19;
            lblGreen.Text = "Green";
            // 
            // lblBlue
            // 
            lblBlue.AutoSize = true;
            lblBlue.Location = new Point(505, 459);
            lblBlue.Name = "lblBlue";
            lblBlue.Size = new Size(30, 15);
            lblBlue.TabIndex = 20;
            lblBlue.Text = "Blue";
            // 
            // lblBrightness
            // 
            lblBrightness.AutoSize = true;
            lblBrightness.Location = new Point(505, 490);
            lblBrightness.Name = "lblBrightness";
            lblBrightness.Size = new Size(62, 15);
            lblBrightness.TabIndex = 21;
            lblBrightness.Text = "Brightness";
            // 
            // btnPkt
            // 
            btnPkt.Location = new Point(140, 358);
            btnPkt.Name = "btnPkt";
            btnPkt.Size = new Size(75, 23);
            btnPkt.TabIndex = 22;
            btnPkt.Text = "GetData";
            btnPkt.UseVisualStyleBackColor = true;
            btnPkt.Click += btnPkt_Click;
            // 
            // btnReq
            // 
            btnReq.Location = new Point(305, 358);
            btnReq.Name = "btnReq";
            btnReq.Size = new Size(75, 23);
            btnReq.TabIndex = 23;
            btnReq.Text = "REQUEST";
            btnReq.UseVisualStyleBackColor = true;
            btnReq.Click += btnReq_Click;
            // 
            // txbDXL
            // 
            txbDXL.Location = new Point(260, 407);
            txbDXL.Name = "txbDXL";
            txbDXL.Size = new Size(100, 23);
            txbDXL.TabIndex = 24;
            txbDXL.TextChanged += motorValue_Changed;
            txbDXL.KeyPress += motorValue_KeyPressed;
            // 
            // txbLSV
            // 
            txbLSV.Location = new Point(260, 444);
            txbLSV.Name = "txbLSV";
            txbLSV.Size = new Size(100, 23);
            txbLSV.TabIndex = 25;
            txbLSV.TextChanged += motorValue_Changed;
            txbLSV.KeyPress += motorValue_KeyPressed;
            // 
            // txbHall
            // 
            txbHall.Location = new Point(140, 469);
            txbHall.Name = "txbHall";
            txbHall.ReadOnly = true;
            txbHall.Size = new Size(100, 23);
            txbHall.TabIndex = 26;
            txbHall.TabStop = false;
            // 
            // txbLSV_Read
            // 
            txbLSV_Read.Location = new Point(140, 441);
            txbLSV_Read.Name = "txbLSV_Read";
            txbLSV_Read.ReadOnly = true;
            txbLSV_Read.Size = new Size(100, 23);
            txbLSV_Read.TabIndex = 27;
            txbLSV_Read.TabStop = false;
            // 
            // txbDXL_Read
            // 
            txbDXL_Read.Location = new Point(140, 407);
            txbDXL_Read.Name = "txbDXL_Read";
            txbDXL_Read.ReadOnly = true;
            txbDXL_Read.Size = new Size(100, 23);
            txbDXL_Read.TabIndex = 28;
            txbDXL_Read.TabStop = false;
            // 
            // btnPush
            // 
            btnPush.Location = new Point(378, 444);
            btnPush.Name = "btnPush";
            btnPush.Size = new Size(75, 23);
            btnPush.TabIndex = 29;
            btnPush.Text = "Push";
            btnPush.UseVisualStyleBackColor = true;
            btnPush.Click += btnPush_Click;
            // 
            // btnRotate
            // 
            btnRotate.Location = new Point(378, 410);
            btnRotate.Name = "btnRotate";
            btnRotate.Size = new Size(75, 23);
            btnRotate.TabIndex = 30;
            btnRotate.Text = "Rotate";
            btnRotate.UseVisualStyleBackColor = true;
            btnRotate.Click += btnRotate_Click;
            // 
            // txbCount
            // 
            txbCount.Location = new Point(57, 358);
            txbCount.Name = "txbCount";
            txbCount.ReadOnly = true;
            txbCount.Size = new Size(44, 23);
            txbCount.TabIndex = 31;
            txbCount.TabStop = false;
            // 
            // btnLock
            // 
            btnLock.Location = new Point(58, 530);
            btnLock.Name = "btnLock";
            btnLock.Size = new Size(139, 42);
            btnLock.TabIndex = 32;
            btnLock.Text = "Lock Button";
            btnLock.UseVisualStyleBackColor = true;
            btnLock.Click += btnLock_Click;
            // 
            // btnFixedPush
            // 
            btnFixedPush.Location = new Point(224, 530);
            btnFixedPush.Name = "btnFixedPush";
            btnFixedPush.Size = new Size(136, 42);
            btnFixedPush.TabIndex = 33;
            btnFixedPush.Text = "Push Button";
            btnFixedPush.UseVisualStyleBackColor = true;
            btnFixedPush.Click += btnFixedPush_Click;
            // 
            // btnSolLED
            // 
            btnSolLED.Location = new Point(997, 400);
            btnSolLED.Name = "btnSolLED";
            btnSolLED.Size = new Size(86, 32);
            btnSolLED.TabIndex = 34;
            btnSolLED.Text = "SOL LED";
            btnSolLED.UseVisualStyleBackColor = true;
            btnSolLED.Click += btnSolLED_Click;
            // 
            // txbSolLED
            // 
            txbSolLED.Location = new Point(891, 402);
            txbSolLED.Name = "txbSolLED";
            txbSolLED.Size = new Size(100, 23);
            txbSolLED.TabIndex = 35;
            // 
            // btnSol
            // 
            btnSol.Location = new Point(997, 456);
            btnSol.Name = "btnSol";
            btnSol.Size = new Size(86, 32);
            btnSol.TabIndex = 36;
            btnSol.Text = "Push";
            btnSol.UseVisualStyleBackColor = true;
            btnSol.Click += btnSol_Click;
            // 
            // btnPhoto
            // 
            btnPhoto.Location = new Point(997, 506);
            btnPhoto.Name = "btnPhoto";
            btnPhoto.Size = new Size(86, 32);
            btnPhoto.TabIndex = 37;
            btnPhoto.Text = "StartSensing";
            btnPhoto.UseVisualStyleBackColor = true;
            btnPhoto.Click += btnPhoto_Click;
            // 
            // txbSolState
            // 
            txbSolState.Location = new Point(891, 459);
            txbSolState.Name = "txbSolState";
            txbSolState.ReadOnly = true;
            txbSolState.Size = new Size(100, 23);
            txbSolState.TabIndex = 38;
            txbSolState.TabStop = false;
            // 
            // txbPhoto
            // 
            txbPhoto.Location = new Point(891, 512);
            txbPhoto.Name = "txbPhoto";
            txbPhoto.ReadOnly = true;
            txbPhoto.Size = new Size(100, 23);
            txbPhoto.TabIndex = 39;
            txbPhoto.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1122, 609);
            Controls.Add(txbPhoto);
            Controls.Add(txbSolState);
            Controls.Add(btnPhoto);
            Controls.Add(btnSol);
            Controls.Add(txbSolLED);
            Controls.Add(btnSolLED);
            Controls.Add(btnFixedPush);
            Controls.Add(btnLock);
            Controls.Add(txbCount);
            Controls.Add(btnRotate);
            Controls.Add(btnPush);
            Controls.Add(txbDXL_Read);
            Controls.Add(txbLSV_Read);
            Controls.Add(txbHall);
            Controls.Add(txbLSV);
            Controls.Add(txbDXL);
            Controls.Add(btnReq);
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
            Controls.Add(lblLSV);
            Controls.Add(lblDXL);
            Controls.Add(btnTimer);
            Controls.Add(rtbConsole);
            Controls.Add(lblBaud);
            Controls.Add(lblPort);
            Controls.Add(baudBox);
            Controls.Add(portBox);
            Controls.Add(btnConnect);
            Name = "Form1";
            Text = "UA_TriGripper";
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
        private Label lblDXL;
        private Label lblLSV;
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
        private Button btnReq;
        private TextBox txbDXL;
        private TextBox txbLSV;
        private TextBox txbHall;
        private TextBox txbLSV_Read;
        private TextBox txbDXL_Read;
        private Button btnPush;
        private Button btnRotate;
        private TextBox txbCount;
        private Button btnLock;
        private Button btnFixedPush;
        private Button btnSolLED;
        private TextBox txbSolLED;
        private Button btnSol;
        private Button btnPhoto;
        private TextBox txbSolState;
        private TextBox txbPhoto;
    }
}
