using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RimageKorea
{
    /// <summary>
    /// 환경설정 Entity
    /// </summary>
    public class ConfigEntity
    {
        public string HostIP { get; set; }
        public string HostName { get; set; }
        public string HostPort { get; set; }
        /// <summary>
        /// 로컬컴퓨터 작업 폴더(공유되어 있어야 함)
        /// </summary>
        public string LocalShareFolder { get; set; }
        /// <summary>
        /// CD 라벨 파일 경로
        /// </summary>
        public string CDLabelFile { get; set; }
        /// <summary>
        /// DVD 라벨 파일 경로
        /// </summary>
        public string DVDLabelFile { get; set; }
        /// <summary>
        /// 머지파일이 생성될 폴더(서버쪽)
        /// </summary>
        public string MergeFileFolder { get; set; }
        /// <summary>
        /// 머지파일 경로
        /// </summary>
        public string MergeFileServerFolder { get; set; }
        /// <summary>
        /// 서버쪽에서 설정된 내컴퓨터 네트워크드라이브명
        /// </summary>
        public string ServerNetworkDrive { get; set; }
        /// <summary>
        /// 내컴퓨터에서 설정한 서버 네트워크드라이브명
        /// </summary>
        public string DicomDownloadFolder { get; set; }
        /// <summary>
        /// 병원명
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 로고파일경로
        /// </summary>
        public string LogFilePath { get; set; }
        /// <summary>
        /// 라벨프린트 사용여부
        /// </summary>
        public string LabelPrint { get; set; }
        /// <summary>
        /// 머지파일 출력내용
        /// </summary>
        public string MergePrint { get; set; }
        /// <summary>
        /// 읽은 CD가 존재하는 폴더 : 공유 -> 서버일경우 SourceFolder폴더와 동일
        /// </summary>
        public string LocalSourceFolder { get; set; }
        /// <summary>
        /// 읽은 CD가 존재하는 폴더 : 로컬
        /// </summary>
        public string SourceFolder { get; set; }
        /// <summary>
        /// CD 한장에 구울수 있는 최대용량
        /// </summary>
        public string CDMaxSize { get; set; }
        /// <summary>
        /// 굽기 프로그램 선택 (Ver 1 or 2)
        /// </summary>
        public string programType { get; set; }
        /// <summary>
        /// 시작 프로그램 (1:읽기/굽기, 2:읽기, 3:굽기)
        /// </summary>
        public string startProgram { get; set; }
        /// <summary>
        /// CD 라벨 프린트 여부
        /// </summary>
        public string CDPrintYN { get; set; }
        /// <summary>
        /// CD,Ribbon 최소보유수량 이하 일 경우 Message Box 띄울것인지 여부 (0~3)
        /// </summary>
        public string MsgBoxShow { get; set; }
        /// <summary>
        /// CD 최소보유량
        /// </summary>
        public int CDRemainQty { get; set; }
        /// <summary>
        /// 리본 최소보유량 
        /// </summary>
        public int RibbonRemainQty { get; set; }
        /// <summary>
        /// 빈체크시간간격
        /// </summary>
        public int BinCheckTime { get; set; }
        /// <summary>
        /// 생성 폴더 형태
        /// </summary>
        public string FolderPattern { get; set; }
        /// <summary>
        /// 레코더 갯수
        /// </summary>
        public string RecorderCnt { get; set; }
        /// <summary>
        /// 굽기 자동실행 여부
        /// </summary>
        public string AutoExecute { get; set; }
        /// <summary>
        /// 리포트보기 Y/N
        /// </summary>
        public string ReportView { get; set; }
        /// <summary>
        /// 삭제유형 - 0:완전삭제, 1:휴지통으로, 2:삭제안함.
        /// </summary>
        public string DelType { get; set; }
        /// <summary>
        /// 마로팍스에서 success 창만 닫기
        /// </summary>
        public string AutoCloseType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 굽기완료 후 폴더 처리 (0.삭제안함, 1.성공했을때만 삭제, 2.성공,실패 모두 삭제)
        /// </summary>
        public string DeleteAfterBurn { get; set; }
        /// <summary>
        /// 다운로드 완료를 "end.txt" 파일로 체크할지 여부
        /// </summary>
        public string DownloadCheckbyFileName { get; set; }
        /// <summary>
        /// 프로그램 종료시 폴더 삭제할 지 여부
        /// </summary>
        public string DelFolderWhenClose { get; set; }
        /// <summary>
        /// 명령내역 및 환자폴더 보관기간
        /// </summary>
        public int RetentionPeriod { get; set; }
        /// <summary>
        /// 서버유형 : R.원격서버, L.로컬서버
        /// </summary>
        public string ServerType { get; set; }
        /// <summary>
        /// 후킹 유형
        /// </summary>
        public string AutoExecuteHookingType { get; set; }
        /// <summary>
        /// 프로그램 캡션
        /// </summary>
        public string ProgramCaption { get; set; }
        /// <summary>
        /// 굽기대상 환자구분(단일,다중)
        /// </summary>
        public string BurnPatientKind { get; set; }
        /// <summary>
        /// 굽기결과 팝업알람창 띄우기(Y/N)
        /// </summary>
        public string PopUpAlamYN { get; set; }
        /// <summary>
        /// 굽기결과 팝업알람창 유지시간 (초)
        /// </summary>
        public string PopUpKeepTime { get; set; }
        /// <summary>
        /// 멀티 환자 선택 수정창 팝업여부(Y/N)
        /// </summary>
        public string PopUpSelPatInfoYN { get; set; }
        /// <summary>
        /// 후킹후 Sleeptime : ProgramType1
        /// </summary>
        public int HookSleepTime1 { get; set; }
        /// <summary>
        /// 후킹후 Sleeptime : ProgramType2
        /// </summary>
        public int HookSleepTime2 { get; set; }
        /// <summary>
        /// 수동실행 버튼 보이기 여부
        /// </summary>
        public string ManualExeBtnView { get; set; }
        /// <summary>
        /// 프로그램 종료시 확인 메시지 띄우기
        /// </summary>
        public string CloseConfirm { get; set; }
        /// <summary>
        /// 굽기완료 후 소리로 알리기
        /// </summary>
        public string SoundAlam { get; set; }
        /// <summary>
        /// Dicomdir 파일 존재 여부 체크
        /// </summary>
        public string ExistDicomDir { get; set; }
        /// <summary>
        /// 굽기전 파일 복사 여부
        /// </summary>
        public string FileCopyYN { get; set; }
        /// <summary>
        /// 에러 발생시 팝업창으로 보여주기
        /// </summary>
        public string ErrorPupupUse { get; set; }
        /// <summary>
        /// 복사 신청서 자동 프린트 여부
        /// </summary>
        public string AutoPrintApp { get; set; }
        /// <summary>
        /// 복사 신청서 하단 이미지 경로
        /// </summary>
        public string BottomImagePath { get; set; }
        /// <summary>
        /// 프로그램시작시 작업폴더 내 내용 삭제 여부
        /// </summary>
        public string DeleteInJobFolder { get; set; }
        /// <summary>
        /// 다운로드 완료 체크 파일명
        /// </summary>
        public string DownCheckFileName { get; set; }
        /// <summary>
        /// 다중환자 사용안함 여부 (Y.사용안함, N.사용)
        /// </summary>
        public string DisableMultiPatient { get; set; }
        /// <summary>
        /// 폴더사이즈 체크 시간(초)
        /// </summary>
        public string FolderSizeCheckTime { get; set; }
        /// <summary>
        /// DVD 최대크기
        /// </summary>
        public string DvdMaxSize { get; set; }
        /// <summary>
        /// CD 프린트 여부
        /// </summary>
        public bool UseLabelPrint { get; set; }
        /// <summary>
        /// USB 복사 사용
        /// </summary>
        public string UseUSBCopy { get; set; }
    }
}
