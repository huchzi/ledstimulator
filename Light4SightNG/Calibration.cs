using System;
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

            Start.Enabled = false;

            // Give 5 sec for darkening the room
            Thread.Sleep(5000);

            clAudioControl calAudio = new clAudioControl();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j <= 100; j += 10)
                {
                    calAudio.InitWaveContainer();
                    clSignalGeneration.CalibrationSignal(i, j / 100.0);
                    calAudio.PlaySignal();
                    Thread.Sleep(5000);
                    calAudio.StopSignal();
                    Thread.Sleep(2000);
                }
            }
            Start.Enabled = true;

        }

    }
}
