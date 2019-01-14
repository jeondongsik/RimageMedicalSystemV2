using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Xml.Serialization;

namespace RimageKorea
{
    public class GetPatientInfoFromMDB
    {
        string mdbFile = string.Empty;
        Dictionary<string, string> patientList;
        int patientCount = 0;

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

        public GetPatientInfoFromMDB(string mdbPath)
        {
            this.mdbFile = mdbPath;
            this.patientList = new Dictionary<string, string>();
        }

        /// <summary>
        /// mdb 파일에서 환자정보 가져오기
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetInfo()
        {
            Dictionary<string, string> dicomdirInfo = null;
            dicomdirInfo = new Dictionary<string, string>();

            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + this.mdbFile;
            string strAccessSelect = "Select * From Study";

            OleDbConnection myAccessConn = new OleDbConnection(strAccessConn);

            try
            {
                DataTable dtInfo = new DataTable();

                OleDbCommand myAccessCommand = new OleDbCommand(strAccessSelect, myAccessConn);
                OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(myAccessCommand);

                myAccessConn.Open();
                myDataAdapter.Fill(dtInfo);

                if (dtInfo.Rows.Count > 0)
                {
                    string ID = Utils.CheckNull(dtInfo.Rows[0]["patient_id"]);
                    string Name = Utils.ReplacePatientName(Utils.CheckNull(dtInfo.Rows[0]["patient_name"]));
                    string Sex = Utils.CheckNull(dtInfo.Rows[0]["patient_sex"]);
                    string SexKr = string.Empty;
                    if (Sex.ToUpper().Contains("F"))
                        SexKr = "여";
                    else if (Sex.ToUpper().Contains("M"))
                        SexKr = "남";

                    string BirthDay = Utils.CheckNull(dtInfo.Rows[0]["patient_birth_date"]);
                    int Age = Utils.GetPatientAge(BirthDay, ID);

                    dicomdirInfo.Add("ID", ID);
                    dicomdirInfo.Add("Name", Name);
                    dicomdirInfo.Add("Sex", Sex);
                    dicomdirInfo.Add("SexKr", SexKr);
                    dicomdirInfo.Add("BirthDay", BirthDay);
                    dicomdirInfo.Add("Age", Age.ToString());

                    foreach (DataRow dr in dtInfo.Rows)
                    {
                        if (this.patientList.ContainsKey(Utils.CheckNull(dr["patient_id"])) == false)
                        {
                            this.patientList.Add(Utils.CheckNull(dr["patient_id"]), Utils.ReplacePatientName(Utils.CheckNull(dr["patient_name"])));
                        }
                    }

                    this.patientCount = this.patientList.Count;
                }
                else 
                {
                    dicomdirInfo = null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (myAccessConn.State == ConnectionState.Open)
                    myAccessConn.Close();

                if (myAccessConn != null)
                    myAccessConn = null;
            }

            return dicomdirInfo;
        }

    }

}
