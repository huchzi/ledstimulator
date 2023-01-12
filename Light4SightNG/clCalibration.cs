using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.Windows.Forms;


namespace Light4SightNG
{
    internal static class clCalibration
    {
        
    
        private static double[,] daMesswerte = new double[8,6];

        private static double[] daMesspunkte = new double[6] {0.02,0.04,0.1,0.4,0.7,1.0};
        
        private static int iMesspunkt, iKanal;

        private static double[] dSteigungKanal = new double[8];
        private static double[] dySchnittpunkt = new double[8];
        private static double dxy1, dxy2, dxy3, dxy4, dxy5, dxy6, dxy7, dxy8;

        private static clAudioControl AudioControl;
        public static LogWriter calibrationfile;

        private static Thread m_CalThread = null;

        public static void startcal()
        {
            ThreadStart CalThreadStart = new ThreadStart(Calibration);
            m_CalThread = new Thread(CalThreadStart);
            m_CalThread.Start();
        }

        public static void Calibration()
        {
            AudioControl = new clAudioControl();

            //Schleife für die Kanäle
            for (iKanal = 0; iKanal <= 7; iKanal++)
            {
                //Schleife für die Amplituden
                for (iMesspunkt = 0; iMesspunkt < daMesspunkte.Length; iMesspunkt++)
                {
                    AudioControl.InitWaveContainer();
                    clSignalGeneration.CreateChannelArrays();
                    //clSignalGeneration.KalibrierungsSignal(iKanal, daMesspunkte[iMesspunkt]);
                    clSignalGeneration.WriteCalChannel(iKanal, daMesspunkte[iMesspunkt]);
                    AudioControl.PlaySignal();
                    clGlobals.flagSignalWiedergabe = true;

                    while (clGlobals.flagSignalWiedergabe==true)
                    {
                        Thread.Sleep(100);
                    }
                    //clGlobals.threadtest = "Kanal " + (iKanal + 1) + ": Messung " + (iMesspunkt + 1) + "von 16";
                    //CalHinweisChanger(iKanal, iMesspunkt);
                    /*
                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm is Light4SightNG)
                            (frm as Light4SightNG).lblCalHinweis.Text = "Kanal "+ (iKanal+1) + ": Messung "+ (iMesspunkt+1) + "von 16";
                    }*/
                    
                    daMesswerte[iKanal,iMesspunkt] = clGlobals.KalibrierungsMesswert;
                    AudioControl.StopSignal();
                    clSignalGeneration.ClearChannelArrays();
                }
            }
            LineareRegression();
        }

      /*  private static void CalHinweisChanger(int iKanal, int iMesspunkt)
        {
            //string strtext = "Kanal " + (iKanal + 1) + ": Messung " + (iMesspunkt + 1) + "von 16";
            /*foreach (Form frm in Application.OpenForms)
            {
                if (frm is Light4SightNG)
                    (frm as Light4SightNG).Invoke(Light4SightNG.ActiveForm.Invoke( CalHinweisAendern,strtext);
            }*/
            //Light4SightNG.ActiveForm.BeginInvoke(CalHinweisAendern, strtext);
       /* }*/

