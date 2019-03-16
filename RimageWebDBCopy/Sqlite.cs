using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.IO;

using System.Data.SQLite;

namespace RimageWebDBCopy
{
    public class Sqlite
    {
        private string connectionString;

        public Sqlite(string dbFilePath)
        {
            this.connectionString = @"Data Source=" + dbFilePath;
        }

        public string Insert(string Idx, string startDateTime, string endDateTime, string PatientID, string PatientName,
        int Copies, string MediaType, string FilesLength, string IPAddress, string Status, string StudyDesc, string Modality = "")
        {
            string retStr = string.Empty;
            StringBuilder sbSql = new StringBuilder();

            string year = string.Empty;
            string month = string.Empty;
            string day = string.Empty;

            if (startDateTime.Length > 7)
            {
                year = startDateTime.Substring(0, 4);
                month = startDateTime.Substring(4, 2);
                day = startDateTime.Substring(6, 2);
            }

            sbSql.Append(" Insert Into BurnHistory ( ");
            sbSql.Append("      Idx, StartDateTime, EndDateTime, PatientID, PatientName, Copies, MediaType, FilesLength, IPAddress, Status, StudyDesc, Modality, Year, Month, Day ) ");
            sbSql.Append(" Values ( ");
            sbSql.Append("  '" + Idx + "' ");
            sbSql.Append(" ,'" + startDateTime + "' ");
            sbSql.Append(" ,'" + endDateTime + "' ");
            sbSql.Append(" ,'" + PatientID + "' ");
            sbSql.Append(" ,'" + PatientName.Replace("'", "") + "' ");
            sbSql.Append(" , " + Copies.ToString() + " ");
            sbSql.Append(" ,'" + MediaType + "' ");
            sbSql.Append(" ,'" + FilesLength + "' ");
            sbSql.Append(" ,'" + IPAddress + "' ");
            sbSql.Append(" ,'" + Status + "' ");
            sbSql.Append(" ,'" + StudyDesc.Replace("'", "") + "' ");
            sbSql.Append(" ,'" + Modality.Replace("'", "") + "' ");
            sbSql.Append(" ,'" + year + "' ");
            sbSql.Append(" ,'" + month + "' ");
            sbSql.Append(" ,'" + day + "' ");
            sbSql.Append(" )");

            SQLiteConnection conn = new SQLiteConnection(this.connectionString);

            try
            {
                conn.Open();

                SQLiteCommand dbCommand = new SQLiteCommand(sbSql.ToString(), conn);
                dbCommand.ExecuteNonQuery();
            }
            catch
            {
                retStr = string.Format("{0}-{1}-{2}-{3}", Idx, startDateTime, PatientID, PatientName);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return retStr;
        }
    }
}
