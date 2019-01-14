using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Management;

namespace RimageKorea
{
    public class CheckLicense
    {
        int checkSum = 777;
        string lic;
        string ResultStr = "";

        public string Result
        {
            get { return ResultStr; }
            set { ResultStr = value; }
        }

        public string License
        {
            get { return lic; }
            set { lic = value; }
        }

        public CheckLicense()
        {

        }

        public bool validateKeygen()
        {
            bool result = false;

            if (validateLic() && validateMacAddress())
            {
                result = true;
            }
            else
            {
                if (!validateLic())
                {
                    ResultStr = "잘못된 라이센스입니다.";
                }
                if (!validateMacAddress())
                {
                    ResultStr = "설치된 컴퓨터의 Mac Address와 일치하지 않습니다.";
                }
            }

            return result;
        }

        private bool validateLic()
        {
            bool result = false;
            string desDe = DESHasher.DESDecrypt(lic);

            if (desDe.Length == 27)
            {
                string n1 = desDe.Substring(0, 2);
                string n2 = desDe.Substring(10, 2);

                string checkSumStr = desDe.Substring(6, 3);
                int checkSumInt = Convert.ToInt32(checkSumStr);

                int re = Convert.ToInt32(n1) + Convert.ToInt32(n2) + checkSumInt;

                if (re.Equals(checkSum))
                {
                    result = true;
                }
            }

            return result;
        }

        private bool validateMacAddress()
        {
            bool result = false;
            string desDe = DESHasher.DESDecrypt(lic);

            if (desDe.Length == 27)
            {
                string mac = desDe.Substring(15, 12);

                ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled='TRUE'");
                ManagementObjectCollection queryCol = query.Get();
                foreach (ManagementObject mo in queryCol)
                {
                    if (mo["MACAddress"].ToString().Replace(":", "").Equals(mac))
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
