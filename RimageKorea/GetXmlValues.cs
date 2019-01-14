using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RimageKorea
{
    public class GetXmlValues
    {
        public GetXmlValues()
        {
        }

        public string getValues(XmlDocument doc, string nodeName, string attName)
        {
            string retStr = "";
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
    }
}
