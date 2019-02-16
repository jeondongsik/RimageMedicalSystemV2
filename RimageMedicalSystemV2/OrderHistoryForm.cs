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
        private MainForm _mainForm;
        private List<BurnOrderedInfoEntity> _orderedList;

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
            this._mainForm = this.Owner as MainForm;

            this.GetOrders();
        }

        /// <summary>
        /// 굽기명령 정보 불러오기
        /// </summary>
        private void GetOrders()
        {
            try
            {
                this._orderedList = new List<BurnOrderedInfoEntity>();

                string orderFolder = Path.Combine(GlobalVar.ProgramExecuteFolder, GlobalVar.ORDER_FOLDER);

                foreach (var file in Directory.GetFiles(orderFolder))
                {
                    if (file.Contains("_ORD.json"))
                    {
                        string json = File.ReadAllText(file);
                        BurnOrderedInfoEntity order = JsonParser.ConvertToBurnOrderedInfoEntity(json);

                        if (order != null)
                        {
                            order.DeleteIcon = global::RimageMedicalSystemV2.Properties.Resources.close_16x16;
                            order.RetryIcon = global::RimageMedicalSystemV2.Properties.Resources.refreshpivottable_16x16;

                            this._orderedList.Add(order);
                        }
                    }
                }

                this.gcPatientlist.DataSource = this._orderedList;
            }
            catch { }
        }

        /// <summary>
        /// 재굽기 or 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvPatientlist_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            //// 더블클릭
            if (e.Clicks == 2)
            {
                if (e.RowHandle < 0)
                    return;

                try
                {
                    if (e.Column == this.gridColRetry)
                    {
                        //// 재굽기 실행
                        if (MessageBox.Show("선택한 환자 정보 다시굽기를 실행합니다.", "Rimage Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (this._mainForm != null)
                            {
                                var orderInfo = this._orderedList[this.gvPatientlist.GetDataSourceRowIndex(e.RowHandle)];
                                this._mainForm.StartBurn(orderInfo);
                            }
                        }
                    }

                    if (e.Column == this.gridColDelete)
                    {
                        //// 삭제
                        if (MessageBox.Show("선택한 환자 폴더를 삭제하겠습니까?", "Rimage Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            int idx = this.gvPatientlist.GetDataSourceRowIndex(e.RowHandle);

                            //// 환자 폴더 삭제
                            this.Cursor = Cursors.WaitCursor;
                            FileControl.DeletePatFolder(this._orderedList[idx].patFolderFullPath);
                            this.Cursor = Cursors.Default;

                            this._orderedList.RemoveAt(idx);

                            this.gvPatientlist.RefreshData();
                            this.gcPatientlist.RefreshDataSource();
                        }
                    }
                }
                catch { }
            }
        }
    }
}