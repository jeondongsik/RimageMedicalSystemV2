using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RimageKorea
{
    public class CheckFiles
    {
        /// <summary>
        /// 체크해야할 파일이 다 들어 있는지 체크한다.
        /// </summary>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public static bool Exists(string targetPath)
        {
            try
            {
                if (File.Exists(GlobalVar.CheckFileList))
                {
                    string[] files = File.ReadAllLines(GlobalVar.CheckFileList, System.Text.Encoding.Default);
                    List<string> fileList = new List<string>();

                    if (files != null)
                    {
                        foreach (string str in files)
                        {
                            if (fileList.Contains(str.ToLower()) == false)
                                fileList.Add(str.ToLower());
                        }

                        int cnt = 0;

                        foreach (string fileName in Directory.GetFiles(targetPath))
                        {
                            FileInfo fi = new FileInfo(fileName);

                            if (fileList.Contains(fi.Name.ToLower()) == true)
                            {
                                cnt++;
                            }
                        }

                        if (cnt == fileList.Count)
                            return true;
                        else
                            return false;
                    }

                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch { return true; }
        }
    }
}
