using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using System.Xml.Schema;

namespace RimageKorea
{
    public class DiscData
    {
        public DiscData()
        {
            clientId = "Client01";
            orderId = RandomOrderNumber.GetNewOrderNumber().PadRight(7, '0') + DateTime.Now.Millisecond.ToString().PadRight(3, '0');
            percentComplete = "0 %";
            ProductionOrderPath = "\\Rimage\\";
            multiPatientYN = "N";
        }

        public DiscData(string ClientID, string OrderID)
        {
            clientId = ClientID;
            orderId = OrderID;
        }

        private int myVar;
        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }

        private string clientId;
        public string ClientID
        {
            get
            {
                return clientId;
            }
            set
            {
                clientId = value;
            }
        }

        /// <summary>
        /// Unique Id for the current order
        /// </summary>
        /// 
        private string orderId;
        public string OrderID
        {
            get
            {
                return orderId;
            }
            set
            {
                orderId = value;
            }
        }

        private string originator;
        public string Originator
        {
            get
            {
                return originator;
            }
            set
            {
                originator = value;
            }
        }

        /// <summary>
        /// Optional parameter for targeting a specific server
        /// </summary>
        private string target;
        public string Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        private string format;
        public string Format
        {
            get
            {
                return format;
            }
            set
            {
                format = value;
            }
        }

        /// <summary>
        /// specify parent or edit list
        /// </summary>
        /// 
        private string source;
        public string Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
            }
        }

        /// <summary>
        /// Specify Volume Label for Disc
        /// </summary>
        /// 
        private string volumeName = "RimageMedicalDisc";
        public string VolumeName
        {
            get
            {
                return volumeName;
            }
            set
            {
                volumeName = value;
            }
        }

        private string labelName;
        public string LabelName
        {
            get
            {
                return labelName;
            }
            set
            {
                labelName = value;
            }
        }

        private string mergeName;
        public string MergeName
        {
            get
            {
                return mergeName;
            }
            set
            {
                mergeName = value;
            }
        }

        private string imagePath;
        public string ImagePath
        {
            get
            {
                return imagePath;
            }
            set
            {
                imagePath = value;
            }
        }

        private string parentFolder;
        public string ParentFolder
        {
            get
            {
                return parentFolder;
            }
            set
            {
                parentFolder = value;
            }
        }

        private string editListPath;
        public string EditListPath
        {
            get
            {
                return editListPath;
            }
            set
            {
                editListPath = value;
            }
        }

        private string orderStatus;
        public string OrderStatus
        {
            get
            {
                return orderStatus;
            }
            set
            {
                orderStatus = value;
            }
        }

        private string percentComplete;
        public string PercentComplete
        {
            get
            {
                return percentComplete;
            }
            set
            {
                percentComplete = value;
            }
        }

        private string productionOrderPath;
        public string ProductionOrderPath
        {
            get
            {
                return productionOrderPath;
            }
            set
            {
                productionOrderPath = value;
            }
        }

        private string durable;
        public string Durable
        {
            get
            {
                return durable;
            }
            set
            {
                durable = value;
            }
        }

        private string mediaType;
        public string MediaType
        {
            get
            {
                return mediaType;
            }
            set
            {
                mediaType = value;
            }
        }

        private string copies;
        public string Copies
        {
            get
            {
                return copies;
            }
            set
            {
                copies = value;
            }
        }

        private bool useLabelPrint;
        public bool UseLabelPrint
        {
            get
            {
                return useLabelPrint;
            }
            set
            {
                useLabelPrint = value;
            }
        }

        private int orderViewIndex;
        public int OrderViewIndex
        {
            get
            {
                return orderViewIndex;
            }
            set
            {
                orderViewIndex = value;
            }
        }

        private string multiPatientYN;
        public string MultiPatientYN
        {
            get
            {
                return multiPatientYN;
            }
            set
            {
                multiPatientYN = value;
            }
        }
    }

    public class DiscStatus
    {
        public string OrderType;
        public string OrderID;
        public string ClientID;
        public string ServerID;
        public string Originator;
        public string Status;
        public string Stage;
        public string State;
        public string PercentCompleted;
        public string ErrorMessage;
        public string ErrorCode;
        public string DeviceCurrentState;
        public string xml;
        public string Cluster;
    }
}
