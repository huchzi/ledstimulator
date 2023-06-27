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
            this.logBox = new System.Windows.Forms.TextBox();
            this.PollJoystick = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Examiner = new System.Windows.Forms.ComboBox();
            this.Filters = new System.Windows.Forms.TextBox();
            this.Distance = new System.Windows.Forms.TextBox();
            this.SaveResults = new System.Windows.Forms.Button();
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
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(12, 41);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.Size = new System.Drawing.Size(481, 248);
            this.logBox.TabIndex = 1;
            // 
            // PollJoystick
            // 
            this.PollJoystick.DoWork += new System.ComponentModel.DoWorkEventHandler(this.pollJoystick_DoWork);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 303);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Examiner";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 330);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Filter";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 356);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Distance";
            // 
            // Examiner
            // 
            this.Examiner.FormattingEnabled = true;
            this.Examiner.Items.AddRange(new object[] {
            "CH",
            "AA",
            "JK"});
            this.Examiner.Location = new System.Drawing.Point(79, 300);
            this.Examiner.Name = "Examiner";
            this.Examiner.Size = new System.Drawing.Size(282, 21);
            this.Examiner.TabIndex = 5;
            // 
            // Filters
            // 
            this.Filters.Location = new System.Drawing.Point(79, 327);
            this.Filters.Name = "Filters";
            this.Filters.Size = new System.Drawing.Size(282, 20);
            this.Filters.TabIndex = 6;
            // 
            // Distance
            // 
            this.Distance.Location = new System.Drawing.Point(79, 353);
            this.Distance.Name = "Distance";
            this.Distance.Size = new System.Drawing.Size(282, 20);
            this.Distance.TabIndex = 7;
            // 
            // SaveResults
            // 
            this.SaveResults.Location = new System.Drawing.Point(15, 386);
            this.SaveResults.Name = "SaveResults";
            this.SaveResults.Size = new System.Drawing.Size(95, 23);
            this.SaveResults.TabIndex = 8;
            this.SaveResults.Text = "Save Results";
            this.SaveResults.UseVisualStyleBackColor = true;
            this.SaveResults.Click += new System.EventHandler(this.SaveResults_Click);
            // 
            // Calibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 421);
            this.Controls.Add(this.SaveResults);
            this.Controls.Add(this.Distance);
            this.Controls.Add(this.Filters);
            this.Controls.Add(this.Examiner);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox Examiner;
        private System.Windows.Forms.TextBox Filters;
        private System.Windows.Forms.TextBox Distance;
        private System.Windows.Forms.Button SaveResults;
    }
}