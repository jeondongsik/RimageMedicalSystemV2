using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Data;
using System.Xml;

namespace RimageKorea
{
    /// <summary>
    /// 원광대 URL 호출하여 환자정보 불러오기 (xml 반환)
    /// </summary>
    public static class GetPatientNameFromCallUrl
    {
        /// <summary>
        /// 원광대 환자 정보 가져오기
        /// </summary>
        /// <param name="stype">환자번호 : 1, 주민번호 : 2</param>
        /// <param name="inputString">type에 따라서 넘겨줘야 할 텍스트</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetPatInfo(string stype, string inputString)
        {
            Dictionary<string, string> returnValue = null;
            string outputString = string.Empty;
            StringBuilder sbObject = null;

            try
            {
                //// 126 의대병원 기관코드
                //// 127 한방병원 기관코드
                //// 128 치대병원 기관코드
                //// 교육서버 테스트용 string pUrl = @"https://edu126.wkuh.org/cmcnu/.live";

                string pUrl = @"https://emr126.wkuh.org/cmcnu/.live";
                string pParam = string.Format("submit_id=DRAZZ00403&business_id=al&pid={0}&instcd={1}", inputString, "126");

                string xmlText = GetPatientNameFromCallUrl.HttpCall(pUrl, pParam, "GET");
                ////StringBuilder sbXml = new StringBuilder();
                ////sbXml.Append("<?xml version=\"1.0\" encoding=\"utf - 8\"?>");
                ////sbXml.Append("< root >< orddinfo >< item >< hngnm >< ![CDATA[복사2]] ></ hngnm >< rgstno1 >< ![CDATA[890101]] ></ rgstno1 >< rgstno2 >< ![CDATA[2]] ></ rgstno2 ></ item >");
                ////sbXml.Append("< resultKM type = \"status\" error = \"no\"  clear = \"true\" description = \"info|정상 처리되었습니다.\" updateinstance = \"true\" source = \"1718031394665\" />");
                ////sbXml.Append("< message >< type > info </ type >< code > info </ code >< msg > 정상 처리되었습니다.</ msg >< description ></ description ></ message >");
                ////sbXml.Append("</ orddinfo ></ root >");

                returnValue = new Dictionary<string, string>();
                ////returnValue.Add("xmlText", xmlText);
                returnValue.Add("환자번호", inputString);

                ////string path = @"C:\test11.xml";
                //XmlReader rd = XmlReader.Create(pUrl + "?" + pParam);

                //StringBuilder sb = new StringBuilder();

                //while (rd.Read())
                //{
                //    sb.AppendLine(rd.ReadOuterXml());
                //}
                //string xmltext = sb.ToString();

                // Create memory stream 
                //MemoryStream memStream = new MemoryStream();

                //byte[] data = Encoding.Unicode.GetBytes(xmlText);
                //memStream.Write(data, 0, data.Length);
                //memStream.Position = 0;

                // rd.Close();

                ////string filePath = Path.Combine(Environment.CurrentDirectory, GlobalVar.LOG_ORDER_FLD, "12345.xml");
                ////FileControl.createXmlFile(sbXml.ToString(), filePath);

                var m_strFilePath = pUrl + "?" + pParam;
                string xmlStr = string.Empty;

                using (var wc = new WebClient())
                {
                    wc.Encoding = System.Text.Encoding.UTF8;
                    xmlStr = wc.DownloadString(m_strFilePath);                    
                }
                
                ////ErrorLog.LogWrite("RimageKorea.GetPatientNameFromCallUrl", xmlStr, Environment.CurrentDirectory);

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlStr);
               
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmlDoc.OuterXml);

                ErrorLog.LogWrite("RimageKorea.GetPatientNameFromCallUrl", xml.OuterXml, Environment.CurrentDirectory);
                XmlNodeList nodeList1 = xml.SelectNodes("/root/orddinfo/item/hngnm");
                string hngnm = string.Empty;
                string rgstno1 = string.Empty;
                string rgstno2 = string.Empty;

