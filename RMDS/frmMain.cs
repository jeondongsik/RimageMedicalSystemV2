using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Xml;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Rimage.Client.Api;
using Rimage.Client.Api.Exception;
using RimageKorea;

namespace RMDS
{
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpData;
    }

    public partial class frmMain : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, ref COPYDATASTRUCT lParam);

        const int WM_COPYDATA = 0x004A;

        public string AppDirectory = Directory.GetCurrentDirectory();
        public delegate void OrderStatus(string orderStatus);
        public delegate void SysExecption(CSystemException sysExecption);
        
        public OrderStatus StatusDel;
        public SysExecption systemListenerDel;
        private SystemListener systemListener;
        
        /// <summary>
        /// 내 아이피
        /// </summary>
        string MyIP;
        /// <summary>
        /// 클라이언트 아이디
        /// </summary>
        string ClientId;
        /// <summary>
        /// 0.삭제안함, 1.성공했을때만 삭제, 2.성공,실패 모두 삭제
        /// </summary>
        string DeleteAfterBurn = "0";
        /// <summary>
        /// 서버유형(R:Remote, L:Local)
        /// </summary>
        string ServerType;
        /// <summary>
        /// 리마지 시스템폴더
        /// </summary>
        string RimageSystemFolderPath;
        /// <summary>
        /// 머지파일이 저장될 폴더
        /// </summary>
        string MergeFileFolder;
        
        //환자정보가 위치한 폴더
        string dicomCDFolder = string.Empty;
        
        /// <summary>
        /// 굽기 명령 종합 정보
        /// </summary>
        BurnOrderedInfoEntity burnOrderInfo = null;

        /// <summary>
        /// 접속 서버 정보
        /// </summary>
        string srvInfo = string.Empty;
        /// <summary>
        /// 굽기 명령 정보 - 파일명
        /// </summary>
        string orderID = string.Empty;
        /// <summary>
        /// OrderFile의 Full path
        /// </summary>
        string orderFilePath = string.Empty;
        /// <summary>
        /// 굽기진행 상태, 로그가 기록될 폴더
        /// </summary>
        string orderTracePath = string.Empty;
        /// <summary>
        /// 서버별로 폴더를 생성=>폴더경로
        /// </summary>
        string serverLogPath = string.Empty;

        /// <summary>
        /// 굽기명령 - RimageApi
        /// </summary>
        COrderDescription imgOrder = null;

        /// <summary>
        /// 상태구분
        /// </summary>
        string statusType = "";

        /// <summary>
        /// 현재 프로그램의 Process ID
        /// </summary>
        int processID = 0;

        /// <summary>
        /// 명령 종류
        /// </summary>
        string orderType = "O";
        string serverIP = string.Empty;
        string serverName = string.Empty;
        string serverPort = string.Empty;
        /// <summary>
        /// 호출한 폼의 핸들값
        /// </summary>
        IntPtr callFormHandle = IntPtr.Zero;

        /// <summary>
        /// 초를 체크하기 위함.
        /// </summary>
        int cSecond = 0;

        /// <summary>
        /// Creator
        /// </summary>
        public frmMain()
        {
            InitializeComponent();

            this.Visible = false;

            this.StatusDel = new OrderStatus(this.RecvOrderStatus);
            this.systemListenerDel = new SysExecption(this.sysExecption);
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="args">실행시킨 놈이 보낸 파라미터 배열</param>
        public frmMain(string[] args) : this()
        {
            if (args != null && args.Length > 0)
            {                
                //// 명령구분 : S[서버설정값], O[굽기명령]                
                try
                {
                    string[] arrArgs = args[0].Split('|');

                    if (arrArgs[0].Equals("S"))
                    {
                        //// 서버 설정값, 상태값만 가져온다.
                        this.orderType = arrArgs[0];
                        this.serverIP = arrArgs[1];
                        this.serverName = arrArgs[2];
                        this.serverPort = arrArgs[3];
                        this.callFormHandle = new IntPtr(Convert.ToInt32(arrArgs[4]));
                    }
                    else
                    {
                        //// 굽기 명령 실행
                        this.orderType = arrArgs[0];
                        this.orderID = arrArgs[1];
                        this.callFormHandle = new IntPtr(Convert.ToInt32(arrArgs[2]));
                    }
                }
                catch{}
            }
        }

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            this.GetConfig();

            //// 메인프로그램에 메시지 전송한다.
            this.SendWinMessage("BURN_SRT");

            //// 서버 설정-상태 정보 가져오기일 경우
            if (this.orderType.Equals("S"))
            {
                try
                {
                    //// 서버IP로 폴더 생성
                    this.serverLogPath = Path.Combine(GlobalVar.ProgramExecuteFolder, GlobalVar.ORDER_FOLDER, string.Format("SVR_{0}", this.serverIP));

                    //// 폴더가 없으면 생성
                    if (!Directory.Exists(this.serverLogPath))
                    {
                        Directory.CreateDirectory(this.serverLogPath);
                    }

                    GlobalVar.OrderTrackingFile = Path.Combine(this.serverLogPath, "Orders.xml");

                    //// 서버 접속
                    if (!this.ConnectServer())
                    {
                        //// 서버에 접속되지 않으므로 프로그램 종료
                        this.ApplicationExit(EnumExitType.Fail);
                    }

                    //// 서버 설정/상태 가져오기
                    this.GetServerConfig();

                    //// 서버 상태 가져오기
                    this.GetServerStatus();

                    //// 메인프로그램에 메시지 전송한다.
                    this.SendWinMessage("SRV_END");

                    //// 프로그램 종료
                    this.ApplicationExit(EnumExitType.Success);
                }
                catch (Exception ex)
                {
                    ErrorInfo err = new ErrorInfo();
                    err.Code = "9999";
                    err.Message = ex.Message;
                    err.Description = ex.ToString();                    
                    this.WriteErrLog(err, true);

                    //// 프로그램 종료
                    this.ApplicationExit(EnumExitType.Fail);
                }
            }
            else
            {
                //// 명령정보 읽어오기
                try
                {
                    this.orderFilePath = Path.Combine(GlobalVar.ProgramExecuteFolder, GlobalVar.ORDER_FOLDER, string.Format("{0}.json", orderID));
                    this.orderTracePath = Path.Combine(GlobalVar.ProgramExecuteFolder, GlobalVar.ORDER_FOLDER, this.orderID);

                    //// OrderID로 폴더를 생성한다.
                    if (!Directory.Exists(this.orderTracePath))
                    {
                        Directory.CreateDirectory(orderTracePath);
                    }

                    //// 저장된 경로에서 파일을 읽어온다.
                    this.burnOrderInfo = JsonParser.ConvertToBurnOrderedInfoEntityFromFile(this.orderID);

                    if (this.burnOrderInfo == null)
                    {
                        //// 명령정보 찾을 수 없음.
                        //// 로그 남김
                        //// 프로그램 종료
                        ErrorInfo err = new ErrorInfo();
                        err.Code = "8888";
                        err.Message = "굽기명령정보를 찾을 수 없습니다.\r\n다시 시도하세요!";
                        err.Description = "굽기명령 파일 미생성";

                        this.WriteErrLog(err, true);

                        //// 프로그램 종료
                        this.ApplicationExit(EnumExitType.Fail);
                    }
                    else
                    {
                        //// 서버 정보 
                        this.serverIP = this.burnOrderInfo.TargetServer.IP;
                        this.serverName = this.burnOrderInfo.TargetServer.Name;
                        this.serverPort = this.burnOrderInfo.TargetServer.Port;

                        //// 서버IP로 폴더 생성
                        this.serverLogPath = Path.Combine(GlobalVar.ProgramExecuteFolder, GlobalVar.ORDER_FOLDER, string.Format("SVR_{0}", this.burnOrderInfo.TargetServer.IP));
                        //// 폴더가 없으면 생성
                        if (!Directory.Exists(this.serverLogPath))
                        {
                            Directory.CreateDirectory(this.serverLogPath);
                        }

                        GlobalVar.OrderTrackingFile = Path.Combine(this.serverLogPath, "Orders.xml");

                        //// 서버 접속
                        if (!this.ConnectServer())
                        {
                            //// 서버에 접속되지 않으므로 프로그램 종료
                            this.ApplicationExit(EnumExitType.Fail);
                        }

                        //// 굽기 명령 전송
                        if (!this.SubmitOrder())
                        {
                            //// 명령 실패 => 프로그램 종료
                            this.ApplicationExit(EnumExitType.Fail);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorInfo err = new ErrorInfo();
                    err.Code = "9999";
                    err.Message = ex.Message;
                    err.Description = ex.ToString();
                    this.WriteErrLog(err, true);

                    //// 프로그램 종료
                    this.ApplicationExit(EnumExitType.Fail);
                }
            } 
        }

        /// <summary>
        /// 오류 로그 파일 남기기 - 하나의 파일에 넣지 않고 파일별로 생성한다.
        /// </summary>
        /// <param name="err"></param>
        private void WriteErrLog(ErrorInfo err, bool sendMessage = false)
        {
            try
            {
                err.OrderType = this.orderType;
                err.OrderID = this.orderID;
                err.ServerIP = this.serverIP;
                err.ServerName = this.serverName;

                string json = JsonParser.ConvertToJsonString(err);
                string fileName = string.Format("ERR_{0}{1}.txt", Utils.GetNowTime(), DateTime.Now.Millisecond.ToString().PadLeft(3, '0'));
                string fileFolder = Path.Combine(GlobalVar.ProgramExecuteFolder, GlobalVar.ORDER_FOLDER, "ERRORS");

                if (!Directory.Exists(fileFolder))
                    Directory.CreateDirectory(fileFolder);

                string filePath = Path.Combine(fileFolder, fileName);
                
                if (File.Exists(filePath))
                    File.Delete(filePath);

                FileControl.Write(json, filePath);

                if (sendMessage)
                {
                    //// 메인프로그램에 메시지 전송한다.
                    this.SendWinMessage(string.Format("ERROR_{0}", filePath));
                }
            }
            catch { }
        }

        /// <summary>
        /// 굽기진행 상태 파일로 생성한다.
        /// </summary>
        /// <param name="log"></param>
        private void WriteBurnTraceLog(string log)
        {
            try
            {
                string file = Path.Combine(this.orderTracePath, string.Format("{0}{1}{2}.txt", GlobalVar.PFX_TRC, Utils.GetNowTime(), DateTime.Now.Millisecond.ToString().PadLeft(3, '0')));

                FileControl.Write(log, file);
            }
            catch { }
        }

        /// <summary>
        /// 설정 정보 가져오기
        /// </summary>
        public void GetConfig()
        {
            try
            {
                this.AppDirectory = Application.StartupPath;
                Config cf = new Config(this.AppDirectory);

                this.MergeFileFolder = cf._MergeFileFolder;
                this.DeleteAfterBurn = (String.IsNullOrWhiteSpace(cf._DeleteAfterBurn)) ? "0" : cf._DeleteAfterBurn;
                
                //// 다운체크파일명을 전역변수에 저장
                GlobalVar.DOWN_CHK_FL_NM = (string.IsNullOrWhiteSpace(cf._DownCheckFileName)) ? "end.txt" : cf._DownCheckFileName;
                
                this.ServerType = (String.IsNullOrWhiteSpace(cf._ServerType)) ? "R" : cf._ServerType;

                this.MyIP = NetInfo.MyIP();
                this.ClientId = this.ServerType + this.MyIP.Replace(".", "_") + "_" + DateTime.Now.ToString("ddHHmmss") + RimageKorea.RandomOrderNumber.GetNewOrderNumber2();

                if (this.ServerType.Equals("L"))
                {
                    DirectoryInfo di = new DirectoryInfo(this.MergeFileFolder);
                    this.RimageSystemFolderPath = di.Parent.FullName;
                }

                this.processID = Process.GetCurrentProcess().Id;
            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite(this, ex.ToString(), Application.StartupPath);

                ErrorInfo err = new ErrorInfo();
                err.Code = "1001";
                err.Message = ex.Message;
                err.Description = ex.ToString();
                this.WriteErrLog(err, true);

                this.ApplicationExit(EnumExitType.Fail);
            }
        }
        
        /// <summary>
        /// 리마지 서버 연결
        /// </summary>
        /// <param name="retry">재연결 여부</param>
        private bool ConnectServer(bool retry = false)
        {
            //// Client ID는 명령 실행시마다 바뀐다.
            //// 재연결시에는 새로 생성하지 않는다.
            if (!retry)
                this.ClientId = this.ServerType + this.MyIP.Replace(".", "_") + "_" + DateTime.Now.ToString("ddHHmmss") + RimageKorea.RandomOrderNumber.GetNewOrderNumber2();
            
            try
            {
                if (CheckPing.TestPing(this.serverIP))
                {
                    //// 서버연결
                    if (CSystemManager.GetInstance().Connected == false)
                    {
                        CSystemManager.GetInstance().Connect(this.ClientId, this.serverIP, this.serverPort);
                    }

                    if (CSystemManager.GetInstance().Connected == true)
                    {
                        //Create system listener 
                        systemListener = new SystemListener(this);
                        CSystemManager.GetInstance().ListenForSystemStatus(systemListener);
                        
                        if (string.IsNullOrWhiteSpace(this.RimageSystemFolderPath))
                            this.RimageSystemFolderPath = CSystemManager.GetInstance().GetUncSystemFolder();

                        //// 설정파일에 저장
                        Config cf = new Config(Application.StartupPath);
                        cf.setConfigServerInfo(this.serverIP, this.serverName, this.serverPort);
                        cf = null;

                        RimageKorea.ErrorLog.TraceWrite(this, "-- [" + this.serverIP + "] Connected -- ", Application.StartupPath);

                        return true;
                    }
                }
                else
                {
                    ErrorInfo err = new ErrorInfo();
                    err.Code = "1001";
                    err.Message = string.Format("네트웤이 연결되지 않았습니다.\r\n서버IP[{0}]를 체크해 주세요.", this.serverIP);
                    err.Description = "Rimage Message : Ping Test";
                    this.WriteErrLog(err, true);
                }
            }
            catch (CMsgConnectFailedException me)
            {
                ////ErrMsgShow("서버연결에 실패하였습니다. 네트워크를 점검해 주세요.\r\n" + me.Message, "Rimage Message : ServerConnect", me);
                ErrorInfo err = new ErrorInfo();
                err.Code = "1001";
                err.Message = string.Format("네트웤이 연결되지 않았습니다.\r\n서버IP[{0}]를 체크해 주세요.", this.serverIP);
                err.Description = me.ToString();
                this.WriteErrLog(err, true);
            }
            catch (CBaseException be)
            {
                ErrorInfo err = new ErrorInfo();
                err.Code = "1001";
                err.Message = string.Format("네트웤이 연결되지 않았습니다.\r\n서버IP[{0}]를 체크해 주세요.", this.serverIP);
                err.Description = be.ToString();
                this.WriteErrLog(err, true);
            }

            return false;
        }

        /// <summary>
        /// 서버 설정값 가져오기
        /// </summary>
        private void GetServerConfig()
        {
            string sXml = "";
            string strProdConfig = "";

            CreateOrderXml createOrder = new CreateOrderXml();

            if (CSystemManager.GetInstance().Connected)
            {
                createOrder.SERVER = this.serverName.ToUpper();
                createOrder.SYSFOLDER = CSystemManager.GetInstance().GetUncSystemFolder();

                if (this.ServerType.Equals("R"))
                    createOrder.SYSFOLDER = "\\\\" + this.serverIP + "\\Rimage";
                else
                    createOrder.SYSFOLDER = this.RimageSystemFolderPath;    //CSystemManager.GetInstance().GetUncSystemFolder();

                createOrder.CLIENTID = this.ClientId;

                sXml = createOrder.GenerateProdRequestServeConfig();

                try
                {
                    CSystemManager.GetInstance().SetSynchronousTimeout(30000);
                    strProdConfig = CServerManager.GetInstance().ExecuteServerRequest(this.serverName.ToUpper() + "_PS01", sXml);

                    if (this.ServerType.Equals("R"))
                    {
                        strProdConfig = strProdConfig.Replace("D:\\Rimage\\XML\\", "\\\\" + this.serverIP + "\\Rimage\\XML\\");
                        strProdConfig = strProdConfig.Replace(this.serverName.ToUpper(), this.serverIP);
                    }
                    else
                    {
                        strProdConfig = strProdConfig.Replace(CSystemManager.GetInstance().GetUncSystemFolder(), this.RimageSystemFolderPath);
                    }

                    //// 파일로 저장한다.
                    string file = Path.Combine(this.serverLogPath, string.Format("{0}.txt", GlobalVar.SERVER_CONFIG_FL));

                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }

                    FileControl.Write(strProdConfig, file);
                }
                catch
                {
                    //// txtStatusView.AppendText("Rimage Client API ExecuteServerRequest exception! " + ex.Message + "\r\n");
                    //// 설정값은 오류가 나도 그냥 넘어감.
                }
            }
            else
            {
                ////txtStatusView.AppendText("Not connected to a Messaging Server\r\n");
                //// 설정값은 오류가 나도 그냥 넘어감.
            }
        }
        
        /// <summary>
        /// 서버 상태값 가져오기
        /// </summary>
        private void GetServerStatus()
        {
            string sXml = "";
            string strProdConfig = "";
            CreateOrderXml createOrder = new CreateOrderXml();

            if (CSystemManager.GetInstance().Connected)
            {
                createOrder.SERVER = this.serverName.ToUpper();
                createOrder.SYSFOLDER = CSystemManager.GetInstance().GetUncSystemFolder();

                if (this.ServerType.Equals("R"))
                    createOrder.SYSFOLDER = "\\\\" + this.serverIP + "\\Rimage";
                else
                    createOrder.SYSFOLDER = this.RimageSystemFolderPath;

                createOrder.CLIENTID = this.ClientId;

                sXml = createOrder.GenerateProdRequestAutoloaderStatus();

                try
                {
                    CSystemManager.GetInstance().SetSynchronousTimeout(60000);
                    strProdConfig = CServerManager.GetInstance().ExecuteServerRequest(this.serverName.ToUpper() + "_PS01", sXml);

                    if (this.ServerType.Equals("R"))
                    {
                        strProdConfig = strProdConfig.Replace("D:\\Rimage\\XML\\", "\\\\" + this.serverIP + "\\Rimage\\XML\\");
                        strProdConfig = strProdConfig.Replace(this.serverName.ToUpper(), this.serverIP);
                    }
                    else
                    {
                        strProdConfig = strProdConfig.Replace(CSystemManager.GetInstance().GetUncSystemFolder(), this.RimageSystemFolderPath);
                    }

                    //// 파일로 저장한다.
                    string file = Path.Combine(this.serverLogPath, string.Format("{0}.txt", GlobalVar.SERVER_STATUS_FL));

                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }

                    FileControl.Write(strProdConfig, file);
                }
                catch 
                {
                    ////txtStatusView.AppendText("Rimage Client API ExecuteServerRequest exception! " + ex.Message + "\r\n");
                    //// 설정값은 오류가 나도 그냥 넘어감.
                }
            }
            else
            {
                ////txtStatusView.AppendText("Not connected to a Messaging Server\r\n");
                //// 설정값은 오류가 나도 그냥 넘어감.
            }
        }
        
        /// <summary>
        /// --서버 연결 체크--
        /// </summary>
        /// <returns></returns>
        public bool CheckServerConnect()
        {
            bool returnValue = false;

            try
            {
                if (CSystemManager.GetInstance().Connected)
                {
                    returnValue = true;
                }
                else
                {
                    returnValue = false;
                }
            }
            catch
            {
                return false;
            }

            return returnValue;
        }

        /// <summary>
        /// 굽기 명령 보내기
        /// </summary>        
        private bool SubmitOrder()
        {
            try
            {
                string productionXml = CreateOrderXml.CreateProductionOrder(this.burnOrderInfo.DiscOrder, this.burnOrderInfo.TargetServer.IP, this.burnOrderInfo.RimageSystemFolder);
                this.burnOrderInfo.DiscOrder.ProductionOrderPath = Path.Combine(this.AppDirectory, this.burnOrderInfo.DiscOrder.OrderID + ".pOrd");

                OrderTracking.SaveProducitonFile(productionXml, this.burnOrderInfo.DiscOrder.ProductionOrderPath);

                this.burnOrderInfo.DiscOrder.Durable = "true";

                OrderTracking.AddOrder(this.burnOrderInfo.DiscOrder);

                this.imgOrder = COrderManager.GetInstance().SubmitDurableOrder(this.burnOrderInfo.OrderXml, (IOrderStatusListener)(new OrderListener(this)));

                //// 시작시 로그 저장
                WebUtils.InsertResult(this.burnOrderInfo.DiscOrder.OrderID,
                                  this.burnOrderInfo.StartDateTime,
                                  "",
                                  this.burnOrderInfo.patNo,
                                  this.burnOrderInfo.patName,
                                  this.burnOrderInfo.copies.ToString(),
                                  this.burnOrderInfo.mediType,
                                  this.burnOrderInfo.mediSize,
                                  "?",
                                  ((this.burnOrderInfo.BurnPatientKind.Equals("Y") || this.burnOrderInfo.patList.Count > 1) ? this.burnOrderInfo.DicomDescription : this.burnOrderInfo.StudyModality),
                                  Utils.CheckNull(this.burnOrderInfo.BurnPatientKind, "N"),
                                  this.serverIP,
                                  this.MyIP);

                //// 로그 남긴다.
                ErrorLog.TraceWrite(this, string.Format("OrderID:[{0}] 굽기 명령 전송 완료.", this.burnOrderInfo.DiscOrder.OrderID), GlobalVar.ProgramExecuteFolder);
            }
            catch (CMsgConnectFailedException me)
            {
                //// MessageBox.Show(me.Message + "\n Order Submission Failed");
                ErrorInfo err = new ErrorInfo();
                err.Code = "9999";
                err.Message = string.Concat("Order Submission Failed\n", me.Message);
                err.Description = me.ToString();
                this.WriteErrLog(err, true);

                return false;
            }
            catch (CBaseException be)
            {
                //// MessageBox.Show(be.Message + "\n Order Submission Failed");
                ErrorInfo err = new ErrorInfo();
                err.Code = "9999";
                err.Message = string.Concat("Order Submission Failed\n", be.Message);
                err.Description = be.ToString();
                this.WriteErrLog(err, true);

                return false;
            }

            return true;
        }

        /// <summary>
        /// 굽기 명령 => 이미지생성 끝난 후 ProductionOrder 실행.
        /// </summary>
        /// <param name="OrderXML"></param>
        /// <returns></returns>
        private bool SubmitOrder(string OrderXML)
        {
            try
            {
                COrderDescription imgOrder = COrderManager.GetInstance().SubmitDurableOrder(OrderXML, (IOrderStatusListener)(new OrderListener(this)));
            }
            catch (CMsgConnectFailedException me)
            {
                ErrorInfo err = new ErrorInfo();
                err.Code = "9999";
                err.Message = string.Concat("ReOrder Submission Failed\n", me.Message);
                err.Description = me.ToString();
                this.WriteErrLog(err, true);

                return false;
            }
            catch (CBaseException be)
            {
                ErrorInfo err = new ErrorInfo();
                err.Code = "9999";
                err.Message = string.Concat("ReOrder Submission Failed\n", be.Message);
                err.Description = be.ToString();
                this.WriteErrLog(err, true);

                return false;
            }

            return true;
        }

        /// <summary>
        /// 결과값 받기 - 진행로그에 저장
        /// </summary>
        /// <param name="xmlOrderStatus"></param>
        public void RecvOrderStatus(string xmlOrderStatus)
        {
            try
            {
                if (this.ServerType.Equals("R"))
                {
                    xmlOrderStatus = xmlOrderStatus.Replace("D:\\Rimage\\XML\\", "\\\\" + this.burnOrderInfo.TargetServer.IP + "\\Rimage\\XML\\");
                    xmlOrderStatus = xmlOrderStatus.Replace("C:\\Rimage\\XML\\", "\\\\" + this.burnOrderInfo.TargetServer.IP + "\\Rimage\\XML\\");
                    xmlOrderStatus = xmlOrderStatus.Replace(this.burnOrderInfo.TargetServer.Name.ToUpper(), this.burnOrderInfo.TargetServer.IP);
                    xmlOrderStatus = xmlOrderStatus.Replace("C:\\", "\\\\");
                    xmlOrderStatus = xmlOrderStatus.Replace("D:\\", "\\\\");
                }
                else
                {
                    xmlOrderStatus = xmlOrderStatus.Replace(CSystemManager.GetInstance().GetUncSystemFolder(), this.RimageSystemFolderPath);
                }

                //// Parse status information
                DiscStatus orderInfo = XMLParser.ParseOrderStatus(xmlOrderStatus);
                OrderTracking.ModifyOrder(orderInfo);
                this.statusType = (orderInfo.OrderType == "ImageOrderStatus") ? "Imaging " : "Producing ";

                //// 굽기 진행 상태 클래스에 세팅
                DiscStatusForDisplay statusDisp = new DiscStatusForDisplay();
                statusDisp.ProcessID = this.processID.ToString();
                statusDisp.OrderID = this.burnOrderInfo.DiscOrder.OrderID;
                statusDisp.ServerIP = this.burnOrderInfo.TargetServer.IP;
                statusDisp.Status = orderInfo.Status;               //// IN_PROCESS
                statusDisp.StatusType = this.statusType.Trim();          //// Imaging
                statusDisp.Stage = orderInfo.Stage;                 //// IN_PROCESS
                statusDisp.DeviceCurrentState = orderInfo.DeviceCurrentState;
                statusDisp.Finish = "N";
                statusDisp.ResultCode = "1";
                statusDisp.ResultMessage = "";
                
                //// Update user display
                if (!orderInfo.State.Equals("CANCELLED") || !orderInfo.State.Equals("FAILED"))
                {
                    statusDisp.StateString = this.statusType + orderInfo.State + " " + orderInfo.DeviceCurrentState;
                    statusDisp.PercentCompleted = orderInfo.PercentCompleted;

                    if (orderInfo.State.Trim() == "COMPLETED")
                    {
                        orderInfo.PercentCompleted = "100";
                        statusDisp.PercentCompleted = "100";
                    }

                    //// When Producing COMPLETED
                    if (this.statusType.Trim() == "Producing" && orderInfo.State.Trim() == "COMPLETED" && orderInfo.PercentCompleted == "100")
                    {
                        //// 굽기 성공 완료
                        //// 폴더안의 데이터 비교 
                        long startLen = this.burnOrderInfo.FolderSize;
                        long afterLen = FileControl.GetFolderLengthOnly(this.burnOrderInfo.patFolderFullPath);

                        statusDisp.ResultCode = "2";
                        statusDisp.ResultMessage = "완료";
                        
                        if (afterLen != startLen)
                        {
                            statusDisp.ResultCode = "9";
                            statusDisp.ResultMessage = "취소/실패";
                        }

                        statusDisp.Finish = "Y";
                    }
                    //// txtStatusView.AppendText(item.SubItems[3].Text + " : " + statusType + orderInfo.State + " " + orderInfo.DeviceCurrentState + " " + orderInfo.PercentCompleted + "%" + "\r\n");
                }

                //// 취소, 실패 시 처리
                if (orderInfo.State.Equals("CANCELLED") || orderInfo.State.Equals("FAILED"))
                {
                    statusDisp.ResultCode = "9";
                    statusDisp.ResultMessage = "취소/실패";
                    statusDisp.Finish = "Y";
                }

                //// 이미지오더가 완료되었을 경우 (성공 또는 실패) 시
                if (orderInfo.State.Equals("COMPLETED") || orderInfo.State.Equals("FAILED"))
                {
                    //// Stop listening to image order.
                    COrderDescription currentOrder;
                    currentOrder = new COrderDescription();
                    currentOrder.ClientId = orderInfo.ClientID;
                    currentOrder.OrderId = orderInfo.OrderID;
                    currentOrder.TargetCluster = "DefaultImageCluster";

                    COrderManager.GetInstance().StopListeningForOrder(currentOrder);

                    if (orderInfo.OrderType == "ProductionOrderStatus" || orderInfo.State.Equals("FAILED"))
                    {
                        //// If the order is complete or failed order information no longer needs to persist
                        OrderTracking.RemoveOrder(orderInfo);
                        OrderTracking.DeleteProductionFile(orderInfo.OrderID);
                        //// 이 경우 완전 완료된 상태임.
                        statusDisp.Finish = "Y";
                    }
                    else
                    {
                        //// If image order is completed submit production order
                        statusDisp.Finish = "N";
                        orderInfo.State = "Submitted for production";
                        OrderTracking.ModifyOrder(orderInfo);
                        string xmlProductionOrder = OrderTracking.GetProductionOrderFile(orderInfo);
                        if (xmlProductionOrder != null)
                        {
                            this.SubmitOrder(xmlProductionOrder);
                        }
                    }
                }

                //// 진행상태 기록
                try
                {
                    //// 상태를 Json string으로 변환하여 OrderID 폴더에 기록한다.
                    string js = JsonParser.ConvertToJsonString(statusDisp);
                    this.WriteBurnTraceLog(js);
                }
                catch { }

                //// 굽기완료 시 처리
                //// 오더 정보 Json으로 저장, 완료파일 생성, 결과저장, 파일 삭제
                if (statusDisp.Finish == "Y")
                {
                    try
                    {
                        //// 완료파일 생성
                        if (!File.Exists(System.IO.Path.Combine(this.burnOrderInfo.patFolderFullPath, GlobalVar.BURN_CHK_FL_NM)))
                        {
                            FileControl.CreateTextFile(System.IO.Path.Combine(this.burnOrderInfo.patFolderFullPath, GlobalVar.BURN_CHK_FL_NM));
                        }

                        //// 명령정보를 OrderHistory폴더에 저장한다.
                        string json = JsonParser.ConvertToJsonString(this.burnOrderInfo);
                        string fileName = Path.Combine(Application.StartupPath, GlobalVar.LOG_ORDER_FLD, DateTime.Now.ToString("yyyy-MM-dd"),
                            string.Format("{0}{1}{2}{3}.txt", DateTime.Now.ToString("yyMMddHHmmss"), this.burnOrderInfo.patNo, this.burnOrderInfo.patName,
                            RandomOrderNumber.GetNewOrderNumber().PadRight(7, '0')));

                        FileControl.Write(json, fileName);

                        //// 서버상태값 다시한번 불러온다.
                        //// 서버 설정/상태 가져오기
                        this.GetServerConfig();

                        //// 서버 상태 가져오기
                        this.GetServerStatus();
                    }
                    catch (Exception ex)
                    {
                        ErrorInfo err = new ErrorInfo();
                        err.Code = "9999";
                        err.Message = ex.Message;
                        err.Description = ex.ToString();
                        this.WriteErrLog(err);
                    }

                    ////결과저장
                    WebUtils.InsertResult(this.burnOrderInfo.DiscOrder.OrderID,
                              this.burnOrderInfo.StartDateTime,
                              Utils.GetNowTime(),
                              this.burnOrderInfo.patNo,
                              this.burnOrderInfo.patName,
                              this.burnOrderInfo.copies.ToString(),
                              this.burnOrderInfo.mediType,
                              this.burnOrderInfo.mediSize,
                              statusDisp.ResultMessage,
                              ((this.burnOrderInfo.BurnPatientKind.Equals("Y") || this.burnOrderInfo.patList.Count > 1) ? this.burnOrderInfo.DicomDescription : this.burnOrderInfo.StudyModality),
                              Utils.CheckNull(this.burnOrderInfo.BurnPatientKind, "N"),
                              this.serverIP,
                              this.MyIP);

                    //// EditList Xml파일삭제하자.
                    FileControl.DelEditListFile(this.burnOrderInfo.DiscOrder.OrderID, this.burnOrderInfo.TargetServer.IP, this.RimageSystemFolderPath, this.ServerType);

                    if (this.DeleteAfterBurn == "2" || (this.DeleteAfterBurn == "1" && statusDisp.ResultMessage == "완료"))
                    {
                        FileControl.DeleteFolder(this.burnOrderInfo.patFolderFullPath, false);
                    }

                    //// 메인프로그램에 메시지 전송한다.
                    this.SendWinMessage("BURN_END");

                    //// 굽기 종료되었으므로 프로그램 종료
                    //// 잠깐 쉬었다 종료
                    Thread.Sleep(1000);
                    this.ApplicationExit(EnumExitType.Success);
                }
            }
            catch (CMsgConnectFailedException me)
            {
                ErrorInfo err = new ErrorInfo();
                err.Code = "9999";
                err.Message = me.Message;
                err.Description = me.ToString();
                this.WriteErrLog(err, true);

                this.ApplicationExit(EnumExitType.Fail);
            }
            catch (CBaseException be)
            {
                ErrorInfo err = new ErrorInfo();
                err.Code = "9999";
                err.Message = be.Message;
                err.Description = be.ToString();
                this.WriteErrLog(err, true);

                this.ApplicationExit(EnumExitType.Fail);
            }
        }

        /// <summary>
        /// 리마지 시스템 오류 받기
        /// </summary>
        /// <param name="sysExecption"></param>
        public void sysExecption(CSystemException sysExecption)
        {
            if (sysExecption.Message.Contains("connection broken"))
            {
                Thread.Sleep(1000);

                //// 다시 연결
                if (!CSystemManager.GetInstance().Connected)
                {
                    //// 연결되어 있지 않다면 연결한다.
                    this.ConnectServer(true);
                    Thread.Sleep(1000);

                    if (!this.RecoverOrders())
                    {
                        //// 오더가 복구가 안될 경우 프로그램을 종료한다.
                        this.ApplicationExit(EnumExitType.Fail);
                    }
                }
            }
            else
            {
                ////ErrMsgShow("시스템에 일시적인 장애가 발생하였습니다.\r\n\r\n네트워크 상태 또는 서버 상태를 확인하세요.\r\n", "Rimage Message : sysExecption");
                ErrorInfo err = new ErrorInfo();
                err.Code = "9000";
                err.Message = "시스템에 일시적인 장애가 발생하였습니다.\r\n\r\n네트워크 상태 또는 서버 상태를 확인하세요.\r\n" + sysExecption.Message;
                err.Description = sysExecption.ToString();
                this.WriteErrLog(err, true);

                //// 프로그램 종료
                this.ApplicationExit(EnumExitType.Fail);
            }
        }

        /// <summary>
        /// 시스템 장애로 인해 굽기진행상태를 받지 못한 Order들을 복구시킨다.
        /// </summary>
        /// <returns></returns>
        private bool RecoverOrders()
        {
            try
            {
                if (CSystemManager.GetInstance().Connected)
                {
                    COrderManager.GetInstance().RecoverOrder(this.imgOrder, (IOrderStatusListener)(new OrderListener(this)));
                    COrderManager.GetInstance().ReceiveRecoveredStatuses();
                }
            }
            catch (Exception ex)
            {
                ErrorInfo err = new ErrorInfo();
                err.Code = "9999";
                err.Message = string.Concat("Recover Order Submission Failed\n", ex.Message);
                err.Description = ex.ToString();
                this.WriteErrLog(err, true);

                return false;
            }

            return true;
        }

        /// <summary>
        /// 굽기 취소 
        /// </summary>
        private void CancelOrder()
        {
            try
            {
                COrderDescription pOrder = new COrderDescription();
                pOrder.ClientId = ClientId;
                pOrder.OrderId = this.orderID;

                if (this.statusType.Contains("Imaging"))
                    pOrder.TargetCluster = "DefaultImageCluster";
                else
                    pOrder.TargetCluster = "DefaultProductionCluster";

                COrderManager.GetInstance().CancelOrder(pOrder, true);

                //// 프로그램을 종료시키는 타이머 실행
                //// this.timerAppExit.Enabled = true;
            }
            catch { }
        }

        /// <summary>
        /// 프로그램 종료
        /// </summary>
        private void ApplicationExit(EnumExitType etype)
        {
            //// 연결을 종료한다
            this.Disconnect();

            //// 굽기일 경우에만 ..
            if (this.orderType.Equals("O"))
            {
                try
                {
                    //// 종료 로그 파일 생성한다.
                    string file = Path.Combine(this.orderTracePath, GlobalVar.BURN_JOB_END_FL);
                    FileControl.Write(etype.ToString(), file);

                    //// 로그기록
                    if (this.burnOrderInfo != null)
                    {
                        ErrorLog.TraceWrite(this, string.Format("-- [{0}] Complete. [{1}] -- ", this.burnOrderInfo.DiscOrder.OrderID, etype.ToString()), Application.StartupPath);
                    }
                }
                catch { }
            }

            CSystemManager.Terminate();

            Application.Exit();
            Application.ExitThread();

            try
            {
                Environment.Exit(0);
            }
            catch { }
        }

        /// <summary>
        /// 서버연결끊기
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if ((CSystemManager.GetInstance().Connected))
                {
                    try
                    {
                        CSystemManager.GetInstance().RemoveSystemListener();
                        CSystemManager.GetInstance().Disconnect();
                    }
                    catch
                    {   
                    }

                    ErrorLog.TraceWrite(this, "-- [" + this.burnOrderInfo.TargetServer.IP + "] Disconnected -- ", Application.StartupPath);
                }
            }
            catch
            {                
            }
        }

        /// <summary>
        /// 윈도우 메시지 보내기
        /// </summary>
        /// <param name="msg"></param>
        private void SendWinMessage(string msg)
        {
            try
            {
                byte[] buff = System.Text.Encoding.Default.GetBytes(msg);
                
                COPYDATASTRUCT cds = new COPYDATASTRUCT();
                cds.dwData = IntPtr.Zero;
                cds.cbData = buff.Length + 1;
                cds.lpData = msg;
                
                SendMessage(this.callFormHandle, WM_COPYDATA, 0, ref cds);
            }
            catch { }
        }

        /// <summary>
        /// 프로그램 종료을 위한 타이머 Tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerAppExit_Tick(object sender, EventArgs e)
        {
            this.cSecond++;

            if (this.cSecond == 6)
            {
                ////5초 후에 종료
                this.timerAppExit.Enabled = false;
                this.ApplicationExit(EnumExitType.None);
            }
        }

        /// <summary>
        /// 취소 명령이 들어왔는지 체크
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerCancelCheck_Tick(object sender, EventArgs e)
        {
            this.timerCancelCheck.Enabled = false;

            if (this.CheckCancelFile())
            {
                this.CancelOrder();
            }
            else
            {
                this.timerCancelCheck.Enabled = true;
            }
        }

        /// <summary>
        /// 취소명령이 들어왔는지 체크한다.
        /// </summary>
        /// <returns></returns>
        private bool CheckCancelFile()
        {
            try
            {
                string folder = Path.Combine(GlobalVar.ProgramExecuteFolder, GlobalVar.ORDER_FOLDER, "CANCEL");
                
                foreach (string dir in Directory.GetFiles(folder, this.orderID))
                {
                    if (dir.EndsWith(this.orderID))
                    {
                        return true;
                    }
                }
            }
            catch { }

            return false;
        }
    }
}
