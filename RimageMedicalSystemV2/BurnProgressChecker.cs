using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.IO;
using RimageKorea;

namespace RimageMedicalSystemV2
{
    /// <summary>
    /// 굽기 진행상태를 계속체크하면서 결과값을 메인화면으로 보내주는 역할을 한다.
    /// </summary>
    public class BurnProgressChecker
    {
        string orderLogPath = string.Empty;
        MainForm mReportForm; // status to Form1

        /// <summary>
        /// 진행상태를 주기적으로 읽어오는 타이머
        /// </summary>
        System.Timers.Timer tTimer;

        /// <summary>
        /// 서버상태,설정 정보를 주기적으로 읽어오는 타이머
        /// </summary>
        ////System.Timers.Timer sTimer;

        /// <summary>
        /// 서버설정값 가져온 횟수
        /// </summary>
        Dictionary<string, long> getServerConfigCount = new Dictionary<string, long>();

        /// <summary>
        /// 읽기 작업중인지 여부
        /// </summary>
        bool processing = false;

        public BurnProgressChecker(MainForm mainForm)
        {
            this.mReportForm = mainForm;
            this.orderLogPath = Path.Combine(GlobalVar.ProgramExecuteFolder, GlobalVar.ORDER_FOLDER);

            this.tTimer = new System.Timers.Timer();
            this.tTimer.Interval = 2800; // 3초
            this.tTimer.Elapsed += new ElapsedEventHandler(tTimer_Elapsed);
            this.tTimer.Enabled = false;

            ////this.sTimer = new System.Timers.Timer();
            ////this.sTimer.Interval = 3000; // 3초
            ////this.sTimer.Elapsed += new ElapsedEventHandler(sTimer_Elapsed);
            ////this.sTimer.Enabled = false;
        }

        /// <summary>
        /// 타이머 시작
        /// </summary>
        public void StartTimer()
        {
            this.tTimer.Enabled = true;
            this.tTimer.Start();

            ////this.sTimer.Enabled = true;
            ////this.sTimer.Start();
        }

        private void sTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ////this.sTimer.Stop();

            ////try
            ////{
            ////    //// 서버상태,설정
            ////    this.ReadServerConfig();
            ////}
            ////catch 
            ////{
            ////}
            ////finally
            ////{
            ////    //// 서버별 설정값을 주기적으로 읽어오는 건 처음 한번만 필요
            ////    if (this.getServerConfigCount.Count > 0 &&
            ////        this.getServerConfigCount.ContainsKey(this.mReportForm.NowSeletedServer.IP) && 
            ////        this.getServerConfigCount[this.mReportForm.NowSeletedServer.IP] > 0)
            ////    {
            ////        this.sTimer.Stop();
            ////        this.sTimer.Enabled = false;
            ////    }
            ////    else
            ////    {
            ////        this.sTimer.Start();
            ////    }
            ////}
        }

        private void tTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.tTimer.Stop();
            this.processing = true;

            try
            {
                //// 진행상태 읽기
                this.ReadBurnProgress();

                //// 오류읽기
                //// - system오류, 굽기오류, 기타
                //// this.ReadError();

                this.processing = false;
            }
            catch
            {
                this.processing = false;
            }
            finally
            {
                if (!this.processing)
                    this.tTimer.Start();
            }
        }

        /// <summary>
        /// 서버 설정값/상태값 읽기
        /// </summary>
        /// <returns></returns>
        public void ReadServerConfig()
        {
            try
            {
                //// 연결된 서버들의 설정값을 읽어서 메인화면으로 전송
                //// 결과값
                Dictionary<string, Hashtable> result = new Dictionary<string, Hashtable>();

                foreach (var dir in Directory.GetDirectories(this.orderLogPath, "SVR_*"))
                {
                    //// IP 추가
                    string ip = dir.Replace("SVR_", "");

                    if (!result.ContainsKey(ip))
                        result.Add(ip, new Hashtable());

                    //// 서버IP 폴더 목록만 가져온다.
                    foreach (string file in Directory.GetFiles(dir))
                    {
                        string xml = File.ReadAllText(file);

                        if (file.StartsWith(GlobalVar.SERVER_CONFIG_FL))
                        {
                            if (result[ip].ContainsKey("CONFIG"))
                            {
                                result[ip]["CONFIG"] = xml;
                            }
                            else
                            {
                                result[ip].Add("CONFIG", xml);
                            }
                        }
                        else if (file.StartsWith(GlobalVar.SERVER_STATUS_FL))
                        {
                            if (result[ip].ContainsKey("STATUS"))
                            {
                                result[ip]["STATUS"] = xml;
                            }
                            else
                            {
                                result[ip].Add("STATUS", xml);
                            }
                        }
                    }

                    if (result.ContainsKey(ip))
                    {
                        if (this.getServerConfigCount.ContainsKey(ip))
                        {
                            this.getServerConfigCount[ip]++;
                        }
                        else
                        {
                            this.getServerConfigCount.Add(ip, 1);
                        }
                    }
                }

                if (result.Count() > 0)
                {
                    //// 메인화면에 Invoke
                    this.mReportForm.Invoke(this.mReportForm.serverStatusDel, result);
                }
            }
            catch { }
        }

        /// <summary>
        /// 진행상태 읽기
        /// </summary>
        public void ReadBurnProgress()
        {
            try
            {
                //// 오더아이디별 폴더로 되어 있음.
                foreach (string fld in Directory.GetDirectories(this.orderLogPath))
                {
                    if (fld.StartsWith("SVR_"))
                        continue;

                    //// 굽기완료된 주문폴더인지 체크
                    var endFiles = Directory.GetFiles(fld, GlobalVar.BURN_CHK_FL_NM);
                    if (endFiles != null && endFiles.Length > 0)
                    {
                        //// 굽기완료되었을 경우 서버 상태값 읽어온다.
                        this.ReadServerConfig();
                        continue;
                    }

                    //// 폴더명이 주문번호임.
                    //// 기록된 파일중 제일 늦은거
                    var fileLst = Directory.GetFiles(fld, string.Format("{0}*", GlobalVar.PFX_TRC));
                    if (fileLst != null && fileLst.Length > 0)
                    {
                        var fList = fileLst.OrderByDescending(s => s).ToList();
                        string file = fList.First();
                        string json = File.ReadAllText(file);

                        DiscStatusForDisplay obj = JsonParser.ConvertToDiscStatusForDisplay(json);

                        if (obj != null)
                        {
                            this.mReportForm.Invoke(this.mReportForm.burningTraceDel, obj);
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 오류 읽기
        /// </summary>
        public void ReadError()
        {
            try
            {
                
            }
            catch { }
        }
    }
}
