using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace RimageKorea
{
    public class CmdExecute
    {
        public static void start(string cmdText)
        {
            //Network 변수
            StreamWriter DosWriter;
            StreamReader DosRedaer;
            StreamReader ErrorReader;

            //프로세스 생성및 초기화
            Process DosPr = new Process();
            ProcessStartInfo psI = new ProcessStartInfo("cmd");

            psI.UseShellExecute = false;
            psI.RedirectStandardInput = true;
            psI.RedirectStandardOutput = true;
            psI.RedirectStandardError = true;
            psI.CreateNoWindow = true;
 

            //명령 실행
            DosPr.StartInfo = psI;
            DosPr.Start();
            DosWriter = DosPr.StandardInput;
            DosRedaer = DosPr.StandardOutput;
            ErrorReader = DosPr.StandardError;
            DosWriter.AutoFlush = true;

            DosWriter.WriteLine(cmdText);
            DosWriter.Close();
        }
    }
}
