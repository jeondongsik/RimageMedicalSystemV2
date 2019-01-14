using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

namespace DicomReader
{
    /// <summary>
    /// DicomDir 정보.
    /// </summary>
    public class DicomInfomation
    {
        DicomDecoder dd;
        string dicomFilePath;
        string dicomFileName;
        string errMessage;
        string errCode;
        Dictionary<string, string> patientInfo;
        //DataTable dtStudy;
        //DataTable dtSeries;
        DataTable dtImages;
        bool blID = false;
        bool blName = false;
        bool blSex = false;
        bool blBirthDay = false;
        bool blImagePath = false;
        //bool blModality = false;
        bool blAge = false;
        bool blImageNumber = false;
        Dictionary<string, string> patientList;
        string lstPatID;
        string lstPatName;
        int patientCount = 0;        
        
        #region Property
        /// <summary>
        /// DicomDir or Dicom.dcm 파일 FullPath
        /// </summary>
        public string _DicomFilePath
        {
            get { return dicomFilePath; }
            set { dicomFilePath = value; }
        }
        /// <summary>
        /// DicomDir or Dicom.dcm 파일명(확장자 포함)
        /// </summary>
        public string _DicomFileName
        {
            get { return dicomFileName; }
            set { dicomFileName = value; }
        }
        /// <summary>
        /// 에러메시지 리턴
        /// </summary>
        public string _ErrMessage
        {
            get { return errMessage; }
            set { errMessage = value; }
        }
        /// <summary>
        /// 에러코드
        /// </summary>
        public string _ErrCode
        {
            get { return errCode; }
            set { errCode = value; }
        }
        /// <summary>
        /// 환자목록
        /// </summary>
        public Dictionary<string, string> PatientLST
        {
            get { return this.patientList; }
            set { this.patientList = value; }
        }
        /// <summary>
        /// 환자수
        /// </summary>
        public int PatientCount
        {
            get { return this.patientCount; }
            set { this.patientCount = value; }
        }        
        #endregion

        #region 생성자
        public DicomInfomation()
        {
            dd = new DicomDecoder();

            if (this.patientInfo == null)
                this.patientInfo = new Dictionary<string, string>();
            else
                this.patientInfo.Clear();

            this.patientList = new Dictionary<string, string>();

            this.InitPatientInfo();
        }
        #endregion

        #region 환자정보 Dictionay 초기화.
        /// <summary>
        /// 환자정보 초기화.
        /// </summary>
        private void InitPatientInfo()
        {
            this.patientInfo.Add("ID", "");
            this.patientInfo.Add("Name", "");
            this.patientInfo.Add("Sex", "");
            this.patientInfo.Add("SexKr", "");
            this.patientInfo.Add("BirthDay", "");
            this.patientInfo.Add("Age", "");
            this.patientInfo.Add("StudyDesc", "");
            this.patientInfo.Add("Modality", "");
            this.patientInfo.Add("ImageCount", "");
            this.patientInfo.Add("ImagePath", "");
            this.patientInfo.Add("DicomDesc", "");
        }
        #endregion

