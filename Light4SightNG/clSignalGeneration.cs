using System;



namespace Light4SightNG
{
    class clSignalGeneration
    {
        private static double IR_MHLR, IR_KLR, IG_MHLR, IG_KLR, IB_MHLR, IB_KLR, IC_MHLR, IC_KLR;
        private static double OR_MHLR, OR_KLR, OG_MHLR, OG_KLR, OB_MHLR, OB_KLR, OC_MHLR, OC_KLR;

        public static void Untersuchungssignal()
        {
            #region IR Kanal

            if (Light4SightNG.IRChannel.SignalAktiv)
            {
                IR_MHLR = 32700 * Light4SightNG.IRChannel.MittlereHelligkeit_100;
                IR_KLR = (IR_MHLR * (1 + Light4SightNG.IRChannel.Kontrast_100 / 100)) - IR_MHLR;
                //IR_KLR = ((32700 - IR_MHLR) / 100) * IRChannel.Kontrast_100;

                switch (Light4SightNG.IRChannel.Signalform)
                {
                    case "Sinus":
                        {
                            clGlobals.Kanal_1_IR = Sinus(IR_MHLR, IR_KLR, Light4SightNG.IRChannel.Frequenz, Light4SightNG.IRChannel.Phasenverschiebung);
                            break;
                        }
                    case "Rechteck":
                        {
                            clGlobals.Kanal_1_IR = Rechteck(IR_MHLR, IR_KLR, Light4SightNG.IRChannel.Frequenz, Light4SightNG.IRChannel.Phasenverschiebung);
                            break;
                        }
                    case "On-Ramp":
                        {
                            clGlobals.Kanal_1_IR = OnRamp(IR_MHLR, IR_KLR, Light4SightNG.IRChannel.Frequenz, Light4SightNG.IRChannel.Phasenverschiebung);
                            break;
                        }
                    case "Off-Ramp":
                        {
                            clGlobals.Kanal_1_IR = OffRamp(IR_MHLR, IR_KLR, Light4SightNG.IRChannel.Frequenz, Light4SightNG.IRChannel.Phasenverschiebung);
                            break;
                        }
                }
            }

            #endregion

            #region IG Kanal
            if (Light4SightNG.IGChannel.SignalAktiv)
            {
                IG_MHLR = 32700 * Light4SightNG.IGChannel.MittlereHelligkeit_100;
                IG_KLR = (IG_MHLR * (1 + Light4SightNG.IGChannel.Kontrast_100 / 100)) - IG_MHLR;
                //IG_KLR = ((32700 - IG_MHLR) / 100) * IGChannel.Kontrast_100;
                switch (Light4SightNG.IGChannel.Signalform)
                {
                    case "Sinus":
                        {
                            clGlobals.Kanal_2_IG = Sinus(IG_MHLR, IG_KLR, Light4SightNG.IGChannel.Frequenz, Light4SightNG.IGChannel.Phasenverschiebung);
                            break;
                        }
                    case "Rechteck":
                        {
                            clGlobals.Kanal_2_IG = Rechteck(IG_MHLR, IG_KLR, Light4SightNG.IGChannel.Frequenz, Light4SightNG.IGChannel.Phasenverschiebung);
                            break;
                        }
                    case "On-Ramp":
                        {
                            clGlobals.Kanal_2_IG = OnRamp(IG_MHLR, IG_KLR, Light4SightNG.IGChannel.Frequenz, Light4SightNG.IGChannel.Phasenverschiebung);
                            break;
                        }
                    case "Off-Ramp":
                        {
                            clGlobals.Kanal_2_IG = OffRamp(IG_MHLR, IG_KLR, Light4SightNG.IGChannel.Frequenz, Light4SightNG.IGChannel.Phasenverschiebung);
                            break;
                        }
                }
            }
            #endregion

            #region IB Kanal
            if (Light4SightNG.IBChannel.SignalAktiv)
            {
                IB_MHLR = 32700 * Light4SightNG.IBChannel.MittlereHelligkeit_100;
                IB_KLR = (IB_MHLR * (1 + Light4SightNG.IBChannel.Kontrast_100 / 100)) - IB_MHLR;
                //IB_KLR = ((32700 - IB_MHLR) / 100) * IBChannel.Kontrast_100;
                switch (Light4SightNG.IBChannel.Signalform)
                {
                    case "Sinus":
                        {
                            clGlobals.Kanal_3_IB = Sinus(IB_MHLR, IB_KLR, Light4SightNG.IBChannel.Frequenz, Light4SightNG.IBChannel.Phasenverschiebung);
                            break;
                        }
                    case "Rechteck":
                        {
                            clGlobals.Kanal_3_IB = Rechteck(IB_MHLR, IB_KLR, Light4SightNG.IBChannel.Frequenz, Light4SightNG.IBChannel.Phasenverschiebung);
                            break;
                        }
                    case "On-Ramp":
                        {
                            clGlobals.Kanal_3_IB = OnRamp(IB_MHLR, IB_KLR, Light4SightNG.IBChannel.Frequenz, Light4SightNG.IBChannel.Phasenverschiebung);
                            break;
                        }
                    case "Off-Ramp":
                        {
                            clGlobals.Kanal_3_IB = OffRamp(IB_MHLR, IB_KLR, Light4SightNG.IBChannel.Frequenz, Light4SightNG.IBChannel.Phasenverschiebung);
                            break;
                        }
                }
            }

            #endregion

            #region IC Kanal
            if (Light4SightNG.ICChannel.SignalAktiv)
            {
                IC_MHLR = 32700 * Light4SightNG.ICChannel.MittlereHelligkeit_100;
                IC_KLR = (IC_MHLR * (1 + Light4SightNG.ICChannel.Kontrast_100 / 100)) - IC_MHLR;
                //IC_KLR = ((32700 - IC_MHLR) / 100) * ICChannel.Kontrast_100;
                switch (Light4SightNG.ICChannel.Signalform)
                {
                    case "Sinus":
                        {
                            clGlobals.Kanal_4_IC = Sinus(IC_MHLR, IC_KLR, Light4SightNG.ICChannel.Frequenz, Light4SightNG.ICChannel.Phasenverschiebung);
                            break;
                        }
                    case "Rechteck":
                        {
                            clGlobals.Kanal_4_IC = Rechteck(IC_MHLR, IC_KLR, Light4SightNG.ICChannel.Frequenz, Light4SightNG.ICChannel.Phasenverschiebung);
                            break;
                        }
                    case "On-Ramp":
                        {
                            clGlobals.Kanal_4_IC = OnRamp(IC_MHLR, IC_KLR, Light4SightNG.ICChannel.Frequenz, Light4SightNG.ICChannel.Phasenverschiebung);
                            break;
                        }
                    case "Off-Ramp":
                        {
                            clGlobals.Kanal_4_IC = OffRamp(IC_MHLR, IC_KLR, Light4SightNG.ICChannel.Frequenz, Light4SightNG.ICChannel.Phasenverschiebung);
                            break;
                        }
                }
            }
            #endregion

            #region OR Kanal
            if (Light4SightNG.ORChannel.SignalAktiv)
            {
                OR_MHLR = 32700 * Light4SightNG.ORChannel.MittlereHelligkeit_100;
                OR_KLR = (OR_MHLR * (1 + Light4SightNG.ORChannel.Kontrast_100 / 100)) - OR_MHLR;
                //OR_KLR = ((32700 - OR_MHLR) / 100) * ORChannel.Kontrast_100;
                switch (Light4SightNG.ORChannel.Signalform)
                {
                    case "Sinus":
                        {
                            clGlobals.Kanal_5_OR = Sinus(OR_MHLR, OR_KLR, Light4SightNG.ORChannel.Frequenz, Light4SightNG.ORChannel.Phasenverschiebung);
                            break;
                        }
                    case "Rechteck":
                        {
                            clGlobals.Kanal_5_OR = Rechteck(OR_MHLR, OR_KLR, Light4SightNG.ORChannel.Frequenz, Light4SightNG.ORChannel.Phasenverschiebung);
                            break;
                        }
                    case "On-Ramp":
                        {
                            clGlobals.Kanal_5_OR = OnRamp(OR_MHLR, OR_KLR, Light4SightNG.ORChannel.Frequenz, Light4SightNG.ORChannel.Phasenverschiebung);
                            break;
                        }
                    case "Off-Ramp":
                        {
                            clGlobals.Kanal_5_OR = OffRamp(OR_MHLR, OR_KLR, Light4SightNG.ORChannel.Frequenz, Light4SightNG.ORChannel.Phasenverschiebung);
                            break;
                        }
                }
            }
            #endregion

            #region OG Kanal
            if (Light4SightNG.OGChannel.SignalAktiv)
            {
                OG_MHLR = 32700 * Light4SightNG.OGChannel.MittlereHelligkeit_100;
                OG_KLR = (OG_MHLR * (1 + Light4SightNG.OGChannel.Kontrast_100 / 100)) - OG_MHLR;
                //OG_KLR = ((32700 - OG_MHLR) / 100) * OGChannel.Kontrast_100;
                switch (Light4SightNG.OGChannel.Signalform)
                {
                    case "Sinus":
                        {
                            clGlobals.Kanal_6_OG = Sinus(OG_MHLR, OG_KLR, Light4SightNG.OGChannel.Frequenz, Light4SightNG.OGChannel.Phasenverschiebung);
                            break;
                        }
                    case "Rechteck":
                        {
                            clGlobals.Kanal_6_OG = Rechteck(OG_MHLR, OG_KLR, Light4SightNG.OGChannel.Frequenz, Light4SightNG.OGChannel.Phasenverschiebung);
                            break;
                        }
                    case "On-Ramp":
                        {
                            clGlobals.Kanal_6_OG = OnRamp(OG_MHLR, OG_KLR, Light4SightNG.OGChannel.Frequenz, Light4SightNG.OGChannel.Phasenverschiebung);
                            break;
                        }
                    case "Off-Ramp":
                        {
                            clGlobals.Kanal_6_OG = OffRamp(OG_MHLR, OG_KLR, Light4SightNG.OGChannel.Frequenz, Light4SightNG.OGChannel.Phasenverschiebung);
                            break;
                        }
                }
            }
            #endregion

            #region OB Kanal
            if (Light4SightNG.OBChannel.SignalAktiv)
            {
                OB_MHLR = 32700 * Light4SightNG.OBChannel.MittlereHelligkeit_100;
                OB_KLR = (OB_MHLR * (1 + Light4SightNG.OBChannel.Kontrast_100 / 100)) - OB_MHLR;
                //OB_KLR = ((32700 - OB_MHLR) / 100) * OBChannel.Kontrast_100;
                switch (Light4SightNG.OBChannel.Signalform)
                {
                    case "Sinus":
                        {
                            clGlobals.Kanal_7_OB = Sinus(OB_MHLR, OB_KLR, Light4SightNG.OBChannel.Frequenz, Light4SightNG.OBChannel.Phasenverschiebung);
                            break;
                        }
                    case "Rechteck":
                        {
                            clGlobals.Kanal_7_OB = Rechteck(OB_MHLR, OB_KLR, Light4SightNG.OBChannel.Frequenz, Light4SightNG.OBChannel.Phasenverschiebung);
                            break;
                        }
                    case "On-Ramp":
                        {
                            clGlobals.Kanal_7_OB = OnRamp(OB_MHLR, OB_KLR, Light4SightNG.OBChannel.Frequenz, Light4SightNG.OBChannel.Phasenverschiebung);
                            break;
                        }
                    case "Off-Ramp":
                        {
                            clGlobals.Kanal_7_OB = OffRamp(OB_MHLR, OB_KLR, Light4SightNG.OBChannel.Frequenz, Light4SightNG.OBChannel.Phasenverschiebung);
                            break;
                        }
                }
            }
            #endregion

            #region OC Kanal
            if (Light4SightNG.OCChannel.SignalAktiv)
            {
                OC_MHLR = 32700 * Light4SightNG.OCChannel.MittlereHelligkeit_100;
                OC_KLR = (OC_MHLR * (1 + Light4SightNG.OCChannel.Kontrast_100 / 100)) - OC_MHLR;
                //OC_KLR = ((32700 - OC_MHLR) / 100) * OCChannel.Kontrast_100;
                switch (Light4SightNG.OCChannel.Signalform)
                {
                    case "Sinus":
                        {
                            clGlobals.Kanal_8_OC = Sinus(OC_MHLR, OC_KLR, Light4SightNG.OCChannel.Frequenz, Light4SightNG.OCChannel.Phasenverschiebung);
                            break;
                        }
                    case "Rechteck":
                        {
                            clGlobals.Kanal_8_OC = Rechteck(OC_MHLR, OC_KLR, Light4SightNG.OCChannel.Frequenz, Light4SightNG.OCChannel.Phasenverschiebung);
                            break;
                        }
                    case "On-Ramp":
                        {
                            clGlobals.Kanal_8_OC = OnRamp(OC_MHLR, OC_KLR, Light4SightNG.OCChannel.Frequenz, Light4SightNG.OCChannel.Phasenverschiebung);
                            break;
                        }
                    case "Off-Ramp":
                        {
                            clGlobals.Kanal_8_OC = OffRamp(OC_MHLR, OC_KLR, Light4SightNG.OCChannel.Frequenz, Light4SightNG.OCChannel.Phasenverschiebung);
                            break;
                        }
                }
            }

            #endregion
            ConcatChannels();

        }

