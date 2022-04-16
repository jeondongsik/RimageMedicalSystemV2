using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RimageKorea
{
    /// <summary>
    /// Compumedics Profusion 팍스 - EEG4PatientInfo.xml 파일에서 환자 정보를 읽어온다.
    /// </summary>
    public class GetPatientInfoFromEEG4Xml
    {
        string _name;
        string _name2;
        string _id;
        string _sex;
        string _age;
        string _birthday;

        public string Name
        {
            get { return _name; }
        }

        public string ID
        {
            get { return _id; }
        }

        public string Sex
        {
            get { return _sex; }
        }

        public string Age
        {
            get { return _age; }
        }

        /// <summary>
        /// 환자 정보 읽기
        /// </summary>
        /// <param name="xmlPath"></param>
        public void GetInfo (string xmlPath)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(xmlPath);

                XmlNodeList nodeList = xml.SelectNodes("/EEG4PatientInformation");

                foreach (XmlNode node in nodeList)
                {
                    this._id = node["Reference"].InnerText;
                    this._name = node["Surname"].InnerText;
                    this._name2 = node["GivenName"].InnerText;
                    this._sex = node["Sex"].InnerText;
                    this._birthday = node["DOB"].InnerText;
                }

                //// 이름정리
                this._name = string.Format("{0} {1}", this._name.Replace(this._id, ""), this._name2.Replace(this._id, ""));

                //// 성별
                if (this._sex == "Male")
                    this._sex = "남";
                else if (this._sex == "Female")
                    this._sex = "여";
                else
                    this._sex = "";

                //// 나이 계산
                //// 생일 형태가 14/08/1970 형태임
                //// 5/07/1953

                if (!string.IsNullOrEmpty(this._birthday))
                {
                    string bod = "";

                    if (this._birthday.Length == 9)
                        bod = this._birthday.Substring(5, 4) + "-" + this._birthday.Substring(2, 2) + "-" + this._birthday.Substring(0, 1).PadLeft(2, '0');
                    else
                        bod = this._birthday.Substring(6, 4) + "-" + this._birthday.Substring(3, 2) + "-" + this._birthday.Substring(0, 2);

                    int Age = Utils.GetPatientAge(bod, this._id);

                    this._age = Age.ToString();
                }
            }
            catch
            {
                this._id = string.Empty;
                this._name = string.Empty;
                this._sex = string.Empty;
                this._age = string.Empty;
                this._birthday = string.Empty;
            }

            /*
<EEG4PatientInformation>
    <Surname>Seo jin-hyeok(서진혁) 00671857</Surname>
    <GivenName>00671857</GivenName>
    <Given2Name/>
    <Title/>
    <Reference>2021-0481</Reference>
    <UR/>
    <Sex>Male</Sex>
    <Handedness/>
    <DOB>14/08/1970</DOB>
    <Status/>
    <TestDate/>
    <Weight>0</Weight>
    <Height>0</Height>
    <NeckSize>0</NeckSize>
    <MartialStatus/>
    <ReasonForStudy/>
    <Diagnosis/>
    <Medication/>
    <Occupation/>
    <ReferringPhysician/>
    <Doctor/>
    <Technician/>
    <Hospital/>
    <Address>
    <Street/>
    <Suburb/>
    <State/>
    <PostCode/>
    <HomePhone/>
    <BusinessPhone/>
    </Address>
    <Guardian>
    <Name1/>
    <Relationship1/>
    <HomePhone1/>
    <BusinessPhone1/>
    <Name2/>
    <Relationship2/>
    <HomePhone2/>
    <BusinessPhone2/>
    </Guardian>
    <PatientPhoto/>
</EEG4PatientInformation>
     */
        }
    }
}
