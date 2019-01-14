using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Rimage.Rapid.Dotnet.Api;

namespace RimageKorea
{
    public class GetServerMode
    {
        string HostIP;
        string HostName;
        string HostPort;
        string ClientID;

        public string errMessage
        {
            get;
            set;
        }
        public int retResult
        {
            get;
            set;
        }
        public string AppDirectory
        {
            get;
            set;
        }
        public int NumberOfOrders
        {
            get;
            set;
        }
        public string OrderLists
        {
            get;
            set;
        }

        public GetServerMode()
        {
            this.retResult = 0;
            this.errMessage = "";            
        }

        #region 서버 접속 정보
        public void getConfig()
        {
            Config cf = new Config(AppDirectory);

            HostIP = cf._HostIP;
            HostName = cf._HostName;
            HostPort = cf._HostPort;

            string MyIP = NetInfo.MyIP();
            ClientID = "M" + MyIP.Replace(".", "") + "00";
        } 
        #endregion

        #region 현재 서버 모드
        public string ServerMode()
        {
            string result = "";
            string param = "";

            Thread.Sleep(1000);
            if (this.retResult == 0 && SingleConnectionSession.GetInstance().Connected)
            {
                ProductionServer srv = (ProductionServer)SingleConnectionSession.GetInstance().GetServer(HostName.ToUpper() + "_PS01");

                if (srv == null)
                {
                    this.retResult = -3;
                    this.errMessage = "서버에 연결하는 중 에러가 발생했습니다.";
                }
                else
                {                    
                    param = srv.GetProductionParam(ProductionModeParamType.OperatingModeParam).ToString().ToLower().Replace(" ", "");
                }

                result = param;
                this.retResult = 0;
            }
            else
            {
                this.retResult = -1;
                this.errMessage = "서버에 연결되어 있지 않습니다.";
            }


            return result;
        } 
        #endregion

        #region Reset Bin
        public void ResetBin(ushort autoLoader)
        {
            if (this.retResult == 0 && SingleConnectionSession.GetInstance().Connected)
            {
                ProductionServer srv = (ProductionServer)SingleConnectionSession.GetInstance().GetServer(HostName.ToUpper() + "_PS01");

                if (srv == null)
                {
                    this.retResult = -3;
                    this.errMessage = "서버에 연결하는 중 에러가 발생했습니다.";
                }
                else
                {
                    srv.ResetInputBins(autoLoader);
                    this.retResult = 0;
                }
            }
        } 
        #endregion

        #region BulkRead Mode로 변경
        public bool setBulkReadMode()
        {
            bool retBl = false;

            Thread.Sleep(1000);

            if (SingleConnectionSession.GetInstance().Connected)
            {
                ProductionServer srv = (ProductionServer)SingleConnectionSession.GetInstance().GetServer(HostName.ToUpper() + "_PS01");

                if (srv.GetProductionParam(ProductionModeParamType.OperatingModeParam).ToString().ToLower().Replace(" ", "") != "bulkread")
                {
                    try
                    {
                        srv.Pause();
                    }
                    catch (RimageException ex)
                    {
                        this.retResult = -1;
                        this.errMessage = ex.Message;
                    }
                    Thread.Sleep(3000);
                    string param = srv.GetProductionParam(ProductionModeParamType.OperatingModeParam);
                    srv.SetProductionParam(ProductionModeParamType.OperatingModeParam, ServerParamValues.ProductionOperatingModeBulkRead);
                    srv.CommitParamChanges();                    

                    try
                    {
                        srv.Resume();
                        retBl = true;
                        this.retResult = 0;
                        Thread.Sleep(3000);
                    }
                    catch (RimageException ex)
                    {
                        this.retResult = -1;
                        this.errMessage = ex.Message;
                    }
                }
            }
            return retBl;
        }
        #endregion

