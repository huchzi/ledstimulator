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

        double intensityOuter = 0.5;
        double intensityInner = 0.5;

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
            CalculateIntensities(activeLED);
            clSignalGeneration.CalibrationSignal(activeLED, intensityOuter, intensityInner);

            brightAudio.PlaySignal();

            PollJoystick.RunWorkerAsync();


        }

        private void StartCalibration_Click(object sender, EventArgs e)
        {

            Start.Enabled = false;

            debugBox.Clear();

            foreach (double d in ratios)
            {
                debugBox.AppendText(d.ToString());
                debugBox.AppendText("\n");
            }

            brightAudio.StopSignal();

            // Give 5 sec for darkening the room
            Thread.Sleep(5000);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j <= 100; j += 10)
                {
                    brightAudio.InitWaveContainer();
                    CalculateIntensities(i);
                    clSignalGeneration.CalibrationSignal(i, intensityOuter * j / 100.0, intensityInner * j / 100.0);
                    brightAudio.PlaySignal();
                    Thread.Sleep(5000);
                    brightAudio.StopSignal();
                    Thread.Sleep(2000);
                }
            }
            Start.Enabled = true;

            brightAudio.InitWaveContainer();
            CalculateIntensities(activeLED);
            clSignalGeneration.CalibrationSignal(activeLED, intensityOuter, intensityInner);

            brightAudio.PlaySignal();

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
            ratios[activeLED] += .025;
            CalculateIntensities(activeLED);
            clSignalGeneration.CalibrationSignal(activeLED, intensityOuter, intensityInner);
            brightAudio.UpdateSignal();

        }

        private void decRatio()
        {
            ratios[activeLED] -= .025;
            CalculateIntensities(activeLED);
            clSignalGeneration.CalibrationSignal(activeLED, intensityOuter, intensityInner);
            brightAudio.UpdateSignal();
        }

        private void incLED()
        {
            activeLED += 1;
            if (activeLED == 4) activeLED = 0;
            CalculateIntensities(activeLED);
            clSignalGeneration.CalibrationSignal(activeLED, intensityOuter, intensityInner);
            brightAudio.UpdateSignal();

        }

        private void decLED()
        {
            activeLED -= 1;
            if (activeLED == -1) activeLED = 3;
            CalculateIntensities(activeLED);
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

        private void CalculateIntensities(int LED)
        {
            intensityInner = 0.5;
            intensityOuter = 0.5 + ratios[LED];

            if (ratios[LED] > 0)
            {
                intensityInner = 0.5 / intensityOuter;
                intensityOuter = 0.5;
            } 

        }

    }
}