        public static void CalibrationSignal(int Kanal, double Elongation)
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
                    WriteToWaveContainer(zero, k, i);
                }

                //******** Trägerfrequenzsignal für die Ausgabe berechnen *********
                dValue = (double)((Elongation * 32700) * Math.Sin(dWinkel));	//Trägerfrequenz Elongation entsprechend der übergebenen amplitude berechnen
                WriteToWaveContainer(dValue, Kanal, i);
                WriteToWaveContainer(dValue, Kanal + 4, i);

                dWinkel += 2 * Math.PI * clGlobals.TraegerFrequenz / clGlobals.AbtastFrequenz;
                if (dWinkel > 2 * Math.PI)
                    dWinkel -= 2 * Math.PI;

            }

        }

        public static void CreateChannelArrays()
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

        public static void ClearChannelArrays()
        {
            clGlobals.Kanal_1_IR = null;
            clGlobals.Kanal_2_IG = null;
            clGlobals.Kanal_3_IB = null;
            clGlobals.Kanal_4_IC = null;
            clGlobals.Kanal_5_OR = null;
            clGlobals.Kanal_6_OG = null;
            clGlobals.Kanal_7_OB = null;
            clGlobals.Kanal_8_OC = null;
        }

        public static void ConcatChannels()
        {
            for (int i = 0; i <= (clGlobals.AbtastFrequenz - 1); i++)
            {
                WriteToWaveContainer(clGlobals.Kanal_1_IR[i], 0, i);
                WriteToWaveContainer(clGlobals.Kanal_2_IG[i], 1, i);
                WriteToWaveContainer(clGlobals.Kanal_3_IB[i], 2, i);
                WriteToWaveContainer(clGlobals.Kanal_4_IC[i], 3, i);
                WriteToWaveContainer(clGlobals.Kanal_5_OR[i], 4, i);
                WriteToWaveContainer(clGlobals.Kanal_6_OG[i], 5, i);
                WriteToWaveContainer(clGlobals.Kanal_7_OB[i], 6, i);
                WriteToWaveContainer(clGlobals.Kanal_8_OC[i], 7, i);
            }
        }

        private static void WriteToWaveContainer(double dValue, int iChannel, int iPosition)
        {
            clGlobals.waveDaten[(((iPosition * 8) + iChannel) * 2)] = clGlobals.Lowbyte((Int16)dValue);
            clGlobals.waveDaten[(((iPosition * 8) + iChannel) * 2) + 1] = clGlobals.Highbyte((Int16)dValue);
        }

        private static double[] Sinus(double MHLR, double KLR, int Frequenz, int Phasenwinkel)
        {
            double[] TempSinus = new Double[clGlobals.AbtastFrequenz];
            double dWinkel = 0.0;

            for (int i = 0; i <= clGlobals.AbtastFrequenz - 1; i++)
            {
                TempSinus[i] = (double)((MHLR + KLR * Math.Sin(Frequenz * clGlobals.DeltaPI * i + clGlobals.DeltaPhiSinus * Phasenwinkel)) * Math.Sin(dWinkel));

                dWinkel += 2 * Math.PI * clGlobals.TraegerFrequenz / clGlobals.AbtastFrequenz;
                if (dWinkel > 2 * Math.PI)
                    dWinkel -= 2 * Math.PI;
            }

            return TempSinus;
        }

        private static double[] Rechteck(double MHLR, double KLR, int Frequenz, int Phasenwinkel)
        {
            double[] TempRechteck = new Double[clGlobals.AbtastFrequenz];
            int iPeriode = 0;
            double dWinkel = 0.0;

            for (int i = 0; i <= clGlobals.AbtastFrequenz - 1; i++)
            {
                if (i == 0) //wenn die Schleife das erste Mal durchlaufen wird muss der Anfangswert berechnet werden
                    iPeriode = (int)(clGlobals.DeltaPhi * Phasenwinkel) / Frequenz;

                if (iPeriode >= (clGlobals.AbtastFrequenz / Frequenz))	//prüft ob die Anzahl der Abtastwerte für eine Periode des Rechteck-Signals überschritten wurde
                {
                    iPeriode = 0;	//wenn ja, Zähler zurücksetzen
                }

                if (iPeriode <= ((clGlobals.AbtastFrequenz / Frequenz)) / 2)	//prüfen ob der Zählwert innerhlab der ersten hälfte der Periode ist
                {
                    TempRechteck[i] = (double)((MHLR - KLR) * Math.Sin(dWinkel));	//wenn ja, wird der unter Wert für die Elongation berechnet
                }
                else	//wenn nein,
                {
                    TempRechteck[i] = (double)((MHLR + KLR) * Math.Sin(dWinkel));	//wird hier der obere Wert für die Elongation berechnet
                }

                dWinkel += 2 * Math.PI * clGlobals.TraegerFrequenz / clGlobals.AbtastFrequenz;
                if (dWinkel > 2 * Math.PI)
                    dWinkel -= 2 * Math.PI;

                iPeriode++;	//Zähler erhöhen
            }

            return TempRechteck;

        }

        private static double[] OnRamp(double MHLR, double KLR, int Frequenz, int Phasenwinkel)
        {
            double[] TempOnRamp = new Double[clGlobals.AbtastFrequenz];
            int iPeriode = 0;
            double dWinkel = 0.0;

            for (int i = 0; i <= clGlobals.AbtastFrequenz - 1; i++)
            {
                if (i == 0)	//wenn die Schleife das erste Mal durchlaufen wird muss der Anfangswert berechnet werden
                    iPeriode = (int)(clGlobals.DeltaPhi * Phasenwinkel) / Frequenz;

                if (iPeriode >= (clGlobals.AbtastFrequenz / Frequenz)) //prüft ob die Anzahl der Abtastwerte für eine Periode des On-Ramp-Signals überschritten wurde
                {
                    iPeriode = 0;	//wenn ja, Zähler zurücksetzen
                }

                // (mittlere Helligkeit - Kontrast) ergibt den Startwert | pro Schleifendurchlauf wird hier eine Wert, abhängig von der Position in der Periode hinzu addiert
                TempOnRamp[i] = (double)((MHLR - KLR + ((2 * KLR / (clGlobals.AbtastFrequenz / Frequenz)) * iPeriode)) * Math.Sin(dWinkel));

                dWinkel += 2 * Math.PI * clGlobals.TraegerFrequenz / clGlobals.AbtastFrequenz;
                if (dWinkel > 2 * Math.PI)
                    dWinkel -= 2 * Math.PI;

                iPeriode++; //Zähler erhöhen
            }

            return TempOnRamp;

        }

        private static double[] OffRamp(double MHLR, double KLR, int Frequenz, int Phasenwinkel)
        {
            double[] TempOffRamp = new Double[clGlobals.AbtastFrequenz];
            int iPeriode = 0;
            double dWinkel = 0.0;

            for (int i = 0; i <= clGlobals.AbtastFrequenz - 1; i++)
            {
                if (i == 0)	//wenn die Schleife das erste Mal durchlaufen wird muss der Anfangswert berechnet werden
                    iPeriode = (int)(clGlobals.DeltaPhi * Phasenwinkel) / Frequenz;

                if (iPeriode >= (clGlobals.AbtastFrequenz / Frequenz))	//prüft ob die Anzahl der Abtastwerte für eine Periode des Off-Ramp-Signals überschritten wurde
                {
                    iPeriode = 0;	//wenn ja, Zähler zurücksetzen
                }
                // (mittlere Helligkeit + Kontrast) ergibt den Startwert | pro Schleifendurchlauf wird hier eine Wert, abhängig von der Position in der Periode subtrahiert
                TempOffRamp[i] = (double)((MHLR + KLR - ((2 * KLR / (clGlobals.AbtastFrequenz / Frequenz)) * iPeriode)) * Math.Sin(dWinkel));

                dWinkel += 2 * Math.PI * clGlobals.TraegerFrequenz / clGlobals.AbtastFrequenz;
                if (dWinkel > 2 * Math.PI)
                    dWinkel -= 2 * Math.PI;

                iPeriode++;	//Zähler erhöhen
            }

            return TempOffRamp;

        }

        public static double[] StandardSinus(double Elongation)
        {

            double[] StdSinus = new Double[clGlobals.AbtastFrequenz];
            double dWinkel = 0.0;

            for (int i = 0; i <= clGlobals.AbtastFrequenz - 1; i++)
            {
                StdSinus[i] = (Int16)((double)((Elongation * 32767) * Math.Sin(dWinkel)));

                dWinkel += 2 * Math.PI * clGlobals.TraegerFrequenz / clGlobals.AbtastFrequenz;
                if (dWinkel > 2 * Math.PI)
                    dWinkel -= 2 * Math.PI;
            }

            return StdSinus;
        }

        public static double[] VoidSignal()
        {
            double[] VoidValues = new Double[clGlobals.AbtastFrequenz];

            for (int i = 0; i <= clGlobals.AbtastFrequenz - 1; i++)
            {
                VoidValues[i] = 0.0;
            }

            return VoidValues;
        }
    }
}
