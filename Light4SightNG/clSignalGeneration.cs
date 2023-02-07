using System;
namespace CalibrateLEDStimulator
{
    class clSignalGeneration
    {

        public static void CalibrationSignal(double[] LEDs)
        {
            double dValue = 0.0;
            double dWinkel = 0.0;
            double zero = 0.0;
            int k = 0;
            int i = 0;


            for (i = 0; i <= (clGlobals.AbtastFrequenz - 1); i++) //Schleife wird entsprechend der Abtastrate durchlaufen und generiert so ein Audiosignal für eine Sekunde
            {
                // fill all channels with 0 first
                for (k = 0; k < 8; k++)
                {
                    dValue = (double)((LEDs[k] * 32700) * Math.Sin(dWinkel));   //Trägerfrequenz Elongation entsprechend der übergebenen amplitude berechnen
                    WriteToWaveContainer(dValue, k, i);
                }

                dWinkel += 2 * Math.PI * clGlobals.CarrierFrequency / clGlobals.AbtastFrequenz;
                if (dWinkel > 2 * Math.PI)
                    dWinkel -= 2 * Math.PI;

            }

        }

        private static void WriteToWaveContainer(double dValue, int iChannel, int iPosition)
        {
            clGlobals.waveDaten[(((iPosition * 8) + iChannel) * 2)] = clGlobals.Lowbyte((Int16)dValue);
            clGlobals.waveDaten[(((iPosition * 8) + iChannel) * 2) + 1] = clGlobals.Highbyte((Int16)dValue);
        }

    }
}
