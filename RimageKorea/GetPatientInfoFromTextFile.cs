using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RimageKorea
{
    /// <summary>
    /// 순천향대 Pacs 파일중 텍스트에서 환자정보 가져올 때
    /// </summary>
    public class GetPatientInfoFromTextFile
    {
        string _name;
        string _id;
        string _sex;
        string _age;

        public string Name
        {
            get { return _name; }
        }

        public string ID
        {
            get { return _id; }
        }

        public string Sex
        {
            get { return _sex; }
        }

        public string Age
        {
            get { return _age; }
        }
        
        /// <summary>
        /// 환자정보를 파일에서 읽기
        /// </summary>
        /// <param name="path"></param>
        public void GetInfo(string path)
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

            try
            {
                //string[] lines = System.IO.File.ReadAllLines(path, Encoding.Default);
                string text = string.Empty;

                using (var streamReader = new StreamReader(fileStream, Encoding.Default))
                {
                    text = streamReader.ReadLine();

                    string[] arrLine = text.Split('!');

                    this._id = arrLine[0];
                    this._name = arrLine[1].Trim();
                    this._sex = arrLine[2];
                    this._age = arrLine[3];

                    string SexKr = string.Empty;
                    if (this._sex.ToUpper().Contains("F"))
                        SexKr = "여";
                    else if (this._sex.ToUpper().Contains("M"))
                        SexKr = "남";

                    this._sex = SexKr;
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            catch
            {
                this._id = string.Empty;
                this._name = string.Empty;
                this._sex = string.Empty;
                this._age = string.Empty;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
        }
    }
}
