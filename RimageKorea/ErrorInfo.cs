using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RimageKorea
{
    /// <summary>
    /// 에러 정보
    /// </summary>
    public class ErrorInfo
    {
        /// <summary>
        /// 명령의 종류
        /// </summary>
        public string OrderType { get; set; }
        /// <summary>
        /// 명령아이디
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 오류 코드
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 오류 간략메시지
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 오류 상세내용
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 서버 IP
        /// </summary>
        public string ServerIP { get; set; }
        /// <summary>
        /// 서버명
        /// </summary>
        public string ServerName { get; set; }
    }
}
