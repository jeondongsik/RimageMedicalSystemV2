using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using DicomReader;

namespace RimageKorea
{
    /// <summary>
    /// 원광대학교 환자정보 반환해주는 클래스 - Delphi Dll 사용함.
    /// </summary>
    public class GetPatientNameFromDllImport
    {
        //// C:/wonmis/dll/srxdll.dll 경로는 원광대학교병원 통합전산시스템 WONMIS 기본 dll 사용 폴더명 입니다. 
        //// [DllImport("C:/wonmis/dll/srxdll.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        [DllImport("C:/wonmis/dll/srxdll.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]        
        public static extern string get_pat_info(string stype, string inputString, out string outputString);

        /// <summary>
        /// 환자 정보 가져오기
        /// </summary>
        /// <param name="stype">환자번호 : 1, 주민번호 : 2</param>
        /// <param name="inputString">type에 따라서 넘겨줘야 할 텍스트</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetPatInfo(string stype, string inputString)
        {
            Dictionary<string, string> returnValue = null;
            string outputString = string.Empty;
            StringBuilder sbObject = null;

            try
            {
                string errorString = get_pat_info(stype, inputString, out outputString);

                // Databe Error인 경우 outputString 에 DataBase Error : 메시지를 넘겨줌
                // 값이 없는 경우 outputString 에 0 을 넘겨줌
                // 정상적인 값인경우 환자번호^환자명^성별^생년월일 ex) 00760761^홍길동^M^19810123

                if (errorString != null)
                {
                    sbObject = new StringBuilder();
                    sbObject.Append("RimageKorea.GetPatientNameFromDllImport");

                    ErrorLog.LogWrite(sbObject, errorString, Environment.CurrentDirectory);

                    return null;
                }
                else
                {
                    if (outputString == "0")
                        return null;
                    else if (outputString.Contains("^"))
                    {
                        string[] arrStr = outputString.Split('^');
                        if (arrStr.Length != 4)
                        {
                            sbObject = new StringBuilder();
                            sbObject.Append("RimageKorea.GetPatientNameFromDllImport");

                            ErrorLog.LogWrite(sbObject, string.Format("반환문자열의 형식이 맞지 않습니다. >> {0}", outputString), Environment.CurrentDirectory);

                            return null;
                        }
                        else
                        {
                            returnValue = new Dictionary<string, string>();
                            returnValue.Add("환자번호", arrStr[0]);
                            returnValue.Add("환자명", arrStr[1]);
                            returnValue.Add("성별", arrStr[2]);
                            returnValue.Add("생년월일", arrStr[3]);

                            if (arrStr[2] == "M")
                                returnValue["성별"] = "남";
                            else if (arrStr[2] == "F")
                                returnValue["성별"] = "여";

                            int Age = Utils.GetPatientAge(arrStr[3], arrStr[0]);

                            returnValue.Add("나이", Age.ToString());
                            
                            return returnValue;
                        }
                    }
                    else
                    {
                        sbObject = new StringBuilder();
                        sbObject.Append("RimageKorea.GetPatientNameFromDllImport");

                        ErrorLog.LogWrite(sbObject, outputString, Environment.CurrentDirectory);
                    }
                }
            }
            catch (Exception ex)
            {
                sbObject = new StringBuilder();
                sbObject.Append("RimageKorea.GetPatientNameFromDllImport");

                ErrorLog.LogWrite(sbObject, ex.ToString(), Environment.CurrentDirectory);
            }

            return returnValue;
        }
    }
}
