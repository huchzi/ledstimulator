using SlimDX.DirectInput;
using System;
using System.Threading;
using System.Windows.Forms;

namespace CalibrateLEDStimulator
{
    public partial class Calibration : Form
    {
        Guid gamepad_uid;
        Joystick gamepad;
        DirectInput dinput;
        clAudioControl brightAudio;

        ColorMatch myLEDs = new ColorMatch();

        readonly int[] calLevels = { 1, 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 95, 99, 100 };
        readonly String[] ledNames = { "Red", "Green", "Blue", "Cyan" };

        public Calibration()
        {
            InitializeComponent();

            myLEDs = new ColorMatch();

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
            clSignalGeneration.CalibrationSignal(myLEDs.SettingsLED);
            brightAudio.PlaySignal();

            PollJoystick.WorkerSupportsCancellation = true;
            PollJoystick.RunWorkerAsync();
        }

        private void pollJoystick_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (!PollJoystick.CancellationPending)
            {
                bool[] buttons = gamepad.GetCurrentState().GetButtons();

                // change Red/Cyan
                if (buttons[0]) this.Invoke(new MethodInvoker(incRatio));
                if (buttons[2]) this.Invoke(new MethodInvoker(decRatio));
                // change Green/Blue
                if (buttons[3]) this.Invoke(new MethodInvoker(incLED));
                if (buttons[1]) this.Invoke(new MethodInvoker(decLED));
                // change Luminances
                if (buttons[5]) this.Invoke(new MethodInvoker(incLum));
                if (buttons[4]) this.Invoke(new MethodInvoker(decLum));
                // get values
                if (buttons[6]) this.Invoke(new MethodInvoker(getValues));
                Thread.Sleep(200);
            }
        }

        private void getValues()
        {
            logBox.AppendText($"Red to Cyan: {myLEDs.RatioRedCyan}{Environment.NewLine}Green/Blue: {myLEDs.RatioGreenBlue}{Environment.NewLine}Intensities: {myLEDs.RatioLuminance}{Environment.NewLine}");
        }

        private void incLum()
        {
            myLEDs.incrementRedCyan();
            brightAudio.UpdateSignal(myLEDs);
        }

        private void decLum()
        {
            myLEDs.incrementGreenBlue();
            brightAudio.UpdateSignal(myLEDs);
        }

        private void incRatio()
        {
            myLEDs.incrementRed();
            brightAudio.UpdateSignal(myLEDs);

        }

        private void decRatio()
        {
            myLEDs.incrementCyan();
            brightAudio.UpdateSignal(myLEDs);
        }

        private void incLED()
        {
            myLEDs.incrementGreen();
            brightAudio.UpdateSignal(myLEDs);

        }

        private void decLED()
        {
            myLEDs.incrementBlue();
            brightAudio.UpdateSignal(myLEDs);
        }

        private void Calibration_FormClosed(object sender, FormClosedEventArgs e)
        {
            PollJoystick.CancelAsync();
            Thread.Sleep(1000);
            PollJoystick.Dispose();
            Thread.Sleep(500);
            gamepad.Dispose();
            dinput = null;
            brightAudio.StopSignal();
            Thread.Sleep(500);
            brightAudio.Dispose();
        }

    }
}
