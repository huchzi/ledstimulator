using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Light4SightNG
{
    internal class StdStrategie
    {
        private double[] Kontrast_100 = new double[8];
        private double[] Frequenz_100 = new double[8];

        private int maxFaktor = 40960;

        private int faktorSC1;
        private int faktorStepSC1;

        private int faktorSC2;
        private int faktorStepSC2;

        private int iDurchlaufSC1 = 1, iDurchlaufSC2 = 1;
        private int iSC1K100 = 0, iSC1K0 = 0, iSC2K100 = 0, iSC2K0 = 0;
        private int rot, gruen, blau, cyan;

        private bool SC1_gesehen_alt = false, SC2_gesehen_alt = false, SC1_gesehen = false, SC2_gesehen = false;
        private bool SC1_aktiv = false, SC1_7 = false, SC2_7 = true;

        private string LED_Gruppe = "außen";

        public bool testeCFF = false;

        private int finalCFF = -1;
        private int currentCFF = -1;

        private string logKontraste;

        private clAudioControl AudioControl = new clAudioControl();

        private Random dRand = new Random();

        public event EventHandler<AbbruchEventArgs> abbruch;
        
        public StdStrategie(string LEDgruppe, bool CFFtest)
        {
            LED_Gruppe = LEDgruppe;
            testeCFF = CFFtest;
        }

        public StdStrategie()
        {
        }

        protected virtual void OnAbbruch(AbbruchEventArgs e)
        {
            Light4SightNG.cff = finalCFF;
            this.abbruch(this, e);
        }

        public void StartStdStrategie()
        {
            if (!testeCFF) InitValuesContrast(LED_Gruppe); else InitValuesCFF(LED_Gruppe);
            
            #region Kontrastwerte der aktiven Kanäle einlesen
            if (Light4SightNG.IRChannel.SignalAktiv)
            {
                Kontrast_100[0] = Light4SightNG.IRChannel.KonSC1_100;
            }
            if (Light4SightNG.IGChannel.SignalAktiv)
            {
               Kontrast_100[1] = Light4SightNG.IGChannel.KonSC1_100;
            }
            if (Light4SightNG.IBChannel.SignalAktiv)
            {
                Kontrast_100[2] = Light4SightNG.IBChannel.KonSC1_100;
            }
            if (Light4SightNG.ICChannel.SignalAktiv)
            {
                Kontrast_100[3] = Light4SightNG.ICChannel.KonSC1_100;
             }
            if (Light4SightNG.ORChannel.SignalAktiv)
            {
                Kontrast_100[4] = Light4SightNG.ORChannel.KonSC1_100;
            }
            if (Light4SightNG.OGChannel.SignalAktiv)
            {
               Kontrast_100[5] = Light4SightNG.OGChannel.KonSC1_100;
             }
            if (Light4SightNG.OBChannel.SignalAktiv)
            {
                Kontrast_100[6] = Light4SightNG.OBChannel.KonSC1_100;
            }
            if (Light4SightNG.OCChannel.SignalAktiv)
            {
                Kontrast_100[7] = Light4SightNG.OCChannel.KonSC1_100;
            }
            #endregion
           
            if (LED_Gruppe == "innen")
            #region unterscheidung welche led_gruppe und entsprechnde index zuweisung
            {
                rot = 0;
                gruen = 1;
                blau = 2;
                cyan = 3;
            }
            else
            {
                rot = 4;
                gruen = 5;
                blau = 6;
                cyan = 7;
            }
            #endregion
            
            prepareLogFile();
            Randomisierung();
        }

        private void StaircaseDown()
        {
            bool bk7 = false;
            string logmessage = "Down: Schwelle erreicht!;;";
            int neuerFaktorSC1;

            if (iSC1K0 < 3 && iSC1K100 < 3)
            {
                #region Ausnahme für ersten Durchlauf
                if (iDurchlaufSC1 == 1)
                {
                    ChangeContrast(LED_Gruppe, faktorSC1, logmessage);
                }
                #endregion

                #region Kontrastwert überprüfen und neuen Kontrast einstellen
                else
                {
                    #region Proband hat ein Flackern gesehen
                    if (this.SC1_gesehen == true)
                    {
                        #region Proband hat beim Mal davor auch ein Flackern gesehen
                        if (this.SC1_gesehen_alt == true)
                        {
                            neuerFaktorSC1 = faktorSC1 + faktorStepSC1;
                            if (ChangeContrast(LED_Gruppe, neuerFaktorSC1, logmessage))
                            {
                                faktorSC1 = neuerFaktorSC1;
                            }
                            else
                            {
                                iSC1K100++;
                                Logmessage("Versuch Überschreitung: " + Convert.ToString(iSC1K100), false);
                            }
                        }
                        #endregion
                        #region Proband hat beim letzten Mal kein Flackern gesehen
                        else
                        {
                            faktorStepSC1 = -faktorStepSC1 / 2;
                            neuerFaktorSC1 = faktorSC1 + faktorStepSC1;
                            if (ChangeContrast(LED_Gruppe, neuerFaktorSC1, logmessage))
                            {
                                faktorSC1 = neuerFaktorSC1;
                                bk7 = TesteAbbruch(faktorSC1, faktorStepSC1, true);
                            }
                            else
                            {
                                iSC1K100++;
                                Logmessage("Versuch Überschreitung: " + Convert.ToString(iSC1K100), false);
                            }
                        }
                        #endregion
                    }
                    #endregion
                    #region Proband hat KEIN Flackern gesehen
                    else
                    {
                        #region Proband hat beim Mal davor auch KEIN Flackern gesehen
                        //Flackern wurde beim letzten Mal NICHT gesehen (ab 2tem Durchlauf)
                        if (this.SC1_gesehen_alt == true)
                        {
                            faktorStepSC1 = -faktorStepSC1 / 2;
                            neuerFaktorSC1 = faktorSC1 + faktorStepSC1;
                            if (ChangeContrast(LED_Gruppe, neuerFaktorSC1, logmessage))
                            {
             
                                faktorSC1 = neuerFaktorSC1;
                                bk7 = TesteAbbruch(faktorSC1, faktorStepSC1, true);
                            }
                            else
                            {
                                iSC1K100++;
                                Logmessage("Versuch Überschreitung: " + Convert.ToString(iSC1K100), false);
                            }

                        }
                        #endregion

                        #region Proband hat beim letzten Mal Flackern gesehen
                        //Flackern wurde beim letzten Mal gesehen
                        else
                        {
                            neuerFaktorSC1 = faktorSC1 + faktorStepSC1;
                            if (ChangeContrast(LED_Gruppe, neuerFaktorSC1, logmessage))
                            {
                                faktorSC1 = neuerFaktorSC1;
                            }
                            else
                            {
                                iSC1K100++;
                                Logmessage("Versuch Überschreitung: " + Convert.ToString(iSC1K100), false);
                            }

                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion

                if (bk7) Logmessage(logKontraste, false);
                
                if (SC1_7 == true && SC2_7 == true)
                {
                    this.OnAbbruch(new AbbruchEventArgs(""));
                }

                if (SC1_7 == false)
                {
                    if (iDurchlaufSC1 == 1) SC1_gesehen_alt = true; 
                    else SC1_gesehen_alt = SC1_gesehen;
                    SignalWiedergeben();
                    iDurchlaufSC1++;
                }
                else Randomisierung();

            }
            else
            {
                this.OnAbbruch(new AbbruchEventArgs(""));
            }
        }

        private void StaircaseUp()
        {
            bool bk7 = false;
            string logmessage = "Up: Schwelle erreicht!;;";
            int neuerFaktorSC2;

            if (iSC2K0 < 3 && iSC2K100 < 3)
            {
                #region Ausnahme für ersten Durchlauf
                if (iDurchlaufSC2 == 1)
                {
                    ChangeContrast(LED_Gruppe, faktorSC2, logmessage);
                }
                #endregion

                #region Kontrastwert überprüfen und neuen Kontrast einstellen
                else
                {
                    #region Proband hat ein Flackern gesehen
                    if (this.SC2_gesehen == true)
                    {
                        #region Proband hat beim Mal davor auch ein Flackern gesehen
                        if (this.SC2_gesehen_alt == true)
                        {
                            neuerFaktorSC2 = faktorSC2 + faktorStepSC2;
                            if (ChangeContrast(LED_Gruppe, neuerFaktorSC2, logmessage))
                            {
                                faktorSC2 = neuerFaktorSC2;
                            }
                            else
                            {
                                iSC2K100++;
                                Logmessage("Versuch Überschreitung: " + Convert.ToString(iSC2K100), false);
                            }
                        }
                        #endregion
                        #region Proband hat beim letzten Mal kein Flackern gesehen
                        else
                        {
                            faktorStepSC2 = -faktorStepSC2 / 2;
                            neuerFaktorSC2 = faktorSC2 + faktorStepSC2;
                            if (ChangeContrast(LED_Gruppe, neuerFaktorSC2, logmessage))
                            {
                                faktorSC2 = neuerFaktorSC2;
                                bk7 = TesteAbbruch(faktorSC2, faktorStepSC2, false);
                            }
                            else
                            {
                                iSC2K100++;
                                Logmessage("Versuch Überschreitung: " + Convert.ToString(iSC2K100), false);
                            }
                        }
                        #endregion
                    }
                    #endregion
                    #region Proband hat KEIN Flackern gesehen
                    else
                    {
                        #region Proband hat beim Mal davor auch KEIN Flackern gesehen
                        //Flackern wurde beim letzten Mal NICHT gesehen (ab 2tem Durchlauf)
                        if (this.SC2_gesehen_alt == true)
                        {
                            faktorStepSC2 = -faktorStepSC2 / 2;
                            neuerFaktorSC2 = faktorSC2 + faktorStepSC2;
                            if (ChangeContrast(LED_Gruppe, neuerFaktorSC2, logmessage))
                            {
                                faktorSC2 = neuerFaktorSC2;
                                bk7 = TesteAbbruch(faktorSC2, faktorStepSC2, false);
                            }
                            else
                            {
                                iSC2K100++;
                                Logmessage("Versuch Überschreitung: " + Convert.ToString(iSC2K100), false);
                            }

                        }
                        #endregion

                        #region Proband hat beim letzten Mal Flackern gesehen
                        //Flackern wurde beim letzten Mal gesehen
                        else
                        {
                            neuerFaktorSC2 = faktorSC2 + faktorStepSC2;
                            if (ChangeContrast(LED_Gruppe, neuerFaktorSC2, logmessage))
                            {
                                faktorSC2 = neuerFaktorSC2;
                            }
                            else
                            {
                                iSC2K100++;
                                Logmessage("Versuch Überschreitung: " + Convert.ToString(iSC2K100), false);
                            }

                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion

                if (bk7) Logmessage(logKontraste, false);

                if (SC1_7 == true && SC2_7 == true)
                {
                    this.OnAbbruch(new AbbruchEventArgs(""));
                }

                if (SC2_7 == false)
                {
                    if (iDurchlaufSC2 == 1) SC2_gesehen_alt = false; 
                    else SC2_gesehen_alt = SC2_gesehen;
                    SignalWiedergeben();
                    iDurchlaufSC2++;
                }
                else Randomisierung();

            }
            else
            {
                this.OnAbbruch(new AbbruchEventArgs(""));
            }
        }

        private bool ChangeContrast(string LED_Gruppe, int neuerFaktor, string logmessage)
        {
            double FaktorCFF;
            if (neuerFaktor < 0) { return (false); }
            if (neuerFaktor > maxFaktor) { return (false); }

            if (!testeCFF)
            {
                if (LED_Gruppe == "innen")
                {
                    Light4SightNG.IRChannel.Kontrast_100 = Kontrast_100[rot] * neuerFaktor / maxFaktor;
                    Light4SightNG.IGChannel.Kontrast_100 = Kontrast_100[gruen] * neuerFaktor / maxFaktor;
                    Light4SightNG.IBChannel.Kontrast_100 = Kontrast_100[blau] * neuerFaktor / maxFaktor;
                    Light4SightNG.ICChannel.Kontrast_100 = Kontrast_100[cyan] * neuerFaktor / maxFaktor;
                    logKontraste = logmessage + Light4SightNG.IRChannel.Kontrast_100 + ";" + Light4SightNG.IGChannel.Kontrast_100 + ";" + Light4SightNG.IBChannel.Kontrast_100 + ";" + Light4SightNG.ICChannel.Kontrast_100;
                }
                else
                {
                    Light4SightNG.ORChannel.Kontrast_100 = Kontrast_100[rot] * neuerFaktor / maxFaktor;
                    Light4SightNG.OGChannel.Kontrast_100 = Kontrast_100[gruen] * neuerFaktor / maxFaktor;
                    Light4SightNG.OBChannel.Kontrast_100 = Kontrast_100[blau] * neuerFaktor / maxFaktor;
                    Light4SightNG.OCChannel.Kontrast_100 = Kontrast_100[cyan] * neuerFaktor / maxFaktor;
                    logKontraste = logmessage + Light4SightNG.ORChannel.Kontrast_100 + ";" + Light4SightNG.OGChannel.Kontrast_100 + ";" + Light4SightNG.OBChannel.Kontrast_100 + ";" + Light4SightNG.OCChannel.Kontrast_100;
                }
            }
            else 
            {
                if (LED_Gruppe == "innen")
                {
                    Light4SightNG.IRChannel.Kontrast_100 = Kontrast_100[rot];
                    Light4SightNG.IGChannel.Kontrast_100 = Kontrast_100[gruen];
                    Light4SightNG.IBChannel.Kontrast_100 = Kontrast_100[blau];
                    Light4SightNG.ICChannel.Kontrast_100 = Kontrast_100[cyan];
                    FaktorCFF = (1 - Convert.ToDouble(neuerFaktor) / Convert.ToDouble(maxFaktor));
                    Light4SightNG.IRChannel.Frequenz = Convert.ToInt16(Frequenz_100[rot] * FaktorCFF);
                    Light4SightNG.IGChannel.Frequenz = Convert.ToInt16(Frequenz_100[gruen] * FaktorCFF);
                    Light4SightNG.IBChannel.Frequenz = Convert.ToInt16(Frequenz_100[blau] * FaktorCFF);
                    Light4SightNG.ICChannel.Frequenz = Convert.ToInt16(Frequenz_100[cyan] * FaktorCFF);
                    currentCFF = Convert.ToInt16(Frequenz_100[rot] * FaktorCFF);
                    logKontraste = logmessage + Light4SightNG.IRChannel.Frequenz + ";" + Light4SightNG.IGChannel.Frequenz + ";" + Light4SightNG.IBChannel.Frequenz + ";" + Light4SightNG.ICChannel.Frequenz;
                }
                else
                {
                    Light4SightNG.ORChannel.Kontrast_100 = Kontrast_100[rot];
                    Light4SightNG.OGChannel.Kontrast_100 = Kontrast_100[gruen];
                    Light4SightNG.OBChannel.Kontrast_100 = Kontrast_100[blau];
                    Light4SightNG.OCChannel.Kontrast_100 = Kontrast_100[cyan];
                    FaktorCFF = (1 - Convert.ToDouble(neuerFaktor) / Convert.ToDouble(maxFaktor));
                    Light4SightNG.ORChannel.Frequenz = Convert.ToInt16(Frequenz_100[rot] * FaktorCFF);
                    Light4SightNG.OGChannel.Frequenz = Convert.ToInt16(Frequenz_100[gruen] * FaktorCFF);
                    Light4SightNG.OBChannel.Frequenz = Convert.ToInt16(Frequenz_100[blau] * FaktorCFF);
                    Light4SightNG.OCChannel.Frequenz = Convert.ToInt16(Frequenz_100[cyan] * FaktorCFF);
                    currentCFF = Convert.ToInt16(Frequenz_100[rot] * FaktorCFF);
                    logKontraste = logmessage + Light4SightNG.ORChannel.Frequenz + ";" + Light4SightNG.OGChannel.Frequenz + ";" + Light4SightNG.OBChannel.Frequenz + ";" + Light4SightNG.OCChannel.Frequenz;
                }

            }
            return (true);
        }

        private void SignalWiedergeben()
        {
            AudioControl.InitWaveContainer();
            clSignalGeneration.CreateChannelArrays();
            clSignalGeneration.Untersuchungssignal();
            AudioControl.PlaySignal();
        }

        public void SignalStoppen()
        {
            AudioControl.StopSignal();
            Thread.Sleep(100);
            clSignalGeneration.ClearChannelArrays();
            Thread.Sleep(100);
        }

        public void gesehen_KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Y)
            {
                SignalStoppen();
                Thread.Sleep(10);
                if (SC1_aktiv == true)
                {
                    SC1_gesehen = true;
                    if (!testeCFF) Logmessage("Down:;gesehen;" + Light4SightNG.IRChannel.Kontrast_100 + ";" + Light4SightNG.IGChannel.Kontrast_100 + ";" + Light4SightNG.IBChannel.Kontrast_100 + ";" + Light4SightNG.ICChannel.Kontrast_100 + ";;" + Light4SightNG.ORChannel.Kontrast_100 + ";" + Light4SightNG.OGChannel.Kontrast_100 + ";" + Light4SightNG.OBChannel.Kontrast_100 + ";" + Light4SightNG.OCChannel.Kontrast_100, false);
                    else Logmessage("Down:;gesehen;" + Light4SightNG.IRChannel.Frequenz + ";" + Light4SightNG.IGChannel.Frequenz + ";" + Light4SightNG.IBChannel.Kontrast_100 + ";" + Light4SightNG.ICChannel.Frequenz + ";;" + Light4SightNG.ORChannel.Frequenz + ";" + Light4SightNG.OGChannel.Frequenz + ";" + Light4SightNG.OBChannel.Frequenz + ";" + Light4SightNG.OCChannel.Frequenz, false);
                }
                else
                {
                    SC2_gesehen = true;
                    if (!testeCFF) Logmessage("Up:;gesehen;" + Light4SightNG.IRChannel.Kontrast_100 + ";" + Light4SightNG.IGChannel.Kontrast_100 + ";" + Light4SightNG.IBChannel.Kontrast_100 + ";" + Light4SightNG.ICChannel.Kontrast_100 + ";;" + Light4SightNG.ORChannel.Kontrast_100 + ";" + Light4SightNG.OGChannel.Kontrast_100 + ";" + Light4SightNG.OBChannel.Kontrast_100 + ";" + Light4SightNG.OCChannel.Kontrast_100, false);
                    else Logmessage("Up:;gesehen;" + Light4SightNG.IRChannel.Frequenz + ";" + Light4SightNG.IGChannel.Frequenz + ";" + Light4SightNG.IBChannel.Kontrast_100 + ";" + Light4SightNG.ICChannel.Frequenz + ";;" + Light4SightNG.ORChannel.Frequenz + ";" + Light4SightNG.OGChannel.Frequenz + ";" + Light4SightNG.OBChannel.Frequenz + ";" + Light4SightNG.OCChannel.Frequenz, false);
                }
                Randomisierung();
            }
        }

        public void nichtgesehen_KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.M)
            {
                Thread.Sleep(10);
                SignalStoppen();
                if (SC1_aktiv == true)
                {
                    SC1_gesehen = false;
                    if (!testeCFF) Logmessage("Down:;nicht gesehen;" + Light4SightNG.IRChannel.Kontrast_100 + ";" + Light4SightNG.IGChannel.Kontrast_100 + ";" + Light4SightNG.IBChannel.Kontrast_100 + ";" + Light4SightNG.ICChannel.Kontrast_100 + ";;" + Light4SightNG.ORChannel.Kontrast_100 + ";" + Light4SightNG.OGChannel.Kontrast_100 + ";" + Light4SightNG.OBChannel.Kontrast_100 + ";" + Light4SightNG.OCChannel.Kontrast_100, false);
                    else Logmessage("Down:;nicht gesehen;" + Light4SightNG.IRChannel.Frequenz + ";" + Light4SightNG.IGChannel.Frequenz + ";" + Light4SightNG.IBChannel.Kontrast_100 + ";" + Light4SightNG.ICChannel.Frequenz + ";;" + Light4SightNG.ORChannel.Frequenz + ";" + Light4SightNG.OGChannel.Frequenz + ";" + Light4SightNG.OBChannel.Frequenz + ";" + Light4SightNG.OCChannel.Frequenz, false);
                }
                else
                {
                    SC2_gesehen = false;
                    if (!testeCFF) Logmessage("Up:;nicht gesehen;" + Light4SightNG.IRChannel.Kontrast_100 + ";" + Light4SightNG.IGChannel.Kontrast_100 + ";" + Light4SightNG.IBChannel.Kontrast_100 + ";" + Light4SightNG.ICChannel.Kontrast_100 + ";;" + Light4SightNG.ORChannel.Kontrast_100 + ";" + Light4SightNG.OGChannel.Kontrast_100 + ";" + Light4SightNG.OBChannel.Kontrast_100 + ";" + Light4SightNG.OCChannel.Kontrast_100, false);
                    else Logmessage("Up:;nicht gesehen;" + Light4SightNG.IRChannel.Frequenz + ";" + Light4SightNG.IGChannel.Frequenz + ";" + Light4SightNG.IBChannel.Kontrast_100 + ";" + Light4SightNG.ICChannel.Frequenz + ";;" + Light4SightNG.ORChannel.Frequenz + ";" + Light4SightNG.OGChannel.Frequenz + ";" + Light4SightNG.OBChannel.Frequenz + ";" + Light4SightNG.OCChannel.Frequenz, false);
                }
                Randomisierung();
            }
        }

        private void prepareLogFile()
        {
            string line2, line3, line4, line5, line6, line7, line8, line9, line10;
            Logmessage(";;Centerfield;;;;;Surroundfield;;",true);
            Logmessage(";;R;G;B;C;;R;G;B;C;",true);

            if (Light4SightNG.IRChannel.SignalAktiv == true)
            {
                line2 = ("Signal aktiv;;" + Light4SightNG.IRChannel.SignalAktiv.ToString() + ";");
                line3 = ("Signalform;;" + Light4SightNG.IRChannel.Signalform + ";");
                line4 = ("Helligkeit;;" + Light4SightNG.IRChannel.MittlereHelligkeit_cdm2.ToString() + ";");
                line5 = ("Frequenz;;" + Light4SightNG.IRChannel.Frequenz.ToString() + ";");
                line6 = ("Phase;;" + Light4SightNG.IRChannel.Phasenverschiebung.ToString() + ";");
                line7 = ("Kontrast SC1;;" + Light4SightNG.IRChannel.KonSC1_100.ToString() + ";");
                line8 = ("Kontrast SC2;;" + Light4SightNG.IRChannel.KonSC2_100.ToString() + ";");
                line9 = ("Delta Kontrast SC1;;" + Light4SightNG.IRChannel.SC1DeltaK_100.ToString() + ";");
                line10 = ("Delta Kontrast SC2;;" + Light4SightNG.IRChannel.SC2DeltaK_100.ToString() + ";");
            }
            else
            {
                line2 = ("Signal aktiv;;");
                line3 = ("Signalform;;;");
                line4 = ("Helligkeit;;;");
                line5 = ("Frequenz;;;");
                line6 = ("Phase;;;");
                line7 = ("Kontrast SC1;;;");
                line8 = ("Kontrast SC2;;;");
                line9 = ("Delta Kontrast SC1;;;");
                line10 = ("Delta Kontrast SC2;;;");
            }
              
            if (Light4SightNG.IGChannel.SignalAktiv == true)
            {
                line2 = (line2 + Light4SightNG.IGChannel.SignalAktiv.ToString() + ";");
                line3 = (line3 + Light4SightNG.IGChannel.Signalform + ";");
                line4 = (line4 + Light4SightNG.IGChannel.MittlereHelligkeit_cdm2.ToString() + ";");
                line5 = (line5 + Light4SightNG.IGChannel.Frequenz.ToString() + ";");
                line6 = (line6 + Light4SightNG.IGChannel.Phasenverschiebung.ToString() + ";");
                line7 = (line7 + Light4SightNG.IGChannel.KonSC1_100.ToString() + ";");
                line8 = (line8 + Light4SightNG.IGChannel.KonSC2_100.ToString() + ";");
                line9 = (line9 + Light4SightNG.IGChannel.SC1DeltaK_100.ToString() + ";");
                line10 = (line10 + Light4SightNG.IGChannel.SC2DeltaK_100.ToString() + ";");
            }
            else
            {
                line2 = (line2 + ";");
                line3 = (line3 + ";");
                line4 = (line4 + ";");
                line5 = (line5 + ";");
                line6 = (line6 + ";");
                line7 = (line7 + ";");
                line8 = (line8 + ";");
                line9 = (line9 + ";");
                line10 = (line10 + ";");
            }


            if (Light4SightNG.IBChannel.SignalAktiv == true)
            {
                line2 = (line2 + Light4SightNG.IBChannel.SignalAktiv.ToString() + ";");
                line3 = (line3 + Light4SightNG.IBChannel.Signalform + ";");
                line4 = (line4 + Light4SightNG.IBChannel.MittlereHelligkeit_cdm2.ToString() + ";");
                line5 = (line5 + Light4SightNG.IBChannel.Frequenz.ToString() + ";");
                line6 = (line6 + Light4SightNG.IBChannel.Phasenverschiebung.ToString() + ";");
                line7 = (line7 + Light4SightNG.IBChannel.KonSC1_100.ToString() + ";");
                line8 = (line8 + Light4SightNG.IBChannel.KonSC2_100.ToString() + ";");
                line9 = (line9 + Light4SightNG.IBChannel.SC1DeltaK_100.ToString() + ";");
                line10 = (line10 + Light4SightNG.IBChannel.SC2DeltaK_100.ToString() + ";");
            }
            else
            {
                line2 = (line2 + ";");
                line3 = (line3 + ";");
                line4 = (line4 + ";");
                line5 = (line5 + ";");
                line6 = (line6 + ";");
                line7 = (line7 + ";");
                line8 = (line8 + ";");
                line9 = (line9 + ";");
                line10 = (line10 + ";");
            }

            if (Light4SightNG.ICChannel.SignalAktiv == true)
            {
                line2 = (line2 + Light4SightNG.ICChannel.SignalAktiv.ToString() + ";;");
                line3 = (line3 + Light4SightNG.ICChannel.Signalform + ";;");
                line4 = (line4 + Light4SightNG.ICChannel.MittlereHelligkeit_cdm2.ToString() + ";;");
                line5 = (line5 + Light4SightNG.ICChannel.Frequenz.ToString() + ";;");
                line6 = (line6 + Light4SightNG.ICChannel.Phasenverschiebung.ToString() + ";;");
                line7 = (line7 + Light4SightNG.ICChannel.KonSC1_100.ToString() + ";;");
                line8 = (line8 + Light4SightNG.ICChannel.KonSC2_100.ToString() + ";;");
                line9 = (line9 + Light4SightNG.ICChannel.SC1DeltaK_100.ToString() + ";;");
                line10 = (line10 + Light4SightNG.ICChannel.SC2DeltaK_100.ToString() + ";;");
            }
            else
            {
                line2 = (line2 + ";;;");
                line3 = (line3 + ";;;");
                line4 = (line4 + ";;;");
                line5 = (line5 + ";;;");
                line6 = (line6 + ";;;");
                line7 = (line7 + ";;;");
                line8 = (line8 + ";;;");
                line9 = (line9 + ";;;");
                line10 = (line10 + ";;;");
            }

            if (Light4SightNG.ORChannel.SignalAktiv == true)
            {
                line2 = (line2 + Light4SightNG.ORChannel.SignalAktiv.ToString() + ";");
                line3 = (line3 + Light4SightNG.ORChannel.Signalform + ";");
                line4 = (line4 + Light4SightNG.ORChannel.MittlereHelligkeit_cdm2.ToString() + ";");
                line5 = (line5 + Light4SightNG.ORChannel.Frequenz.ToString() + ";");
                line6 = (line6 + Light4SightNG.ORChannel.Phasenverschiebung.ToString() + ";");
                line7 = (line7 + Light4SightNG.ORChannel.KonSC1_100.ToString() + ";");
                line8 = (line8 + Light4SightNG.ORChannel.KonSC2_100.ToString() + ";");
                line9 = (line9 + Light4SightNG.ORChannel.SC1DeltaK_100.ToString() + ";");
                line10 = (line10 + Light4SightNG.ORChannel.SC2DeltaK_100.ToString() + ";");
            }
            else
            {
                line2 = (line2 + ";");
                line3 = (line3 + ";");
                line4 = (line4 + ";");
                line5 = (line5 + ";");
                line6 = (line6 + ";");
                line7 = (line7 + ";");
                line8 = (line8 + ";");
                line9 = (line9 + ";");
                line10 = (line10 + ";");
            }
              
            if (Light4SightNG.OGChannel.SignalAktiv == true)
            {
                line2 = (line2 + Light4SightNG.OGChannel.SignalAktiv.ToString() + ";");
                line3 = (line3 + Light4SightNG.OGChannel.Signalform + ";");
                line4 = (line4 + Light4SightNG.OGChannel.MittlereHelligkeit_cdm2.ToString() + ";");
                line5 = (line5 + Light4SightNG.OGChannel.Frequenz.ToString() + ";");
                line6 = (line6 + Light4SightNG.OGChannel.Phasenverschiebung.ToString() + ";");
                line7 = (line7 + Light4SightNG.OGChannel.KonSC1_100.ToString() + ";");
                line8 = (line8 + Light4SightNG.OGChannel.KonSC2_100.ToString() + ";");
                line9 = (line9 + Light4SightNG.OGChannel.SC1DeltaK_100.ToString() + ";");
                line10 = (line10 + Light4SightNG.OGChannel.SC2DeltaK_100.ToString() + ";");
            }
            else
            {
                line2 = (line2 + ";");
                line3 = (line3 + ";");
                line4 = (line4 + ";");
                line5 = (line5 + ";");
                line6 = (line6 + ";");
                line7 = (line7 + ";");
                line8 = (line8 + ";");
                line9 = (line9 + ";");
                line10 = (line10 + ";");
            }

            if (Light4SightNG.OBChannel.SignalAktiv == true)
            {
                line2 = (line2 + Light4SightNG.OBChannel.SignalAktiv.ToString() + ";");
                line3 = (line3 + Light4SightNG.OBChannel.Signalform + ";");
                line4 = (line4 + Light4SightNG.OBChannel.MittlereHelligkeit_cdm2.ToString() + ";");
                line5 = (line5 + Light4SightNG.OBChannel.Frequenz.ToString() + ";");
                line6 = (line6 + Light4SightNG.OBChannel.Phasenverschiebung.ToString() + ";");
                line7 = (line7 + Light4SightNG.OBChannel.KonSC1_100.ToString() + ";");
                line8 = (line8 + Light4SightNG.OBChannel.KonSC2_100.ToString() + ";");
                line9 = (line9 + Light4SightNG.OBChannel.SC1DeltaK_100.ToString() + ";");
                line10 = (line10 + Light4SightNG.OBChannel.SC2DeltaK_100.ToString() + ";");
            }
            else
            {
                line2 = (line2 + ";");
                line3 = (line3 + ";");
                line4 = (line4 + ";");
                line5 = (line5 + ";");
                line6 = (line6 + ";");
                line7 = (line7 + ";");
                line8 = (line8 + ";");
                line9 = (line9 + ";");
                line10 = (line10 + ";");
            }

            if (Light4SightNG.OCChannel.SignalAktiv == true)
            {
                line2 = (line2 + Light4SightNG.OCChannel.SignalAktiv.ToString() + ";");
                line3 = (line3 + Light4SightNG.OCChannel.Signalform + ";");
                line4 = (line4 + Light4SightNG.OCChannel.MittlereHelligkeit_cdm2.ToString() + ";");
                line5 = (line5 + Light4SightNG.OCChannel.Frequenz.ToString() + ";");
                line6 = (line6 + Light4SightNG.OCChannel.Phasenverschiebung.ToString() + ";");
                line7 = (line7 + Light4SightNG.OCChannel.KonSC1_100.ToString() + ";");
                line8 = (line8 + Light4SightNG.OCChannel.KonSC2_100.ToString() + ";");
                line9 = (line9 + Light4SightNG.OCChannel.SC1DeltaK_100.ToString() + ";");
                line10 = (line10 + Light4SightNG.OCChannel.SC2DeltaK_100.ToString() + ";");
            }
            else
            {
                line2 = (line2 + ";");
                line3 = (line3 + ";");
                line4 = (line4 + ";");
                line5 = (line5 + ";");
                line6 = (line6 + ";");
                line7 = (line7 + ";");
                line8 = (line8 + ";");
                line9 = (line9 + ";");
                line10 = (line10 + ";");
            }

            Logmessage(line2, true);
            Logmessage(line3,true);
            Logmessage(line4,true);
            Logmessage(line5, true);
            Logmessage(line6, true);
            Logmessage(line7, true);
            Logmessage(line8, true);
            Logmessage(line9, true);
            Logmessage(line10, true);
            Logmessage("", true);
        }

        private void Logmessage(string text, bool header)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is Light4SightNG)
                {
                    (frm as Light4SightNG).LogFile(text, header);
                }
            }
        }

        private void Randomisierung()
        {
            if (SC1_7 && SC2_7)
            {
                this.OnAbbruch(new AbbruchEventArgs(""));
            }

            if (clGlobals.Staircase == "up" && !SC2_7)
            {
                StaircaseUp();
                SC1_aktiv = false;
            }

            if (clGlobals.Staircase == "down" && !SC1_7)
            {
                StaircaseDown();
                SC1_aktiv = true;
            }

            if (clGlobals.Staircase == "beide")
            {
                if (SC1_7 && SC2_7)
                {
                    this.OnAbbruch(new AbbruchEventArgs(""));
                }

                double tmpRand = dRand.NextDouble();

                if ((tmpRand >= 0.5 || SC2_7) && !SC1_7)
                {
                    SC1_aktiv = true;
                    StaircaseDown();//sc1
                }
                else if ((tmpRand < 0.5 || SC1_7) && !SC2_7)
                {
                    SC1_aktiv = false;
                    StaircaseUp();//sc2
                }
            }
        }

        private void InitValuesCFF(string LED_Gruppe)
        {
            faktorSC1 = maxFaktor - maxFaktor / 128;
            faktorSC2 = 0;
            faktorStepSC1 = -faktorSC1 / 5;
            faktorStepSC2 = faktorSC1 / 5;
            Frequenz_100[0] = 100;
            Frequenz_100[1] = 100;
            Frequenz_100[2] = 100;
            Frequenz_100[3] = 100;
            Frequenz_100[4] = 100;
            Frequenz_100[5] = 100;
            Frequenz_100[6] = 100;
            Frequenz_100[7] = 100;

        }

        private void InitValuesContrast(string LED_Gruppe)
        {
            faktorSC1 = maxFaktor;
            faktorStepSC1 = -maxFaktor / 5;
            faktorSC2 = 0;
            faktorStepSC2 = maxFaktor / 5;
        }

        private bool TesteAbbruch(int faktor, int step, bool SC1)
        {
            if (testeCFF) faktor = maxFaktor - faktor;
            if (Math.Abs(step) < (faktor / 7))
            {
                if (testeCFF)
                {
                    if (finalCFF == -1) finalCFF = currentCFF;
                    else finalCFF = (finalCFF + currentCFF) / 2;
                }
                if (SC1) SC1_7 = true; else SC2_7 = true;
                return (true);
            }
            else
            {
                return (false);
            }
        }

    }
}
