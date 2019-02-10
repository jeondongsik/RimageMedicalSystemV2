﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace RimageKorea
{
    public class LogDelete
    {
        public string folderLoot { get; set; }
        public int retentionPeriod { get; set; }
        public string localSharedFolder { get; set; }
        public string downloadFolder { get; set; }
        public string deleteInJobFolder { get; set; }

        //Order Log, 환자폴더, Error log 파일 삭제
        public void Workflow(System.ComponentModel.BackgroundWorker worker, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                //// Order.xml 파일 삭제
                foreach (string file in Directory.GetFiles(folderLoot, "Orders.xml"))
                {
                    File.Delete(file);
                }
                //// .pOrd 파일 삭제
                foreach (string file in Directory.GetFiles(folderLoot, "*.pOrd"))
                {
                    File.Delete(file);
                }
            }
            catch { }

            //// 굽기 명령 정보 삭제
            try
            {
                DirectoryInfo logDir = new DirectoryInfo(Path.Combine(folderLoot, GlobalVar.ORDER_FOLDER));
                Dictionary<string, string> delTargetOrderFiles = new Dictionary<string, string>();
                if (1 > retentionPeriod) retentionPeriod = 1;

                foreach (FileInfo fi in logDir.GetFiles())
                {
                    if (fi.CreationTime > DateTime.Now.AddDays(retentionPeriod * -1).Date)
                    {
                        continue;
                    }
                    
                    //// 굽기명령 파일인 경우 하위 환자폴더도 삭제
                    if (fi.Name.EndsWith("ORD"))
                    {
                        string json = File.ReadAllText(fi.FullName);
                        BurnOrderedInfoEntity burnOrderInfo = JsonParser.ConvertToBurnOrderedInfoEntity(json);

                        //// 환자폴더 삭제
                        Directory.Delete(burnOrderInfo.patFolderFullPath, true);
                        //// 머지파일 삭제
                        FileControl.DeleteFile(burnOrderInfo.MegPath);
                        //// EditList 파일 삭제
                        FileControl.DeleteFile(burnOrderInfo.EditListPath);
                    }

                    fi.Delete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite(null, ex.ToString(), folderLoot);
            }
            
            //에러로그 삭제
            try
            {
                DirectoryInfo errDir = new DirectoryInfo(Path.Combine(folderLoot, GlobalVar.LOG_ERR_FLD));
                foreach (FileInfo fi in errDir.GetFiles())
                {
                    if (fi.CreationTime.Date <= DateTime.Now.AddDays(31).Date)
                    {
                        FileControl.DeleteFile(fi.FullName, false);
                    }
                }
            }
            catch { }

            //에러로그 삭제
            try
            {
                DirectoryInfo errDir = new DirectoryInfo(Path.Combine(folderLoot, GlobalVar.ORDER_FOLDER, "ERRORS"));
                foreach (FileInfo fi in errDir.GetFiles())
                {
                    if (fi.CreationTime.Date <= DateTime.Now.AddDays(31).Date)
                    {
                        FileControl.DeleteFile(fi.FullName, false);
                    }
                }
            }
            catch { }

            //// 작업폴더 내용 삭제하기
            if (deleteInJobFolder == "Y")
            {
                try
                {
                    //// 다운로드 폴더
                    FileControl.ClearDirectory(this.downloadFolder);
                }
                catch { }
            }

            //// 굽기 로그 삭제
            try
            {
                DirectoryInfo dirOrder  = new DirectoryInfo(Path.Combine(folderLoot, GlobalVar.ORDER_FOLDER));

                foreach (DirectoryInfo dri in dirOrder.GetDirectories())
                {
                    if (dri.Name.StartsWith("ERRORS"))
                        continue;

                    try
                    {
                        dri.Delete(true);
                    }
                    catch { }
                }
            }
            catch { }
        }
    }
}
