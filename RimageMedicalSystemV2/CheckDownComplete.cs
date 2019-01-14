using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using RimageKorea;

namespace RimageMedicalSystemV2
{
    public class CheckDownComplete
    {
        MainForm mReportForm;
        string dicomDirLoc;

        Timer tmChecker;

        long preSize = 0;
        //// int samCnt = 0;
        //// int chkCnt = 5; ////사이즈같은상태 체크횟수

        public CheckDownComplete(MainForm _reForm, string _dicomDirLoc)
        {
            try
            {
                //// this.chkCnt = GlobalVar.FolderSizeCheckTime + 1;
                this.mReportForm = _reForm;
                this.dicomDirLoc = _dicomDirLoc;

                this.tmChecker = new Timer();
                this.tmChecker.Enabled = true;
                this.tmChecker.Interval = GlobalVar.FolderSizeCheckTime * 1000;
                this.tmChecker.Tick += new System.EventHandler(this.tmChecker_Tick);
                this.tmChecker.Start();
            }
            catch { }
        }

        public void StartTimer()
        {
            if (this.tmChecker != null)
            {
                this.tmChecker.Start();
            }
        }

        public void StopTimer()
        {
            if (this.tmChecker != null)
            {
                this.tmChecker.Stop();
            }
        }

        private void tmChecker_Tick(object sender, EventArgs e)
        {
            this.CheckDirectorySize();
        }

        /// <summary>
        /// 폴더사이즈 체크
        /// </summary>
        private void CheckDirectorySize()
        {
            try
            {
                if (Directory.Exists(this.dicomDirLoc))
                {
                    //// DicomDir 파일이 존재할 때부터 체크
                    //// 후킹완료여부 파일 체크 : 이 파일이 존재하면 이 폴더는 후킹완료된 건.
                    if (File.Exists(Path.Combine(this.dicomDirLoc, "DICOMDIR")) &&
                        !File.Exists(Path.Combine(this.dicomDirLoc, GlobalVar.DOWN_CHK_FL_NM)))
                    {
                        ErrorLog.TraceWrite("CheckDirectorySize", string.Format(">>Start>>체크폴더 : {0}", this.dicomDirLoc), Environment.CurrentDirectory);

                        //// 사이즈 체크
                        long nowSize = FileControl.GetFolderLengthOnly(this.dicomDirLoc);

                        //// 로그기록
                        ErrorLog.TraceWrite("CheckDirectorySize", string.Format(">>이전 : {0}, 현재 : {1}", this.preSize.ToString(), nowSize.ToString()), Environment.CurrentDirectory);

                        if (this.preSize != nowSize)
                        {
                            //// 다르면 계속 체크
                            this.preSize = nowSize;
                        }
                        else
                        {
                            //// 사이즈가 같으면 완료
                            this.StopTimer();

                            //// end.txt 파일 생성
                            FileControl.CreateTextFile(Path.Combine(this.dicomDirLoc, GlobalVar.DOWN_CHK_FL_NM));

                            //// 이전사이즈 초기화
                            this.preSize = 0;

                            //// 로그기록
                            ErrorLog.TraceWrite("CheckDirectorySize", string.Format(">>End>>사이즈 동일하여 체크 완료."), Environment.CurrentDirectory);

                            //// 메인화면으로 처리 명령
                            this.mReportForm.Invoke(this.mReportForm.hookingComp2);

                            //// 사이즈 같은 횟수 증가
                            //// this.samCnt++;

                            ////if (this.samCnt == this.chkCnt)
                            ////{
                            ////    //// 완료
                            ////    this.StopTimer();
                            ////    //// end.txt 파일 생성
                            ////    FileControl.CreateTextFile(Path.Combine(this.dicomDirLoc, GlobalVar.DOWN_CHK_FL_NM));

                            ////    this.preSize = 0;
                            ////    this.samCnt = 0;

                            ////    this.mReportForm.Invoke(this.mReportForm.hookingComp2);
                            ////}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite("FileControl", ex.ToString(), Environment.CurrentDirectory);
                //// end.txt 파일 생성
                FileControl.CreateTextFile(Path.Combine(this.dicomDirLoc, GlobalVar.DOWN_CHK_FL_NM));

                this.preSize = 0;
                //// this.samCnt = 0;
            }
        }
    }
}
