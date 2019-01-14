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
    public class AlertDialog : Dialog 
    {
        string[] m_Buttons = { "", "" };

        public string GetButton(int index)
        {
            return m_Buttons[index];
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

        public AlertDialog()
        {
            //m_Buttons[0] = "";
            //m_Buttons[1] = "";
        }

        public AlertDialog(string alertDialog)
        {
            string Alerts = "true";
            string txt = "";
            XmlTextReader reader = null;
            TextReader r = null;

            r = new StringReader(alertDialog);
            reader = new XmlTextReader((TextReader)r);

            try
            {
                while (reader.Read())
                {
                    if (reader.Name.Equals("AlertDialog") && reader.NodeType == XmlNodeType.Element)
                    {
                        ServerId = reader.GetAttribute("ServerId");
                        DialogId = reader.GetAttribute("ID");
                        Title = reader.GetAttribute("Title");
                        Message = reader.GetAttribute("Message");

                        if (Alerts.ToUpper() == "TRUE")
                        {
                            string filespec = "RimageClient-Alerts.XML";
                            StreamWriter sw = File.AppendText(filespec);
                            sw.WriteLine("AlertDialog");
                            sw.WriteLine("");
                            sw.WriteLine(ServerId);
                            sw.WriteLine(DialogId);
                            sw.WriteLine(Title);
                            sw.WriteLine(Message);
                            sw.WriteLine("End Alert Dialog");
                            sw.WriteLine("");
                            sw.Flush();
                            sw.Close();
                        }
                    }

                    if (reader.Name.Equals("OneButton") && reader.NodeType == XmlNodeType.Element)
                    {
                        txt = SetButton(0, reader.GetAttribute("Text"));
                        NumberOfButtons = 1;
                    }
                    else if (reader.Name.Equals("TwoButton") && reader.NodeType == XmlNodeType.Element)
                    {
                        NumberOfButtons = 2;
                    }
                    else if (reader.Name.Equals("Left") && reader.NodeType == XmlNodeType.Element)
                    {
                        txt = SetButton(0, reader.GetAttribute("Text"));
                    }
                    else if (reader.Name.Equals("Right") && reader.NodeType == XmlNodeType.Element)
                    {
                        txt = SetButton(1, reader.GetAttribute("Text"));
                    }
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("ServerId = " + ServerId);
                sb.AppendLine("DialogId = " + DialogId);
                sb.AppendLine("Title = " + Title);
                sb.AppendLine("Message = " + Message);
                sb.AppendLine("txt = " + txt);
                sb.AppendLine("NumberOfButtons = " + NumberOfButtons.ToString());

                MessageBox.Show(sb.ToString(), Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}
