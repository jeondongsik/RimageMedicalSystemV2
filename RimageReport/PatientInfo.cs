using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RimageReport
{
    public class PatientInfo
    {
        public string StudyDate { get; set; }
        public string StudyDesc { get; set; }
        public string StudyDate2 { get; set; }
        public string StudyDesc2 { get; set; }

        public PatientInfo(string studyDate, string studyDesc)
        {
            this.StudyDate = studyDate;
            this.StudyDesc = studyDesc;
            this.StudyDate2 = "";
            this.StudyDesc2 = "";
        }
    }
}