        #region Normal Mode로 변경
        public bool setNormalMode()
        {
            bool retBl = false;

            Thread.Sleep(1000);

            if (SingleConnectionSession.GetInstance().Connected)
            {
                ProductionServer srv = (ProductionServer)SingleConnectionSession.GetInstance().GetServer(HostName.ToUpper() + "_PS01");

                if (srv.GetProductionParam(ProductionModeParamType.OperatingModeParam).ToString().ToLower() != "normal")
                {
                    try
                    {
                        srv.Pause();
                    }
                    catch (RimageException ex)
                    {
                        this.retResult = -1;
                        this.errMessage = ex.Message;
                    }
                    Thread.Sleep(3000);

                    string param = srv.GetProductionParam(ProductionModeParamType.OperatingModeParam);
                    srv.SetProductionParam(ProductionModeParamType.OperatingModeParam, ServerParamValues.ProductionOperatingModeNormal);
                    srv.CommitParamChanges();

                    try
                    {
                        srv.Resume();
                        retBl = true;
                        this.retResult = 0;
                        Thread.Sleep(3000);
                    }
                    catch (RimageException ex)
                    {
                        this.retResult = -1;
                        this.errMessage = ex.Message;
                    }
                }
            }
            return retBl;
        }
        #endregion

        #region Get Active Orders
        public void getActiveOrders()
        {
            StringBuilder sb = null;
            int number = 0;

            Thread.Sleep(1000);

            try
            {
                if (SingleConnectionSession.GetInstance().Connected)
                {
                    ProductionServer srv = (ProductionServer)SingleConnectionSession.GetInstance().GetServer(HostName.ToUpper() + "_PS01");

                    if (srv != null)
                    {
                        sb = new StringBuilder();

                        foreach (ServerOrder order in srv.GetActiveOrders())
                        {
                            sb.Append("OrderId = " + order.OrderId + ", ");
                            sb.Append("Related JobId = " + order.JobId + ", ");
                            sb.Append("ClientId = " + order.ClientId + ", ");
                            sb.Append("State = " + order.State.ToString() + ", ");
                            sb.Append("Stage = " + order.Stage.ToString() + ", ");
                            sb.Append("Percent done = " + order.PercentCompleted.ToString() + "\r\n");

                            number++;
                        }

                        this.retResult = 0;
                        this.OrderLists = sb.ToString();
                    }
                    else
                    {
                        this.retResult = -3;
                        this.errMessage = "서버에 연결하는 중 에러가 발생했습니다.";
                    }
                }
                else
                {
                    this.retResult = -1;
                    this.errMessage = "서버에 연결되어 있지 않습니다.";
                }
            }
            catch (Exception ex)
            {
                this.retResult = -1;
                this.errMessage = ex.Message;
            }

            this.NumberOfOrders = number;
        } 
        #endregion

        #region Connect Single
        public void ConnectSingle()
        {
            try
            {
                if (CheckPing.TestPing(HostIP))
                {
                    SingleConnectionSession.GetInstance().Connect(ClientID, HostIP, HostPort);
                    Thread.Sleep(3000);

                    this.retResult = 0;
                }
                else
                {
                    this.retResult = -1;
                    this.errMessage = "네트웤이 연결되지 않았습니다.\r\n서버 IP를 체크해 주세요.";
                }
            }
            catch
            {
                this.retResult = -2;
                this.errMessage = "서버연결에 실패하였습니다. 네트워크를 점검해 주세요.\r\n";
            }
        } 
        #endregion

        #region Disconnect
        public void Disconnect()
        {
            try
            {
                if (SingleConnectionSession.GetInstance().Connected)
                {
                    SingleConnectionSession.GetInstance().Disconnect();
                    this.retResult = 0;
                }
                else
                {
                    this.retResult = 0;
                }
            }
            catch
            {
                this.retResult = -2;
                this.errMessage = "서버연결을 끊는 도중 에러가 발생했습니다.";
            }
        } 
        #endregion
    }
}
