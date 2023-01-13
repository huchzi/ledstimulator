using System;
using System.IO;



namespace Light4SightNG
{
    internal static class clGlobals
    {
        public static bool bPlaySignal = false;
        public static bool flagSignalWiedergabe = false;
        public static bool flagDebugLog = false;
        public static bool flagUntersuchunglaeuft = false;

        public static string LEDBereich = "alle";
        public static string Staircase = "beide";

        public static byte[] waveDaten;

        // private static string[] strSteigungTmp      = new string[8];
        // private static string[] strSchnittpunktTmp  = new string[8];
        private static string[] poly4Tmp = new string[8];
        private static string[] poly3Tmp = new string[8];
        private static string[] poly2Tmp = new string[8];
        private static string[] poly1Tmp = new string[8];
        private static string[] interceptTmp = new string[8];
        private static string[] dMaxMHTmp = new string[8];

        private static Int16 iANZAHL_KANAELE = 8;												//acht Kanäle
        private static int iABTASTFREQUENZ = 96000;											//Abtastrate für die Erzeugung der Audiodaten und für die Wiedergabe
        private static int iBYTES_PRO_SEKUNDE = iANZAHL_KANAELE * iABTASTFREQUENZ * 2;			//Wiedergegebene Bytes pro Sekunde (8 Kanäle * 96000 samples pro Sekunde * 2 Byte pro Sample)			
        private static Int16 iSAMPLE_CONTAINER_GROESSE = 16;												//Größe eines Samples
        private static Int16 iBLOCKAUSRICHTUNG = (Int16)(iANZAHL_KANAELE * iSAMPLE_CONTAINER_GROESSE / 8);	//interne Struktur Wave-Daten (Blockausrichtung)
        private static int iTRAEGERFREQUENZ = 20000;
        private static int iSAMPLE_LAENGE = 1;

        private static double dDeltaPI = (2 * Math.PI / iABTASTFREQUENZ);
        private static double dDeltaPhi = iABTASTFREQUENZ / 360;
        private static double dDeltaPhiSinus = (2 * Math.PI / 360);

        private static double dKalMesswert;


        public static double DeltaPhiSinus
        {
            get
            {
                return dDeltaPhiSinus;
            }
        }

        public static double DeltaPhi
        {
            get
            {
                return dDeltaPhi;
            }
        }

        public static double DeltaPI
        {
            get
            {
                return dDeltaPI;
            }
        }

        public static Int16 AnzahlKanaele
        {
            get
            {
                return iANZAHL_KANAELE;
            }
        }

        public static int AbtastFrequenz
        {
            get
            {
                return iABTASTFREQUENZ;
            }
        }

        public static int BytesProSekunde
        {
            get
            {
                return iBYTES_PRO_SEKUNDE;
            }
        }

        public static Int16 SampleContainerGroesse
        {
            get
            {
                return iSAMPLE_CONTAINER_GROESSE;
            }
        }

        public static Int16 Blockausrichtung
        {
            get
            {
                return iBLOCKAUSRICHTUNG;
            }
        }

        public static int TraegerFrequenz
        {
            get
            {
                return iTRAEGERFREQUENZ;
            }
        }

        public static int SampleLaenge
        {
            get
            {
                return iSAMPLE_LAENGE;
            }
        }

        public static byte Highbyte(Int16 HiTemp)
        {
            byte hi = (byte)(HiTemp >> 8);
            return hi;
        }

        public static byte Lowbyte(Int16 LowTemp)
        {
            byte lo = (byte)(LowTemp & 255);
            return lo;
        }

        public static double KalibrierungsMesswert
        {
            set
            {
                dKalMesswert = value;
            }
            get
            {
                return dKalMesswert;
            }
        }

        public static int KalibrierungsdatenLesen()
        {
            try
            {
                StreamReader srKalibrierungsdaten = new StreamReader(".\\calibrationdataPoly4.csv");
                char[] charSep = new char[] { ';' };

                dMaxMHTmp = srKalibrierungsdaten.ReadLine().Split(charSep, StringSplitOptions.RemoveEmptyEntries);
                poly4Tmp = srKalibrierungsdaten.ReadLine().Split(charSep, StringSplitOptions.RemoveEmptyEntries);
                poly3Tmp = srKalibrierungsdaten.ReadLine().Split(charSep, StringSplitOptions.RemoveEmptyEntries);
                poly2Tmp = srKalibrierungsdaten.ReadLine().Split(charSep, StringSplitOptions.RemoveEmptyEntries);
                poly1Tmp = srKalibrierungsdaten.ReadLine().Split(charSep, StringSplitOptions.RemoveEmptyEntries);
                interceptTmp = srKalibrierungsdaten.ReadLine().Split(charSep, StringSplitOptions.RemoveEmptyEntries);
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        public static double dMaxMH(int Kanal)
        {
            return (double.Parse(dMaxMHTmp[Kanal]));
        }

        public static double poly4(int Kanal)
        {
            return (double.Parse(poly4Tmp[Kanal]));
        }

        public static double poly3(int Kanal)
        {
            return (double.Parse(poly3Tmp[Kanal]));
        }

        public static double poly2(int Kanal)
        {
            return (double.Parse(poly2Tmp[Kanal]));
        }

        public static double poly1(int Kanal)
        {
            return (double.Parse(poly1Tmp[Kanal]));
        }

        public static double intercept(int Kanal)
        {
            return (double.Parse(interceptTmp[Kanal]));
        }

    }
}