                foreach (XmlNode node in nodeList1)
                {
                    hngnm = node.InnerText;
                    ////ErrorLog.LogWrite("RimageKorea.GetPatientNameFromCallUrl - 1", node.InnerText, Environment.CurrentDirectory);
                }

                XmlNodeList nodeList2 = xml.SelectNodes("/root/orddinfo/item/rgstno1");
                foreach (XmlNode node in nodeList2)
                {
                    rgstno1 = node.InnerText;
                    ////ErrorLog.LogWrite("RimageKorea.GetPatientNameFromCallUrl - 2", node.InnerText, Environment.CurrentDirectory);
                }

                XmlNodeList nodeList3 = xml.SelectNodes("/root/orddinfo/item/rgstno2");
                foreach (XmlNode node in nodeList3)
                {
                    rgstno2 = node.InnerText;
                    ////ErrorLog.LogWrite("RimageKorea.GetPatientNameFromCallUrl - 3", node.InnerText, Environment.CurrentDirectory);
                }

                //// 환자명
                returnValue.Add("환자명", hngnm.Trim());
                //// 성별
                returnValue.Add("성별", Utils.GetGenderWithRgn("", rgstno2));
                //// 생년월일
                if (rgstno2 == "1" || rgstno2 == "2")
                    rgstno1 = "19" + rgstno1;
                else
                    rgstno1 = "20" + rgstno1;

                returnValue.Add("생년월일", rgstno1);

                int Age = Utils.GetPatientAge(rgstno1, inputString);
                returnValue.Add("나이", Age.ToString());
                
                //// 결과는 xml 형태임
                //// example
                /*
                 *  hngnmn	환자명
                    rgstno1	주민번호앞자리
                    rgstno2	주민번호뒷자리-(한자리)

                 <root>
	                <orddinfo>
		                <item>
			                <hngnm> <![CDATA[ 복사1 ]]> </hngnm>
			                <rgstno1>
			                    <![CDATA[ 920101 ]]>
			                </rgstno1>
			                <rgstno2>
			                    <![CDATA[ 1 ]]>
			                </rgstno2>
		                </item>
		                <resultKM type="status" error="no" clear="true" description="info|정상 처리되었습니다." updateinstance="true" source="1717936144740"/>
		                <message>
			                <type>info</type>
			                <code>info</code>
			                <msg>정상 처리되었습니다.</msg>
			                <description/>
		                </message>
	                </orddinfo>
                </root>
                */


            }
            catch (Exception ex)
            {
                sbObject = new StringBuilder();
                sbObject.Append("RimageKorea.GetPatientNameFromCallUrl");

                ErrorLog.LogWrite(sbObject, ex.ToString(), Environment.CurrentDirectory);
            }

            return returnValue;
        }

        /// <summary>
        /// HttpRequest GET 호출
        /// </summary>
        /// <param name="p_sUrl"></param>
        /// <param name="p_sParam"></param>
        /// <param name="p_sMethod"></param>
        /// <returns></returns>
        public static string HttpCall(string p_sUrl, string p_sParam, string p_sMethod)
        {
            try
            {
                HttpWebRequest httpWebRequest = null;

                //// 인코딩 UTF-8
                byte[] sendData = Encoding.UTF8.GetBytes(p_sParam);

                if (p_sMethod == "POST")
                {
                    httpWebRequest = (HttpWebRequest)WebRequest.Create(p_sUrl);
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    httpWebRequest.Method = p_sMethod;
                    httpWebRequest.ContentLength = sendData.Length;

                    Stream requestStream = httpWebRequest.GetRequestStream();

                    requestStream.Write(sendData, 0, sendData.Length);
                    requestStream.Close();
                }
                else if (p_sMethod == "GET")
                {
                    httpWebRequest = (HttpWebRequest)WebRequest.Create(p_sUrl + "?" + p_sParam);
                    httpWebRequest.Method = p_sMethod;
                }

                HttpWebResponse httpWebResponse;

                using (httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                    string result = streamReader.ReadToEnd();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
