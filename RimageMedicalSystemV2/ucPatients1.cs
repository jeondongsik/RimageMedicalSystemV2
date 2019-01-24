using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using RimageKorea;

namespace RimageMedicalSystemV2
{
    /// <summary>
    /// 굽기 타입 1
    /// </summary>
    public partial class ucPatients1 : DevExpress.XtraEditors.XtraUserControl
    {
        BurnOrderedInfoEntity _orderInfo;
        /// <summary>
        /// 굽기 명령 정보
        /// </summary>
        public BurnOrderedInfoEntity OrderInfo
        {
            get { return this._orderInfo; }
            set { this._orderInfo = value; }
        }

        public ucPatients1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 컨트럴 초기화
        /// </summary>
        public void Clear()
        {
            this.txtPatientID.EditValue = "";
            this.txtPatientName.EditValue = "";
            this.txtDataLength.EditValue = "";
            this.cbCopies.SelectedIndex = 0;
            this.cbMedia.SelectedIndex = 0;
            this.treeView1.Nodes.Clear();
        }
    }
}
