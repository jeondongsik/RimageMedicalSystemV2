using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace RimageKorea
{
    /// <summary>
    /// 삼성병원 - 환자 정보 가져오기 (소켓통신)
    /// 개발 : 119.7.7.135
    /// 테스트 : 119.7.7.105
    /// 운영: 119.6.6.33
    /// 포트 : 21505
    /// </summary>
    public class GetPatientNameFromSocket
    {
        /// <summary>
        /// 환자명 가져오기
        /// </summary>
        /// <param name="patID"></param>
        /// <param name="ConInfo"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetName(string patID, Dictionary<string, string> ConInfo)
        {
            try
            {
                byte[] bytes = new byte[1024];
                
                int port = int.Parse(ConInfo["Port"]); //21505;
                string ip = ConInfo[ConInfo["CurrentTarget"]];
                string server = ConInfo["CurrentTarget"].Substring(6, 1);

                string message = GetRequestMessage(patID, server);

                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

                try
                {
                    sender.SendTimeout = 5000;
                    sender.ReceiveTimeout = 5000;
                    sender.Connect(localEndPoint);
                }
                catch
                {
                    throw;
                }

                byte[] data = Encoding.UTF8.GetBytes(message);
                int bytesSent = sender.Send(data);

                //데이터 받기
                int bytesRec = sender.Receive(bytes);

                string resultMsg = Encoding.UTF8.GetString(bytes, 0, bytesRec);

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

                return GetResponseMessage(resultMsg);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 요청 전문
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public static string GetRequestMessage(string patID, string serverType)
        {
            try
            {
                StringBuilder sbDesc = new StringBuilder();
                List<StreamFormat> requestSystemHeader = GetFormat("RequestSystemHeader");
                List<StreamFormat> requestJobHeader = GetFormat("RequestJobHeader");

                List<StreamData> requestSystemData = new List<StreamData>();
                //전문 전체 길이
                requestSystemData.Add(new StreamData("UUID", Guid.NewGuid().ToString(), "string"));
                requestSystemData.Add(new StreamData("TLGR_PRGR_NO", "1", "int"));
                requestSystemData.Add(new StreamData("TLGR_TYPE_CD", "Q", "string"));
                requestSystemData.Add(new StreamData("INRF_ID", "swd_p3ptntl_s01", "string"));
                requestSystemData.Add(new StreamData("TLGR_RQST_DT", string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), DateTime.Now.Millisecond.ToString().PadLeft(3, '0')), "string"));
                requestSystemData.Add(new StreamData("TLGR_RQST_SYST_IP", NetInfo.MyIP(), "string"));
                requestSystemData.Add(new StreamData("TLGR_MEDI_DVSN_CD", "pa", "string"));
                requestSystemData.Add(new StreamData("TLGR_RQST_SYST_ID", "pa3", "string"));
                requestSystemData.Add(new StreamData("TLGR_TRNM_USER_ID", "pa3002", "string"));
                requestSystemData.Add(new StreamData("SYST_ENVR_DVSN_CD", serverType, "string"));

                string data = string.Empty;
                string type = "string";

                foreach (StreamFormat format in requestSystemHeader)
                {
                    if (format.Request == "Y")
                    {
                        var req = from R in requestSystemData
                                  where R.EngName == format.EngName
                                  select R;

                        if (req != null && req.Count() == 1)
                        {
                            data = req.First().Data;
                            type = req.First().DataType;
                        }
                        else
                        {
                            data = string.Empty;
                            type = "string";
                        }

                        if (type == "int")
                            sbDesc.Append(data.PadLeft(format.Length, '0'));
                        else
                            sbDesc.Append(data.PadRight(format.Length, ' '));
                    }
                }

                data = string.Empty;
                type = "string";
                List<StreamData> requestJobData = new List<StreamData>();
                requestJobData.Add(new StreamData("USER_ID", "pa3002", "string"));

                foreach (StreamFormat format in requestJobHeader)
                {
                    if (format.Request == "Y")
                    {
                        var req = from R in requestJobData
                                  where R.EngName == format.EngName
                                  select R;

                        if (req != null && req.Count() == 1)
                        {
                            data = req.First().Data;
                            type = req.First().DataType;
                        }
                        else
                        {
                            data = string.Empty;
                            type = "string";
                        }

                        if (type == "int")
                            sbDesc.Append(data.PadLeft(format.Length, '0'));
                        else
                            sbDesc.Append(data.PadRight(format.Length, ' '));
                    }
                }

                sbDesc.Append(patID.PadRight(8, ' '));

                string totalLen = sbDesc.ToString().Length.ToString().PadLeft(10, '0');

                return totalLen + sbDesc.ToString();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 응답 전문
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetResponseMessage(string msg)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(msg))
                    return null;

                byte[] response = Encoding.UTF8.GetBytes(msg);


                List<StreamFormat> responseSystemHeader = GetFormat("ResponseSystemHeader");
                List<StreamFormat> responseJobHeader = GetFormat("ResponseJobHeader");
                List<StreamFormat> responseBody = GetFormat("ResponseBody");

                int offset = 0;
                Dictionary<string, string> resp = new Dictionary<string, string>();

                foreach (StreamFormat format in responseSystemHeader)
                {
                    if (format.Response == "Y")
                    {
                        resp.Add(format.EngName, Encoding.UTF8.GetString(response, offset, format.Length));
                        offset = offset + format.Length;
                    }
                }

                foreach (StreamFormat format in responseJobHeader)
                {
                    if (format.Response == "Y")
                    {
                        resp.Add(format.EngName, Encoding.UTF8.GetString(response, offset, format.Length));
                        offset = offset + format.Length;
                    }
                }

                if (resp["PRSG_RSLT_DVSN_CD"] == "1")
                {
                    foreach (StreamFormat format in responseBody)
                    {
                        if (format.Response == "Y")
                        {
                            resp.Add(format.EngName, Encoding.UTF8.GetString(response, offset, format.Length));
                            offset = offset + format.Length;
                        }
                    }
                }

                return resp;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 접속정보 가져오기
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetConnectionInfo(string filePath)
        {
            Dictionary<string, string> info = null;

            try
            {
                info = new Dictionary<string, string>();

                if (File.Exists(filePath))
                {
                    foreach (string line in File.ReadLines(filePath))
                    {
                        string[] arrStr = line.Split('=');
                        info.Add(arrStr[0].Replace(" ", ""), arrStr[1].Replace(" ", "").Replace("\"", ""));
                    }
                }
            }
            catch { }

            return info;
        }

        /// <summary>
        /// 전문 포멧 가져오기
        /// </summary>
        /// <param name="formatType">RequestSystemHeader, ResponseSystemHeader, RequestJobHeader, ResponseJobHeader</param>
        /// <returns></returns>
        private static List<StreamFormat> GetFormat(string formatType)
        {
            List<StreamFormat> messageFormat = new List<StreamFormat>();

            switch (formatType)
            {
                case "RequestSystemHeader":
                case "ResponseSystemHeader":
                    messageFormat.Add(new StreamFormat("TLGR_LNGT_VL", 0, 10, "N", "Y"));
                    messageFormat.Add(new StreamFormat("UUID", 10, 36, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("TLGR_PRGR_NO", 46, 2, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("TLGR_TYPE_CD", 48, 1, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("INRF_ID", 49, 15, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("TLGR_RQST_DT", 64, 20, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("LNKD_SYST_INFM_VL", 84, 8, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("SESN_ID", 92, 8, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("TLGR_RQST_SYST_IP", 100, 39, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("TLGR_MEDI_DVSN_CD", 139, 2, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("TLGR_RQST_SYST_ID", 141, 3, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("TLGR_TRNM_SRVC_ID", 144, 15, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("TLGR_TRNM_USER_ID", 159, 10, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("TLGR_RECV_SRVC_ID", 169, 15, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("TLGR_RECV_USER_ID", 184, 10, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("PRSG_RSLT_DVSN_CD", 194, 1, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("FILE_PRSG_YN", 195, 1, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("SYNC_PRSG_DVSN_CD", 196, 1, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("ENCR_YN", 197, 1, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("CMRN_YN", 198, 1, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("SYST_ENVR_DVSN_CD", 199, 1, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("TEST_TLGR_YN", 200, 1, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("INRF_STTS_CD", 201, 3, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("WINDOWS_HANDLE_ID", 204, 8, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("DATA_INFM_YN", 212, 1, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("SYST_HEDR_RMRK_VL", 213, 43, "Y", "Y"));
                    break;

                case "RequestJobHeader":
                case "ResponseJobHeader":
                    messageFormat.Add(new StreamFormat("EVNT_TYP_CD", 0, 1, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("HSPT_CD", 1, 5, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("CNTR_CD", 6, 6, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("DPRT_CD", 12, 6, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("NATN_CD", 18, 2, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("LNGG_CD", 20, 4, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("AFTR_MTRL_ENST_YN", 24, 1, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("MNMM_RQST_VL", 25, 4, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("MESG_CD", 29, 7, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("MESG_CTN", 36, 150, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("ADDD_MESG_CD", 186, 7, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("ADDD_MESG_CTN", 193, 150, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("ERR_PRGM_NM", 343, 15, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("ERR_FUNC_NM", 358, 40, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("ERR_LINE_NO", 398, 5, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("ERR_SQL_NO", 403, 8, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("ERR_TP_NO", 411, 8, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("ERR_MESG_PRSG_DVSN_CD", 419, 1, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("ENCF_CD", 420, 10, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("DTLS_ENCF_CD", 430, 10, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("MESG_MNGM_DVSN_YN", 440, 1, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("USER_ID", 441, 6, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("SCRY_APLY_YN", 447, 1, "Y", "Y"));
                    messageFormat.Add(new StreamFormat("BSWR_HEDR_RMRK_VL", 448, 52, "Y", "Y"));
                    break;
                case "ResponseBody":
                    messageFormat.Add(new StreamFormat("PTNO", 0, 8, "N", "Y"));
                    messageFormat.Add(new StreamFormat("PTNT_NM", 8, 100, "N", "Y"));
                    messageFormat.Add(new StreamFormat("GEND_CD", 108, 1, "N", "Y"));
                    messageFormat.Add(new StreamFormat("BTDT", 109, 14, "N", "Y"));
                    messageFormat.Add(new StreamFormat("PTNT_TLNO", 123, 30, "N", "Y"));
                    break;
                default:
                    break;
            }

            return messageFormat;
        }
    }

    /// <summary>
    /// 요청전문 형식
    /// </summary>
    public class StreamFormat
    {
        public string EngName { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }

        public StreamFormat(string eng, int offset, int len, string req, string res)
        {
            this.Request = req;
            this.Response = res;
            this.EngName = eng;
            this.Offset = offset;
            this.Length = len;
        }
    }

    public class StreamData
    {
        public string EngName { get; set; }
        public string Data { get; set; }
        public string DataType { get; set; }

        public StreamData(string name, string data, string type)
        {
            this.EngName = name;
            this.Data = data;
            this.DataType = type;
        }
    }
}
