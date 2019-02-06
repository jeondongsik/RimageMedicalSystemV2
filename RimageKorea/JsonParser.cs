using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace RimageKorea
{
    public class JsonParser
    {
        public static string ConvertToJsonString(object target)
        {
            try
            {
                JsonSerializerSettings jsonSetting = new JsonSerializerSettings();
                jsonSetting.NullValueHandling = NullValueHandling.Ignore;
                
                return JsonConvert.SerializeObject(target, jsonSetting);
            }
            catch { }

            return string.Empty;
        }
        
        /// <summary>
        /// 명령정보를 JSON 텍스트에서 BurnOrderedInfoEntity 객체로 변환
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static BurnOrderedInfoEntity ConvertToBurnOrderedInfoEntity(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                return JsonConvert.DeserializeObject<BurnOrderedInfoEntity>(json);
            }
            catch { }

            return null;
        }

        /// <summary>
        /// 명령정보를 파일에서 읽어봐서 BurnOrderedInfoEntity 객체로 변환
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static BurnOrderedInfoEntity ConvertToBurnOrderedInfoEntityFromFile(string orderID)
        {
            if (string.IsNullOrWhiteSpace(orderID))
                return null;

            try
            {
                //// 명령JSON 파일을 불러온다.
                string fileName = Path.Combine(GlobalVar.ProgramExecuteFolder, GlobalVar.ORDER_FOLDER, string.Format("{0}.json", orderID));

                if (!File.Exists(fileName))
                    return null;

                string json = File.ReadAllText(fileName);

                return JsonConvert.DeserializeObject<BurnOrderedInfoEntity>(json);
            }
            catch { throw; }
        }

        /// <summary>
        /// 굽기진행상태 JSON 텍스트에서 DiscStatusForDisplay 객체로 변환
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DiscStatusForDisplay ConvertToDiscStatusForDisplay(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                return JsonConvert.DeserializeObject<DiscStatusForDisplay>(json);
            }
            catch { }

            return null;
        }

        /// <summary>
        /// 오류 JSON 텍스트에서 DiscStatusForDisplay 객체로 변환
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static ErrorInfo ConvertToErrorInfo(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                return JsonConvert.DeserializeObject<ErrorInfo>(json);
            }
            catch { }

            return null;
        }
    }
}
