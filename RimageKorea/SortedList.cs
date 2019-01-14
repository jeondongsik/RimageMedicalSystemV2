using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RimageKorea
{
    public class SortList
    {
        /// <summary>
        /// 순번
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// 문자열
        /// </summary>
        public string ValueString { get; set; }

        public SortList(int seq, string str)
        {
            this.Sequence = seq;
            this.ValueString = str;
        }
    }
}
