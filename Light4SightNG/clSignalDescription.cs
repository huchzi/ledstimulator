using System;

namespace Light4SightNG
{
    public class clSignalDescription
    {
        #region Variablen
        /// <summary>
        /// Private Eigenschaften des Objekts
        /// </summary>

        //Temps
        private double dTempMH;
        private int iTemp;

        private int iFrequenz = 0, iPhasenverschiebung = 0;
        private String sSignalform;
        private double dMaxMHCal_cdm2 = 0.0;
        private double poly4, poly3, poly2, poly1, intercept;
        private double dMH_cdm2 = 0.0, dMH_100 = 0.0, dKontrast_100 = 0.0;
        private double dKonSC1_100 = 0.0, dKonSC2_100 = 0.0;
        private double dSC1DeltaK_100 = 0.0, dSC2DeltaK_100 = 0.0;
        private bool bSignalaktiv = false, bFehler = false; //bFehler = true wenn ein Fehler aufgetreten ist

        #endregion

        #region Methoden

        public clSignalDescription()
        {
        }

        public double MaxMHCal
        {
            set
            {
                dMaxMHCal_cdm2 = Math.Round(value, 2);
            }

            get
            {
                return dMaxMHCal_cdm2;
            }
        }

        /// <summary>
        /// Methode Signalaktiv
        /// [set,get]
        /// </summary>
        /// <value>bool[false = inaktiv, true = aktiv]</value>
        /// <return>bool[false = inaktiv, true = aktiv]</return>
        public bool SignalAktiv
        {
            set
            {
                bSignalaktiv = value;
            }

            get
            {
                return bSignalaktiv;
            }
        }



        #endregion
    }
}
