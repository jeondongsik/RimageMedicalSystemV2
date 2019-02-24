using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RimageKorea
{
    /// <summary>
    /// 굽기프로그램 종료 이유
    /// </summary>
    public enum EnumExitType
    {
        Success,
        Fail,
        None
    }

    /// <summary>
    /// 파일 복사 대상
    /// </summary>
    public enum MediaType
    {
        CD_DVD,
        USB
    }
}
