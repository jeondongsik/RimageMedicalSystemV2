using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace RimageKorea
{
    public class Config
    {
        string HostIP;
        string HostName;
        string HostPort;
        string LocalShareFolder;
        string CDLabelFile;
        string DVDLabelFile;
        string MergeFileFolder;
        string MergeFileServerFolder;
        string ServerNetworkDrive;
        string DicomDownloadFolder;
        string HospitalName;
        string LogFilePath;
        string LabelPrint;
        string MergePrint;
        string LocalSourceFolder;
        string SourceFolder;
        string CDMaxSize;
        string programType;
        string startProgram;
        string CDPrintYN;
        string MsgBoxShow;
        string CDRemainQty;
        string RibbonRemainQty;
        string BinCheckTime;
        string FolderPattern;
        string RecorderCnt;
        string AutoExecute;
        string ReportView;
        string DelType;
        string AutoCloseType;
        string FilePath;
        string Message;
        string DeleteAfterBurn;
        string DownloadCheckbyFileName;
        string DelFolderWhenClose;
        string RetentionPeriod;
        string ServerType;
        string AutoExecuteHookingType;
        string ProgramCaption;
        string BurnPatientKind;
        //2015.09.06 add
        string PopUpAlamYN;
        string PopUpKeepTime;
        string PopUpSelPatInfoYN;
        string HookSleepTime1;
        string HookSleepTime2;
        string ManualExeBtnView;
        string CloseConfirm;
        string SoundAlam;
        //2016.07.09 add
        string ExistDicomDir;
        string FileCopyYN;
        string ErrorPupupUse;
        string AutoPrintApp;
        string BottomImagePath;
        //2016.12.24 add
        string DeleteInJobFolder;
        //2018.02.15 add
        string DownCheckFileName;
        //2018.02.20 add
        string DisableMultiPatient;
        /// <summary>
        /// 폴더사이즈 체크 시간(초)
        /// </summary>
        string FolderSizeCheckTime;
        string DvdMaxSize;
        /// <summary>
        /// USB 복사 사용
        /// </summary>
        string UseUSBCopy;
        /// <summary>
        /// 서버명을 IP로 표시
        /// </summary>
        string DisplayServeIP;
        /// <summary>
        /// 조회시 다운로드 폴더 용량 체크하기
        /// </summary>
        string IsSizeCheck;

        XmlDocument doc = new XmlDocument();

        #region Properties        
        /// <summary>
        /// 메시지
        /// </summary>
        public string _Message
        {
            get { return Message; }
            set { this.Message = value; }
        }
        /// <summary>
        /// 서버 아이피주소
        /// </summary>
        public string _HostIP
        {
            get { return getConfig("HostIP", ""); }
            set { this.HostIP = value; }
        }
        /// <summary>
        /// 서버명
        /// </summary>
        public string _HostName
        {
            get { return getConfig("HostName", ""); }
            set { HostName = value; }
        }
        /// <summary>
        /// 접속 Port
        /// </summary>
        public string _HostPort
        {
            get { return getConfig("HostPort", ""); }
            set { this.HostPort = value; }
        }
        /// <summary>
        /// 로컬컴퓨터 작업 폴더(공유되어 있어야 함)
        /// </summary>
        public string _LocalShareFolder
        {
            get { return getConfig("LocalShareFolder", ""); }
            set { this.LocalShareFolder = value; }
        }
        /// <summary>
        /// CD 라벨 파일 경로
        /// </summary>
        public string _CDLabelFile
        {
            get { return getConfig("CDLabelFile", ""); }
            set { this.CDLabelFile = value; }
        }
        /// <summary>
        /// DVD 라벨 파일 경로
        /// </summary>
        public string _DVDLabelFile
        {
            get { return getConfig("DVDLabelFile", ""); }
            set { this.DVDLabelFile = value; }
        }
        /// <summary>
        /// 머지파일이 생성될 폴더(서버쪽)
        /// </summary>
        public string _MergeFileFolder
        {
            get { return getConfig("MergeFileFolder", ""); }
            set { this.MergeFileFolder = value; }
        }
        /// <summary>
        /// 머지파일 경로
        /// </summary>
        public string _MergeFileServerFolder
        {
            get { return getConfig("MergeFileServerFolder", ""); }
            set { this.MergeFileServerFolder = value; }
        }
        /// <summary>
        /// 서버쪽에서 설정된 내컴퓨터 네트워크드라이브명
        /// </summary>
        public string _ServerNetworkDrive
        {
            get { return getConfig("ServerNetworkDrive", ""); }
            set { this.ServerNetworkDrive = value; }
        }
        /// <summary>
        /// 내컴퓨터에서 설정한 서버 네트워크드라이브명
        /// </summary>
        public string _DicomDownloadFolder
        {
            get { return getConfig("DicomDownloadFolder", ""); }
            set { this.DicomDownloadFolder = value; }
        }
        /// <summary>
        /// 병원명
        /// </summary>
        public string _HospitalName
        {
            get { return getConfig("HospitalName", ""); }
            set { this.HospitalName = value; }
        }
        /// <summary>
        /// 로고파일경로
        /// </summary>
        public string _LogFilePath
        {
            get { return getConfig("LogFilePath", ""); }
            set { this.LogFilePath = value; }
        }
        /// <summary>
        /// 라벨프린트 사용여부
        /// </summary>
        public string _LabelPrint
        {
            get { return getConfig("LabelPrint", ""); }
            set { this.LabelPrint = value; }
        }
        /// <summary>
        /// 머지파일 출력내용
        /// </summary>
        public string _MergePrint
        {
            get { return getConfig("MergePrint", ""); }
            set { this.MergePrint = value; }
        }
        /// <summary>
        /// 읽은 CD가 존재하는 폴더 : 공유 -> 서버일경우 SourceFolder폴더와 동일
        /// </summary>
        public string _LocalSourceFolder
        {
            get { return getConfig("LocalSourceFolder", ""); }
            set { this.LocalSourceFolder = value; }
        }
        /// <summary>
        /// 읽은 CD가 존재하는 폴더 : 로컬
        /// </summary>
        public string _SourceFolder
        {
            get { return getConfig("SourceFolder", ""); }
            set { this.SourceFolder = value; }
        }
        /// <summary>
        /// CD 한장에 구울수 있는 최대용량
        /// </summary>
        public string _CDMaxSize
        {
            get { return getConfig("CDMaxSize", ""); }
            set { this.CDMaxSize = value; }
        }
        /// <summary>
        /// 굽기 프로그램 선택 (Ver 1 or 2)
        /// </summary>
        public string _programType
        {
            get { return getConfig("programType", ""); }
            set { this.programType = value; }
        }
        /// <summary>
        /// 시작 프로그램 (1:읽기/굽기, 2:읽기, 3:굽기)
        /// </summary>
        public string _startProgram
        {
            get { return getConfig("startProgram", ""); }
            set { this.startProgram = value; }
        }
        /// <summary>
        /// CD 라벨 프린트 여부
        /// </summary>
        public string _CDPrintYN
        {
            get { return getConfig("CDPrintYN", ""); }
            set { this.CDPrintYN = value; }
        }

        /// <summary>
        /// CD,Ribbon 최소보유수량 이하 일 경우 Message Box 띄울것인지 여부 (0~3)
        /// </summary>
        public string _MsgBoxShow
        {
            get { return getConfig("MsgBoxShow", ""); }
            set { MsgBoxShow = value; }
        }
        /// <summary>
        /// CD 최소보유량
        /// </summary>
        public string _CDRemainQty
        {
            get { return getConfig("CDQty", ""); }
            set { this.CDRemainQty = value; }
        }
        /// <summary>
        /// 리본 최소보유량 
        /// </summary>
        public string _RibbonRemainQty
        {
            get { return getConfig("RibbonQty", ""); }
            set { this.RibbonRemainQty = value; }
        }
        /// <summary>
        /// 빈체크시간간격
        /// </summary>
        public string _BinCheckTime
        {
            get { return getConfig("BinCheckTime", ""); }
            set { this.BinCheckTime = value; }
        }
        /// <summary>
        /// 생성 폴더 형태
        /// </summary>
        public string _FolderPattern
        {
            get { return getConfig("FolderPattern", ""); }
            set { this.FolderPattern = value; }
        }
        /// <summary>
        /// 레코더 갯수
        /// </summary>
        public string _RecorderCnt
        {
            get { return getConfig("RecorderCnt", ""); }
            set { RecorderCnt = value; }
        }
        /// <summary>
        /// 굽기 자동실행 여부
        /// </summary>
        public string _AutoExecute
        {
            get { return getConfig("AutoExecute", ""); }
            set { this.AutoExecute = value; }
        }
        /// <summary>
        /// 굽기 자동실행 여부
        /// </summary>
        public string _AutoExecuteHookingType
        {
            get { return getConfig("AutoExecuteHookingType", ""); }
            set { this.AutoExecuteHookingType = value; }
        }
        /// <summary>
        /// 리포트보기 Y/N
        /// </summary>
        public string _ReportView
        {
            get { return getConfig("ReportView", ""); }
            set { this.ReportView = value; }
        }
        /// <summary>
        /// 삭제유형 - 0:완전삭제, 1:휴지통으로, 2:삭제안함.
        /// </summary>
        public string _DelType
        {
            get { return getConfig("DelType", ""); }
            set { this.DelType = value; }
        }
        /// <summary>
        /// 마로팍스에서 success 창만 닫기
        /// </summary>
        public string _AutoCloseType
        {
            get { return getConfig("AutoCloseType", ""); }
            set { this.AutoCloseType = value; }
        }
        /// <summary>
        /// 굽기완료 후 폴더 처리 (0.삭제안함, 1.성공했을때만 삭제, 2.성공,실패 모두 삭제)
        /// </summary>
        public string _DeleteAfterBurn
        {
            get { return getConfig("DeleteAfterBurn", ""); }
            set { this.DeleteAfterBurn = value; }
        }
        /// <summary>
        /// 다운로드 완료를 "end.txt" 파일로 체크할지 여부
        /// </summary>
        public string _DownloadCheckbyFileName
        {
            get { return getConfig("DownloadCheckbyFileName", ""); }
            set { this.DownloadCheckbyFileName = value; }
        }
        /// <summary>
        /// 프로그램 종료시 폴더 삭제할 지 여부
        /// </summary>
        public string _DelFolderWhenClose
        {
            get { return getConfig("DelFolderWhenClose", ""); }
            set { this.DelFolderWhenClose = value; }
        }
        /// <summary>
        /// 명령내역 및 환자폴더 보관기간
        /// </summary>
        public string _RetentionPeriod
        {
            get { return getConfig("RetentionPeriod", ""); }
            set { this.RetentionPeriod = value; }
        }
        /// <summary>
        /// 서버유형 : R.원격서버, L.로컬서버
        /// </summary>
        public string _ServerType
        {
            get { return getConfig("ServerType", ""); }
            set { this.ServerType = value; }
        }
        /// <summary>
        /// 프로그램 캡션
        /// </summary>
        public string _ProgramCaption
        {
            get { return getConfig("ProgramCaption", ""); }
            set { this.ProgramCaption = value; }
        }
        /// <summary>
        /// 굽기대상 환자구분(단일,다중)
        /// </summary>
        public string _BurnPatientKind
        {
            get { return getConfig("BurnPatientKind", ""); }
            set { this.BurnPatientKind = value; }
        }
        /// <summary>
        /// 굽기결과 팝업알람창 띄우기(Y/N)
        /// </summary>
        public string _PopUpAlamYN
        {
            get { return getConfig("PopUpAlamYN", ""); }
            set { this.PopUpAlamYN = value; }
        }
        /// <summary>
        /// 굽기결과 팝업알람창 유지시간 (초)
        /// </summary>
        public string _PopUpKeepTime
        {
            get { return getConfig("PopUpKeepTime", ""); }
            set { this.PopUpKeepTime = value; }
        }
        /// <summary>
        /// 멀티 환자 선택 수정창 팝업여부(Y/N)
        /// </summary>
        public string _PopUpSelPatInfoYN
        {
            get { return getConfig("PopUpSelPatInfoYN", ""); }
            set { this.PopUpSelPatInfoYN = value; }
        }
        /// <summary>
        /// 후킹후 Sleeptime : ProgramType1
        /// </summary>
        public string _HookSleepTime1
        {
            get { return getConfig("HookSleepTime1", ""); }
            set { this.HookSleepTime1 = value; }
        }
        /// <summary>
        /// 후킹후 Sleeptime : ProgramType2
        /// </summary>
        public string _HookSleepTime2
        {
            get { return getConfig("HookSleepTime2", ""); }
            set { this.HookSleepTime2 = value; }
        }
        /// <summary>
        /// 수동실행 버튼 보이기 여부
        /// </summary>
        public string _ManualExeBtnView
        {
            get { return getConfig("ManualExeBtnView", ""); }
            set { this.ManualExeBtnView = value; }
        }
        /// <summary>
        /// 프로그램 종료시 확인 메시지 띄우기
        /// </summary>
        public string _CloseConfirm
        {
            get { return getConfig("CloseConfirm", ""); }
            set { this.CloseConfirm = value; }
        }
        /// <summary>
        /// 굽기완료 후 소리로 알리기
        /// </summary>
        public string _SoundAlam
        {
            get { return getConfig("SoundAlam", ""); }
            set { this.SoundAlam = value; }
        }
        /// <summary>
        /// Dicomdir 파일 존재 여부 체크
        /// </summary>
        public string _ExistDicomDir
        {
            get { return getConfig("ExistDicomDir", ""); }
            set { this.ExistDicomDir = value; }
        }
        /// <summary>
        /// 굽기전 파일 복사 여부
        /// </summary>
        public string _FileCopyYN
        {
            get { return getConfig("FileCopyYN", ""); }
            set { this.FileCopyYN = value; }
        }
        /// <summary>
        /// 에러 발생시 팝업창으로 보여주기
        /// </summary>
        public string _ErrorPupupUse
        {
            get { return getConfig("ErrorPupupUse", ""); }
            set { this.ErrorPupupUse = value; }
        }
        /// <summary>
        /// 복사 신청서 자동 프린트 여부
        /// </summary>
        public string _AutoPrintApp
        {
            get { return getConfig("AutoPrintApp", ""); }
            set { this.AutoPrintApp = value; }
        }
        /// <summary>
        /// 복사 신청서 하단 이미지 경로
        /// </summary>
        public string _BottomImagePath
        {
            get { return getConfig("BottomImagePath", ""); }
            set { this.BottomImagePath = value; }
        }
        /// <summary>
        /// 프로그램시작시 작업폴더 내 내용 삭제 여부
        /// </summary>
        public string _DeleteInJobFolder
        {
            get { return getConfig("DeleteInJobFolder", ""); }
            set { this.DeleteInJobFolder = value; }
        }
        /// <summary>
        /// 다운로드 완료 체크 파일명
        /// </summary>
        public string _DownCheckFileName
        {
            get { return getConfig("DownCheckFileName", ""); }
            set { this.DownCheckFileName = value; }
        }
        /// <summary>
        /// 다중환자 사용여부
        /// </summary>
        public string _DisableMultiPatient
        {
            get { return getConfig("DisableMultiPatient", ""); }
            set { this.DisableMultiPatient = value; }
        }
        /// <summary>
        /// 폴더사이즈 체크 시간(초)
        /// </summary>
        public string _FolderSizeCheckTime
        {
            get { return getConfig("FolderSizeCheckTime", ""); }
            set { this.FolderSizeCheckTime = value; }
        }
        /// <summary>
        /// DVD 최대크기
        /// </summary>
        public string _DvdMaxSize
        {
            get { return getConfig("DvdMaxSize", ""); }
            set { this.DvdMaxSize = value; }
        }
        /// <summary>
        /// USB 복사 사용 여부
        /// </summary>
        public string _UseUSBCopy
        {
            get { return getConfig("UseUSBCopy", ""); }
            set { this.UseUSBCopy = value; }
        }
        /// <summary>
        /// 서버명을 IP로 표시
        /// </summary>
        public string _DisplayServeIP
        {
            get { return getConfig("DisplayServeIP", ""); }
            set { this.DisplayServeIP = value; }
        }

        /// <summary>
        /// 조회시 다운로드 폴더 용량 체크하기
        /// </summary>
        public string _IsSizeCheck
        {
            get { return getConfig("IsSizeCheck", ""); }
            set { this.IsSizeCheck = value; }
        }
        
        #endregion

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="xmlPath"></param>
        public Config(string xmlPath)
        {
            if (File.Exists(Path.Combine(xmlPath, "config.xml")))
            {
                this.FilePath = xmlPath;
                this.Message = "정상";
            }
            else
            {
                this.Message = "환경설정 파일이 없습니다.";
            }
        }

        /// <summary>
        /// 설정파일 읽어오기
        /// </summary>
        /// <param name="nodeName">노드명</param>
        /// <param name="attName">속성명</param>
        /// <returns></returns>
        private string getConfig(string nodeName, string attName)
        {
            string retStr = "";
            XmlDocument doc = new XmlDocument();
            string xmlPath = Path.Combine(this.FilePath, "config.xml");

            doc.Load(xmlPath);
            XmlElement node = (XmlElement)doc.DocumentElement.FirstChild;

            do
            {
                if (node.HasAttributes)
                {
                    foreach (XmlAttribute attr in node.Attributes)
                    {
                        if (node.Name.Equals(nodeName) && attr.Name.Equals(attName))
                        {
                            retStr = attr.Value;
                            break;
                        }
                    }
                }
                else if (node.Name.Equals(nodeName))
                {
                    retStr = node.InnerText;
                    break;
                }

            } while ((node = (XmlElement)node.NextSibling) != null);

            return retStr;
        }

        /// <summary>
        /// 환경설정 저장
        /// </summary>
        /// <param name="whereFrom">all:전체, burn:굽기, read:읽기, burn_start:초기설정</param>
        public void setConfig(string whereFrom)
        {
            string xmlPath = Path.Combine(this.FilePath, "config.xml");
            doc.Load(xmlPath);

            //공통
            updateConfigXml("HostIP", this.HostIP);
            updateConfigXml("HostName", this.HostName);
            updateConfigXml("HostPort", this.HostPort);

            //굽기
            if (whereFrom.Equals("all") || whereFrom.Equals("burn"))
            {
                updateConfigXml("LocalShareFolder", this.LocalShareFolder);
                updateConfigXml("CDLabelFile", this.CDLabelFile);
                updateConfigXml("DVDLabelFile", this.DVDLabelFile);
                updateConfigXml("MergeFileFolder", this.MergeFileFolder);
                updateConfigXml("MergeFileServerFolder", this.MergeFileServerFolder);
                updateConfigXml("ServerNetworkDrive", this.ServerNetworkDrive);
                updateConfigXml("DicomDownloadFolder", this.DicomDownloadFolder);
                updateConfigXml("HospitalName", this.HospitalName);
                updateConfigXml("LogFilePath", this.LogFilePath);
                updateConfigXml("LabelPrint", this.LabelPrint);
                updateConfigXml("MergePrint", this.MergePrint);
                updateConfigXml("CDMaxSize", this.CDMaxSize);
                updateConfigXml("CDPrintYN", this.CDPrintYN);
                updateConfigXml("MsgBoxShow", this.MsgBoxShow);
                updateConfigXml("CDQty", this.CDRemainQty);
                updateConfigXml("RibbonQty", this.RibbonRemainQty);
                updateConfigXml("BinCheckTime", this.BinCheckTime);
                updateConfigXml("FolderPattern", this.FolderPattern);
                updateConfigXml("AutoExecute", this.AutoExecute);
                updateConfigXml("AutoExecuteHookingType", this.AutoExecuteHookingType);
                updateConfigXml("ReportView", this.ReportView);
                updateConfigXml("DelType", this.DelType);
                updateConfigXml("AutoCloseType", this.AutoCloseType);
                updateConfigXml("DeleteAfterBurn", this.DeleteAfterBurn);
                updateConfigXml("DownloadCheckbyFileName", this.DownloadCheckbyFileName);
                updateConfigXml("DelFolderWhenClose", this.DelFolderWhenClose);
                updateConfigXml("RetentionPeriod", this.RetentionPeriod);
                updateConfigXml("ProgramCaption", this.ProgramCaption);
                updateConfigXml("BurnPatientKind", this.BurnPatientKind);
                updateConfigXml("PopUpAlamYN", this.PopUpAlamYN);
                updateConfigXml("PopUpKeepTime", this.PopUpKeepTime);
                updateConfigXml("PopUpSelPatInfoYN", this.PopUpSelPatInfoYN);
                updateConfigXml("HookSleepTime1", this.HookSleepTime1);
                updateConfigXml("HookSleepTime2", this.HookSleepTime2);
                updateConfigXml("ManualExeBtnView", this.ManualExeBtnView);
                updateConfigXml("CloseConfirm", this.CloseConfirm);
                updateConfigXml("SoundAlam", this.SoundAlam);
                updateConfigXml("ExistDicomDir", this.ExistDicomDir);
                updateConfigXml("FileCopyYN", this.FileCopyYN);
                updateConfigXml("ErrorPupupUse", this.ErrorPupupUse);
                updateConfigXml("AutoPrintApp", this.AutoPrintApp);
                updateConfigXml("BottomImagePath", this.BottomImagePath);
                updateConfigXml("DeleteInJobFolder", this.DeleteInJobFolder);
                updateConfigXml("DownCheckFileName", this.DownCheckFileName);
                updateConfigXml("DisableMultiPatient", this.DisableMultiPatient);
                updateConfigXml("FolderSizeCheckTime", this.FolderSizeCheckTime);
                updateConfigXml("DvdMaxSize", this.DvdMaxSize);
                updateConfigXml("UseUSBCopy", this.UseUSBCopy);
                updateConfigXml("DisplayServeIP", this.DisplayServeIP);
                updateConfigXml("IsSizeCheck", this.IsSizeCheck);
            }

            //초기설정
            if (whereFrom.Equals("all") || whereFrom.Equals("burn_start"))
            {
                updateConfigXml("MergeFileFolder", this.MergeFileFolder);
            }

            //읽기
            if (whereFrom.Equals("all") || whereFrom.Equals("read"))
            {
                updateConfigXml("LocalSourceFolder", this.LocalSourceFolder);
                updateConfigXml("SourceFolder", this.SourceFolder);
            }

            //
            if (whereFrom.Equals("all"))
            {
                updateConfigXml("programType", this.programType);
                updateConfigXml("startProgram", this.startProgram);
                updateConfigXml("RecorderCnt", this.RecorderCnt);
            }

            doc.Save(xmlPath);
        }

        /// <summary>
        /// XML 저장/수정
        /// </summary>
        /// <param name="nodename"></param>
        /// <param name="newvalue"></param>
        private void updateConfigXml(string nodename, string newvalue)
        {
            try
            {
                XmlNode node;
                node = doc.DocumentElement;
                bool modifyYN = false;

                foreach (XmlNode node1 in node.ChildNodes)
                {
                    if (node1.Name.Equals(nodename))
                    {
                        node1.InnerText = newvalue;
                        modifyYN = true;
                        break;
                    }
                }

                if (modifyYN == false)
                {
                    node.AppendChild(this.CreateNode(doc, nodename, newvalue));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Xml 노드 생성
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private XmlNode CreateNode(XmlDocument xmlDoc, string name, string value)
        {
            XmlNode node = xmlDoc.CreateElement(string.Empty, name, string.Empty);
            node.InnerXml = value;
            return node;
        }

        /// <summary>
        /// 최종 접속한 서버정보를 저장한다.
        /// </summary>
        public void setConfigServerInfo(string hostIP, string hostName, string hostPort)
        {
            string xmlPath = Path.Combine(this.FilePath, "config.xml");
            doc.Load(xmlPath);

            //공통
            updateConfigXml("HostIP", hostIP);
            updateConfigXml("HostName", hostName);
            updateConfigXml("HostPort", hostPort);

            doc.Save(xmlPath);
        }

        /// <summary>
        /// 서버유형 세팅
        /// </summary>
        /// <param name="serverType"></param>
        public void setServerType(string serverType)
        {
            string xmlPath = Path.Combine(this.FilePath, "config.xml");
            doc.Load(xmlPath);

            updateConfigXml("ServerType", serverType);
            doc.Save(xmlPath);
        }
    }
}
