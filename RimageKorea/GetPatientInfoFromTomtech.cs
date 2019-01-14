using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RimageKorea
{
    /// <summary>
    /// 아산병원 초음파 (Tomtech)에서 환자 아이디, 이름 가져올 때
    /// </summary>
    public class GetPatientInfoFromTomtech
    {
        string _name;
        string _id;

        public string Name
        {
            get { return _name; }
        }

        public string ID
        {
            get { return _id; }
        }

        /// <summary>
        /// 환자아이디, 이름 폴더에서 불러오기 - Tomtech
        /// </summary>
        /// <param name="path"></param>
        public void GetInfo(string folderPath)
        {
            try
            {
                if (Directory.Exists(folderPath))
                {
                    foreach (string folder in Directory.GetDirectories(folderPath))
                    {
                        DirectoryInfo di = new DirectoryInfo(folder);
                        if (di.Name.ToLower() != "viewer")
                        {
                            //// Viewer 폴더가 아니고 '_'가 포함되어 있어야 함.
                            if (di.Name.Contains('_'))
                            {
                                this._id = di.Name.Substring(0, di.Name.IndexOf('_'));
                                this._name = di.Name.Substring(di.Name.IndexOf('_') + 1);

                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                this._id = string.Empty;
                this._name = string.Empty;
            }
        }
    }
}
