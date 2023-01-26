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

        int[] calLevels;

        public Calibration()
        {
            InitializeComponent();

            activeLED = 0;
            ratios = new double[] { 1.0, 1.0, 1.0, 1.0};

            dinput = new DirectInput();
            foreach (DeviceInstance di in dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                logBox.AppendText($"{di.ProductGuid.ToString()}\n{di.ProductName}\n\n".Replace("\n", Environment.NewLine));
                if (di.ProductName == "Logitech Cordless RumblePad 2 USB") gamepad_uid = di.ProductGuid;
            }

            gamepad = new Joystick(dinput, gamepad_uid);
            gamepad.Acquire();

            brightAudio = new clAudioControl();
            brightAudio.InitWaveContainer();
            CalculateIntensities(activeLED, 0.5);
            clSignalGeneration.CalibrationSignal(activeLED, intensityOuter, intensityInner);

            brightAudio.PlaySignal();

            PollJoystick.RunWorkerAsync();


        }

        private void StartCalibration_Click(object sender, EventArgs e)
        {

            Start.Enabled = false;

            logBox.Clear();

            MessageBox.Show("You have 5s to turn off any light.");

            logBox.AppendText($"Red: {ratios[0]}\nGreen: {ratios[1]}\nBlue: {ratios[2]}\nCyan: {ratios[3]}\n\n".Replace("\n", Environment.NewLine));

            brightAudio.StopSignal();

            // Give 5 sec for darkening the room
            Thread.Sleep(5000);

            logBox.AppendText($"Calibration started: {System.DateTime.Now.ToString()}\n".Replace("\n", Environment.NewLine));

            calLevels = new int[] {1, 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 95, 99, 100 };

            for (int i = 0; i < 4; i++)
            {

                brightAudio.InitWaveContainer();
                clSignalGeneration.CalibrationSignal(i, 0, 0);
                brightAudio.PlaySignal();

                foreach(int j in calLevels)
                {
                    CalculateIntensities(i, 1.0);
                    clSignalGeneration.CalibrationSignal(i, intensityOuter * j / 100.0, intensityInner * j / 100.0);
                    brightAudio.UpdateSignal();
                    Thread.Sleep(6000);
                }

                brightAudio.StopSignal();
                Thread.Sleep(2000);
            }
            Start.Enabled = true;

            logBox.AppendText($"Calibration ended: {System.DateTime.Now.ToString()}\n".Replace("\n", Environment.NewLine));

            Thread.Sleep(5000);

            brightAudio.InitWaveContainer();
            CalculateIntensities(activeLED, 0.5);
            clSignalGeneration.CalibrationSignal(activeLED, intensityOuter, intensityInner);

            brightAudio.PlaySignal();

        }

        private void pollJoystick_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                bool[] buttons = gamepad.GetCurrentState().GetButtons();

                // change ratio
                if (buttons[0]) this.Invoke(new MethodInvoker(incRatio));
                if (buttons[2]) this.Invoke(new MethodInvoker(decRatio));
                // change LED
                if (buttons[3]) this.Invoke(new MethodInvoker(incLED));
                if (buttons[1]) this.Invoke(new MethodInvoker(decLED));
                Thread.Sleep(200);
            }
        }

        private void incRatio()
        {
            ratios[activeLED] += .025;
            CalculateIntensities(activeLED, 0.5);
            clSignalGeneration.CalibrationSignal(activeLED, intensityOuter, intensityInner);
            brightAudio.UpdateSignal();

        }

        private void decRatio()
        {
            ratios[activeLED] -= .025;
            CalculateIntensities(activeLED, 0.5);
            clSignalGeneration.CalibrationSignal(activeLED, intensityOuter, intensityInner);
            brightAudio.UpdateSignal();
        }

        private void incLED()
        {
            activeLED += 1;
            if (activeLED == 4) activeLED = 0;
            CalculateIntensities(activeLED, 0.5);
            clSignalGeneration.CalibrationSignal(activeLED, intensityOuter, intensityInner);
            brightAudio.UpdateSignal();

        }

        private void decLED()
        {
            activeLED -= 1;
            if (activeLED == -1) activeLED = 3;
            CalculateIntensities(activeLED, 0.5);
            clSignalGeneration.CalibrationSignal(activeLED, intensityOuter, intensityInner);
            brightAudio.UpdateSignal();
        }

        private void Calibration_FormClosed(object sender, FormClosedEventArgs e)
        {
            PollJoystick.CancelAsync();
            PollJoystick.Dispose();
            Thread.Sleep(500);
            gamepad.Dispose();
            dinput = null;
            brightAudio.StopSignal();
            brightAudio.Dispose();
        }

        private void CalculateIntensities(int LED, double baseIntensity)
        {
            intensityInner = baseIntensity;
            intensityOuter = baseIntensity * (1 + ratios[LED]);

            if (ratios[LED] > 0)
            {
                intensityInner = baseIntensity / (1 + ratios[LED]);
                intensityOuter = baseIntensity;
            } 

        }

    }
}
