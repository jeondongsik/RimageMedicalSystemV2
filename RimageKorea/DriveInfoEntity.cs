using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RimageKorea
{
    public class DriveInfoEntity
    {
        public string Name { get; set; }
        public long AvailableFreeSpace { get; set; }
        public long TotalFreeSpace { get; set; }
        public long TotalSize { get; set; }
    }
}
