using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

            Messung = new Light4SightNG();
            Messung.ShowDialog();
            Messung.Dispose();

        }
    }
}
