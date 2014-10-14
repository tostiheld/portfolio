namespace ArduinoController
{
    partial class ControllerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TurnOnLed = new System.Windows.Forms.Button();
            this.TurnOffLed = new System.Windows.Forms.Button();
            this.Read = new System.Windows.Forms.Button();
            this.towerstate = new System.Windows.Forms.Label();
            this.reset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TurnOnLed
            // 
            this.TurnOnLed.Location = new System.Drawing.Point(13, 13);
            this.TurnOnLed.Name = "TurnOnLed";
            this.TurnOnLed.Size = new System.Drawing.Size(88, 23);
            this.TurnOnLed.TabIndex = 0;
            this.TurnOnLed.Text = "Turn on LED1";
            this.TurnOnLed.UseVisualStyleBackColor = true;
            this.TurnOnLed.Click += new System.EventHandler(this.button1_Click);
            // 
            // TurnOffLed
            // 
            this.TurnOffLed.Location = new System.Drawing.Point(13, 43);
            this.TurnOffLed.Name = "TurnOffLed";
            this.TurnOffLed.Size = new System.Drawing.Size(88, 23);
            this.TurnOffLed.TabIndex = 1;
            this.TurnOffLed.Text = "Turn off LED1";
            this.TurnOffLed.UseVisualStyleBackColor = true;
            this.TurnOffLed.Click += new System.EventHandler(this.button2_Click);
            // 
            // Read
            // 
            this.Read.Location = new System.Drawing.Point(13, 73);
            this.Read.Name = "Read";
            this.Read.Size = new System.Drawing.Size(88, 23);
            this.Read.TabIndex = 2;
            this.Read.Text = "Read towers";
            this.Read.UseVisualStyleBackColor = true;
            this.Read.Click += new System.EventHandler(this.Read_Click);
            // 
            // towerstate
            // 
            this.towerstate.AutoSize = true;
            this.towerstate.Location = new System.Drawing.Point(107, 78);
            this.towerstate.Name = "towerstate";
            this.towerstate.Size = new System.Drawing.Size(65, 13);
            this.towerstate.TabIndex = 3;
            this.towerstate.Text = "Tower State";
            // 
            // reset
            // 
            this.reset.Location = new System.Drawing.Point(13, 103);
            this.reset.Name = "reset";
            this.reset.Size = new System.Drawing.Size(88, 23);
            this.reset.TabIndex = 4;
            this.reset.Text = "Reset";
            this.reset.UseVisualStyleBackColor = true;
            this.reset.Click += new System.EventHandler(this.reset_Click);
            // 
            // ControllerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 275);
            this.Controls.Add(this.reset);
            this.Controls.Add(this.towerstate);
            this.Controls.Add(this.Read);
            this.Controls.Add(this.TurnOffLed);
            this.Controls.Add(this.TurnOnLed);
            this.Name = "ControllerForm";
            this.Text = "Arduino Controller";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button TurnOnLed;
        private System.Windows.Forms.Button TurnOffLed;
        private System.Windows.Forms.Button Read;
        private System.Windows.Forms.Label towerstate;
        private System.Windows.Forms.Button reset;


    }
}

