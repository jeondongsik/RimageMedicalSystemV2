using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RimageKorea
{
    [Serializable]
    public class PatientList
    {
        /// <summary>
        /// DicomDir 모든 정보
        /// </summary>
        public List<PatientInfoEntity> Base { get; set; }
        /// <summary>
        /// 환자목록 - 환자ID, Name
        /// </summary>
        public Dictionary<string, string> SimpleList { get; set; }
        /// <summary>
        /// 환자별 StudyDescription
        /// </summary>
        public string AllDesc { get; set; }
    }

    /// <summary>
    /// DicomDir에 포함된 환자정보
    /// </summary>
    [Serializable]
    public class PatientInfoEntity
    {
        /// <summary>
        /// 순번
        /// </summary>
        public int listNumber { get; set; }
        /// <summary>
        /// 환자기본정보
        /// </summary>
        public PatientBaseInfo Patient { get; set; }
        /// <summary>
        /// 레코드 정보
        /// </summary>
        public List<PatientRecordInfo> RecordList { get; set; }

        /// <summary>
        /// StudyDescription 연결 - [일자]StudyDescription^^ ~
        /// </summary>
        public string StudyDescArray { get; set; }
        /// <summary>
        /// Modality 연결
        /// </summary>
        public string ModalityArray { get; set; }
        /// <summary>
        /// StudyModality 연결
        /// </summary>
        public string StudyModalityArray { get; set; }
    }

    /// <summary>
    /// 환자 기본 정보
    /// </summary>
    [Serializable]
    public class PatientBaseInfo
    {
        string _id;
        string _name;
        string _gender;
        string _genderKR;
        string _birthDate;
        string _issueDate;
        string _age;

        /// <summary>
        /// 환자 아이디
        /// </summary>
        public string ID
        {
            get { return this._id; }
            set { this._id = value; }
        }
        /// <summary>
        /// 환자명
        /// </summary>
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }
        /// <summary>
        /// 버닝일자
        /// </summary>
        public string issueDate
        {
            get
            {
                this._issueDate = DateTime.Now.ToString("yyyy-MM-dd");
                return this._issueDate;
            }
            set { this._issueDate = value; }
        }
        /// <summary>
        /// 성별
        /// </summary>
        public string Gender
        {
            get { return this._gender; }
            set { this._gender = value; }
        }
        /// <summary>
        /// 성별(한글)
        /// </summary>
        public string GenderKR
        {
            get {
                if (this._gender == "F")
                    this._genderKR = "여";
                else if (this._gender == "M")
                    this._genderKR = "남";
                else
                    this._genderKR = this._gender;

                return this._genderKR;
            }
            set { this._genderKR = value; }
        }
        /// <summary>
        /// 생일
        /// </summary>
        public string BirthDate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this._birthDate) == false)
                {
                    string bDay = this._birthDate.Replace("-", "").Replace(".", "").Replace("/", "").Replace("_", "").Replace(" ", "");
                    if (bDay.Length > 8)
                        this._birthDate = bDay.Substring(0, 8);
                }

                return this._birthDate;
            }
            set
            {
                this._birthDate = value;
            }
        }
        /// <summary>
        /// 나이(만)
        /// </summary>
        public string Age
        {
            get
            {
                if (string.IsNullOrEmpty(this._birthDate) == false)
                    this.SetPatientAge(this._birthDate);

                return this._age;
            }
            set { this._age = value; }
        }

        /// <summary>
        /// 만나이 계산
        /// </summary>
        /// <param name="patBirthDay"></param>
        public void SetPatientAge(string patBirthDay)
        {
            int result = 0;
            int c1 = 0;

            try
            {
                if (string.IsNullOrWhiteSpace(patBirthDay) == false)
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

            this._age = result.ToString();
        }
    }

    /// <summary>
    /// 환자의 Record 정보
    /// </summary>
    [Serializable]
    public class PatientRecordInfo
    {
        /// <summary>
        /// 환자 아이디
        /// </summary>
        public string PatID { get; set; }

        /// <summary>
        /// Study번호
        /// </summary>
        public string StudyNumber { get; set; }
        /// <summary>
        /// Study일자
        /// </summary>
        public string StudyDate { get; set; }
        /// <summary>
        /// Study일시
        /// </summary>
        public string StudyTime { get; set; }
        /// <summary>
        /// StudyDesc
        /// </summary>
        public string StudyDesc { get; set; }
        /// <summary>
        /// StudyModality
        /// </summary>
        public string StudyModality { get; set; }

        /// <summary>
        /// 시리즈번호
        /// </summary>
        public string SeriesNumber { get; set; }
        /// <summary>
        /// 시리즈일자
        /// </summary>
        public string SeriesDate { get; set; }
        /// <summary>
        /// 시리지일시
        /// </summary>
        public string SeriesTime { get; set; }
        /// <summary>
        /// Modality
        /// </summary>
        public string Modality { get; set; }
        /// <summary>
        /// BodyPartExamined
        /// </summary>
        public string BodyPart { get; set; }

        /// <summary>
        /// 이미지번호
        /// </summary>
        public string ImageNumber { get; set; }
        /// <summary>
        /// ReferencedFileID
        /// </summary>
        public string ImageFilePath { get; set; }
    }
}
