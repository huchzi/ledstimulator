using System;
using System.IO;


namespace CalibrateLEDStimulator
{
    internal static class clGlobals
    {
        public static bool bPlaySignal = false;
        public static bool flagSignalWiedergabe = false;
        public static bool flagDebugLog = false;
        public static bool flagUntersuchunglaeuft = false;

        public static byte[] waveDaten;

        private static Int16 iANZAHL_KANAELE = 8;												//acht Kanäle
        private static int iABTASTFREQUENZ = 96000;											//Abtastrate für die Erzeugung der Audiodaten und für die Wiedergabe
        private static int iBYTES_PRO_SEKUNDE = iANZAHL_KANAELE * iABTASTFREQUENZ * 2;			//Wiedergegebene Bytes pro Sekunde (8 Kanäle * 96000 samples pro Sekunde * 2 Byte pro Sample)			
        private static Int16 iSAMPLE_CONTAINER_GROESSE = 16;												//Größe eines Samples
        private static Int16 iBLOCKAUSRICHTUNG = (Int16)(iANZAHL_KANAELE * iSAMPLE_CONTAINER_GROESSE / 8);	//interne Struktur Wave-Daten (Blockausrichtung)
        private static int iTRAEGERFREQUENZ = 20000;
        private static int iSAMPLE_LAENGE = 1;

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

        public static int CarrierFrequency
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

    }
}
