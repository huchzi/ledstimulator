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

        public Calibration()
        {
            InitializeComponent();
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
            clSignalGeneration.CalibrationSignal(1, (double)Brightness.Value / 100.0);

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
                    clSignalGeneration.CalibrationSignal(i, j / 100.0);
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
                if (gamepad.GetCurrentState().GetButtons()[3]) Brightness.Invoke(new MethodInvoker(incNumeric));
                if (gamepad.GetCurrentState().GetButtons()[1]) Brightness.Invoke(new MethodInvoker(decNumeric));
                Thread.Sleep(200);
            }
        }

        private void incNumeric()
        {
            if (Brightness.Value <= 90) Brightness.Value += 10;
            clSignalGeneration.CalibrationSignal(1, (double)Brightness.Value / 100.0);
            brightAudio.UpdateSignal();

        }

        private void decNumeric()
        {
            if (Brightness.Value >= 10) Brightness.Value -= 10;
            clSignalGeneration.CalibrationSignal(1, (double)Brightness.Value / 100.0);
            brightAudio.UpdateSignal();
        }

        private void Calibration_FormClosed(object sender, FormClosedEventArgs e)
        {
            PollJoystick.Dispose();
            gamepad.Dispose();
            dinput = null;
            brightAudio.StopSignal();
            brightAudio.Dispose();
        }
    }
}
