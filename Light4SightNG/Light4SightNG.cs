using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Light4SightNG
{
    public partial class Light4SightNG : Form
    {
        //Kanalobjekkte erzeugen
        public static clSignalDescription IRChannel = new clSignalDescription();
        public static clSignalDescription IGChannel = new clSignalDescription();
        public static clSignalDescription IBChannel = new clSignalDescription();
        public static clSignalDescription ICChannel = new clSignalDescription();
        public static clSignalDescription ORChannel = new clSignalDescription();
        public static clSignalDescription OGChannel = new clSignalDescription();
        public static clSignalDescription OBChannel = new clSignalDescription();
        public static clSignalDescription OCChannel = new clSignalDescription();

        public static int cff;

        private StdStrategie stdStrategie;

        private List<clSignalDescription> channels = new List<clSignalDescription>();

        //Objekt für die Audioschnittstelle. Dient der Steuerung der Wiedergabe(Start,Stop,Puffer)
        private clAudioControl AudioControl = new clAudioControl();

        //private Thread m_CalHinweisThread = null;

        //Objekte für Logging und Debugging

        public LogWriter logfiletmp;//Wird durch die Funktion LogFile an die Namenskonvention von DebugFile angepasst
        //public LogWriter DebugFile = new LogWriter("debugdata.txt", true);

        public Light4SightNG()
        {
            InitializeComponent();

            #region Kalibrierung einlesen und MaxWerte berechnen/anzeigen
            if (clGlobals.KalibrierungsdatenLesen() == 0)
            {
                this.lblIRMHMax.Text = clGlobals.dMaxMH(0).ToString();
                IRChannel.MaxMHCal = clGlobals.dMaxMH(0);
                IRChannel.ParameterPolynom(clGlobals.poly4(0), clGlobals.poly3(0), clGlobals.poly2(0), clGlobals.poly1(0), clGlobals.intercept(0));

                this.lblIGMHMax.Text = clGlobals.dMaxMH(1).ToString();
                IGChannel.MaxMHCal = clGlobals.dMaxMH(1);
                IGChannel.ParameterPolynom(clGlobals.poly4(1), clGlobals.poly3(1), clGlobals.poly2(1), clGlobals.poly1(1), clGlobals.intercept(1));

                this.lblIBMHMax.Text = clGlobals.dMaxMH(2).ToString();
                IBChannel.MaxMHCal = clGlobals.dMaxMH(2);
                IBChannel.ParameterPolynom(clGlobals.poly4(2), clGlobals.poly3(2), clGlobals.poly2(2), clGlobals.poly1(2), clGlobals.intercept(2));

                this.lblICMHMax.Text = clGlobals.dMaxMH(3).ToString();
                ICChannel.MaxMHCal = clGlobals.dMaxMH(3);
                ICChannel.ParameterPolynom(clGlobals.poly4(3), clGlobals.poly3(3), clGlobals.poly2(3), clGlobals.poly1(3), clGlobals.intercept(3));

                this.lblORMHMax.Text = clGlobals.dMaxMH(4).ToString();
                ORChannel.MaxMHCal = clGlobals.dMaxMH(4);
                ORChannel.ParameterPolynom(clGlobals.poly4(4), clGlobals.poly3(4), clGlobals.poly2(4), clGlobals.poly1(4), clGlobals.intercept(4));

                this.lblOGMHMax.Text = clGlobals.dMaxMH(5).ToString();
                OGChannel.MaxMHCal = clGlobals.dMaxMH(5);
                OGChannel.ParameterPolynom(clGlobals.poly4(5), clGlobals.poly3(5), clGlobals.poly2(5), clGlobals.poly1(5), clGlobals.intercept(5));

                this.lblOBMHMax.Text = clGlobals.dMaxMH(6).ToString();
                OBChannel.MaxMHCal = clGlobals.dMaxMH(6);
                OBChannel.ParameterPolynom(clGlobals.poly4(6), clGlobals.poly3(6), clGlobals.poly2(6), clGlobals.poly1(6), clGlobals.intercept(6));

                this.lblOCMHMax.Text = clGlobals.dMaxMH(7).ToString();
                OCChannel.MaxMHCal = clGlobals.dMaxMH(7);
                OCChannel.ParameterPolynom(clGlobals.poly4(7), clGlobals.poly3(7), clGlobals.poly2(7), clGlobals.poly1(7), clGlobals.intercept(7));
            }
            else
            {
                this.lblIRMHMax.Text = "Kalibrierung durchführen!";
                this.lblIGMHMax.Text = "Kalibrierung durchführen!";
                this.lblIBMHMax.Text = "Kalibrierung durchführen!";
                this.lblICMHMax.Text = "Kalibrierung durchführen!";
                this.lblORMHMax.Text = "Kalibrierung durchführen!";
                this.lblOGMHMax.Text = "Kalibrierung durchführen!";
                this.lblOBMHMax.Text = "Kalibrierung durchführen!";
                this.lblOCMHMax.Text = "Kalibrierung durchführen!";
                btnUntersuchungAbbrechenActive(false);
                btnUntersuchungStartenActive(false);
            }
            #endregion

            channels.Add(IRChannel);
            channels.Add(IGChannel);
            channels.Add(IBChannel);
            channels.Add(ICChannel);
            channels.Add(ORChannel);
            channels.Add(OGChannel);
            channels.Add(OBChannel);
            channels.Add(OCChannel);

            treeView1.ExpandAll();
            this.btnUntersuchungAbbrechenActive(false);

        }

        public void LogFile(string text, bool header)
        {
            if (clGlobals.flagDebugLog == true)
            {
                logfiletmp.add(text);
                //DebugFile.add("LOGFILE: " + logfiletmp.add(text));
                this.tbUntersuchungsVerlauf.AppendText("\r\n" + text);
            }
            else
            {
                logfiletmp.add(text);
                if (!header)
                {
                    this.tbUntersuchungsVerlauf.AppendText("\r\n" + text);
                }
            }
        }

        private void closeAllPanels()
        {
            this.pnlOuterRed.Hide();
            this.pnlOuterRed.Visible = false;
            this.pnlOuterGreen.Hide();
            this.pnlOuterGreen.Visible = false;
            this.pnlOuterBlue.Hide();
            this.pnlOuterBlue.Visible = false;
            this.pnlOuterCyan.Hide();
            this.pnlOuterCyan.Visible = false;
            this.pnlInnerRed.Hide();
            this.pnlInnerRed.Visible = false;
            this.pnlInnerGreen.Hide();
            this.pnlInnerGreen.Visible = false;
            this.pnlInnerBlue.Hide();
            this.pnlInnerBlue.Visible = false;
            this.pnlInnerCyan.Hide();
            this.pnlInnerCyan.Visible = false;
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            #region Panel Control
            switch (e.Node.Name.ToString())
            {
                case "tnOuterRed":
                    {
                        if (this.pnlOuterRed.Visible == false)
                        {
                            this.closeAllPanels();
                            this.btnUntersuchungStartenActive(true);
                            this.pnlOuterRed.Show();
                            this.pnlOuterRed.Visible = true;
                        }
                        break;
                    }
                case "tnOuterGreen":
                    {
                        if (this.pnlOuterGreen.Visible == false)
                        {
                            this.closeAllPanels();
                            this.btnUntersuchungStartenActive(true);
                            this.pnlOuterGreen.Show();
                            this.pnlOuterGreen.Visible = true;
                        }
                        break;
                    }
                case "tnOuterBlue":
                    {
                        if (this.pnlOuterBlue.Visible == false)
                        {
                            this.closeAllPanels();
                            this.btnUntersuchungStartenActive(true);
                            this.pnlOuterBlue.Show();
                            this.pnlOuterBlue.Visible = true;
                        }
                        break;
                    }
                case "tnOuterCyan":
                    {
                        if (this.pnlOuterCyan.Visible == false)
                        {
                            this.closeAllPanels();
                            this.btnUntersuchungStartenActive(true);
                            this.pnlOuterCyan.Show();
                            this.pnlOuterCyan.Visible = true;
                        }
                        break;
                    }
                case "tnInnerRed":
                    {
                        if (this.pnlInnerRed.Visible == false)
                        {
                            this.closeAllPanels();
                            this.btnUntersuchungStartenActive(true);
                            this.pnlInnerRed.Show();
                            this.pnlInnerRed.Visible = true;
                        }
                        break;
                    }
                case "tnInnerGreen":
                    {
                        if (this.pnlInnerGreen.Visible == false)
                        {
                            this.closeAllPanels();
                            this.btnUntersuchungStartenActive(true);
                            this.pnlInnerGreen.Show();
                            this.pnlInnerGreen.Visible = true;
                        }
                        break;
                    }
                case "tnInnerBlue":
                    {
                        if (this.pnlInnerBlue.Visible == false)
                        {
                            this.closeAllPanels();
                            this.btnUntersuchungStartenActive(true);
                            this.pnlInnerBlue.Show();
                            this.pnlInnerBlue.Visible = true;
                        }
                        break;
                    }
                case "tnInnerCyan":
                    {
                        if (this.pnlInnerCyan.Visible == false)
                        {
                            this.closeAllPanels();
                            this.btnUntersuchungStartenActive(true);
                            this.pnlInnerCyan.Show();
                            this.pnlInnerCyan.Visible = true;
                        }
                        break;
                    }

                default:
                    this.closeAllPanels();
                    break;
            }
            #endregion


        }

        private void SignalEigenschaftenEinlesen()
        {
            IRChannel.SignalAktiv = cbAktivIR.Checked;
            if (cbAktivIR.Checked)
            {
                IRChannel.Signalform = cbSigFormIR.Text;
                IRChannel.Frequenz = int.Parse(tbFreqIR.Text);
                IRChannel.KonSC1_100 = double.Parse(tbKonIRSC1.Text.ToString());
                IRChannel.KonSC2_100 = double.Parse(tbKonIRSC2.Text.ToString());
                IRChannel.SC1DeltaK_100 = double.Parse(tbIRSC1DeltaK.Text.ToString());
                IRChannel.SC2DeltaK_100 = double.Parse(tbIRSC2DeltaK.Text.ToString());
                IRChannel.MittlereHelligkeit_cdm2 = double.Parse(tbMHIR.Text);
                IRChannel.Phasenverschiebung = int.Parse(tbPhasVerschIR.Text);
            }

            IGChannel.SignalAktiv = cbAktivIG.Checked;
            if (cbAktivIG.Checked)
            {
                IGChannel.Signalform = cbSigFormIG.Text;
                IGChannel.Frequenz = int.Parse(tbFreqIG.Text);
                IGChannel.KonSC1_100 = double.Parse(tbKonIGSC1.Text.ToString());
                IGChannel.KonSC2_100 = double.Parse(tbKonIGSC2.Text.ToString());
                IGChannel.SC1DeltaK_100 = double.Parse(tbIGSC1DeltaK.Text.ToString());
                IGChannel.SC2DeltaK_100 = double.Parse(tbIGSC2DeltaK.Text.ToString());
                IGChannel.MittlereHelligkeit_cdm2 = double.Parse(tbMHIG.Text);
                IGChannel.Phasenverschiebung = int.Parse(tbPhasVerschIG.Text);
            }

            IBChannel.SignalAktiv = cbAktivIB.Checked;
            if (cbAktivIB.Checked)
            {
                IBChannel.Signalform = cbSigFormIB.Text;
                IBChannel.Frequenz = int.Parse(tbFreqIB.Text);
                IBChannel.KonSC1_100 = double.Parse(tbKonIBSC1.Text.ToString());
                IBChannel.KonSC2_100 = double.Parse(tbKonIBSC2.Text.ToString());
                IBChannel.SC1DeltaK_100 = double.Parse(tbIBSC1DeltaK.Text.ToString());
                IBChannel.SC2DeltaK_100 = double.Parse(tbIBSC2DeltaK.Text.ToString());
                IBChannel.MittlereHelligkeit_cdm2 = double.Parse(tbMHIB.Text);
                IBChannel.Phasenverschiebung = int.Parse(tbPhasVerschIB.Text);
            }

            ICChannel.SignalAktiv = cbAktivIC.Checked;
            if (cbAktivIC.Checked)
            {
                ICChannel.Signalform = cbSigFormIC.Text;
                ICChannel.Frequenz = int.Parse(tbFreqIC.Text);
                ICChannel.KonSC1_100 = double.Parse(tbKonICSC1.Text.ToString());
                ICChannel.KonSC2_100 = double.Parse(tbKonICSC2.Text.ToString());
                ICChannel.SC1DeltaK_100 = double.Parse(tbICSC1DeltaK.Text.ToString());
                ICChannel.SC2DeltaK_100 = double.Parse(tbICSC2DeltaK.Text.ToString());
                ICChannel.MittlereHelligkeit_cdm2 = double.Parse(tbMHIC.Text);
                ICChannel.Phasenverschiebung = int.Parse(tbPhasVerschIC.Text);
            }

            ORChannel.SignalAktiv = cbAktivOR.Checked;
            if (cbAktivOR.Checked)
            {
                ORChannel.Signalform = cbSigFormOR.Text;
                ORChannel.Frequenz = int.Parse(tbFreqOR.Text);
                ORChannel.KonSC1_100 = double.Parse(tbKonORSC1.Text.ToString());
                ORChannel.KonSC2_100 = double.Parse(tbKonORSC2.Text.ToString());
                ORChannel.SC1DeltaK_100 = double.Parse(tbORSC1DeltaK.Text.ToString());
                ORChannel.SC2DeltaK_100 = double.Parse(tbORSC2DeltaK.Text.ToString());
                ORChannel.MittlereHelligkeit_cdm2 = double.Parse(tbMHOR.Text);
                ORChannel.Phasenverschiebung = int.Parse(tbPhasVerschOR.Text);
            }

            OGChannel.SignalAktiv = cbAktivOG.Checked;
            if (cbAktivOG.Checked)
            {
                OGChannel.Signalform = cbSigFormOG.Text;
                OGChannel.Frequenz = int.Parse(tbFreqOG.Text);
                OGChannel.KonSC1_100 = double.Parse(tbKonOGSC1.Text.ToString());
                OGChannel.KonSC2_100 = double.Parse(tbKonOGSC2.Text.ToString());
                OGChannel.SC1DeltaK_100 = double.Parse(tbOGSC1DeltaK.Text.ToString());
                OGChannel.SC2DeltaK_100 = double.Parse(tbOGSC2DeltaK.Text.ToString());
                OGChannel.MittlereHelligkeit_cdm2 = double.Parse(tbMHOG.Text);
                OGChannel.Phasenverschiebung = int.Parse(tbPhasVerschOG.Text);
            }

            OBChannel.SignalAktiv = cbAktivOB.Checked;
            if (cbAktivOB.Checked)
            {
                OBChannel.Signalform = cbSigFormOB.Text;
                OBChannel.Frequenz = int.Parse(tbFreqOB.Text);
                OBChannel.KonSC1_100 = double.Parse(tbKonOBSC1.Text.ToString());
                OBChannel.KonSC2_100 = double.Parse(tbKonOBSC2.Text.ToString());
                OBChannel.SC1DeltaK_100 = double.Parse(tbOBSC1DeltaK.Text.ToString());
                OBChannel.SC2DeltaK_100 = double.Parse(tbOBSC2DeltaK.Text.ToString());
                OBChannel.MittlereHelligkeit_cdm2 = double.Parse(tbMHOB.Text);
                OBChannel.Phasenverschiebung = int.Parse(tbPhasVerschOB.Text);
            }

            OCChannel.SignalAktiv = cbAktivOC.Checked;
            if (cbAktivOC.Checked)
            {
                OCChannel.Signalform = cbSigFormOC.Text;
                OCChannel.Frequenz = int.Parse(tbFreqOC.Text);
                OCChannel.KonSC1_100 = double.Parse(tbKonOCSC1.Text.ToString());
                OCChannel.KonSC2_100 = double.Parse(tbKonOCSC2.Text.ToString());
                OCChannel.SC1DeltaK_100 = double.Parse(tbOCSC1DeltaK.Text.ToString());
                OCChannel.SC2DeltaK_100 = double.Parse(tbOCSC2DeltaK.Text.ToString());
                OCChannel.MittlereHelligkeit_cdm2 = double.Parse(tbMHOC.Text);
                OCChannel.Phasenverschiebung = int.Parse(tbPhasVerschOC.Text);
            }


        }

        private void btnUntersuchungStarten_Click(object sender, EventArgs e)
        {
            if (CheckProband())
            {
                DateTime time = DateTime.Now;
                string format = "yyyy-MM-dd_HHmmss";

                logfiletmp = new LogWriter(tbProbandenNummer.Text.ToString() + "_" + this.cbAugenseite.Text.ToString() + "_" + time.ToString(format) + ".txt", false);
                //stdStrategie = new StdStrategie();
                //stdStrategie.abbruch += new EventHandler<AbbruchEventArgs>(stdStrategie_abbruch);
                this.SignalEigenschaftenEinlesen();

                clGlobals.flagUntersuchunglaeuft = true;
                this.btnUntersuchungAbbrechen.Enabled = true;
                this.btnUntersuchungStarten.Enabled = false;
                this.tbUntersuchungsVerlauf.Clear();
                this.tbUntersuchungsVerlauf.Visible = true;
                this.tbProbandenNummer.Enabled = false;
                this.cbAugenseite.Enabled = false;
                this.treeView1.Enabled = false;
                this.KeyPreview = true;

                stdStrategie = new StdStrategie();
                stdStrategie.abbruch += new EventHandler<AbbruchEventArgs>(stdStrategie_abbruch);
                stdStrategie.StartStdStrategie();


            }
        }

        private bool CheckProband()
        {
            if (tbProbandenNummer.Text.ToString() == "" || this.cbAugenseite.Text.ToString() == "")
            {
                MessageBox.Show("Probandendaten nicht korrekt" + "\nProbandennummer: " + tbProbandenNummer.Text.ToString() + "\nAugenseite: " + this.cbAugenseite.Text.ToString());
                return false;
            }
            else return true;
        }

        void stdStrategie_abbruch(object sender, AbbruchEventArgs e)
        {
            this.KeyPreview = false;
            if (stdStrategie != null)
            {
                stdStrategie.SignalStoppen();
                Thread.Sleep(100);
                stdStrategie = null;
            }
            this.logfiletmp.close();
            this.treeView1.Enabled = true;
            this.btnUntersuchungAbbrechen.Enabled = false;
            this.btnUntersuchungStarten.Enabled = true;
            this.tbProbandenNummer.Enabled = true;
            this.cbAugenseite.Enabled = true;
        }

        private void btnUntersuchungAbbrechen_Click(object sender, EventArgs e)
        {
            Thread.Sleep(100);
            AbbruchEventArgs mye = new AbbruchEventArgs("");
            stdStrategie_abbruch(this, mye);
        }

        public void btnUntersuchungStartenActive(bool bstatus)
        {
            if (bstatus == true)
                this.btnUntersuchungStarten.Enabled = true;
            else
                this.btnUntersuchungStarten.Enabled = false;
        }

        public void btnUntersuchungAbbrechenActive(bool bstatus)
        {
            if (bstatus == true)
                this.btnUntersuchungAbbrechen.Enabled = true;
            else
                this.btnUntersuchungAbbrechen.Enabled = false;
        }

        private void Light4SightNG_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Y)
            {
                stdStrategie.gesehen_KeyDown(e);
            }
            if (e.KeyCode == Keys.M)
            {
                stdStrategie.nichtgesehen_KeyDown(e);
            }
        }

        private void Light4SightNG_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.KeyPreview = false;
            AudioControl.Dispose();

        }

        private void btnLoadPreset_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath).ToString() + @"\presets\";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Filter = "preset files (*.pre)|*.pre";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName);

                string blah;
                List<string> temp = new List<string>();
                blah = sr.ReadToEnd();


                char[] delimiters = new char[] { '\n' };
                char[] delimiters2 = new char[] { ';' };
                List<string[]> columns = new List<string[]>();
                temp.AddRange(blah.Split(delimiters));
                foreach (string s in temp)
                {
                    columns.Add(s.Split(delimiters2));
                }

                string[] values = columns[0];
                if (values[0] == "True") cbAktivIR.Checked = true; else cbAktivIR.Checked = false;
                cbSigFormIR.Text = values[1];
                tbFreqIR.Text = values[2];
                tbMHIR.Text = values[3];
                tbPhasVerschIR.Text = values[4];
                tbKonIRSC1.Text = values[5];
                tbIRSC1DeltaK.Text = values[6];
                tbKonIRSC2.Text = values[7];
                tbIRSC2DeltaK.Text = values[8];

                values = columns[1];
                if (values[0] == "True") cbAktivIG.Checked = true; else cbAktivIG.Checked = false;
                cbSigFormIG.Text = values[1];
                tbFreqIG.Text = values[2];
                tbMHIG.Text = values[3];
                tbPhasVerschIG.Text = values[4];
                tbKonIGSC1.Text = values[5];
                tbIGSC1DeltaK.Text = values[6];
                tbKonIGSC2.Text = values[7];
                tbIGSC2DeltaK.Text = values[8];

                values = columns[2];
                if (values[0] == "True") cbAktivIB.Checked = true; else cbAktivIB.Checked = false;
                cbSigFormIB.Text = values[1];
                tbFreqIB.Text = values[2];
                tbMHIB.Text = values[3];
                tbPhasVerschIB.Text = values[4];
                tbKonIBSC1.Text = values[5];
                tbIBSC1DeltaK.Text = values[6];
                tbKonIBSC2.Text = values[7];
                tbIBSC2DeltaK.Text = values[8];

                values = columns[3];
                if (values[0] == "True") cbAktivIC.Checked = true; else cbAktivIC.Checked = false;
                cbSigFormIC.Text = values[1];
                tbFreqIC.Text = values[2];
                tbMHIC.Text = values[3];
                tbPhasVerschIC.Text = values[4];
                tbKonICSC1.Text = values[5];
                tbICSC1DeltaK.Text = values[6];
                tbKonICSC2.Text = values[7];
                tbICSC2DeltaK.Text = values[8];

                values = columns[4];
                if (values[0] == "True") cbAktivOR.Checked = true; else cbAktivOR.Checked = false;
                cbSigFormOR.Text = values[1];
                tbFreqOR.Text = values[2];
                tbMHOR.Text = values[3];
                tbPhasVerschOR.Text = values[4];
                tbKonORSC1.Text = values[5];
                tbORSC1DeltaK.Text = values[6];
                tbKonORSC2.Text = values[7];
                tbORSC2DeltaK.Text = values[8];

                values = columns[5];
                if (values[0] == "True") cbAktivOG.Checked = true; else cbAktivOG.Checked = false;
                cbSigFormOG.Text = values[1];
                tbFreqOG.Text = values[2];
                tbMHOG.Text = values[3];
                tbPhasVerschOG.Text = values[4];
                tbKonOGSC1.Text = values[5];
                tbOGSC1DeltaK.Text = values[6];
                tbKonOGSC2.Text = values[7];
                tbOGSC2DeltaK.Text = values[8];

                values = columns[6];
                if (values[0] == "True") cbAktivOB.Checked = true; else cbAktivOB.Checked = false;
                cbSigFormOB.Text = values[1];
                tbFreqOB.Text = values[2];
                tbMHOB.Text = values[3];
                tbPhasVerschOB.Text = values[4];
                tbKonOBSC1.Text = values[5];
                tbOBSC1DeltaK.Text = values[6];
                tbKonOBSC2.Text = values[7];
                tbOBSC2DeltaK.Text = values[8];

                values = columns[7];
                if (values[0] == "True") cbAktivOC.Checked = true; else cbAktivOC.Checked = false;
                cbSigFormOC.Text = values[1];
                tbFreqOC.Text = values[2];
                tbMHOC.Text = values[3];
                tbPhasVerschOC.Text = values[4];
                tbKonOCSC1.Text = values[5];
                tbOCSC1DeltaK.Text = values[6];
                tbKonOCSC2.Text = values[7];
                tbOCSC2DeltaK.Text = values[8];
            }
        }

        private void btnSavePreset_Click(object sender, EventArgs e)
        {
            SignalEigenschaftenEinlesen();
            DirectoryInfo d = new DirectoryInfo(@".\presets\");
            d.Create();

            saveFileDialog1.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath).ToString() + @"\presets\";
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.Filter = "preset files (*.pre)|*.pre";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
                string temp = string.Empty;

                foreach (clSignalDescription chan in channels)
                {
                    temp = chan.SignalAktiv.ToString() + ";";
                    temp = temp + chan.Signalform + ";";
                    temp = temp + chan.Frequenz.ToString() + ";";
                    temp = temp + chan.MittlereHelligkeit_cdm2.ToString() + ";";
                    temp = temp + chan.Phasenverschiebung.ToString() + ";";
                    temp = temp + chan.KonSC1_100.ToString() + ";";
                    temp = temp + chan.SC1DeltaK_100.ToString() + ";";
                    temp = temp + chan.KonSC2_100.ToString() + ";";
                    temp = temp + chan.SC2DeltaK_100.ToString() + ";";
                    sw.WriteLine(temp);
                }
                sw.Flush();
                sw.Close();
            }

        }










    }




}
