using SlimDX.XAudio2;
using System;
using System.IO;
using System.Threading;


namespace CalibrateLEDStimulator
{
    internal class clAudioControl : IDisposable
    {
        private long cbWaveSize;

        private SlimDX.Multimedia.WaveFormat SignalFormat = null;
        private static XAudio2 AudioDevice = new XAudio2();
        private MasteringVoice WaveMasterVoice = new MasteringVoice(AudioDevice);
        private AudioBuffer WaveBuffer = null;
        private MemoryStream WaveMemStream = null;
        private SourceVoice WaveSourceVoice = null;
        private Thread m_soundThread = null;
        private ThreadStart soundThreadStart = null;

        public clAudioControl()
        {
            SignalFormat = new SlimDX.Multimedia.WaveFormat();
            SignalFormat.AverageBytesPerSecond = clGlobals.BytesProSekunde;
            SignalFormat.BlockAlignment = clGlobals.Blockausrichtung;
            SignalFormat.Channels = clGlobals.AnzahlKanaele;
            SignalFormat.SamplesPerSecond = clGlobals.AbtastFrequenz;
            SignalFormat.BitsPerSample = clGlobals.SampleContainerGroesse;
            SignalFormat.FormatTag = SlimDX.Multimedia.WaveFormatTag.Pcm;
            cbWaveSize = (long)(clGlobals.SampleLaenge * SignalFormat.Channels * SignalFormat.SamplesPerSecond * SignalFormat.BitsPerSample / 8);
        }

        public bool InitWaveContainer()
        {
            try
            {
                clGlobals.waveDaten = new byte[cbWaveSize];
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool PlaySignal()
        {
            soundThreadStart = new ThreadStart(SoundPlayerThread);
            m_soundThread = new Thread(soundThreadStart);
            m_soundThread.Start();

            return true;
        }

        private void SoundPlayerThread()
        {
            clGlobals.bPlaySignal = true;

            WaveMemStream = new MemoryStream(clGlobals.waveDaten);

            WaveBuffer = new AudioBuffer();
            WaveBuffer.Flags = BufferFlags.EndOfStream;
            WaveBuffer.AudioData = WaveMemStream;
            WaveBuffer.AudioBytes = clGlobals.BytesProSekunde;
            WaveBuffer.LoopCount = XAudio2.LoopInfinite;
            // WaveBuffer.LoopCount = 1;

            WaveSourceVoice = new SourceVoice(AudioDevice, SignalFormat);
            WaveSourceVoice.SubmitSourceBuffer(WaveBuffer);

            WaveSourceVoice.Start();

            while (clGlobals.bPlaySignal)
            {
                Thread.Sleep(10);
            }

            WaveSourceVoice.Stop();
            Thread.Sleep(10);
            WaveMemStream.Close();
            WaveMemStream.Dispose();
            WaveBuffer.Dispose();
            WaveSourceVoice.Dispose();
            Thread.Sleep(100);
            soundThreadStart = null;
            clGlobals.waveDaten = null;
            this.m_soundThread.Abort();
        }

        public void StopSignal()
        {
            if (clGlobals.bPlaySignal == true)
            {
                clGlobals.bPlaySignal = false;
            }
        }

        public void UpdateSignal()
        {
            WaveSourceVoice.Stop();

            WaveMemStream = new MemoryStream(clGlobals.waveDaten);

            WaveBuffer = new AudioBuffer();
            WaveBuffer.Flags = BufferFlags.EndOfStream;
            WaveBuffer.AudioData = WaveMemStream;
            WaveBuffer.AudioBytes = clGlobals.BytesProSekunde;
            WaveBuffer.LoopCount = XAudio2.LoopInfinite;

            WaveSourceVoice = new SourceVoice(AudioDevice, SignalFormat);
            WaveSourceVoice.SubmitSourceBuffer(WaveBuffer);

            WaveSourceVoice.Start();
        }

        public void UpdateSignal(ColorMatch LEDs)
        {
            clSignalGeneration.CalibrationSignal(LEDs.SettingsLED);
            this.UpdateSignal();
        }

        #region IDisposable Member

        public void Dispose()
        {
            this.StopSignal();

            WaveMasterVoice.Dispose();


            // AudioDevice.Dispose();
        }

        #endregion
    }


}
