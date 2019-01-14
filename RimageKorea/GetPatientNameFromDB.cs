using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using System.IO;

namespace RimageKorea
{
    public static class GetPatientNameFromDB
    {
        /*
        DataBaseType = "Oracle"
        DataSource = "MEDIS"
        UserID = "rimage"
        UserPass = "image001"
        TableName = "TBLLINK_PATIENT"
        PatientNameColumn = "patname"
        PatientIDColumn = "patID"
         */
        /// <summary>
        /// DB 정보 가져오기
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDBConn(string filePath)
        {
            Dictionary<string, string> info = null;

            try
            {
                info = new Dictionary<string, string>();

                if (File.Exists(filePath))
                {
                    foreach (string line in File.ReadLines(filePath))
                    {
                        string[] arrStr = line.Split('=');
                        info.Add(arrStr[0].Replace(" ", ""), arrStr[1].Replace(" ", "").Replace("\"", ""));
                    }
                }
            }
            catch { }

            return info;
        }

        /// <summary>
        /// 환자명 조회하기
        /// </summary>
        /// <param name="patID"></param>
        /// <param name="dbInfo"></param>
        /// <returns></returns>
        public static string GetName(string patID, Dictionary<string, string> dbInfo)
        {
            string patName = string.Empty;
            OleDbConnection conn = null;
            string sqlString = string.Empty;
            string conString = string.Empty;
            DataSet ds = null;

            try
            {
                if (dbInfo == null)
                    return string.Empty;

                sqlString = "SELECT {0} FROM {1} WHERE {2} = '{3}'";
                conString = "Provider=MSDAORA.1;Password={0};User ID={1};Data Source={2};Persist Security Info=True;OLEDB.NET=True";

                string sql = string.Format(sqlString, dbInfo["PatientNameColumn"], dbInfo["TableName"], dbInfo["PatientIDColumn"], patID);
                string con = string.Format(conString, dbInfo["UserPass"], dbInfo["UserID"], dbInfo["DataSource"]);

                conn = new OleDbConnection(con);
                conn.Open();

                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                OleDbDataAdapter adp = new OleDbDataAdapter(cmd);

                ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    patName = Utils.CheckNull(ds.Tables[0].Rows[0][0]).Trim();
                }
            }
            catch { }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

            return patName;
        }

        /// <summary>
        /// 병동 조회하기
        /// </summary>
        /// <param name="patID"></param>
        /// <param name="dbInfo"></param>
        /// <returns></returns>
        public static string GetResidency(string patID, Dictionary<string, string> dbInfo)
        {
            string residence = string.Empty;
            OleDbConnection conn = null;
            string sqlString = string.Empty;
            string conString = string.Empty;
            DataSet ds = null;

            try
            {
                if (dbInfo == null)
                    return string.Empty;

                sqlString = "SELECT neta1.fn_ap_patient_wardnm('{0}') RET_VAL FROM DUAL";
                conString = "Provider=MSDAORA.1;Password={0};User ID={1};Data Source={2};Persist Security Info=True;OLEDB.NET=True";

                string sql = string.Format(sqlString, patID);
                string con = string.Format(conString, dbInfo["UserPass"], dbInfo["UserID"], dbInfo["DataSource"]);

                conn = new OleDbConnection(con);
                conn.Open();

                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                OleDbDataAdapter adp = new OleDbDataAdapter(cmd);

                ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    residence = Utils.CheckNull(ds.Tables[0].Rows[0][0]).Trim();
                }
            }
            catch { }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

            return residence;
        }
    }
}
