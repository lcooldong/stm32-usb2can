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
            button1 = new Button();
            portBox = new ComboBox();
            baudBox = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(449, 11);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // portBox
            // 
            portBox.FormattingEnabled = true;
            portBox.Location = new Point(92, 11);
            portBox.Name = "portBox";
            portBox.Size = new Size(121, 23);
            portBox.TabIndex = 1;
            // 
            // baudBox
            // 
            baudBox.FormattingEnabled = true;
            baudBox.Location = new Point(296, 11);
            baudBox.Name = "baudBox";
            baudBox.Size = new Size(121, 23);
            baudBox.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(57, 15);
            label1.Name = "label1";
            label1.Size = new Size(29, 15);
            label1.TabIndex = 3;
            label1.Text = "Port";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(244, 14);
            label2.Name = "label2";
            label2.Size = new Size(34, 15);
            label2.TabIndex = 4;
            label2.Text = "Baud";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(baudBox);
            Controls.Add(portBox);
            Controls.Add(button1);
            Name = "Form1";
            Text = "F";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private ComboBox portBox;
        private ComboBox baudBox;
        private Label label1;
        private Label label2;
    }
}
