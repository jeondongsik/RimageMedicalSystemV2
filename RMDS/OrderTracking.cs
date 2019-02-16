using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using Rimage.Client.Api;
using Rimage.Client.Api.Exception;
using System.Windows.Forms;
using RimageKorea;

namespace RMDS
{
    class OrderTracking
    {
        public static string AppDirectory = Directory.GetCurrentDirectory();

        // Create XML Document for persistant Order information
        public static void CreateDocument()
        {
            XmlDocument document = new XmlDocument();
            XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", Encoding.UTF8.HeaderName, null);
            document.AppendChild(declaration);
            XmlElement documentElement = document.CreateElement("PendingOrders");
            document.AppendChild(documentElement);

            // create the orders element
            XmlElement ordersElement = document.CreateElement("orders");
            document.DocumentElement.AppendChild(ordersElement);
            document.Save(GlobalVar.OrderTrackingFile);
        }

        // Add Persistant Order information to XML file
        public static void AddOrder(DiscData discOrder)
        {
            // Make sure file exists before attempting to add orders
            if (!File.Exists(GlobalVar.OrderTrackingFile)) CreateDocument();

            XmlDocument document = new XmlDocument();
            document.Load(GlobalVar.OrderTrackingFile);
            // Create a document fragment to contain the XML to be inserted.
            XmlDocumentFragment order = document.CreateDocumentFragment();
            // Set the contents of the document fragment.
            XmlDocumentFragment orderxml = document.CreateDocumentFragment();

            string newOrder = "<order OrderId = \"" + discOrder.OrderID + "\"" + " ClientId=" + "\"" + discOrder.ClientID + "\"" +
                      " Status=" + "\"" + discOrder.OrderStatus + "\"" + " PercentComplete= " + "\"" + discOrder.PercentComplete + "\"" + " ProductionOrderPath= " + "\"" + discOrder.ProductionOrderPath + "\"" + " Durable=" + "\"" + discOrder.Durable + "\"" + "/>";

            orderxml.InnerXml = newOrder;
            XmlNode currNode = document.DocumentElement.FirstChild;
            currNode.InsertAfter(orderxml, currNode.LastChild);
            document.Save(GlobalVar.OrderTrackingFile);
        }

        // Remove Persistant order information
        public static void RemoveOrder(DiscStatus discOrder)
        {
            // Make sure file exists before attempting to remove orders
            if (System.IO.File.Exists(GlobalVar.OrderTrackingFile))
            {
                XmlDocument document = new XmlDocument();
                try
                {
                    document.Load(GlobalVar.OrderTrackingFile);
                    XmlNode completedOrder;
                    completedOrder = document.SelectSingleNode("//PendingOrders/orders/order[@OrderId=" + "\'" + discOrder.OrderID + "\']");

                    // if Order is not found do not attempt to update
                    if (completedOrder != null)
                    {
                        XmlNode parentNode;
                        parentNode = completedOrder.ParentNode;
                        parentNode.RemoveChild(completedOrder);
                        document.Save(GlobalVar.OrderTrackingFile);
                    }
                }
                catch (IOException ioEx)
                {
                    MessageBox.Show("Error reading persistant order information" + "\n" + ioEx.Message);
                }
            }
        }

        // Update Persistant order information
        public static void ModifyOrder(DiscStatus discOrder)
        {
            if (System.IO.File.Exists(GlobalVar.OrderTrackingFile))
            {
                XmlDocument document = new XmlDocument();
                try
                {
                    document.Load(GlobalVar.OrderTrackingFile);
                    XmlElement Order;
                    Order = (XmlElement)document.SelectSingleNode("//PendingOrders/orders/order[@OrderId=" + "\'" + discOrder.OrderID + "\']");

                    // if Order is not found do not attempt to update
                    if (Order != null)
                    {
                        Order.SetAttribute("Status", discOrder.State);
                        Order.SetAttribute("PercentComplete", discOrder.PercentCompleted);
                        document.Save(GlobalVar.OrderTrackingFile);
                    }
                }
                catch (IOException ioEx)
                {
                    MessageBox.Show("Error reading persistant order information" + "\n" + ioEx.Message);
                }
            }
        }

        // Scan order file for recoverable orders
        public static ArrayList GetRecoverableStatus()
        {
            // Make sure file exists before attempting scan
            if (System.IO.File.Exists(GlobalVar.OrderTrackingFile))
            {
                XmlDocument document = new XmlDocument();
                document.Load(GlobalVar.OrderTrackingFile);
                XmlNodeList Order;
                Order = (XmlNodeList)document.SelectNodes("//PendingOrders/orders/*");

                ArrayList recoverableStatuses = new ArrayList();

                foreach (XmlNode node in Order)
                {
                    DiscStatus status;
                    status = new DiscStatus();
                    string durable = node.Attributes.GetNamedItem("Durable").Value;
                    if (durable == "true")
                    {
                        status.OrderID = node.Attributes.GetNamedItem("OrderId").Value;
                        status.ClientID = node.Attributes.GetNamedItem("ClientId").Value;
                        status.Status = node.Attributes.GetNamedItem("Status").Value;
                        recoverableStatuses.Add(status);
                        status = null;
                    }
                    else
                    {
                        // order is not durable it cannot be recovered
                        status.OrderID = node.Attributes.GetNamedItem("OrderId").Value;
                        status.ClientID = node.Attributes.GetNamedItem("ClientId").Value;
                        status.Status = node.Attributes.GetNamedItem("Status").Value;
                        OrderTracking.DeleteProductionFile(status.OrderID);
                        OrderTracking.RemoveOrder(status);
                    }
                }

                if (recoverableStatuses.Count == 0)
                {
                    return null;
                }
                else
                {
                    return recoverableStatuses;
                }
            }
            else
            {
                return null;
            }
        }

        // Save production order xml for later use
        public static void SaveProducitonFile(string xml, string fileName)
        {
            try
            {
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine(xml);
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // Delete production order xml file    
        public static void DeleteProductionFile(string orderId)
        {

            string fileToDelete = Path.Combine(AppDirectory, orderId + ".pOrd");
            //make sure file exits first
            if (File.Exists(fileToDelete))
            {
                try
                {
                    File.Delete(fileToDelete);
                }
                catch (IOException e)
                {
                    MessageBox.Show(e.Message + "\n" + "Failed to remove production order file", "Rimage SNUH");
                }
            }
        }

        // Read production order file 
        public static string GetProductionOrderFile(DiscStatus orderInfo)
        {
            string productionXml = null;

            // Get produciton order File Name
            XmlDocument document = new XmlDocument();
            document.Load(GlobalVar.OrderTrackingFile);
            XmlElement order;
            order = (XmlElement)document.SelectSingleNode("//PendingOrders/orders/order[@OrderId=" + "\'" + orderInfo.OrderID + "\']");
            if (order != null)
            {
                string pfilename = order.GetAttribute("ProductionOrderPath");

                // Read production order file into string
                try
                {
                    System.IO.StreamReader pFile = new System.IO.StreamReader(pfilename);
                    productionXml = pFile.ReadToEnd();
                    pFile.Close();
                }
                catch (Exception)
                {

                    return null;
                }
            }

            return productionXml;
        }
    }
}