        private static void LineareRegression()
        {	//Funktion für die lineare Regression.
            //Berechnet aus den eingelesenen Messwerten und den vorgegebenen Messpunkten
            //für jeden Kanal die Parameter der Ausgleichsgerade (Steigung und Schnittpunkt mit der Y-Achse

            //Variablen für die Berechnung deklarieren
            double dxDurchschnittMesspunkte = 0, dyDurchschnittKanal1 = 0, dyDurchschnittKanal2 = 0, dyDurchschnittKanal3 = 0, dyDurchschnittKanal4 = 0;
            double dyDurchschnittKanal5 = 0, dyDurchschnittKanal6 = 0, dyDurchschnittKanal7 = 0, dyDurchschnittKanal8 = 0;
            double dxQuadrat = 0;
            double dsk1, dsk2, dsk3, dsk4, dsk5, dsk6, dsk7, dsk8;
   
            //Durchschnitte der einzelnen Kanäle (dyDurchschnittKanalX) und der Messpunkte (dxDurschnittMesspunkte) berechnen
            //dxDurchschnittMesspunkte    = daMesspunkte.Average();
            //dyDurchschnittKanal1        = daMesswerte
            for (int i = 0; i < 6; i++)
            {
                dxDurchschnittMesspunkte = dxDurchschnittMesspunkte + daMesspunkte[i];
                dyDurchschnittKanal1 = dyDurchschnittKanal1 + daMesswerte[0,i];
                dyDurchschnittKanal2 = dyDurchschnittKanal2 + daMesswerte[1,i];
                dyDurchschnittKanal3 = dyDurchschnittKanal3 + daMesswerte[2,i];
                dyDurchschnittKanal4 = dyDurchschnittKanal4 + daMesswerte[3,i];
                dyDurchschnittKanal5 = dyDurchschnittKanal5 + daMesswerte[4,i];
                dyDurchschnittKanal6 = dyDurchschnittKanal6 + daMesswerte[5,i];
                dyDurchschnittKanal7 = dyDurchschnittKanal7 + daMesswerte[6,i];
                dyDurchschnittKanal8 = dyDurchschnittKanal8 + daMesswerte[7,i];
            }
            dxDurchschnittMesspunkte = dxDurchschnittMesspunkte / 6;
            dyDurchschnittKanal1 = dyDurchschnittKanal1 / 6;
            dyDurchschnittKanal2 = dyDurchschnittKanal2 / 6;
            dyDurchschnittKanal3 = dyDurchschnittKanal3 / 6;
            dyDurchschnittKanal4 = dyDurchschnittKanal4 / 6;
            dyDurchschnittKanal5 = dyDurchschnittKanal5 / 6;
            dyDurchschnittKanal6 = dyDurchschnittKanal6 / 6;
            dyDurchschnittKanal7 = dyDurchschnittKanal7 / 6;
            dyDurchschnittKanal8 = dyDurchschnittKanal8 / 6;
            

            // Summen der Abweichungsquadrate berechnen
            for (int i = 0; i < 6; i++)
            {
                double tmpx = daMesspunkte[i] - dxDurchschnittMesspunkte, tmpy1 = daMesswerte[0, i] - dyDurchschnittKanal1, tmpy2 = daMesswerte[1, i] - dyDurchschnittKanal2;
                double tmpy3 = daMesswerte[2, i] - dyDurchschnittKanal3, tmpy4 = daMesswerte[3, i] - dyDurchschnittKanal4, tmpy5 = daMesswerte[4, i] - dyDurchschnittKanal5;
                double tmpy6 = daMesswerte[5, i] - dyDurchschnittKanal6, tmpy7 = daMesswerte[6, i] - dyDurchschnittKanal7, tmpy8 = daMesswerte[7, i] - dyDurchschnittKanal8;
                dxQuadrat = dxQuadrat + tmpx * tmpx;
                dxy1 = dxy1 + tmpx * tmpy1;
                dxy2 = dxy2 + tmpx * tmpy2;
                dxy3 = dxy3 + tmpx * tmpy3;
                dxy4 = dxy4 + tmpx * tmpy4;
                dxy5 = dxy5 + tmpx * tmpy5;
                dxy6 = dxy6 + tmpx * tmpy6;
                dxy7 = dxy7 + tmpx * tmpy7;
                dxy8 = dxy8 + tmpx * tmpy8;
            }

            // Gültigkeit der Steigung überprüfen (auf unendlich)
            //Debug. Assert(Math.Abs(dxQuadrat) != 0);
            if (dxQuadrat > 0)
            {
                //Steigungen berechnen
                dSteigungKanal[0] = Math.Round(dsk1 = dxy1 / dxQuadrat, 4);
                dSteigungKanal[1] = Math.Round(dsk2 = dxy2 / dxQuadrat, 4);
                dSteigungKanal[2] = Math.Round(dsk3 = dxy3 / dxQuadrat, 4);
                dSteigungKanal[3] = Math.Round(dsk4 = dxy4 / dxQuadrat, 4);
                dSteigungKanal[4] = Math.Round(dsk5 = dxy5 / dxQuadrat, 4);
                dSteigungKanal[5] = Math.Round(dsk6 = dxy6 / dxQuadrat, 4);
                dSteigungKanal[6] = Math.Round(dsk7 = dxy7 / dxQuadrat, 4);
                dSteigungKanal[7] = Math.Round(dsk8 = dxy8 / dxQuadrat, 4);

                //Schnittpunkte mit den Y-Achsen berechnen
                dySchnittpunkt[0] = Math.Round(dsk1 = dyDurchschnittKanal1 - dSteigungKanal[0] * dxDurchschnittMesspunkte, 4);
                dySchnittpunkt[1] = Math.Round(dsk2 = dyDurchschnittKanal2 - dSteigungKanal[1] * dxDurchschnittMesspunkte, 4);
                dySchnittpunkt[2] = Math.Round(dsk3 = dyDurchschnittKanal3 - dSteigungKanal[2] * dxDurchschnittMesspunkte, 4);
                dySchnittpunkt[3] = Math.Round(dsk4 = dyDurchschnittKanal4 - dSteigungKanal[3] * dxDurchschnittMesspunkte, 4);
                dySchnittpunkt[4] = Math.Round(dsk5 = dyDurchschnittKanal5 - dSteigungKanal[4] * dxDurchschnittMesspunkte, 4);
                dySchnittpunkt[5] = Math.Round(dsk6 = dyDurchschnittKanal6 - dSteigungKanal[5] * dxDurchschnittMesspunkte, 4);
                dySchnittpunkt[6] = Math.Round(dsk7 = dyDurchschnittKanal7 - dSteigungKanal[6] * dxDurchschnittMesspunkte, 4);
                dySchnittpunkt[7] = Math.Round(dsk8 = dyDurchschnittKanal8 - dSteigungKanal[7] * dxDurchschnittMesspunkte, 4);
            }
            else
            {
                //fehlermeldung generieren
            }

            calibrationfile = new LogWriter(".\\calibrationdata.csv", false);
            string strtmp1="", strtmp2="";
            for (int i=0; i <= 7; i++)
            {
                strtmp1 = strtmp1 + dSteigungKanal[i] + ";";
                strtmp2 = strtmp2 + dySchnittpunkt[i] + ";";
            }
            calibrationfile.add(strtmp1);
            calibrationfile.add(strtmp2);
            calibrationfile.close();


        }

    }
}
