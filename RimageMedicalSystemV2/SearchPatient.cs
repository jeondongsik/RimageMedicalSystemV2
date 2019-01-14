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
        public static BurnOrderedInfoEntity Get(DirectoryInfo sdir, string existDicomDir, string folderPattern, 
            Dictionary<string, string> DBConnInfo, string cdMaxSize, string dvdMaxSize, string autoLoaderMediaType, 
            string disableMultiPatient, string checkFile = "")
        {
            try
            {
                string mdbFileName = "PiView.mdb";
                string mdbFileNameMaro = @"Viewer\Export.mdb";

                bool burnAablity = true;
                bool seekTarget = true;

                bool isDicom = false;
                bool isMdb = false;
                bool isNoDicom = false;

                ////root에 checkFile ("end.txt") 파일이 존재하는지 체크하자.
                if (checkFile != "")
                {
                    burnAablity = CheckFileExists(sdir, checkFile);
                }

                if (burnAablity)
                {
                    //굽기완료된 폴더인지 체크(burn.end 파일체크)
                    seekTarget = CheckFileExists(sdir, GlobalVar.BURN_CHK_FL_NM);
                }

                //// 조회조건 모두 통과시
                if (burnAablity && seekTarget)
                {
                    //// 굽기 정보
                    BurnOrderedInfoEntity orderInfo = new BurnOrderedInfoEntity();
                    orderInfo.patDate = DateTime.Now.ToString("yyyy-MM-dd");
                    orderInfo.LocalShareFolder = sdir.Name;
                    orderInfo.patFolder = sdir.Name;
                    orderInfo.patFolderFullPath = sdir.FullName;

                    string pID = string.Empty;
                    string pName = string.Empty;
                    string pSex = string.Empty;
                    string pAge = string.Empty;
                    string pModality = string.Empty;
                    Dictionary<string, string> pList = null;
                    Dictionary<string, string> dicomdirInfo = null;
                    Dictionary<string, string> patList = null;

                    //DicomDir 파일이 존재하는지 체크
                    if (existDicomDir == "Y")
                    {
                        //DicomDir 파일이 존재하지 않으면 Pass
                        if (!(File.Exists(Path.Combine(sdir.FullName, "DICOMDIR")) || File.Exists(Path.Combine(sdir.FullName, mdbFileName))))
                            return null;
                    }

                    //MDB 파일에서 읽기
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
                        }

                        pList = cls.PatientLST;

                        isMdb = true;
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
                        }

                        pList = cls2.PatientLST;

                        isMdb = true;
                        #endregion
                    }

                    ////DICOMDIR 파일에서 읽기
                    isDicom = false;
                    isNoDicom = false;
                    if (File.Exists(Path.Combine(sdir.FullName, "DICOMDIR")))
                    {
                        #region -- DICOMDIR 파일에서 환자정보를 읽어온다. --
                        //DICOMDIR에서 가져온다.
                        dicomdirInfo = new Dictionary<string, string>();
                        List<string> dicImgList = null;
                        PatientList patInfor = null;
                        dicomdirInfo = GetPatient(sdir.FullName, disableMultiPatient, out dicImgList, out patInfor, out patList);
                        
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

                        isDicom = true;
                        #endregion
                    }
                    else
                    {
                        //DicomDir 파일이 없을 시 폴더명에서 환자아이디, 이름만 가져온다.
                        if (sdir.Name.Contains("_"))
                        {
                            try
                            {
                                string[] arrStr = sdir.Name.Split('_');
                                orderInfo.patNo = arrStr[1];
                                orderInfo.patName = arrStr[2];

                                isNoDicom = true;
                            }
                            catch { }
                        }
                    }

                    ////MDB파일에서 가져온 데이터가 있으면 대체.
                    if (isMdb == true)
                    {                        
                        orderInfo.patNo = (string.IsNullOrWhiteSpace(pID)) ? orderInfo.patNo : pID;
                        orderInfo.patName = (string.IsNullOrWhiteSpace(pName)) ? orderInfo.patName : pName;
                        orderInfo.patSex = (string.IsNullOrWhiteSpace(pSex)) ? orderInfo.patSex : pSex;
                        orderInfo.patAge = (string.IsNullOrWhiteSpace(pAge)) ? orderInfo.patAge : pAge;
                        orderInfo.Count = (pList.Count.Equals(0)) ? orderInfo.Count : pList.Count;
                        orderInfo.patList = (pList == null) ? orderInfo.patList : pList;
                    }

                    //Studyinfo.ini에서 환자아이디,명,나이를 가져올 경우
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
                        }
                        catch { }
                    }

                    ////DicomDir 없이 환자명을 가져온 경우에는 건너뛴다.
                    if (isNoDicom == false)
                    {
                        switch (folderPattern)
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
                            case "4":
                                ////환자명을 영문에서 한글로 : 병원 DB에서 조회한다..
                                if (isDicom == true && isMdb == false)
                                {
                                    ////Dicom정보를 가져왔을 때 - mdb에서 가져왔을 경우에는 제외
                                    string name = GetPatientNameFromDB.GetName(orderInfo.patNo, DBConnInfo);
                                    if (string.IsNullOrWhiteSpace(name) == false)
                                    {
                                        orderInfo.patName = name;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    orderInfo.patName = Utils.ReplaceSpecialWord(orderInfo.patName);
                    orderInfo.patName = orderInfo.patName.Replace(",", ".");

                    //연구용 자료일 경우
                    if (orderInfo.patNo.Contains("UNKNOWN") || string.IsNullOrWhiteSpace(orderInfo.patNo))
                    {
                        orderInfo.patNo = "NA" + DateTime.Now.ToString("ddhhmmss") + "RD";
                        orderInfo.patName = "연구용자료";
                    }

                    orderInfo.patNo = Utils.ReplaceSpecialWord(orderInfo.patNo);
                    orderInfo.ImgFiles = FileControl.GetFolderLength(sdir.FullName);
                    orderInfo.FolderSize = orderInfo.ImgFiles.FolderSize;
                    orderInfo.EditList = orderInfo.ImgFiles.EditList;

                    long fldLen = orderInfo.FolderSize / 1024 / 1024;
                    orderInfo.mediSize = fldLen.ToString() + " Mbyte";

                    //// 사이즈별 미디어 선택
                    if (orderInfo.FolderSize > Convert.ToInt64(dvdMaxSize))
                    {
                        orderInfo.mediType = "DVDR-DL";     //DVDR-DL
                    }
                    else if (orderInfo.FolderSize > Convert.ToInt64(cdMaxSize) &&
                             orderInfo.FolderSize <= Convert.ToInt64(dvdMaxSize))
                    {
                        orderInfo.mediType = "DVDR";     //DVDR
                    }
                    else
                    {
                        orderInfo.mediType = "CDR";     //CDR
                    }

                    //// BinCheck후에 미디어타입 다시 세팅
                    if (autoLoaderMediaType.Equals("CDR"))
                    {
                        orderInfo.mediType = "CDR";
                    }
                    else if (autoLoaderMediaType.Equals("DVDR"))
                    {
                        orderInfo.mediType = "DVDR";     //DVD

                        if (orderInfo.FolderSize > Convert.ToInt64(dvdMaxSize))
                        {
                            orderInfo.mediType = "DVDR-DL";     //DVDR-DL
                        }
                        else
                        {
                            orderInfo.mediType = "DVDR";     //DVDR
                        }
                    }

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
        private static bool CheckFileExists(DirectoryInfo dirInfo, string checkFileName)
        {
            bool retVal = false;

            foreach (FileInfo fi in dirInfo.GetFiles())
            {
                if (fi.Name.Equals(checkFileName))
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
        private static Dictionary<string, string> GetPatient(string srcFolder, string disableMultiPatient, out List<string> imgFiles, out PatientList patInfor, out Dictionary<string, string> patList)
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
