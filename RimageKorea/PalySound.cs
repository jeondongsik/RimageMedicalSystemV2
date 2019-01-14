using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Runtime.InteropServices;

namespace RimageKorea
{
    public class PalySound
    {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

        public static void Run(string path)
        {
            //SoundPlayer wp = new SoundPlayer(path);
            //wp.PlaySync();
            mciSendString("close MediaFile", null, 0, IntPtr.Zero);
            mciSendString("open \"" + path + "\" type mpegvideo alias MediaFile", null, 0, IntPtr.Zero);
            mciSendString("play MediaFile", null, 0, IntPtr.Zero);
        }
    }
}
