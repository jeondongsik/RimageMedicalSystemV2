using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Xml;
using System.Runtime.InteropServices;
using DevExpress.XtraEditors;

using Rimage.Client.Api;
using Rimage.Client.Api.Exception;

using RimageKorea;
using DicomReader;
using RimageReport;
using System.Linq;

namespace RimageMedicalSystemV2
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Reduce flicker
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6)
                {
                    cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                }
                return cp;
            }
        }

        #region Windows API dll Import
        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern int FindWindowEx(int hWnd1, int hWnd2, string lpsz1, string lpsz2);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lparam);
        [DllImport("user32.dll")]
        public static extern int GetParent(int hWnd);

        [DllImport("user32")]
        private static extern int FlashWindow(System.IntPtr hWnd, int bInvert);
        [DllImport("user32")]
        public static extern IntPtr GetForegroundWindow();
        #endregion

        #region --Class Member--
        const int WM_COPYDATA = 0x004A;

        public string AppDirectory = Directory.GetCurrentDirectory();
        public delegate void ServerStatus(Dictionary<string, Hashtable> status);
        public delegate void SysExecption(ErrorInfo err);
        public delegate void BurningTrace(DiscStatusForDisplay trace);
        public delegate void HookingComplete();

        /// <summary>
        /// 서버 설정/상태 값 받기위한 Delegate
        /// </summary>
        public ServerStatus serverStatusDel;
        /// <summary>
        /// 시스템오류를 받기 위한 Delegate
        /// </summary>
        public SysExecption systemListenerDel;
        /// <summary>
        /// 굽기 진행 상태를 받기 위한 Delegate
        /// </summary>
        public BurningTrace burningTraceDel;

        public HookingComplete hookingComp;
        public HookingComplete hookingComp2;
        ////private SystemListener systemListener;
        //private ServerListener serverEventListener;
        string patientName = string.Empty;
        string patientID = string.Empty;
        string patientSex = string.Empty;
        string patientBirthDay = string.Empty;
        string patientAge = string.Empty;
        string patientFolderName;
        string patStudy = string.Empty;
        string patStudyDesc = string.Empty;
        string patModality = string.Empty;
        string programType = "1";
        string LocalShareFolder;
        string CDLabelFile;
        string DVDLabelFile;
        string MergeFileFolder;
        string MergeFileServerFolder;
        string ServerNetworkDrive;
        string DicomDownloadFolder;
        string MyIP;
        string ClientId;
        string HospitalName;
        string logFilePath;
        long copyFileLen = 0;
        string LabelPrintYN;
        string MergePrint;
        string CDMaxSize;
        bool UseLabelPrint = true;      //CD 프린트 여부
        string MsgBoxShowCheck = "3";
        int CDRemainLimit = 0;          //CD, DVD수량 최소보유수량
        int RibbonRemainLimit = 0;      //Ribbon 최소보유수량
        int BinCheckTime = 0;           //Bin 체크시간
        string AutoExecute = "0";       //자동실행 조건
        string AutoExecuteHookingType = "0";    //후킹유형
        string ReportView = "Y";        //Report보기 여부
        string DelType = "0";           //0.삭제, 1.휴지통으로
        string AutoCloseType = "N";     //Y.Success창만 닫기        
        bool checkCopying = false;      //파일복사중인지 여부
        string DeleteAfterBurn = "0";   //0.삭제안함, 1.성공했을때만 삭제, 2.성공,실패 모두 삭제
        string DownloadCheckbyFileName = "N";   //다운로드 완료를 "end.txt" 파일로 체크할지 여부
        string DelFolderWhenClose = "Y";    //프로그램 종료시 폴더 삭제
        int retentionPeriod = 1;        //Order history 저장일수
        string ServerListFile;
        List<ServerInfo> ServerList;    //서버목록
        string ServerType;              //서버유형
        string RimageSystemFolderPath;
        string MdbFileName = "PiView.mdb";
        //int configCDRBinCount = 0;      //CDR Bin갯수
        //int configDVDRBinCount = 0;     //DVDR Bin갯수

        string ProgramCaption;          //프로그램 캡션
        string BurnPatientKind;         //NORM : 단일, MULTY : 다중
        string FolderPattern = "0";     //환자명 가져오기는 패턴

        //2015.09.06 add
        string PopUpAlamYN;         //굽기결과 팝업여부
        string PopUpKeepTime;       //굽기결과 팝업 유지시간(초)
        string PopUpSelPatInfoYN;   //멀티 환자 선택 수정창 팝업여부

        int HookSleepTime1 = 3000;      //후킹후 SleepTime(ProgramType1)
        int HookSleepTime2 = 5000;      //후킹후 SleepTime(ProgramType2)
        string ManualExeBtnView;
        
        Dictionary<string, string> dicPatientList = null;       //Dicom에 포함된 환자목록
        Dictionary<string, string> dicPatientListForMerge = null;   //다중환자 굽기시 머지파일에 쓰여질 환자 목록
        int PatientCount = 0;           //Dicom에 포함된 환자수

        //환자정보가 위치한 폴더
        string dicomCDFolder = string.Empty;
        //종료시 종료할 것인지 확인 여부
        string CloseConfirm = "N";
        //굽기 종료시 소리로 알림 여부
        string SoundAlam = "N";

        //조회 진행중인지 체크
        bool isSearching = false;
        // 창을 잡은 시각
        DateTime HookingTimestamp;
        // 굽기전 파일 복사 여부
        string FileCopyYN = "N";
        // 에러 발생시 팝업 여부
        string ErrorPupupUse = "Y";
        /// <summary>
        /// 복사신청서 자동 인쇄 여부
        /// </summary>
        string AutoPrintApp = "N";
        //2016.12.24 add
        string DeleteInJobFolder = "N";

        //2018.02.20 add
        string DisableMultiPatient = "N";
        /// <summary>
        /// DVD 최대크기
        /// </summary>
        string DvdMaxSize = "0";

        //Bin별 Input Type을 담기 위한 변수
        string AutoLoaderMediaType = "";
        string nowConfigBin = "1";
        private int BinCount;
        string nowBin1Set = string.Empty;
        string nowBin2Set = string.Empty;
        string nowBin3Set = string.Empty;
        string nowBin4Set = string.Empty;

        /*Bin별 Cd수량 및 Ribbon 수량을 체크하기 위한 변수***********/
        string nowBinNumber = "1";
        string nowPrinter = "1";
        int nowRemainCD_Qty = 0;
        int nowRemainDVD_Qty = 0;
        int nowRemainCMYRibbon_Qty = 0;
        int nowRemainTransferRibbon_Qty = 0;

        int nowRemainCD1 = 0;
        int nowRemainCD2 = 0;
        int nowRemainCD3 = 0;
        int nowRemainCD4 = 0;
        /***********************************************************/

        Dictionary<string, string> idic = null;
        Dictionary<string, string> mdic = null;
        Dictionary<string, string> odic = null;
        List<string> lstFolderToDelete = new List<string>();     //삭제해야할 폴더 목록

        public const int WM_CLOSE = 0x0010; //닫기
        Dictionary<string, int> dicWhnd = null;                 //후킹으로 잡아낸 window 핸들 저장
        Dictionary<string, int> dicWhndNexus = null;            //후킹으로 잡아낸 window 핸들 저장-Nexus용

        Dictionary<string, string> DBConnInfo = null;           //DB 접속정보

        BurnOrderedInfoEntity burnOrderInfo = null;             //굽기 명령 정보 종합

        CheckDownImages hookChecker = null;                     //// 울산병원의 후킹 체크 객체
        CheckDownComplete hookChecker2 = null;                  //// 폴더사이즈 체크하여 다운로드 완료 체크

        /// <summary>
        /// 연결된 서버 목록
        /// </summary>
        List<RimageServerInfo> connectedServers = null;

        string LastHostIP;
        string LastHostName;
        string LastHostPort;

        /// <summary>
        /// 현재 선택한 서버 정보
        /// </summary>
        public ServerInfo NowSeletedServer = null;

        BurnProgressChecker burnProcessChecker = null;
        private int nowRemainDVD_DL_Qty;
        #endregion

        public MainForm()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            try
            {
                ErrorLog.TraceWrite(this, "-- Program Initialize  -- ", Application.StartupPath);

                this.systemListenerDel = new SysExecption(GetSysExecption);
                this.serverStatusDel = new ServerStatus(GetServerConfig);
                this.burningTraceDel = new BurningTrace(GetOrderStatus);

                this.idic = new Dictionary<string, string>();
                this.mdic = new Dictionary<string, string>();
                this.odic = new Dictionary<string, string>();
                this.dicWhnd = new Dictionary<string, int>();
                this.dicWhndNexus = new Dictionary<string, int>();
                this.ServerListFile = Path.Combine(Application.StartupPath, "ServerList.xml");

                this.GetConfig();
                this.InitializeControlValues();

                ////환경설정 후에 정의
                this.hookingComp = new HookingComplete(hooking_Complete);
                this.hookingComp2 = new HookingComplete(hooking_Complete2);

                ////자동굽기 여부에 따라 타이머 사용 설정
                if (this.AutoExecute == "0")
                {
                    this.tmrHookChecker.Enabled = false;
                }
                else
                {
                    //// 9.울산병원인 경우
                    if (this.AutoExecuteHookingType == "9")
                    {
                        this.hookChecker = new CheckDownImages(this, this.DicomDownloadFolder);
                    }
                    else if (this.AutoExecuteHookingType == "11")
                    {
                        //// 11.폴더사이즈를 체크하여 다운로드 완료 체크하는 경우
                        this.hookChecker2 = new CheckDownComplete(this, this.DicomDownloadFolder);
                    }
                    else
                    {
                        //// 윈도우메시지 체크하는 타이머
                        this.tmrHookChecker.Enabled = true;
                    }
                }
          
                if (this.ManualExeBtnView.Equals("Y"))
                {
                    this.buttonRetry.Enabled = true;
                }
                else
                {
                    this.buttonRetry.Enabled = false;
                }

                KillProcess.DelProcess(GlobalVar.RIMAGE_ENTERANCE);

                //// 인피니티 팍스 실행 프로세스 죽인다.
                KillProcess.DelProcess("SCDBurn");
                KillProcess.DelProcess("CDBurner");
            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite(this, ex.ToString(), Application.StartupPath);
            }
        }

        /// <summary>
        /// 폼로드 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.tmrDownloadChecker.Stop();

                if (string.IsNullOrWhiteSpace(this.ProgramCaption) == true)
                    this.Text = GlobalVar.PRODUCT_NAME;
                else
                    this.Text = this.ProgramCaption;

                Version assemVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string clientVersion = string.Format("{0}.{1}.{2}.{3}", assemVersion.Major, assemVersion.Minor, assemVersion.Build, assemVersion.Revision);
                this.Text += " :: Version " + clientVersion;

                this.Text += "  " + DateTime.Now.ToString("yyyy년 MM월 dd일");

                this.SetDriveInfo();

                //// 폴더 생성
                ExecuteWhenStart.CreateBaseDirectory(Application.StartupPath);

                if (0 < this.retentionPeriod)
                {
                    this.btnOrderedList.Visible = true;
                }
                else
                {
                    this.btnOrderedList.Visible = false;
                }

                this.DeleteLog();

                //// 다운로드 폴더 체크하여 없으면 생성하자
                if (false == string.IsNullOrWhiteSpace(this.DicomDownloadFolder) && false == Directory.Exists(this.DicomDownloadFolder))
                {
                    Directory.CreateDirectory(this.DicomDownloadFolder);
                }

                //// 로그 체크 시작
                this.burnProcessChecker = new BurnProgressChecker(this);
                this.burnProcessChecker.StartTimer();
            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite(this, ex.ToString(), Application.StartupPath);
            }
        }

        /// <summary>
        /// 컨트롤값 초기화
        /// </summary>
        private void InitializeControlValues()
        {
            //// UserControl 에서 초기화
        }

        /// <summary>
        /// 환자 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {

        }



        /// <summary>
        /// 서버 설정값 가져오기
        /// </summary>
        public void GetConfig()
        {
            try
            {
                AppDirectory = Application.StartupPath;
                Config cf = new Config(AppDirectory);

                this.LastHostIP = cf._HostIP;
                this.LastHostName = cf._HostName;
                this.LastHostPort = cf._HostPort;
                this.programType = cf._programType;

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
                    srvInfo.IP = cf._HostIP;
                    srvInfo.Name = cf._HostName;
                    srvInfo.Port = cf._HostPort;
                    this.ServerList.Add(srvInfo);
                }

                //// 서버목록 바인딩
                this.SetServerList();

                //// 마지막 선택했던 서버가 선택되도록 처리                
                //// 핑체크만 한다.                
                if (CheckPing.TestPing(this.LastHostIP))
                {
                    this.NowSeletedServer = this.GetServerInfo(this.LastHostIP);
                    this.SelectServerLabel(this.LastHostIP, true);
                }
                else
                {
                    this.SelectServerLabel(this.LastHostIP, false);
                }

                this.LocalShareFolder = cf._LocalShareFolder;
                this.CDLabelFile = cf._CDLabelFile;
                this.DVDLabelFile = cf._DVDLabelFile;
                this.MergeFileFolder = cf._MergeFileFolder;
                this.MergeFileServerFolder = cf._MergeFileServerFolder;
                this.ServerNetworkDrive = cf._ServerNetworkDrive;
                this.DicomDownloadFolder = cf._DicomDownloadFolder;
                this.HospitalName = cf._HospitalName;
                this.logFilePath = cf._LogFilePath;
                this.LabelPrintYN = cf._LabelPrint;
                this.MergePrint = cf._MergePrint;
                if (this.MergePrint.Length != 7)
                {
                    if (this.MergePrint.Length == 6)
                        this.MergePrint += "N";
                    else
                        this.MergePrint = "YYNYNNN";
                }

                this.CDMaxSize = cf._CDMaxSize;
                if (this.CDMaxSize.Length < 4 && this.CDMaxSize != "")
                {
                    this.CDMaxSize = (Convert.ToInt32(this.CDMaxSize) * 1024 * 1024).ToString();
                }

                this.CDRemainLimit = Convert.ToInt32(cf._CDRemainQty);
                this.RibbonRemainLimit = Convert.ToInt32(cf._RibbonRemainQty);
                this.BinCheckTime = Convert.ToInt32(cf._BinCheckTime);
                this.AutoExecute = cf._AutoExecute;
                this.AutoExecuteHookingType = cf._AutoExecuteHookingType;
                this.ReportView = cf._ReportView;
                this.DelType = cf._DelType;
                this.AutoCloseType = cf._AutoCloseType;
                this.DeleteAfterBurn = (String.IsNullOrWhiteSpace(cf._DeleteAfterBurn)) ? "0" : cf._DeleteAfterBurn;
                this.DownloadCheckbyFileName = (String.IsNullOrWhiteSpace(cf._DownloadCheckbyFileName)) ? "N" : cf._DownloadCheckbyFileName;
                this.DelFolderWhenClose = (String.IsNullOrWhiteSpace(cf._DelFolderWhenClose)) ? "Y" : cf._DelFolderWhenClose;
                this.retentionPeriod = (String.IsNullOrWhiteSpace(cf._RetentionPeriod)) ? 1 : Convert.ToInt32(cf._RetentionPeriod);
                this.MsgBoxShowCheck = cf._MsgBoxShow;
                this.ProgramCaption = cf._ProgramCaption;
                this.BurnPatientKind = cf._BurnPatientKind;
                this.FolderPattern = cf._FolderPattern;

                this.PopUpAlamYN = (String.IsNullOrWhiteSpace(cf._PopUpAlamYN)) ? "Y" : cf._PopUpAlamYN;
                this.PopUpKeepTime = (String.IsNullOrWhiteSpace(cf._PopUpKeepTime)) ? "20" : cf._PopUpKeepTime;
                this.PopUpSelPatInfoYN = (String.IsNullOrWhiteSpace(cf._PopUpSelPatInfoYN)) ? "Y" : cf._PopUpSelPatInfoYN;

                this.HookSleepTime1 = Convert.ToInt32((String.IsNullOrWhiteSpace(cf._HookSleepTime1) ? "3000" : cf._HookSleepTime1));
                this.HookSleepTime2 = Convert.ToInt32((String.IsNullOrWhiteSpace(cf._HookSleepTime2) ? "5000" : cf._HookSleepTime2));

                this.ManualExeBtnView = (String.IsNullOrWhiteSpace(cf._ManualExeBtnView)) ? "N" : cf._ManualExeBtnView;

                this.CloseConfirm = (String.IsNullOrWhiteSpace(cf._CloseConfirm)) ? "N" : cf._CloseConfirm;
                this.SoundAlam = (String.IsNullOrWhiteSpace(cf._SoundAlam)) ? "N" : cf._SoundAlam;

                this.FileCopyYN = (String.IsNullOrWhiteSpace(cf._FileCopyYN)) ? "N" : cf._FileCopyYN;
                this.ErrorPupupUse = (String.IsNullOrWhiteSpace(cf._ErrorPupupUse)) ? "Y" : cf._ErrorPupupUse;
                this.AutoPrintApp = (String.IsNullOrWhiteSpace(cf._AutoPrintApp)) ? "N" : cf._AutoPrintApp;

                this.DeleteInJobFolder = (String.IsNullOrWhiteSpace(cf._DeleteInJobFolder)) ? "N" : cf._DeleteInJobFolder;

                //// 다운체크파일명을 전역변수에 저장
                GlobalVar.DOWN_CHK_FL_NM = (string.IsNullOrWhiteSpace(cf._DownCheckFileName)) ? "end.txt" : cf._DownCheckFileName;

                //// 다중환자 사용 안함 
                this.DisableMultiPatient = (string.IsNullOrWhiteSpace(cf._DisableMultiPatient)) ? "N" : cf._DisableMultiPatient;

                this.DvdMaxSize = (string.IsNullOrWhiteSpace(cf._DvdMaxSize)) ? "4831838208" : cf._DvdMaxSize;

                if (cf._CDPrintYN == "N")
                    UseLabelPrint = false;
                else
                    UseLabelPrint = true;

                if (cf._MsgBoxShow == "Y" || string.IsNullOrWhiteSpace(MsgBoxShowCheck))
                    MsgBoxShowCheck = "3";

                this.ServerType = (String.IsNullOrWhiteSpace(cf._ServerType)) ? "R" : cf._ServerType;

                this.MyIP = GetMyIP.MyIP();
                this.ClientId = this.ServerType + this.MyIP.Replace(".", "_") + "_" + DateTime.Now.ToString("ddHHmmss") + RimageKorea.RandomOrderNumber.GetNewOrderNumber2();

                if (this.ServerType.Equals("L"))
                {
                    DirectoryInfo di = new DirectoryInfo(this.MergeFileFolder);
                    this.RimageSystemFolderPath = di.Parent.FullName;
                }

                //DB접속정보
                this.DBConnInfo = GetPatientNameFromDB.GetDBConn(Path.Combine(Application.StartupPath, GlobalVar.DBCONN_FILE_NAME));

                //환자정보가 위치한 폴더
                this.dicomCDFolder = this.DicomDownloadFolder;
                if (this.AutoExecuteHookingType == "8")
                {
                    //8.Deit pacs인 경우
                    this.dicomCDFolder = this.DicomDownloadFolder + "\\DICOMCD";
                }

                this.HookingTimestamp = DateTime.Now.AddSeconds(-10);

                //// 폴더사이즈체크시간
                if (!string.IsNullOrWhiteSpace(cf._FolderSizeCheckTime))
                {
                    if (!int.TryParse(cf._FolderSizeCheckTime, out GlobalVar.FolderSizeCheckTime))
                        GlobalVar.FolderSizeCheckTime = 5;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite(this, ex.ToString(), Application.StartupPath);
                ErrMsgShow("환경설정값 가져오기 에러\r\n" + ex.Message, "Rimage Message : GetConfig", ex);
            }
        }

        /// <summary>
        /// 메시지 박스 Clear
        /// </summary>
        private void ClearMessageBox()
        {
            this.txtMessages.EditValue = "";
            this.txtStatusView.Clear();
        }

        /// <summary>
        /// --굽기 프로그램에서 발생한 에러 메시지--
        /// </summary>
        /// <param name="sysExecption"></param>
        public void GetSysExecption(ErrorInfo err)
        {
            try
            {
                this.txtMessages.Text = string.Format("{0} 서버 오류 발생", err.ServerIP);

                if (err.Code == "1001")
                {

                }
                else
                {

                }

                ////if (sysExecption.Message.Contains("connection broken"))
                ////{
                ////    ////Thread.Sleep(1000);

                ////    //////다시 연결 시도
                ////    //////연결되어 있지 않다면 연결한다.
                ////    ////this.ServerConnect(srv.Host, srv.Port, srv.Name);
                ////    ////Thread.Sleep(1000);

                ////    this.RecoverOrders(srv.Host);

                ////    textboxMessages.Text = sysExecption.Message;
                ////    this.SelectServerLabel(srv.Host, false);

                ////    ErrMsgShow(string.Format("서버 ({0}) 상태를 확인한 후 오른쪽 IP 버튼을 클릭하여 다시 연결하세요.\r\n", srv.Host), "Rimage Message : sysExecption");
                ////}
                ////else
                ////{
                ////    ErrMsgShow(string.Format("시스템에 일시적인 장애가 발생하였습니다.\r\n\r\n네트워크 상태 또는 서버({0}) 상태를 확인하세요.\r\n", srv.Host), "Rimage Message : sysExecption");
                ////}

                //DialogResult rst = MessageBox.Show("시스템에 일시적인 장애가 발생하였습니다.\r\n\r\n진행중인 작업은 모두 취소되었습니다.\r\n\r\n네트워크 상태 또는 서버 상태를 확인하세요.\r\n\r\n\"OK\"을 클릭하면 서버에 다시 연결합니다.", "Server Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //if (rst == System.Windows.Forms.DialogResult.OK)
                //{
                //    this.textboxMessages.AppendText("서버에 다시 연결합니다. 잠시 기다려 주세요.\r\n");
                //    Thread.Sleep(10000);
                //    buttonConnect_Click(null, null);
                //}
            }
            catch { }
        }

        /// <summary>
        /// 서버 설정 및 상태값 가져오기
        /// </summary>
        /// <param name="result"></param>
        private void GetServerConfig(Dictionary<string, Hashtable> result)
        {
            try
            {
                //// 현재 선택된 서버IP에 해당되는 로그만 가져온다.
                if (result.ContainsKey(this.NowSeletedServer.IP))
                {
                    //// 설정값
                    this.SetServerConfig(result[this.NowSeletedServer.IP]["CONFIG"].ToString());

                    //// 상태값
                    this.SetServerStatus(result[this.NowSeletedServer.IP]["STATUS"].ToString());
                }
            }
            catch { }
        }

        /// <summary>
        /// 서버 설정값 세팅
        /// </summary>
        /// <param name="strProdConfig"></param>
        private void SetServerConfig(string strProdConfig)
        {
            try
            {
                DataTable dt = XMLParser.ConvertXmlToDataTable(strProdConfig, "BinSetting");

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.BinCount = dt.Rows.Count;

                    //// 기본 세팅
                    this.nowBin1Set = "CDRInputOnly";
                    this.nowBin2Set = "CDRInputOnly";
                    this.nowBin3Set = "CDRInputOnly";
                    this.nowBin4Set = "CDRInputOnly";

                    foreach (DataRow dr in dt.Rows)
                    {
                        switch (dr["BinNumber"].ToString())
                        {
                            case "1":
                                this.nowBin1Set = dr["Setting"].ToString();
                                break;
                            case "2":
                                this.nowBin2Set = dr["Setting"].ToString();
                                break;
                            case "3":
                                this.nowBin3Set = dr["Setting"].ToString();
                                break;
                            case "4":
                                this.nowBin4Set = dr["Setting"].ToString();
                                break;
                            default:
                                break;
                        }
                    }
                }

                /*
                 <BinSetting BinNumber="1" Setting="CDRInputOnly" AvailableBinSettings="Input,Output,OutputReject,Reject,Collate,ReservedInput,CDRInputOnly,DVDRInputOnly,DVDR-DLInputOnly,SOQ_CDRInputOnly,SOQ_DVDRInputOnly,SOQ_DVDR-DLInputOnly"/>
                 <BinSetting BinNumber="2" Setting="DVDRInputOnly" AvailableBinSettings="Input,Output,OutputReject,Reject,Collate,ReservedInput,CDRInputOnly,DVDRInputOnly,DVDR-DLInputOnly,SOQ_CDRInputOnly,SOQ_DVDRInputOnly,SOQ_DVDR-DLInputOnly"/>
                 */
            }
            catch (Exception ex)
            {
                DialogResult rst = MessageBox.Show("시스템에 일시적인 장애가 발생하였습니다.\r\n\r\n네트워크 상태 또는 서버 상태를 확인하세요.\r\n" + ex.ToString(), "Server Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 서버 상태값 세팅
        /// </summary>
        /// <param name="strProdConfig"></param>
        private void SetServerStatus(string strProdConfig)
        {
            try
            {
                /*
                  <Bin Number="1" Level="56" DiscsRemaining="28" Usage="Input"/>
                  <Bin Number="2" Level="0" DiscsRemaining="0" Usage="Input"/>
                  <Printer Number="1" Offline="false" LabelsRemaining="494" DiscCount="122" RibbonPanelSetsRemaining="494" RetransferSheetPanelsRemaining="938"/>
                 */

                DataTable dt = XMLParser.ConvertXmlToDataTable(strProdConfig, "Bin");

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.nowRemainCD_Qty = 0;
                    this.nowRemainDVD_Qty = 0;
                    this.nowRemainDVD_DL_Qty = 0;

                    this.txtBin1Discs.Text = "0";
                    this.txtBin2Discs.Text = "0";
                    this.txtBin3Discs.Text = "0";
                    this.txtBin4Discs.Text = "0";

                    foreach (DataRow dr in dt.Rows)
                    {
                        int qty = Convert.ToInt32(dr["DiscsRemaining"]);

                        switch (dr["Number"].ToString())
                        {
                            case "1":
                                this.txtBin1Discs.Text = qty.ToString();
                                this.PlusRemainQuantity(this.nowBin1Set, qty);
                                break;
                            case "2":
                                this.txtBin2Discs.Text = qty.ToString();
                                this.PlusRemainQuantity(this.nowBin2Set, qty);
                                break;
                            case "3":
                                this.txtBin3Discs.Text = qty.ToString();
                                this.PlusRemainQuantity(this.nowBin3Set, qty);
                                break;
                            case "4":
                                this.txtBin4Discs.Text = qty.ToString();
                                this.PlusRemainQuantity(this.nowBin4Set, qty);
                                break;
                            default:
                                break;
                        }
                    }
                }

                DataTable dt2 = XMLParser.ConvertXmlToDataTable(strProdConfig, "Printer");

                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    this.txtCMYRibbons.Text = Utils.CheckNull(dt2.Rows[0]["RibbonPanelSetsRemaining"]);
                    this.txtRetansferRibbons.Text = Utils.CheckNull(dt2.Rows[0]["RetransferSheetPanelsRemaining"]);

                    this.nowRemainCMYRibbon_Qty = Convert.ToInt32(Utils.CheckNull(dt2.Rows[0]["RibbonPanelSetsRemaining"], "0"));
                    this.nowRemainTransferRibbon_Qty = Convert.ToInt32(Utils.CheckNull(dt2.Rows[0]["RetransferSheetPanelsRemaining"], "0"));
                }
            }
            catch
            {
                MessageBox.Show("시스템에 일시적인 장애가 발생하였습니다.\r\n\r\n네트워크 상태 또는 서버 상태를 확인하세요.\r\n\r\n\"OK\"을 클릭하면 서버에 다시 연결합니다.", "Server Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 남은 수량 더하기
        /// </summary>
        /// <param name="binSet"></param>
        /// <param name="qty"></param>
        private void PlusRemainQuantity(string binSet, int qty)
        {
            if (binSet == "DVDR-DLInputOnly")
            {
                this.nowRemainDVD_DL_Qty += qty;
            }
            else if (binSet == "DVDRInputOnly")
            {
                this.nowRemainDVD_Qty += qty;
            }
            else
            {
                this.nowRemainCD_Qty += qty;
            }
        }

        /// <summary>
        /// 굽기 결과값을 받아서 처리한다.
        /// </summary>
        /// <param name="trace"></param>
        public void GetOrderStatus(DiscStatusForDisplay trace)
        {
            try
            {
                if (trace == null)
                    return;

                //if (this.OrderView.Items == null)
                //    return;

                //if (this.OrderView.Items.Count == 0)
                //    return;

                //////// 해당 OrderID Row를 찾아서 업데이트한다.                
                ////ListViewItem item = new ListViewItem();

                ////try
                ////{
                ////    item = OrderView.FindItemWithText(trace.OrderID, true, 0);
                ////}
                ////catch
                ////{
                ////    return;
                ////}

                ////if (item == null)
                ////    return;

                ////if (Utils.CheckNull(item.SubItems[4].Tag) == "Y")
                ////    return;

                ////BurnOrderedInfoEntity orderInfo = item.Tag as BurnOrderedInfoEntity;

                ////item.SubItems[4].Text = trace.StateString;
                ////item.SubItems[4].Tag = trace.Finish;
                ////item.SubItems[12].Tag = trace.StatusType;
                ////item.SubItems[5].Text = trace.PercentCompleted;
                ////item.SubItems[6].Text = trace.ResultMessage;
                ////item.SubItems[12].Text = trace.ResultCode;

                ////Color retCol = Color.Black;

                ////if (trace.ResultCode == "2")
                ////    retCol = Color.Blue;
                ////else if (trace.ResultCode == "9")
                ////    retCol = Color.Red;

                ////item.ForeColor = retCol;

                //////// 굽기완료시
                ////if (trace.Finish == "Y")
                ////{
                ////    //// 오른쪽 하단에 메시지를 보여준다.
                ////    if (this.PopUpAlamYN == "Y")
                ////    {
                ////        if (trace.ResultCode == "2")
                ////            this.NotifyBurningResult(string.Format("{0}[{1}] - {2}{3}", item.SubItems[2].Text, item.SubItems[3].Text, trace.ResultMessage, "\r\n굽기가 완료되었습니다."));
                ////        else if (trace.ResultCode == "9")
                ////            this.NotifyBurningResult(string.Format("{0}[{1}] - 취소/실패{2}", item.SubItems[2].Text, item.SubItems[3].Text, "\r\n굽기가 완료되었습니다."));
                ////    }

                ////    //// 라벨프린트(스티커) : 성공시에만..
                ////    if (trace.ResultCode == "2" && this.LabelPrintYN == "Y")
                ////    {
                ////        this.LabelPrint(orderInfo.patNo, orderInfo.patName, orderInfo.patSex, orderInfo.patAge, orderInfo.copies.ToString());
                ////    }

                ////    //// EditList Xml파일삭제하자.
                ////    this.DelEditListFile(orderInfo.EditListPath);
                ////    odic.Remove(orderInfo.DiscOrder.OrderID);

                ////    //// 환자 폴더 삭제
                ////    if (this.DeleteAfterBurn == "1" && trace.ResultCode == "2")
                ////    {
                ////        this.DeleteDestinationFolder(orderInfo.patFolderFullPath);
                ////    }
                ////    if (this.DeleteAfterBurn == "2")
                ////    {
                ////        this.DeleteDestinationFolder(orderInfo.patFolderFullPath);
                ////    }

                ////    this.txtStatusView.AppendText(item.SubItems[3].Text + " : " + trace.StatusType + trace.State + " " + trace.DeviceCurrentState + " " + trace.PercentCompleted + "%" + "\r\n");

                ////    //// 완료된 오더폴더에 종료파일 생성한다.
                ////    FileControl.CreateTextFile(Path.Combine(GlobalVar.ProgramExecuteFolder, GlobalVar.ORDER_FOLDER, orderInfo.DiscOrder.OrderID, GlobalVar.BURN_CHK_FL_NM));

                ////    //// 오더 폴더,파일 삭제
                ////    FileControl.DeleteBurnEndOrder(orderInfo.DiscOrder.OrderID);

                ////    //// RDMS가 정상종료되었는지 체크-> 아니라면 종료 처리
                ////    try
                ////    {
                ////        Process prc = Process.GetProcessById(Convert.ToInt32(trace.ProcessID));
                ////        if (prc != null)
                ////            prc.Kill();
                ////    }
                ////    catch { }
                ////}
            }
            catch (Exception ex)
            {
                this.txtStatusView.AppendText(ex.Message);
                ErrorLog.LogWrite(this, ex.ToString(), Application.StartupPath);
            }
        }

        /// <summary>
        /// 에러처리
        /// </summary>
        /// <param name="msg">에러문자열</param>
        /// <param name="caption">Caption</param>
        private void ErrMsgShow(string msg, string caption, Exception ex = null)
        {
            this.Cursor = Cursors.Default;
            this.txtMessages.Text = msg;
            ErrorLog.LogWrite(this, ((ex == null) ? msg : ex.ToString()), Application.StartupPath);

            if (this.ErrorPupupUse.Equals("Y"))
                MessageBox.Show(msg, caption);
        }

        /// <summary>
        /// 등록된 서버 보여주기
        /// </summary>
        private void SetServerList()
        {
            try
            {
                if (this.ServerList != null && this.ServerList.Count > 0)
                {
                    int i = 1;
                    foreach (var srv in this.ServerList)
                    {
                        Label lbl = this.grpServerList.Controls.Find(string.Format("Server{0}", i.ToString()), false)[0] as Label;
                        lbl.Text = string.Format("[{0}] {1}", srv.No, srv.IP);                        
                        lbl.Image = global::RimageMedicalSystemV2.Properties.Resources.add_16x16;
                        lbl.Tag = srv;

                        i++;
                    }
                }
                else
                {
                    MessageBox.Show("등록된 서버가 없습니다.\r\n환경설정에서 서버를 등록해 주세요!", "Rimage Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch { }
        }

        /// <summary>
        /// 울산병원 후킹-환자정보 다운로드 완료 후 처리
        /// </summary>
        private void hooking_Complete()
        {
            if (this.isSearching)
            {
                //조회 진행중이면 return
                return;
            }
            else
            {
                if (this.hookChecker == null)
                {
                    this.hookChecker = new CheckDownImages(this, this.DicomDownloadFolder);
                    return;
                }

                try
                {
                    //잠시 쉬자
                    Thread.Sleep(this.HookSleepTime1);

                    // 프로그램 실행
                    if (this.AutoExecute == "1")
                    {
                        //조회 실행
                        this.isSearching = true;
                        this.btnSearch_Click(null, null);

                        this.hookChecker.StartTimer1();
                    }
                    else if (this.AutoExecute == "2")
                    {
                        //조회 및 굽기까지 실행
                        this.isSearching = true;
                        this.btnSearch_Click(null, null);
                        Thread.Sleep(300);

                        this.hookChecker.StartTimer1();

                        this.btnBurn_Click(null, null);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 폴더사이즈 체크하여 다운로드 체크하는 경우 완료 후 처리
        /// </summary>
        private void hooking_Complete2()
        {
            if (this.isSearching)
            {
                //조회 진행중이면 return
                return;
            }
            else
            {
                if (this.hookChecker2 == null)
                {
                    this.hookChecker2 = new CheckDownComplete(this, this.DicomDownloadFolder);
                    return;
                }

                try
                {
                    //잠시 쉬자
                    Thread.Sleep(this.HookSleepTime1);

                    // 프로그램 실행
                    if (this.AutoExecute == "1")
                    {
                        //조회 실행
                        this.isSearching = true;
                        this.btnSearch_Click(null, null);

                        this.hookChecker2.StartTimer();
                    }
                    else if (this.AutoExecute == "2")
                    {
                        //조회 및 굽기까지 실행
                        this.isSearching = true;
                        this.btnSearch_Click(null, null);
                        Thread.Sleep(300);

                        this.hookChecker2.StartTimer();

                        this.btnBurn_Click(null, null);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 굽기 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBurn_Click(object sender, EventArgs e)
        {
            
        }



        /// <summary>
        /// --데이터 파일이 저장되는 Drive 용량 보여주기--
        /// </summary>
        private void SetDriveInfo()
        {
            try
            {
                //// 데이터 파일이 저장되는 Drive 용량 보여주기
                DriveInfo di = FileControl.GetDriveInfo(this.LocalShareFolder.Substring(0, 1) + ":\\");
                decimal TotalSize = Math.Round(Convert.ToDecimal(di.TotalSize) / 1024 / 1024 / 1024, 2, MidpointRounding.AwayFromZero);
                decimal UseSize = Math.Round(Convert.ToDecimal(di.TotalSize - di.AvailableFreeSpace) / 1024 / 1024 / 1024, 2, MidpointRounding.AwayFromZero);
                decimal AvailableFreeSpace = Math.Round(Convert.ToDecimal(di.AvailableFreeSpace) / 1024 / 1024 / 1024, 2, MidpointRounding.AwayFromZero);
                decimal AvailableFreeSpaceRate = Math.Round(Convert.ToDecimal(di.AvailableFreeSpace) / Convert.ToDecimal(di.TotalSize) * 100, 1, MidpointRounding.AwayFromZero);

                string str = string.Format("Current Save Drive : {0} [전체용량 - {1}GB] [사용량 : {2}G, 잔여량 : {3}G]", 
                    di.Name, TotalSize.ToString(), UseSize.ToString(), AvailableFreeSpace.ToString());

                if (10 >= AvailableFreeSpaceRate)
                {
                    this.lblDriveStatus.Text = string.Format("{0}{1}", str, "  [★ 드라이브 용량이 " + AvailableFreeSpaceRate.ToString() + "% 남았습니다.]");
                }
                else
                {
                    this.lblDriveStatus.Text = str;
                }
            }
            catch
            {
                this.lblDriveStatus.Text = "  데이터 다운로드 폴더를 설정해 주세요.";
            }
        }

        /// <summary>
        /// Log 삭제하기
        /// </summary>
        private void DeleteLog()
        {
            if (this.backgroundWorker2.IsBusy == false)
            {
                LogDelete clsLD = new LogDelete();
                clsLD.folderLoot = Application.StartupPath;
                clsLD.retentionPeriod = this.retentionPeriod;
                clsLD.localSharedFolder = this.LocalShareFolder;
                clsLD.downloadFolder = this.DicomDownloadFolder;
                clsLD.deleteInJobFolder = this.DeleteInJobFolder;

                this.backgroundWorker2.RunWorkerAsync(clsLD);
            }
        }

        /// <summary>
        /// Log 삭제 비동기 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                LogDelete cls = e.Argument as LogDelete;
                e.Result = 0;
                cls.Workflow(worker, e);
            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite(this, ex.ToString(), Application.StartupPath);
            }
        }

        /// <summary>
        /// 로그 삭제 완료 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                RimageKorea.ErrorLog.LogWrite(this, e.Error.ToString(), Application.StartupPath);
            }

            this.backgroundWorker2.CancelAsync();

            //폴더가 완전히 삭제되었는지 다시 한번 체크해보자.
            this.txtStatusView.AppendText(this.retentionPeriod.ToString() + "일 전의 로그와 데이터 삭제가 완료되었습니다." + Environment.NewLine);
            this.CheckDeletePatientFolder();
        }

        /// <summary>
        /// 보관기간 이전의 환자폴더들이 남아 있는지 체크
        /// </summary>
        private void CheckDeletePatientFolder()
        {
            if (this.programType == "1")
                CheckDeletePatientFolder1();
            else
                CheckDeletePatientFolder2();
        }

        /// <summary>
        /// 보관기간 이전의 환자폴더들이 남아 있는지 체크-Type1
        /// </summary>
        private void CheckDeletePatientFolder1()
        {
            try
            {
                DirectoryInfo sharedFolder = new DirectoryInfo(this.LocalShareFolder);
                DirectoryInfo downFolder = new DirectoryInfo(this.DicomDownloadFolder);
                int k = 0;
                int p = this.retentionPeriod;
                if (1 > this.retentionPeriod)
                    p = 1;

                if (sharedFolder.Exists)
                {
                    this.Cursor = Cursors.WaitCursor;

                    string pattern = "*";

                    //Deit_Pacs인 경우..
                    if (this.AutoExecuteHookingType == "8")
                        pattern = "MC_*";

                    foreach (DirectoryInfo di in sharedFolder.GetDirectories(pattern))
                    {
                        if (di.FullName != downFolder.FullName && di.FullName != sharedFolder.FullName)
                        {
                            if (di.CreationTime.Date <= DateTime.Now.AddDays(p * -1).Date)
                            {
                                if (FileControl.DeleteFolder(di.FullName, false) == false)
                                {
                                    FileControl.FileAttributeChange(di.FullName);
                                    if (FileControl.DeleteFolder(di.FullName, false) == false)
                                    {
                                        k++;
                                    }
                                }
                            }
                        }
                    }
                    this.Cursor = Cursors.Default;
                }

                if (k > 0)
                {
                    //ErrMsgShow(this.retentionPeriod.ToString() + "일 전에 다운받은 환자데이터 폴더가 존재합니다.\r\n자동으로 삭제가 안되었을 수 있습니다.\r\n수동으로 삭제하여 주세요.", "Information");
                    MessageBox.Show(this.retentionPeriod.ToString() + "일 전에 다운받은 환자데이터 폴더가 존재합니다.\r\n자동으로 삭제가 안되었을 수 있습니다.\r\n수동으로 삭제하여 주세요.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            { }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 보관기간 이전의 환자폴더들이 남아 있는지 체크-Type2
        /// </summary>
        private void CheckDeletePatientFolder2()
        {
            try
            {
                DirectoryInfo sharedFolder = new DirectoryInfo(this.LocalShareFolder);
                int k = 0;
                int p = this.retentionPeriod;
                if (1 > this.retentionPeriod)
                    p = 1;

                if (sharedFolder.Exists)
                {
                    this.Cursor = Cursors.WaitCursor;
                    foreach (DirectoryInfo di in sharedFolder.GetDirectories("*.*"))
                    {
                        if (di.CreationTime.Date <= DateTime.Now.AddDays(retentionPeriod * -1).Date && di.FullName != sharedFolder.FullName)
                        {
                            if (FileControl.DeleteFolder(di.FullName, false) == false)
                            {
                                FileControl.FileAttributeChange(di.FullName);
                                if (FileControl.DeleteFolder(di.FullName, false) == false)
                                {
                                    k++;
                                }
                            }
                        }
                    }
                    this.Cursor = Cursors.Default;
                }

                if (k > 0)
                {
                    //ErrMsgShow(this.retentionPeriod.ToString() + "일 전에 다운받은 환자데이터 폴더가 존재합니다.\r\n자동으로 삭제가 안되었을 수 있습니다.\r\n수동으로 삭제하여 주세요.", "Information");
                    MessageBox.Show(this.retentionPeriod.ToString() + "일 전에 다운받은 환자데이터 폴더가 존재합니다.\r\n자동으로 삭제가 안되었을 수 있습니다.\r\n수동으로 삭제하여 주세요.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            { }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 서버목록에서 IP에 해당하는 서버정보 반환
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private ServerInfo GetServerInfo(string ip)
        {
            ServerInfo srv = null;

            try
            {
                var hst = this.ServerList.Where(c => c.IP == ip);
                if (hst != null && hst.Count() > 0)
                {
                    srv = hst.First();
                }
            }
            catch { }

            return srv;
        }

        /// <summary>
        /// 선택한 서버 Border색상 변경
        /// </summary>
        /// <param name="srv"></param>
        private void SelectServerLabel(string hostIP, bool connected)
        {
            try
            {
                foreach (Control ctrl in this.grpServerList.Controls)
                {
                    LabelControl lbl = ctrl as LabelControl;

                    if (ctrl.Tag == null)
                    {
                        lbl.Appearance.Image = global::RimageMedicalSystemV2.Properties.Resources.add_16x162;
                        lbl.Appearance.BorderColor = Color.FromArgb(105, 105, 105);
                        continue;
                    }

                    ServerInfo s = ctrl.Tag as ServerInfo;

                    if (s.IP == hostIP)
                    {
                        lbl.Appearance.BorderColor = Color.FromArgb(192, 255, 255);
                        if (connected)
                        {
                            lbl.Appearance.Image = global::RimageMedicalSystemV2.Properties.Resources.highpriority_16x16;
                        }
                        else
                        {
                            lbl.Appearance.Image = global::RimageMedicalSystemV2.Properties.Resources.cancel_16x16;
                        }
                    }
                    else
                    {
                        lbl.Appearance.Image = global::RimageMedicalSystemV2.Properties.Resources.add_16x162;
                        lbl.Appearance.BorderColor = Color.FromArgb(105, 105, 105);
                    }
                }

                //// 서버 설정 가져오기
                if (connected)
                {
                    this.GetConnectedServerInfo();
                }
            }
            catch { }
        }

        /// <summary>
        /// 자동실행 처리(후킹)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrHookChecker_Tick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 주기적으로 다운로드 완료된 폴더가 존재하는지 체크하자. - Type2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrDownloadChecker_Tick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 빈체크 (CD 수량)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrBinCheker_Tick(object sender, EventArgs e)
        {

        }
    }

    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpData;
    }
}
