namespace Gripper_V4
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
            rtb_Console = new RichTextBox();
            cb_PortName = new ComboBox();
            cb_Baudrate = new ComboBox();
            btn_Connect = new Button();
            btn_Gripper_Move = new Button();
            btn_Pusher_Move = new Button();
            lbl_PortName = new Label();
            lbl_Baudrate = new Label();
            lbl_Gripper = new Label();
            lbl_Pusher = new Label();
            txtb_Gripper_pos = new TextBox();
            txtb_Pusher_pos = new TextBox();
            txtb_Pusher_target = new TextBox();
            txtb_Gripper_target = new TextBox();
            btn_Gripper = new Button();
            btn_Pusher = new Button();
            btn_Receive = new Button();
            lbl_Status = new Label();
            SuspendLayout();
            // 
            // rtb_Console
            // 
            rtb_Console.Location = new Point(33, 91);
            rtb_Console.Name = "rtb_Console";
            rtb_Console.Size = new Size(648, 218);
            rtb_Console.TabIndex = 0;
            rtb_Console.Text = "";
            // 
            // cb_PortName
            // 
            cb_PortName.FormattingEnabled = true;
            cb_PortName.Location = new Point(83, 29);
            cb_PortName.Name = "cb_PortName";
            cb_PortName.Size = new Size(317, 23);
            cb_PortName.TabIndex = 1;
            // 
            // cb_Baudrate
            // 
            cb_Baudrate.FormattingEnabled = true;
            cb_Baudrate.Location = new Point(472, 30);
            cb_Baudrate.Name = "cb_Baudrate";
            cb_Baudrate.Size = new Size(121, 23);
            cb_Baudrate.TabIndex = 2;
            // 
            // btn_Connect
            // 
            btn_Connect.Location = new Point(606, 29);
            btn_Connect.Name = "btn_Connect";
            btn_Connect.Size = new Size(75, 23);
            btn_Connect.TabIndex = 3;
            btn_Connect.Text = "Connect";
            btn_Connect.UseVisualStyleBackColor = true;
            btn_Connect.Click += btn_Connect_Click;
            // 
            // btn_Gripper_Move
            // 
            btn_Gripper_Move.Location = new Point(353, 339);
            btn_Gripper_Move.Name = "btn_Gripper_Move";
            btn_Gripper_Move.Size = new Size(75, 23);
            btn_Gripper_Move.TabIndex = 4;
            btn_Gripper_Move.Text = "Grip";
            btn_Gripper_Move.UseVisualStyleBackColor = true;
            btn_Gripper_Move.Click += btn_Gripper_Move_Click;
            // 
            // btn_Pusher_Move
            // 
            btn_Pusher_Move.Location = new Point(353, 393);
            btn_Pusher_Move.Name = "btn_Pusher_Move";
            btn_Pusher_Move.Size = new Size(75, 23);
            btn_Pusher_Move.TabIndex = 5;
            btn_Pusher_Move.Text = "Move";
            btn_Pusher_Move.UseVisualStyleBackColor = true;
            btn_Pusher_Move.Click += btn_Pusher_Move_Click;
            // 
            // lbl_PortName
            // 
            lbl_PortName.AutoSize = true;
            lbl_PortName.Location = new Point(33, 32);
            lbl_PortName.Name = "lbl_PortName";
            lbl_PortName.Size = new Size(29, 15);
            lbl_PortName.TabIndex = 6;
            lbl_PortName.Text = "Port";
            // 
            // lbl_Baudrate
            // 
            lbl_Baudrate.AutoSize = true;
            lbl_Baudrate.Location = new Point(427, 33);
            lbl_Baudrate.Name = "lbl_Baudrate";
            lbl_Baudrate.Size = new Size(34, 15);
            lbl_Baudrate.TabIndex = 7;
            lbl_Baudrate.Text = "Baud";
            // 
            // lbl_Gripper
            // 
            lbl_Gripper.AutoSize = true;
            lbl_Gripper.Location = new Point(33, 343);
            lbl_Gripper.Name = "lbl_Gripper";
            lbl_Gripper.Size = new Size(46, 15);
            lbl_Gripper.TabIndex = 8;
            lbl_Gripper.Text = "Gripper";
            // 
            // lbl_Pusher
            // 
            lbl_Pusher.AutoSize = true;
            lbl_Pusher.Location = new Point(33, 397);
            lbl_Pusher.Name = "lbl_Pusher";
            lbl_Pusher.Size = new Size(43, 15);
            lbl_Pusher.TabIndex = 9;
            lbl_Pusher.Text = "Pusher";
            // 
            // txtb_Gripper_pos
            // 
            txtb_Gripper_pos.Location = new Point(94, 340);
            txtb_Gripper_pos.Name = "txtb_Gripper_pos";
            txtb_Gripper_pos.ReadOnly = true;
            txtb_Gripper_pos.Size = new Size(100, 23);
            txtb_Gripper_pos.TabIndex = 11;
            // 
            // txtb_Pusher_pos
            // 
            txtb_Pusher_pos.Location = new Point(94, 394);
            txtb_Pusher_pos.Name = "txtb_Pusher_pos";
            txtb_Pusher_pos.ReadOnly = true;
            txtb_Pusher_pos.Size = new Size(100, 23);
            txtb_Pusher_pos.TabIndex = 12;
            // 
            // txtb_Pusher_target
            // 
            txtb_Pusher_target.Location = new Point(219, 393);
            txtb_Pusher_target.Name = "txtb_Pusher_target";
            txtb_Pusher_target.Size = new Size(100, 23);
            txtb_Pusher_target.TabIndex = 13;
            // 
            // txtb_Gripper_target
            // 
            txtb_Gripper_target.Location = new Point(219, 340);
            txtb_Gripper_target.Name = "txtb_Gripper_target";
            txtb_Gripper_target.Size = new Size(100, 23);
            txtb_Gripper_target.TabIndex = 14;
            // 
            // btn_Gripper
            // 
            btn_Gripper.Location = new Point(472, 343);
            btn_Gripper.Name = "btn_Gripper";
            btn_Gripper.Size = new Size(122, 69);
            btn_Gripper.TabIndex = 15;
            btn_Gripper.Text = "Close";
            btn_Gripper.UseVisualStyleBackColor = true;
            btn_Gripper.Click += btn_Gripper_Click;
            // 
            // btn_Pusher
            // 
            btn_Pusher.Location = new Point(639, 343);
            btn_Pusher.Name = "btn_Pusher";
            btn_Pusher.Size = new Size(122, 69);
            btn_Pusher.TabIndex = 16;
            btn_Pusher.Text = "Push";
            btn_Pusher.UseVisualStyleBackColor = true;
            btn_Pusher.Click += btn_Pusher_Click;
            // 
            // btn_Receive
            // 
            btn_Receive.Location = new Point(698, 91);
            btn_Receive.Name = "btn_Receive";
            btn_Receive.Size = new Size(75, 23);
            btn_Receive.TabIndex = 17;
            btn_Receive.Text = "Read";
            btn_Receive.UseVisualStyleBackColor = true;
            btn_Receive.Click += btn_Receive_Click;
            // 
            // lbl_Status
            // 
            lbl_Status.AutoSize = true;
            lbl_Status.Location = new Point(706, 33);
            lbl_Status.Name = "lbl_Status";
            lbl_Status.Size = new Size(67, 15);
            lbl_Status.TabIndex = 18;
            lbl_Status.Text = "MotorState";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lbl_Status);
            Controls.Add(btn_Receive);
            Controls.Add(btn_Pusher);
            Controls.Add(btn_Gripper);
            Controls.Add(txtb_Gripper_target);
            Controls.Add(txtb_Pusher_target);
            Controls.Add(txtb_Pusher_pos);
            Controls.Add(txtb_Gripper_pos);
            Controls.Add(lbl_Pusher);
            Controls.Add(lbl_Gripper);
            Controls.Add(lbl_Baudrate);
            Controls.Add(lbl_PortName);
            Controls.Add(btn_Pusher_Move);
            Controls.Add(btn_Gripper_Move);
            Controls.Add(btn_Connect);
            Controls.Add(cb_Baudrate);
            Controls.Add(cb_PortName);
            Controls.Add(rtb_Console);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox rtb_Console;
        private ComboBox cb_PortName;
        private ComboBox cb_Baudrate;
        private Button btn_Connect;
        private Button btn_Gripper_Move;
        private Button btn_Pusher_Move;
        private Label lbl_PortName;
        private Label lbl_Baudrate;
        private Label lbl_Gripper;
        private Label lbl_Pusher;
        private TextBox txtb_Gripper_pos;
        private TextBox txtb_Pusher_pos;
        private TextBox txtb_Pusher_target;
        private TextBox txtb_Gripper_target;
        private Button btn_Gripper;
        private Button btn_Pusher;
        private Button btn_Receive;
        private Label lbl_Status;
    }
}
