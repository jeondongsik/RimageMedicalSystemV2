using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Runtime.InteropServices;
using DevExpress.XtraEditors;

using RimageKorea;
using RimageReport;
using System.Linq;
using System.Diagnostics;

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
        public delegate void CopyToUSBComplete(string message, DiscStatusForDisplay trace);

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
        public CopyToUSBComplete copyToUSBComplete;

        string patientName = string.Empty;
        string patientID = string.Empty;
        string patientSex = string.Empty;
        string patientBirthDay = string.Empty;
        string patientAge = string.Empty;
        string patStudy = string.Empty;
        string patStudyDesc = string.Empty;
        string patModality = string.Empty;
        string MyIP;
        string ClientId;
        long copyFileLen = 0;
        bool checkCopying = false;      //파일복사중인지 여부
        string ServerListFile;
        List<ServerInfo> ServerList;    //서버목록
        string RimageSystemFolderPath;

        //환자정보가 위치한 폴더
        string dicomCDFolder = string.Empty;
        //조회 진행중인지 체크
        bool isSearching = false;
        // 창을 잡은 시각
        DateTime HookingTimestamp;

        //Bin별 Input Type을 담기 위한 변수
        string AutoLoaderMediaType = "";
        private int BinCount;
        string nowBin1Set = string.Empty;
        string nowBin2Set = string.Empty;
        string nowBin3Set = string.Empty;
        string nowBin4Set = string.Empty;

        /*Bin별 Cd수량 및 Ribbon 수량을 체크하기 위한 변수***********/        
        int nowRemainCD_Qty = 0;
        int nowRemainDVD_Qty = 0;
        int nowRemainCMYRibbon_Qty = 0;
        int nowRemainTransferRibbon_Qty = 0;
        /***********************************************************/

        /// <summary>
        /// 굽기 폴더 경로 저장
        /// </summary>
        ////Dictionary<string, string> idic = null;
        ////Dictionary<string, string> mdic = null;
        ////Dictionary<string, string> odic = null;
        List<string> lstFolderToDelete = new List<string>();     //삭제해야할 폴더 목록

        public const int WM_CLOSE = 0x0010; //닫기
        Dictionary<string, int> dicWhnd = null;                 //후킹으로 잡아낸 window 핸들 저장
        Dictionary<string, int> dicWhndNexus = null;            //후킹으로 잡아낸 window 핸들 저장-Nexus용
        Dictionary<string, string> DBConnInfo = null;           //DB 접속정보        
        CheckDownImages hookChecker = null;                     //// 울산병원의 후킹 체크 객체
        CheckDownComplete hookChecker2 = null;                  //// 폴더사이즈 체크하여 다운로드 완료 체크

        string LastHostIP;
        string LastHostName;
        string LastHostPort;

        /// <summary>
        /// 현재 선택한 서버 정보
        /// </summary>
        public ServerInfo NowSeletedServer = null;

        BurnProgressChecker burnProcessChecker = null;
        private int nowRemainDVD_DL_Qty;

        /// <summary>
        /// 굽기 대기 목록
        /// </summary>
        Dictionary<string, string> _BurnPendingList = null;

        /// <summary>
        /// 굽기 목록 Datasource
        /// </summary>
        List<BurnOrderedInfoEntity> _BurningList = null;
        
        /// <summary>
        /// 작업 대상 유형 - CD,DVD 굽기 또는 USB 복사
        /// </summary>
        public MediaType mediaType = MediaType.CD_DVD;
        
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
                this.copyToUSBComplete = new CopyToUSBComplete(CompleteCopyToUSB);

                this.dicWhnd = new Dictionary<string, int>();
                this.dicWhndNexus = new Dictionary<string, int>();
                this.ServerListFile = Path.Combine(Application.StartupPath, "ServerList.xml");

                //// 실행중인 RMDS 죽임.
                this.KillBurnPrograms();

                //// 설정값 가져오기
                this.GetConfig();

                //// 컨트롤 초기화
                this.InitializeControlValues();
                this.InitEnabledButton();

                ////환경설정 후에 정의
                this.hookingComp = new HookingComplete(hooking_Complete);
                this.hookingComp2 = new HookingComplete(hooking_Complete2);

                ////자동굽기 여부에 따라 타이머 사용 설정
                if (GlobalVar.configEntity.AutoExecute == "0")
                {
                    this.tmrHookChecker.Enabled = false;
                    this.tmrDownloadChecker.Enabled = false;
                }
                else
                {
                    if (GlobalVar.configEntity.DownloadCheckbyFileName == "Y")
                    {
                        this.tmrDownloadChecker.Enabled = true;
                        this.tmrDownloadChecker.Start();
                    }
                    else
                    {
                        //// 9.울산병원인 경우
                        if (GlobalVar.configEntity.AutoExecuteHookingType == "9")
                        {
                            this.hookChecker = new CheckDownImages(this, GlobalVar.configEntity.DicomDownloadFolder);
                        }
                        else if (GlobalVar.configEntity.AutoExecuteHookingType == "11")
                        {
                            //// 11.폴더사이즈를 체크하여 다운로드 완료 체크하는 경우
                            this.hookChecker2 = new CheckDownComplete(this, GlobalVar.configEntity.DicomDownloadFolder);
                        }
                        else
                        {
                            //// 윈도우메시지 체크하는 타이머
                            this.tmrHookChecker.Enabled = true;
                        }
                    }
                }

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
                if (string.IsNullOrWhiteSpace(GlobalVar.configEntity.ProgramCaption) == true)
                    this.Text = GlobalVar.PRODUCT_NAME;
                else
                    this.Text = GlobalVar.configEntity.ProgramCaption;

                Version assemVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string clientVersion = string.Format("{0}.{1}.{2}.{3}", assemVersion.Major, assemVersion.Minor, assemVersion.Build, assemVersion.Revision);
                this.Text += " :: Version " + clientVersion;

                this.Text += "  " + DateTime.Now.ToString("yyyy년 MM월 dd일");

                this.SetDriveInfo();

                //// 폴더 생성
                ExecuteWhenStart.CreateBaseDirectory(Application.StartupPath);

                //// 로그삭제
                this.DeleteLog();

                //// 다운로드 폴더 체크하여 없으면 생성하자 - ProgramType1인 경우
                if (GlobalVar.configEntity.programType == "1")
                {
                    if (!string.IsNullOrWhiteSpace(GlobalVar.configEntity.DicomDownloadFolder) && !Directory.Exists(GlobalVar.configEntity.DicomDownloadFolder))
                    {
                        Directory.CreateDirectory(GlobalVar.configEntity.DicomDownloadFolder);
                    }
                }
                else
                {
                    this.ucPatients21.FrmParent = this;
                }

                this.gcBurninglist.DataSource = this._BurningList;

                //// 로그 체크 시작
                this.burnProcessChecker = new BurnProgressChecker(this);
                this.burnProcessChecker.StartTimer();
            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite(this, ex.ToString(), Application.StartupPath);
            }
            finally
            {
                KillProcess.DelProcess(GlobalVar.RIMAGE_ENTERANCE);
                this.grpPatInfo.Focus();
            }
        }

        /// <summary>
        /// 컨트롤값 초기화
        /// </summary>
        private void InitializeControlValues()
        {
            //// UserControl 에서 초기화
            if (GlobalVar.configEntity.programType == "1")
            {
                this.ucPatients11.Clear();
                this.ucPatients11.Visible = true;
            }
            else
            {
                this.ucPatients21.Visible = true;
            }

            this._BurningList = new List<BurnOrderedInfoEntity>();
        }

        /// <summary>
        /// 버튼 사용여부
        /// </summary>
        public void InitEnabledButton()
        {
            if (GlobalVar.configEntity.AutoExecute == "1")
            {
                this.btnSearch.Enabled = false;
                this.btnBurn.Enabled = true;
            }
            else if (GlobalVar.configEntity.AutoExecute == "2")
            {
                this.btnSearch.Enabled = false;
                this.btnBurn.Enabled = false;
            }
            else
            {
                this.btnSearch.Enabled = true;
                this.btnBurn.Enabled = true;
            }

            if (GlobalVar.configEntity.ReportView == "N")
                this.btnReport.Visible = false;
            else
                this.btnReport.Visible = true;

            if (GlobalVar.configEntity.ManualExeBtnView.Equals("Y"))
            {
                this.buttonRetry.Enabled = true;
            }
            else
            {
                this.buttonRetry.Enabled = false;
            }

            if (GlobalVar.configEntity.UseUSBCopy.Equals("Y"))
            {
                this.btnUSBCopy.Visible = true;
            }
            else
            {
                this.btnUSBCopy.Visible = false;
            }

            if (GlobalVar.configEntity.programType == "1")
            {
                //// 프로그램 Type1일 경우 환자정보 삭제 버튼 노출
                this.btnPatientDelete.Visible = true;
            }

            if (GlobalVar.configEntity.ManualExeBtnView.Equals("Y"))
            {
                this.buttonRetry.Visible = true;
                this.txtMessages.Size = new Size(439, 39);
                this.txtMessages.Location = new Point(31, 6);
            }
            else
            {
                this.buttonRetry.Visible = false;
                this.txtMessages.Size = new Size(465, 39);
                this.txtMessages.Location = new Point(5, 6);
            }

            if (0 < GlobalVar.configEntity.RetentionPeriod)
            {
                this.btnOrderedList.Visible = true;
            }
            else
            {
                this.btnOrderedList.Visible = false;
            }
        }

        /// <summary>
        /// 환자 조회 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (GlobalVar.configEntity.programType == "1")
            {
                this.SearchPatient1();
            }
            else
            {
                this.SearchPatient2();
            }
        }

        /// <summary>
        /// 프로그램 타입 1 환자 검색
        /// </summary>
        private void SearchPatient1()
        {
            this.Cursor = Cursors.WaitCursor;

            string retMessage = string.Empty;

            try
            {
                //// 컨트롤 초기화
                this.ucPatients11.Clear();
                this.txtMessages.Text = "";
                this.txtStatusView.AppendText("환자정보를 가져오고 있습니다.\r\n");

                BurnOrderedInfoEntity orderInfo = SearchPatient.Get(Application.StartupPath, new DirectoryInfo(this.dicomCDFolder), this.DBConnInfo, this.AutoLoaderMediaType);
                
                if (orderInfo != null)
                {
                    orderInfo.DicomCDFolder = this.dicomCDFolder;

                    this.ucPatients11.OrderInfo = orderInfo;
                    this.ucPatients11.ShowPatientInfo();

                    this.txtStatusView.AppendText("폴더전체크기 : " + orderInfo.FolderSize.ToString() + "bytes\r\n");

                    ////Drive 용량 다시 체크
                    this.SetDriveInfo();
                    this.txtMessages.Text = "환자정보 조회 완료.";

                    retMessage = string.Format("{0}[{1}] 조회 완료", orderInfo.patName, orderInfo.patNo);

                    StringBuilder sbTemp = new StringBuilder();
                    sbTemp.AppendLine(string.Format("ID : {0}", orderInfo.patNo));
                    sbTemp.AppendLine(string.Format("Name : {0}", orderInfo.patName));
                    sbTemp.AppendLine(string.Format("Gender : {0}", orderInfo.patSex));
                    sbTemp.AppendLine(string.Format("Age : {0}", orderInfo.patAge));
                    sbTemp.AppendLine(string.Format("BirthDay : {0}", orderInfo.patBirtyDay));
                    sbTemp.AppendLine(string.Format("Study : {0}", orderInfo.StudyModality.Replace("^^", "\r\n")));
                    this.txtStatusView.AppendText(sbTemp.ToString() + "\r\n");
                }
                else
                {
                    retMessage = "DICOMDIR 파일을 읽을 수 없거나 환자정보가 없습니다.";
                    this.ErrMsgShow(retMessage, "Rimage Error Message : DICOMDIR file is bad.");
                }
            }
            catch (Exception ex)
            {
                retMessage = "환자정보 불러오기 에러";
                this.ErrMsgShow(string.Format("환자정보 조회 중 오류가 발생하였습니다.\r\nError ▶ {0}", ex.Message), "Rimage Error Message", ex);
            }
            finally
            {
                //// 조회완료 파일 생성한다.
                FileControl.CreateTextFile(Path.Combine(GlobalVar.configEntity.DicomDownloadFolder, GlobalVar.SEEK_CHK_FL_NM));

                //// 오른쪽 하단에 메시지를 보여준다.
                if (GlobalVar.configEntity.AutoExecute == "1" && GlobalVar.configEntity.PopUpAlamYN == "Y")
                {
                    this.NotifyBurningResult(retMessage);
                }

                this.isSearching = false;
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 프로그램 타입 2 환자 검색
        /// </summary>
        private void SearchPatient2()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //// 초기화
                this.txtMessages.Text = "";
                this.txtStatusView.Text = "";

                if ("0" == GlobalVar.configEntity.AutoExecute)
                    this.txtStatusView.AppendText("환자정보를 가져오고 있습니다.\r\n");

                if (GlobalVar.configEntity.AutoExecute != "0" && GlobalVar.configEntity.DownloadCheckbyFileName == "Y")
                {
                    this.SearchDownloadFolder(GlobalVar.DOWN_CHK_FL_NM);
                }
                else
                {
                    this.SearchDownloadFolder();
                }
            }
            catch (Exception ex)
            {
                this.ErrMsgShow(string.Format("환자정보 조회 중 오류가 발생하였습니다.\r\nError ▶ {0}", ex.Message), "Rimage Error Message", ex);
            }
            finally
            {
                this.isSearching = false;
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 환자 조회
        /// </summary>
        /// <param name="checkFile"></param>
        private void SearchDownloadFolder(string checkFile = "")
        {
            string retMessage = string.Empty;
            int i = 0;

            try
            {
                DirectoryInfo dri = new DirectoryInfo(GlobalVar.configEntity.LocalShareFolder);                
                bool patExists = false;

                if (dri.Exists)
                {
                    if (this.ucPatients21.PatientInfoList == null)
                    {
                        this.ucPatients21.PatientInfoList = new List<BurnOrderedInfoEntity>();
                    }

                    foreach (DirectoryInfo sdir in dri.GetDirectories())
                    {
                        //// 현재 굽기 실행중인지 체크한다.
                        if (this._BurningList != null && this._BurningList.Count > 0)
                        {
                            patExists = this.ExistsBurningItem(sdir.Name);
                        }

                        if (!patExists)
                        {
                            BurnOrderedInfoEntity orderInfo = SearchPatient.Get(Application.StartupPath, sdir, this.DBConnInfo, this.AutoLoaderMediaType, checkFile);

                            if (orderInfo != null)
                            {
                                bool found = false;
                                orderInfo.DicomCDFolder = sdir.FullName;

                                if (this.ucPatients21.PatientInfoList.Count == 0)
                                {
                                    this.ucPatients21.PatientInfoList.Add(orderInfo);
                                    found = true;
                                }
                                else
                                {
                                    if (!this.ucPatients21.PatientInfoList.Any(p => p.DicomCDFolder == orderInfo.DicomCDFolder))
                                    {
                                        this.ucPatients21.PatientInfoList.Insert(0, orderInfo);
                                        found = true;
                                    }
                                }

                                this.ucPatients21.gcPatientlist.DataSource = this.ucPatients21.PatientInfoList;

                                if (found)
                                {
                                    this.ucPatients21.gvPatientlist.RefreshData();
                                    this.ucPatients21.gcPatientlist.RefreshDataSource();

                                    this.ucPatients21.gvPatientlist.SelectRow(0);

                                    this.txtStatusView.AppendText(string.Format("{0} {1} found.{2}", orderInfo.patNo, orderInfo.patName, Environment.NewLine));

                                    i++;
                                }
                            }
                        }

                        patExists = false;
                    }
                }
                else
                {
                    retMessage = "폴더가 존재하지 않습니다.\r\n네트워크 드라이브를 설정해주세요.";
                    ErrMsgShow(retMessage, "Rimage Message : 조회 결과");
                }

                if (i == 0)
                {
                    //자동굽기일 경우는 보여주지 않는다.
                    if ("0" == GlobalVar.configEntity.AutoExecute)
                        txtStatusView.AppendText("새로운 환자정보가 존재하지 않습니다.\r\n");
                }
                else
                {
                    retMessage = i.ToString() + " 건의 새로운 환자정보가 조회되었습니다.";
                    txtStatusView.AppendText(retMessage + "\r\n");
                }

                ////Drive 용량 다시 체크
                this.SetDriveInfo();
                this.txtMessages.Text = "환자정보 조회 완료.";
            }
            catch (Exception ex)
            {
                retMessage = "폴더 읽기 에러";
                ErrMsgShow(retMessage + "\r\n" + ex.Message, "Rimage Message : searchDownloadFolder", ex);
            }
            finally
            {
                //// 오른쪽 하단에 메시지를 보여준다.
                if (GlobalVar.configEntity.AutoExecute == "1" && GlobalVar.configEntity.PopUpAlamYN == "Y")
                {
                    this.NotifyBurningResult(retMessage);
                }
            }
        }

        /// <summary>
        /// 굽기 목록에 존재하는지 동일환자 폴더가 체크
        /// </summary>
        /// <param name="foldername"></param>
        /// <returns></returns>
        private bool ExistsBurningItem(string foldername)
        {
            try
            {
                if (this._BurningList.Any(o => o.patFolder == foldername && o.Finish != "Y"))
                    return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 굽기 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBurn_Click(object sender, EventArgs e)
        {
            if (!this.EnableServerConnect())
                return;

            this.mediaType = MediaType.CD_DVD;

            if (GlobalVar.configEntity.programType == "1")
            {
                this.ReadyBurn1();
            }
            else
            {
                this.ReadyBurn2();
            }
        }

        /// <summary>
        /// 굽기 전 사전 작업 - ProgramType1
        /// </summary>
        public void ReadyBurn1()
        {
            try
            {
                DialogResult result = System.Windows.Forms.DialogResult.Yes;
                var orderInfo = this.ucPatients11.OrderInfo;
                orderInfo.patListForMerge = orderInfo.patList;

                //// 환자정보가 2개 이상일 때는 무조건 창을 띄운다. => 환경설정값 체크 추가
                if (orderInfo.patList != null && orderInfo.patList.Count > 1 && GlobalVar.configEntity.PopUpSelPatInfoYN == "Y")
                {
                    //// 상위에서 이름을 바꿀수 있으므로 정보를 다시 넣어보자.
                    if (orderInfo.patList.ContainsKey(this.ucPatients11.txtPatientID.Text))
                        orderInfo.patList[this.ucPatients11.txtPatientID.Text] = this.ucPatients11.txtPatientName.Text;

                    FrmCheckPatientForMerge frm = new FrmCheckPatientForMerge();
                    frm.SetPatientList(orderInfo.patList);
                    result = frm.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        orderInfo.patListForMerge = frm.PatientList;
                        foreach (KeyValuePair<string, string> kvp in orderInfo.patListForMerge)
                        {
                            this.ucPatients11.txtPatientID.Text = kvp.Key;
                            this.ucPatients11.txtPatientName.Text = kvp.Value;
                            break;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;
                    this.txtMessages.Text = "굽기 준비중입니다. 잠시만 기다려 주세요.";

                    orderInfo.patName = this.ucPatients11.txtPatientName.Text;
                    orderInfo.copies = Convert.ToInt32(this.ucPatients11.cbCopies.EditValue);
                    orderInfo.mediType = Convert.ToString(this.ucPatients11.cbMedia.EditValue);

                    if (GlobalVar.configEntity.AutoExecuteHookingType == "8")
                    {
                        //// Process Kill - Deit_Burn.exe
                        KillProcess.DelProcess("Deit_Burn");
                        Thread.Sleep(500);
                        KillProcess.DelProcess("Deit_Burn");

                        if (this.backgroundWorker4.IsBusy == false)
                            this.backgroundWorker4.RunWorkerAsync("Deit_Burn");
                    }
                    else
                    {
                        this.ChageNameAndOrderBurning(true);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 폴더이름을 바꾸고 굽기 실행
        /// </summary>
        private void ChageNameAndOrderBurning(bool isPossible)
        {
            if (isPossible)
            {
                ////다운로드 폴더이름을 환자정보를 포함한 신규이름으로 변경
                ////다운로드 폴더명으로 폴더 생성
                string pNm = this.ucPatients11.txtPatientName.Text;
                if (string.IsNullOrWhiteSpace(pNm))
                    pNm = this.ucPatients11.OrderInfo.patName.Replace(" ", "");
                pNm = Utils.ReplaceSpecialWord(pNm);
                pNm = pNm.Replace(",", ".");

                //// 파일복사 후 굽기진행 설정이면
                if (GlobalVar.configEntity.FileCopyYN.Equals("Y"))
                {
                    //// 파일을 복사한 후에 굽기 실행일 경우
                    //// 환자명으로 폴더 생성
                    this.ucPatients11.OrderInfo.patFolder = FileControl.CreatePatientFolder(this.ucPatients11.OrderInfo.patNo, pNm);

                    //// 다운받은 데이터를 공유폴더로 이동시킨다.
                    this.BurnCDAfterCopyFiles();
                }
                else
                {
                    try
                    {
                        //// 폴더명을 변경한 후 굽기 명령을 보낼 경우
                        this.checkCopying = true;
                        this.ucPatients11.OrderInfo.patFolder = FileControl.ChangeDownloadFolderToPatientFolder(this.ucPatients11.OrderInfo.patNo, pNm);
                        this.checkCopying = false;

                        Thread.Sleep(300);
                        //// 굽기명령을 보낸다.
                        this.StartBurn(this.ucPatients11.OrderInfo);
                    }
                    catch (Exception ex)
                    {
                        this.ErrMsgShow("폴더명에 허용되지 않는 문자열이 있습니다.\r\n환자명에서 특수문자를 삭제한 후 다시 시도하세요.\r\n" + ex.Message, "Rimage Message : ChangeDownloadFolderToPatientFolder", ex);
                    }
                    finally
                    {
                        this.checkCopying = false;
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            else
            {
                ErrMsgShow("다른 프로세스에 의해 사용중이므로 폴더명을 바꿀 수 없습니다.", "Rimage Message : ChangeDownloadFolderToPatientFolder");
            }
        }

        /// <summary>
        /// 굽기 전 사전 작업 - ProgramType2
        /// </summary>
        /// <param name="autoExe">true : 자동실행시, false : 수동실행시 (Default)</param>
        public void ReadyBurn2(bool autoExe = false)
        {
            //// 수동모드일 경우에는 메시지 박스 경고 처리
            if (this.ucPatients21.PatientInfoList == null)
            {
                if (!autoExe)
                    MessageBox.Show("조회된 환자정보가 없습니다.", "Rimage Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.ucPatients21.PatientInfoList.Count == 0)
            {
                if (!autoExe)
                    MessageBox.Show("조회된 환자정보가 없습니다.", "Rimage Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!autoExe && this.ucPatients21.gvPatientlist.SelectedRowsCount == 0)
            {
                MessageBox.Show("환자를 선택하세요.\r\n선택된 환자가 없습니다.", "Rimage Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //// USB일 경우 한개의 정보만 가능.
            if (this.mediaType == MediaType.USB && this.ucPatients21.gvPatientlist.SelectedRowsCount > 1)
            {
                MessageBox.Show("USB 복사는 한명의 환자정보만 가능합니다.\r\n한 명의 환자만 선택하세요.", "Rimage Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //// 조회된 환자 목록을 돌면서 굽기 실행한다.///////////////////////////////////////////////
            int idx = 0;
            List<int> orderedIdx = new List<int>();

            foreach (BurnOrderedInfoEntity orderInfo in this.ucPatients21.PatientInfoList)
            {                
                if (!autoExe)
                {
                    //// 수동모드일 경우 선택한 Row만 굽기 진행한다.                    
                    if (!this.ucPatients21.gvPatientlist.IsRowSelected(idx))
                    {
                        continue;
                    }
                }

                //// USB인 경우 선택된 한명만.
                if (this.mediaType == MediaType.USB)
                {
                    if (!this.ucPatients21.gvPatientlist.IsRowSelected(idx))
                    {
                        continue;
                    }
                }

                //// 자동일 경우 현재 진행중이거나 완료된 상태인건 건너뛴다.
                if (autoExe)
                {
                    //// 현재 굽기 실행중인지 체크한다.
                    if (this._BurningList != null && this._BurningList.Count > 0)
                    {
                        if (this.ExistsBurningItem(orderInfo.patFolder))
                            continue;
                    }

                    //// 굽기완료된 폴더인지 체크(burn.end 파일체크)
                    if (CheckFiles.CheckFileExists(new DirectoryInfo(orderInfo.patFolderFullPath), GlobalVar.BURN_CHK_FL_NM))
                    {
                        continue;
                    }
                }

                //// 굽기 시작
                if (this.StartBurn(orderInfo))
                {
                    orderedIdx.Add(idx);
                }

                idx++;
            }

            //// 조회목록에서 삭제
            //// USB일 경우 복사 시작시..
            if (this.mediaType != MediaType.USB)
            {
                this.ucPatients21.RemoveAtList(orderedIdx);
            }
        }

        /// <summary>
        /// 다중굽기시 환자정보를 수동으로 선택한 후 결과를 리턴한다.
        /// </summary>
        /// <param name="dicPatList"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetCheckPatientList(Dictionary<string, string> dicPatList)
        {
            Dictionary<string, string> lst = null;

            try
            {
                DialogResult result = System.Windows.Forms.DialogResult.Yes;

                //// 다중환장굽기이고 자동실행유형이 굽기까지 실행이 아닐 때
                FrmCheckPatientForMerge frm = new FrmCheckPatientForMerge();
                frm.SetPatientList(dicPatList);
                result = frm.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    lst = frm.PatientList;
                }
            }
            catch { }

            return lst;
        }

        /// <summary>
        /// 재굽기
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <param name="reburn"></param>
        /// <returns></returns>
        public void RetryBurn(BurnOrderedInfoEntity orderInfo)
        {
            try
            {
                ////굽기 진행중인지 체크한다.
                //// 현재 굽기 실행중인지 체크한다.
                if (this._BurningList != null && this._BurningList.Count > 0)
                {
                    if (this.ExistsBurningItem(orderInfo.patFolder))
                    {
                        ////굽기 진행중임.
                        MessageBox.Show("이미 굽기진행중인 환자정보입니다.\r\n완료 후 다시 시도하세요.", "Rimage Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                //// 굽기완료 파일 삭제 (burn.end 파일체크)
                string filePath = Path.Combine(orderInfo.patFolderFullPath, GlobalVar.BURN_CHK_FL_NM);
                if (File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch { }
                }

                orderInfo.Finish = "N";

                this.StartBurn(orderInfo, true);
            }
            catch { }
        }

        /// <summary>
        /// CD 굽기
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <param name="reburn">재굽기여부</param>
        public bool StartBurn(BurnOrderedInfoEntity orderInfo, bool reburn = false)
        {
            int j = 1;

            if (this._BurningList == null)
            {
                this._BurningList = new List<BurnOrderedInfoEntity>();
            }

            j = this._BurningList.Count + 1;

            try
            {
                //// 서버 연결 체크 : USB가 아닐 경우
                if (this.mediaType != MediaType.USB && this.NowSeletedServer == null)
                {
                    this.ErrMsgShow("연결된 서버가 존재하지 않습니다.\r\n먼저 서버를 클릭하여 선택하세요.", "Rimage Message");
                    return false;
                }

                //// 체크파일이 있으면 체크한다.
                if (!CheckFiles.Exists(Path.Combine(GlobalVar.configEntity.LocalShareFolder, orderInfo.patFolder)))
                {
                    this.ErrMsgShow("환자폴더에 필수파일이 존재하지 않습니다.\r\n환자폴더를 체크하세요.", "Rimage Message");
                    return false;
                }

                //// 다중 환자 체크
                bool blContinue = true;

                if (orderInfo.patList.ContainsKey(orderInfo.patNo) == true)
                    orderInfo.patList[orderInfo.patNo] = orderInfo.patName;

                Dictionary<string, string> dicPatListMerge = orderInfo.patList;
                //// 환자정보가 2개 이상일 때는 무조건 창을 띄운다. => 환경설정값 체크 추가
                //// 다중환자사용안함이면 건너뜀
                //// USB 복사 이면 건너뜀
                if (orderInfo.patList != null && orderInfo.patList.Count > 1 && GlobalVar.configEntity.PopUpSelPatInfoYN == "Y" 
                    && GlobalVar.configEntity.DisableMultiPatient == "N" && this.mediaType != MediaType.USB)
                {
                    dicPatListMerge = this.GetCheckPatientList(orderInfo.patList);
                    if (dicPatListMerge == null)
                        blContinue = false;
                    else
                        blContinue = true;
                }

                if (!blContinue)
                {
                    return false;
                }

                //// 등록된 환자가 여러건일 경우
                if (orderInfo.patList.Count > 1 && GlobalVar.configEntity.DisableMultiPatient == "N" && this.mediaType != MediaType.USB)
                {
                    foreach (KeyValuePair<string, string> kvp in dicPatListMerge)
                    {
                        orderInfo.patNo = kvp.Key;
                        orderInfo.patName = kvp.Value;
                        break;
                    }
                }
                orderInfo.patListForMerge = dicPatListMerge;
                
                orderInfo.No = j;
                orderInfo.Result = "0";

                if (this.NowSeletedServer != null)
                {
                    orderInfo.TargetServer = this.NowSeletedServer.ShallowCopy();
                }

                //// 다중환자 사용일 경우에만 체크
                if (GlobalVar.configEntity.DisableMultiPatient == "N" && this.mediaType != MediaType.USB)
                {
                    if (orderInfo.patListForMerge != null && orderInfo.patListForMerge.Count > 1)
                    {
                        orderInfo.BurnPatientKind = "Y";
                        orderInfo.Count = orderInfo.patListForMerge.Count;
                    }
                    else
                    {
                        orderInfo.BurnPatientKind = "N";
                        orderInfo.Count = 1;
                    }
                }
                orderInfo.patFolderFullPath = Path.Combine(GlobalVar.configEntity.LocalShareFolder, orderInfo.patFolder);

                string progMessage = string.Empty;
                if (this.mediaType == MediaType.USB)
                {
                    orderInfo.mediType = "USB";
                    progMessage = "Submitted for Coping to USB";
                }
                else
                {
                    progMessage = "Submitted for Imaging";
                }

                orderInfo.OrderId = string.Format("{0}_{1}_{2}{3}_ORD", Utils.ReplaceSpecialWord(orderInfo.patName).Replace(" ", "").Replace(".", "_").Trim(), orderInfo.patNo, DateTime.Now.ToString("ddHHmmss"), RandomOrderNumber.GetNewOrderNumber2());
                orderInfo.patDate = DateTime.Now.ToShortDateString();
                orderInfo.Progress = progMessage;
                orderInfo.ProcessingRate = "0 %";
                orderInfo.BurnState = "대기";
                orderInfo.ServerNo = string.Format("[{0}]", this.NowSeletedServer.No);
                orderInfo.Sort = 0;
                
                ///////////////////////////////////////////////////////////////////////////////////////                    
                orderInfo.StartDateTime = Utils.GetNowTime();
                orderInfo.JobPath = GlobalVar.configEntity.ServerNetworkDrive + orderInfo.patFolder;

                if (GlobalVar.configEntity.AutoExecuteHookingType == "8")
                {
                    //// 8.Deit pacs인 경우
                    orderInfo.JobPath = GlobalVar.configEntity.ServerNetworkDrive + orderInfo.patFolder + "\\DICOMCD";
                }

                if (GlobalVar.configEntity.ServerType.Equals("R"))
                {
                    //// 원격 서버일 경우
                    orderInfo.MegPath = GlobalVar.configEntity.MergeFileServerFolder.Replace("ServerIP", orderInfo.TargetServer.IP);
                    orderInfo.MegFilePath = GlobalVar.configEntity.MergeFileFolder.Replace("ServerIP", orderInfo.TargetServer.IP);
                }
                else
                {
                    //// 로컬 서버일 경우 : 로컬경로 그대로 설정하면 됨.
                    orderInfo.MegPath = GlobalVar.configEntity.MergeFileServerFolder;
                    orderInfo.MegFilePath = GlobalVar.configEntity.MergeFileFolder;
                }

                //// 머지파일 Fullpath
                orderInfo.MegPath = string.Format("{0}{1}{2}{3}.txt", orderInfo.MegPath,
                    Utils.ReplaceSpecialWord(orderInfo.patNo), DateTime.Now.ToString("MMddHHmmss"),
                    DateTime.Now.Millisecond.ToString().PadLeft(3, '0'));

                //// 머지파일 생성
                //// USB일 경우에는 생성하지 않음.
                if (GlobalVar.configEntity.UseLabelPrint && this.mediaType != MediaType.USB)
                {
                    if (!string.IsNullOrWhiteSpace(orderInfo.patAge))
                    {
                        ////재굽기 아닐 경우에만
                        if (!reburn)
                            orderInfo.patSex = string.Format("{0}/{1}", orderInfo.patSex, orderInfo.patAge);
                    }

                    try
                    {
                        //// 여러환자 정보일 때..
                        if (orderInfo.BurnPatientKind.Equals("Y"))
                            FileControl.CreateMergeFile(orderInfo.MegPath, orderInfo.patListForMerge, orderInfo.patDate);
                        else
                            FileControl.CreateMergeFile(orderInfo.MegFilePath, GlobalVar.configEntity.MergePrint, orderInfo.patFolder, orderInfo.patDate
                                , orderInfo.patNo, orderInfo.patName, orderInfo.patSex, orderInfo.StudyModality, orderInfo.Modality,
                                orderInfo.patName, orderInfo.MegPath, "", this.DBConnInfo);
                    }
                    catch { }

                    //// 머지파일 다시 체크 - 파일이 존재하지 않는다면 다시 생성
                    if (!File.Exists(orderInfo.MegPath))
                    {
                        try
                        {
                            //// 여러환자 정보일 때..
                            if (orderInfo.BurnPatientKind.Equals("Y") == true)
                                FileControl.CreateMergeFile(orderInfo.MegPath, orderInfo.patListForMerge, orderInfo.patDate);
                            else
                                FileControl.CreateMergeFile(orderInfo.MegFilePath, GlobalVar.configEntity.MergePrint, orderInfo.patFolder, orderInfo.patDate
                                , orderInfo.patNo, orderInfo.patName, orderInfo.patSex, orderInfo.StudyModality, orderInfo.Modality,
                                orderInfo.patName, orderInfo.MegPath, "", this.DBConnInfo);
                        }
                        catch (Exception ex)
                        {
                            this.ErrMsgShow("MergeFile 생성 에러\r\n" + ex.Message, "Rimage Message : createMergeFile", ex);
                            return false;
                        }
                    }
                }

                //// 굽기 주문 보낸다./////////////////////////////////////////////////////////////////////////////
                string RimageSystemFolder = string.Empty;
                try
                {
                    if (GlobalVar.configEntity.ServerType.Equals("R"))
                        RimageSystemFolder = string.Format("\\\\{0}\\Rimage", orderInfo.TargetServer.IP);
                    else
                        RimageSystemFolder = this.RimageSystemFolderPath;   //CSystemManager.GetInstance().GetUncSystemFolder();
                }
                catch (Exception me)
                {
                    MessageBox.Show(me.Message, "Rimage Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                orderInfo.RimageSystemFolder = RimageSystemFolder;

                try
                {
                    DiscData discOrder;
                    discOrder = new DiscData();

                    discOrder.MyProperty = 1;
                    discOrder.ClientID = MainFormBiz.GetClientId(this.NowSeletedServer.IP, GlobalVar.configEntity.ServerType, this.MyIP);
                    discOrder.OrderID = orderInfo.OrderId;
                    discOrder.VolumeName = GlobalVar.configEntity.HospitalName;
                    discOrder.ParentFolder = orderInfo.JobPath;
                    discOrder.Copies = orderInfo.copies.ToString();
                    discOrder.MediaType = orderInfo.mediType;
                    discOrder.OrderStatus = progMessage;
                    discOrder.ImagePath = RimageSystemFolder + "\\" + discOrder.OrderID + ".img";
                    discOrder.EditListPath = RimageSystemFolder + "\\EditList\\" + discOrder.OrderID + ".xml";
                    discOrder.UseLabelPrint = GlobalVar.configEntity.UseLabelPrint;

                    string labelFileName = string.Empty;
                    if (GlobalVar.configEntity.ServerType.Equals("R"))
                    {
                        if (orderInfo.mediType.Equals("CDR"))
                            labelFileName = GlobalVar.configEntity.CDLabelFile.Replace("ServerIP", orderInfo.TargetServer.IP);
                        else
                            labelFileName = GlobalVar.configEntity.DVDLabelFile.Replace("ServerIP", orderInfo.TargetServer.IP);
                    }
                    else
                    {
                        if (orderInfo.mediType.Equals("CDR"))
                            labelFileName = GlobalVar.configEntity.CDLabelFile;
                        else
                            labelFileName = GlobalVar.configEntity.DVDLabelFile;
                    }

                    //// Multi 환자일 경우
                    if (orderInfo.BurnPatientKind.Equals("Y"))
                    {
                        FileInfo fi = new FileInfo(labelFileName);
                        labelFileName = fi.FullName.Replace(fi.Name, GlobalVar.COMBO_LabelFile);

                        discOrder.MultiPatientYN = "Y";
                    }

                    discOrder.LabelName = labelFileName;
                    discOrder.MergeName = orderInfo.MegPath;

                    //// USB가 아닐 경우에
                    if (this.mediaType != MediaType.USB)
                    {
                        //// EditList 파일 생성
                        string editListXml = FileControl.createEditListXml(orderInfo.ImgFiles.EditList, orderInfo.DicomCDFolder, orderInfo.JobPath, RimageSystemFolder, orderInfo.JobPath);
                        FileControl.createEditListFile(editListXml, discOrder.EditListPath);

                        //// EditList 파일이 없으면 다시 생성
                        if (!File.Exists(discOrder.EditListPath))
                        {
                            editListXml = FileControl.createEditListXml(orderInfo.ImgFiles.EditList, orderInfo.DicomCDFolder, orderInfo.JobPath, RimageSystemFolder, orderInfo.JobPath);
                            FileControl.createEditListFile(editListXml, discOrder.EditListPath);
                        }

                        orderInfo.EditListXml = editListXml;
                        
                        string imageXml = CreateOrderXml.CreateImageOrder(discOrder, orderInfo.TargetServer.IP, RimageSystemFolder);
                        
                        orderInfo.OrderXml = imageXml;
                        orderInfo.DiscOrder = discOrder;
                    }

                    //////////////////////////////////////////////////////////////////////////////////////////
                    //// 오더정보를 JSON 파일로 변경하여 저장한다.
                    string ordJson = JsonParser.ConvertToJsonString(orderInfo);
                    string ordFile = FileControl.CreateOrderJsonFile(orderInfo.OrderId, ordJson);

                    //// 오더 파일이 생성되었는지 체크
                    if (!File.Exists(ordFile))
                    {
                        //// 다시 한번 시도
                        FileControl.CreateOrderJsonFile(orderInfo.OrderId, ordJson);
                    }

                    //// 굽기 실행
                    if (this.mediaType == MediaType.USB)
                    {
                        //// USB 일 경우 여기까지 통과되면 환자 객체를 USB화면으로 넘긴다.
                        this.ExeCopyToUSB(orderInfo);
                        return true;
                    }
                    else 
                    {
                        ////this.SaveRimageWeb(orderInfo);

                        //// 굽기 프로그램을 실행한다.
                        Process proc = Process.Start(GlobalVar.BURM_PROGRAM, string.Format("O|{0}|{1}", orderInfo.OrderId, this.Handle.ToInt32().ToString()));

                        //// 파일 생성이 완료된 후  프로그램 실행을 위해 1초 쉰다.
                        //// Thread.Sleep(1000);

                        //// 대기 목록에 추가
                        this.AddBurnPendingList(orderInfo.OrderId, ordJson);

                        //////// 그리드에 추가한다.
                        ////this.AddBurningList(orderInfo);

                        //////// 프로그램 1일 경우 조회된 값 초기화.
                        ////if (GlobalVar.configEntity.programType == "1")
                        ////    this.ucPatients11.Clear();
                    }

                    
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    ErrMsgShow("굽기 명령 실행 중 에러 발생\r\n" + ex.Message, "Rimage Message : Submit_Order", ex);
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                this.ErrMsgShow("굽기 명령 실행 중 에러 발생\r\n" + ex.Message, "Rimage Message : BurningOrder", ex);
            }

            return true;
        }

        /// <summary>
        /// 굽기 대기 목록에 추가
        /// </summary>
        /// <param name="orderInfo"></param>
        public bool AddBurnPendingList(string orderID, string orderInfo)
        {
            try
            {
                if (this._BurnPendingList == null)
                    this._BurnPendingList = new Dictionary<string, string>();

                if (!this._BurnPendingList.ContainsKey(orderID))
                    this._BurnPendingList.Add(orderID, orderInfo);

                return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 굽기 대기 목록에서 삭제 - 굽기 제대로 시작 안됨.
        /// </summary>
        /// <param name="orderID"></param>
        public void RemoveBurningList(string orderID)
        {
            try
            {
                if (orderID == "NULL")
                {
                    this.txtMessages.Text = "굽기 전송 실패 : 명령정보 생성 오류.";
                    MessageBox.Show("굽기 명령 전송에 실패하였습니다.\r\n다시 시도해 주세요.", "Rimage Error");
                }

                if (this._BurnPendingList == null)
                    return;

                if (this._BurnPendingList.ContainsKey(orderID))
                {
                    if (this._BurnPendingList.ContainsKey(orderID))
                        this._BurnPendingList.Remove(orderID);

                    this.txtMessages.Text = string.Format("[{0}] 굽기 전송 실패.", orderID);
                    MessageBox.Show("굽기 명령 전송에 실패하였습니다.\r\n다시 시도해 주세요.", "Rimage Error");
                }
            }
            catch { }
        }

        /// <summary>
        /// 굽기 목록에 추가
        /// </summary>
        /// <param name="orderID"></param>
        public void AddBurningList(string orderID)
        {
            try
            {
                if (this._BurnPendingList == null)
                    return;

                if (this._BurnPendingList.ContainsKey(orderID))
                {
                    BurnOrderedInfoEntity orderInfo = JsonParser.ConvertToBurnOrderedInfoEntity(this._BurnPendingList[orderID]);

                    //// 그리드에 추가한다.
                    this.AddBurningList(orderInfo);

                    //// 대기 목록에서 삭제
                    this._BurnPendingList.Remove(orderID);

                    //// 프로그램 1일 경우 조회된 값 초기화.
                    if (GlobalVar.configEntity.programType == "1")
                        this.ucPatients11.Clear();

                    //// 복사 신청서 인쇄 - 환경설정에 따라, 멀티환자가 아닐경우에만.
                    if (GlobalVar.configEntity.AutoPrintApp.Equals("Y") && orderInfo.BurnPatientKind.Equals("N"))
                    {
                        xFormReport report = new xFormReport();
                        report.PrintCopyRequestForm(orderInfo.patNo, orderInfo.patName, orderInfo.StudyModality);
                        report.Dispose();
                    }

                    this.txtStatusView.AppendText(string.Format("[{0}]으로 굽기주문 전송하였습니다.\r\n", orderInfo.TargetServer.IP));
                    ErrorLog.TraceWrite(this, string.Format("++ 전송대상 서버:[{0}]-{1}", orderInfo.TargetServer.No, orderInfo.TargetServer.IP), Application.StartupPath);
                }
            }
            catch { }
        }

        private void SaveRimageWeb(BurnOrderedInfoEntity orderInfo)
        {
            ////결과저장
            WebUtils.InsertResult(orderInfo.OrderId,
                      orderInfo.StartDateTime,
                      Utils.GetNowTime(),
                      orderInfo.patNo,
                      orderInfo.patName,
                      orderInfo.copies.ToString(),
                      orderInfo.mediType,
                      orderInfo.mediSize,
                      "?",
                      ((orderInfo.BurnPatientKind.Equals("Y") || orderInfo.patList.Count > 1) ? orderInfo.DicomDescription : orderInfo.StudyModality),
                      Utils.CheckNull(orderInfo.BurnPatientKind, "N"),
                      orderInfo.TargetServer.IP,
                      this.MyIP);
        }

        /// <summary>
        /// 굽기 목록에 추가
        /// </summary>
        /// <param name="orderInfo"></param>
        public void AddBurningList(BurnOrderedInfoEntity orderInfo)
        {
            try
            {
                if (this._BurningList.Count == 0)
                    this._BurningList.Add(orderInfo);
                else
                    this._BurningList.Insert(0, orderInfo);

                this.gcBurninglist.RefreshDataSource();
                this.gvBurninglist.RefreshData();

                this.UpdateBurningGrid(orderInfo.OrderId, orderInfo.Progress, "", orderInfo.BurnState);
            }
            catch { }
        }

        /// <summary>
        /// 굽기 목록 상태값 업데이트
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="progress"></param>
        /// <param name="percent"></param>
        /// <param name="state"></param>
        private void UpdateBurningGrid(string orderId, string progress, string percent, string state)
        {
            if (this._BurningList != null)
            {
                if (this._BurningList.Any(o => o.OrderId == orderId))
                {
                    var oInfo = this._BurningList.Where(b => b.OrderId == orderId).First();

                    if (progress != "")
                        oInfo.Progress = progress;
                    if (percent != "")
                        oInfo.ProcessingRate = percent;
                    if (state != "")
                        oInfo.BurnState = state;

                    this.gcBurninglist.RefreshDataSource();
                    this.gvBurninglist.RefreshData();
                }
            }
        }

        /// <summary>
        /// 굽기 목록 상태값 업데이트
        /// </summary>
        /// <param name="trace"></param>
        private void UpdateBurningGrid(DiscStatusForDisplay trace)
        {
            if (this._BurningList != null)
            {
                if (this._BurningList.Any(o => o.OrderId == trace.OrderID))
                {
                    var oInfo = this._BurningList.Where(b => b.OrderId == trace.OrderID).First();

                    oInfo.Progress = trace.StateString;
                    oInfo.ProcessingRate = string.Format("{0} %", trace.PercentCompleted);
                    oInfo.BurnState = trace.ResultMessage;
                    oInfo.Result = trace.ResultCode;

                    this.gcBurninglist.RefreshDataSource();
                    this.gvBurninglist.RefreshData();
                }
            }
        }

        /// <summary>
        /// 복사할 대상 디렉토리 정보를 이용하여 폴더를 생성한 후 파일을 복사한다.
        /// </summary>
        public void BurnCDAfterCopyFiles()
        {
            this.checkCopying = true;
            this.copyFileLen = 0;
            DirectoryInfo dirInfo = new DirectoryInfo(GlobalVar.configEntity.DicomDownloadFolder);

            try
            {
                if (dirInfo.Exists)
                {
                    this.Cursor = Cursors.WaitCursor;

                    if (this.backgroundWorker1.IsBusy == false)
                    {
                        Worker cls = new Worker();
                        cls.srcDir = dirInfo;
                        cls.DicomDownloadFolder = GlobalVar.configEntity.DicomDownloadFolder;
                        cls.LocalShareFolder = GlobalVar.configEntity.LocalShareFolder;
                        cls.patientFolderName = this.ucPatients11.OrderInfo.patFolder;
                        cls.startupPath = Application.StartupPath;

                        this.backgroundWorker1.RunWorkerAsync(cls);
                    }
                    else
                    {
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("파일복사가 진행되고 있습니다. 잠시 후 다시 시도하세요.", "Rimage Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                this.checkCopying = false;
                this.ErrMsgShow("굽기 시작 중 에러가 발생했습니다.\r\n" + ex.Message, "Rimage Message : burnCDAfterCopyFiles", ex);
            }
        }

        /// <summary>
        /// 파일복사 비동기 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                Worker cls = e.Argument as Worker;
                e.Result = 0;
                cls.ExecJob(worker, e);
            }
            catch (Exception ex)
            {
                this.txtMessages.Text = ex.ToString();
                RimageKorea.ErrorLog.LogWrite(this, ex.ToString(), Application.StartupPath);
            }
        }

        /// <summary>
        /// 파일복사 진행상황 보여주기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentState state = e.UserState as CurrentState;
            this.txtStatusView.AppendText(state.retMessage);
        }

        /// <summary>
        /// 파일복사 완료 후 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.txtStatusView.AppendText("Error: " + e.Error.Message + "\r\n");
            }
            else if (e.Cancelled)
            {
                this.txtStatusView.AppendText("Copying files canceled.\r\n");
            }
            else
            {
                this.txtStatusView.AppendText("Finished copying files.\r\n");
            }

            CurrentState state = e.Result as CurrentState;
            this.copyFileLen = state.fileSize;

            this.backgroundWorker1.CancelAsync();

            //제대로 복사되었는지 다시 체크 후 굽기 명령 보냄.
            this.RecheckCDAfterCopyFiles();
        }

        /// <summary>
        /// 파일을 복사한 후 제대로 복사했는지 체크 후 굽기명령을 보낸다.
        /// </summary>
        public void RecheckCDAfterCopyFiles()
        {
            this.checkCopying = true;
            DirectoryInfo dirInfo = new DirectoryInfo(GlobalVar.configEntity.DicomDownloadFolder);
            bool dicomDirCheck = true;
            string errMsg = "";

            try
            {
                if (dirInfo.Exists)
                {
                    //// 여기서 이동한 폴더에 있는 DicomDir정보를 다시 조회한다.
                    //// 환자정보 가져오기
                    List<string> imgList = new List<string>();
                    Dictionary<string, string> patList = new Dictionary<string, string>();
                    PatientList patInfor = null;
                    Dictionary<string, string> dicomdirInfo = SearchPatient.GetPatient(Path.Combine(GlobalVar.configEntity.LocalShareFolder, this.ucPatients11.OrderInfo.patFolder),
                        GlobalVar.configEntity.DisableMultiPatient, out imgList, out patInfor, out patList);

                    if (dicomdirInfo == null)
                    {
                        dicomDirCheck = false;
                        errMsg = "버닝할 환자번호가 조회된 환자번호와 일치하지 않습니다. 데이터를 재전송하세요.\r\n";
                    }
                    else
                    {
                        //// 연구용인 경우는 그냥 패스..
                        //// this.patientName = "연구용자료";                        
                        if (this.ucPatients11.OrderInfo.patNo.Trim().StartsWith("NA") || dicomdirInfo["ID"].Trim().Equals("") || this.ucPatients11.OrderInfo.patName.Equals("연구용자료"))
                        {
                            dicomDirCheck = true;
                        }
                        else
                        {
                            //// 제출용(1명이상의환자일 때 패스..)
                            if (this.ucPatients11.OrderInfo.patList != null && this.ucPatients11.OrderInfo.patList.Count > 1)
                            {
                                dicomDirCheck = true;
                            }
                            else
                            {
                                if (this.ucPatients11.OrderInfo.patNo.Trim() != dicomdirInfo["ID"].Trim())
                                {
                                    dicomDirCheck = false;
                                    errMsg = "버닝할 환자번호가 조회된 환자번호와 일치하지 않습니다. 데이터를 재전송하세요.\r\n";
                                }
                                else
                                {
                                    dicomDirCheck = true;
                                }
                            }
                        }
                    }
                }

                if (dicomDirCheck == true)
                {
                    this.txtStatusView.Clear();

                    //// 굽기 명령을 보낸다.
                    this.checkCopying = false;
                    this.StartBurn(this.ucPatients11.OrderInfo);

                    //// Nexus인 경우 첫번째창도 닫아준다.
                    try
                    {
                        if (null != this.dicWhndNexus && true == this.dicWhndNexus.ContainsKey("Window1"))
                        {
                            if (this.dicWhndNexus["Window1"] != 0)
                            {
                                SendMessage((IntPtr)this.dicWhndNexus["Window1"], WM_CLOSE, 0, 0);
                                this.dicWhndNexus["Window1"] = 0;
                            }
                        }
                    }
                    catch { }
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    this.txtStatusView.AppendText(errMsg);

                    MessageBox.Show(errMsg, "Rimage Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                this.checkCopying = false;
            }
            catch (Exception ex)
            {
                this.ErrMsgShow("굽기 시작 중 에러가 발생했습니다.\r\n" + ex.Message, "Rimage Message : burnCDAfterCopyFiles", ex);
            }
            finally
            {
                this.checkCopying = false;
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 선택한 서버가 연결이 가능한지 먼저 체크
        /// </summary>
        /// <returns></returns>
        public bool EnableServerConnect()
        {
            try
            {
                //// 서버를 선택했는지 먼저 체크한다.
                if (this.NowSeletedServer == null)
                {
                    MessageBox.Show("먼저 굽기를 실행할 서버를 선택하세요.", "Rimage Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                //// 핑체크
                if (CheckPing.TestPing(this.NowSeletedServer.IP))
                {
                    return true;
                }
                else
                {
                    MessageBox.Show(string.Format("서버 ({0})에 연결되어 있지 않습니다.\r\n네트워크 상태 또는 서버 상태를 확인한 후 \"서버연결 버튼\"을 클릭하여 서버에 연결하세요.\r\n또는 다른 서버를 선택하세요.", this.NowSeletedServer.IP), "Rimage Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 서버 설정값 가져오기
        /// </summary>
        public void GetConfig()
        {
            try
            {
                if (GlobalVar.configEntity == null)
                {
                    GlobalVar.configEntity = new ConfigEntity();
                }

                AppDirectory = Application.StartupPath;
                Config cf = new Config(AppDirectory);

                this.LastHostIP = cf._HostIP;
                this.LastHostName = cf._HostName;
                this.LastHostPort = cf._HostPort;
                                
                GlobalVar.configEntity.programType = cf._programType;

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
                
                GlobalVar.configEntity.LocalShareFolder = cf._LocalShareFolder;
                GlobalVar.configEntity.CDLabelFile = cf._CDLabelFile;
                GlobalVar.configEntity.DVDLabelFile = cf._DVDLabelFile;
                GlobalVar.configEntity.MergeFileFolder = cf._MergeFileFolder;
                GlobalVar.configEntity.MergeFileServerFolder = cf._MergeFileServerFolder;
                GlobalVar.configEntity.ServerNetworkDrive = cf._ServerNetworkDrive;
                GlobalVar.configEntity.DicomDownloadFolder = cf._DicomDownloadFolder;
                GlobalVar.configEntity.HospitalName = cf._HospitalName;
                GlobalVar.configEntity.LogFilePath = cf._LogFilePath;
                GlobalVar.configEntity.LabelPrint = cf._LabelPrint;
                GlobalVar.configEntity.MergePrint = cf._MergePrint;
                GlobalVar.configEntity.CDPrintYN = cf._CDPrintYN;

                if (GlobalVar.configEntity.MergePrint.Length != 7)
                {
                    if (GlobalVar.configEntity.MergePrint.Length == 6)
                        GlobalVar.configEntity.MergePrint += "N";
                    else
                        GlobalVar.configEntity.MergePrint = "YYNYNNN";
                }

                GlobalVar.configEntity.CDMaxSize = cf._CDMaxSize;
                if (GlobalVar.configEntity.CDMaxSize.Length < 4 && GlobalVar.configEntity.CDMaxSize != "")
                {
                    GlobalVar.configEntity.CDMaxSize = (Convert.ToInt32(GlobalVar.configEntity.CDMaxSize) * 1024 * 1024).ToString();
                }

                GlobalVar.configEntity.CDRemainQty = Convert.ToInt32(cf._CDRemainQty);
                GlobalVar.configEntity.RibbonRemainQty = Convert.ToInt32(cf._RibbonRemainQty);
                GlobalVar.configEntity.BinCheckTime = Convert.ToInt32(cf._BinCheckTime);
                GlobalVar.configEntity.AutoExecute = cf._AutoExecute;
                GlobalVar.configEntity.AutoExecuteHookingType = cf._AutoExecuteHookingType;
                GlobalVar.configEntity.ReportView = cf._ReportView;
                GlobalVar.configEntity.DelType = cf._DelType;
                GlobalVar.configEntity.AutoCloseType = cf._AutoCloseType;
                GlobalVar.configEntity.DeleteAfterBurn = (String.IsNullOrWhiteSpace(cf._DeleteAfterBurn)) ? "0" : cf._DeleteAfterBurn;
                GlobalVar.configEntity.DownloadCheckbyFileName = (String.IsNullOrWhiteSpace(cf._DownloadCheckbyFileName)) ? "N" : cf._DownloadCheckbyFileName;
                GlobalVar.configEntity.DelFolderWhenClose = (String.IsNullOrWhiteSpace(cf._DelFolderWhenClose)) ? "Y" : cf._DelFolderWhenClose;
                GlobalVar.configEntity.RetentionPeriod = (String.IsNullOrWhiteSpace(cf._RetentionPeriod)) ? 1 : Convert.ToInt32(cf._RetentionPeriod);
                GlobalVar.configEntity.MsgBoxShow = cf._MsgBoxShow;
                GlobalVar.configEntity.ProgramCaption = cf._ProgramCaption;
                GlobalVar.configEntity.BurnPatientKind = cf._BurnPatientKind;
                GlobalVar.configEntity.FolderPattern = cf._FolderPattern;
                GlobalVar.configEntity.PopUpAlamYN = (String.IsNullOrWhiteSpace(cf._PopUpAlamYN)) ? "Y" : cf._PopUpAlamYN;
                GlobalVar.configEntity.PopUpKeepTime = (String.IsNullOrWhiteSpace(cf._PopUpKeepTime)) ? "20" : cf._PopUpKeepTime;
                GlobalVar.configEntity.PopUpSelPatInfoYN = (String.IsNullOrWhiteSpace(cf._PopUpSelPatInfoYN)) ? "Y" : cf._PopUpSelPatInfoYN;
                GlobalVar.configEntity.HookSleepTime1 = Convert.ToInt32((String.IsNullOrWhiteSpace(cf._HookSleepTime1) ? "3000" : cf._HookSleepTime1));
                GlobalVar.configEntity.HookSleepTime2 = Convert.ToInt32((String.IsNullOrWhiteSpace(cf._HookSleepTime2) ? "5000" : cf._HookSleepTime2));
                GlobalVar.configEntity.ManualExeBtnView = (String.IsNullOrWhiteSpace(cf._ManualExeBtnView)) ? "N" : cf._ManualExeBtnView;
                GlobalVar.configEntity.CloseConfirm = (String.IsNullOrWhiteSpace(cf._CloseConfirm)) ? "N" : cf._CloseConfirm;
                GlobalVar.configEntity.SoundAlam = (String.IsNullOrWhiteSpace(cf._SoundAlam)) ? "N" : cf._SoundAlam;
                GlobalVar.configEntity.FileCopyYN = (String.IsNullOrWhiteSpace(cf._FileCopyYN)) ? "N" : cf._FileCopyYN;
                GlobalVar.configEntity.ErrorPupupUse = (String.IsNullOrWhiteSpace(cf._ErrorPupupUse)) ? "Y" : cf._ErrorPupupUse;
                GlobalVar.configEntity.AutoPrintApp = (String.IsNullOrWhiteSpace(cf._AutoPrintApp)) ? "N" : cf._AutoPrintApp;
                GlobalVar.configEntity.DeleteInJobFolder = (String.IsNullOrWhiteSpace(cf._DeleteInJobFolder)) ? "N" : cf._DeleteInJobFolder;
                GlobalVar.configEntity.ExistDicomDir = (String.IsNullOrWhiteSpace(cf._ExistDicomDir)) ? "N" : cf._ExistDicomDir;
                GlobalVar.configEntity.FolderSizeCheckTime = (String.IsNullOrWhiteSpace(cf._FolderSizeCheckTime)) ? "3" : cf._FolderSizeCheckTime;

                //// 다운체크파일명을 전역변수에 저장
                GlobalVar.DOWN_CHK_FL_NM = (string.IsNullOrWhiteSpace(cf._DownCheckFileName)) ? "end.txt" : cf._DownCheckFileName;
                GlobalVar.configEntity.DownCheckFileName = (string.IsNullOrWhiteSpace(cf._DownCheckFileName)) ? "end.txt" : cf._DownCheckFileName;

                //// 다중환자
                GlobalVar.configEntity.DisableMultiPatient = (string.IsNullOrWhiteSpace(cf._DisableMultiPatient)) ? "N" : cf._DisableMultiPatient;
                GlobalVar.configEntity.DvdMaxSize = (string.IsNullOrWhiteSpace(cf._DvdMaxSize)) ? "4831838208" : cf._DvdMaxSize;
                GlobalVar.configEntity.UseUSBCopy = (string.IsNullOrWhiteSpace(cf._UseUSBCopy)) ? "N" : cf._UseUSBCopy;
                GlobalVar.configEntity.DisplayServeIP = (string.IsNullOrWhiteSpace(cf._DisplayServeIP)) ? "N" : cf._DisplayServeIP;

                if (cf._CDPrintYN == "N")
                    GlobalVar.configEntity.UseLabelPrint = false;
                else
                    GlobalVar.configEntity.UseLabelPrint = true;

                if (cf._MsgBoxShow == "Y" || string.IsNullOrWhiteSpace(GlobalVar.configEntity.MsgBoxShow))
                    GlobalVar.configEntity.MsgBoxShow = "3";

                GlobalVar.configEntity.ServerType = (String.IsNullOrWhiteSpace(cf._ServerType)) ? "R" : cf._ServerType;

                this.MyIP = GetMyIP.MyIP();
                this.ClientId = GlobalVar.configEntity.ServerType + this.MyIP.Replace(".", "_") + "_" + DateTime.Now.ToString("ddHHmmss") + RimageKorea.RandomOrderNumber.GetNewOrderNumber2();

                if (GlobalVar.configEntity.ServerType.Equals("L"))
                {
                    DirectoryInfo di = new DirectoryInfo(GlobalVar.configEntity.MergeFileFolder);
                    this.RimageSystemFolderPath = di.Parent.FullName;
                }

                //DB접속정보
                this.DBConnInfo = GetPatientNameFromDB.GetDBConn(Path.Combine(Application.StartupPath, GlobalVar.DBCONN_FILE_NAME));

                //환자정보가 위치한 폴더
                this.dicomCDFolder = GlobalVar.configEntity.DicomDownloadFolder;
                if (GlobalVar.configEntity.AutoExecuteHookingType == "8")
                {
                    //8.Deit pacs인 경우
                    this.dicomCDFolder = GlobalVar.configEntity.DicomDownloadFolder + "\\DICOMCD";
                }

                this.HookingTimestamp = DateTime.Now.AddSeconds(-10);

                //// 폴더사이즈체크시간
                if (!string.IsNullOrWhiteSpace(cf._FolderSizeCheckTime))
                {
                    if (!int.TryParse(cf._FolderSizeCheckTime, out GlobalVar.FolderSizeCheckTime))
                        GlobalVar.FolderSizeCheckTime = 5;
                }

                //// 서버목록 바인딩
                this.SetServerList();

                //// 마지막 선택했던 서버가 선택되도록 처리                
                //// 핑체크만 한다.                
                if (CheckPing.TestPing(this.LastHostIP))
                {
                    this.NowSeletedServer = this.GetServerInfo(this.LastHostIP);

                    if (this.NowSeletedServer == null)
                    {
                        this.NowSeletedServer = this.GetServerInfo(this.ServerList.First().IP);

                        if (this.NowSeletedServer == null)
                        {
                            this.NowSeletedServer = new ServerInfo();
                        }
                    }

                    this.SelectServerLabel(this.LastHostIP, true);
                }
                else
                {
                    this.SelectServerLabel(this.LastHostIP, false);
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
                    if (result[this.NowSeletedServer.IP].ContainsKey("CONFIG"))
                        this.SetServerConfig(result[this.NowSeletedServer.IP]["CONFIG"].ToString());

                    //// 상태값
                    if (result[this.NowSeletedServer.IP].ContainsKey("STATUS"))
                        this.SetServerStatus(result[this.NowSeletedServer.IP]["STATUS"].ToString());

                    ////this.txtMessages.Text = string.Format("{0} 연결 성공", this.NowSeletedServer.IP);
                    ////this.txtStatusView.AppendText(string.Format("{0} 연결되었습니다.\r\n", this.NowSeletedServer.IP));
                }
                ////else
                ////{
                ////    this.txtMessages.Text = string.Format("{0} 연결 실패.", this.NowSeletedServer.IP);
                ////    this.txtStatusView.AppendText(string.Format("{0} 연결되지 않습니다. 네트워크를 체크해주세요.\r\n", this.NowSeletedServer.IP));
                ////}
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
                    try
                    {
                        if (dt2.Columns.Contains("RibbonPanelSetsRemaining"))
                            this.txtCMYRibbons.Text = Utils.CheckNull(dt2.Rows[0]["RibbonPanelSetsRemaining"]);
                        else
                            this.txtCMYRibbons.Text = "0";

                        if (dt2.Columns.Contains("RetransferSheetPanelsRemaining"))
                            this.txtRetansferRibbons.Text = Utils.CheckNull(dt2.Rows[0]["RetransferSheetPanelsRemaining"]);
                        else
                            this.txtCMYRibbons.Text = "0";

                        if (dt2.Columns.Contains("RibbonPanelSetsRemaining"))
                            this.nowRemainCMYRibbon_Qty = Convert.ToInt32(Utils.CheckNull(dt2.Rows[0]["RibbonPanelSetsRemaining"], "0"));
                        else
                            this.nowRemainCMYRibbon_Qty = 0;

                        if (dt2.Columns.Contains("RetransferSheetPanelsRemaining"))
                            this.nowRemainTransferRibbon_Qty = Convert.ToInt32(Utils.CheckNull(dt2.Rows[0]["RetransferSheetPanelsRemaining"], "0"));
                        else
                            this.nowRemainTransferRibbon_Qty = 0;
                    }
                    catch { }
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

                if (this._BurningList == null)
                    return;

                if (this._BurningList.Count == 0)
                    return;

                //// 해당 OrderID Row를 찾아서 업데이트한다.
                this.UpdateBurningGrid(trace);

                //// 굽기완료시
                if (trace.Finish == "Y")
                {
                    var orderInfo = this._BurningList.Where(b => b.OrderId == trace.OrderID).FirstOrDefault();

                    if (string.IsNullOrEmpty(orderInfo.patNo))
                        return;

                    orderInfo.Finish = "Y";                    

                    ////결과저장
                    WebUtils.InsertResult(orderInfo.OrderId,
                              orderInfo.StartDateTime,
                              Utils.GetNowTime(),
                              orderInfo.patNo,
                              orderInfo.patName,
                              orderInfo.copies.ToString(),
                              orderInfo.mediType,
                              orderInfo.mediSize,
                              trace.ResultMessage,
                              ((orderInfo.BurnPatientKind.Equals("Y") || orderInfo.patList.Count > 1) ? orderInfo.DicomDescription : orderInfo.StudyModality),
                              Utils.CheckNull(orderInfo.BurnPatientKind, "N"),
                              orderInfo.TargetServer.IP,
                              this.MyIP);

                    //// 오더정보를 JSON 파일로 변경하여 저장한다.
                    FileControl.CreateOrderJsonFile(orderInfo.OrderId, JsonParser.ConvertToJsonString(orderInfo));

                    //// 오른쪽 하단에 메시지를 보여준다.
                    if (GlobalVar.configEntity.PopUpAlamYN == "Y")
                    {
                        if (trace.ResultCode == "2")
                            this.NotifyBurningResult(string.Format("{0}[{1}] - {2}\r\n굽기가 완료되었습니다.", orderInfo.patNo, orderInfo.patName, trace.ResultMessage));
                        else if (trace.ResultCode == "9")
                            this.NotifyBurningResult(string.Format("{0}[{1}] - 취소/실패\r\n굽기가 완료되었습니다.", orderInfo.patNo, orderInfo.patName));
                    }

                    //// 라벨프린트(스티커) : 성공시에만..
                    if (trace.ResultCode == "2" && GlobalVar.configEntity.LabelPrint == "Y")
                    {
                        string ret = MainFormBiz.LabelPrint(orderInfo.TargetServer.IP, orderInfo.patNo, orderInfo.patName, orderInfo.patSex, orderInfo.patAge, orderInfo.copies.ToString());
                        this.txtStatusView.AppendText(ret);
                    }

                    //// EditList Xml파일삭제하자.
                    FileControl.DeleteFile(orderInfo.EditListPath);

                    //// 환자 폴더 삭제
                    //// 보관기간이 1일 이상이면 삭제 안함.
                    if (GlobalVar.configEntity.RetentionPeriod < 1)
                    {
                        if (GlobalVar.configEntity.DeleteAfterBurn == "1" && trace.ResultCode == "2")
                        {
                            this.DeleteDestinationFolder(orderInfo.patFolderFullPath);
                        }
                        if (GlobalVar.configEntity.DeleteAfterBurn == "2")
                        {
                            this.DeleteDestinationFolder(orderInfo.patFolderFullPath);
                        }
                    }

                    this.txtStatusView.AppendText(orderInfo.patName + " : " + trace.StatusType + trace.State + " " + trace.DeviceCurrentState + " " + trace.PercentCompleted + "%" + "\r\n");

                    //// 완료된 오더폴더에 종료파일 생성한다.
                    FileControl.CreateTextFile(Path.Combine(GlobalVar.ProgramExecuteFolder, GlobalVar.ORDER_FOLDER, orderInfo.DiscOrder.OrderID, GlobalVar.BURN_CHK_FL_NM));
                    
                    //// RDMS가 정상종료되었는지 체크-> 아니라면 종료 처리
                    try
                    {
                        Process prc = Process.GetProcessById(Convert.ToInt32(trace.ProcessID));
                        if (prc != null)
                            prc.Kill();
                    }
                    catch { }

                    //// 오더 폴더 삭제
                    FileControl.DeleteBurnEndOrder(orderInfo.DiscOrder.OrderID);

                    this.txtStatusView.AppendText(string.Format("{0}[{1}] - {2}\r\n굽기가 완료되었습니다.", orderInfo.patNo, orderInfo.patName, trace.ResultMessage));
                }
            }
            catch (Exception ex)
            {
                this.txtStatusView.AppendText(ex.Message + "\r\n");
                ErrorLog.LogWrite(this, ex.ToString(), Application.StartupPath);
            }
        }

        /// <summary>
        /// 굽기완료시 오른쪽 하단에 메시지를 보여준다.
        /// </summary>
        /// <param name="msg"></param>
        private void NotifyBurningResult(string msg)
        {
            try
            {
                this.StartFlash();

                NotifyWindow nw;
                nw = new NotifyWindow("RimageBurningMessage", msg, Convert.ToInt32(GlobalVar.configEntity.PopUpKeepTime) * 1000);
                nw.TitleClicked += new System.EventHandler(NotifyTitleClick);
                nw.TextClicked += new System.EventHandler(NotifyTextClick);
                nw.SetDimensions(200, 120);
                nw.Notify();

                if (GlobalVar.configEntity.SoundAlam.Equals("Y"))
                {
                    string soundFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"Media\Afternoon\Windows Notify.wav");
                    PalySound.Run(soundFilePath);
                }
            }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartFlash()
        {
            if (this.WindowState == FormWindowState.Minimized || GetForegroundWindow() != this.Handle)
                FlashWindow(this.Handle, 1);
        }

        /// <summary>
        /// 굽기완료 메시지창 타이틀 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void NotifyTitleClick(object sender, System.EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }

            this.BringToFront();
        }
        /// <summary>
        /// 굽기완료 메시지창 메시지 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void NotifyTextClick(object sender, System.EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }

            this.BringToFront();
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

            if (GlobalVar.configEntity.ErrorPupupUse.Equals("Y"))
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
                        LabelControl lbl = this.grpServerList.Controls.Find(string.Format("Server{0}", i.ToString()), false)[0] as LabelControl;
                        lbl.Text = string.Format("[{0}] {1}", srv.No, GlobalVar.configEntity.DisplayServeIP.Equals("Y") ? srv.IP : srv.Name);
                        lbl.Appearance.Image = global::RimageMedicalSystemV2.Properties.Resources.add_16x16;
                        lbl.Tag = srv;

                        i++;
                    }
                }
                else
                {
                    MessageBox.Show("등록된 서버가 없습니다.\r\n환경설정에서 서버를 등록해 주세요!", "Rimage Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    this.hookChecker = new CheckDownImages(this, GlobalVar.configEntity.DicomDownloadFolder);
                    return;
                }

                try
                {
                    //잠시 쉬자
                    Thread.Sleep(GlobalVar.configEntity.HookSleepTime1);

                    // 프로그램 실행
                    if (GlobalVar.configEntity.AutoExecute == "1")
                    {
                        //조회 실행
                        this.isSearching = true;
                        this.btnSearch_Click(null, null);

                        this.hookChecker.StartTimer1();
                    }
                    else if (GlobalVar.configEntity.AutoExecute == "2")
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
                    this.hookChecker2 = new CheckDownComplete(this, GlobalVar.configEntity.DicomDownloadFolder);
                    return;
                }

                try
                {
                    //잠시 쉬자
                    Thread.Sleep(GlobalVar.configEntity.HookSleepTime1);

                    // 프로그램 실행
                    if (GlobalVar.configEntity.AutoExecute == "1")
                    {
                        //조회 실행
                        this.isSearching = true;
                        this.btnSearch_Click(null, null);

                        this.hookChecker2.StartTimer();
                    }
                    else if (GlobalVar.configEntity.AutoExecute == "2")
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
        /// --데이터 파일이 저장되는 Drive 용량 보여주기--
        /// </summary>
        private void SetDriveInfo()
        {
            try
            {
                //// 데이터 파일이 저장되는 Drive 용량 보여주기
                DriveInfo di = FileControl.GetDriveInfo(GlobalVar.configEntity.LocalShareFolder.Substring(0, 1) + ":\\");
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
                clsLD.retentionPeriod = GlobalVar.configEntity.RetentionPeriod;
                clsLD.localSharedFolder = GlobalVar.configEntity.LocalShareFolder;
                clsLD.downloadFolder = GlobalVar.configEntity.DicomDownloadFolder;
                clsLD.deleteInJobFolder = GlobalVar.configEntity.DeleteInJobFolder;

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
            this.txtStatusView.AppendText(GlobalVar.configEntity.RetentionPeriod.ToString() + "일 전의 로그와 데이터 삭제가 완료되었습니다." + Environment.NewLine);
            this.CheckDeletePatientFolder();
        }

        /// <summary>
        /// 보관기간 이전의 환자폴더들이 남아 있는지 체크
        /// </summary>
        private void CheckDeletePatientFolder()
        {
            if (GlobalVar.configEntity.programType == "1")
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
                DirectoryInfo sharedFolder = new DirectoryInfo(GlobalVar.configEntity.LocalShareFolder);
                DirectoryInfo downFolder = new DirectoryInfo(GlobalVar.configEntity.DicomDownloadFolder);
                int k = 0;
                int p = GlobalVar.configEntity.RetentionPeriod;
                if (1 > GlobalVar.configEntity.RetentionPeriod)
                    p = 1;

                if (sharedFolder.Exists)
                {
                    this.Cursor = Cursors.WaitCursor;

                    string pattern = "*";

                    //Deit_Pacs인 경우..
                    if (GlobalVar.configEntity.AutoExecuteHookingType == "8")
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
                    MessageBox.Show(GlobalVar.configEntity.RetentionPeriod.ToString() + "일 전에 다운받은 환자데이터 폴더가 존재합니다.\r\n자동으로 삭제가 안되었을 수 있습니다.\r\n수동으로 삭제하여 주세요.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                DirectoryInfo sharedFolder = new DirectoryInfo(GlobalVar.configEntity.LocalShareFolder);
                int k = 0;
                int p = GlobalVar.configEntity.RetentionPeriod;
                if (1 > GlobalVar.configEntity.RetentionPeriod)
                    p = 1;

                if (sharedFolder.Exists)
                {
                    this.Cursor = Cursors.WaitCursor;
                    foreach (DirectoryInfo di in sharedFolder.GetDirectories("*.*"))
                    {
                        if (di.CreationTime.Date <= DateTime.Now.AddDays(GlobalVar.configEntity.RetentionPeriod * -1).Date && di.FullName != sharedFolder.FullName)
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
                    MessageBox.Show(GlobalVar.configEntity.RetentionPeriod.ToString() + "일 전에 다운받은 환자데이터 폴더가 존재합니다.\r\n자동으로 삭제가 안되었을 수 있습니다.\r\n수동으로 삭제하여 주세요.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    if (lbl.Tag == null)
                    {
                        lbl.Appearance.Image = global::RimageMedicalSystemV2.Properties.Resources.add_16x16;
                        lbl.Appearance.BorderColor = Color.FromArgb(105, 105, 105);
                        continue;
                    }

                    ServerInfo s = ctrl.Tag as ServerInfo;

                    if (s.IP == hostIP)
                    {
                        lbl.Appearance.BorderColor = Color.FromArgb(192, 0, 0);
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
                        lbl.Appearance.Image = global::RimageMedicalSystemV2.Properties.Resources.add_16x16;
                        lbl.Appearance.BorderColor = Color.FromArgb(105, 105, 105);
                    }
                }

                //// 서버 설정 가져오기
                if (connected)
                {
                    this.txtMessages.Text = string.Format("{0} 연결 성공", this.NowSeletedServer.IP);
                    this.txtStatusView.AppendText(string.Format("{0} 네트워크 연결되었습니다.\r\n", this.NowSeletedServer.IP));
                    this.GetConnectedServerInfo();
                }
                else
                {
                    this.txtMessages.Text = string.Format("{0} 연결 실패.", this.NowSeletedServer.IP);
                    this.txtStatusView.AppendText(string.Format("{0} 연결되지 않습니다. 네트워크를 체크해주세요.\r\n", this.NowSeletedServer.IP));
                }
            }
            catch { }
        }

        /// <summary>
        /// 서버의 설정, 상태값 읽어오도록 프로그램 실행
        /// </summary>
        private void GetConnectedServerInfo()
        {
            try
            {
                bool exeOK = false;

                //// IP 폴더가 없을 경우
                //// 파일이 없을 경우
                //// 생성일자가 1시간이 지났을 경우
                string logPath = Path.Combine(GlobalVar.ProgramExecuteFolder, GlobalVar.ORDER_FOLDER, string.Format("SVR_{0}", this.NowSeletedServer.IP));

                if (!Directory.Exists(logPath))
                {
                    exeOK = true;
                }
                else
                {
                    bool configExist = false;
                    bool statusExist = false;
                    string fConfig = string.Empty;
                    string fStatus = string.Empty;

                    foreach (string file in Directory.GetFiles(logPath))
                    {
                        string xml = File.ReadAllText(file);

                        if (file.StartsWith(GlobalVar.SERVER_CONFIG_FL))
                        {
                            configExist = true;
                            fConfig = file;
                        }
                        else if (file.StartsWith(GlobalVar.SERVER_STATUS_FL))
                        {
                            statusExist = true;
                            fStatus = file;
                        }
                    }

                    //// 파일이 두개 다 존재할 경우
                    if (configExist && statusExist)
                    {
                        FileInfo f1 = new FileInfo(fConfig);
                        int t1 = DateTime.Compare(f1.CreationTime, DateTime.Now.AddHours(-1));

                        FileInfo f2 = new FileInfo(fStatus);
                        int t2 = DateTime.Compare(f2.CreationTime, DateTime.Now.AddHours(-1));

                        //// 둘 중 하나라도 1시간보다 이전에 생성되었다면 
                        if (t1 < 0 || t2 < 0)
                        {
                            exeOK = true;
                        }
                    }
                    else
                    {
                        exeOK = true;
                    }
                }

                if (exeOK)
                    Process.Start(GlobalVar.BURM_PROGRAM, string.Format("S|{0}|{1}|{2}|{3}", this.NowSeletedServer.IP, this.NowSeletedServer.Name, this.NowSeletedServer.Port, this.Handle.ToInt32().ToString()));
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
            this.AutoExec();
        }

        /// <summary>
        /// 후킹 실행
        /// </summary>
        private void AutoExec()
        {
            bool retVal1 = false;
            bool retVal2 = false;
            bool retVal3 = false;
            bool retVal4 = false;
            bool retVal5 = false;
            bool retVal6 = false;
            bool retVal7 = false;
            bool retVal8 = false;
            bool retVal10 = false;

            try
            {
                switch (GlobalVar.configEntity.AutoExecuteHookingType)
                {
                    case "1":
                        retVal1 = AutoExec1();      //1.GE-OLD-SMC
                        break;
                    case "2":
                        retVal5 = AutoExec2();      //2.GE-NEW
                        break;
                    case "3":
                        retVal2 = AutoExec3();      //3.Maro-SNU
                        break;
                    case "4":
                        retVal4 = AutoExec4();      //4.InfinityPACS-NEW
                        break;
                    case "5":
                        retVal3 = AutoExec5();      //5.StarPACS
                        break;
                    case "6":
                        retVal6 = AutoExec6();      //6.NEXUS
                        break;
                    case "7":
                        retVal7 = AutoExec7();      //7. MEDIOS
                        break;
                    case "8":
                        retVal8 = AutoExec8();      //8. DEITPACS
                        break;
                    case "10":
                        retVal10 = AutoExec10();      //10.DCAS
                        break;
                    default:
                        break;
                }

                if (retVal1 || retVal2 || retVal3 || retVal4 || retVal5 || retVal6 || retVal7 || retVal8 || retVal10)
                {
                    Thread.Sleep(GlobalVar.configEntity.HookSleepTime2);

                    // 실행
                    if (GlobalVar.configEntity.AutoExecute == "1")
                    {
                        //조회 실행
                        this.btnSearch_Click(null, null);
                    }
                    else if (GlobalVar.configEntity.AutoExecute == "2")
                    {
                        //조회 및 굽기까지 실행
                        this.btnSearch_Click(null, null);
                        Thread.Sleep(200);

                        this.mediaType = MediaType.CD_DVD;
                        if (GlobalVar.configEntity.programType == "1")
                        {
                            this.ReadyBurn1();
                        }
                        else
                        {
                            this.ReadyBurn2(true);
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// SMC
        /// </summary>
        /// <returns></returns>
        private bool AutoExec1()
        {
            // window class, caption
            bool retVal = false;
            int phw;
            int hw = FindWindow("#32770", "Create DICOM Volume");

            if (hw != 0) // 프로그램이 실행한 경우 
            {
                phw = FindWindowEx(hw, 0, "Static", "Finished writing volume");

                if (phw != 0)
                {
                    retVal = true;
                    // kill parent dialog.
                    SendMessage((IntPtr)hw, WM_CLOSE, 0, 0);
                }
            }

            return retVal;
        }

        /// <summary>
        /// GE-한글
        /// </summary>
        /// <returns></returns>
        private bool AutoExec2()
        {
            // window class, caption
            bool retVal = false;
            int phw;
            int hw = FindWindow("#32770", "Create DICOM Volume");

            if (hw != 0) // 프로그램이 실행한 경우 
            {
                phw = FindWindowEx(hw, 0, "Static", "Volume written and verified");

                if (phw != 0)
                {
                    retVal = true;
                    // kill parent dialog.
                    SendMessage((IntPtr)hw, WM_CLOSE, 0, 0);
                }
            }

            return retVal;
        }

        /// <summary>
        /// SNU-Maro
        /// </summary>
        /// <returns></returns>
        private bool AutoExec3()
        {
            // window class, caption
            bool retVal = false;
            int phw;
            int hw = FindWindow("#32770", "Success");

            if (hw != 0) // 프로그램이 실행한 경우 
            {
                phw = GetParent(hw);

                SendMessage((IntPtr)hw, WM_CLOSE, 0, 0);

                //Success창까지만 닫기로 설정되어 있지 않은 경우 부모창도 닫는다.
                if (GlobalVar.configEntity.AutoCloseType == "Y")
                {
                    retVal = true;
                }
                else
                {
                    Thread.Sleep(200);

                    if (phw != 0)
                    {
                        retVal = true;
                        // kill parent dialog.
                        SendMessage((IntPtr)phw, WM_CLOSE, 0, 0);
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// InfinityPACS-NEW
        /// </summary>
        /// <returns></returns>
        private bool AutoExec4()
        {
            // window class, caption
            bool retVal = false;
            int phw;
            int hw = FindWindow(null, "DVD / CD Burner");

            if (hw != 0) // 프로그램이 실행한 경우 
            {
                phw = FindWindow(null, "Information");

                if (phw != 0)
                {
                    retVal = true;
                    // kill parent dialog.
                    SendMessage((IntPtr)phw, WM_CLOSE, 0, 0);

                    // Process를 죽인다.(SCDBurn.exe, CDBurner.exe)
                    KillProcess.DelProcess("SCDBurn");
                    KillProcess.DelProcess("CDBurner");
                }
            }

            return retVal;
        }

        /// <summary>
        /// StarPACS
        /// </summary>
        /// <returns></returns>
        private bool AutoExec5()
        {
            // window class, caption
            bool retVal = false;
            int hw = 0;

            hw = FindWindow(null, "Send");
            if (hw != 0) // 프로그램이 실행한 경우 
            {
                if (!dicWhnd.ContainsKey(hw.ToString()))
                {
                    dicWhnd.Add(hw.ToString(), hw);
                }
            }

            if (dicWhnd.Count == 2)
            {
                int clwnd = 0;
                foreach (KeyValuePair<string, int> dicvalue in this.dicWhnd)
                {
                    clwnd = dicvalue.Value;
                }
                retVal = true;
                SendMessage((IntPtr)clwnd, WM_CLOSE, 0, 0);

                this.dicWhnd.Clear();
            }

            //창이 없다면 
            if (hw == 0)
            {
                this.dicWhnd.Clear();
            }

            return retVal;
        }

        /// <summary>
        /// 성모병원 - Nexus
        /// </summary>
        /// <returns></returns>
        private bool AutoExec6()
        {
            bool retVal = false;
            int hw = FindWindow(null, "Burning CD-Rom");
            int phw = FindWindow(null, "NexusEV BurnCD");

            //굽기까지 띄운 상태
            if (phw != 0)
            {
                retVal = true;

                if (this.dicWhndNexus.ContainsKey("Window1"))
                    this.dicWhndNexus["Window1"] = hw;
                else
                    this.dicWhndNexus.Add("Window1", hw);

                if (this.dicWhndNexus.ContainsKey("Window2"))
                    this.dicWhndNexus["Window2"] = phw;
                else
                    this.dicWhndNexus.Add("Window2", phw);

                //if (this.dicWhndNexus.ContainsKey("NexusCapture"))
                //    this.dicWhndNexus["NexusCapture"] = 1;
                //else
                //    this.dicWhndNexus.Add("NexusCapture", 1);

                // kill parent dialog.
                Thread.Sleep(1000);
                SendMessage((IntPtr)phw, WM_CLOSE, 0, 0);
            }

            return retVal;
        }

        /// <summary>
        /// DEDIOS
        /// </summary>
        /// <returns></returns>
        private bool AutoExec7()
        {
            // window class, caption
            bool retVal = false;

            int hw = FindWindow(null, "DicomDirModule");

            if (hw != 0) // 프로그램이 실행한 경우 
            {
                Thread.Sleep(200);
                retVal = true;
                // kill parent dialog.
                SendMessage((IntPtr)hw, WM_CLOSE, 0, 0);
            }

            return retVal;
        }

        /// <summary>
        /// 순천향 병원 - DEIT PACS
        /// </summary>
        /// <returns></returns>
        private bool AutoExec8()
        {
            bool retVal = false;

            int hw = FindWindow(null, "Deit_Burn");

            if (hw != 0) // 프로그램이 실행한 경우 
            {
                Thread.Sleep(200);
                retVal = true;
                // kill parent dialog.
                SendMessage((IntPtr)hw, WM_CLOSE, 0, 0);

                //Process Kill - Deit_Burn.exe
                //RimageKorea.KillProcess.DelProcess("Deit_Burn");
            }

            return retVal;
        }

        /// <summary>
        /// DCAS
        /// </summary>
        /// <returns></returns>
        private bool AutoExec10()
        {
            bool retVal = false;

            int hw = FindWindow("#32770", "Renaming and Writing Done !!!!");

            if (hw != 0) // 프로그램이 실행한 경우 
            {
                Thread.Sleep(200);
                retVal = true;
                // kill parent dialog.
                SendMessage((IntPtr)hw, WM_CLOSE, 0, 0);
            }

            return retVal;
        }

        /// <summary>
        /// 주기적으로 다운로드 완료된 폴더가 존재하는지 체크하자. - Type2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrDownloadChecker_Tick(object sender, EventArgs e)
        {
            ////현재 환자조회중인지, 굽기를 위해 파일복사중인지 체크
            if (this.isSearching || this.checkCopying)
            {
                return;
            }

            this.tmrDownloadChecker.Stop();
            bool check = false;

            try
            {
                string checkFolder = string.Empty;
                if (GlobalVar.configEntity.programType == "1")
                {
                    checkFolder = GlobalVar.configEntity.DicomDownloadFolder;
                }
                else
                {
                    checkFolder = GlobalVar.configEntity.LocalShareFolder;
                }

                if (Directory.Exists(checkFolder))
                {
                    if (GlobalVar.configEntity.programType == "1")
                    {
                        if (CheckFiles.CheckFileExists(new DirectoryInfo(checkFolder), GlobalVar.DOWN_CHK_FL_NM) &&
                           !CheckFiles.CheckFileExists(new DirectoryInfo(checkFolder), GlobalVar.SEEK_CHK_FL_NM))
                        {
                            check = true;
                        }
                    }
                    else
                    {
                        foreach (string sdir in Directory.GetDirectories(GlobalVar.configEntity.LocalShareFolder))
                        {
                            //// 다운체크파일이 존재하고 burn.end 파일이 없을 경우에만 다음단계 실행
                            //// 환자목록에 없을 경우에만..
                            if (CheckFiles.CheckFileExists(new DirectoryInfo(sdir), GlobalVar.DOWN_CHK_FL_NM) &&
                               !CheckFiles.CheckFileExists(new DirectoryInfo(sdir), GlobalVar.BURN_CHK_FL_NM))
                            {
                                check = true;
                                break;
                            }
                        }
                    }

                    if (check)
                    {
                        check = false;

                        //// PACS 다운로드 창 닫아준다.
                        int hw = FindWindow(null, "PACSPLUS Publisher Process");

                        if (hw != 0) // 프로그램이 실행한 경우 
                        {
                            Thread.Sleep(500);
                            // kill parent dialog.
                            SendMessage((IntPtr)hw, WM_CLOSE, 0, 0);
                        }

                        //// 실행
                        if (GlobalVar.configEntity.AutoExecute == "1")
                        {
                            //// 조회 실행
                            this.isSearching = true;
                            this.btnSearch_Click(null, null);
                        }
                        else if (GlobalVar.configEntity.AutoExecute == "2")
                        {
                            Thread.Sleep(1000);

                            ////조회 및 굽기까지 실행
                            this.isSearching = true;
                            this.btnSearch_Click(null, null);
                            Thread.Sleep(3000);

                            this.mediaType = MediaType.CD_DVD;
                            if (GlobalVar.configEntity.programType == "1")
                            {
                                this.ReadyBurn1();
                            }
                            else
                            {
                                this.ReadyBurn2(true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite(this, ex.ToString(), Application.StartupPath);
            }
            finally
            {
                this.tmrDownloadChecker.Start();
            }
        }

        /// <summary>
        /// 빈체크 (CD 수량)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrBinCheker_Tick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 윈도우 메시지 받기
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            try
            {
                switch (m.Msg)
                {
                    case WM_COPYDATA:
                        COPYDATASTRUCT cds = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));

                        if (cds.lpData.StartsWith("BURN_SRT"))
                        {
                            //// 굽기 시작
                            string orderID = cds.lpData.Substring(cds.lpData.IndexOf(":") + 1);
                            this.AddBurningList(orderID);
                        }
                        else if (cds.lpData.StartsWith("BURN_ERROR"))
                        {
                            //// 굽기 시작이 안됨
                            string orderID = cds.lpData.Substring(cds.lpData.IndexOf(":") + 1);
                            this.RemoveBurningList(orderID);
                        }
                        else if (cds.lpData == "SRV_END")
                        {
                            //// 서버 설정 가져오기 완료 => 파일읽어와 화면에 보여준다.
                            this.burnProcessChecker.ReadServerConfig();
                        }
                        else if (cds.lpData.StartsWith("BURN_END"))
                        {
                            //// 굽기 완료
                        }
                        else if (cds.lpData.StartsWith("ERROR"))
                        {
                            //// 오류 발생 처리
                            this.HandleError(cds.lpData);
                        }
                        break;
                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
            catch { }
        }

        /// <summary>
        /// 백그라운드에서 지정된 Process 죽이기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker4.CancellationPending)
            {
                e.Cancel = true;
                backgroundWorker4.ReportProgress(0);
                return;
            }

            string pName = e.Argument as string;

            bool ret = KillProcess.IsProcess(pName, 0);

            e.Result = ret;
        }
        /// <summary>
        /// 백그라운드에서 지정된 Process 죽이기 완료 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (backgroundWorker4.IsBusy)
                backgroundWorker4.CancelAsync();

            bool? result = e.Result as Nullable<Boolean>;

            if (result != null)
            {
                this.ChageNameAndOrderBurning(!result.Value);
            }
            else
            {
                ErrMsgShow("다른 프로세스에 의해 사용중이므로 폴더명을 바꿀 수 없습니다.", "Rimage Message : ChangeDownloadFolderToPatientFolder");
            }
        }

        /// <summary>
        /// 완료된 후 폴더 삭제 - backgroundWorker3
        /// </summary>
        /// <param name="folderPath"></param>
        private void DeleteDestinationFolder(string folderPath)
        {
            try
            {
                if (this.backgroundWorker3.IsBusy == false)
                {
                    this.backgroundWorker3.RunWorkerAsync(folderPath);
                }
                else
                {
                    if (!lstFolderToDelete.Contains(folderPath))
                        lstFolderToDelete.Add(folderPath);
                }
            }
            catch
            {
                //skip
            }
        }

        /// <summary>
        /// 완료된 폴더 삭제 비동기 처리 시작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string str = e.Argument as string;
                e.Result = str;

                FileControl.DeleteFolder(str, false);
            }
            catch
            {
                //skip
            }
        }
        /// <summary>
        /// 완료된 폴더 삭제 비동기 처리 진행 상태
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        /// <summary>
        /// 완료된 폴더 삭제 비동기 처리 완료시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            this.backgroundWorker3.CancelAsync();

            if (this.lstFolderToDelete.Contains(e.Result.ToString()))
                this.lstFolderToDelete.Remove(e.Result.ToString());

            if (this.lstFolderToDelete.Count > 0)
            {
                this.DeleteDestinationFolder(this.lstFolderToDelete[0]);
            }
        }

        /// <summary>
        /// 자동실행 => 수동으로 재 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRetry_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("버닝을 수동으로 실행합니다.\r\n이미 자동실행되었다면 취소하십시오.\r\n계속 진행할까요?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // 프로그램 실행
                if (GlobalVar.configEntity.AutoExecute == "1")
                {
                    //조회 실행
                    this.btnSearch_Click(null, null);
                }
                else if (GlobalVar.configEntity.AutoExecute == "2")
                {
                    //조회 및 굽기까지 실행
                    this.btnSearch_Click(null, null);
                    Thread.Sleep(300);
                    this.btnBurn_Click(null, null);
                }
            }
        }

        /// <summary>
        /// 환자 정보 삭제 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPatientDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("조회된 환자정보를 삭제합니다.\r\n잠시 기다려 주세요.", "Rimage Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;

                try
                {
                    if (GlobalVar.configEntity.programType == "1")
                    {
                        this.ucPatients11.DeletePatientFiles(this);
                        this.ucPatients11.Clear();
                        this.ucPatients11.OrderInfo = null;
                    }

                    this.txtMessages.Text = "조회된 환자정보를 삭제하였습니다.";
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// 굽기 취소 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelBurning_Click(object sender, EventArgs e)
        {
            this.CancelOrder();
        }

        /// <summary>
        /// 굽기 취소 실행
        /// </summary>
        private void CancelOrder()
        {
            try
            {
                if (this._BurningList == null)
                    return;
                if (this._BurningList.Count == 0)
                    return;

                //// 환자 선택
                if (this.gvBurninglist.SelectedRowsCount == 0)
                {
                    MessageBox.Show("먼저 굽기 취소할 대상을 선택하세요.", "Rimage Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var orderInfo = this._BurningList[this.gvBurninglist.FocusedRowHandle];

                //// 굽기완료된 폴더인지 체크(burn.end 파일체크)
                if (!CheckFiles.CheckFileExists(new DirectoryInfo(orderInfo.patFolderFullPath), GlobalVar.BURN_CHK_FL_NM))
                {
                    string folder = Path.Combine(GlobalVar.ProgramExecuteFolder, GlobalVar.ORDER_FOLDER, "CANCEL");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    string file = Path.Combine(folder, orderInfo.OrderId);
                    if (!File.Exists(file))
                    {
                        File.Create(file);
                    }

                    //// item.SubItems[4].Text = "Submitted for cancel job";
                    //// item.SubItems[6].Text = "취소";
                    orderInfo.Progress = "Submitted for cancel job";
                    orderInfo.BurnState = "취소";

                    this.gvBurninglist.RefreshData();
                }
            }
            catch { }
        }

        /// <summary>
        /// 화면정리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("완료된 작업 내역을 삭제합니다.\r\n잠시 기다려 주세요.", "화면 정리", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (dr == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;

                if (GlobalVar.configEntity.programType == "1")
                {
                    this.ucPatients11.Clear();
                    this.ucPatients11.OrderInfo = null;
                }

                //// 관련 폴더 및 파일 삭제
                this.ClearCompleteBurnList();

                this.gvBurninglist.RefreshData();

                this.txtMessages.Text = "화면정리 완료.";
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 굽기 완료된 데이터를 삭제한다.
        /// </summary>
        /// <returns></returns>
        private bool ClearCompleteBurnList()
        {
            this.txtStatusView.Clear();

            try
            {
                int idx = 0;
                List<int> idxList = new List<int>();

                foreach (var orderInfo in this._BurningList)
                {
                    if (orderInfo.Finish == "Y")
                    {
                        //// 환자정보 삭제안함이 아니고, 보관기간이 0일 경우에만 환자폴더 삭제
                        if (GlobalVar.configEntity.DeleteAfterBurn != "0" && GlobalVar.configEntity.RetentionPeriod == 0)
                        {
                            DirectoryInfo sourceDir = null;
                            sourceDir = new DirectoryInfo(orderInfo.patFolderFullPath);

                            //// 혹시 환자폴더가 환자 상위폴더와 같을 경우를 대비해 체크 한번 하자.
                            if (sourceDir.Exists && sourceDir.FullName != GlobalVar.configEntity.LocalShareFolder)
                            {
                                if (GlobalVar.configEntity.DelType.Equals("0"))
                                    FileControl.DeleteFolder(sourceDir.FullName, false);
                                else
                                    FileControl.DeleteFolder(sourceDir.FullName, true);
                            }
                        }

                        //// 머지파일, EditFile 삭제
                        try
                        {
                            FileControl.DeleteFolder(orderInfo.MegPath, false);
                            FileControl.DeleteFolder(orderInfo.EditListPath, false);
                        }
                        catch { }

                        //// 오더 정보 삭제
                        try
                        {
                            DirectoryInfo dirOrder = new DirectoryInfo(Path.Combine(Application.StartupPath, GlobalVar.ORDER_FOLDER));

                            foreach (DirectoryInfo dri in dirOrder.GetDirectories(orderInfo.OrderId))
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

                        idxList.Add(idx);
                    }

                    idx++;
                }

                if (idxList.Count > 0)
                {
                    int cnt = this._BurningList.Count - 1;

                    for (int j = cnt; j >= 0; j--)
                    {
                        if (idxList.Contains(j))
                            this._BurningList.RemoveAt(j);
                    }
                }
            }
            catch { }

            return true;
        }

        /// <summary>
        /// 재굽기 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOrderedList_Click(object sender, EventArgs e)
        {
            OrderHistoryForm frm = new OrderHistoryForm();
            frm.ShowDialog(this);
            frm.Dispose();
        }

        /// <summary>
        /// 보고서 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReport_Click(object sender, EventArgs e)
        {
            xFormReport frm = new xFormReport();
            frm.ServerList = this.ServerList;
            frm.NowServerInfo = this.NowSeletedServer;
            frm.ShowDialog();
            frm.Dispose();
        }

        /// <summary>
        /// 프로그램 종료시 처리..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = System.Windows.Forms.DialogResult.OK;

            //// 굽기진행중인지 체크

            if (GlobalVar.configEntity.CloseConfirm.Equals("Y"))
            {
                result = MessageBox.Show("프로그램을 종료하겠습니까?", "Rimage Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            }

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                if (GlobalVar.configEntity.DelFolderWhenClose.Equals("Y"))
                {
                    MessageBox.Show("다운받은 환자데이터를 삭제합니다.\r\n이 작업은 몇 분간의 시간이 걸릴 수 있습니다.\r\n작업이 완료된 후 프로그램이 종료될 때까지 기다려주세요.", "Rimage Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.ClearDicomFolder();
                }

                this.ClearImgFiles();
                this.KillBurnPrograms();

                ErrorLog.TraceWrite(this, "-- Program Exit --", Application.StartupPath);
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 환자 폴더 정리
        /// </summary>
        private void ClearDicomFolder()
        {
            try
            {
                DirectoryInfo dir1 = new DirectoryInfo(GlobalVar.configEntity.LocalShareFolder);
                DirectoryInfo dir2 = new DirectoryInfo(GlobalVar.configEntity.DicomDownloadFolder);

                //// 다운로드 안..
                if (dir2.Exists)
                {
                    foreach (DirectoryInfo subDir2 in dir2.GetDirectories())
                    {
                        if (subDir2.Exists)
                        {
                            FileControl.DeleteFolder(subDir2.FullName, false);
                        }
                    }

                    foreach (FileInfo fl in dir2.GetFiles())
                    {
                        if (fl.Exists)
                        {
                            fl.Delete();
                        }
                    }
                }

                //// 공유폴더 안..
                if (dir1.Exists)
                {
                    string pattern = "*";

                    //Deit_Pacs인 경우..
                    if (GlobalVar.configEntity.AutoExecuteHookingType == "8")
                        pattern = "MC_*";

                    foreach (DirectoryInfo subDir1 in dir1.GetDirectories(pattern))
                    {
                        if (subDir1.Exists)
                        {
                            if (subDir1.FullName != dir2.FullName)
                            {
                                if (GlobalVar.configEntity.DelType.Equals("0"))
                                    FileControl.DeleteFolder(subDir1.FullName, false);
                                else
                                    FileControl.DeleteFolder(subDir1.FullName, true);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 서버에 있는 img 파일 삭제
        /// </summary>
        private void ClearImgFiles()
        {
            string response = "";
            string webaddress = "";
            webaddress = "http://" + this.NowSeletedServer.IP + "/RimageWeb/imgFileDelete.aspx";
            //webaddress = "http://localhost:4557/RimageWeb/imgFileDelete.aspx";

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
        /// 실행중인 굽기 프로그램이 존재하는지 체크 후 모두 종료
        /// </summary>
        private void KillBurnPrograms()
        {
            KillProcess.DelProcess("RMDS");
        }

        /// <summary>
        /// 서버 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerButton_Click(object sender, EventArgs e)
        {
            try
            {
                LabelControl lable = sender as LabelControl;
                if (lable.Tag != null)
                {
                    ServerInfo srv = lable.Tag as ServerInfo;

                    //// 현재 선택된 서버로 저장
                    this.NowSeletedServer = srv;

                    this.txtStatusView.AppendText(string.Format("{0} 연결중입니다. 잠시 기다려주세요.\r\n", srv.IP));
                    this.tmrConnectServer.Enabled = true;
                }
            }
            catch { }
        }

        /// <summary>
        /// 서버에 연결하고 Config, Status 값을 가져온다.
        /// </summary>
        private void ConnectServer()
        {
            try
            {
                if (CheckPing.TestPing(this.NowSeletedServer.IP))
                {
                    this.SelectServerLabel(this.NowSeletedServer.IP, true);
                }
                else
                {
                    this.SelectServerLabel(this.NowSeletedServer.IP, false);
                }
            }
            catch { }
        }

        /// <summary>
        /// 굽기 프로그램에서 받은 오류파일을 읽어서 처리한다.
        /// </summary>
        /// <param name="fileName"></param>
        private void HandleError(string fileName)
        {
            try
            {
                ////파일을 읽어온다.
                string json = File.ReadAllText(fileName);
                ErrorInfo err = JsonParser.ConvertToErrorInfo(json);

                if (err.OrderType == "O")
                {
                    this.txtMessages.Text = string.Format("굽기 오류 발생\r\n{0}", err.Message);
                    this.txtStatusView.AppendText(string.Format("오류발생 출력번호 : {0}\r\n", err.OrderID));
                    this.txtStatusView.AppendText(err.Description);
                }
            }
            catch { }
        }

        /// <summary>
        /// 홈페이지 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblHomepage_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// USB 복사
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUSBCopy_Click(object sender, EventArgs e)
        {
            //// 복사 진행중인지 체크한다.
            if (GlobalVar.isCopyingToUSB)
            {
                MessageBox.Show("USB 복사가 진행중입니다.\r\n종료 후 시도하세요.", "Rimage Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.mediaType = MediaType.USB;

            try
            {
                if (GlobalVar.configEntity.programType == "1")
                {
                    this.ReadyBurn1();
                }
                else
                {
                    this.ReadyBurn2();
                }
            }
            catch { }
        }

        /// <summary>
        /// USB 굽기 실행
        /// </summary>
        private void ExeCopyToUSB(BurnOrderedInfoEntity orderInfo)
        {
            frmCopyToUSB frm = new frmCopyToUSB();

            try
            {
                int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
                int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;

                Point parentPoint = this.Location;

                int parentHeight = this.Height;
                int parentWidth = this.Width;

                int childHeight = frm.Height;
                int childWidth = frm.Width;

                int resultX;
                int resultY;

                if ((parentPoint.Y + parentHeight + childHeight) > screenHeight)
                {
                    resultY = parentPoint.Y;
                    resultX = parentPoint.X + parentWidth;
                }
                else
                {
                    // Position on the edge.
                    resultY = parentPoint.Y;
                    resultX = parentPoint.X + parentWidth;
                }

                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = new Point(resultX, resultY);

                frm.OrderInfo = orderInfo;
                frm.MyOwnerForm = this;
            }
            catch { }

            frm.Show();
        }

        /// <summary>
        /// USB 굽기 종료
        /// </summary>
        /// <param name="message"></param>
        private void CompleteCopyToUSB(string message, DiscStatusForDisplay trace)
        {
            GlobalVar.isCopyingToUSB = false;
            //// 그리드 업데이트
            this.UpdateBurningGrid(trace);
            this.txtStatusView.AppendText(message);
        }

        /// <summary>
        /// 복사 진행 시작 후 조회된 환자 정보 Clear
        /// </summary>
        public void ClearSendUSBPatInfo(string patFolder)
        {
            try
            {
                if (GlobalVar.configEntity.programType == "1")
                {
                    this.ucPatients11.Clear();
                }
                else
                {
                    this.ucPatients21.RemoveAt(patFolder);
                }
            }
            catch { }
        }

        /// <summary>
        /// 하단의 홈페이지를 클릭했을 경우 원격지원 파일을 실행한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hyperLinkEdit1_OpenLink(object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e)
        {
            e.EditValue = "http://www.seetrol.co.kr/download/client.exe";
        }

        /// <summary>
        /// 서버연결
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrConnectServer_Tick(object sender, EventArgs e)
        {
            this.tmrConnectServer.Stop();
            this.tmrConnectServer.Enabled = false;

            this.ConnectServer();
        }

        /// <summary>
        /// 그리드의 폰트 색상
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvBurninglist_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (this._BurningList == null)
                return;

            try
            {
                string retCode = this._BurningList[e.RowHandle].Result;

                if (retCode == "2")
                {
                    //// 성공시 파란색
                    e.Appearance.ForeColor = Color.Blue;
                }
                else if (retCode == "9")
                {
                    //// 취소/실패시 빨강색
                    e.Appearance.ForeColor = Color.Red;
                }
            }
            catch { }
        }
    }

    /// <summary>
    /// 윈도우 이벤트 메시지 구조체
    /// </summary>
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpData;
    }
}
