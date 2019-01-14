using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RimageKorea
{
    public class ExecuteWhenStart
    {
        /// <summary>
        /// 필요한 폴더를 생성한다.
        /// </summary>
        public static void CreateBaseDirectory(string baseDir)
        {
            try
            {
                //Order Log를 생성하기 위한 폴더 생성
                if (!Directory.Exists(Path.Combine(baseDir, GlobalVar.LOG_ORDER_FLD)))
                {
                    Directory.CreateDirectory(Path.Combine(baseDir, GlobalVar.LOG_ORDER_FLD));
                }

                //일자, EditList 폴더
                if (!Directory.Exists(Path.Combine(baseDir, GlobalVar.LOG_ORDER_FLD, DateTime.Now.ToString("yyyy-MM-dd"))))
                {
                    Directory.CreateDirectory(Path.Combine(baseDir, GlobalVar.LOG_ORDER_FLD, DateTime.Now.ToString("yyyy-MM-dd")));
                }
                if (!Directory.Exists(Path.Combine(baseDir, GlobalVar.LOG_ORDER_FLD, DateTime.Now.ToString("yyyy-MM-dd"), GlobalVar.LOG_ORDER_EDITLIST_FLD)))
                {
                    Directory.CreateDirectory(Path.Combine(baseDir, GlobalVar.LOG_ORDER_FLD, DateTime.Now.ToString("yyyy-MM-dd"), GlobalVar.LOG_ORDER_EDITLIST_FLD));
                }

                //// 굽기명령정보 저장 폴더 생성
                if (!Directory.Exists(Path.Combine(baseDir, GlobalVar.ORDER_FOLDER)))
                {
                    Directory.CreateDirectory(Path.Combine(baseDir, GlobalVar.ORDER_FOLDER));
                }
            }
            catch { }
        }
    }
}
