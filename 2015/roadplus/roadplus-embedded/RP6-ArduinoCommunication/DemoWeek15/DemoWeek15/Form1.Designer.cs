namespace DemoWeek15
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnMaxSpeed = new System.Windows.Forms.Button();
            this.txtMaxSpeed = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.serialPortSelectionBox = new System.Windows.Forms.ComboBox();
            this.refreshSerialPortsButton = new System.Windows.Forms.Button();
            this.BtnSpeed = new System.Windows.Forms.Button();
            this.LblSpeed = new System.Windows.Forms.Label();
            this.readMessageTimer = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnMaxSpeed
            // 
            this.btnMaxSpeed.Location = new System.Drawing.Point(112, 101);
            this.btnMaxSpeed.Name = "btnMaxSpeed";
            this.btnMaxSpeed.Size = new System.Drawing.Size(122, 53);
            this.btnMaxSpeed.TabIndex = 0;
            this.btnMaxSpeed.Text = "Send max speed";
            this.btnMaxSpeed.UseVisualStyleBackColor = true;
            this.btnMaxSpeed.Click += new System.EventHandler(this.btnMaxSpeed_Click);
            // 
            // txtMaxSpeed
            // 
            this.txtMaxSpeed.Location = new System.Drawing.Point(240, 116);
            this.txtMaxSpeed.Name = "txtMaxSpeed";
            this.txtMaxSpeed.Size = new System.Drawing.Size(382, 22);
            this.txtMaxSpeed.TabIndex = 1;
            this.txtMaxSpeed.Text = "0";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(428, 34);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(88, 24);
            this.connectButton.TabIndex = 9;
            this.connectButton.TabStop = false;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // serialPortSelectionBox
            // 
            this.serialPortSelectionBox.FormattingEnabled = true;
            this.serialPortSelectionBox.Location = new System.Drawing.Point(303, 34);
            this.serialPortSelectionBox.Name = "serialPortSelectionBox";
            this.serialPortSelectionBox.Size = new System.Drawing.Size(121, 24);
            this.serialPortSelectionBox.TabIndex = 8;
            // 
            // refreshSerialPortsButton
            // 
            this.refreshSerialPortsButton.Location = new System.Drawing.Point(189, 34);
            this.refreshSerialPortsButton.Name = "refreshSerialPortsButton";
            this.refreshSerialPortsButton.Size = new System.Drawing.Size(108, 26);
            this.refreshSerialPortsButton.TabIndex = 7;
            this.refreshSerialPortsButton.TabStop = false;
            this.refreshSerialPortsButton.Text = "Rescan Ports";
            this.refreshSerialPortsButton.UseVisualStyleBackColor = true;
            this.refreshSerialPortsButton.Click += new System.EventHandler(this.refreshSerialPortsButton_Click_1);
            // 
            // BtnSpeed
            // 
            this.BtnSpeed.Location = new System.Drawing.Point(112, 173);
            this.BtnSpeed.Name = "BtnSpeed";
            this.BtnSpeed.Size = new System.Drawing.Size(122, 53);
            this.BtnSpeed.TabIndex = 10;
            this.BtnSpeed.Text = "retrieve speed from car";
            this.BtnSpeed.UseVisualStyleBackColor = true;
            this.BtnSpeed.Click += new System.EventHandler(this.BtnSpeed_Click);
            // 
            // LblSpeed
            // 
            this.LblSpeed.AutoSize = true;
            this.LblSpeed.Location = new System.Drawing.Point(255, 190);
            this.LblSpeed.Name = "LblSpeed";
            this.LblSpeed.Size = new System.Drawing.Size(16, 17);
            this.LblSpeed.TabIndex = 11;
            this.LblSpeed.Text = "0";
            // 
            // readMessageTimer
            // 
            this.readMessageTimer.Interval = 15;
            this.readMessageTimer.Tick += new System.EventHandler(this.readMessageTimer_Tick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(112, 294);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(306, 142);
            this.textBox1.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 531);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.LblSpeed);
            this.Controls.Add(this.BtnSpeed);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.serialPortSelectionBox);
            this.Controls.Add(this.refreshSerialPortsButton);
            this.Controls.Add(this.txtMaxSpeed);
            this.Controls.Add(this.btnMaxSpeed);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMaxSpeed;
        private System.Windows.Forms.TextBox txtMaxSpeed;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.ComboBox serialPortSelectionBox;
        private System.Windows.Forms.Button refreshSerialPortsButton;
        private System.Windows.Forms.Button BtnSpeed;
        private System.Windows.Forms.Label LblSpeed;
        private System.Windows.Forms.Timer readMessageTimer;
        private System.Windows.Forms.TextBox textBox1;
    }
}

