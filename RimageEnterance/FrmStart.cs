using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Xml;
using System.Net;
using System.Diagnostics;
using DevExpress.XtraEditors;
using RimageKorea;

namespace RimageEnterance
{
    public partial class FrmStart : DevExpress.XtraEditors.XtraForm
    {
        string HostIP;
        string HostName;
        string HostPort;
        string programType;
        string startProgram;
        string AppDirectory;
        string ServerListFile;
        List<ServerInfo> ServerList;    //서버목록

        public FrmStart()
        {
            InitializeComponent();

            RimageKorea.ErrorLog.TraceWrite(this, "-- Program Start -- ", Application.StartupPath);

            KillProcess.DelProcess(GlobalVar.RIMAGE_CONFIG_SETTING);
            KillProcess.DelProcess(GlobalVar.RIMAGE_MEDICAL_CLIENT);

            this.ServerListFile = Path.Combine(Application.StartupPath, "ServerList.xml");

            GetConfig();
            PingCheck();
        }

        /// <summary>
        /// 환경설정값 가져오기
        /// </summary>
        protected void GetConfig()
        {
            try
            {
                AppDirectory = Application.StartupPath;
                Config cf = new Config(AppDirectory);

                HostIP = cf._HostIP;
                HostName = cf._HostName;
                HostPort = cf._HostPort;
                programType = cf._programType;
                startProgram = cf._startProgram;


                //서버목록을 불러온다.
                if (File.Exists(this.ServerListFile))
                {
                    DataSet dsServerList = new DataSet();           //서버목록
                    dsServerList.ReadXml(this.ServerListFile);
                    this.ServerList = new List<ServerInfo>();

                    foreach (DataRow dr in dsServerList.Tables[0].Rows)
                    {
                        ServerInfo srvInfo = new ServerInfo();
                        srvInfo.No = dr["No"].ToString();
                        srvInfo.IP = dr["IP"].ToString();
                        srvInfo.Name = dr["Name"].ToString();
                        srvInfo.Port = dr["Port"].ToString();

                        this.ServerList.Add(srvInfo);
                    }
                }
                else
                {
                    this.ServerList = new List<ServerInfo>();
                    ServerInfo srvInfo = new ServerInfo();
                    srvInfo.No = "1";
                    srvInfo.IP = this.HostIP;
                    srvInfo.Name = this.HostName;
                    srvInfo.Port = this.HostPort;
                    this.ServerList.Add(srvInfo);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("프로그램 구동 중 에러가 발생 했습니다.\r\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 핑체크하기
        /// </summary>
        protected void PingCheck()
        {
            bool connection = false;
            int tryCount = 0;
            List<string> checkIP = new List<string>();

            try
            {
                connection = CheckPing.TestPing(HostIP);
                if (false == checkIP.Contains(this.HostIP))
                    checkIP.Add(this.HostIP);

                if (false == connection && 1 < this.ServerList.Count)
                {
                    tryCount++;

                    //등록된 서버목록을 돌아가면서 체크
                    foreach (ServerInfo srv in this.ServerList)
                    {
                        if (false == checkIP.Contains(srv.IP))
                        {
                            checkIP.Add(srv.IP);
                            if (CheckPing.TestPing(srv.IP))
                            {
                                connection = true;
                                this.HostIP = srv.IP;
                                this.HostName = srv.Name;
                                this.HostPort = srv.Port;
                                break;
                            }
                        }
                    }
                }


                if (true == connection)
                {
                    //백업 (매월 1, 11, 21)에 실행한다.
                    //if (DateTime.Now.Day == 1 || DateTime.Now.Day == 11 || DateTime.Now.Day == 21)
                    //    BackUpResult();

                    //현재 서버정보를 환경설정파일에 등록한다.
                    Config cf = new Config(Application.StartupPath);
                    cf.setConfigServerInfo(this.HostIP, this.HostName, this.HostPort);
                    cf = null;

                    //프로그램 실행
                    Process.Start(GlobalVar.RIMAGE_MEDICAL_CLIENT + ".exe");
                }
                else
                {
                    showMessage sm = new showMessage();
                    sm.Owner = this;
                    sm.ShowDialog();
                    sm.Dispose();
                }
            }
            catch (Exception ex)
            {
                RimageKorea.ErrorLog.LogWrite(this, "프로그램 구동 중 에러가 발생 했습니다.\r\n" + ex.ToString(), Application.StartupPath);
                MessageBox.Show("프로그램 구동 중 에러가 발생 했습니다.\r\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 저장된 결과 백업하기
        /// </summary>
        private void BackUpResult()
        {
            string response = "";
            string webaddress = "";
            webaddress = "http://" + HostIP + "/RimageWeb/DataBackUp.aspx";

            try
            {
                using (WebClient webc = new WebClient())
                {
                    NameValueCollection nvcol = new NameValueCollection();

                    byte[] ResponseByte = webc.UploadValues(webaddress, nvcol);
                    response = Encoding.UTF8.GetString(ResponseByte);

                    webc.Dispose();
                }
            }
            catch
            {
                //
            }
        }
    }
}