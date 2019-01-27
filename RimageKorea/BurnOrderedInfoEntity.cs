using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rimage.Client.Api;

namespace RimageKorea
{
    /// <summary>
    /// 환자의 굽기 명령 정보
    /// </summary>
    public class BurnOrderedInfoEntity
    {
        /// <summary>
        /// 굽기 시작 일시
        /// </summary>
        public string StartDateTime { get; set; }
        /// <summary>
        /// 버닝종료 일시
        /// </summary>
        public string CreateDateTime { get; set; }
        /// <summary>
        /// 버닝일자
        /// </summary>
        public string patDate { get; set; }
        /// <summary>
        /// 환자아이디
        /// </summary>
        public string patNo { get; set; }
        /// <summary>
        /// 환자명
        /// </summary>
        public string patName { get; set; }
        /// <summary>
        /// 수량
        /// </summary>
        public int copies { get; set; }
        /// <summary>
        /// 미디어타입
        /// </summary>
        public string mediType { get; set; }
        /// <summary>
        /// 미디어사이즈
        /// </summary>
        public string mediSize { get; set; }
        /// <summary>
        /// 공유폴더
        /// </summary>
        public string LocalShareFolder { get; set; }
        /// <summary>
        /// Modality 정보
        /// </summary>
        public string Modality { get; set; }
        /// <summary>
        /// Study 정보
        /// </summary>
        public string StudyDescription { get; set; }
        /// <summary>
        /// Study Modaliy
        /// </summary>
        public string StudyModality { get; set; }
        /// <summary>
        /// All Desc
        /// </summary>
        public string DicomDescription { get; set; }
        /// <summary>
        /// 성별
        /// </summary>
        public string patSex { get; set; }
        /// <summary>
        /// 나이
        /// </summary>
        public string patAge { get; set; }
        /// <summary>
        /// 생일
        /// </summary>
        public string patBirtyDay { get; set; }
        /// <summary>
        /// 환자폴더명
        /// </summary>
        public string patFolder { get; set; }
        /// <summary>
        /// 환자의 전체경로명
        /// </summary>
        public string patFolderFullPath { get; set; }
        /// <summary>
        /// 폴더사이즈
        /// </summary>
        public long FolderSize { get; set; }
        /// <summary>
        /// EditList FullName
        /// </summary>
        public string EditListPath { get; set; }
        /// <summary>
        /// EditList
        /// </summary>
        public List<string> EditList { get; set; }
        /// <summary>
        /// 결과값
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 환자목록
        /// </summary>
        public Dictionary<string, string> patList { get; set; }
        /// <summary>
        /// 환자목록-수정
        /// </summary>
        public Dictionary<string, string> patListForMerge { get; set; }
        /// <summary>
        /// 다중환자굽기여부
        /// </summary>
        public string BurnPatientKind { get; set; }
        /// <summary>
        /// 환자 수
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 구울 파일목록
        /// </summary>
        public FolderInfo ImgFiles { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string JobPath { get; set; }
        /// <summary>
        /// 머지파일 Fullpath
        /// </summary>
        public string MegPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MegFilePath { get; set; }

        /// <summary>
        /// 굽기실행할 서버정보
        /// </summary>
        public ServerInfo TargetServer { get; set; }

        /// <summary>
        /// 굽기주문 정보
        /// </summary>
        public string OrderXml { get; set; }

        /// <summary>
        /// 굽기명령 정보
        /// </summary>
        public DiscData DiscOrder { get; set; }

        /// <summary>
        /// Dicom Image 목록
        /// </summary>
        public List<string> DicomImgList { get; set; }

        /// <summary>
        /// 환자정보가 위치한 폴더 (로컬)
        /// </summary>
        public string DicomCDFolder { get; set; }

        /// <summary>
        /// DicomDir에 포함된 환장 정보
        /// </summary>
        public PatientList PatInfor { get; set; }

        /// <summary>
        /// 진행상태
        /// </summary>
        public string Progress { get; set; }
        /// <summary>
        /// 진행률(%)
        /// </summary>
        public string ProcessingRate { get; set; }
        /// <summary>
        /// 굽기 상태
        /// </summary>
        public string BurnState { get; set; }
        /// <summary>
        /// 출력번호
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 서버번호
        /// </summary>
        public string ServerNo { get; set; }
    }
}
