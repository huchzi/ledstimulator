using System;

namespace Light4SightNG
{
    class clTestChannels
    {
        public static void CreateTestChannelArrays()
        {
            clGlobals.Kanal_1_IR = new double[clGlobals.AbtastFrequenz];
            clGlobals.Kanal_2_IG = new double[clGlobals.AbtastFrequenz];
            clGlobals.Kanal_3_IB = new double[clGlobals.AbtastFrequenz];
            clGlobals.Kanal_4_IC = new double[clGlobals.AbtastFrequenz];
            clGlobals.Kanal_5_OR = new double[clGlobals.AbtastFrequenz];
            clGlobals.Kanal_6_OG = new double[clGlobals.AbtastFrequenz];
            clGlobals.Kanal_7_OB = new double[clGlobals.AbtastFrequenz];
            clGlobals.Kanal_8_OC = new double[clGlobals.AbtastFrequenz];
        }

        public static void WriteToWaveTestContainer(double dValue, int iChannel, int iPosition)
        {
            clGlobals.waveDaten[(((iPosition * 8) + iChannel) * 2)] = clGlobals.Lowbyte((Int16)dValue);
            clGlobals.waveDaten[(((iPosition * 8) + iChannel) * 2) + 1] = clGlobals.Highbyte((Int16)dValue);
        }

        public static void ConcatTestChannels()
        {
            for (int i = 0; i <= clGlobals.AbtastFrequenz - 1; i++)
            {
                WriteToWaveTestContainer(clGlobals.Kanal_1_IR[i], 0, i);
                WriteToWaveTestContainer(clGlobals.Kanal_2_IG[i], 1, i);
                WriteToWaveTestContainer(clGlobals.Kanal_3_IB[i], 2, i);
                WriteToWaveTestContainer(clGlobals.Kanal_4_IC[i], 3, i);
                WriteToWaveTestContainer(clGlobals.Kanal_5_OR[i], 4, i);
                WriteToWaveTestContainer(clGlobals.Kanal_6_OG[i], 5, i);
                WriteToWaveTestContainer(clGlobals.Kanal_7_OB[i], 6, i);
                WriteToWaveTestContainer(clGlobals.Kanal_8_OC[i], 7, i);
            }
        }

        public static void TestSinus(double MHLR, double KLR, int Frequenz, int Phasenwinkel)
        {
            double[] TempSinus = new Double[clGlobals.AbtastFrequenz];
            double dWinkel = 0.0;

            for (int i = 0; i <= clGlobals.AbtastFrequenz - 1; i++)
            {
                TempSinus[i] = (Int16)((double)(((MHLR + KLR * Math.Sin(Frequenz * (clGlobals.DeltaPI * i) + clGlobals.DeltaPhiSinus * Phasenwinkel))) * Math.Sin(dWinkel)));

                dWinkel += 2 * Math.PI * clGlobals.TraegerFrequenz / clGlobals.AbtastFrequenz;
                if (dWinkel > 2 * Math.PI)
                    dWinkel -= 2 * Math.PI;
            }

            clGlobals.Kanal_1_IR = TempSinus;
        }

        public static void TestTraeger()
        {
            double[] TempSinus = new Double[clGlobals.AbtastFrequenz];
            double dWinkel = 0.0;

            for (int i = 0; i <= clGlobals.AbtastFrequenz - 1; i++)
            {
                TempSinus[i] = (Int16)(32767 * Math.Sin(dWinkel));  //Trägerfrequenz Elongation entsprechend der übergebenen amplitude berechnen
                dWinkel += 2 * Math.PI * clGlobals.TraegerFrequenz / clGlobals.AbtastFrequenz;
                if (dWinkel > 2 * Math.PI)
                    dWinkel -= 2 * Math.PI;

            }

            clGlobals.Kanal_1_IR = TempSinus;
        }

