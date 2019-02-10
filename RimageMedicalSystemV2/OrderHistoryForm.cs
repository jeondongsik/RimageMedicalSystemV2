using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using RimageKorea;

namespace RimageMedicalSystemV2
{
    public partial class OrderHistoryForm : DevExpress.XtraEditors.XtraForm
    {
        public OrderHistoryForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderHistoryForm_Load(object sender, EventArgs e)
        {
            this.GetOrders();
        }

        /// <summary>
        /// 굽기명령 정보 불러오기
        /// </summary>
        private void GetOrders()
        {
            try
            {
                List<BurnOrderedInfoEntity> orderedList = new List<BurnOrderedInfoEntity>();

                string orderFolder = Path.Combine(GlobalVar.ProgramExecuteFolder, GlobalVar.ORDER_FOLDER);

                foreach (var file in Directory.GetFiles(orderFolder))
                {
                    string json = File.ReadAllText(file);
                    BurnOrderedInfoEntity order = JsonParser.ConvertToBurnOrderedInfoEntity(json);

                    if (order != null)
                    {
                        order.DeleteIcon = global::RimageMedicalSystemV2.Properties.Resources.close_16x16;
                        order.RetryIcon = global::RimageMedicalSystemV2.Properties.Resources.refreshpivottable_16x16;

                        orderedList.Add(order);
                    }
                }

                this.gcPatientlist.DataSource = orderedList;
            }
            catch { }
        }        
    }
}