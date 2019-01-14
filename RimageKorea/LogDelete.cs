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

            try
            {
                DirectoryInfo logDir = new DirectoryInfo(Path.Combine(folderLoot, GlobalVar.LOG_ORDER_FLD));
                Dictionary<string, string> delTargetOrderFiles = new Dictionary<string, string>();
                if (1 > retentionPeriod) retentionPeriod = 1;

                foreach (DirectoryInfo dinfo in logDir.GetDirectories())
                {
                    DateTime diDt = Convert.ToDateTime(dinfo.Name).Date;
                    if (diDt <= DateTime.Now.AddDays(retentionPeriod * -1).Date)
                    {
                        //환자 폴더도 삭제
                        string targetFolder = Path.Combine(logDir.FullName, diDt.ToString("yyyy-MM-dd"));
                        DataTable dt = FileControl.GetOrderedList(targetFolder);

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!delTargetOrderFiles.ContainsKey(dr["fileName"].ToString()))
                                delTargetOrderFiles.Add(dr["fileName"].ToString(), dr["EditListPath"].ToString());

                            if (Directory.Exists(Path.Combine(dr["LocalShareFolder"].ToString(), dr["patFolder"].ToString())))
                            {                                
                                if (false == string.IsNullOrWhiteSpace(dr["patFolder"].ToString()))
                                {
                                    if (FileControl.DeleteFolder(Path.Combine(dr["LocalShareFolder"].ToString(), dr["patFolder"].ToString()), false) == false)
                                    {
                                        //파일삭제 실패 시 읽기전용 파일의 속성 변경 후 다시한번 시도
                                        FileControl.FileAttributeChange(Path.Combine(dr["LocalShareFolder"].ToString(), dr["patFolder"].ToString()));

                                        if (FileControl.DeleteFolder(Path.Combine(dr["LocalShareFolder"].ToString(), dr["patFolder"].ToString()), false) == false)
                                        {
                                            if (delTargetOrderFiles.ContainsKey(dr["fileName"].ToString()))
                                                delTargetOrderFiles.Remove(dr["fileName"].ToString());
                                        }
                                    }
                                }
                            }
                        }

                        //로그폴더안의 OrderList.xml 및 EditFile삭제
                        //FileControl.DeleteFolder(dinfo.FullName, false);
                        foreach (KeyValuePair<string, string> kv in delTargetOrderFiles)
                        {
                            FileControl.DeleteFile(kv.Key);
                            FileControl.DeleteFile(kv.Value);
                        }

                        //폴더안의 파일이 없다면 폴더도 삭제
                        if (0 == FileControl.GetFolderLengthOnly(dinfo.FullName))
                        {
                            try
                            {
                                FileControl.DeleteFolder(dinfo.FullName, false);
                            }
                            catch { }
                        }
                    }
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
        }
    }
}
