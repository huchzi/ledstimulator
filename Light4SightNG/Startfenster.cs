using System;
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
