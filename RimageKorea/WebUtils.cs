using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace RimageKorea
{
    public class WebUtils
    {
        /// <summary>
        /// 굽기 명령 저장
        /// </summary>
        /// <param name="Idx">굽기명령아이디</param>
        /// <param name="startDateTime">시작시각(yyyyMMddHHmmss)</param>
        /// <param name="endDateTime">종료시각(yyyyMMddHHmmss)</param>
        /// <param name="PatientID">환자아이디</param>
        /// <param name="PatientName">환자명</param>
        /// <param name="Copies">수량</param>
        /// <param name="MediaType">미디어유형</param>
        /// <param name="FilesLength">파일크기</param>
        /// <param name="Status">상태</param>
        /// <param name="StudyDesc">Study</param>
        /// <param name="multiPatYN">다중환자굽기 여부</param>
        /// <param name="hostIP">서버아이피</param>
        /// <param name="myIP">클라이언트 아이피</param>
        public static void InsertResult(string Idx, string startDateTime, string endDateTime, string PatientID, string PatientName, string Copies, string MediaType,
                       string FilesLength, string Status, string StudyDesc, string multiPatYN = "N", string hostIP = "", string myIP = "")
        {
            string response = "";
            string webaddress = "";

            webaddress = "http://" + hostIP + "/RimageWeb/InsertResult.aspx";
            //webaddress = "http://localhost:4557/RimageWeb/InsertResult.aspx";

            try
            {
                if (multiPatYN.Equals("Y"))
                {
                    PatientName = "심사용";
                }

                using (WebClient webc = new WebClient())
                {
                    NameValueCollection nvcol = new NameValueCollection();

                    nvcol.Add("Idx", Idx);
                    nvcol.Add("startDateTime", startDateTime);
                    nvcol.Add("endDateTime", endDateTime);
                    nvcol.Add("PatientID", PatientID);
                    nvcol.Add("PatientName", PatientName.Replace("'", ""));
                    nvcol.Add("Copies", Copies);
                    nvcol.Add("MediaType", MediaType);
                    nvcol.Add("FilesLength", FilesLength);
                    nvcol.Add("IPAddress", myIP);
                    nvcol.Add("Status", Status);
                    nvcol.Add("StudyDesc", StudyDesc.Replace("'", ""));

                    byte[] ResponseByte = webc.UploadValues(webaddress, nvcol);
                    response = Encoding.UTF8.GetString(ResponseByte);

                    webc.Dispose();

                    //// txtStatusView.AppendText(string.Format("{0} Input {1}\r\n", PatientName, response));
                }
            }
            catch
            {
                //
            }
        }
    }
}
