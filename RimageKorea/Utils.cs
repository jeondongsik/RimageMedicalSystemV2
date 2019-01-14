using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RimageKorea
{
    public class Utils
    {
        /// <summary>
        /// 폴더명에 넣을 수 없는 특수문자를 공백으로 Replace
        /// </summary>
        /// <param name="orgString"></param>
        /// <returns></returns>
        public static string ReplaceSpecialWord(string orgString)
        {
            string returnStr = "";
                        
            returnStr = orgString.Replace("\\", "");
            returnStr = returnStr.Replace((char)27, ' ');
            returnStr = returnStr.Replace("/", "");
            returnStr = returnStr.Replace(":", "");
            returnStr = returnStr.Replace("*", "");
            returnStr = returnStr.Replace("?", "");
            returnStr = returnStr.Replace("\"", "");
            returnStr = returnStr.Replace("<", "");
            returnStr = returnStr.Replace(">", "");
            returnStr = returnStr.Replace("|", "");
            returnStr = returnStr.Replace("^", " ");
            returnStr = returnStr.Replace("&", "");
            returnStr = returnStr.Replace("'", "");
            returnStr = returnStr.Replace("_", "").TrimStart().TrimEnd();

            return returnStr;
        }

        /// <summary>
        /// 환자명 쓸데없는 문자 없애기
        /// </summary>
        /// <param name="ogrName"></param>
        /// <returns></returns>
        public static string ReplacePatientName(string ogrName)
        {
            string returnStr = "";

            returnStr = ogrName.Replace((char)27, ' ');

            if (returnStr.Contains("="))
            {
                string[] arrStr = ogrName.Split('=');
                returnStr = arrStr[0];
            }
            
            returnStr = returnStr.Replace("\0", "").Replace("^", "_").Replace("\r\n", "").Trim();
            returnStr = returnStr.Replace("\t", "").Replace("$", "").Replace("\b", "");
            returnStr = returnStr.Replace("_", "").TrimStart().TrimEnd();

            return returnStr;
        }

        /// <summary>
        /// 머지 파일에 찍어줄 Modality 문자열 생성
        /// </summary>
        /// <param name="orgString">원본문자열</param>
        /// <param name="cnt">갯수</param>
        /// <returns></returns>
        public static string ReturnModality(string orgString, int cnt)
        {
            string returnStr = "";
            string[] arrStr = null;
            string[] arrChg = new string[cnt + 1];

            if (orgString.Contains("^^") == false)
            {
                returnStr = orgString;
            }
            else
            {
                arrStr = SplitByString(orgString, "^^");

                //문자열 생성
                returnStr = SortedStr(arrStr, cnt);
            }

            return returnStr;
        }

        /// <summary>
        /// 문자열로 자르기
        /// </summary>
        /// <param name="testString"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static string[] SplitByString(string testString, string split)
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
        /// Dictionary 정렬
        /// </summary>
        /// <param name="ar"></param>
        /// <param name="cnt"></param>
        /// <returns></returns>
        public static string SortedStr(string[] ar, int cnt)
        {
            string returnStr = "";

            //정렬 기준 생성 (CT, MR, CR, PT, NM, ES, XA, US, MG, OT)
            Dictionary<string, int> stdSort = GlobalVar.ModalityStandard;

            //정렬하기 위해서 기준에 따라 문자열 생성
            int i = 20;
            SortedDictionary<int, string> sortedStr = new SortedDictionary<int, string>();
            foreach (string str in ar)
            {
                string newStr = str;
                if (newStr.Contains("\\"))
                {
                    newStr = str.Substring(0, str.IndexOf("\\"));
                }

                //기준 Modality에 존재할 경우에만 찍는다.
                if (stdSort.ContainsKey(newStr.Trim()))
                {
                    if (!sortedStr.ContainsValue(newStr.Trim()))
                    {
                        sortedStr.Add(stdSort[newStr.Trim()], newStr.Trim());
                    }
                }

                i++;
            }

            //SortedDictionary에 존재하는 값을 읽어서 문자열로 생성한다.
            int count = 0;
            foreach (KeyValuePair<int, string> kvp in sortedStr)
            {
                count++;
                returnStr += kvp.Value + " ";
            }

            //if (sortedStr.Count > cnt)
            //{
            //    returnStr = returnStr.Substring(0, returnStr.Length - 1) + "...";
            //}
            //else
            //{
            //    returnStr = returnStr.Substring(0, returnStr.Length - 1);
            //}

            return returnStr.TrimEnd();
        }

        /// <summary>
        /// 클래스를 문자열로 변환
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="orderData"></param>
        /// <returns></returns>
        public static string ToString<T>(DiscData orderData)
        {
            Type elementType = typeof(T);
            StringBuilder sbStr = new StringBuilder();

            //add a column to table for each public property on T
            foreach (var propInfo in elementType.GetProperties())
            {   
                sbStr.AppendLine(propInfo.Name + " : " + propInfo.GetValue(orderData, null) ?? "-");
            }

            return sbStr.ToString();
        }

        /// <summary>
        /// 문자열을 왼쪽부터 바이트로 자르기
        /// </summary>
        /// <param name="sourceStr">원본 문자열</param>
        /// <param name="len">잘라올 길이(Byte단위)</param>
        /// <returns>String - 값</returns>
        public static string LeftByte(string sourceStr, int len)
        {
            string isTmpStrValue = string.Empty;
            string isReturnValue = string.Empty;
            int iByteCont = 0;

            try
            {
                sourceStr = sourceStr.PadRight(len, ' ');

                for (int i = 0; i < sourceStr.Length; i++)
                {
                    isTmpStrValue = isTmpStrValue + sourceStr.Substring(i, 1);
                    iByteCont = Encoding.Default.GetByteCount(isTmpStrValue);

                    if (iByteCont == len)
                    {
                        isReturnValue = isTmpStrValue.ToString();
                        break;
                    }
                    else if (iByteCont > len)
                    {
                        isReturnValue = isTmpStrValue.Substring(0, isTmpStrValue.Length - 1);
                        isReturnValue = isReturnValue + string.Empty.PadRight(1, ' ').ToString();
                        break;
                    }
                }
            }
            catch
            {
                isReturnValue = "";
            }

            return isReturnValue.Trim();
        }

        /// <summary>
        /// DB null , null -> string.Empty
        /// </summary>
        /// <param name="obj">Object - 입력 값</param>
        /// <returns>string - 값</returns>
        public static string CheckNull(Object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }
        }

        /// <summary>
        /// DB null , null -> defVal
        /// </summary>
        /// <param name="obj">Object - 입력 값</param>
        /// <param name="defVal">Null이거나 빈값일 때 반환받을 값</param>
        /// <returns></returns>
        public static string CheckNull(Object obj, string defVal = "")
        {
            if (obj == null || obj == DBNull.Value)
            {
                return defVal;
            }
            else
            {
                return string.IsNullOrWhiteSpace(obj.ToString()) ? defVal : obj.ToString();
            }
        }

        /// <summary>
        /// 환자나이계산
        /// </summary>
        /// <param name="patBirthDay"></param>
        /// <param name="patID"></param>
        /// <returns></returns>
        public static int GetPatientAge(string patBirthDay, string patID)
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

        /// <summary>
        /// 환자목록을 Generic으로 받아 String 로 반환
        /// </summary>
        /// <param name="dicPatList"></param>
        /// <returns></returns>
        public static string GetPatientList(Dictionary<string, string> dicPatList)
        {
            string retStr = string.Empty;

            if (dicPatList == null) return retStr;
            
            foreach (KeyValuePair<string, string> kp in dicPatList)
            {
                retStr += kp.Key + "/" + kp.Value + "^^";
            }

            if (retStr.EndsWith("^^"))
                retStr = retStr.Substring(0, retStr.Length - 2);

            return retStr;
        }

        /// <summary>
        /// 환자목록을 Dictionary형태로 반환
        /// </summary>
        /// <param name="patList"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetPatList(string patList)
        {
            Dictionary<string, string> retList = new Dictionary<string, string>();

            try
            {
                if (patList.Contains("^^"))
                {
                    string[] arrStr = Utils.SplitByString(patList, "^^");

                    foreach (string str in arrStr)
                    {
                        string[] arrPat = str.Split('/');
                        if (retList.ContainsKey(arrPat[0]) == false)
                            retList.Add(arrPat[0], arrPat[1]);
                    }
                }
            }
            catch { }

            return retList;
        }

        /// <summary>
        /// 한글인지 체크..
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsHangul(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;

            string startChar = str.Substring(0, 1);
            int byteCount = Encoding.Default.GetByteCount(startChar);

            if (byteCount == 2)
                return true;

            return false;
        }

        /// <summary>
        /// StudyModality를 정렬하고 제한갯수만큼 자른다.
        /// </summary>
        /// <param name="studyModality"></param>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public static string ConvertStudyModality(string studyModality, int limitCount)
        {
            string returnStr = string.Empty;

            try
            {
                string[] arrStr = null;

                if (studyModality.Contains("^^") == false)
                {
                    returnStr = studyModality;
                }
                else
                {
                    List<SortList> sortedList = new List<SortList>();
                    Dictionary<string, int> stdSort = GlobalVar.ModalityStandard;

                    int i = 20;
                    arrStr = SplitByString(studyModality, "^^");
                    string modal = string.Empty;

                    foreach (string str in arrStr)
                    {
                        modal = str.Substring(str.IndexOf("[") + 1, 2);

                        if (stdSort.ContainsKey(modal))
                        {
                            sortedList.Add(new SortList(stdSort[modal], str.Replace(",", "")));
                        }
                        else
                        {
                            sortedList.Add(new SortList(i++, str.Replace(",", "")));
                        }
                    }

                    //// 정렬하기
                    var afList = sortedList.OrderBy(x => x.Sequence);
                    int j = 0;

                    //// 다시 문자열로 생성한다.
                    foreach (SortList sl in afList)
                    {
                        if (j < limitCount)
                        {
                            returnStr += string.Format("{0}{1}", sl.ValueString, ",");
                            j++;
                        }
                    }

                    int diff = sortedList.Count - limitCount;

                    if (diff > 0)
                    {
                        returnStr += string.Format("Plus {0} more studies on disc.", diff.ToString());
                    }

                    if (returnStr.EndsWith(","))
                    {
                        returnStr = returnStr.Substring(0, returnStr.LastIndexOf(","));
                    }
                }
            }
            catch
            {
            }

            return returnStr;
        }

        /// <summary>
        /// 서버 번호 반환
        /// </summary>
        /// <param name="servers"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetServerNo(List<ServerInfo> servers, string ip)
        {
            string no = "1";

            try
            {
                var hst = servers.Where(c => c.IP == ip);
                if (hst != null && hst.Count() > 0)
                {
                    no = hst.First().No;
                }
            }
            catch { }

            return no;
        }

        /// <summary>
        /// 현재시간 반환 (yyyyMMddhhmmss)
        /// </summary>
        /// <returns></returns>
        public static string GetNowTime()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// String to Stream
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();

            try
            {
                var writer = new StreamWriter(stream);
                writer.Write(s);
                writer.Flush();
                stream.Position = 0;
            }
            catch { }

            return stream;
        }
    }
}
