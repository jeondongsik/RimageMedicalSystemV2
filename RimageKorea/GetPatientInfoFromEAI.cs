using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace RimageKorea
{
    /// <summary>
    /// 아산병원 EGG 뇌파 환자 정보 EAI 통해서 가져오기
    /// </summary>
    public static class GetPatientInfoFromEAI
    {
        /// <summary>
        /// 환자정보 가져오기 (PatId, PatName:이름, Age:나이)
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetPatInfo(string patID)
        {
            Dictionary<string, string> patInfo = null;

            try
            {
                patInfo = RequestEAI(patID);
            }
            catch { }

            return patInfo;
        }

        /// <summary>
        /// Http Request Header 생성
        /// </summary>
        /// <returns></returns>
        public static HttpWebRequest CreateHeader()
        {
            try
            {
                string url = "http://10.90.30.26:27000/indigo/routeService";    //// 운영적용

                //// 개발 : http://10.90.40.22:27000/indigo/routeService
                //// 실습 : http://10.90.50.70:27000/indigo/routeService
                //// 운영 : http://10.90.30.26:27000/indigo/routeService
                /*
                 TxId 는 고정값인지? 아니면 유니크한 값을 생성해야 하는지?
                 ip 는 호출하는 컴퓨터의 값을 보내면 되나?
                 */
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencode";
                request.Headers.Add("IfId", "IF_P04_L02_A03_101");
                request.Headers.Add("TxId", "20230708331882200000007");             //// 20230708331882 200 000007
                request.Headers.Add("Param", "WWAWP0051R");
                request.Headers.Add("sourceLocale", "UTF-8");
                request.Headers.Add("reqDt", DateTime.Now.ToString("yyyyMMddHHmmssfff"));      //// 20230708131010 001
                request.Headers.Add("serviceName", "WWAWP0051R");

                return request;
            }
            catch { throw; }
        }

        /// <summary>
        /// 전송할 파라미터 생성
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public static string CreateReqParam(string patID)
        {
            try
            {
                ReqRoot reqParams = new ReqRoot();
                reqParams.header = new ReqHeader();
                reqParams.header.systemid = "IMR";
                reqParams.header.user = "";
                reqParams.header.ip = GetMyIP.MyIP();
                reqParams.header.dataFlag = "Y";
                reqParams.header.version = 1;
                reqParams.header.source = "WA";
                reqParams.header.bizCode = "";
                reqParams.header.debug = 1;
                reqParams.header.customerMessage = "";
                reqParams.header.messageId = "";
                reqParams.header.resultCode = "";
                reqParams.header.guid = "";
                reqParams.header.reqdt = "";
                reqParams.header.resdt = "";
                reqParams.header.trno = 0;
                reqParams.header.encToken = "";
                reqParams.header.sid = "";
                reqParams.header.className = "kr.amc.amis.wa.wp";
                reqParams.header.dbName = "";
                reqParams.header.server = "";
                reqParams.header.service = "WWAWP0051R";
                reqParams.header.issingle = 1;

                reqParams.preDatas = new ReqPreDatas();
                reqParams.preDatas.ipd = new Ipd();
                reqParams.preDatas.ipd.paid = new Paid();
                reqParams.preDatas.ipd.paid.type = "S";
                reqParams.preDatas.ipd.paid.value = patID;

                string jsBody = JsonParser.ConvertToJsonString(reqParams);

                return jsBody;
            }
            catch { throw; }
        }

        public static Dictionary<string, string> RequestEAI(string patID)
        {
            try
            {
                Dictionary<string, string> patInfo = null;

                HttpWebRequest request = CreateHeader();

                string jsBody = CreateReqParam(patID);

                StreamWriter reqStream = new StreamWriter(request.GetRequestStream());
                reqStream.Write(jsBody); //http body에 data 쓰기
                reqStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); //요청 보내고 응답 기다리기
                StreamReader respStream = new StreamReader(response.GetResponseStream());
                string resp = respStream.ReadToEnd();   // 결과를 string으로 읽기
                respStream.Close();

                //// 응답값
                RespRoot ret = JsonConvert.DeserializeObject<RespRoot>(resp); 
                if (ret != null)
                {
                    if ("S" == ret.header.resultCode)
                    {
                        patInfo = new Dictionary<string, string>();
                        patInfo.Add("PatId", ret.preDatas.opd.paid.value);
                        patInfo.Add("PatName", ret.preDatas.opd.panm.value);
                        patInfo.Add("PatSex", Utils.GetGenderWithRgn(ret.preDatas.opd.rgn1.value, ret.preDatas.opd.rgn2.value));

                        ////주민등록번호로 나이 계산하기
                        int age = Utils.CalcuAgeWithRgn(ret.preDatas.opd.rgn1.value, ret.preDatas.opd.rgn2.value, ret.preDatas.opd.paid.value);
                        patInfo.Add("Age", age.ToString());
                    }
                }

                return patInfo;
                /*
                 IfId: IF_P04_L02_A03_101
TxId: 20230708331882200000007
Param: WWAWP0051R
sourceLocale: UTF-8
reqDt: 20230708131010001
serviceName: WWAWP0051R
                 */
            }
            catch (Exception ex)
            {
                StringBuilder sbObject = new StringBuilder();
                sbObject.Append("RimageKorea.GetPatientInfoFromEAI");

                ErrorLog.LogWrite(sbObject, ex.ToString(), Environment.CurrentDirectory);
            }

            return null;
        }
    }

    public class ReqRoot
    {
        public ReqHeader header { get; set; }
        public ReqPreDatas preDatas { get; set; }
    }

    public class ReqHeader
    {
        public string systemid { get; set; }
        public string user { get; set; }
        public string ip { get; set; }
        public string dataFlag { get; set; }
        public int version { get; set; }
        public string source { get; set; }
        public string bizCode { get; set; }
        public int debug { get; set; }
        public string customerMessage { get; set; }
        public string messageId { get; set; }
        public string message { get; set; }
        public string resultCode { get; set; }
        public string guid { get; set; }
        public string reqdt { get; set; }
        public string resdt { get; set; }
        public int trno { get; set; }
        public string encToken { get; set; }
        public string sid { get; set; }
        public string className { get; set; }
        public string dbName { get; set; }
        public string server { get; set; }
        public string service { get; set; }
        public int issingle { get; set; }
    }

    public class Paid
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Ipd
    {
        public Paid paid { get; set; }
    }

    public class ReqPreDatas
    {
        public Ipd ipd { get; set; }
    }



    public class RespRoot
    {
        public RespHeader header { get; set; }
        public RespPreDatas preDatas { get; set; }
    }

    public class RespHeader
    {
        public string systemid { get; set; }
        public string user { get; set; }
        public string ip { get; set; }
        public string dataFlag { get; set; }
        public double version { get; set; }
        public string source { get; set; }
        public string bizCode { get; set; }
        public int debug { get; set; }
        public string customerMessage { get; set; }
        public string messageId { get; set; }
        public string message { get; set; }
        public string resultCode { get; set; }
        public string bizResultCode { get; set; }
        public string guid { get; set; }
        public string reqdt { get; set; }
        public string resdt { get; set; }
        public int trno { get; set; }
        public string encToken { get; set; }
        public string sid { get; set; }
        public string className { get; set; }
        public string dbName { get; set; }
        public string server { get; set; }
        public string service { get; set; }
        public string serviceName { get; set; }
        public string serviceDesc { get; set; }
        public string recordSetType { get; set; }
        public int issingle { get; set; }
        public string userinfo { get; set; }
        public string sessionId { get; set; }
    }

    public class Rtrn
    {
        public string type { get; set; }
        public int value { get; set; }
    }
    
    public class Rgn1
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Rgn2
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Panm
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Opd
    {
        public Rtrn rtrn { get; set; }
        public Paid paid { get; set; }
        public Rgn1 rgn1 { get; set; }
        public Rgn2 rgn2 { get; set; }
        public Panm panm { get; set; }
    }

    public class RespPreDatas
    {
        public Opd opd { get; set; }
    }
}
