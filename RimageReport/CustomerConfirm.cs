using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using RimageKorea;

namespace RimageReport
{
    public partial class CustomerConfirm : DevExpress.XtraReports.UI.XtraReport
    {
        BurnOrderedInfoEntity _burnInfo;
        ArrayList _listDataSource = new ArrayList();

        /// <summary>
        /// 굽기 정보
        /// </summary>
        public BurnOrderedInfoEntity BurnInfo
        {
            get { return this._burnInfo; }
            set { this._burnInfo = value; }
        }
        
        /// <summary>
        /// 생성자
        /// </summary>
        public CustomerConfirm()
        {
            InitializeComponent();
            SetBottomImage();
        }

        /// <summary>
        /// Bottom이미지
        /// </summary>
        public void SetBottomImage()
        {
            if (GlobalVar.BottomImage != null)
            {
                this.pbCenterLogo.Image = GlobalVar.BottomImage;                
            }
        }
    }
}
