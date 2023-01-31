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
        String[] ledNames;

        public Calibration()
        {
            InitializeComponent();

            activeLED = 0;
            ratios = new double[] { 0.0, 0.0, 0.0, 0.0};
            ledNames = new string[] { "Red", "Green", "Blue", "Cyan"};

            dinput = new DirectInput();

            bool foundPad = false;
            while (!foundPad)
            {
                foreach (DeviceInstance di in dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
                {
                    logBox.AppendText($"{di.ProductGuid.ToString()}\n{di.ProductName}\n\n".Replace("\n", Environment.NewLine));
                    if (di.ProductName == "Logitech Cordless RumblePad 2 USB")
                    {
                        gamepad_uid = di.ProductGuid;
                        foundPad = true;
                    }
                }

                if (!foundPad) MessageBox.Show("No Gampad detected. Please make sure that it is plugged in!");

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

            brightAudio.StopSignal();

            logBox.Clear();

            logBox.AppendText("Calibrating person: \n");
            logBox.AppendText("Filters: \n");
            logBox.AppendText("Name of ILT output file: \n\n");

            logBox.AppendText(
                $"Red: {ratios[0]}\n".Replace("\n", Environment.NewLine) +
                $"Green: {ratios[1]}\n".Replace("\n", Environment.NewLine) +
                $"Blue: {ratios[2]}\n".Replace("\n", Environment.NewLine) +
                $"Cyan: {ratios[3]}\n\n".Replace("\n", Environment.NewLine));

            MessageBox.Show("Start recording. After pressing the button, calibration will start in 20 sec.");

            // Give 5 sec for darkening the room
            Thread.Sleep(20000);

            logBox.AppendText($"Calibration started: {System.DateTime.Now.ToString()}\n".Replace("\n", Environment.NewLine));
            logBox.AppendText("LED;intensity_level;start;end\n");

            calLevels = new int[] {1, 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 95, 99, 100 };

            for (int i = 0; i < 4; i++)
            {

                brightAudio.InitWaveContainer();
                clSignalGeneration.CalibrationSignal(i, 0, 0);
                brightAudio.PlaySignal();

                foreach (int j in calLevels)
                {
                    CalculateIntensities(i, 1.0);
                    clSignalGeneration.CalibrationSignal(i, intensityOuter * j / 100.0, intensityInner * j / 100.0);
                    brightAudio.UpdateSignal();
                    Thread.Sleep(1000);
                    String start = System.DateTime.Now.ToString("HH.mm.ss.ffffff");
                    Thread.Sleep(4000);
                    logBox.AppendText($"{ledNames[i]};{j.ToString()};{start};{System.DateTime.Now.ToString("HH.mm.ss.ffffff")}\n".Replace("\n", Environment.NewLine));
                    Thread.Sleep(1000);
                }

                brightAudio.StopSignal();
                Thread.Sleep(1000);

                if (i < 3)
                {
                    MessageBox.Show($"Next LED: {ledNames[i + 1]}. Please adjust constant.");
                } else
                {
                    MessageBox.Show("Calibration finished. Please stop recording.");
                }
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
