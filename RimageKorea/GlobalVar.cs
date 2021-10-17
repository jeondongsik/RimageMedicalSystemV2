using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RimageKorea
{
    /// <summary>
    /// 공통 상수
    /// </summary>
    public static class GlobalVar
    {
        public const string LOG_ERR_FLD = "Log";
        public const string LOG_ORDER_FLD = "OrderHistory";
        public const string LOG_ORDER_EDITLIST_FLD = "EditList";
		public const string LOG_END_FLD = "End";
        public const string BURN_CHK_FL_NM = "burn.end";      //굽기완료된 폴더에 생성시킨다.
        public const string SEEK_CHK_FL_NM = "seek.end";      //찾기완료된 폴더에 생성시킨다.
        public static string DOWN_CHK_FL_NM = "end.txt";       //다운로드가 완료되었을때 생성(아산병원에서 사용)

        public const string COMBO_LabelFile = "MULTIPLE.btw";       //여러명의 환자를 구울 때 라벨파일명

        public const string CheckFileList = "CheckFiles.lst";       //체크할 파일 목록을 저장한 파일명

        public const int sleepTime1 = 5000;   //후킹후 SleepTime
        public const int sleepTime2 = 10000;   //후킹후 SleepTime

        public const string DBCONN_FILE_NAME = "CONINFO.DAT";
        public const string SOCKET_CON_FILE = "CONTPSVR.DAT";

        //// 프로그램 실행파일 3으로 바꿀 경우 수정 >>>>>>>>>>>>>>>>>>>>>>>
        public const string PRODUCT_NAME = "Rimage Medical Disc System";
        public const string RIMAGE_MEDICAL_CLIENT = "RimageMedicalSystemV2";
        public const string RIMAGE_CONFIG_SETTING = "RimageConfigSetting";
        public const string RIMAGE_ENTERANCE = "RimageEnterance";
        public const string RMDS_PROG = "RMDS";

        /// <summary>
        /// 굽기 프로그램명
        /// </summary>
        public static string BURM_PROGRAM = "RMDS.exe";

        //// 프로그램 실행파일 3으로 바꿀 경우 수정 >>>>>>>>>>>>>>>>>>>>>>>

        public const string CD_INFO_FILE = "CD_Info.txt";
        public const string CD_STUDY_INFO_FILE = "Studyinfo.ini";

        public const string IMG_FOLDER = "Images";  //// 이미지 저장 폴더

        /// <summary>
        /// 굽기명령 정보가 저장되는 폴더
        /// </summary>
        public const string ORDER_FOLDER = "Orders";

        /// <summary>
        /// 복사 신청서 하단 이미지 파일
        /// </summary>
        public static Image BottomImage
        {
            get
            {
                Config cf = new Config(Environment.CurrentDirectory);
                string imageName = cf._BottomImagePath;
                string imageFolder = System.IO.Path.Combine(Environment.CurrentDirectory, IMG_FOLDER);
                string imagePath = System.IO.Path.Combine(imageFolder, imageName);

                if (!string.IsNullOrWhiteSpace(imagePath) && System.IO.File.Exists(imagePath))
                {
                    return Image.FromFile(imagePath);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 정렬 기준 생성 (CT, MR, CR, PT, NM, ES, XA, US, MG, OT)
        /// </summary>
        public static Dictionary<string, int> ModalityStandard
        {
            get
            {
                //정렬 기준 생성 (CT, MR, CR, PT, NM, ES, XA, US, MG, OT)
                Dictionary<string, int> stdSort = new Dictionary<string, int>();
                stdSort.Add("CT", 1);
                stdSort.Add("MR", 2);
                stdSort.Add("CR", 3);
                stdSort.Add("PT", 4);
                stdSort.Add("NM", 5);
                stdSort.Add("ES", 6);
                stdSort.Add("XA", 7);
                stdSort.Add("US", 8);
                stdSort.Add("MG", 9);
                stdSort.Add("OT", 10);
                stdSort.Add("DX", 11);
                stdSort.Add("RF", 12);

                return stdSort;
            }
        }

        /// <summary>
        /// 폴더사이즈 체크 시간
        /// </summary>
        public static int FolderSizeCheckTime = 5;

        /// <summary>
        /// 프로그램 폴더
        /// </summary>
        public static string ProgramExecuteFolder = Environment.CurrentDirectory;

        /// <summary>
        /// 굽기진행중 로그 파일명 Prefix
        /// </summary>
        public const string PFX_TRC = "TRC_";
        /// <summary>
        /// 오류 로그 파일명
        /// </summary>
        public const string ERR_FL_NM = "ERROR";

        /// <summary>
        /// 서버설정내용 기록파일명
        /// </summary>
        public const string SERVER_CONFIG_FL = "ServerConfig";
        /// <summary>
        /// 서버상태 기록 파일명
        /// </summary>
        public const string SERVER_STATUS_FL = "ServerStatus";

        /// <summary>
        /// 굽기 프로그램 종료 확인 파일명
        /// </summary>
        public const string BURN_JOB_END_FL = "RMDS_EXIT.txt";

        /// <summary>
        /// 환경설정값
        /// </summary>
        public static ConfigEntity configEntity = null;

        /// <summary>
        /// 오더 트래킹 파일명
        /// </summary>
        public static string OrderTrackingFile = "Order.xml";

        /// <summary>
        /// USB 복사 진행중인지 여부
        /// </summary>
        public static bool isCopyingToUSB = false;

        /// <summary>
        /// 아산병원 Tomctech 뷰어 파일위치
        /// </summary>
        public const string TOMTECH_VIEWR_FOLDER = @"C:\TomtechV2";
    }
}