        /* public static void Untersuchungssignal()
         {
             #region IR Kanal
             if (clGlobals.IRChannel.SignalAktiv)
             {
                 switch (clGlobals.IRChannel.Signalform)
                 {
                     case "Sinus":
                         {
                             clGlobals.Kanal_1_IR = Sinus(IR_MHLR, IR_KLR, clGlobals.IRChannel.Frequenz, clGlobals.IRChannel.Phasenverschiebung);
                             break;
                         }
                     case "Rechteck":
                         {
                             clGlobals.Kanal_1_IR = Rechteck(IR_MHLR, IR_KLR, clGlobals.IRChannel.Frequenz, clGlobals.IRChannel.Phasenverschiebung);
                             break;
                         }
                     case "On-Ramp":
                         {
                             clGlobals.Kanal_1_IR = OnRamp(IR_MHLR, IR_KLR, clGlobals.IRChannel.Frequenz, clGlobals.IRChannel.Phasenverschiebung);
                             break;
                         }
                     case "Off-Ramp":
                         {
                             clGlobals.Kanal_1_IR = OffRamp(IR_MHLR, IR_KLR, clGlobals.IRChannel.Frequenz, clGlobals.IRChannel.Phasenverschiebung);
                             break;
                         }
                 }
             }

             #endregion

             #region IG Kanal
             if (clGlobals.IGChannel.SignalAktiv)
             {
                 switch (clGlobals.IGChannel.Signalform)
                 {
                     case "Sinus":
                         {
                             clGlobals.Kanal_2_IG = Sinus(IG_MHLR, IG_KLR, clGlobals.IGChannel.Frequenz, clGlobals.IGChannel.Phasenverschiebung);
                             break;
                         }
                     case "Rechteck":
                         {
                             clGlobals.Kanal_2_IG = Rechteck(IG_MHLR, IG_KLR, clGlobals.IGChannel.Frequenz, clGlobals.IGChannel.Phasenverschiebung);
                             break;
                         }
                     case "On-Ramp":
                         {
                             clGlobals.Kanal_2_IG = OnRamp(IG_MHLR, IG_KLR, clGlobals.IGChannel.Frequenz, clGlobals.IGChannel.Phasenverschiebung);
                             break;
                         }
                     case "Off-Ramp":
                         {
                             clGlobals.Kanal_2_IG = OffRamp(IG_MHLR, IG_KLR, clGlobals.IGChannel.Frequenz, clGlobals.IGChannel.Phasenverschiebung);
                             break;
                         }
                 }
             }
             #endregion

             #region IB Kanal
             if (clGlobals.IBChannel.SignalAktiv)
             {
                 switch (clGlobals.IBChannel.Signalform)
                 {
                     case "Sinus":
                         {
                             clGlobals.Kanal_3_IB = Sinus(IB_MHLR, IB_KLR, clGlobals.IBChannel.Frequenz, clGlobals.IBChannel.Phasenverschiebung);
                             break;
                         }
                     case "Rechteck":
                         {
                             clGlobals.Kanal_3_IB = Rechteck(IB_MHLR, IB_KLR, clGlobals.IBChannel.Frequenz, clGlobals.IBChannel.Phasenverschiebung);
                             break;
                         }
                     case "On-Ramp":
                         {
                             clGlobals.Kanal_3_IB = OnRamp(IB_MHLR, IB_KLR, clGlobals.IBChannel.Frequenz, clGlobals.IBChannel.Phasenverschiebung);
                             break;
                         }
                     case "Off-Ramp":
                         {
                             clGlobals.Kanal_3_IB = OffRamp(IB_MHLR, IB_KLR, clGlobals.IBChannel.Frequenz, clGlobals.IBChannel.Phasenverschiebung);
                             break;
                         }
                 }
             }

             #endregion

             #region IC Kanal
             if (clGlobals.ICChannel.SignalAktiv)
             {
                 switch (clGlobals.ICChannel.Signalform)
                 {
                     case "Sinus":
                         {
                             clGlobals.Kanal_4_IC = Sinus(IC_MHLR, IC_KLR, clGlobals.ICChannel.Frequenz, clGlobals.ICChannel.Phasenverschiebung);
                             break;
                         }
                     case "Rechteck":
                         {
                             clGlobals.Kanal_4_IC = Rechteck(IC_MHLR, IC_KLR, clGlobals.ICChannel.Frequenz, clGlobals.ICChannel.Phasenverschiebung);
                             break;
                         }
                     case "On-Ramp":
                         {
                             clGlobals.Kanal_4_IC = OnRamp(IC_MHLR, IC_KLR, clGlobals.ICChannel.Frequenz, clGlobals.ICChannel.Phasenverschiebung);
                             break;
                         }
                     case "Off-Ramp":
                         {
                             clGlobals.Kanal_4_IC = OffRamp(IC_MHLR, IC_KLR, clGlobals.ICChannel.Frequenz, clGlobals.ICChannel.Phasenverschiebung);
                             break;
                         }
                 }
             }
             #endregion

             #region OR Kanal
             if (clGlobals.IRChannel.SignalAktiv)
             {
                 switch (clGlobals.ORChannel.Signalform)
                 {
                     case "Sinus":
                         {
                             clGlobals.Kanal_5_OR = Sinus(OR_MHLR, OR_KLR, clGlobals.ORChannel.Frequenz, clGlobals.ORChannel.Phasenverschiebung);
                             break;
                         }
                     case "Rechteck":
                         {
                             clGlobals.Kanal_5_OR = Rechteck(OR_MHLR, OR_KLR, clGlobals.ORChannel.Frequenz, clGlobals.ORChannel.Phasenverschiebung);
                             break;
                         }
                     case "On-Ramp":
                         {
                             clGlobals.Kanal_5_OR = OnRamp(OR_MHLR, OR_KLR, clGlobals.ORChannel.Frequenz, clGlobals.ORChannel.Phasenverschiebung);
                             break;
                         }
                     case "Off-Ramp":
                         {
                             clGlobals.Kanal_5_OR = OffRamp(OR_MHLR, OR_KLR, clGlobals.ORChannel.Frequenz, clGlobals.ORChannel.Phasenverschiebung);
                             break;
                         }
                 }
             }
             #endregion

             #region OG Kanal
             if (clGlobals.OGChannel.SignalAktiv)
             {
                 switch (clGlobals.OGChannel.Signalform)
                 {
                     case "Sinus":
                         {
                             clGlobals.Kanal_6_OG = Sinus(OG_MHLR, OG_KLR, clGlobals.OGChannel.Frequenz, clGlobals.OGChannel.Phasenverschiebung);
                             break;
                         }
                     case "Rechteck":
                         {
                             clGlobals.Kanal_6_OG = Rechteck(OG_MHLR, OG_KLR, clGlobals.OGChannel.Frequenz, clGlobals.OGChannel.Phasenverschiebung);
                             break;
                         }
                     case "On-Ramp":
                         {
                             clGlobals.Kanal_6_OG = OnRamp(OG_MHLR, OG_KLR, clGlobals.OGChannel.Frequenz, clGlobals.OGChannel.Phasenverschiebung);
                             break;
                         }
                     case "Off-Ramp":
                         {
                             clGlobals.Kanal_6_OG = OffRamp(OG_MHLR, OG_KLR, clGlobals.OGChannel.Frequenz, clGlobals.OGChannel.Phasenverschiebung);
                             break;
                         }
                 }
             }
             #endregion

             #region OB Kanal
             if (clGlobals.IRChannel.SignalAktiv)
             {
                 switch (clGlobals.IRChannel.Signalform)
                 {
                     case "Sinus":
                         {
                             clGlobals.Kanal_7_OB = Sinus(OB_MHLR, OB_KLR, clGlobals.OBChannel.Frequenz, clGlobals.OBChannel.Phasenverschiebung);
                             break;
                         }
                     case "Rechteck":
                         {
                             clGlobals.Kanal_7_OB = Rechteck(OB_MHLR, OB_KLR, clGlobals.OBChannel.Frequenz, clGlobals.OBChannel.Phasenverschiebung);
                             break;
                         }
                     case "On-Ramp":
                         {
                             clGlobals.Kanal_7_OB = OnRamp(OB_MHLR, OB_KLR, clGlobals.OBChannel.Frequenz, clGlobals.OBChannel.Phasenverschiebung);
                             break;
                         }
                     case "Off-Ramp":
                         {
                             clGlobals.Kanal_7_OB = OffRamp(OB_MHLR, OB_KLR, clGlobals.OBChannel.Frequenz, clGlobals.OBChannel.Phasenverschiebung);
                             break;
                         }
                 }
             }
             #endregion

             #region OC Kanal
             if (clGlobals.OCChannel.SignalAktiv)
             {
                 switch (clGlobals.OCChannel.Signalform)
                 {
                     case "Sinus":
                         {
                             clGlobals.Kanal_8_OC = Sinus(OC_MHLR, OC_KLR, clGlobals.OCChannel.Frequenz, clGlobals.OCChannel.Phasenverschiebung);
                             break;
                         }
                     case "Rechteck":
                         {
                             clGlobals.Kanal_8_OC = Rechteck(OC_MHLR, OC_KLR, clGlobals.OCChannel.Frequenz, clGlobals.OCChannel.Phasenverschiebung);
                             break;
                         }
                     case "On-Ramp":
                         {
                             clGlobals.Kanal_8_OC = OnRamp(OC_MHLR, OC_KLR, clGlobals.OCChannel.Frequenz, clGlobals.OCChannel.Phasenverschiebung);
                             break;
                         }
                     case "Off-Ramp":
                         {
                             clGlobals.Kanal_8_OC = OffRamp(OC_MHLR, OC_KLR, clGlobals.OCChannel.Frequenz, clGlobals.OCChannel.Phasenverschiebung);
                             break;
                         }
                 }
             }
             #endregion

         }*/



    }
}
