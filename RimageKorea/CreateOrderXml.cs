using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace RimageKorea
{
    public class CreateOrderXml
    {
        string strSysFolder;
        string strServer;
        string strClientID;

        /// <summary>
        /// 서버시스템 폴더 경로
        /// </summary>
        public string SYSFOLDER
        {
            get { return strSysFolder; }
            set { strSysFolder = value; }
        }

        /// <summary>
        /// 서버명
        /// </summary>
        public string SERVER
        {
            get { return strServer; }
            set { strServer = value; }
        }

        /// <summary>
        /// 접속아이디
        /// </summary>
        public string CLIENTID
        {
            get { return strClientID; }
            set { strClientID = value; }
        }

        public CreateOrderXml()
        {
        }

        #region Check Bins
        /// <summary>
        /// Check Bins
        /// </summary>
        public string GenerateProdRequestCheckBins(string AutoLoader)
        {
            StringBuilder sbTest = new StringBuilder();
            XmlTextWriter xmlProdRequest = new XmlTextWriter(new StringWriter(sbTest));

            xmlProdRequest.Formatting = Formatting.Indented;
            xmlProdRequest.Indentation = 4;

            xmlProdRequest.WriteStartDocument();

            try
            {
                xmlProdRequest.WriteDocType("ProductionServerRequest", null, strSysFolder + @"\XML\ProductionServerRequest_1.9.dtd", null);

            }
            catch
            {
                xmlProdRequest.WriteDocType("ProductionServerRequest", null, strSysFolder + @"\XML\ProductionServerRequest_1.1.dtd", null);
            }

            xmlProdRequest.WriteStartElement("ProductionServerRequest");

            //Write ServerID Attribute to the ProductionRequest root Element
            xmlProdRequest.WriteAttributeString("ServerId", strServer + "_PS01");

            //Write ClientId Attribute to the ProductionRequest root Element
            xmlProdRequest.WriteAttributeString("ClientId", strClientID);

            //Write getparametersettings Element
            xmlProdRequest.WriteStartElement("CheckBins");

            //Write getparametersettings Element
            xmlProdRequest.WriteAttributeString("LoaderNumber", AutoLoader);

            //End getparametersettings element
            //xmlProdRequest.WriteEndElement();

            //End getparametersettings element
            xmlProdRequest.WriteEndElement();

            //end ProductionRequest Element
            xmlProdRequest.WriteEndElement();

            //End the document and close the writer
            xmlProdRequest.WriteEndDocument();
            xmlProdRequest.Close();

            //Output XML to string
            return sbTest.ToString();
        } 
        #endregion

        #region Reset Bins
        /// <summary>
        /// Reset Bins
        /// </summary>
        public string GenerateProdRequestResetBins(string AutoLoader)
        {
            StringBuilder sbTest = new StringBuilder();
            XmlTextWriter xmlProdRequest = new XmlTextWriter(new StringWriter(sbTest));

            xmlProdRequest.Formatting = Formatting.Indented;
            xmlProdRequest.Indentation = 4;

            xmlProdRequest.WriteStartDocument();

            //Write document type
            try
            {
                xmlProdRequest.WriteDocType("ProductionServerRequest", null, strSysFolder + @"\XML\ProductionServerRequest_1.9.dtd", null);
            }
            catch
            {
                xmlProdRequest.WriteDocType("ProductionServerRequest", null, strSysFolder + @"\XML\ProductionServerRequest_1.1.dtd", null);
            }

            //Write Production request root element
            xmlProdRequest.WriteStartElement("ProductionServerRequest");

            //Write ServerID Attribute to the ProductionRequest root Element
            xmlProdRequest.WriteAttributeString("ServerId", strServer + "_PS01");

            //Write ClientId Attribute to the ProductionRequest root Element
            xmlProdRequest.WriteAttributeString("ClientId", strClientID);

            //Write getparametersettings Element
            xmlProdRequest.WriteStartElement("ResetInputBins");

            //Write getparametersettings Element
            xmlProdRequest.WriteAttributeString("LoaderNumber", AutoLoader);

            //End getparametersettings element
            //xmlProdRequest.WriteEndElement()

            //End getparametersettings element
            xmlProdRequest.WriteEndElement();

            //end ProductionRequest Element
            xmlProdRequest.WriteEndElement();

            //End the document and close the writer
            xmlProdRequest.WriteEndDocument();
            xmlProdRequest.Close();

            //Output XML to string
            return sbTest.ToString();
        } 
        #endregion
        
        #region Server Status
        /// <summary>
        /// Get AutoloaderStatus
        /// </summary>
        /// <returns></returns>
        public string GenerateProdRequestAutoloaderStatus()
        {
            StringBuilder sb = new StringBuilder();
            XmlTextWriter xmlProdRequest = new XmlTextWriter(new StringWriter(sb));

            xmlProdRequest.Formatting = Formatting.Indented;
            xmlProdRequest.Indentation = 4;

            xmlProdRequest.WriteStartDocument();

            //Write document type
            try
            {
                xmlProdRequest.WriteDocType("ProductionServerRequest", null, strSysFolder + "\\XML\\ProductionServerRequest_1.9.dtd", null);
            }
            catch
            {
                xmlProdRequest.WriteDocType("ProductionServerRequest", null, strSysFolder + "\\XML\\ProductionServerRequest_1.1.dtd", null);
            }

            //Write Production request root element
            xmlProdRequest.WriteStartElement("ProductionServerRequest");

            //Write ServerID Attribute to the ProductionRequest root Element
            xmlProdRequest.WriteAttributeString("ServerId", strServer + "_PS01");

            //Write ClientId Attribute to the ProductionRequest root Element
            xmlProdRequest.WriteAttributeString("ClientId", strClientID);

            //Write getparametersettings Element
            xmlProdRequest.WriteStartElement("GetServerStatus");

            //Write getparametersettings Element
            xmlProdRequest.WriteAttributeString("GetAutoloaderStatus", "true");

            //Write getparametersettings Element
            xmlProdRequest.WriteAttributeString("GetCommandLineSwitches", "true");

            //Write getparametersettings Element
            xmlProdRequest.WriteAttributeString("GetActivationInfo", "true");

            xmlProdRequest.WriteAttributeString("GetAdaptiveTracingInfo", "true");

            //End getparametersettings element
            //xmlProdRequest.WriteEndElement();

            //End getparametersettings element
            xmlProdRequest.WriteEndElement();

            //end ProductionRequest Element
            xmlProdRequest.WriteEndElement();

            //End the document and close the writer
            xmlProdRequest.WriteEndDocument();
            xmlProdRequest.Close();

            return sb.ToString();
        } 
        #endregion

        #region Get Serve Config
        /// <summary>
        /// Get Serve Config
        /// </summary>
        /// <returns></returns>
        public string GenerateProdRequestServeConfig()
        {
            StringBuilder sbTest = new StringBuilder();
            XmlTextWriter xmlProdRequest = new XmlTextWriter(new StringWriter(sbTest));

            xmlProdRequest.Formatting = Formatting.Indented;
            xmlProdRequest.Indentation = 4;
            xmlProdRequest.WriteStartDocument();

            //Write document type
            try
            {
                xmlProdRequest.WriteDocType("ProductionServerRequest", null, strSysFolder + "\\XML\\ProductionServerRequest_1.9.dtd", null);
            }
            catch
            {
                xmlProdRequest.WriteDocType("ProductionServerRequest", null, strSysFolder + "\\XML\\ProductionServerRequest_1.1.dtd", null);
            }


            //Write Production request root element
            xmlProdRequest.WriteStartElement("ProductionServerRequest");

            //Write ClientId Attribute to the ProductionRequest root Element
            xmlProdRequest.WriteAttributeString("ClientId", strClientID);

            //Write ServerID Attribute to the ProductionRequest root Element
            xmlProdRequest.WriteAttributeString("ServerId", strServer + "_PS01");

            //Write getparametersettings Element
            xmlProdRequest.WriteStartElement("GetParameterSettings");

            //End getparametersettings element
            xmlProdRequest.WriteEndElement();

            //end ProductionRequest Element
            xmlProdRequest.WriteEndElement();

            //End the document and close the writer
            xmlProdRequest.WriteEndDocument();
            xmlProdRequest.Close();

            //Output XML to string
            return sbTest.ToString();
        } 
        #endregion

        #region Get Order List
        /// <summary>
        /// Get Order List
        /// </summary>
        /// <returns></returns>
        public string GenerateProdRequestOrderList()
        {
            StringBuilder sbTest = new StringBuilder();
            XmlTextWriter xmlProdRequest = new XmlTextWriter(new StringWriter(sbTest));

            xmlProdRequest.Formatting = Formatting.Indented;
            xmlProdRequest.Indentation = 4;
            xmlProdRequest.WriteStartDocument();

            //Write document type
            try
            {
                xmlProdRequest.WriteDocType("ProductionServerRequest", null, strSysFolder + "\\XML\\ProductionServerRequest_1.9.dtd", null);
            }
            catch
            {
                xmlProdRequest.WriteDocType("ProductionServerRequest", null, strSysFolder + "\\XML\\ProductionServerRequest_1.1.dtd", null);
            }

            //Write Production request root element
            xmlProdRequest.WriteStartElement("ProductionServerRequest");

            //Write ClientId Attribute to the ProductionRequest root Element
            xmlProdRequest.WriteAttributeString("ClientId", strClientID);

            //Write ServerID Attribute to the ProductionRequest root Element
            xmlProdRequest.WriteAttributeString("ServerId", strServer + "_PS01");

            //Write getparametersettings Element
            xmlProdRequest.WriteStartElement("GetOrderList");

            //End getparametersettings element
            xmlProdRequest.WriteEndElement();

            //End the document and close the writer
            xmlProdRequest.WriteEndDocument();
            xmlProdRequest.Close();

            //Output XML to string
            return sbTest.ToString();
        } 
        #endregion

        #region Create Image Order - burning
        /// <summary>
        /// Create Image Order - burning
        /// </summary>
        /// <param name="orderData">attribute value class</param>
        /// <param name="serverIP">서버IP</param>
        /// <param name="RimageSystemFolder">RimageSystemFolder</param>
        /// <returns></returns>
        public static string CreateImageOrder(DiscData orderData, string serverIP, string RimageSystemFolder)
        {
            //Declare Variables:
            XmlDocument pXMLDoc = new XmlDocument();
            XmlElement pRootElement;
            XmlElement memberElement1;
            XmlElement memberElement2;
            XmlAttribute pAttribute;
            XmlProcessingInstruction newPI;
            XmlDocumentType doctype;

            // Create a procesing instruction.
            newPI = pXMLDoc.CreateProcessingInstruction("xml", "version=\"1.0\"");

            // Add the processing instruction node to the document.
            pXMLDoc.AppendChild(newPI);

            //Create a document type node and add it to the document.
            doctype = pXMLDoc.CreateDocumentType("ImageOrder", null, RimageSystemFolder + "\\XML\\ImageOrder_1.8.DTD", null);
            pXMLDoc.AppendChild(doctype);

            //Create Image Order Root Element:
            pRootElement = pXMLDoc.CreateElement("ImageOrder");
            pXMLDoc.AppendChild(pRootElement);

            //Create Attribute to Root Element:
            pAttribute = pXMLDoc.CreateAttribute("OrderId");
            pAttribute.Value = orderData.OrderID;
            pRootElement.Attributes.SetNamedItem(pAttribute);

            //Create Attribute to Root Element:
            pAttribute = pXMLDoc.CreateAttribute("ClientId");
            pAttribute.Value = orderData.ClientID;
            pRootElement.Attributes.SetNamedItem(pAttribute);

            //Create Attribute to Root Element:
            pAttribute = pXMLDoc.CreateAttribute("Originator");
            pAttribute.Value = orderData.Originator;
            pRootElement.Attributes.SetNamedItem(pAttribute);

            //Create Target Element under Root:
            memberElement1 = pXMLDoc.CreateElement("Target");
            pRootElement.AppendChild(memberElement1);

            //Create Format Element under Root:
            memberElement1 = pXMLDoc.CreateElement("Format");
            pRootElement.AppendChild(memberElement1);

            if (orderData.MediaType.Equals("CDR"))
            {
                //Create UDFFormat Member Element:
                memberElement2 = pXMLDoc.CreateElement("PCMACFormat");
                memberElement1.AppendChild(memberElement2);

                //Create Attribute to Member Element:
                //Set to iso level 2
                pAttribute = pXMLDoc.CreateAttribute("ISO");
                pAttribute.Value = "none";
                memberElement2.Attributes.SetNamedItem(pAttribute);

                pAttribute = pXMLDoc.CreateAttribute("Joliet");
                pAttribute.Value = "true";
                memberElement2.Attributes.SetNamedItem(pAttribute);
            }
            else if (orderData.MediaType.Equals("DVDR"))
            {
                memberElement2 = pXMLDoc.CreateElement("UDFFormat");
                memberElement1.AppendChild(memberElement2);

                pAttribute = pXMLDoc.CreateAttribute("UDF");
                pAttribute.Value = "102ISO";
                memberElement2.Attributes.SetNamedItem(pAttribute);

                pAttribute = pXMLDoc.CreateAttribute("Apple");
                pAttribute.Value = "none";
                memberElement2.Attributes.SetNamedItem(pAttribute);
            }
            else if (orderData.MediaType.Equals("DVDR-DL"))
            {
                memberElement2 = pXMLDoc.CreateElement("UDFFormat");
                memberElement1.AppendChild(memberElement2);

                pAttribute = pXMLDoc.CreateAttribute("UDF");
                pAttribute.Value = "102ISO";
                memberElement2.Attributes.SetNamedItem(pAttribute);

                pAttribute = pXMLDoc.CreateAttribute("Apple");
                pAttribute.Value = "none";
                memberElement2.Attributes.SetNamedItem(pAttribute);
            }

            //Create FormatOptions Member Element:
            memberElement2 = pXMLDoc.CreateElement("FormatOptions");
            memberElement1.AppendChild(memberElement2);

            pAttribute = pXMLDoc.CreateAttribute("Zip");
            pAttribute.Value = "false";
            memberElement2.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("ForceDot");
            pAttribute.Value = "false";
            memberElement2.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("Versions");
            pAttribute.Value = "false";
            memberElement2.Attributes.SetNamedItem(pAttribute);

            pAttribute = pXMLDoc.CreateAttribute("AllowBootableCD");
            pAttribute.Value = "false";
            memberElement2.Attributes.SetNamedItem(pAttribute);



            //Create Source Element under Root:
            memberElement1 = pXMLDoc.CreateElement("Source");
            pRootElement.AppendChild(memberElement1);

            //Create EditList Member Element:
            //memberElement2 = pXMLDoc.CreateElement("ParentFolder");
            //memberElement1.AppendChild(memberElement2);

            //Create Attribute to Member Element:
            //pAttribute = pXMLDoc.CreateAttribute("ParentFolderPath");
            //pAttribute.Value = orderData.ParentFolder;
            //memberElement2.Attributes.SetNamedItem(pAttribute);
            memberElement2 = pXMLDoc.CreateElement("XMLEditList");
            memberElement1.AppendChild(memberElement2);

            pAttribute = pXMLDoc.CreateAttribute("EditListPath");
            pAttribute.Value = orderData.EditListPath;
            memberElement2.Attributes.SetNamedItem(pAttribute);

            //	Create Output Element under Root:
            memberElement1 = pXMLDoc.CreateElement("Output");
            pRootElement.AppendChild(memberElement1);

            //	Create Attribute to Member Element:
            pAttribute = pXMLDoc.CreateAttribute("ImageFile");
            pAttribute.Value = RimageSystemFolder + "\\" + orderData.OrderID + ".img";
            memberElement1.Attributes.SetNamedItem(pAttribute);

            //	Create Attribute to Member Element:
            pAttribute = pXMLDoc.CreateAttribute("Type");
            pAttribute.Value = "PowerImage";
            memberElement1.Attributes.SetNamedItem(pAttribute);

            //Create Attribute to Member Element:
            pAttribute = pXMLDoc.CreateAttribute("Size");

            if (orderData.MediaType.Equals("CDR"))
                pAttribute.Value = "80";
            else if (orderData.MediaType.Equals("DVDR"))
                pAttribute.Value = "500";
            else if (orderData.MediaType.Equals("DVDR-DL"))
                pAttribute.Value = "DVD-DL";

            memberElement1.Attributes.SetNamedItem(pAttribute);

            // Create VolumeName element under root
            memberElement1 = pXMLDoc.CreateElement("VolumeName");
            pRootElement.AppendChild(memberElement1);

            // Creates Attribute to the Member Element
            pAttribute = pXMLDoc.CreateAttribute("VolName");
            pAttribute.Value = orderData.VolumeName;
            memberElement1.Attributes.SetNamedItem(pAttribute);


            return pXMLDoc.OuterXml;
        } 
        #endregion

        #region Create Production Order - burning
        /// <summary>
        /// Create Production Order - burning
        /// </summary>
        /// <param name="orderData">attribute value class</param>
        /// <param name="serverIP">서버IP</param>
        /// <param name="RimageSystemFolder">RimageSystemFolder</param>
        /// <returns></returns>
        public static string CreateProductionOrder(DiscData orderData, string serverIP, string RimageSystemFolder)
        {
            // Create the XML string of production order
            XmlDocument dom = new XmlDocument();
            XmlElement rootElem;
            XmlElement memberElem1;
            XmlElement memberElem2;
            XmlElement memberElem3;
            XmlElement memberElem4;
            XmlAttribute attribute;
            XmlProcessingInstruction newPI;
            XmlDocumentType doctype;

            // Create a procesing instruction.
            newPI = dom.CreateProcessingInstruction("xml", "version=\"1.0\"");

            // Add the processing instruction node to the document.
            dom.AppendChild(newPI);

            //Create a document type node and add it to the document.
            doctype = dom.CreateDocumentType("ProductionOrder", null, RimageSystemFolder + "\\XML\\ProductionOrder_1.10.DTD", null);
            dom.AppendChild(doctype);

            // Creates root element
            rootElem = dom.CreateElement("ProductionOrder");
            dom.AppendChild(rootElem);

            // Creates Attribute to the root Element
            attribute = dom.CreateAttribute("OrderId");
            attribute.Value = orderData.OrderID;
            rootElem.Attributes.SetNamedItem(attribute);

            // Creates Attribute to the root Element
            attribute = dom.CreateAttribute("ClientId");
            attribute.Value = orderData.ClientID;
            rootElem.Attributes.SetNamedItem(attribute);

            // Creates Attribute to the root Element
            //attribute = dom.CreateAttribute("Originator");
            //attribute.Value = orderData.Originator;
            //rootElem.Attributes.SetNamedItem(attribute);

            // Creates Attribute to the root Element
            attribute = dom.CreateAttribute("Copies");
            attribute.Value = orderData.Copies;
            rootElem.Attributes.SetNamedItem(attribute);

            // Create element under root element
            memberElem1 = dom.CreateElement("Media");
            rootElem.AppendChild(memberElem1);

            // Creates Attribute to the Member Element
            attribute = dom.CreateAttribute("Type");
            attribute.Value = orderData.MediaType;
            memberElem1.Attributes.SetNamedItem(attribute);

            // Create element under root element
            memberElem1 = dom.CreateElement("Target");
            rootElem.AppendChild(memberElem1);

            // Creates Attribute to the member Element
            attribute = dom.CreateAttribute("Server");
            attribute.Value = "ANY";
            memberElem1.Attributes.SetNamedItem(attribute);

            ////xw.WriteAttributeString("Cluster", "DefaultProductionCluster");
            ////xw.WriteAttributeString("Server", targetServer);

            // Creates Attribute to the memeber Element
            attribute = dom.CreateAttribute("Line");
            attribute.Value = "ANY";
            memberElem1.Attributes.SetNamedItem(attribute);

            // Create element under root element
            memberElem1 = dom.CreateElement("Action");
            rootElem.AppendChild(memberElem1);

            // Creates Member element
            memberElem2 = dom.CreateElement("Record");
            memberElem1.AppendChild(memberElem2);

            // Create element under Member element
            memberElem3 = dom.CreateElement("WriteTrack");
            memberElem2.AppendChild(memberElem3);

            // Creates Attribute to the Member Element
            attribute = dom.CreateAttribute("Filename");
            attribute.Value = orderData.ImagePath;
            memberElem3.Attributes.SetNamedItem(attribute);

            // Creates Attribute to the Member Element
            attribute = dom.CreateAttribute("DeleteAfterRecording");
            attribute.Value = "true";
            memberElem3.Attributes.SetNamedItem(attribute);

            // Create element under Member element
            memberElem4 = dom.CreateElement("Data");
            memberElem3.AppendChild(memberElem4);

            // Creates Attribute to the Member Element
            attribute = dom.CreateAttribute("Type");
            attribute.Value = "Mode1";
            memberElem4.Attributes.SetNamedItem(attribute);

            // Create element under Member element
            memberElem1 = dom.CreateElement("Action");
            rootElem.AppendChild(memberElem1);

            // Creates Member element
            memberElem2 = dom.CreateElement("Record");
            memberElem1.AppendChild(memberElem2);

            memberElem3 = dom.CreateElement("Fixate");
            memberElem2.AppendChild(memberElem3);

            // Creates Attribute to the Member Element
            attribute = dom.CreateAttribute("Type");
            attribute.Value = "SAO";
            memberElem3.Attributes.SetNamedItem(attribute);

            // Creates Attribute to the Member Element
            attribute = dom.CreateAttribute("Final");
            attribute.Value = "true";
            memberElem3.Attributes.SetNamedItem(attribute);

            if (orderData.UseLabelPrint == true)
            {
                // Create element under Member element
                memberElem1 = dom.CreateElement("Action");
                rootElem.AppendChild(memberElem1);

                // Create element under Member element
                memberElem2 = dom.CreateElement("Label");
                memberElem1.AppendChild(memberElem2);

                // Creates Attribute to the Member Element
                attribute = dom.CreateAttribute("Filename");
                attribute.Value = orderData.LabelName;
                memberElem2.Attributes.SetNamedItem(attribute);

                // Create element under Member element
                memberElem3 = dom.CreateElement("BTW");
                memberElem2.AppendChild(memberElem3);

                attribute = dom.CreateAttribute("Merge_Filename");
                attribute.Value = orderData.MergeName;
                memberElem3.Attributes.SetNamedItem(attribute);

                attribute = dom.CreateAttribute("DeleteMergeFileOnCompletion");
                attribute.Value = "true";
                memberElem3.Attributes.SetNamedItem(attribute);
            }

            return dom.OuterXml.ToString();
        } 
        #endregion	

        #region Bulk Read - Reading Order
        /// <summary>
        /// Bulk Read - Reading Order
        /// </summary>
        /// <param name="MyOrderID">명령번호</param>
        /// <param name="copies">복사횟수</param>
        /// <param name="mediaType">미디어(CDR, DVDR)</param>
        /// <param name="size">미디어 사이즈</param>
        /// <param name="Autoloader">Autoloader</param>
        /// <param name="TrackNumber">TrackNumber</param>
        /// <param name="InputBin">InputBin</param>
        /// <param name="sourceFolder"></param>
        /// <param name="targetFolder"></param>
        /// <returns></returns>
        public string GenerateProdOrderBulkReading(string MyOrderID, int copies, string mediaType, string size, string Autoloader,
            string TrackNumber, string InputBin, string sourceFolder, string targetFolder)
        {
            string server = "ANY";
            StringBuilder sbTest = new StringBuilder();
            XmlTextWriter xmlTextImage = new XmlTextWriter(new StringWriter(sbTest));

            xmlTextImage.Formatting = Formatting.Indented;
            xmlTextImage.Indentation = 5;

            xmlTextImage.WriteStartDocument();

            //Write document type
            try
            {
                xmlTextImage.WriteDocType("ProductionOrder", null, strSysFolder + @"\XML\ProductionOrder_1.11.DTD", null);
            }
            catch
            {
                xmlTextImage.WriteDocType("ProductionOrder", null, strSysFolder + @"\XML\ProductionOrder_1.1.DTD", null);
            }

            //Write ImageOrder root element
            xmlTextImage.WriteStartElement("ProductionOrder");
            //Write OrderId Attribute to the ImageOrder root Element
            xmlTextImage.WriteAttributeString("Copies", copies.ToString());
            //Write OrderId Attribute to the ImageOrder root Element
            xmlTextImage.WriteAttributeString("OrderId", MyOrderID);
            //Write ClientId Attribute to the ImageOrder root Element
            xmlTextImage.WriteAttributeString("ClientId", strClientID);

            //Write Target Element
            xmlTextImage.WriteStartElement("Media");

            //Write Target Attribute
            xmlTextImage.WriteAttributeString("Size", size);

            if (mediaType.Equals("DVDR"))
                //Write Target Attribute
                xmlTextImage.WriteAttributeString("Type", "DVDR");
            else
                //Write Target Attribute
                xmlTextImage.WriteAttributeString("Type", "CDR");

            //end target element
            xmlTextImage.WriteEndElement();

            //Write Target Element
            xmlTextImage.WriteStartElement("Target");

            //Write Target Attribute
            //Lets add some code to choose the autoloader
            //string Autoloader = "ANY"; //Any, Autoloader1, Autoloader2
            //string server = "ANY";
            //string TrackNumber = "1";

            if (Autoloader == "ANY")
            {
                Autoloader = "ANY";
                server = "ANY";
            }
            else if (Autoloader == "Autoloader1")
            {
                Autoloader = "1";
                server = strServer + "_IS01";
            }
            else if (Autoloader == "Autoloader2")
            {
                Autoloader = "2";
                server = strServer + "_PS01";
            }

            xmlTextImage.WriteAttributeString("Line", Autoloader);
            xmlTextImage.WriteAttributeString("Server", server);

            xmlTextImage.WriteAttributeString("Cluster", "DefaultProductionCluster");
            //end target element
            xmlTextImage.WriteEndElement();

            //There are three parts to the ReadFunction

            xmlTextImage.WriteStartElement("InOut");
            xmlTextImage.WriteAttributeString("InputBin", InputBin);
            xmlTextImage.WriteEndElement();


            //Part 1: Read the Map
            //Create Action Element
            xmlTextImage.WriteStartElement("Action");

            //Create Read Order
            xmlTextImage.WriteStartElement("Read");

            //Create CallReadTrack
            xmlTextImage.WriteStartElement("Map");

            xmlTextImage.WriteAttributeString("Filename", strSysFolder + "\\" + sourceFolder + "\\" + MyOrderID + "map.xml");

            xmlTextImage.WriteAttributeString("Quick", "true");
            //End Read Element
            xmlTextImage.WriteEndElement();

            //End Action Element
            xmlTextImage.WriteEndElement();

            //Write EditList end element
            xmlTextImage.WriteEndElement();

            //Part 2: Choose the track and where to put it

            //Create Action Element
            xmlTextImage.WriteStartElement("Action");

            //Create Read Order
            xmlTextImage.WriteStartElement("Read");

            xmlTextImage.WriteAttributeString("ReadSpeed", "Maximum");

            //Create CallReadTrack
            xmlTextImage.WriteStartElement("ReadTrack");

            xmlTextImage.WriteAttributeString("Filename", strSysFolder + "\\" + sourceFolder + "\\" + targetFolder);
            xmlTextImage.WriteAttributeString("TrackNumber", TrackNumber);
            xmlTextImage.WriteAttributeString("ExtractISOFiles", "true");


            //End Read Element
            xmlTextImage.WriteEndElement();

            //End Action Element
            xmlTextImage.WriteEndElement();

            //Write EditList end element
            xmlTextImage.WriteEndElement();

            //Part 3: Terminate the Read
            //Create Action Element
            xmlTextImage.WriteStartElement("Action");

            //Create Read Order
            xmlTextImage.WriteStartElement("Read");

            //Create CallReadTrack
            xmlTextImage.WriteStartElement("TerminateRead");

            //End Read Element
            xmlTextImage.WriteEndElement();

            //End Action Element
            xmlTextImage.WriteEndElement();

            //Write EditList end element
            xmlTextImage.WriteEndElement();

            xmlTextImage.Close();

            //Output XML to string
            return sbTest.ToString();
        }
        #endregion
    }
}
