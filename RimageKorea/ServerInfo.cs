using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RimageKorea
{
    /// <summary>
    /// 서버 정보
    /// </summary>
    [Serializable]
    public class ServerInfo
    {
        /// <summary>
        /// 번호
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 서버명
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 아이피
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 포트
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// 얕은 카피
        /// </summary>
        /// <returns></returns>
        public ServerInfo ShallowCopy()
        {
            return this.MemberwiseClone() as ServerInfo;
        }
    }
}
