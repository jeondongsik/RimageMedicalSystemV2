using System;
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
                    DateTime diDt = fi.CreationTime.Date;
                    if (diDt <= DateTime.Now.AddDays(retentionPeriod * -1).Date)
                    {
                        //// 굽기명령 파일인 경우 하위 환자폴더도 삭제
                        if (fi.Name.EndsWith("ORD"))
                        {
                            string json = File.ReadAllText(fi.FullName);
                            BurnOrderedInfoEntity burnOrderInfo = JsonParser.ConvertToBurnOrderedInfoEntity(json);

                            //// 환자폴더 삭제
                            FileControl.DeleteFolder(burnOrderInfo.patFolderFullPath);
                            //// 머지파일 삭제
                            FileControl.DeleteFile(burnOrderInfo.MegPath);
                            //// EditList 파일 삭제
                            FileControl.DeleteFile(burnOrderInfo.EditListPath);
                            //// TraceLog 폴더 삭제
                            FileControl.DeleteFolder(burnOrderInfo.OrderId);
                        }

                        fi.Delete();
                    }
                }

                //// Order.xml 파일 삭제
                foreach (DirectoryInfo ddr in logDir.GetDirectories("SVR_*"))
                {
                    foreach (FileInfo file in ddr.GetFiles("Orders_*.xml"))
                    {
                        if (file.CreationTime.Date < DateTime.Now.Date)
                        {
                            file.Delete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite(null, ex.ToString(), folderLoot);
            }
            
            //// 에러로그 삭제            
            try
            {
                DirectoryInfo errDir = new DirectoryInfo(Path.Combine(folderLoot, GlobalVar.LOG_ERR_FLD));
                foreach (FileInfo fi in errDir.GetFiles())
                {
                    if (fi.CreationTime.Date <= DateTime.Now.AddDays(-30).Date)
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
                    if (fi.CreationTime.Date <= DateTime.Now.AddDays(-30).Date)
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
                    //// 다운로드 폴더안을 정리
                    foreach (string fl in Directory.GetFiles(this.downloadFolder))
                    {
                        try
                        {
                            File.Delete(fl);
                        }
                        catch { }
                    }

                    foreach (string dir in Directory.GetDirectories(this.downloadFolder))
                    {
                        FileControl.ClearDirectory(dir);
                    }
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

                    if (dri.Name.StartsWith("SVR_"))
                    {
                        ////서버 설정,상태값 파일은 삭제하지 않음.
                        foreach (var fl in Directory.GetFiles(dri.FullName))
                        {
                            if (fl.Contains(GlobalVar.SERVER_CONFIG_FL) || fl.Contains(GlobalVar.SERVER_STATUS_FL))
                                continue;

                            try
                            {
                                File.Delete(fl);
                            }
                            catch { }
                        }
                    }
                    else
                    {
                        try
                        {
                            dri.Delete(true);
                        }
                        catch { }
                    }
                }
            }
            catch { }
        }
    }
}
