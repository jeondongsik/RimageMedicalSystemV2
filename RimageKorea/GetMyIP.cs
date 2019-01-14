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
    public class GetMyIP
    {
        public GetMyIP()
        {
        }

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

        public static string MyHostName()
        {
            return Dns.GetHostName().ToString();
        }

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
