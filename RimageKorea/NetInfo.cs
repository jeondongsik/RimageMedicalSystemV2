using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace RimageKorea
{
    public class NetInfo
    {
        public NetInfo()
        {
        }

        /// <summary>
        /// 내 컴퓨터 IP 가져오기
        /// </summary>
        /// <returns></returns>
        public static string MyIP()
        {
            string ip = "";

            IPHostEntry IPHost = Dns.GetHostEntry(Dns.GetHostName().ToString());

            foreach (IPAddress ipAddr in IPHost.AddressList)
            {
                if (valiIP(ipAddr.ToString()))
                {
                    ip = ipAddr.ToString();
                    break;
                }
            }

            return ip;
        }

        /// <summary>
        /// 내 컴퓨터 Host Name 가져오기
        /// </summary>
        /// <returns></returns>
        public static string MyHostName()
        {
            return Dns.GetHostName().ToString();
        }

        /// <summary>
        /// IP (4P) 유형 체크
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool valiIP(string str)
        {
            bool retStr = false;

            MatchCollection matches = Regex.Matches(str, @"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}");

            if (matches.Count > 0)
            {
                retStr = true;
            }
            
            return retStr;
        }
    }
}
