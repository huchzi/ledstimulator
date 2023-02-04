using System;
using System.Windows.Forms;

namespace CalibrateLEDStimulator
{
    static class Program
    {
        /// <summary>
        /// Main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Calibration());
        }
    }
}