        /// <summary>
        /// Study, Series, Images를 담을 테이블 초기화.
        /// </summary>
        private void InitPatientData()
        {
            //this.dtStudy = new DataTable();
            //dtStudy.TableName = "Study";
            //dtStudy.Columns.Add("ID", typeof(string));
            //dtStudy.Columns.Add("StudyID", typeof(string));
            //dtStudy.Columns.Add("StudyDate", typeof(string));
            //dtStudy.Columns.Add("StudyTime", typeof(string));
            //dtStudy.Columns.Add("StudyModality", typeof(string));
            //dtStudy.Columns.Add("StudyDesc", typeof(string));

            //this.dtSeries = new DataTable();
            //dtSeries.TableName = "Series";
            //dtSeries.Columns.Add("ID", typeof(string));
            //dtSeries.Columns.Add("StudyID", typeof(string));
            //dtSeries.Columns.Add("StudyModality", typeof(string));
            //dtSeries.Columns.Add("StudyDesc", typeof(string));
            //dtSeries.Columns.Add("SeriesNumber", typeof(string));
            //dtSeries.Columns.Add("SeriesModality", typeof(string));

            this.dtImages = new DataTable();
            dtImages.TableName = "Images";
            dtImages.Columns.Add("ID", typeof(string));
            dtImages.Columns.Add("StudyID", typeof(string));
            dtImages.Columns.Add("StudyModality", typeof(string));
            dtImages.Columns.Add("StudyDesc", typeof(string));
            dtImages.Columns.Add("StudyDate", typeof(string));
            dtImages.Columns.Add("StudyTime", typeof(string));
            dtImages.Columns.Add("SeriesNumber", typeof(string));
            dtImages.Columns.Add("SeriesModality", typeof(string));
            dtImages.Columns.Add("imageNumber", typeof(string));
            dtImages.Columns.Add("imagePath", typeof(string));
        }

