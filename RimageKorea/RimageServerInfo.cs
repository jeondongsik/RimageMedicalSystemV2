using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RimageKorea
{
    /// <summary>
    /// 리마지 서버 정보
    /// </summary>
    public class RimageServerInfo
    {
        public string No { get; set; }
        public string ConnID { get; set; }
        public string ClientID { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// UncSystemFolder
        /// </summary>
        public string UncSystemFolder { get; set; }

        public RimageServerInfo ShallowCopy()
        {
            return this.MemberwiseClone() as RimageServerInfo;
        }

        public RimageServerInfo DeepCopy()
        {
            RimageServerInfo other = (RimageServerInfo)this.MemberwiseClone();
            other.No = String.Copy(No);
            other.ConnID = String.Copy(ConnID);
            other.ClientID = String.Copy(ClientID);
            other.Host = String.Copy(Host);
            other.Port = String.Copy(Port);
            other.Name = String.Copy(Name);
            other.UncSystemFolder = String.Copy(UncSystemFolder);

            return other;
        }
    }
}
