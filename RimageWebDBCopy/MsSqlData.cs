using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace RimageWebDBCopy
{
    public class MsSqlData
    {
        private string connectionString;

        public MsSqlData()
        {
            this.connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Rimage;Persist Security Info=True;User ID=sa;Password=rimage11";
        }

        public DataTable GetData(string fromDate)
        {
            DataTable dtResult = new DataTable();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            
            try
            {
                conn = new SqlConnection(this.connectionString);

                string sql = string.Format("Select * From burnHistory WHERE [startDateTime] >= '{0}'", fromDate);

                cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dtResult);
            }
            catch
            {
            }

            return dtResult;
        }
    }
}
