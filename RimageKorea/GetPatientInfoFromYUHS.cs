using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using YUHS;

namespace RimageKorea
{
    /// <summary>
    /// 신촌 세브란스 병원 환자명 가져오기
    /// </summary>
    public static class GetPatientInfoFromYUHS
    {
        public static string GetName(string patID)
        {
            string name = string.Empty;

            ////////////////////////////////////////////////////////////////////////////////
            // public SqlConnection OpenDataBase(string WorkGb, string DbGb) 를 호출하면 됩니다.
            // DbGb의 경우,SDEV(신촌개발) / SEDU(신촌교육) / SPROD(신촌운영) / CALLDB(신촌 CALLDB)
            ////////////////////////////////////////////////////////////////////////////////

            try
            {
                YUHS.DBConnect dbConn = new YUHS.DBConnect();

                //SqlConnection conn = dbConn.OpenDataBase("PATHOINF", "SDEV");  //<---------------------------------------------개발시에는 SDEV

                SqlConnection conn = dbConn.OpenDataBase("PATHOINF", "SPROD");   //<---------------------------------------------운영시에는 SPROD
                SqlCommand cmd = new SqlCommand();

                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 600;

                //cmd.CommandText = "his_user.USP_SP_TES_RC_ExRcptDA_SelectExtrImgCD"; // <--------------------------------- 개발시에는 his_user를 붙힘 (예, his_user.USP_SP___)

                cmd.CommandText = "USP_SP_TES_RC_ExRcptDA_SelectExtrImgCD"; // 개발시에는 his_user.USP_SP___...를 붙힌다.  (his_user를 앞에 붙힌다.)  //<---------------------------------------------운영시에는

                cmd.Parameters.Add("@UnitNo", SqlDbType.VarChar).Value = patID;

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows == true)
                {
                    //string id = GetFieldStringByValue(reader, "UnitNo"); patID = patID.Replace(" ", "");
                    //string name = GetFieldStringByValue(reader, "PatNm");
                    //birthDate = GetFieldStringByValue(reader, "BirthYmd");
                    //sex = GetFieldStringByValue(reader, "sex");
                    //age = GetFieldStringByValue(reader, "age");

                    while (reader.Read())
                    {
                        name = reader["PatNm"] as string;
                    }
                }

                reader.Close();
                conn.Close();
                dbConn.CloseDataBase();
            }
            catch
            {

            }

            return name;
        }
    }
}
