using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

using EvilDICOM.Core;
using EvilDICOM.Core.Interfaces;

using DicomReader;

namespace RimageKorea
{
    public class DicomDirReader
    {
        /// <summary>
        /// DicomDir 정보 가져오기 - EvilDicom
        /// </summary>
        /// <param name="dicomDirPath"></param>
        /// <returns></returns>
        public static PatientList GetPatientrFromEvil(string dicomDirPath)
        {
            PatientList returnValue = null;
            List<PatientInfoEntity> patList = null;
            Dictionary<string, string> patientList = null;
            string dicomDesc = string.Empty;

            try
            {
                if (File.Exists(dicomDirPath) == true)
                {
                    int patNo = 0;
                    int studyNo = 0;
                    int seriesNo = 0;
                    int imgNo = 0;

                    patList = new List<PatientInfoEntity>();

                    DICOMObject dicomDir = DICOMObject.Read(dicomDirPath);

                    string RecordType = string.Empty;
                    string StudyModality = string.Empty;
                    string StudyDate = string.Empty;
                    string StudyTime = string.Empty;
                    string StudyDesc = string.Empty;
                    string SeriesDate = string.Empty;
                    string SeriesTime = string.Empty;
                    string Modality = string.Empty;
                    string BodyPart = string.Empty;

                    PatientInfoEntity patInfo = null;

                    foreach (IDICOMElement dce in dicomDir.AllElements)
                    {
                        if (dce.Tag.CompleteID.Equals("00041430") == true)
                            RecordType = dce.DData.ToString();

                        //첫번째 DirectoryRecordType : PATIENT
                        if (dce.Tag.CompleteID.Equals("00041430") == true && dce.DData.ToString() == "PATIENT")
                        {
                            if (patNo > 0 && patInfo != null)
                                patList.Add(patInfo);

                            patNo++;

                            patInfo = new PatientInfoEntity();
                            patInfo.listNumber = patNo;
                            patInfo.Patient = new PatientBaseInfo();
                            patInfo.RecordList = new List<PatientRecordInfo>();
                        }

                        if (RecordType == "PATIENT")
                        {
                            //환자명
                            if (dce.Tag.CompleteID.Equals("00100010") == true)
                                patInfo.Patient.Name = Utils.ReplacePatientName(dce.DData.ToString());
                            //환자아이디
                            if (dce.Tag.CompleteID.Equals("00100020") == true)
                                patInfo.Patient.ID = Utils.ReplaceSpecialWord(dce.DData.ToString());
                            //환자생일
                            if (dce.Tag.CompleteID.Equals("00100030") == true)
                                patInfo.Patient.BirthDate = dce.DData.ToString();
                            //성별
                            if (dce.Tag.CompleteID.Equals("00100040") == true)
                                patInfo.Patient.Gender = dce.DData.ToString();
                            //나이
                            if (dce.Tag.CompleteID.Equals("00101010") == true)
                                patInfo.Patient.Age = dce.DData.ToString();
                        }

                        if (dce.Tag.CompleteID.Equals("00041430") == true && dce.DData.ToString() == "STUDY")
                        {
                            studyNo++;
                        }

                        if (RecordType == "STUDY")
                        {
                            if (dce.Tag.CompleteID.Equals("00080061"))
                                StudyModality = dce.DData.ToString();
                            if (dce.Tag.CompleteID.Equals("00080020") == true)
                                StudyDate = dce.DData.ToString();
                            if (dce.Tag.CompleteID.Equals("00080030") == true)
                                StudyTime = dce.DData.ToString();
                            if (dce.Tag.CompleteID.Equals("00081030") == true)
                                StudyDesc = dce.DData.ToString();

                            if (string.IsNullOrEmpty(patInfo.Patient.BirthDate))
                            {
                                if (dce.Tag.CompleteID.Equals("00100030"))
                                    patInfo.Patient.BirthDate = dce.DData.ToString();
                            }
                        }

                        if (dce.Tag.CompleteID.Equals("00041430") == true && dce.DData.ToString() == "SERIES")
                        {
                            seriesNo++;
                        }

                        //현재상태가 SERIES 일때
                        if (RecordType == "SERIES")
                        {
                            if (dce.Tag.CompleteID.Equals("00080021") == true)
                                SeriesDate = dce.DData.ToString();
                            if (dce.Tag.CompleteID.Equals("00080031") == true)
                                SeriesTime = dce.DData.ToString();
                            if (dce.Tag.CompleteID.Equals("00080060") == true)
                                Modality = dce.DData.ToString();
                            if (dce.Tag.CompleteID.Equals("00180015") == true)
                                BodyPart = dce.DData.ToString();
                        }

                        if (RecordType == "IMAGE" && dce.Tag.CompleteID.Equals("00041500") == true)
                        {
                            imgNo++;

                            PatientRecordInfo recordInfo = new PatientRecordInfo();
                            recordInfo.PatID = patInfo.Patient.ID;

                            recordInfo.StudyNumber = studyNo.ToString().PadLeft(4, '0');
                            recordInfo.StudyDate = StudyDate;
                            recordInfo.StudyTime = StudyTime;
                            recordInfo.StudyDesc = StudyDesc;
                            recordInfo.StudyModality = StudyModality;

                            recordInfo.SeriesNumber = seriesNo.ToString().PadLeft(4, '0');
                            recordInfo.SeriesDate = SeriesDate;
                            recordInfo.SeriesTime = SeriesTime;
                            recordInfo.Modality = Modality;
                            recordInfo.BodyPart = BodyPart;

                            recordInfo.ImageFilePath = dce.DData.ToString();
                            recordInfo.ImageNumber = imgNo.ToString();

                            patInfo.RecordList.Add(recordInfo);
                        }
                    }

                    if (patInfo != null)
                        patList.Add(patInfo);
                }

                returnValue = new PatientList();
                patientList = new Dictionary<string, string>();

                List<PatientInfoEntity> newPatList = PatientConcat(patList);

                DicomDirReader.SetStudyModalityConcat(ref newPatList, ref patientList, ref dicomDesc);

                if (dicomDesc.EndsWith("^^"))
                {
                    dicomDesc = dicomDesc.Substring(0, dicomDesc.Length - 2);
                }

                returnValue.Base = newPatList;
                returnValue.SimpleList = patientList;
                returnValue.AllDesc = dicomDesc;
            }
            catch { }

            return returnValue;
        }

