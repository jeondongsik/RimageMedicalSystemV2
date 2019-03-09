using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Data;

namespace RimageKorea
{
    public class XMLParser
    {
        public static DiscStatus ParseOrderStatus(string orderXml)
        {
            DiscStatus discStatus = null;

            try
            {
                if (string.IsNullOrWhiteSpace(orderXml))
                    return null;

                discStatus = new DiscStatus();
                XmlDocument document = new XmlDocument();
                // Create memory stream 
                MemoryStream memStream = new MemoryStream();

                byte[] data = Encoding.Unicode.GetBytes(orderXml);
                memStream.Write(data, 0, data.Length);
                memStream.Position = 0;
                document.Load(memStream);
                // Determine the type of order status
                string docType = document.DocumentType.Name;
                discStatus.OrderType = docType;
                XmlAttributeCollection xmlAttributes;

                //string temp;

                // Read Order information
                try
                {
                    xmlAttributes = document.SelectSingleNode("//" + docType).Attributes;
                    discStatus.OrderID = xmlAttributes.GetNamedItem("OrderId").Value;
                    discStatus.ClientID = xmlAttributes.GetNamedItem("ClientId").Value;
                    discStatus.xml = xmlAttributes.GetNamedItem("OriginalOrder").Value;
                }
                catch (Exception ex)
                {
                    ////ErrorLog.LogWrite("XMLParser", ex.ToString(), Environment.CurrentDirectory);
                }

                // Read status information
                xmlAttributes = document.SelectSingleNode("//" + docType + "//Status").Attributes;
                try
                {
                    if (docType == "ProductionOrderStatus")
                    {
                        discStatus.Stage = xmlAttributes.GetNamedItem("Stage").Value;
                    }
                    discStatus.State = xmlAttributes.GetNamedItem("State").Value;
                    discStatus.PercentCompleted = xmlAttributes.GetNamedItem("PercentCompleted").Value;
                    if (docType == "ProductionOrderStatus")
                    {
                        discStatus.DeviceCurrentState = xmlAttributes.GetNamedItem("CurrentOperation").Value;
                    }
                }
                catch (Exception ex)
                {
                    ////ErrorLog.LogWrite("XMLParser", ex.ToString(), Environment.CurrentDirectory);
                }

                if (discStatus.State == "FAILED")
                {
                    discStatus.ErrorMessage = xmlAttributes.GetNamedItem("ErrorMessage").Value;
                }
            }
            catch 
            {
                ////ErrorLog.LogWrite("XMLParser", ex.ToString(), Environment.CurrentDirectory);
            }

            return discStatus;
        }

        /// <summary>
        /// Xml을 DataSet에 담은 후 원하는 테이블 반환한다.
        /// </summary>
        /// <param name="xmlString">xml string</param>
        /// <param name="tableName">반환받을 테이블명 (Xml Node명)</param>
        /// <returns></returns>
        public static DataTable ConvertXmlToDataTable(string xmlString, string tableName)
        {
            DataTable dt = new DataTable();

            try
            {
                DataSet ds = new DataSet();

                using (var stream = Utils.GenerateStreamFromString(xmlString))
                {
                    ds.ReadXml(stream);
                }

                if (ds.Tables.Contains(tableName))
                {
                    dt = ds.Tables[tableName].Clone();

                    foreach (DataRow row in ds.Tables[tableName].Rows)
                    {
                        dt.ImportRow(row);
                    }
                }

                ds.Dispose();
            }
            catch { }

            return dt;
        }
    }
}
