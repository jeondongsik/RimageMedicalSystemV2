using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace RimageKorea
{
    public class CheckPing
    {
        public static bool TestPing(string ip)
        {
            bool result = false;
            Ping pingSender = new Ping();
            PingOptions po = new PingOptions();

            po.DontFragment = true;

            int timeout = 5000;
            PingReply reply = pingSender.Send(ip, timeout);

            pingSender.Dispose();

            if (reply.Status == IPStatus.Success)
            {
                result = true;
            }

            return result;
        }
    }
}
