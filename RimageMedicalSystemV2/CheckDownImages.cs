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
    public class CheckDownImages
    {
        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern int FindWindowEx(int hWnd1, int hWnd2, string lpsz1, string lpsz2);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lparam);

        MainForm mReportForm;
        string dicomDirLoc;

        Timer tmChecker;
        Timer tmChecker2;

        int wndID = 0;
        long preSize = 0;
        int samCnt = 0;
        public const int WM_CLOSE = 0x0010; //닫기

        string captionText = "ExportDicom";
        string checkFolderName = "CDViewer";
        string checkEndFile = "End.txt";

        public CheckDownImages(MainForm _reForm, string _dicomDirLoc)
        {
            try
            {
                this.mReportForm = _reForm;
                this.dicomDirLoc = _dicomDirLoc;

                this.tmChecker = new Timer();
                this.tmChecker.Enabled = true;
                this.tmChecker.Interval = 300;
                this.tmChecker.Tick += new System.EventHandler(this.tmChecker_Tick);
                this.tmChecker.Start();

                this.tmChecker2 = new Timer();
                this.tmChecker2.Enabled = true;
                this.tmChecker2.Interval = 500;
                this.tmChecker2.Tick += new System.EventHandler(this.tmChecker2_Tick);
                this.tmChecker2.Stop();
            }
            catch { }
        }

        public void StartTimer1()
        {
            if (this.tmChecker != null)
            {
                this.tmChecker.Start();
            }
        }

        public void StopTimer1()
        {
            if (this.tmChecker != null)
            {
                this.tmChecker.Stop();
            }
        }

        public void StartTimer2()
        {
            if (this.tmChecker2 != null)
            {
                this.tmChecker2.Start();
            }
        }

        public void StopTimer2()
        {
            if (this.tmChecker2 != null)
            {
                this.tmChecker2.Stop();
            }
        }

        private void tmChecker_Tick(object sender, EventArgs e)
        {
            this.CheckWindowByText();
        }

        private void CheckWindowByText()
        {
            try
            {
                int hwd = FindWindow(null, this.captionText);
                this.wndID = hwd;

                if (hwd > 0)
                {
                    this.wndID = hwd;
                    this.StopTimer1();

                    //// CDViewer 폴더를 찾는다.
                    //// CDViewer 파일의 사이즈를 체크한다. - 500ms 간격으로 찾고 이전 사이즈와 크기 비교
                    //// 5번째까지 파일의 크기변화가 없으면 완료로 판단

                    this.preSize = 0;
                    this.samCnt = 0;

                    this.StartTimer2();
                }
            }
            catch { }
        }

        private void tmChecker2_Tick(object sender, EventArgs e)
        {
            this.CheckDirectorySize();
        }

        private void CheckDirectorySize()
        {
            try
            {
                string chkFoler = Path.Combine(this.dicomDirLoc, this.checkFolderName);

                if (Directory.Exists(chkFoler))
                {
                    //// 후킹완료여부 파일 체크 : 이 파일이 존재하면 이 폴더는 후킹완료된 건.
                    if (File.Exists(Path.Combine(this.dicomDirLoc, this.checkEndFile)))
                    {
                        this.StopTimer2();
                        this.StartTimer1();
                    }
                    else
                    {
                        //// 사이즈 체크
                        long nowSize = FileControl.GetFolderLengthOnly(chkFoler);

                        if (this.preSize != nowSize)
                        {
                            //// 다르면 계속 체크
                            this.preSize = nowSize;
                        }
                        else
                        {
                            //// 사이즈 같은 횟수 증가
                            samCnt++;

                            if (samCnt == 5)
                            {
                                //// 완료
                                //// this.textBox1.AppendText(string.Format("[Hooking(1)] CD Imaging 완료-({0}){1}\r\n", samCnt.ToString(), fi.Length.ToString()));
                                this.StopTimer2();
                                SendMessage((IntPtr)this.wndID, WM_CLOSE, 0, 0);

                                //// end.txt 파일 생성
                                FileControl.CreateTextFile(Path.Combine(this.dicomDirLoc, this.checkEndFile));

                                this.mReportForm.Invoke(this.mReportForm.hookingComp);
                            }
                        }
                    }
                }
            }
            catch
            {
                this.StopTimer2();
                this.StartTimer1();
            }
        }
    }
}
