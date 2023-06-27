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

        OneColor myLEDs = new OneColor();

        readonly int[] calLevels = { 1, 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 95, 99, 100 };
        readonly String[] ledNames = { "Red", "Green", "Blue", "Cyan" };

        public Calibration()
        {
            InitializeComponent();

            myLEDs = new OneColor();

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
            clSignalGeneration.CalibrationSignal(myLEDs);
            brightAudio.PlaySignal();

            PollJoystick.WorkerSupportsCancellation = true;
            PollJoystick.RunWorkerAsync();
        }

        private void StartCalibration_Click(object sender, EventArgs e)
        {

            Start.Enabled = false;

            // turn off LEDs
            myLEDs.BaseIntensity = 0;
            brightAudio.UpdateSignal(myLEDs);

            logBox.Clear();

            logBox.AppendText("Calibrating person: \n");
            logBox.AppendText("Filters: \n");
            logBox.AppendText("Name of ILT output file: \n\n");

            logBox.AppendText(
                $"Red: {myLEDs.RatioRED}{Environment.NewLine}" +
                $"Green: {myLEDs.RatioGREEN}{Environment.NewLine}" +
                $"Blue: {myLEDs.RatioBLUE}{Environment.NewLine}" +
                $"Cyan: {myLEDs.RatioCYAN}{Environment.NewLine}{Environment.NewLine}");

            MessageBox.Show("1. Start ILS-Meter Software.\n" +
                "2. Check that device is connected and set Display Units to W/m²\n" +
                "3. Open settings. Set Meter Factor to 1 and Recording Interval to 1s\n" +
                "4. Start Capture\n" +
                "5. Turn of monitor and 'zero' the device\n" +
                "6. Start recording.\n" +
                "7. After pressing the button, calibration will start in 20 sec.");

            // Give 20 sec for darkening the room
            Thread.Sleep(20000);

            logBox.AppendText($"Calibration started: {System.DateTime.Now}{Environment.NewLine}");
            logBox.AppendText("LED;intensity_level;start;end\n");

            myLEDs.BaseIntensity = 1.0;

            foreach (var LED in ledNames)
            {
                myLEDs.SetActiveLED = LED;

                foreach (int j in calLevels)
                {
                    clSignalGeneration.CalibrationSignal(
                        myLEDs.ActiveLED,
                        myLEDs.IntensityOuter * j / 100.0,
                        myLEDs.IntensityInner * j / 100.0
                        );
                    brightAudio.UpdateSignal();
                    Thread.Sleep(1000);
                    String start = System.DateTime.Now.ToString("HH.mm.ss.ffffff");
                    Thread.Sleep(4000);
                    String end = System.DateTime.Now.ToString("HH.mm.ss.ffffff");
                    logBox.AppendText($"{LED};{j};{start};{end}{Environment.NewLine}");
                    Thread.Sleep(1000);
                }

                myLEDs.BaseIntensity = 0;
                brightAudio.UpdateSignal(myLEDs);

                Thread.Sleep(2000);
            }

            Start.Enabled = true;

            logBox.AppendText($"Calibration ended: {System.DateTime.Now}{Environment.NewLine}");

            Thread.Sleep(5000);

            myLEDs.BaseIntensity = 0.5;
            brightAudio.UpdateSignal(myLEDs);

        }

        private void pollJoystick_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (!PollJoystick.CancellationPending)
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
            myLEDs.incrementRatio();
            brightAudio.UpdateSignal(myLEDs);

        }

        private void decRatio()
        {
            myLEDs.decrementRatio();
            brightAudio.UpdateSignal(myLEDs);
        }

        private void incLED()
        {
            myLEDs.NextLED();
            brightAudio.UpdateSignal(myLEDs);

        }

        private void decLED()
        {
            myLEDs.PreviousLED();
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
