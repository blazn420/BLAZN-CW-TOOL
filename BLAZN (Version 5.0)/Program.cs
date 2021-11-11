using BLAZN.AntiDebug;
using BLAZN.UTILITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BLAZN
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Scanner.ScanAndKill();

            if (DebugProtect1.PerformChecks() == 1)
            {
                Environment.FailFast("");
            }

            if (DebugProtect2.PerformChecks() == 1)
            {
                Environment.FailFast("");
            }

            AntiDebug.DebugProtect3.HideOSThreads();

            //OnProgramStart.Initialize("BLAZN.", "825270", "YiR1OYII6nnmzQxR5GNE5RR30zehHoZO8Bv", "1.0");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MAIN());
        }
    }
}
