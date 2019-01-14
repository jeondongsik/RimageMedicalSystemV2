using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RimageKorea
{
    /// <summary>
    /// 굽기 진행 상태/결과 Entity
    /// </summary>
    public class DiscStatusForDisplay
    {
        public string ProcessID { get; set; }
        public string OrderID { get; set; }
        public string ServerIP { get; set; }
        public string Status { get; set; }
        public string StatusType { get; set; }
        public string Stage { get; set; }
        public string State { get; set; }
        public string StateString { get; set; }
        public string PercentCompleted { get; set; }
        public string DeviceCurrentState { get; set; }
        public string ResultMessage { get; set; }
        public string ResultCode { get; set; }
        public string SortIdx { get; set; }
        public string Finish { get; set; }
    }
}
