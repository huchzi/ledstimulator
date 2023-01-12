using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Light4SightNG
{
    public partial class Calibration : Form
    {
        public Calibration()
        {
            InitializeComponent();
        }

        private void StartCalibration_Click(object sender, EventArgs e)
        {
            clGlobals.Kanal_1_IR = MaxSignal();
            clGlobals.Kanal_2_IG = VoidSignal();
            clGlobals.Kanal_3_IB = VoidSignal();
            clGlobals.Kanal_4_IC = VoidSignal();
            clGlobals.Kanal_5_OR = VoidSignal();
            clGlobals.Kanal_6_OG = VoidSignal();
            clGlobals.Kanal_7_OB = VoidSignal();
            clGlobals.Kanal_8_OC = VoidSignal();

            Start.Enabled = false;
            clAudioControl calAudo = new clAudioControl();
            Thread.Sleep(5000);

            for (int i = 0; i < 8; i++)
            {
                calAudo.InitWaveContainer();
                clSignalGeneration.KalibrierungsSignal(i, 1.0);
                calAudo.PlaySignal();
                Thread.Sleep(2000);
                calAudo.StopSignal();
                Thread.Sleep(2000);
                Start.Enabled = true;
            }
        }

        private static double[] VoidSignal()
        {
            double[] VoidValues = new Double[clGlobals.AbtastFrequenz];

            for (int i = 0; i <= clGlobals.AbtastFrequenz - 1; i++)
            {
                VoidValues[i] = 0.0;
            }

            return VoidValues;
        }

        private static double[] MaxSignal()
        {
            double[] MaxValues = new Double[clGlobals.AbtastFrequenz];

            for (int i = 0; i <= clGlobals.AbtastFrequenz - 1; i++)
            {
                MaxValues[i] = 1.0;
            }

            return MaxValues;
        }
    }
}