        /// <summary>
        /// DicomDir 정보 문자열
        /// </summary>
        /// <returns></returns>
        public List<string> GetDicomDirString()
        {
            try
            {
                //DicomDir 파일정보
                this.ReadDicomFile();
                return dd.dicomInfo;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 환자정보 리턴
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> PatientInfo()
        {
            try
            {
                //DicomDir 파일정보
                this.ReadDicomFile();
                List<string> DicomDirStr = dd.dicomInfo;

                this.lstPatID = string.Empty;
                this.lstPatName = string.Empty;

                this.SetString(DicomDirStr);

                //DicomDir에서 정보를 제대로 가져오지 못했을 때 사진에서..
                if (this.blID == false || this.blName == false || this.blBirthDay == false || this.blSex == false)
                {
                    if (this.patientInfo["ImagePath"] != "")
                    {
                        string imagePath = this.dicomFilePath.Substring(0, this.dicomFilePath.LastIndexOf("\\") + 1);

                        //DicomDCM 파일정보 : DicomDir 정보에서 첫번째 이미지파일 경로를 가져온다.
                        this.dicomFilePath = imagePath + this.patientInfo["ImagePath"].Trim();
                        this.dicomFileName = this.dicomFilePath.Substring(this.dicomFilePath.LastIndexOf("\\") + 1);

                        if (File.Exists(this.dicomFilePath))
                        {
                            this.ReadDicomFile();
                            List<string> DicomDCMStr = dd.dicomInfo;

                            this.SetString(DicomDCMStr);
                        }
                    }
                }

                //최종적으로 정보가 없을 시 null 리턴한다.
                if (this.blID == false || this.blName == false)
                    return null;

                if (string.IsNullOrEmpty(this.patientInfo["Age"]) == true)
                {
                    int age = this.getPatientAge(this.patientInfo["BirthDay"], this.patientInfo["ID"]);
                    this.patientInfo["Age"] = (age.Equals(0)) ? string.Empty : age.ToString();
                }

                if (this.patientInfo["Sex"] == "F")
                    this.patientInfo["SexKr"] = "여";
                else if (this.patientInfo["Sex"] == "M")
                    this.patientInfo["SexKr"] = "남";
                else
                    this.patientInfo["SexKr"] = this.patientInfo["Sex"];

                this.SetPatientList();
            }
            catch 
            {
                return null;
            }

            return this.patientInfo;
        }

        /// <summary>
        /// 이미지 파일에서 정보 리턴
        /// </summary>
        /// <param name="imgName"></param>
        /// <returns></returns>
        public Dictionary<string, string> PatientInfoFromDCM(string imgName)
        {
            try
            {
                this.lstPatID = string.Empty;
                this.lstPatName = string.Empty;

                string imagePath = this.dicomFilePath.Substring(0, this.dicomFilePath.LastIndexOf("\\") + 1);

                //DicomDCM 파일정보 : DicomDir 정보에서 첫번째 이미지파일 경로를 가져온다.
                this.dicomFilePath = imagePath + imgName.Trim();
                this.dicomFileName = this.dicomFilePath.Substring(this.dicomFilePath.LastIndexOf("\\") + 1);

                if (File.Exists(this.dicomFilePath))
                {
                    this.ReadDicomFile();
                    List<string> DicomDCMStr = dd.dicomInfo;

                    this.SetString(DicomDCMStr);

                    //최종적으로 정보가 없을 시 null 리턴한다.
                    if (this.blID == false || this.blName == false)
                        return null;

                    this.patientInfo["Age"] = this.getPatientAge(this.patientInfo["BirthDay"], this.patientInfo["ID"]).ToString();

                    if (this.patientInfo["Sex"] == "F")
                        this.patientInfo["SexKr"] = "여";
                    else if (this.patientInfo["Sex"] == "M")
                        this.patientInfo["SexKr"] = "남";
                    else
                        this.patientInfo["SexKr"] = this.patientInfo["Sex"];
                }
            }
            catch
            {
                return null;
            }

            return this.patientInfo;
        }

        /// <summary>
        /// Dicomdir 문서 내용을 읽는다.(환자정보만..)
        /// </summary>
        /// <param name="strg"></param>
        private void SetString(List<string> strg)
        {
            try
            {
                List<string> str = strg;
                string s1, s2, s3, s4, s5, s11, s12;
                string Modality = string.Empty;
                string StudyDesc = string.Empty;
                string StudyDate = string.Empty;
                string nowType = string.Empty;
                int ind;
                int imgCount = 0;
                string nowID = string.Empty;
                string nowName = string.Empty;
                string dicomDesc = string.Empty;
                string studyTag = string.Empty;
                string studyModality = string.Empty;

                // Add items to the List View Control
                for (int i = 0; i < str.Count; ++i)
                {
                    s1 = str[i];
                    ind = s1.IndexOf("//");
                    s2 = s1.Substring(0, ind);

                    s11 = s1.Substring(0, 4);
                    s12 = s1.Substring(4, 4);

                    s3 = s1.Substring(ind + 2);
                    ind = s3.IndexOf(":");
                    s4 = s3.Substring(0, ind);
                    s5 = s3.Substring(ind + 1);

                    //Type
                    if (s11.Trim() == "0004" && s12.Trim() == "1430")
                    {
                        nowType = s5.Replace("\0", "").Trim();
                    }

                    //ID
                    if (s11.Trim() == "0010" && s12.Trim() == "0020")
                    {
                        string patID = this.ReplacePatientName(s5);

                        if (this.blID == false)
                        {
                            this.patientInfo["ID"] = patID;
                            this.blID = true;
                        }

                        this.lstPatID += patID + "^";

                        if (nowID == "" || nowID != patID)
                            dicomDesc += "Patient ID: " + patID + "^^";

                        nowID = patID;

                    }
                    //Name
                    if (s11.Trim() == "0010" && s12.Trim() == "0010")
                    {
                        string patName = this.ReplacePatientName(s5);

                        if (this.blName == false)
                        {
                            this.patientInfo["Name"] = patName;
                            this.blName = true;
                        }

                        this.lstPatName += patName + "^";

                        if (nowName == "" || nowName != patName)
                            dicomDesc += "Patient's Name: " + patName + "^^";

                        nowName = patName;
                    }

                    //Sex
                    if (this.blSex == false && s11.Trim() == "0010" && s12.Trim() == "0040")
                    {
                        this.patientInfo["Sex"] = s5.Replace("\0", "").Trim();
                        this.blSex = true;

                        //WriteString(s1);
                    }
                    //BirthDay
                    if (this.blBirthDay == false && s11.Trim() == "0010" && s12.Trim() == "0030")
                    {
                        this.patientInfo["BirthDay"] = s5.Replace("\0", "").Trim();
                        this.blBirthDay = true;

                        //WriteString(s1);
                    }
                    //Age
                    if (this.blAge == false && s11.Trim() == "0010" && s12.Trim() == "1010")
                    {
                        this.patientInfo["Age"] = s5.Replace("\0", "").Trim();
                        this.blAge = true;
                    }

                    //Study Modality
                    if (s11.Trim() == "0008" && s12.Trim() == "0061")
                    {
                        s5 = s5.Replace("\0", "").Replace("\r\n", "");

                        studyTag += s5.Trim() + "^^";
                    }

                    //StudyDesc
                    if (s11.Trim() == "0008" && s12.Trim() == "1030")
                    {
                        s5 = s5.Replace("\0", "").Replace("\r\n", "");

                        StudyDesc += s5.Trim() + "^^";
                        dicomDesc += "Study Description: " + s5.Trim() + "^^";
                    }

                    //StudyDate
                    if (s11.Trim() == "0008" && s12.Trim() == "0020")
                    {
                        s5 = s5.Replace("\0", "").Replace("\r\n", "");

                        StudyDate += s5.Trim() + "^^";
                        dicomDesc += "Study Date: " + s5.Trim() + "^^";
                    }

                    //Modality
                    if (s11.Trim() == "0008" && s12.Trim() == "0060")
                    {
                        s5 = s5.Replace("\0", "").Replace("\r\n", "");

                        Modality += s5.Trim() + "^^";
                    }
                    //Series

                    //ImageNumber
                    if (s11.Trim() == "0004" && s12.Trim() == "1430" && s5.ToLower().Contains("image"))
                    {
                        //ImageNumber = s5.Replace("\0", "").Trim();
                        imgCount++;
                    }
                    //Image Path
                    if (this.blImagePath == false && s11.Trim() == "0004" && s12.Trim() == "1500")
                    {
                        this.patientInfo["ImagePath"] = s5;
                        this.blImagePath = true;
                    }
                }

                if (Modality.EndsWith("^^"))
                {
                    Modality = Modality.Substring(0, Modality.Length - 2);
                }
                this.patientInfo["Modality"] = Modality;


                //Study와 Modaliy를 매칭시킨다.
                studyModality = this.MatchingStudyModality(StudyDesc, studyTag);

                if (studyModality.EndsWith("^^"))
                {
                    studyModality = studyModality.Substring(0, studyModality.Length - 2);
                }
                this.patientInfo["StudyModality"] = StudyDesc;

                //Study 날짜와 매칭시킨다.
                StudyDesc = MatchingStudy(StudyDesc, StudyDate);
                
                if (StudyDesc.EndsWith("^^"))
                {
                    StudyDesc = StudyDesc.Substring(0, StudyDesc.Length - 2);
                }
                this.patientInfo["StudyDesc"] = StudyDesc;
                
                if (this.blImageNumber == false)
                {
                    this.patientInfo["ImageCount"] = imgCount.ToString();
                    this.blImageNumber = true;
                }

                if (dicomDesc.EndsWith("^^"))
                {
                    dicomDesc = dicomDesc.Substring(0, dicomDesc.Length - 2);
                }
                this.patientInfo["DicomDesc"] = dicomDesc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void WriteString(string str)
        {
            try
            {
                string path = @"D:\dicomDesc3.txt";
                // This text is added only once to the file.
                if (!File.Exists(path))
                {
                    File.Create(path);
                    // Create a file to write to.
                    //using (StreamWriter sw = File.CreateText(path))
                    //{
                    //    sw.WriteLine(str);
                    //}
                }

                using (StreamWriter stream = new StreamWriter(path, true))
                {
                    stream.WriteLine(str);
                }

            }
            catch { }
        }

        /// <summary>
        /// StudyDate와 StudyDesc 매칭시키기
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="dat"></param>
        /// <returns></returns>
        private string MatchingStudy(string desc, string dat)
        {
            string retStr = "";

            try
            {
                if (!dat.Contains("^^"))
                {
                    retStr = desc;
                }
                else
                {
                    if (!desc.EndsWith("^^"))
                        desc = desc + "^^";
                    if (!dat.EndsWith("^^"))
                        dat = dat + "^^";

                    string[] arrDesc = SplitByString(desc, "^^");
                    string[] arrDat = SplitByString(dat, "^^");

                    try
                    {
                        if (arrDesc.Length == arrDat.Length || arrDesc.Length < arrDat.Length)
                        {
                            for (int i = 0; i < arrDesc.Length; i++)
                            {
                                if (arrDesc[i] != "")
                                {
                                    retStr += "[" + (arrDat[i] == "" ? "00000000" : arrDat[i]) + "]" + arrDesc[i] + "^^";
                                }
                            }
                        }
                        else
                        {
                            int j = arrDat.Length - 1;

                            for (int i = 0; i < arrDesc.Length; i++)
                            {
                                if (arrDesc[i] != "")
                                {
                                    if (j > i)
                                    {
                                        retStr += "[" + (arrDat[i] == "" ? "00000000" : arrDat[i]) + "]" + arrDesc[i] + "^^";
                                    }
                                    else
                                    {
                                        retStr += "[00000000]" + arrDesc[i] + "^^";
                                    }
                                }
                            }
                        }

                    }
                    catch
                    {
                        retStr = desc;
                    }
                }
            }
            catch { }

            return retStr;
        }

        /// <summary>
        /// 스터디와 Modality Tag와 매핑시킨다.
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        private string MatchingStudyModality(string desc, string tag)
        {
            string retStr = "";

            try
            {
                if (!tag.Contains("^^"))
                {
                    retStr = desc;
                }
                else
                {
                    if (!desc.EndsWith("^^"))
                        desc = desc + "^^";
                    if (!tag.EndsWith("^^"))
                        tag = tag + "^^";

                    string[] arrDesc = SplitByString(desc, "^^");
                    string[] arrDat = SplitByString(tag, "^^");

                    try
                    {
                        if (arrDesc.Length == arrDat.Length || arrDesc.Length < arrDat.Length)
                        {
                            for (int i = 0; i < arrDesc.Length; i++)
                            {
                                if (arrDesc[i] != "")
                                {
                                    retStr += "[" + (arrDat[i] == "" ? "ZZ" : arrDat[i]) + "]" + arrDesc[i] + "^^";
                                }
                            }
                        }
                        else
                        {
                            int j = arrDat.Length - 1;

                            for (int i = 0; i < arrDesc.Length; i++)
                            {
                                if (arrDesc[i] != "")
                                {
                                    if (j > i)
                                    {
                                        retStr += "[" + (arrDat[i] == "" ? "ZZ" : arrDat[i]) + "]" + arrDesc[i] + "^^";
                                    }
                                    else
                                    {
                                        retStr += "[ZZ]" + arrDesc[i] + "^^";
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        retStr = desc;
                    }
                }
            }
            catch { }

            return retStr;
        }

        /// <summary>
        /// 문자열로 Split 시키기
        /// </summary>
        /// <param name="testString"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        private string[] SplitByString(string testString, string split)
        {
            int offset = 0;
            int index = 0;
            int[] offsets = new int[testString.Length + 1];
            while (index < testString.Length)
            {
                int indexOf = testString.IndexOf(split, index);
                if (indexOf != -1)
                {
                    offsets[offset++] = indexOf;
                    index = (indexOf + split.Length);
                }
                else
                {
                    index = testString.Length;
                }
            }
            string[] final = new string[offset + 1];
            if (offset == 0)
            {
                final[0] = testString;
            }
            else
            {
                offset--;
                final[0] = testString.Substring(0, offsets[0]);
                for (int i = 0; i < offset; i++)
                {
                    final[i + 1] = testString.Substring(offsets[i] + split.Length, offsets[i + 1] - offsets[i] - split.Length);
                }
                final[offset + 1] = testString.Substring(offsets[offset] + split.Length);
            }
            return final;
        }

        /// <summary>
        /// 환자 Study, Series, Image 정보 리턴
        /// </summary>
        /// <returns></returns>
        public DataTable PatientDicomInfo()
        {
            //DicomDir 파일정보
            this.ReadDicomFile();
            List<string> DicomDirStr = dd.dicomInfo;

            this.InitPatientData();
            this.SetData(DicomDirStr);
            
            return dtImages;
        }

        /// <summary>
        /// Dicomdir 문서 내용을 읽는다.(Study, Series, Image를 DataTable에 모두 담는다)
        /// </summary>
        /// <param name="strg"></param>
        private bool SetData(List<string> strg)
        {
            bool retVal = false;

            try
            {
                List<string> str = strg;
                string s1, s2, s3, s4, s5, s11, s12;
                string nowType = "";
                int ind;
                int id = 0;
                string StudyID = "", StudyDate = "", StudyTime = "", StudyModality = "", StudyDesc = "";
                string SeriesNumber = "", SeriesModality = "";
                string imageNumber = "", imagePath = "";

                // Add items to the List View Control
                for (int i = 0; i < str.Count; ++i)
                {
                    s1 = str[i];
                    ind = s1.IndexOf("//");
                    s2 = s1.Substring(0, ind);

                    s11 = s1.Substring(0, 4);
                    s12 = s1.Substring(4, 4);

                    s3 = s1.Substring(ind + 2);
                    ind = s3.IndexOf(":");
                    s4 = s3.Substring(0, ind);
                    s5 = s3.Substring(ind + 1);

                    //Type
                    if (s11.Trim() == "0004" && s12.Trim() == "1430")
                    {
                        nowType = s5.Replace("\0", "").Trim();
                    }

                    if (nowType.ToLower().Equals("study"))
                    {
                        //Study Date
                        if (s11.Trim() == "0008" && s12.Trim() == "0020")
                        {
                            StudyDate = s5.Replace("\0", "").Trim();
                        }
                        //Study Time
                        if (s11.Trim() == "0008" && s12.Trim() == "0030")
                        {
                            StudyTime = s5.Replace("\0", "").Trim();
                        }
                        //Study Modality
                        if (s11.Trim() == "0008" && s12.Trim() == "0061")
                        {
                            StudyModality = s5.Replace("\0", "").Replace("\r\n", "");
                        }
                        //StudyDesc
                        if (s11.Trim() == "0008" && s12.Trim() == "1030")
                        {
                            StudyDesc = s5.Replace("\0", "").Replace("\r\n", "");
                        }
                        //StudyID
                        if (s11.Trim() == "0020" && s12.Trim() == "0010")
                        {
                            StudyID = s5.Replace("\0", "").Replace("\r\n", "");
                        }
                    }

                    if (nowType.ToLower().Equals("series"))
                    {
                        //Series Number
                        if (s11.Trim() == "0020" && s12.Trim() == "0011")
                        {
                            SeriesNumber = s5.Replace("\0", "").Trim();
                        }
                        //Series Modality
                        if (s11.Trim() == "0008" && s12.Trim() == "0060")
                        {
                            SeriesModality = s5.Replace("\0", "").Replace("\r\n", "");
                        }
                    }

                    if (nowType.ToLower().Equals("image"))
                    {
                        //ImageNumber
                        if (s11.Trim() == "0020" && s12.Trim() == "0013")
                        {
                            imageNumber = s5.Replace("\0", "").Trim();
                        }
                        //Image Path
                        if (s11.Trim() == "0004" && s12.Trim() == "1500")
                        {
                            imagePath = s5.Replace("\0", "");
                        }

                        if (imageNumber != "" & imagePath != "")
                        {
                            id++;
                            DataRow row = this.dtImages.NewRow();

                            row["ID"] = id.ToString();
                            row["StudyID"] = StudyID;
                            row["StudyModality"] = StudyModality;
                            row["StudyDesc"] = StudyDesc;
                            row["StudyDate"] = StudyDate;
                            row["StudyTime"] = StudyTime;
                            row["SeriesNumber"] = SeriesNumber;
                            row["SeriesModality"] = SeriesModality;
                            row["imageNumber"] = imageNumber;
                            row["imagePath"] = imagePath;

                            this.dtImages.Rows.Add(row);

                            imageNumber = "";
                            imagePath = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;
        }

        /// <summary>
        /// 이미지 파일 정보를 반환한다.
        /// </summary>
        /// <returns></returns>
        public DataTable PatientDicomImageInfo(int id)
        {
            string s1, s2, s3, s4, s5, s11, s12;
            int ind;
            string StudyID = "", StudyDate = "", StudyTime = "", StudyModality = "", StudyDesc = "";
            string SeriesNumber = "", SeriesModality = "";

            try
            {
                //DicomDir 파일정보
                this.ReadDicomFile();
                List<string> DicomDirStr = dd.dicomInfo;

                this.InitPatientData();

                DataRow row = this.dtImages.NewRow();
                row["ID"] = id.ToString();                

                for (int i = 0; i < DicomDirStr.Count; ++i)
                {
                    s1 = DicomDirStr[i];
                    ind = s1.IndexOf("//");
                    s2 = s1.Substring(0, ind);

                    s11 = s1.Substring(0, 4);
                    s12 = s1.Substring(4, 4);

                    s3 = s1.Substring(ind + 2);
                    ind = s3.IndexOf(":");
                    s4 = s3.Substring(0, ind);
                    s5 = s3.Substring(ind + 1);
                    
                    if (s11.Trim() == "0008" && s12.Trim() == "0020")
                    {
                        StudyDate = s5.Replace("\0", "").Trim();
                        row["StudyDate"] = StudyDate;
                    }
                    //Study Time
                    if (s11.Trim() == "0008" && s12.Trim() == "0030")
                    {
                        StudyTime = s5.Replace("\0", "").Trim();
                        row["StudyTime"] = StudyTime;
                    }
                    //Study Modality
                    if (s11.Trim() == "0008" && s12.Trim() == "0061")
                    {
                        StudyModality = s5.Replace("\0", "").Replace("\r\n", "");
                        row["StudyModality"] = StudyModality;
                    }
                    //StudyDesc
                    if (s11.Trim() == "0008" && s12.Trim() == "1030")
                    {
                        StudyDesc = s5.Replace("\0", "").Replace("\r\n", "");
                        row["StudyDesc"] = StudyDesc;
                    }
                    //StudyID
                    if (s11.Trim() == "0020" && s12.Trim() == "0010")
                    {
                        StudyID = s5.Replace("\0", "").Replace("\r\n", "");
                        row["StudyID"] = StudyID;
                    }

                    //Series Number
                    if (s11.Trim() == "0020" && s12.Trim() == "0011")
                    {
                        SeriesNumber = s5.Replace("\0", "").Trim();
                        row["SeriesNumber"] = SeriesNumber;
                    }
                    //Series Modality
                    if (s11.Trim() == "0008" && s12.Trim() == "0060")
                    {
                        SeriesModality = s5.Replace("\0", "").Replace("\r\n", "");
                        row["SeriesModality"] = SeriesModality;
                    }
                }

                this.dtImages.Rows.Add(row);

            }
            catch
            {
                return null;
            }

            return dtImages;
        }

        /// <summary>
        /// 파일 오픈
        /// </summary>
        private void ReadDicomFile()
        {
            string fileName = this.dicomFilePath;
            string fileNameOnly = this.dicomFileName;

            dd.DicomFileName = fileName;
            bool result = dd.dicomFileReadSuccess;

            if (result == true)
            {
                //정상오픈
                this.errCode = "OK";
                this.errMessage = "Sucesses.";
            }
            else
            {
                if (dd.dicmFound == false)
                {
                    this.errCode = "Fail";
                    this.errMessage = "This does not seem to be a DICOM 3.0 file. Sorry, I can't open this.";
                }
                else if (dd.dicomDir == true)
                {
                    this.errCode = "OK";
                    this.errMessage = "This seems to be a DICOMDIR file, and does not contain an image.";
                }
                else
                {
                    this.errCode = "OK";
                    this.errMessage = "Sorry, I can't read a DICOM file with this Transfer Syntax\n" +
                        "You may view the initial tags instead.";
                }
            }
        }

        /// <summary>
        /// 환자 추가
        /// </summary>
        /// <param name="patID"></param>
        /// <param name="patName"></param>
        private void SetPatientList()
        {
            try
            {
                if (this.lstPatID.Contains("^") && this.lstPatName.Contains("^"))
                {
                    string[] arrPatID = this.lstPatID.Split('^');
                    string[] arrPatNM = this.lstPatName.Split('^');

                    for (int i = 0; i < arrPatID.Length; i++)
                    {
                        if (string.IsNullOrEmpty(arrPatID[i]) == false && this.patientList.ContainsKey(arrPatID[i]) == false)
                            this.patientList.Add(arrPatID[i], arrPatNM[i]);
                    }

                    this.patientCount = this.patientList.Count;
                }
                else
                {
                    this.patientCount = 0;
                }
            }
            catch { }
        }

        #region 나이계산 (만나이)
        public int getPatientAge(string patBirthDay, string patID)
        {
            int result = 0;
            int c1 = 0;

            try
            {
                if (patID.Contains("UNKNOWN") || patID == "")
                {
                    result = 0;
                }
                else
                {
                    patBirthDay = patBirthDay.Replace("-", "").Replace(".", "").Replace("/", "").Replace("_", "").Replace(" ", "");

                    if (patBirthDay.Length > 3)
                    {
                        c1 = DateTime.Now.Year - Convert.ToInt16(patBirthDay.Substring(0, 4));
                    }

                    if (patBirthDay.Length > 7)
                    {
                        int mm = Convert.ToInt32(patBirthDay.Substring(4, 2));
                        int dd = Convert.ToInt32(patBirthDay.Substring(6, 2));

                        if (mm < 13 && dd < 32)
                        {
                            DateTime chgNow = new DateTime(DateTime.Now.Year, Convert.ToInt32(patBirthDay.Substring(4, 2)), Convert.ToInt32(patBirthDay.Substring(6, 2)));

                            if (DateTime.Now.Date.CompareTo(chgNow) == -1)
                                result = c1 - 1;
                            else
                                result = c1;
                        }
                    }
                    else
                    {
                        result = c1;
                    }
                }
            }
            catch
            {
                result = 0;
                //
            }

            return result;
        }
        #endregion

        /// <summary>
        /// 환자명 쓸데없는 문자 없애기
        /// </summary>
        /// <param name="ogrName"></param>
        /// <returns></returns>
        public string ReplacePatientName(string ogrName)
        {
            string returnStr = "";

            returnStr = ogrName.Replace((char)27, ' ');

            if (returnStr.Contains("="))
            {
                string[] arrStr = ogrName.Split('=');
                returnStr = arrStr[0];
            }

            returnStr = returnStr.Replace("\0", "").Replace("^", "").Replace("\r\n", "").Trim();
            returnStr = returnStr.Replace("\t", "").Replace("$", "").Replace("\b", "");

            return returnStr;
        }
    }
}
