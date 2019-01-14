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
