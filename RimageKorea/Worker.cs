using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace RimageKorea
{
    public class Worker
    {
        public string startupPath { get; set; }
        public DirectoryInfo srcDir { get; set; }
        public string DicomDownloadFolder { get; set; }
        public string LocalShareFolder { get; set; }
        public string patientFolderName { get; set; }

        /// <summary>
        /// 환자정보 파일 복사하기 1
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        public void ExecJob(System.ComponentModel.BackgroundWorker worker, System.ComponentModel.DoWorkEventArgs e)
        {
            CurrentState state = new CurrentState();
            state.fileSize = 0;
            state.retCode = 0;
            state.retMessage = "파일복사 시작 >>> \r\n";
            worker.ReportProgress(1, state);

            if (worker.CancellationPending)
            {
                state.retCode = 0;
                state.retMessage = "파일복사가 작업자에 의해 취소되었습니다.\r\n";
                worker.ReportProgress(100, state);
                e.Cancel = true;
            }
            else
            {
                try
                {
                    if (srcDir.Exists)
                    {
                        //1.복사할 대상 디렉토리 생성
                        creatdFolder(srcDir);
                        state.retMessage = "복사할 대상 디렉토리 생성 완료.\r\n";
                        worker.ReportProgress(10, state);

                        //2.파일복사
                        moveFiles(srcDir, worker, e, state);

                        //3.원폴더 삭제
                        DirectoryInfo dri = new DirectoryInfo(this.DicomDownloadFolder);
                        foreach (DirectoryInfo d in dri.GetDirectories())
                        {
                            if (d.Exists)
                                d.Delete(true);
                        }
                        state.retMessage = "Copying files done.\r\n";
                        worker.ReportProgress(100, state);
                    }
                    else
                    {
                        state.retCode = -1;
                        state.retMessage = "파일복사 중 에러발생.\r\n";
                        worker.ReportProgress(100, state);
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.LogWrite(this, ex.ToString(), startupPath);
                    state.retCode = -1;
                    state.retMessage = "파일복사 중 에러발생.\r\n";
                    worker.ReportProgress(100, state);
                }

                e.Result = state;
            }
        }

        /// <summary>
        /// 복사할 대상 디렉토리 생성
        /// </summary>
        /// <param name="directory"></param>
        public void creatdFolder(DirectoryInfo directory)
        {
            foreach (DirectoryInfo d in directory.GetDirectories())
            {
                string targetDir = d.FullName.Replace(DicomDownloadFolder, LocalShareFolder + "\\" + patientFolderName);
                DirectoryInfo tdir = new DirectoryInfo(targetDir);

                if (!tdir.Exists)
                    tdir.Create();
                                
                creatdFolder(d);
            }
        }

        /// <summary>
        /// 파일이동
        /// </summary>
        /// <param name="directory"></param>
        private void moveFiles(DirectoryInfo directory, System.ComponentModel.BackgroundWorker worker, System.ComponentModel.DoWorkEventArgs e, CurrentState state)
        {
            FileInfo[] files = directory.GetFiles();
            DirectoryInfo[] dirs = directory.GetDirectories();

            foreach (FileInfo file in files)
            {
                state.fileSize += file.Length;
                string copyto = file.FullName.Replace(this.DicomDownloadFolder, this.LocalShareFolder + "\\" + this.patientFolderName);
                file.CopyTo(copyto);
                file.Delete();
                //txtStatusView.AppendText(this.copyFileLen.ToString() + " byte\r\n");

                state.retMessage = state.fileSize.ToString() + " byte copied.\r\n";
                worker.ReportProgress(20, state);
            }

            foreach (DirectoryInfo dri in dirs)
            {
                moveFiles(dri, worker, e, state);
            }
        }
    }
}
