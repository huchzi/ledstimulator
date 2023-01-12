using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light4SightNG
{
    public class LogWriter
    {
        private StreamWriter LogFile;
        private bool debugfile = false;
        private string filename;

        public LogWriter(string dateiname, bool debug)
        {
            this.filename = @".\Untersuchungen\" + dateiname;

            if (debug == true)
                this.debugfile = true;
            try
            {
                DirectoryInfo d = new DirectoryInfo(@".\Untersuchungen\");
                d.Create();
                this.LogFile = new StreamWriter(this.filename);
            }
            catch
            {
            }
        }

        public string add(string info)
        {
            try
            {
                this.LogFile.WriteLine(info);
                this.LogFile.Flush();
                if (this.debugfile == false)
                    return ("In " + this.filename + "geschrieben: " + info);
                else
                    return ("");
            }
            catch 
            {
                return ("Fehler beim Schreiben in " + this.filename);
            }
        }

        public string close()
        {
            try
            {
                this.LogFile.Close();
                if (this.debugfile == false)
                    return (this.filename + " wurde geschlossen");
                else
                    return "";
            }
            catch
            {
                if (this.debugfile == false)
                    return ("Beim Schließen von " + this.filename + " ist ein Fehler aufgetreten.");
                else
                    return "";
            }
        }


    }
}
