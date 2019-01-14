using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RimageKorea
{
    /// <summary>
    /// 굽기 결과
    /// </summary>
    public class MediaDiscResult
    {
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string BurningDateTime { get; set; }

        IList<PatientRecordInfo> RecordInfo { get; set; }
    }
}
