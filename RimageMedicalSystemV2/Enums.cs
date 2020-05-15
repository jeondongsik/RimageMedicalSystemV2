using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RimageMedicalSystemV2
{
    public static class Enums
    {
        /// <summary>
        /// 굽기 진행 상태
        /// </summary>
        public enum BurnState
        {
            /// <summary>
            /// 명령 보냄
            /// </summary>
            Ordered = 10,
            /// <summary>
            /// 굽기진행중
            /// </summary>
            Running = 20,
            /// <summary>
            /// 종료
            /// </summary>
            Finish = 30
        }
    }
}
