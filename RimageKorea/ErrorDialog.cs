using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Xml;
using System.Xml.Schema;
using System.Windows.Forms;

namespace RimageKorea
{
    public class ErrorDialog : Dialog 
    {
        private string m_Device;
        private string m_ErrorCode;
        private string[] m_Buttons = { "", "", "" };
        private string[] m_ActionIds = { "", "", "" };

        public string Device
        {
            get { return m_Device; }
            set { m_Device = value; }
        }
        public string ErrorCode
        {
            get { return m_ErrorCode; }
            set { m_ErrorCode = value; }
        }

        public string GetButton(int index)
        {
            return m_Buttons[index];
        }
        public string GetActionId(int index)
        {
            return m_ActionIds[index];
        }

        public string SetButton(int index, string text)
        {
            string retStr = "";

            try
            {
                m_Buttons[index] = text;
                retStr = GetButton(index);
            }
            catch
            {
                retStr = "No buttons set";
            }

            return retStr;
        }

        public string SetActionId(int index, string id)
        {
            string retStr = "";

            try
            {
                m_ActionIds[index] = id;
                retStr = GetActionId(index);
            }
            catch
            {
                retStr = "No ActionId set";
            }

            return retStr;
        }

        public ErrorDialog()
        {
            //생성자
        }

        public ErrorDialog(string errorDialog)
        {
            string txt = "";
            string actId = "";
            XmlTextReader xtReader = null;
            TextReader r = null;

            r = new StringReader(errorDialog);
            xtReader = new XmlTextReader((TextReader)r);

            try
            {
                while (xtReader.Read())
                {
                    if (xtReader.NodeType == XmlNodeType.Element)
                    {
                        if (xtReader.Name.Equals("ErrorDialog"))
                        {
                            ServerId = xtReader.GetAttribute("ServerId");
                            DialogId = xtReader.GetAttribute("ID");
                            Title = xtReader.GetAttribute("Title");
                            Message = xtReader.GetAttribute("Message");
                            Device = xtReader.GetAttribute("Device");
                            ErrorCode = xtReader.GetAttribute("ErrorCode");
                        }

                        if (xtReader.Name.Equals("Top"))
                        {
                            txt += SetButton(0, xtReader.GetAttribute("Text"));
                            actId += SetActionId(0, xtReader.GetAttribute("ActionId"));
                            NumberOfButtons = NumberOfButtons + 1;
                        }
                        else if (xtReader.Name.Equals("Middle"))
                        {
                            txt += SetButton(1, xtReader.GetAttribute("Text"));
                            actId += SetActionId(1, xtReader.GetAttribute("ActionId"));
                            NumberOfButtons = NumberOfButtons + 1;
                        }
                        else if (xtReader.Name.Equals("Bottom"))
                        {
                            txt += SetButton(2, xtReader.GetAttribute("Text"));
                            actId += SetActionId(2, xtReader.GetAttribute("ActionId"));
                            NumberOfButtons = NumberOfButtons + 1;
                        }
                    }
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("ServerId = " + ServerId);
                sb.AppendLine("DialogId = " + DialogId);
                sb.AppendLine("Title = " + Title);
                sb.AppendLine("Message = " + Message);
                sb.AppendLine("Device = " + Device);
                sb.AppendLine("ErrorCode = " + ErrorCode);
                //sb.AppendLine("txt = " + txt);
                //sb.AppendLine("ActionId = " + actId);
                //sb.AppendLine("NumberOfButtons = " + NumberOfButtons.ToString());

                MessageBox.Show(sb.ToString(), Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (r != null)
                {
                    r.Close();
                }
                if (xtReader != null)
                {
                    xtReader.Close();
                }
            }
        }
    }
}
