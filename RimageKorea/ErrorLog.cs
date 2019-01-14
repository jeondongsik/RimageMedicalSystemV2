using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RimageKorea
{
    public class ErrorLog
    {
        /// <summary>
        /// 에러 로그 쓰기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="sMsg"></param>
        /// <param name="strPath"></param>
        public static void LogWrite(object sender, string sMsg, string strPath)
        {
            string path = strPath + "\\" + GlobalVar.LOG_ERR_FLD;
            
            DateTime d1 = DateTime.Now;
            string[] sDateTime = d1.GetDateTimeFormats();

            int year = d1.Year;
            int month = d1.Month;
            int day = d1.Day;

            string Filename = path.ToString() + "\\errlog_" + year.ToString() + month.ToString().PadLeft(2, '0') + day.ToString().PadLeft(2, '0') + ".txt";
            //string[] strDir = Directory.GetDirectories(path);
            Directory.CreateDirectory(path);

            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(Filename, true, System.Text.Encoding.Default);
                sMsg = sDateTime[76] + Convert.ToChar(9) + sender.ToString() + Convert.ToChar(9) + sMsg;        //[76]	"2008-02-11 15:25:30"	string, 9번 텝
                sw.WriteLine(sMsg);
                sw.Flush();
            }
            catch { }
            finally
            {
                if (sw != null) sw.Close();
            }
        }
        
        /// <summary>
        /// 추적로그 쓰기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="sMsg"></param>
        /// <param name="strPath"></param>
        public static void TraceWrite(object sender, string sMsg, string strPath)
        {
            string path = strPath + "\\" + GlobalVar.LOG_ERR_FLD;

            DateTime d1 = DateTime.Now;
            string[] sDateTime = d1.GetDateTimeFormats();

            int year = d1.Year;
            int month = d1.Month;
            int day = d1.Day;

            string Filename = path.ToString() + "\\tracelog_" + year.ToString() + month.ToString().PadLeft(2, '0') + day.ToString().PadLeft(2, '0') + ".txt";
            //string[] strDir = Directory.GetDirectories(path);
            Directory.CreateDirectory(path);

            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(Filename, true, System.Text.Encoding.Default);
                sMsg = sDateTime[76] + Convert.ToChar(9) + sender.ToString() + Convert.ToChar(9) + sMsg;        //[76]	"2008-02-11 15:25:30"	string, 9번 텝
                sw.WriteLine(sMsg);
                sw.Flush();
            }
            catch { }
            finally
            {
                if (sw != null) sw.Close();
            }
        }
    }
}
