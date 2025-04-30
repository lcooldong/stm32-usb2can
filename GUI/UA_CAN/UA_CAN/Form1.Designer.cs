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
            components = new System.ComponentModel.Container();
            btnConnect = new Button();
            portBox = new ComboBox();
            baudBox = new ComboBox();
            lblPort = new Label();
            lblBaud = new Label();
            rtbConsole = new RichTextBox();
            timer1 = new System.Windows.Forms.Timer(components);
            btnTimer = new Button();
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
            rtbConsole.Size = new Size(543, 225);
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
        private System.Windows.Forms.Timer timer1;
        private Button btnTimer;
    }
}
