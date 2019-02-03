using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace RimageMedicalSystemV2
{
    public class MainFormBiz
    {
        /// <summary>
        /// 라벨 프린트하기
        /// </summary>
        /// <param name="patID"></param>
        /// <param name="patName"></param>
        /// <param name="patSex"></param>
        /// <param name="patAge"></param>
        public static string LabelPrint(string ip, string patID, string patName, string patSex, string patAge, string copies)
        {
            string response = "";
            string webaddress = "";
            webaddress = "http://" + ip + "/RimageWeb/LabelPrint.aspx";
            //webaddress = "http://localhost:4557/RimageWeb/LabelPrint.aspx";

            if (patSex != "" && patAge != "0")
            {
                patName += "   (" + patSex + "\\" + patAge + ")";
            }
            else if (patSex != "" && patAge == "0")
            {
                patName += "   (" + patSex + ")";
            }
            else if (patSex == "" && patAge != "0")
            {
                patName += "   (" + patAge + ")";
            }

            try
            {
                using (WebClient webc = new WebClient())
                {
                    NameValueCollection nvcol = new NameValueCollection();

                    nvcol.Add("patID", patID);
                    nvcol.Add("patName", patName);
                    nvcol.Add("copies", copies);

                    byte[] ResponseByte = webc.UploadValues(webaddress, nvcol);
                    response = Encoding.UTF8.GetString(ResponseByte);

                    webc.Dispose();

                    return "Label Print" + response + "\r\n";
                }
            }
            catch
            {
                //
            }

            return string.Empty;
        }
    }
}
