using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RimageKorea;
using DicomReader;

namespace RimageMedicalSystemV2
{
    public class SearchPatient
    {
        /// <summary>
        /// 폴더에서 환자 정보 조회하기
        /// </summary>
        /// <param name="sdir"></param>        
        /// <param name="existDicomDir"></param>
        /// <param name="folderPattern">생성되는 환자 폴더 형태</param>
        /// <param name="DBConnInfo"></param>
        /// <param name="checkFile"></param>
        /// <param name="cdMaxSize"></param>
        /// <param name="dvdMaxSize"></param>
        /// <param name="autoLoaderMediaType"></param>
        /// <returns></returns>
        public static BurnOrderedInfoEntity Get(string programPath, DirectoryInfo sdir,
            Dictionary<string, string> DBConnInfo, string autoLoaderMediaType, string checkFile = "")
        {
            try
            {
                string mdbFileName = "PiView.mdb";
                string mdbFileNameMaro = @"Viewer\Export.mdb";

                bool existCheckFile = true;
                bool isBurnEnd = true;
                bool existPatInfoFile = true;
                bool isDicom = false;
                bool isMdb = false;
                bool isNoDicom = false;
                bool isText = false;
                bool isINI = false;
                bool isXml = false;

                //// root에 checkFile ("end.txt") 파일이 존재하는지 체크하자.
                if (checkFile != "")
                {
                    existCheckFile = CheckFileExists(sdir, checkFile);
                }

                //// 다운로드 완료 파일로 체크시 파일이 존재할 경우 (기본은 true) 
                if (existCheckFile)
                {
                    //// 굽기완료된 폴더인지 체크(burn.end 파일체크)
                    //// 완료된 상태이면 True
                    isBurnEnd = CheckFileExists(sdir, GlobalVar.BURN_CHK_FL_NM);
                }

                //// 조회조건 모두 통과시 (
                if (existCheckFile && !isBurnEnd)
                {
                    //// 굽기 정보
                    BurnOrderedInfoEntity orderInfo = new BurnOrderedInfoEntity();
                    orderInfo.patDate = DateTime.Now.ToString("yyyy-MM-dd");
                    orderInfo.LocalShareFolder = sdir.Name;
                    orderInfo.patFolder = sdir.Name;
                    orderInfo.patFolderFullPath = sdir.FullName;
                    orderInfo.DeleteIcon = global::RimageMedicalSystemV2.Properties.Resources.close_16x16;
                    orderInfo.copies = 1;
                    orderInfo.BurnPatientKind = "N";

                    string pID = string.Empty;
                    string pName = string.Empty;
                    string pSex = string.Empty;
                    string pAge = string.Empty;
                    string pModality = string.Empty;
                    Dictionary<string, string> pList = null;
                    Dictionary<string, string> dicomdirInfo = null;
                    Dictionary<string, string> patList = null;
                    int patCount = 0;

                    //// 먼저 DicomDir 파일이 존재하는지 체크
                    if (GlobalVar.configEntity.ExistDicomDir == "Y")
                    {
                        if (File.Exists(Path.Combine(sdir.FullName, "DICOMDIR")))
                        {
                            existPatInfoFile = true;
                        }
                        else
                        {
                            existPatInfoFile = false;
                        }

                        //// 존재하지 않으면 Pass
                        if (!existPatInfoFile)
                            return null;
                    }

                    //// 다시 한번 체크 >> 환자정보가 담긴 파일이 존재하지 않을 경우 Pass 
                    if (GlobalVar.configEntity.ExistDicomDir == "Y")
                    {
                        //// 환자정보가 담긴 파일이 존재하지 않을 경우 Pass 
                        if (File.Exists(Path.Combine(sdir.FullName, "DICOMDIR")) ||
                            File.Exists(Path.Combine(sdir.FullName, mdbFileName)) ||
                            File.Exists(Path.Combine(sdir.FullName, mdbFileNameMaro)) ||
                            File.Exists(Path.Combine(sdir.FullName, GlobalVar.CD_INFO_FILE)) ||
                            File.Exists(Path.Combine(sdir.FullName, GlobalVar.CD_STUDY_INFO_FILE)))
                        {
                            existPatInfoFile = true;
                        }
                        else
                        {
                            existPatInfoFile = false;
                        }

                        //// 존재하지 않으면 Pass
                        if (!existPatInfoFile)
                            return null;
                    }
                    
                    //// MDB 파일이 존재하면 우선 MDB파일에서 정보를 가져온다.
                    isMdb = false;
                    if (File.Exists(Path.Combine(sdir.FullName, mdbFileName)))
                    {
                        #region -- MDB 파일에서 읽기 --
                        GetPatientInfoFromMDB cls = new GetPatientInfoFromMDB(Path.Combine(sdir.FullName, mdbFileName));
                        dicomdirInfo = new Dictionary<string, string>();
                        dicomdirInfo = cls.GetInfo();

                        if (dicomdirInfo != null)
                        {
                            pID = dicomdirInfo["ID"];
                            pName = dicomdirInfo["Name"];
                            pSex = dicomdirInfo["SexKr"];
                            pAge = dicomdirInfo["Age"];

                            pList = cls.PatientLST;
                            patCount = cls.PatientCount;

                            isMdb = true;
                        }
                        #endregion
                    }
                    else if (File.Exists(Path.Combine(sdir.FullName, mdbFileNameMaro)))    //Maro형태에서..
                    {
                        #region -- MDB 파일에서 읽기 --
                        GetPatientInfoFromMDB2 cls2 = new GetPatientInfoFromMDB2(Path.Combine(sdir.FullName, mdbFileNameMaro));
                        dicomdirInfo = new Dictionary<string, string>();
                        dicomdirInfo = cls2.GetInfo();

                        if (dicomdirInfo != null)
                        {
                            pID = dicomdirInfo["ID"];
                            pName = dicomdirInfo["Name"];
                            pSex = dicomdirInfo["SexKr"];
                            pAge = dicomdirInfo["Age"];
                            pModality = dicomdirInfo["Modality"];

                            pList = cls2.PatientLST;
                            patCount = cls2.PatientCount;

                            isMdb = true;
                        }
                        #endregion
                    }

                    //// 환자명 정보가 담긴 파일이 존재하면 거기서 정보를 가져오자
                    if (File.Exists(Path.Combine(sdir.FullName, GlobalVar.CD_INFO_FILE)))
                    {
                        GetPatientInfoFromTextFile cls = new GetPatientInfoFromTextFile();
                        cls.GetInfo(Path.Combine(sdir.FullName, GlobalVar.CD_INFO_FILE));

                        pID = cls.ID;
                        pName = cls.Name;
                        pSex = cls.Sex;

                        if (Utils.IsHangul(cls.Name) == false)
                            pName = string.Empty;

                        isText = true;
                    }

                    //// DICOMDIR 파일에서 읽기
                    isDicom = false;
                    isNoDicom = false;
                    if (File.Exists(Path.Combine(sdir.FullName, "DICOMDIR")))
                    {
                        #region -- DICOMDIR 파일에서 환자정보를 읽어온다. --
                        //// DICOMDIR에서 가져온다.
                        dicomdirInfo = new Dictionary<string, string>();
                        List<string> dicImgList = null;
                        PatientList patInfor = null;
                        dicomdirInfo = GetPatient(sdir.FullName, GlobalVar.configEntity.DisableMultiPatient, out dicImgList, out patInfor, out patList);

                        orderInfo.PatInfor = patInfor;

                        if (dicomdirInfo != null && dicomdirInfo["Result"] != "NotFound")
                        {
                            orderInfo.patNo = dicomdirInfo["ID"];
                            orderInfo.patName = dicomdirInfo["Name"];
                            orderInfo.patSex = dicomdirInfo["SexKr"];
                            orderInfo.patAge = dicomdirInfo["Age"];
                            orderInfo.patBirtyDay = dicomdirInfo["BirthDay"];
                            orderInfo.Modality = dicomdirInfo["Modality"];
                            orderInfo.StudyDescription = dicomdirInfo["StudyDesc"];
                            orderInfo.DicomDescription = dicomdirInfo["DicomDesc"];
                            orderInfo.StudyModality = dicomdirInfo["StudyModality"];
                            orderInfo.Count = Convert.ToInt32(dicomdirInfo["PatientCount"]);
                            orderInfo.patList = patList;
                            orderInfo.DicomImgList = dicImgList;

                            //// 환자나이가 0이거나 빈값이면 생일로 나이 계산한다.
                            if (string.IsNullOrWhiteSpace(orderInfo.patAge) || orderInfo.patAge.Trim() == "0")
                            {
                                orderInfo.patAge = Utils.GetPatientAge(orderInfo.patBirtyDay, orderInfo.patNo).ToString();
                            }

                            isDicom = true;
                        }
                        #endregion
                    }
                    else
                    {
                        //// DicomDir 파일이 없을 시 폴더명에서 환자아이디, 이름만 가져온다.
                        //// 단 13. Compumedics가 아닌 경우                        
                        if (sdir.Name.Contains("_") && GlobalVar.configEntity.AutoExecuteHookingType != "13" && GlobalVar.configEntity.AutoExecuteHookingType != "14")
                        {
                            try
                            {
                                string[] arrStr = sdir.Name.Split('_');
                                if (arrStr.Length > 2)
                                {
                                    orderInfo.patNo = arrStr[1];
                                    orderInfo.patName = arrStr[2];
                                }

                                isNoDicom = true;
                            }
                            catch { }
                        }

                        //// 14.EGG-뇌파의 경우
                        if (GlobalVar.configEntity.AutoExecuteHookingType == "14")
                        {
                            //// 병원서버에서 환자정보 읽어온다.
                            if (sdir.Name.Contains("("))
                            {
                                string ppid = sdir.Name.Substring(0, sdir.Name.IndexOf("("));
                                Dictionary<string, string> eggInfo = GetPatientInfoFromEAI.GetPatInfo(ppid);
                                if (eggInfo != null)
                                {
                                    orderInfo.patNo = eggInfo["PatId"];
                                    orderInfo.patName = eggInfo["PatName"];
                                    orderInfo.patSex = eggInfo["PatSex"];
                                    orderInfo.patAge = eggInfo["Age"];
                                    orderInfo.patBirtyDay = "";

                                    orderInfo.Modality = "EGG";
                                    orderInfo.StudyDescription = "EGG";
                                    orderInfo.DicomDescription = "EGG";
                                    orderInfo.StudyModality = "EGG";
                                }
                                else
                                {
                                    orderInfo.patNo = ppid;
                                    orderInfo.patName = "테스트";
                                    orderInfo.patSex = "남";
                                    orderInfo.patAge = "50";
                                    orderInfo.patBirtyDay = "";

                                    orderInfo.Modality = "EGG";
                                    orderInfo.StudyDescription = "EGG";
                                    orderInfo.DicomDescription = "EGG";
                                    orderInfo.StudyModality = "EGG";
                                }
                            }

                            isNoDicom = true;
                        }
                    }

                    //// MDB파일에서 가져온 데이터가 있으면 대체.
                    if (isMdb == true)
                    {
                        orderInfo.patNo = (string.IsNullOrWhiteSpace(pID)) ? orderInfo.patNo : pID;
                        orderInfo.patName = (string.IsNullOrWhiteSpace(pName)) ? orderInfo.patName : pName;
                        orderInfo.patSex = (string.IsNullOrWhiteSpace(pSex)) ? orderInfo.patSex : pSex;
                        orderInfo.patAge = (string.IsNullOrWhiteSpace(pAge)) ? orderInfo.patAge : pAge;
                        orderInfo.Count = (pList.Count.Equals(0)) ? orderInfo.Count : pList.Count;
                        orderInfo.patList = (pList == null) ? orderInfo.patList : pList;
                    }

                    //// TextFile에서 가져온 데이터가 있으면 대체
                    if (isText == true)
                    {
                        orderInfo.patNo = (string.IsNullOrWhiteSpace(pID)) ? orderInfo.patNo : pID;
                        orderInfo.patName = (string.IsNullOrWhiteSpace(pName)) ? orderInfo.patName : pName;
                        orderInfo.patSex = (string.IsNullOrWhiteSpace(pSex)) ? orderInfo.patSex : pSex;
                    }

                    //// Studyinfo.ini에서 환자아이디,명,나이를 가져올 경우
                    if (File.Exists(Path.Combine(sdir.FullName, "Studyinfo.ini")))
                    {
                        /* [Studyinfo]
                            PatientName=허경례
                            PatientId=15621928
                            PatientAge/Sex=51/F
                         */
                        try
                        {
                            string[] fullText = File.ReadAllLines(Path.Combine(sdir.FullName, "Studyinfo.ini"), Encoding.Default);
                            foreach (string str in fullText)
                            {
                                if (str.Contains("PatientName"))
                                {
                                    orderInfo.patName = str.Substring(str.IndexOf('=') + 1);
                                }
                                if (str.Contains("PatientId"))
                                {
                                    orderInfo.patNo = str.Substring(str.IndexOf('=') + 1);
                                }
                                if (str.Contains("PatientAge"))
                                {
                                    string[] arrTxt = str.Substring(str.IndexOf('=') + 1).Split('/');
                                    orderInfo.patAge = arrTxt[0];
                                    orderInfo.patSex = arrTxt[1];
                                }
                            }

                            isINI = true;
                        }
                        catch { }
                    }

                    //// 13.Compumedics ProFusion 인 경우 EEG4PatientInfo.xml 에서 데이터 가져온다.
                    //// 이 경우 ProgramType1이지만 Download 폴더 아래 환자정보 폴더가 생기고 그 안에 정보가 있음.
                    if (GlobalVar.configEntity.FolderPattern == "9")
                    {
                        //// 환자폴더의 패턴 >> { 포함
                        //// HTML, ProFusionEEG5 폴더 제외
                        
                        string[] dirList = Directory.GetDirectories(sdir.FullName);
                        string patInfoFolder = string.Empty;        //// 환자 폴더

                        if (dirList != null && dirList.Length > 0)
                        {
                            foreach (string name in dirList)
                            {
                                if (name.StartsWith("HTML") || name.StartsWith("ProFusionEEG5"))
                                    continue;

                                if (name.Contains("{"))
                                {
                                    patInfoFolder = name;
                                    break;
                                }
                            }

                            if (File.Exists(Path.Combine(patInfoFolder, "EEG4PatientInfo.xml")))
                            {
                                try
                                {
                                    GetPatientInfoFromEEG4Xml cls2 = new GetPatientInfoFromEEG4Xml();
                                    cls2.GetInfo(Path.Combine(patInfoFolder, "EEG4PatientInfo.xml"));

                                    orderInfo.patNo = cls2.ID;
                                    orderInfo.patName = cls2.Name;
                                    orderInfo.patSex = cls2.Sex;
                                    orderInfo.patAge = cls2.Age;
                                    orderInfo.patBirtyDay = cls2.BirthDay;

                                    orderInfo.Modality = "EGG";
                                    orderInfo.StudyDescription = "EGG";
                                    orderInfo.DicomDescription = "EGG";
                                    orderInfo.StudyModality = "EGG";

                                    isXml = true;
                                }
                                catch { }
                            }
                        }

                        if (orderInfo.patList == null)
                            orderInfo.patList = new Dictionary<string, string>();
                    }

                    //// DicomDir 없이 환자명을 가져온 경우에는 건너뛴다.
                    //// INI 파일에서 가져온 경우, 
                    if (!isNoDicom)
                    {
                        switch (GlobalVar.configEntity.FolderPattern)
                        {
                            case "1": //날짜_환자번호_환자명
                                orderInfo.patName = sdir.Name.Substring(sdir.Name.LastIndexOf("_") + 1);
                                //// 연구용 자료가 아닐 경우에만..
                                if (!orderInfo.patNo.Contains("UNKNOWN") && !string.IsNullOrWhiteSpace(orderInfo.patNo))
                                {
                                    if (sdir.Name.Contains("_"))
                                    {
                                        try
                                        {
                                            string[] arrStr = sdir.Name.Split('_');
                                            orderInfo.patNo = arrStr[1];
                                        }
                                        catch { }
                                    }
                                }
                                break;
                            case "2":   //환자번호(환자명)
                                orderInfo.patName = sdir.Name.Substring(sdir.Name.IndexOf("(") + 1, sdir.Name.Length - sdir.Name.IndexOf("(") - 2);
                                orderInfo.patName = orderInfo.patName.Replace("(", "").Replace(")", "");
                                break;
                            case "3":
                                //YUHS - 세브란스에서 dll 제공
                                //신촌 세브란스 병원
                                orderInfo.patName = GetPatientInfoFromYUHS.GetName(orderInfo.patNo);
                                break;
                            case "4":
                                ////환자명을 영문에서 한글로 : 병원 DB에서 조회한다..
                                if (isDicom == true && isMdb == false)
                                {
                                    ////Dicom정보를 가져왔을 때 - mdb에서 가져왔을 경우에는 제외
                                    string name = GetPatientNameFromDB.GetName(orderInfo.patNo, DBConnInfo);
                                    if (!string.IsNullOrWhiteSpace(name))
                                    {
                                        orderInfo.patName = name;
                                    }
                                }
                                break;
                            case "5":
                                //// 소켓연결로 환자명을 가져오게 설정된 경우 소켓연결정보 파일이 존재할 경우에만 실행.
                                if (File.Exists(Path.Combine(programPath, GlobalVar.SOCKET_CON_FILE)))
                                {
                                    Dictionary<string, string> socketCon = GetPatientNameFromDB.GetDBConn(Path.Combine(programPath, GlobalVar.SOCKET_CON_FILE));

                                    if (socketCon != null && socketCon["UsageSocket"] == "Y")
                                    {
                                        //// 소켓통신으로 환자명을 가져온다.
                                        Dictionary<string, string> socketPatInfo = GetPatientNameFromSocket.GetName(orderInfo.patNo, socketCon);
                                        if (socketPatInfo["PRSG_RSLT_DVSN_CD"] == "1")
                                        {
                                            orderInfo.patName = socketPatInfo["PTNT_NM"].Trim();
                                        }
                                    }
                                }
                                break;
                            case "6":
                            case "7":
                                //// Tomtech-아산병원초음파 : 7은 New버전
                                GetPatientInfoFromTomtech clsTT = new GetPatientInfoFromTomtech();
                                clsTT.GetInfo(sdir.FullName);

                                if (!string.IsNullOrWhiteSpace(clsTT.ID))
                                    orderInfo.patNo = clsTT.ID;
                                if (string.IsNullOrWhiteSpace(orderInfo.patName))
                                    orderInfo.patName = clsTT.Name;
                                break;
                            case "8":
                                //// 원광대병원 dll 사용                                
                                Dictionary<string, string> dllPatInfo = GetPatientNameFromDllImport.GetPatInfo("1", orderInfo.patNo);
                                if (dllPatInfo != null)
                                {                                    
                                    orderInfo.patName = dllPatInfo["환자명"];
                                    orderInfo.patSex = dllPatInfo["성별"];
                                    orderInfo.patAge = dllPatInfo["나이"];
                                    orderInfo.patBirtyDay = dllPatInfo["생년월일"];
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    orderInfo.patName = Utils.ReplaceSpecialWord(orderInfo.patName);
                    orderInfo.patName = orderInfo.patName.Replace(",", ".");

                    //// 연구용 자료일 경우
                    if (orderInfo.patNo.Contains("UNKNOWN") || string.IsNullOrWhiteSpace(orderInfo.patNo))
                    {
                        orderInfo.patNo = "NA" + DateTime.Now.ToString("ddhhmmss") + "RD";
                        orderInfo.patName = "연구용자료";
                    }

                    orderInfo.patNo = Utils.ReplaceSpecialWord(orderInfo.patNo);
                    orderInfo.ImgFiles = FileControl.GetFolderLength(sdir.FullName);
                    orderInfo.FolderSize = orderInfo.ImgFiles.FolderSize;

                    long fldLen = orderInfo.FolderSize / 1024 / 1024;
                    orderInfo.mediSize = fldLen.ToString() + " Mbyte";

                    //// 사이즈별 미디어 선택
                    if (orderInfo.FolderSize > Convert.ToInt64(GlobalVar.configEntity.DvdMaxSize))
                    {
                        orderInfo.mediType = "DVDR-DL";     //DVDR-DL
                    }
                    else if (orderInfo.FolderSize > Convert.ToInt64(GlobalVar.configEntity.CDMaxSize) &&
                             orderInfo.FolderSize <= Convert.ToInt64(GlobalVar.configEntity.DvdMaxSize))
                    {
                        orderInfo.mediType = "DVDR";     //DVDR
                    }
                    else
                    {
                        orderInfo.mediType = "CDR";     //CDR
                    }

                    //// BinCheck후에 미디어타입 다시 세팅
                    ////if (autoLoaderMediaType.Equals("CDR"))
                    ////{
                    ////    orderInfo.mediType = "CDR";
                    ////}
                    ////else if (autoLoaderMediaType.Equals("DVDR"))
                    ////{
                    ////    orderInfo.mediType = "DVDR";     //DVD

                    ////    if (orderInfo.FolderSize > Convert.ToInt64(GlobalVar.configEntity.DvdMaxSize))
                    ////    {
                    ////        orderInfo.mediType = "DVDR-DL";     //DVDR-DL
                    ////    }
                    ////    else
                    ////    {
                    ////        orderInfo.mediType = "DVDR";     //DVDR
                    ////    }
                    ////}

                    return orderInfo;
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// 다운로드 완료를 감지하기 위한 파일이 존재하는지 체크
        /// </summary>
        /// <param name="dirInfo"></param>
        /// <param name="checkFileName"></param>
        /// <returns>true : 굽기실행가능</returns>
        public static bool CheckFileExists(DirectoryInfo dirInfo, string checkFileName)
        {
            bool retVal = false;

            foreach (FileInfo fi in dirInfo.GetFiles())
            {
                if (fi.Name == checkFileName)
                {
                    retVal = true;
                    break;
                }
            }

            return retVal;
        }

        /// <summary>
        /// 환자정보 가져오기
        /// </summary>
        /// <param name="srcFolder"></param>
        /// <param name="imgFiles"></param>
        /// <param name="patInfor"></param>
        /// <param name="patList"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetPatient(string srcFolder, string disableMultiPatient, out List<string> imgFiles, out PatientList patInfor, out Dictionary<string, string> patList)
        {
            imgFiles = null;
            patList = null;
            Dictionary<string, string> dicResult = null;
            string DicomDirFile = Path.Combine(srcFolder, "DICOMDIR");
            FileInfo fi = new FileInfo(DicomDirFile);
            bool isDicomRead = false;
            patInfor = null;

            try
            {
                if (fi.Exists)
                {
                    dicResult = new Dictionary<string, string>();
                    dicResult.Add("ID", "");
                    dicResult.Add("Name", "");
                    dicResult.Add("Sex", "");
                    dicResult.Add("SexKr", "");
                    dicResult.Add("BirthDay", "");
                    dicResult.Add("Age", "");
                    dicResult.Add("StudyDesc", "");
                    dicResult.Add("Modality", "");
                    dicResult.Add("ImageCount", "");
                    dicResult.Add("ImagePath", "");
                    dicResult.Add("DicomDesc", "");
                    dicResult.Add("StudyModality", "");
                    dicResult.Add("PatientCount", "0");

                    //먼저 원래것으로..
                    patInfor = DicomDirReader.GetPatientrFromDicomNet(DicomDirFile);

                    if (patInfor == null || patInfor.Base.Count == 0)
                    {
                        //EvilDicom으로
                        patInfor = DicomDirReader.GetPatientrFromEvil(DicomDirFile);
                    }

                    //DICOMDIR에서 정보를 읽을 수 없을 경우 image file에서..
                    if (patInfor == null || patInfor.Base.Count == 0)
                    {
                        //환자 폴더 내에서 이미지 파일을 찾아 정보를 가져오도록 하자.
                        DicomInfomation di = new DicomInfomation();

                        //이미지 파일 목록 가져오기 - .dcm
                        List<string> lstFiles = FileControl.GetDicomImageFiles(new DirectoryInfo(srcFolder));

                        if (lstFiles != null && lstFiles.Count > 0)
                        {
                            foreach (string dcm in lstFiles)
                            {
                                di._DicomFilePath = dcm;
                                fi = new FileInfo(dcm);
                                di._DicomFileName = fi.Name;

                                dicResult = di.PatientInfo();

                                if (dicResult != null)
                                {
                                    isDicomRead = true;

                                    patList = di.PatientLST;
                                    dicResult["PatientCount"] = di.PatientCount.ToString();

                                    dicResult.Add("DcmFilePath", dcm);
                                    dicResult.Add("DcmFileName", fi.Name);

                                    break;
                                }
                            }

                            imgFiles = lstFiles;
                        }
                    }
                    else
                    {
                        isDicomRead = true;

                        dicResult["ID"] = patInfor.Base[0].Patient.ID;
                        dicResult["Name"] = patInfor.Base[0].Patient.Name;
                        dicResult["Sex"] = Utils.CheckNull(patInfor.Base[0].Patient.Gender);
                        dicResult["SexKr"] = Utils.CheckNull(patInfor.Base[0].Patient.GenderKR);
                        dicResult["BirthDay"] = Utils.CheckNull(patInfor.Base[0].Patient.BirthDate);
                        dicResult["Age"] = Utils.CheckNull(patInfor.Base[0].Patient.Age);
                        dicResult["StudyDesc"] = patInfor.Base[0].StudyDescArray;
                        dicResult["Modality"] = patInfor.Base[0].ModalityArray;
                        dicResult["ImageCount"] = patInfor.Base[0].RecordList.Count.ToString();
                        dicResult["StudyModality"] = patInfor.Base[0].StudyModalityArray;

                        //// 다중환자굽기 사용안함 설정 + 다중환자인 경우
                        if (disableMultiPatient == "Y" && patInfor.Base.Count > 1)
                        {
                            List<string> lstStdy = new List<string>();
                            foreach (PatientInfoEntity pInfo in patInfor.Base)
                            {
                                lstStdy.Add(pInfo.StudyModalityArray);
                            }
                            dicResult["StudyModality"] = string.Join("^^", lstStdy);
                        }

                        if (null != patInfor.Base[0].RecordList && patInfor.Base[0].RecordList.Count > 0)
                            dicResult["ImagePath"] = patInfor.Base[0].RecordList[0].ImageFilePath;

                        //생일 또는 성별을 못 가져왔을 때 이미지 파일에서 가져오도록..
                        if (string.IsNullOrEmpty(patInfor.Base[0].Patient.BirthDate) ||
                            string.IsNullOrEmpty(patInfor.Base[0].Patient.Gender))
                        {
                            DicomInfomation di = new DicomInfomation();
                            di._DicomFilePath = DicomDirFile;
                            Dictionary<string, string> dicImgInfo = di.PatientInfoFromDCM(dicResult["ImagePath"]);

                            if (dicImgInfo != null)
                            {
                                dicResult["Sex"] = dicImgInfo["Sex"];
                                dicResult["SexKr"] = dicImgInfo["SexKr"];
                                dicResult["BirthDay"] = dicImgInfo["BirthDay"];
                                dicResult["Age"] = dicImgInfo["Age"];
                            }
                        }

                        dicResult["DicomDesc"] = patInfor.AllDesc;

                        patList = patInfor.SimpleList;
                        dicResult["PatientCount"] = patInfor.Base.Count.ToString();

                        dicResult.Add("DcmFilePath", DicomDirFile);
                        dicResult.Add("DcmFileName", "DICOMDIR");
                    }

                    if (isDicomRead == false)
                    {
                        dicResult = new Dictionary<string, string>();
                        dicResult.Add("Result", "NotFound");
                    }
                    else
                    {
                        dicResult.Add("Result", "");
                    }
                }
                else
                {
                    ////this.ErrMsgShow("DICOMDIR 파일을 찾을 수 없습니다.\r\n", "Rimage Message : Not Exists DICOMDIR file");
                    dicResult.Add("Result", "NotFound");
                }
            }
            catch { }

            return dicResult;
        }
    }
}
