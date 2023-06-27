namespace CalibrateLEDStimulator
{
    partial class Calibration
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
            this.Start = new System.Windows.Forms.Button();
            this.logBox = new System.Windows.Forms.TextBox();
            this.PollJoystick = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(12, 12);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(75, 23);
            this.Start.TabIndex = 0;
            this.Start.Text = "Start Calibration";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.StartCalibration_Click);
            // 
            // debugBox
            // 
            this.logBox.Location = new System.Drawing.Point(12, 41);
            this.logBox.Multiline = true;
            this.logBox.Name = "debugBox";
            this.logBox.Size = new System.Drawing.Size(481, 248);
            this.logBox.TabIndex = 1;
            // 
            // PollJoystick
            // 
            this.PollJoystick.DoWork += new System.ComponentModel.DoWorkEventHandler(this.pollJoystick_DoWork);
            // 
            // Calibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 301);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.Start);
            this.Name = "Calibration";
            this.Text = "Calibration";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Calibration_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.TextBox logBox;
        private System.ComponentModel.BackgroundWorker PollJoystick;
    }
}