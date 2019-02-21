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
        string AppDirectory;
        string ServerListFile;
        List<ServerInfo> ServerList;    //서버목록

        public FrmStart()
        {
            InitializeComponent();

            ErrorLog.TraceWrite(this, "-- Program Start -- ", Application.StartupPath);
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

                this.HostIP = cf._HostIP;
                this.HostName = cf._HostName;
                this.HostPort = cf._HostPort;

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
            List<string> checkIP = new List<string>();

            try
            {                
                if (this.ServerList != null && this.ServerList.Count > 1)
                {
                    //등록된 서버목록을 돌아가면서 체크
                    foreach (ServerInfo srv in this.ServerList)
                    {
                        if (!checkIP.Contains(srv.IP))
                        {
                            if (CheckPing.TestPing(srv.IP))
                            {
                                checkIP.Add(srv.IP);
                            }
                        }
                    }
                }

                if (checkIP.Count > 0)
                {
                    //백업 (매월 1, 11, 21)에 실행한다.
                    //if (DateTime.Now.Day == 1 || DateTime.Now.Day == 11 || DateTime.Now.Day == 21)
                    //    BackUpResult();
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
                ErrorLog.LogWrite(this, "프로그램 구동 중 에러가 발생 했습니다.\r\n" + ex.ToString(), Application.StartupPath);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;

            this.GetConfig();
            this.PingCheck();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmStart_Load(object sender, EventArgs e)
        {
            KillProcess.DelProcess(GlobalVar.RIMAGE_CONFIG_SETTING);
            KillProcess.DelProcess(GlobalVar.RIMAGE_MEDICAL_CLIENT);
            
            this.ServerListFile = Path.Combine(Application.StartupPath, "ServerList.xml");

            this.timer1.Enabled = true;
        }
    }
}