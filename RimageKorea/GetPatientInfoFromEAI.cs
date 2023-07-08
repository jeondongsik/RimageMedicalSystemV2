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
    public static class GetPatientInfoFromEAI
    {

        public static Dictionary<string, string> GetName(string patID)
        {
            try
            {
                Dictionary<string, string> patInfo = new Dictionary<string, string>();



            }
            catch { }

            return null;
        }

        public static Dictionary<string, string> RequestEAI(string patID)
        {
            try
            {
                Dictionary<string, string> patInfo = new Dictionary<string, string>();

                //// 개발 : http://10.90.40.22:27000/indigo/routeService
                //// 실습 : http://10.90.50.70:27000/indigo/routeService
                //// 운영 : http://10.90.30.26:27000/indigo/routeService
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://10.90.40.22:27000/indigo/routeService");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencode";
                request.Headers.Add("IfId", "IF_P04_L02_A03_101");
                request.Headers.Add("TxId", "20230708331882200000007");
                request.Headers.Add("Param", "WWAWP0051R");
                request.Headers.Add("sourceLocale", "UTF-8");
                request.Headers.Add("reqDt", "20230708131010001");
                request.Headers.Add("serviceName", "WWAWP0051R");

                ReqRoot root = new ReqRoot();
                string jsBody = JsonParser.ConvertToJsonString(root);

                StreamWriter reqStream = new StreamWriter(request.GetRequestStream());
                reqStream.Write(jsBody); //http body에 data 쓰기
                reqStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); //요청 보내고 응답 기다리기
                StreamReader respStream = new StreamReader(response.GetResponseStream());
                string resp = respStream.ReadToEnd();   // 결과를 string으로 읽기
                respStream.Close();

                //// 응답값
                RespRoot ret = JsonConvert.DeserializeObject<RespRoot>(resp); 



                /*
                 IfId: IF_P04_L02_A03_101
TxId: 20230708331882200000007
Param: WWAWP0051R
sourceLocale: UTF-8
reqDt: 20230708131010001
serviceName: WWAWP0051R
                 */
            }
            catch { }

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
