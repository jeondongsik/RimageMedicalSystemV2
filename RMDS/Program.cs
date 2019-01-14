using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RMDS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ////args = new string[1];
            ////args[0] = "성길순_38678794_17123132692";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain(args));
        }
    }
}
