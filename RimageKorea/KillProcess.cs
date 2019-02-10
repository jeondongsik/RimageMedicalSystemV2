using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace RimageKorea
{
    public class KillProcess
    {
        public KillProcess()
        {
        }

        /// <summary>
        /// 실행중인 Process 죽이기
        /// </summary>
        /// <param name="processName"></param>
        public static void DelProcess(string processName)
        {
            try
            {
                Process[] process = Process.GetProcessesByName(processName);

                foreach (Process proc in process)
                {
                    proc.Kill();
                }
            }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="tryTimeout"></param>
        /// <returns></returns>
        public static bool IsProcess(string processName, int tryTimeout)
        {
            try
            {
                Process[] process = Process.GetProcessesByName(processName);

                if (process == null)
                    return false;

                if (process.Length == 0)
                    return false;
                                
                if (tryTimeout > 20000)
                {
                    return false;
                }

                Thread.Sleep(1000);
                tryTimeout += 1000;

                if (IsProcess(processName, tryTimeout))
                    return IsProcess(processName, tryTimeout);
                else
                    return false;
            }
            catch { }

            return true;
        }
    }
}
