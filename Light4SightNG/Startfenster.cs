using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Light4SightNG
{
    public partial class Startfenster : Form
    {
        private static Light4SightNG Messung;
        
        public Startfenster()
        {
            InitializeComponent();
        }

        private void startbutton_Click(object sender, EventArgs e)
        {
            if (CBrod.Checked)
            {
                Messung = new Light4SightNG();
                Messung.testeCFF = true;
                Messung.SetDesktopLocation(this.Location.X + this.Size.Width, this.Location.Y);
                Light4SightNG.cff = -999;
                Messung.ShowDialog(tbProbandenNummer.Text, cbAugenseite.Text, @"presets\rod.pre");
                Rcff.Text = Convert.ToString(Light4SightNG.cff);
                Messung.Dispose();
                Messung = null;
                Thread.Sleep(100);
            }
            if (CBlcone.Checked)
            {
                Messung = new Light4SightNG();
                Messung.testeCFF = true;
                Messung.SetDesktopLocation(this.Location.X + this.Size.Width, this.Location.Y);
                Light4SightNG.cff = -999;
                Messung.ShowDialog(tbProbandenNummer.Text, cbAugenseite.Text, @"presets\lcone.pre");
                Lcff.Text = Convert.ToString(Light4SightNG.cff);
                Messung.Dispose();
                Messung = null;
                Thread.Sleep(100);
            }
            if (CBmcone.Checked)
            {
                Messung = new Light4SightNG();
                Messung.testeCFF = true;
                Messung.SetDesktopLocation(this.Location.X + this.Size.Width, this.Location.Y);
                Light4SightNG.cff = -999;
                Messung.ShowDialog(tbProbandenNummer.Text, cbAugenseite.Text, @"presets\mcone.pre");
                Mcff.Text = Convert.ToString(Light4SightNG.cff);
                Messung.Dispose();
                Messung = null;
                Thread.Sleep(100);
            }
            if (CBscone.Checked)
            {
                Messung = new Light4SightNG();
                Messung.testeCFF = true;
                Messung.SetDesktopLocation(this.Location.X + this.Size.Width, this.Location.Y);
                Light4SightNG.cff = -999;
                Messung.ShowDialog(tbProbandenNummer.Text, cbAugenseite.Text, @"presets\scone.pre");
                sCFF.Text = Convert.ToString(Light4SightNG.cff);
                Messung.Dispose();
                Messung = null;
                Thread.Sleep(100);
            }
            this.Focus();
            this.Show();
        }
    }
}
