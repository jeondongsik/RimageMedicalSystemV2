using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace RimageKorea
{
    public class ReadNumber
    {
        string filePath;
        XmlDocument doc = new XmlDocument();

        public ReadNumber(string xmlPath)
        {
            this.filePath = Path.Combine(xmlPath, "readnumber.xml");

            if (!File.Exists(filePath))
            {
                //파일생성
                createXml();
            }
        }

        private void createXml()
        {
            XmlTextWriter writer = new XmlTextWriter(filePath, Encoding.UTF8);

            //들여쓰기
            writer.Formatting = Formatting.Indented;

             //시작 엘리먼트를 생성
            writer.WriteStartDocument();

            writer.WriteStartElement("setting");

            writer.WriteElementString("ReadDate", DateTime.Now.ToString("yyyy-MM-dd"));
            writer.WriteElementString("ReadNumber", "0");

            writer.WriteEndElement();

            writer.Flush();
            writer.Close();
            //xml 파일 생성 완료
        }

        public string getNowReadNumber(string nodeName, string attName)
        {
            string retStr = "0";
            XmlDocument xml = new XmlDocument();

            xml.Load(this.filePath);
            XmlElement node = (XmlElement)xml.DocumentElement.FirstChild;

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

        public void saveReadNumer(string ReadDate, string ReadNumber)
        {
            string xmlPath = this.filePath;
            doc.Load(xmlPath);

            //공통
            updateConfigXml("ReadDate", ReadDate);
            updateConfigXml("ReadNumber", ReadNumber);
            
            doc.Save(xmlPath);
        }

        private void updateConfigXml(string nodename, string newvalue)
        {
            try
            {
                XmlNode node;
                node = doc.DocumentElement;

                foreach (XmlNode node1 in node.ChildNodes)
                {
                    if (node1.Name.Equals(nodename))
                    {
                        node1.InnerText = newvalue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
