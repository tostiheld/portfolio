namespace ArduinoRead
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
            this.Arduino = new System.IO.Ports.SerialPort(this.components);
            this.btnSonarOn = new System.Windows.Forms.Button();
            this.btnSonarOff = new System.Windows.Forms.Button();
            this.btnGetDensity = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.btnSendSpeed = new System.Windows.Forms.Button();
            this.nudSpeed = new System.Windows.Forms.NumericUpDown();
            this.btnDisplayOff = new System.Windows.Forms.Button();
            this.btnWarningOn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpeed)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSonarOn
            // 
            this.btnSonarOn.Location = new System.Drawing.Point(21, 22);
            this.btnSonarOn.Name = "btnSonarOn";
            this.btnSonarOn.Size = new System.Drawing.Size(75, 33);
            this.btnSonarOn.TabIndex = 1;
            this.btnSonarOn.Text = "Sonar ON";
            this.btnSonarOn.UseVisualStyleBackColor = true;
            this.btnSonarOn.Click += new System.EventHandler(this.btnSonarOn_Click);
            // 
            // btnSonarOff
            // 
            this.btnSonarOff.Location = new System.Drawing.Point(21, 64);
            this.btnSonarOff.Name = "btnSonarOff";
            this.btnSonarOff.Size = new System.Drawing.Size(75, 33);
            this.btnSonarOff.TabIndex = 2;
            this.btnSonarOff.Text = "Sonar Off";
            this.btnSonarOff.UseVisualStyleBackColor = true;
            this.btnSonarOff.Click += new System.EventHandler(this.btnSonarOff_Click);
            // 
            // btnGetDensity
            // 
            this.btnGetDensity.Location = new System.Drawing.Point(102, 22);
            this.btnGetDensity.Name = "btnGetDensity";
            this.btnGetDensity.Size = new System.Drawing.Size(84, 33);
            this.btnGetDensity.TabIndex = 3;
            this.btnGetDensity.Text = "Get Density";
            this.btnGetDensity.UseVisualStyleBackColor = true;
            this.btnGetDensity.Click += new System.EventHandler(this.btnGetDensity_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // btnSendSpeed
            // 
            this.btnSendSpeed.Location = new System.Drawing.Point(87, 16);
            this.btnSendSpeed.Name = "btnSendSpeed";
            this.btnSendSpeed.Size = new System.Drawing.Size(99, 23);
            this.btnSendSpeed.TabIndex = 4;
            this.btnSendSpeed.Text = "Send To Display";
            this.btnSendSpeed.UseVisualStyleBackColor = true;
            this.btnSendSpeed.Click += new System.EventHandler(this.btnSendSpeed_Click);
            // 
            // nudSpeed
            // 
            this.nudSpeed.Location = new System.Drawing.Point(6, 19);
            this.nudSpeed.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudSpeed.Name = "nudSpeed";
            this.nudSpeed.Size = new System.Drawing.Size(75, 20);
            this.nudSpeed.TabIndex = 5;
            // 
            // btnDisplayOff
            // 
            this.btnDisplayOff.Location = new System.Drawing.Point(87, 45);
            this.btnDisplayOff.Name = "btnDisplayOff";
            this.btnDisplayOff.Size = new System.Drawing.Size(99, 36);
            this.btnDisplayOff.TabIndex = 6;
            this.btnDisplayOff.Text = "Display Off";
            this.btnDisplayOff.UseVisualStyleBackColor = true;
            this.btnDisplayOff.Click += new System.EventHandler(this.btnDisplayOff_Click);
            // 
            // btnWarningOn
            // 
            this.btnWarningOn.Location = new System.Drawing.Point(6, 45);
            this.btnWarningOn.Name = "btnWarningOn";
            this.btnWarningOn.Size = new System.Drawing.Size(75, 36);
            this.btnWarningOn.TabIndex = 7;
            this.btnWarningOn.Text = "Warning ON/OFF";
            this.btnWarningOn.UseVisualStyleBackColor = true;
            this.btnWarningOn.Click += new System.EventHandler(this.btnWarningOn_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 40);
            this.button1.TabIndex = 8;
            this.button1.Text = "Get Temperature";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudSpeed);
            this.groupBox1.Controls.Add(this.btnSendSpeed);
            this.groupBox1.Controls.Add(this.btnWarningOn);
            this.groupBox1.Controls.Add(this.btnDisplayOff);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(234, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 100);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Speed Limit";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSonarOn);
            this.groupBox2.Controls.Add(this.btnSonarOff);
            this.groupBox2.Controls.Add(this.btnGetDensity);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(13, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(215, 108);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sonar";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new System.Drawing.Point(234, 118);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(215, 108);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Temperature";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnDisconnect);
            this.groupBox4.Controls.Add(this.btnConnect);
            this.groupBox4.Location = new System.Drawing.Point(13, 13);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(215, 99);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "groupBox4";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Location = new System.Drawing.Point(6, 48);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 1;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(6, 19);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 261);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "ArduinoControl";
            ((System.ComponentModel.ISupportInitialize)(this.nudSpeed)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.Ports.SerialPort Arduino;
        private System.Windows.Forms.Button btnSonarOn;
        private System.Windows.Forms.Button btnSonarOff;
        private System.Windows.Forms.Button btnGetDensity;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button btnSendSpeed;
        private System.Windows.Forms.NumericUpDown nudSpeed;
        private System.Windows.Forms.Button btnDisplayOff;
        private System.Windows.Forms.Button btnWarningOn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
    }
}

