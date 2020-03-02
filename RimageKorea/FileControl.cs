using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using Microsoft.VisualBasic.FileIO;
using System.Xml;

namespace RimageKorea
{
    [Serializable]
    public class FolderInfo
    {
        public long FolderSize { get; set; }
        public List<string> EditList { get; set; }
    }

    public class FileControl
    {
        public FileControl()
        {
        }

        /// <summary>
        /// 폴더 삭제
        /// </summary>
        /// <param name="source">폴더경로</param>
        /// <param name="recycle">휴지통으로 보내려면 true</param>
        public static bool DeleteFolder(string source, bool recycle)
        {
            bool retVal = true;
            try
            {
                if (!Directory.Exists(source))
                    return true;

                if (recycle == true)
                    FileSystem.DeleteDirectory(source, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                else
                    FileSystem.DeleteDirectory(source, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
            }
            catch (Exception ex)
            {
                retVal = false;
                ErrorLog.LogWrite("FileControl", ex.ToString(), Environment.CurrentDirectory);
            }
            return retVal;
        }

        /// <summary>
        /// 폴더삭제
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool DeleteFolder(string source)
        {
            bool retVal = true;
            try
            {
                if (Directory.Exists(source))
                {
                    Directory.Delete(source, true);
                }
            }
            catch
            {
                
            }
            return retVal;
        }

        /// <summary>
        /// 폴더 사이즈 가져오기
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static FolderInfo GetFolderLength(string folderPath)
        {
            return FileControl.DirectoryLength(new DirectoryInfo(folderPath));
        }

        /// <summary>
        /// 폴더 사이즈 계산 및 파일 목록 반환
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private static FolderInfo DirectoryLength(DirectoryInfo dir)
        {
            //long rtLen = 0;
            FolderInfo fi = new FolderInfo();
            fi.FolderSize = 0;
            fi.EditList = new List<string>();

            try
            {
                if (!dir.Exists)
                {
                    fi.FolderSize = 0;
                }

                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    //end.txt 파일은 제외한다.
                    if (false == file.Name.Equals(GlobalVar.DOWN_CHK_FL_NM) && false == file.Name.Equals(GlobalVar.BURN_CHK_FL_NM))
                    {
                        fi.FolderSize += file.Length;
                        fi.EditList.Add(file.FullName);
                    }
                }

                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach (DirectoryInfo dri in dirs)
                {
                    fi.FolderSize += DirectoryLength(dri).FolderSize;

                    foreach (string str in DirectoryLength(dri).EditList)
                    {
                        fi.EditList.Add(str);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite("FileControl", ex.ToString(), Environment.CurrentDirectory);
            }

            return fi;
        }

        /// <summary>
        /// 머지파일 생성
        /// </summary>
        /// <param name="MergeFileFolder">머지파일폴더</param>
        /// <param name="MergePrint">프린트여부</param>
        /// <param name="patFolder">환자폴더명</param>
        /// <param name="patDate">굽기일자</param>
        /// <param name="patNo"></param>
        /// <param name="patName"></param>
        /// <param name="patSex"></param>
        /// <param name="studyModality">Study정보</param>
        /// <param name="modality"></param>
        /// <param name="patientName"></param>
        /// <param name="megPath">머지파일FullPath</param>
        /// <param name="studyDate"></param>
        public static void CreateMergeFile(string MergeFileFolder, string MergePrint, string patFolder, string patDate, string patNo, string patName, string patSex,
            string studyModality, string modality = "", string patientName = "", string megPath = "", string studyDate = "", Dictionary<string, string> dbInfo = null)
        {
            //string path = MergeFileFolder + patFolder + ".txt";
            string path = megPath;
            FileInfo fi = new FileInfo(path);
            string contents = "";
            bool rdFlag = false;

            try
            {
                if (patName == "")
                    patName = patientName;

                if (patNo.Length > 1 && patNo.Substring(0, 2) == "NA")
                {
                    patNo = " ";
                    rdFlag = true;
                }

                if (fi.Exists)
                {
                    fi.Delete();
                }

                //연구용일 경우
                if (rdFlag)
                {
                    patDate = " ";
                    patSex = " ";
                }

                if (MergePrint.Substring(0, 1) == "Y")
                    contents += patDate;
                else
                    contents += "";

                if (MergePrint.Substring(1, 1) == "Y")
                    contents += "," + patNo;
                else
                    contents += ",";

                if (MergePrint.Substring(2, 1) == "Y")
                    contents += "," + patSex;
                else
                    contents += ",";

                if (MergePrint.Substring(3, 1) == "Y")
                    contents += "," + patName;
                else
                    contents += ",";

                if (MergePrint.Substring(4, 1) == "Y")
                    contents += "," + Utils.ReturnModality(modality, 5);
                else
                    contents += ",";

                if (MergePrint.Substring(5, 1) == "Y")
                    contents += "," + Utils.ConvertStudyModality(studyModality, 6);
                else
                    contents += ",";

                if (MergePrint.Substring(6, 1) == "Y")
                {
                    //// 환자병동 가져온다.
                    string residence = GetPatientNameFromDB.GetResidency(patNo, dbInfo);
                    contents += "," + residence;
                }
                else
                {
                    contents += ",";
                }

                //StudyDate는 마지막에 무조건 찍어준다.
                contents += "," + studyDate;

                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(contents);
                sw.Dispose();
                fs.Close();
                fs.Dispose();
            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite("FileControl", ex.ToString(), Environment.CurrentDirectory);
            }
        }

        /// <summary>
        /// 머지파일 생성 : 여러환자를 찎을 때
        /// </summary>
        /// <param name="megPath"></param>
        /// <param name="dicPatList"></param>
        /// <param name="patDate"></param>
        public static void CreateMergeFile(string megPath, Dictionary<string, string> dicPatList, string patDate)
        {
            if (null == dicPatList) return;

            FileInfo fi = new FileInfo(megPath);
            string contents = string.Empty;
            string[] arrStr = new string[36];

            try
            {
                if (fi.Exists)
                {
                    fi.Delete();
                }

                int i = 0;
                foreach (KeyValuePair<string, string> kp in dicPatList)
                {
                    if (i > 35) break;

                    arrStr[i] = kp.Key + "/" + kp.Value;
                    i++;
                }

                contents = patDate;
                foreach (string str in arrStr)
                {
                    contents += "," + Utils.CheckNull(str);
                }

                FileStream fs = new FileStream(megPath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(contents);
                sw.Dispose();
                fs.Close();
                fs.Dispose();
            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite("FileControl", ex.ToString(), Environment.CurrentDirectory);
            }
        }

        /// <summary>
        /// Editlist Xml생성
        /// </summary>
        /// <param name="EditList"></param>
        /// <param name="originalPath"></param>
        /// <param name="targetPath"></param>
        /// <param name="RimageSystemFolder"></param>
        /// <returns></returns>
        public static string createEditListXml(List<string> EditList, string originalPath, string targetPath, string RimageSystemFolder, string parentFolder = "")
        {
            XmlDocument pXMLDoc = new XmlDocument();
            XmlElement pRootElement;
            XmlElement memberElement1;
            XmlAttribute pAttribute;
            XmlProcessingInstruction newPI;
            XmlDocumentType doctype;

            // Create a procesing instruction.
            newPI = pXMLDoc.CreateProcessingInstruction("xml", "version=\"1.0\"");

            // Add the processing instruction node to the document.
            pXMLDoc.AppendChild(newPI);

            //Create a document type node and add it to the document.
            doctype = pXMLDoc.CreateDocumentType("EditList", null, RimageSystemFolder + "\\XML\\EditList_1.1.dtd", null);
            pXMLDoc.AppendChild(doctype);

            pRootElement = pXMLDoc.CreateElement("EditList");
            pXMLDoc.AppendChild(pRootElement);

            pAttribute = pXMLDoc.CreateAttribute("Version");
            pAttribute.Value = "1.1";
            pRootElement.Attributes.SetNamedItem(pAttribute);

            memberElement1 = pXMLDoc.CreateElement("Options");
            pRootElement.AppendChild(memberElement1);

            pAttribute = pXMLDoc.CreateAttribute("ExpandFolders");
            pAttribute.Value = "true";
            memberElement1.Attributes.SetNamedItem(pAttribute);
            
            //여기서부터 파일목록
            foreach (string str in EditList)
            {                
                memberElement1 = pXMLDoc.CreateElement("SrcDst");
                pRootElement.AppendChild(memberElement1);

                pAttribute = pXMLDoc.CreateAttribute("Src");
                pAttribute.Value = str.Replace(originalPath, targetPath);
                memberElement1.Attributes.SetNamedItem(pAttribute);

                pAttribute = pXMLDoc.CreateAttribute("Dst");
                pAttribute.Value = str.Replace(originalPath, targetPath).Replace(parentFolder, "");
                memberElement1.Attributes.SetNamedItem(pAttribute);
            }

            return pXMLDoc.OuterXml;
        }

        /// <summary>
        /// Editlist Xml 파일 생성
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="fileName"></param>
        public static void createEditListFile(string xml, string fileName)
        {
            try
            {
                if (FileSystem.FileExists(fileName))
                    FileSystem.DeleteFile(fileName);

                FileStream fs = new FileStream(fileName, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(xml);
                sw.Dispose();
                fs.Close();
                fs.Dispose();

                //using (StreamWriter sw = File.CreateText(fileName))
                //{
                //    sw.WriteLine(xml);
                //    sw.Close();
                //}
            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite("FileControl", ex.ToString(), Environment.CurrentDirectory);
            }
        }

        /// <summary>
        /// 폴더 사이즈 가져오기
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static long GetFolderLengthOnly(string folderPath)
        {
            return FileControl.DirectoryLengthOnly(new DirectoryInfo(folderPath));
        }

        /// <summary>
        /// 폴더 사이즈 계산
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private static long DirectoryLengthOnly(DirectoryInfo dir)
        {
            long rtLen = 0;

            try
            {
                if (!dir.Exists)
                {
                    rtLen = 0;
                }

                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    //end.txt 파일은 제외한다.
                    if (false == file.Name.Equals(GlobalVar.DOWN_CHK_FL_NM) && false == file.Name.Equals(GlobalVar.BURN_CHK_FL_NM))
                    {
                        rtLen += file.Length;
                    }
                }

                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach (DirectoryInfo dri in dirs)
                {
                    rtLen += DirectoryLengthOnly(dri);
                }

            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite("FileControl", ex.ToString(), Environment.CurrentDirectory);
            }

            return rtLen;
        }

        /// <summary>
        /// 지정한 파일 삭제
        /// </summary>
        /// <param name="destination"></param>
        public static void DeleteFile(string destination, bool recycle = false)
        {
            try
            {
                //FileInfo fi = new FileInfo(destination);
                //if (fi.Exists)
                //{
                //    fi.Delete();
                //}
                if (recycle == true)
                    FileSystem.DeleteFile(destination, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                else
                    FileSystem.DeleteFile(destination, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
            }
            catch
            {
            }
        }

        /// <summary>
        /// String List를 Text 파일로 생성한다.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        public static void CreateTextFileByListStr(string fileName, List<string> content)
        {
            StringBuilder sbStr = new StringBuilder();

            try
            {
                if (content != null)
                {
                    foreach (string str in content)
                    {
                        sbStr.Append(str + Environment.NewLine);
                    }

                    FileStream fs = new FileStream(fileName, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.Write(sbStr.ToString());
                    sw.Dispose();
                    fs.Close();
                    fs.Dispose();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite("FileControl", ex.ToString(), Environment.CurrentDirectory);
            }
        }

        /// <summary>
        /// 빈파일을 생성한다.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        public static void CreateTextFile(string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Create);
                fs.Close();
                fs.Dispose();
                //StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                //sw.Write(string.Empty);
                //sw.Dispose();
            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite("FileControl", ex.ToString(), Environment.CurrentDirectory);
            }
        }

        /// <summary>
        /// 굽기 명령을 보낸 내역을 Xml로 생성
        /// </summary>
        /// <param name="patDate"></param>
        /// <param name="patNo"></param>
        /// <param name="patName"></param>
        /// <param name="copies"></param>
        /// <param name="mediType"></param>
        /// <param name="mediSize"></param>
        /// <param name="LocalShareFolder"></param>
        /// <param name="Modality"></param>
        /// <param name="StudyDescription"></param>
        /// <param name="patSex"></param>
        /// <param name="patAge"></param>
        /// <param name="patFolder"></param>
        /// <param name="FolderSize"></param>
        /// <param name="EditList"></param>
        /// <param name="EditListPath"></param>
        /// <param name="result"></param>
        /// <param name="patList"></param>
        /// <returns></returns>
        public static string createOrderLogXml(string patDate, string patNo, string patName, string copies, string mediType, string mediSize, string LocalShareFolder, 
                                               string Modality, string StudyDescription, string patSex, string patAge, string patFolder, string FolderSize,
                                               string EditListPath, string result, string studyModality, string dicomDescription, string patList = "", string burnMulty = "N")
        {
            XmlDocument pXMLDoc = new XmlDocument();
            XmlElement pRootElement;
            XmlElement memberElement1;
            XmlAttribute pAttribute;
            XmlProcessingInstruction newPI;
            //XmlDocumentType doctype;

            // Create a procesing instruction. version="1.0" encoding="utf-8"
            newPI = pXMLDoc.CreateProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");

            // Add the processing instruction node to the document.
            pXMLDoc.AppendChild(newPI);

            ////Create a document type node and add it to the document.
            //doctype = pXMLDoc.CreateDocumentType("OrderContent", null, null, null);
            //pXMLDoc.AppendChild(doctype);

            pRootElement = pXMLDoc.CreateElement("Record");
            pXMLDoc.AppendChild(pRootElement);

            memberElement1 = pXMLDoc.CreateElement("OrderInfo");
            pRootElement.AppendChild(memberElement1);

            pAttribute = pXMLDoc.CreateAttribute("CreateDateTime");
            pAttribute.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("patDate");
            pAttribute.Value = patDate;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("patNo");
            pAttribute.Value = patNo;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("patName");
            pAttribute.Value = patName;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("copies");
            pAttribute.Value = copies;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("mediType");
            pAttribute.Value = mediType;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("mediSize");
            pAttribute.Value = mediSize;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("Modality");
            pAttribute.Value = Modality;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("StudyDescription");
            pAttribute.Value = StudyDescription;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("patSex");
            pAttribute.Value = patSex;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("patAge");
            pAttribute.Value = patAge;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("LocalShareFolder");
            pAttribute.Value = LocalShareFolder;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("patFolder");
            pAttribute.Value = patFolder;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("FolderSize");
            pAttribute.Value = FolderSize;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("EditListPath");
            pAttribute.Value = EditListPath;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("Result");
            pAttribute.Value = result;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("patList");
            pAttribute.Value = patList;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("burnMulty");
            pAttribute.Value = burnMulty;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("StudyModality");
            pAttribute.Value = studyModality;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("DicomDescription");
            pAttribute.Value = dicomDescription;
            memberElement1.Attributes.SetNamedItem(pAttribute);

            return pXMLDoc.OuterXml;
        }

        /// <summary>
        /// EditList 저장된 txt 파일에서 읽어서 반환하기
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<string> GetEditListContent(string fileName)
        {
            List<string> editList = null;

            if (File.Exists(fileName))
            {
                editList = new List<string>();

                string[] lines = File.ReadAllLines(fileName);

                foreach (string str in lines)
                {
                    if (String.IsNullOrWhiteSpace(str) == false)
                        editList.Add(str);
                }
            }
            return editList;
        }

        /// <summary>
        /// Order Log 반환
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable GetOrderedList(string filePath)
        {
            DataTable retDt = new DataTable();
            retDt.Columns.Add("CreateDateTime", typeof(System.String));
            retDt.Columns.Add("patDate", typeof(System.String));
            retDt.Columns.Add("patNo", typeof(System.String));
            retDt.Columns.Add("patName", typeof(System.String));
            retDt.Columns.Add("copies", typeof(System.Int32));
            retDt.Columns.Add("mediType", typeof(System.String));
            retDt.Columns.Add("mediSize", typeof(System.String));
            retDt.Columns.Add("LocalShareFolder", typeof(System.String));
            retDt.Columns.Add("Modality", typeof(System.String));
            retDt.Columns.Add("StudyDescription", typeof(System.String));
            retDt.Columns.Add("patSex", typeof(System.String));
            retDt.Columns.Add("patAge", typeof(System.String));
            retDt.Columns.Add("patFolder", typeof(System.String));
            retDt.Columns.Add("FolderSize", typeof(System.Int64));
            retDt.Columns.Add("EditListPath", typeof(System.String));
            retDt.Columns.Add("Result", typeof(System.String));
            retDt.Columns.Add("burning", typeof(System.String));
            retDt.Columns.Add("delete", typeof(System.String));
            retDt.Columns.Add("fileName", typeof(System.String));
            retDt.Columns.Add("patList", typeof(System.String));
            retDt.Columns.Add("burnMulty", typeof(System.String));
            retDt.Columns.Add("DicomDescription", typeof(System.String));
            retDt.Columns.Add("StudyModality", typeof(System.String));

            try
            {
                DirectoryInfo di = new DirectoryInfo(filePath);
                foreach (FileInfo fi in di.GetFiles("*.xml"))
                {
                    DataRow dr = retDt.NewRow();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fi.FullName);
                    XmlElement node = (XmlElement)doc.DocumentElement.FirstChild;

                    do
                    {
                        if (node.Name.Equals("OrderInfo"))
                        {
                            if (node.HasAttributes)
                            {
                                foreach (XmlAttribute attr in node.Attributes)
                                {
                                    if (attr.Name == "copies")
                                        dr[attr.Name] = Convert.ToInt32(attr.Value);
                                    else if (attr.Name == "FolderSize")
                                        dr[attr.Name] = Convert.ToInt64(attr.Value);
                                    else
                                        dr[attr.Name] = attr.Value;
                                }
                            }

                            break;
                        }

                    } while ((node = (XmlElement)node.NextSibling) != null);

                    dr["burning"] = "▶ 굽기";
                    dr["delete"] = "▷ 삭제";
                    dr["fileName"] = fi.FullName;
                    
                    retDt.Rows.Add(dr);
                }
            }
            catch { }

            return retDt;
        }

        /// <summary>
        /// Drive 정보 가져오기
        /// </summary>
        /// <param name="drive"></param>
        /// <returns></returns>
        public static DriveInfo GetDriveInfo(string drive)
        {
            return new DriveInfo(drive);
        }

        /// <summary>
        /// 폴더안의 모든 파일속성 변경하기
        /// </summary>
        /// <param name="dir"></param>
        public static void FileAttributeChange(string directoryPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(directoryPath);

                if (dir.Exists)
                {
                    FileInfo[] files = dir.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        file.Attributes = FileAttributes.Normal;
                    }

                    DirectoryInfo[] dirs = dir.GetDirectories();
                    foreach (DirectoryInfo dri in dirs)
                    {
                        FileAttributeChange(dri.FullName);
                    }
                }

            }
            catch { }
        }

        /// <summary>
        /// 지정한 폴더의 하위 항목(파일,폴더)을 모두 삭제한다.
        /// </summary>
        /// <param name="folderPath"></param>
        public static void ClearDirectory(string folderPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(folderPath);
                if (dir.Exists)
                {
                    dir.Delete(true);
                }
            }
            catch { }
        }

        /// <summary>
        /// 파일의 내용을 모두 반환한다.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetTextFileContent(string filePath)
        {
            string s = string.Empty;

            if (FileSystem.FileExists(filePath))
                s = FileSystem.ReadAllText(filePath);

            return s;
        }
        
        /// <summary>
        /// Dicom img 파일 목록 반환
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static List<string> GetDicomImageFiles(DirectoryInfo dir)
        {
            List<string> lstFiles = new List<string>();

            try
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    if (file.Extension.ToLower().Contains("dcm") == true)
                    {
                        lstFiles.Add(file.FullName);
                    }
                }

                foreach (DirectoryInfo dri in dir.GetDirectories())
                {
                    foreach (string str in GetDicomImageFiles(dri))
                    {
                        lstFiles.Add(str);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite("FileControl", ex.ToString(), Environment.CurrentDirectory);
            }

            return lstFiles;
        }

        /// <summary>
        /// 파일 복사하기
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="destFile"></param>
        /// <returns></returns>
        public static string CopyFile(string srcFile, string destFile)
        {
            try
            {
                if (File.Exists(destFile))
                    return destFile;

                FileSystem.CopyFile(srcFile, destFile, true);                
            }
            catch
            {
                return string.Empty;
            }

            return destFile;
        }

        /// <summary>
        /// 파일 이동하기
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="destFile"></param>
        /// <returns></returns>
        public static string MoveFile(string srcFile, string destFile)
        {
            try
            {
                if (File.Exists(destFile))
                    return destFile;

                FileSystem.MoveFile(srcFile, destFile, true);
            }
            catch
            {
                return string.Empty;
            }

            return destFile;
        }
                
        /// <summary>
        /// 명령정보 JSON 파일로 저장하기
        /// </summary>
        /// <param name="orderID">명령아이디</param>
        /// <param name="json">저장할 명령정보 JSON Text</param>
        /// <returns></returns>
        public static string CreateOrderJsonFile(string orderID, string json)
        {
            try
            {
                string filePath = Path.Combine(GlobalVar.ProgramExecuteFolder, GlobalVar.ORDER_FOLDER, string.Format("{0}.json", orderID));

                //// 파일이 존재하면 삭제하고 다시 생성
                if (File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch { }
                }

                FileStream fs = new FileStream(filePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(json);
                sw.Dispose();
                fs.Close();
                fs.Dispose();

                return filePath;
            }
            catch (Exception ex)
            {
                ErrorLog.LogWrite("FileControl", ex.ToString(), Environment.CurrentDirectory);
            }

            return string.Empty;
        }

        /// <summary>
        /// 파일을 생성하고 내용을 쓴다.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool Write(string message, string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                FileStream fs = new FileStream(fileName, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(message);
                sw.Dispose();
                fs.Close();
                fs.Dispose();

                return true;
            }
            catch 
            {
                ////ErrorLog.LogWrite("FileControl", ex.ToString(), Environment.CurrentDirectory);
            }

            return false;
        }

        /// <summary>
        /// EditList Xml파일 삭제
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="hostIP"></param>
        /// <param name="sysPath"></param>
        /// <param name="serverType"></param>
        public static void DelEditListFile(string orderID, string hostIP, string sysPath, string serverType)
        {
            try
            {
                ////EditList 삭제
                FileInfo fl = null;
                if (serverType.Equals("R"))
                    fl = new FileInfo("\\\\" + hostIP + "\\Rimage\\EditList\\" + orderID + ".xml");
                else
                    fl = new FileInfo(Path.Combine(sysPath, "EditList\\" + orderID + ".xml"));

                if (fl.Exists)
                {
                    fl.Delete();
                }
            }
            catch
            {
                //ErrMsgShow("폴더삭제 시 에러가 발생하였습니다.\r\n직접 디렉토리를 삭제해 주세요.\r\n" + ex.ToString(), "Rimage Message : DelDirectory");
            }
        }

        /// <summary>
        /// 굽기완료된 명령정보 삭제
        /// </summary>
        /// <param name="orderId"></param>
        public static void DeleteBurnEndOrder(string orderId)
        {
            try
            {
                bool delOK = true;

                //// 오더 폴더 삭제
                string orderFolder = Path.Combine(GlobalVar.ProgramExecuteFolder, GlobalVar.ORDER_FOLDER, orderId);

                ////foreach (string str in Directory.GetFiles(GlobalVar.BURN_JOB_END_FL))
                ////{
                ////    if (str.Contains(GlobalVar.BURN_JOB_END_FL))
                ////    {
                ////        delOK = true;
                ////        break;
                ////    }
                ////}

                if (delOK)
                {
                    FileControl.ClearDirectory(orderFolder);
                }
            }
            catch { }
        }

        /// <summary>
        /// 환자폴더 생성
        /// </summary>
        /// <param name="patID"></param>
        /// <param name="patName"></param>
        /// <returns></returns>
        public static string CreatePatientFolder(string patNo, string patName)
        {
            string patientFolderName = string.Format("{0}_{1}_{2}", DateTime.Now.ToString("yyyyMMdd"), patNo, patName);

            try
            {
                string fld = Path.Combine(GlobalVar.configEntity.LocalShareFolder, patientFolderName);
                DirectoryInfo dri = new DirectoryInfo(fld);

                if (dri.Exists)
                {
                    string fld_new = "";

                    if (patName.Contains("("))
                    {
                        int s = patName.IndexOf("(", 0);
                        int e = patName.IndexOf(")", 0);
                        string no = patName.Substring(s + 1, (e - s - 1));
                        int i = Convert.ToInt16(no);

                        fld_new = patName.Replace("(" + no + ")", "(" + (i + 1).ToString() + ")");
                    }
                    else
                    {
                        fld_new = patName + "(1)";
                    }

                    patientFolderName = string.Format("{0}_{1}_{2}", DateTime.Now.ToString("yyyyMMdd"), patNo, fld_new);
                    CreatePatientFolder(patNo, fld_new);
                }
                else
                {
                    dri.Create();
                }
            }
            catch { throw; }

            return patientFolderName;
        }

        /// <summary>
        /// 다운로드 폴더명 바꾸기 => 다운로드 폴더 -> 환자명
        /// </summary>
        /// <param name="patName"></param>
        public static string ChangeDownloadFolderToPatientFolder(string patNo, string patName)
        {
            ////변경후폴더명
            string patientFolderName = string.Format("MC_{0}_{1}_{2}", DateTime.Now.ToString("yyyyMMdd"), patNo, patName);
            try
            {
                string fld = Path.Combine(GlobalVar.configEntity.LocalShareFolder, patientFolderName);
                DirectoryInfo dri = new DirectoryInfo(fld);

                if (dri.Exists)
                {
                    string fld_new = "";

                    if (patName.Contains("("))
                    {
                        int s = patName.IndexOf("(", 0);
                        int e = patName.IndexOf(")", 0);
                        string no = patName.Substring(s + 1, (e - s - 1));
                        int i = Convert.ToInt16(no);

                        fld_new = patName.Replace("(" + no + ")", "(" + (i + 1).ToString() + ")");
                    }
                    else
                    {
                        fld_new = patName + "(1)";
                    }
                                        
                    patientFolderName = string.Format("MC_{0}_{1}_{2}", DateTime.Now.ToString("yyyyMMdd"), patNo, fld_new);
                    ChangeDownloadFolderToPatientFolder(patNo, fld_new);
                }
                else
                {
                    DirectoryInfo downFld = new DirectoryInfo(GlobalVar.configEntity.DicomDownloadFolder);
                    downFld.MoveTo(fld);

                    DirectoryInfo downFld2 = new DirectoryInfo(GlobalVar.configEntity.DicomDownloadFolder);
                    if (downFld2.Exists)
                    {
                        FileControl.ClearDirectory(downFld2.FullName);
                    }
                    else
                    {
                        downFld2.Create();
                    }
                }

                return patientFolderName;
            }
            catch
            {                
                throw;
            }
        }

        /// <summary>
        /// 환자 폴더 삭제
        /// </summary>
        /// <param name="target"></param>
        public static void DeletePatFolder(string target)
        {
            try
            {
                DirectoryInfo sourceDir = new DirectoryInfo(target);

                if (sourceDir.Exists)
                {
                    if (GlobalVar.configEntity.DelType == "0")
                    {
                        DeleteFolder(sourceDir.FullName, false);
                    }
                    else
                    {
                        DeleteFolder(sourceDir.FullName, true);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 폴더 통째로 복사하기
        /// </summary>
        /// <param name="srcDir">소스폴더</param>
        /// <param name="trgDir">대상풀더</param>
        /// <param name="topSrcFolder"></param>
        public static List<string> CopyFolderAndFiles(string srcDir, string trgDir, string topSrcFolder, List<string> addFiles)
        {
            if (addFiles == null)
                addFiles = new List<string>();

            try
            {
                DirectoryInfo srcFolder = new DirectoryInfo(srcDir);
                
                FileInfo[] files = srcFolder.GetFiles();
                DirectoryInfo[] dirs = srcFolder.GetDirectories();

                foreach (FileInfo file in files)
                { 
                    string copyto = file.FullName.Replace(topSrcFolder, trgDir);                    
                    FileSystem.CopyFile(file.FullName, copyto, true);

                    if (!addFiles.Contains(copyto))
                        addFiles.Add(copyto);
                }

                foreach (DirectoryInfo dri in dirs)
                {                    
                    //// 폴더가 없을 경우 생성한다.
                    string targetFolder = dri.FullName.Replace(topSrcFolder, trgDir);
                    if (!Directory.Exists(targetFolder))
                    {
                        FileSystem.CreateDirectory(targetFolder);
                    }

                    CopyFolderAndFiles(dri.FullName, trgDir, topSrcFolder, addFiles);
                }
            }
            catch { }

            return addFiles;
        }
    }
}