        /// <summary>
        /// DicomDir 정보 가져오기 - AS-IS
        /// </summary>
        /// <param name="dicomDirPath"></param>
        /// <returns></returns>
        public static PatientList GetPatientrFromDicomNet(string dicomDirPath)
        {
            PatientList returnValue = null;
            List<PatientInfoEntity> patList = null;
            Dictionary<string, string> patientList = null;
            string dicomDesc = string.Empty;

            try
            {
                if (File.Exists(dicomDirPath) == true)
                {
                    DicomInfomation di = new DicomInfomation();
                    di._DicomFilePath = dicomDirPath;
                    di._DicomFileName = "DICOMDIR";

                    List<string> dicomDirStr = di.GetDicomDirString();

                    int patNo = 0;
                    int studyNo = 0;
                    int seriesNo = 0;
                    int imgNo = 0;

                    patList = new List<PatientInfoEntity>();

                    string RecordType = string.Empty;
                    string StudyModality = string.Empty;
                    string StudyDate = string.Empty;
                    string StudyTime = string.Empty;
                    string StudyDesc = string.Empty;
                    string SeriesDate = string.Empty;
                    string SeriesTime = string.Empty;
                    string Modality = string.Empty;
                    string BodyPart = string.Empty;

                    PatientInfoEntity patInfo = null;

                    string s1, s2, s3, s4, s5, s11, s12;
                    string CompleteID;
                    string DataStr;
                    int ind;
                    
                    foreach(string str in dicomDirStr)
                    {
                        s1 = str;
                        ind = s1.IndexOf("//");
                        s2 = s1.Substring(0, ind);

                        s11 = s1.Substring(0, 4);
                        s12 = s1.Substring(4, 4);

                        s3 = s1.Substring(ind + 2);
                        ind = s3.IndexOf(":");
                        s4 = s3.Substring(0, ind);
                        s5 = s3.Substring(ind + 1);

                        CompleteID = s11.Trim() + s12.Trim();
                        DataStr = s5.Replace("\0", "").Trim();

                        if (CompleteID.Equals("00041430") == true)
                            RecordType = DataStr;

                        //첫번째 DirectoryRecordType : PATIENT
                        if (CompleteID.Equals("00041430") == true && DataStr == "PATIENT")
                        {
                            if (patNo > 0 && patInfo != null)
                                patList.Add(patInfo);

                            patNo++;

                            patInfo = new PatientInfoEntity();
                            patInfo.listNumber = patNo;
                            patInfo.Patient = new PatientBaseInfo();
                            patInfo.RecordList = new List<PatientRecordInfo>();
                        }

                        if (RecordType == "PATIENT")
                        {
                            //환자명
                            if (CompleteID.Equals("00100010"))
                                patInfo.Patient.Name = Utils.ReplacePatientName(DataStr);
                            //환자아이디
                            if (CompleteID.Equals("00100020"))
                                patInfo.Patient.ID = Utils.ReplaceSpecialWord(DataStr);
                            //환자생일
                            if (CompleteID.Equals("00100030"))
                                patInfo.Patient.BirthDate = DataStr;
                            //성별
                            if (CompleteID.Equals("00100040"))
                                patInfo.Patient.Gender = DataStr;
                            //나이
                            if (CompleteID.Equals("00101010"))
                                patInfo.Patient.Age = DataStr;
                        }

                        if (CompleteID.Equals("00041430") && DataStr == "STUDY")
                        {
                            studyNo++;
                        }

                        if (RecordType == "STUDY")
                        {
                            if (CompleteID.Equals("00080061"))
                                StudyModality = DataStr;
                            if (CompleteID.Equals("00080020"))
                                StudyDate = DataStr;
                            if (CompleteID.Equals("00080030"))
                                StudyTime = DataStr;
                            if (CompleteID.Equals("00081030"))
                                StudyDesc = DataStr;

                            if (string.IsNullOrEmpty(patInfo.Patient.BirthDate))
                            {
                                if (CompleteID.Equals("00100030"))
                                    patInfo.Patient.BirthDate = DataStr;
                            }
                        }

                        if (CompleteID.Equals("00041430") && DataStr == "SERIES")
                        {
                            seriesNo++;
                        }

                        //현재상태가 SERIES 일때
                        if (RecordType == "SERIES")
                        {
                            if (CompleteID.Equals("00080021"))
                                SeriesDate = DataStr;
                            if (CompleteID.Equals("00080031"))
                                SeriesTime = DataStr;
                            if (CompleteID.Equals("00080060"))
                                Modality = DataStr;
                            if (CompleteID.Equals("00180015"))
                                BodyPart = DataStr;
                        }

                        if (RecordType == "IMAGE" && CompleteID.Equals("00041500"))
                        {
                            imgNo++;

                            PatientRecordInfo recordInfo = new PatientRecordInfo();
                            recordInfo.PatID = patInfo.Patient.ID;

                            recordInfo.StudyNumber = studyNo.ToString().PadLeft(4, '0');
                            recordInfo.StudyDate = StudyDate;
                            recordInfo.StudyTime = StudyTime;
                            recordInfo.StudyDesc = StudyDesc;
                            recordInfo.StudyModality = StudyModality;

                            recordInfo.SeriesNumber = seriesNo.ToString().PadLeft(4, '0');
                            recordInfo.SeriesDate = SeriesDate;
                            recordInfo.SeriesTime = SeriesTime;
                            recordInfo.Modality = Modality;
                            recordInfo.BodyPart = BodyPart;

                            recordInfo.ImageFilePath = DataStr;
                            recordInfo.ImageNumber = imgNo.ToString();

                            patInfo.RecordList.Add(recordInfo);
                        }
                    }

                    if (patInfo != null)
                        patList.Add(patInfo);
                }

                returnValue = new PatientList();
                patientList = new Dictionary<string, string>();

                List<PatientInfoEntity> newPatList = PatientConcat(patList);

                DicomDirReader.SetStudyModalityConcat(ref newPatList, ref patientList, ref dicomDesc);

                if (dicomDesc.EndsWith("^^"))
                {
                    dicomDesc = dicomDesc.Substring(0, dicomDesc.Length - 2);
                }
                
                returnValue.Base = newPatList;
                returnValue.SimpleList = patientList;
                returnValue.AllDesc = dicomDesc;
            }
            catch { }

            return returnValue;
        }

