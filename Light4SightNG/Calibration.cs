using SlimDX.DirectInput;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Light4SightNG
{
    public partial class Calibration : Form
    {
        Guid gamepad_uid;
        Joystick gamepad;
        DirectInput dinput;
        clAudioControl brightAudio;

        double[] ratios;
        int activeLED;

        double intensityOuter = 1.0;
        double intensityInner = 1.0;

        public Calibration()
        {
            InitializeComponent();

            activeLED = 0;
            ratios = new double[] { 1.0, 1.0, 1.0, 1.0};

            dinput = new DirectInput();
            foreach (DeviceInstance di in dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                debugBox.AppendText(di.ProductGuid.ToString());
                debugBox.AppendText(";");
                debugBox.AppendText(di.ProductName);
                debugBox.AppendText(";\n");

                if (di.ProductName == "Logitech Cordless RumblePad 2 USB") gamepad_uid = di.ProductGuid;
            }

            gamepad = new Joystick(dinput, gamepad_uid);
            gamepad.Acquire();

            Brightness.Value = 50;

            brightAudio = new clAudioControl();
            brightAudio.InitWaveContainer();
            clSignalGeneration.CalibrationSignal(activeLED, 1, 1);

            brightAudio.PlaySignal();

            PollJoystick.RunWorkerAsync();


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
                    clSignalGeneration.CalibrationSignal(i, j / 100.0, j / 100.0);
                    calAudio.PlaySignal();
                    Thread.Sleep(5000);
                    calAudio.StopSignal();
                    Thread.Sleep(2000);
                }
            }
            Start.Enabled = true;

        }

        private void pollJoystick_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                bool[] buttons = gamepad.GetCurrentState().GetButtons();

                // change ratio
                if (buttons[0]) Brightness.Invoke(new MethodInvoker(incRatio));
                if (buttons[2]) Brightness.Invoke(new MethodInvoker(decRatio));
                // change LED
                if (buttons[3]) Brightness.Invoke(new MethodInvoker(incLED));
                if (buttons[1]) Brightness.Invoke(new MethodInvoker(decLED));
                Thread.Sleep(200);
            }
        }

        private void incRatio()
        {
            ratios[activeLED] += .05;
            CaclulateIntensities(activeLED);
            clSignalGeneration.CalibrationSignal(activeLED, intensityOuter, intensityInner);
            brightAudio.UpdateSignal();

        }

        private void decRatio()
        {
            ratios[activeLED] -= .05;
            CaclulateIntensities(activeLED);
            clSignalGeneration.CalibrationSignal(activeLED, intensityOuter, intensityInner);
            brightAudio.UpdateSignal();
        }

        private void incLED()
        {
            activeLED += 1;
            if (activeLED == 4) activeLED = 0;
            CaclulateIntensities(activeLED);
            clSignalGeneration.CalibrationSignal(activeLED, intensityOuter, intensityInner);
            brightAudio.UpdateSignal();

        }

        private void decLED()
        {
            activeLED -= 1;
            if (activeLED == -1) activeLED = 3;
            CaclulateIntensities(activeLED);
            clSignalGeneration.CalibrationSignal(activeLED, intensityOuter, intensityInner);
            brightAudio.UpdateSignal();
        }

        private void Calibration_FormClosed(object sender, FormClosedEventArgs e)
        {
            PollJoystick.Dispose();
            Thread.Sleep(200);
            gamepad.Dispose();
            dinput = null;
            brightAudio.StopSignal();
            brightAudio.Dispose();
        }

        private void CaclulateIntensities(int LED)
        {
            intensityInner = 1;
            intensityOuter = 1 + ratios[LED];

            if (ratios[LED] > 0)
            {
                intensityInner = 1 / intensityOuter;
                intensityOuter = 1;
            } 

        }

    }
}
