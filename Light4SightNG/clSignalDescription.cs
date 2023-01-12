using System;

namespace Light4SightNG
{
    // just a test
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

        //!!!!! Beschreibung überarbeiten
        /// <summary>
        /// Methode Signalform prüft den übergebenen String auf eine der vier Signalformen.
        /// Wenn eine Übereinstimmung gefunden wird, wird ein entsprechender int-Wert in iSignalform gespeichert.
        /// Bei keiner Übereinstimmung wird der Rückgabewert auf 0 gesetzt = Fehler (keine gültige Signalform)
        /// [set,get]
        /// </summary>
        /// <value>String [Sinus=1, Rechteck=2, On-Ramp=3, Off-Ramp=4]</value>
        /// <return>int [0=Fehler, 1=Sinus, 2=Rechteck, 3=On-Ramp, 4=Off-Ramp]</return>
        public String Signalform
        {
            set
            {
                sSignalform = value;
            }
            get
            {
                return sSignalform;
            }
        }

        /// <summary>
        /// Methode Frequenz überprüft ob der übergebene Wert innerhalb des zulässigen Frequenzbereichs von 1-100Hz liegt
        /// [set,get]
        /// </summary>
        /// <value>int [1-100]</value>
        /// <return>int [1-100]Hz, 0 bei Fehler</return>
        public int Frequenz
        {
            set
            {
                if (value >= 1 && value <= 100)
                    iFrequenz = value;
                else
                {
                    if (value == 0 && sSignalform == "Sinus")
                        iFrequenz = value;
                    else
                    {
                        bFehler = true;
                    }
                }
            }

            get
            {
                return iFrequenz;
            }
        }

        public void ParameterPolynom(double p4, double p3, double p2, double p1, double incpt)
        {
            poly4 = p4;
            poly3 = p3;
            poly2 = p2;
            poly1 = p1;
            intercept = incpt;
        }

        public double MittlereHelligkeit_cdm2
        {

            set
            {
                dTempMH = value;

                if (dTempMH >= 0.0 && dTempMH <= dMaxMHCal_cdm2)    //Liegt der einegebene Signalparameter innerhalb des gültigen Bereichs?
                {
                    dMH_cdm2 = dTempMH;	//wenn ja, dann der gekapselten Variable zuweisen
                    // dMH_100  = Math.Round((dTempMH/dMaxMHCal_cdm2),3);
                    dMH_100 = Math.Round((Math.Pow(dTempMH, 4) * poly4 + Math.Pow(dTempMH, 3) * poly3 + Math.Pow(dTempMH, 2) * poly2 + dTempMH * poly1 + intercept), 3);
                    if (dMH_100 < 0) { dMH_100 = 0; }
                    if (dMH_100 > 1) { dMH_100 = 1; }
                    dTempMH = 0;    //für die nächste benutzung vorbereiten
                }
                else    //Wert liegt nicht im gültigen Bereich
                {
                    dTempMH = 0;    //für die nächste Benutzung vorbereiten
                    bFehler = true; //Fehlerindikator setzen
                }
            }

            get
            {
                return dMH_cdm2;
            }
        }

        public double MittlereHelligkeit_100
        {
            get
            {
                return dMH_100;
            }
        }

        public double KonSC1_100
        {
            get
            {
                return dKonSC1_100;
            }
            set
            {
                dKonSC1_100 = value;
            }
        }

        public double KonSC2_100
        {
            get
            {
                return dKonSC2_100;
            }
            set
            {
                dKonSC2_100 = value;
            }
        }

        public double Kontrast_100
        {
            get
            {
                return dKontrast_100;
            }

            set
            {
                dKontrast_100 = value;
            }

        }

        public double SC1DeltaK_100
        {
            get
            {
                return dSC1DeltaK_100;
            }
            set
            {
                dSC1DeltaK_100 = value;
            }
        }

        public double SC2DeltaK_100
        {
            get
            {
                return dSC2DeltaK_100;
            }
            set
            {
                dSC2DeltaK_100 = value;
            }
        }

        public int Phasenverschiebung
        {
            set
            {
                iTemp = value;
                if (iTemp >= 0 && iTemp <= 359)         //wenn die Wandlung geklappt hat wird hier auf den gültigen Wertebereich geprüft
                {
                    iPhasenverschiebung = iTemp;                //liegt der Wert innerhlab der Grenzen, wird er der gekapselten Variable zugewiesen
                    iTemp = 0;                          //für nächste Benutzung zurücksetzen			
                }
                else    //liegt der Wert außerhalb des gültigen Wertebereichs
                {
                    iTemp = 0;                          //für nächste Benutzung zurücksetzen
                    bFehler = true;                     //Fehlerindikatr setzen
                }
            }

            get
            {
                return iPhasenverschiebung;
            }
        }

        public bool Fehler
        {
            get
            {
                return bFehler;
            }
        }

        #endregion
    }
}