        /// <summary>
        /// 동일한 환자가 다른 목록으로 되어 있을 경우 합치기
        /// </summary>
        /// <param name="srcPatList"></param>
        /// <returns></returns>
        public static List<PatientInfoEntity> PatientConcat(List<PatientInfoEntity> srcPatList)
        {
            List<PatientInfoEntity> newPatList = new List<PatientInfoEntity>();

            try
            {
                //환자가 동일하다면 합친다.
                string nowPat = string.Empty;
                PatientInfoEntity nowPatEntity = new PatientInfoEntity();

                foreach (PatientInfoEntity pat in srcPatList)
                {
                    PatientInfoEntity result = SeekPatientInfo(ref newPatList, pat.Patient.ID);

                    if (result == null)
                    {
                        //다른 환자이면..
                        nowPatEntity = pat;
                        newPatList.Add(nowPatEntity);
                    }
                    else
                    {
                        nowPatEntity = result;

                        foreach (PatientRecordInfo record in pat.RecordList)
                        {
                            nowPatEntity.RecordList.Add(record);
                        }
                    }
                }
            }
            catch { }

            return newPatList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcPatList"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PatientInfoEntity SeekPatientInfo(ref List<PatientInfoEntity> srcPatList, string id)
        {
            PatientInfoEntity retPatInfo = null;

            foreach (PatientInfoEntity pat in srcPatList)
            {
                if (pat != null && id == pat.Patient.ID)
                {
                    retPatInfo = pat;
                    break;
                }
            }

            return retPatInfo;
        }

        /// <summary>
        /// Study, Modaliy string으로 연결하기
        /// </summary>
        /// <param name="patList"></param>
        /// <returns></returns>
        public static bool SetStudyModalityConcat(ref List<PatientInfoEntity> patList, ref Dictionary<string, string> patientList, ref string dicomDesc)
        {
            bool result = false;

            try
            {
                foreach (PatientInfoEntity pat in patList)
                {
                    Dictionary<string, string> studyInfo = new Dictionary<string, string>();
                    Dictionary<string, string> studyInfo2 = new Dictionary<string, string>();
                    Dictionary<string, string> modality = new Dictionary<string, string>();
                    Dictionary<string, string> studyInfo3 = new Dictionary<string, string>();

                    dicomDesc += string.Format("Patient ID: {0} ^^Patient's Name: {1}^^", pat.Patient.ID, pat.Patient.Name);

                    foreach (PatientRecordInfo record in pat.RecordList)
                    {
                        if (studyInfo.ContainsKey(record.StudyNumber) == false)
                        {
                            string date = string.IsNullOrWhiteSpace(record.StudyDate) ? record.SeriesDate : record.StudyDate;
                            string desc = string.IsNullOrWhiteSpace(record.StudyDesc) ? record.BodyPart : record.StudyDesc;
                            string modal = string.IsNullOrWhiteSpace(record.StudyModality) ? record.Modality : record.StudyModality;

                            if (string.IsNullOrWhiteSpace(date) == false)
                            {
                                date = date.Replace("-", "").Replace(".", "").Replace("/", "").Replace("_", "").Replace(" ", "");
                                if (date.Length > 8)
                                    date = date.Substring(0, 8);
                            }
                            
                            studyInfo.Add(record.StudyNumber, string.Format("[{0}]{1}", date, desc));
                            studyInfo2.Add(record.StudyNumber, string.Format("Study Date: {0}^^Study Description: {1}", date, desc));
                            studyInfo3.Add(record.StudyNumber, string.Format("{0}: [{1}] {2}", date, modal, desc));
                        }

                        if (modality.ContainsKey(record.SeriesNumber) == false)
                        {
                            modality.Add(record.SeriesNumber, record.Modality);
                        }
                    }

                    //Study
                    foreach (KeyValuePair<string, string> kvp in studyInfo)
                    {
                        pat.StudyDescArray += kvp.Value + "^^";
                    }

                    //Study2
                    foreach (KeyValuePair<string, string> kvp in studyInfo2)
                    {
                        dicomDesc += kvp.Value + "^^";
                    }

                    //StudyModality
                    foreach (KeyValuePair<string, string> kvp in studyInfo3)
                    {
                        pat.StudyModalityArray += kvp.Value + "^^";
                    }

                    //Modality
                    foreach (KeyValuePair<string, string> kvp in modality)
                    {
                        pat.ModalityArray += kvp.Value + "^^";
                    }

                    if (pat.StudyDescArray.EndsWith("^^"))
                    {
                        pat.StudyDescArray = pat.StudyDescArray.Substring(0, pat.StudyDescArray.Length - 2);
                    }

                    if (pat.ModalityArray.EndsWith("^^"))
                    {
                        pat.ModalityArray = pat.ModalityArray.Substring(0, pat.ModalityArray.Length - 2);
                    }

                    if (pat.StudyModalityArray.EndsWith("^^"))
                    {
                        pat.StudyModalityArray = pat.StudyModalityArray.Substring(0, pat.StudyModalityArray.Length - 2);
                    }
                    
                    if (patientList.ContainsKey(pat.Patient.ID) == false)
                        patientList.Add(pat.Patient.ID, pat.Patient.Name);
                }
            }
            catch { }

            return result;
        }

        /// <summary>
        /// 이미지정보를 DataTable에 넣는다.
        /// </summary>
        /// <param name="patList"></param>
        /// <returns></returns>
        public static DataTable GetImageList(List<PatientInfoEntity> patList)
        {
            DataTable dtImages = new DataTable();
            int id = 0;

            try
            {
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

                if (patList != null && patList.Count > 0)
                {
                    foreach (PatientRecordInfo record in patList[0].RecordList)
                    {
                        id++;
                        DataRow row = dtImages.NewRow();

                        string date = string.IsNullOrWhiteSpace(record.StudyDate) ? record.SeriesDate : record.StudyDate;
                        string desc = string.IsNullOrWhiteSpace(record.StudyDesc) ? record.BodyPart : record.StudyDesc;

                        if (string.IsNullOrWhiteSpace(date) == false)
                        {
                            date = date.Replace("-", "").Replace(".", "").Replace("/", "").Replace("_", "").Replace(" ", "");
                            if (date.Length > 8)
                                date = date.Substring(0, 8);
                        }

                        row["ID"] = id.ToString();
                        row["StudyID"] = record.StudyNumber;
                        row["StudyModality"] = record.Modality;
                        row["StudyDesc"] = desc;
                        row["StudyDate"] = date;
                        row["StudyTime"] = record.SeriesTime;
                        row["SeriesNumber"] = record.SeriesNumber;
                        row["SeriesModality"] = record.Modality;
                        row["imageNumber"] = record.ImageNumber;
                        row["imagePath"] = record.ImageFilePath;

                        dtImages.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dtImages;
        }
    }
}
