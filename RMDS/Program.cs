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
            ////string[] a = new string[10];
            ////a[0] = "S|192.168.0.54|RimageSystem2|4664|0";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain(args));
        }
    }
}
