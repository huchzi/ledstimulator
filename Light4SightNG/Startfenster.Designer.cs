namespace Light4SightNG
{
    partial class Startfenster
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Startfenster));
            this.startbutton = new System.Windows.Forms.Button();
            this.Rcff = new System.Windows.Forms.TextBox();
            this.Lcff = new System.Windows.Forms.TextBox();
            this.Mcff = new System.Windows.Forms.TextBox();
            this.sCFF = new System.Windows.Forms.TextBox();
            this.CBrod = new System.Windows.Forms.CheckBox();
            this.CBscone = new System.Windows.Forms.CheckBox();
            this.CBmcone = new System.Windows.Forms.CheckBox();
            this.CBlcone = new System.Windows.Forms.CheckBox();
            this.gbProband = new System.Windows.Forms.GroupBox();
            this.cbAugenseite = new System.Windows.Forms.ComboBox();
            this.lblAugenseite = new System.Windows.Forms.Label();
            this.tbProbandenNummer = new System.Windows.Forms.TextBox();
            this.lblProbandenNummer = new System.Windows.Forms.Label();
            this.gbProband.SuspendLayout();
            this.SuspendLayout();
            // 
            // startbutton
            // 
            this.startbutton.Location = new System.Drawing.Point(38, 267);
            this.startbutton.Name = "startbutton";
            this.startbutton.Size = new System.Drawing.Size(209, 23);
            this.startbutton.TabIndex = 0;
            this.startbutton.Text = "Start";
            this.startbutton.UseVisualStyleBackColor = true;
            this.startbutton.Click += new System.EventHandler(this.startbutton_Click);
            // 
            // Rcff
            // 
            this.Rcff.Location = new System.Drawing.Point(106, 136);
            this.Rcff.Name = "Rcff";
            this.Rcff.Size = new System.Drawing.Size(72, 20);
            this.Rcff.TabIndex = 1;
            // 
            // Lcff
            // 
            this.Lcff.Location = new System.Drawing.Point(106, 162);
            this.Lcff.Name = "Lcff";
            this.Lcff.Size = new System.Drawing.Size(72, 20);
            this.Lcff.TabIndex = 2;
            // 
            // Mcff
            // 
            this.Mcff.Location = new System.Drawing.Point(106, 188);
            this.Mcff.Name = "Mcff";
            this.Mcff.Size = new System.Drawing.Size(72, 20);
            this.Mcff.TabIndex = 3;
            // 
            // sCFF
            // 
            this.sCFF.Location = new System.Drawing.Point(106, 214);
            this.sCFF.Name = "sCFF";
            this.sCFF.Size = new System.Drawing.Size(72, 20);
            this.sCFF.TabIndex = 4;
            // 
            // CBrod
            // 
            this.CBrod.AutoSize = true;
            this.CBrod.Location = new System.Drawing.Point(38, 138);
            this.CBrod.Name = "CBrod";
            this.CBrod.Size = new System.Drawing.Size(46, 17);
            this.CBrod.TabIndex = 9;
            this.CBrod.Text = "Rod";
            this.CBrod.UseVisualStyleBackColor = true;
            // 
            // CBscone
            // 
            this.CBscone.AutoSize = true;
            this.CBscone.Location = new System.Drawing.Point(38, 216);
            this.CBscone.Name = "CBscone";
            this.CBscone.Size = new System.Drawing.Size(61, 17);
            this.CBscone.TabIndex = 10;
            this.CBscone.Text = "S-Cone";
            this.CBscone.UseVisualStyleBackColor = true;
            // 
            // CBmcone
            // 
            this.CBmcone.AutoSize = true;
            this.CBmcone.Location = new System.Drawing.Point(38, 190);
            this.CBmcone.Name = "CBmcone";
            this.CBmcone.Size = new System.Drawing.Size(63, 17);
            this.CBmcone.TabIndex = 11;
            this.CBmcone.Text = "M-Cone";
            this.CBmcone.UseVisualStyleBackColor = true;
            // 
            // CBlcone
            // 
            this.CBlcone.AutoSize = true;
            this.CBlcone.Location = new System.Drawing.Point(38, 164);
            this.CBlcone.Name = "CBlcone";
            this.CBlcone.Size = new System.Drawing.Size(60, 17);
            this.CBlcone.TabIndex = 12;
            this.CBlcone.Text = "L-Cone";
            this.CBlcone.UseVisualStyleBackColor = true;
            // 
            // gbProband
            // 
            this.gbProband.Controls.Add(this.cbAugenseite);
            this.gbProband.Controls.Add(this.lblAugenseite);
            this.gbProband.Controls.Add(this.tbProbandenNummer);
            this.gbProband.Controls.Add(this.lblProbandenNummer);
            this.gbProband.Location = new System.Drawing.Point(29, 18);
            this.gbProband.Name = "gbProband";
            this.gbProband.Size = new System.Drawing.Size(227, 96);
            this.gbProband.TabIndex = 14;
            this.gbProband.TabStop = false;
            this.gbProband.Text = "Proband";
            // 
            // cbAugenseite
            // 
            this.cbAugenseite.FormattingEnabled = true;
            this.cbAugenseite.Items.AddRange(new object[] {
            "OD",
            "OS"});
            this.cbAugenseite.Location = new System.Drawing.Point(108, 54);
            this.cbAugenseite.Name = "cbAugenseite";
            this.cbAugenseite.Size = new System.Drawing.Size(53, 21);
            this.cbAugenseite.TabIndex = 3;
            // 
            // lblAugenseite
            // 
            this.lblAugenseite.AutoSize = true;
            this.lblAugenseite.Location = new System.Drawing.Point(6, 57);
            this.lblAugenseite.Name = "lblAugenseite";
            this.lblAugenseite.Size = new System.Drawing.Size(60, 13);
            this.lblAugenseite.TabIndex = 2;
            this.lblAugenseite.Text = "Augenseite";
            // 
            // tbProbandenNummer
            // 
            this.tbProbandenNummer.Location = new System.Drawing.Point(108, 23);
            this.tbProbandenNummer.Name = "tbProbandenNummer";
            this.tbProbandenNummer.Size = new System.Drawing.Size(77, 20);
            this.tbProbandenNummer.TabIndex = 1;
            // 
            // lblProbandenNummer
            // 
            this.lblProbandenNummer.AutoSize = true;
            this.lblProbandenNummer.Location = new System.Drawing.Point(6, 26);
            this.lblProbandenNummer.Name = "lblProbandenNummer";
            this.lblProbandenNummer.Size = new System.Drawing.Size(96, 13);
            this.lblProbandenNummer.TabIndex = 0;
            this.lblProbandenNummer.Text = "Probandennummer";
            // 
            // Startfenster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 326);
            this.Controls.Add(this.gbProband);
            this.Controls.Add(this.CBlcone);
            this.Controls.Add(this.CBmcone);
            this.Controls.Add(this.CBscone);
            this.Controls.Add(this.CBrod);
            this.Controls.Add(this.sCFF);
            this.Controls.Add(this.Mcff);
            this.Controls.Add(this.Lcff);
            this.Controls.Add(this.Rcff);
            this.Controls.Add(this.startbutton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Startfenster";
            this.Text = "Measure CFF";
            this.gbProband.ResumeLayout(false);
            this.gbProband.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startbutton;
        private System.Windows.Forms.TextBox Rcff;
        private System.Windows.Forms.TextBox Lcff;
        private System.Windows.Forms.TextBox Mcff;
        private System.Windows.Forms.TextBox sCFF;
        private System.Windows.Forms.CheckBox CBrod;
        private System.Windows.Forms.CheckBox CBscone;
        private System.Windows.Forms.CheckBox CBmcone;
        private System.Windows.Forms.CheckBox CBlcone;
        private System.Windows.Forms.GroupBox gbProband;
        private System.Windows.Forms.ComboBox cbAugenseite;
        private System.Windows.Forms.Label lblAugenseite;
        private System.Windows.Forms.TextBox tbProbandenNummer;
        private System.Windows.Forms.Label lblProbandenNummer;
    }
}