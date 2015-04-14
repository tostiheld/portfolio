namespace Domotica
{
    partial class RoadDashMainForm
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
            this.tbSpeed = new System.Windows.Forms.TextBox();
            this.tbAllowedSpeed = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbNewWarning = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnNewWarning = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbSpeed
            // 
            this.tbSpeed.Location = new System.Drawing.Point(127, 70);
            this.tbSpeed.Name = "tbSpeed";
            this.tbSpeed.Size = new System.Drawing.Size(100, 20);
            this.tbSpeed.TabIndex = 0;
            // 
            // tbAllowedSpeed
            // 
            this.tbAllowedSpeed.Location = new System.Drawing.Point(127, 106);
            this.tbAllowedSpeed.Name = "tbAllowedSpeed";
            this.tbAllowedSpeed.Size = new System.Drawing.Size(100, 20);
            this.tbAllowedSpeed.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "currentSpeed";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "speedAllowed";
            // 
            // tbNewWarning
            // 
            this.tbNewWarning.Location = new System.Drawing.Point(127, 169);
            this.tbNewWarning.Name = "tbNewWarning";
            this.tbNewWarning.Size = new System.Drawing.Size(230, 20);
            this.tbNewWarning.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Send a Warning";
            // 
            // btnNewWarning
            // 
            this.btnNewWarning.Location = new System.Drawing.Point(363, 167);
            this.btnNewWarning.Name = "btnNewWarning";
            this.btnNewWarning.Size = new System.Drawing.Size(75, 23);
            this.btnNewWarning.TabIndex = 6;
            this.btnNewWarning.Text = "send";
            this.btnNewWarning.UseVisualStyleBackColor = true;
            this.btnNewWarning.Click += new System.EventHandler(this.btnNewWarning_Click);
            // 
            // RoadDashMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 245);
            this.Controls.Add(this.btnNewWarning);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbNewWarning);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbAllowedSpeed);
            this.Controls.Add(this.tbSpeed);
            this.Name = "RoadDashMainForm";
            this.Text = "Domotica";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DomoticaMainForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbSpeed;
        private System.Windows.Forms.TextBox tbAllowedSpeed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbNewWarning;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnNewWarning;

    }
}

