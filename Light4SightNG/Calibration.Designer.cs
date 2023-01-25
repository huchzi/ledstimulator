namespace Light4SightNG
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
            this.debugBox = new System.Windows.Forms.TextBox();
            this.Brightness = new System.Windows.Forms.NumericUpDown();
            this.PollJoystick = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.Brightness)).BeginInit();
            this.SuspendLayout();
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(89, 24);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(75, 23);
            this.Start.TabIndex = 0;
            this.Start.Text = "Start Calibration";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.StartCalibration_Click);
            // 
            // debugBox
            // 
            this.debugBox.Location = new System.Drawing.Point(55, 53);
            this.debugBox.Multiline = true;
            this.debugBox.Name = "debugBox";
            this.debugBox.Size = new System.Drawing.Size(146, 105);
            this.debugBox.TabIndex = 1;
            // 
            // Brightness
            // 
            this.Brightness.Location = new System.Drawing.Point(69, 176);
            this.Brightness.Name = "Brightness";
            this.Brightness.Size = new System.Drawing.Size(120, 20);
            this.Brightness.TabIndex = 2;
            // 
            // PollJoystick
            // 
            this.PollJoystick.DoWork += new System.ComponentModel.DoWorkEventHandler(this.pollJoystick_DoWork);
            // 
            // Calibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 301);
            this.Controls.Add(this.Brightness);
            this.Controls.Add(this.debugBox);
            this.Controls.Add(this.Start);
            this.Name = "Calibration";
            this.Text = "Calibration";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Calibration_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.Brightness)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.TextBox debugBox;
        private System.Windows.Forms.NumericUpDown Brightness;
        private System.ComponentModel.BackgroundWorker PollJoystick;
    }
}