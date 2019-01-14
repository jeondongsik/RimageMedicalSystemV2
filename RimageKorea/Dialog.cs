using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Rimage.Client.Api;

namespace RimageKorea
{
    public abstract class Dialog
    {
        private string m_ServerId;
        private string m_ClientId;
        private string m_DialogId;
        private string m_Title;
        private string m_Message;
        private int m_NumberOfButtons;
        private string m_DialogReplyXml;

        public Dialog()
        {
        }

        public Dialog(string dialog)
        {
        }

        public string ServerId
        {
            get { return m_ServerId; }
            set { m_ServerId = value; }
        }

        public string ClientId
        {
            get { return m_ClientId; }
            set { m_ClientId = value; }
        }

        public string DialogId
        {
            get { return m_DialogId; }
            set { m_DialogId = value; }
        }

        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        public string Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }

        public int NumberOfButtons
        {
            get { return m_NumberOfButtons; }
            set { m_NumberOfButtons = value; }
        }

        public string DialogReplyXml
        {
            get { return m_DialogReplyXml; }
        }

        public void GenerateReply(int button)
        {
            XmlDocument dom = new XmlDocument();
            XmlElement rootElem;
            XmlElement memberElem1;
            XmlAttribute Attribute;
            XmlProcessingInstruction newPI;
            XmlDocumentType doctype;
            string strSysFolder;
            strSysFolder = CSystemManager.GetInstance().GetUncSystemFolder();

            //Create a procesing instruction.
            newPI = dom.CreateProcessingInstruction("xml", "version='1.0'");

            //Add the processing instruction node to the document.
            dom.AppendChild(newPI);

            //Create a document type node and add it to the document.
            doctype = dom.CreateDocumentType("ProductionServerRequest", null, strSysFolder + "//XML//ProductionServerRequest_1.9.dtd", null);
            dom.AppendChild(doctype);

            //Creates root element
            rootElem = dom.CreateElement("ProductionServerRequest");
            dom.AppendChild(rootElem);

            //Creates Attribute to the root Element
            Attribute = dom.CreateAttribute("ServerId");
            Attribute.Value = ServerId;
            rootElem.Attributes.SetNamedItem(Attribute);

            //Creates Attribute to the root Element
            Attribute = dom.CreateAttribute("ClientId");
            Attribute.Value = ClientId;
            rootElem.Attributes.SetNamedItem(Attribute);

            //Create element under root element
            memberElem1 = dom.CreateElement("SetDialogAction");
            rootElem.AppendChild(memberElem1);

            //Creates Attribute to the Member Element
            Attribute = dom.CreateAttribute("DialogId");
            Attribute.Value = DialogId;
            memberElem1.Attributes.SetNamedItem(Attribute);

            //Creates Attribute to the Member Element
            string text;
            if (button == 0)
                text = "First";
            else if (button == 1)
                text = "Second";
            else
                text = "Third";

            Attribute = dom.CreateAttribute("Button");
            Attribute.Value = text;
            memberElem1.Attributes.SetNamedItem(Attribute);

            m_DialogReplyXml = dom.OuterXml;
        }
    }
}
