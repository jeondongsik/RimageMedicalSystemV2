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

namespace RimageMedicalSystemV2
{
    /// <summary>
    /// 환자정보를 USB로 복사한다.
    /// </summary>
    public partial class frmCopyToUSB : DevExpress.XtraEditors.XtraForm
    {
        List<DriveInfo> usbList;
        List<DriveInfo> others;
        DriveInfo seletedUSB;

        public frmCopyToUSB()
        {
            InitializeComponent();
        }

        private void frmCopyToUSB_Load(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;

            this.FindUsb();
        }

        /// <summary>
        /// USB 드라이버 찾기
        /// </summary>
        public void FindUsb()
        {
            try
            {
                this.usbList = new List<DriveInfo>();
                this.others = new List<DriveInfo>();

                foreach (var di in DriveInfo.GetDrives())
                {
                    if (di.IsReady && di.DriveType == DriveType.Removable)
                    {
                        this.usbList.Add(di);
                    }
                    else
                    {
                        this.others.Add(di);
                    }
                }

                if (this.usbList.Count > 0)
                {
                    int i = 0;
                    foreach (var di in this.usbList)
                    {
                        this.flpDrives.Controls.Add(this.CreateUsbButton(i, di));
                        i++;
                    }

                    if (i == 1)
                    {
                        //// 바로 복사 시작
                        this.seletedUSB = this.usbList[0];

                        ////
                    }
                }
                else
                {
                    this.txtProgressView.AppendText("인식할 수 있는 USB 드라이브가 없습니다.\r\nUSB를 컴퓨터에 연결하세요.\r\n");
                }

                ////if (this.others.Count > 0)
                ////{
                ////    int j = 11;
                ////    foreach (var di in this.others)
                ////    {
                ////        this.flpDrives.Controls.Add(this.CreateDriveButton(j, di));
                ////        j++;
                ////    }
                ////}
            }
            catch { }
        }

        /// <summary>
        /// USB 드라이버 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUSBDrive_Click(object sender, EventArgs e)
        {
            try
            {
                this.seletedUSB = ((sender as SimpleButton).Tag) as DriveInfo;
            }
            catch { }
        }

        /// <summary>
        /// USB 버튼 생성한다.
        /// </summary>
        /// <returns></returns>
        private SimpleButton CreateUsbButton(int idx, DriveInfo di)
        {
            SimpleButton btnNew = new SimpleButton();

            btnNew.Appearance.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnNew.Appearance.Options.UseFont = true;
            btnNew.AppearancePressed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            btnNew.AppearancePressed.Options.UseBorderColor = true;
            btnNew.ImageOptions.Image = global::RimageMedicalSystemV2.Properties.Resources.icons8_usb_2_32;
            btnNew.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            btnNew.Location = new System.Drawing.Point(3, 3);
            btnNew.Name = string.Format("btnUSBDrive{0}", idx.ToString());
            btnNew.Size = new System.Drawing.Size(106, 81);
            btnNew.TabIndex = idx;
            btnNew.Text = di.Name;
            btnNew.Tag = di;
            btnNew.Click += new System.EventHandler(this.btnUSBDrive_Click);

            return btnNew;
        }

        /// <summary>
        /// USB 이외의 드라이브 버튼 생성
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="di"></param>
        /// <returns></returns>
        private SimpleButton CreateDriveButton(int idx, DriveInfo di)
        {
            SimpleButton btnNew = new SimpleButton();

            btnNew.Appearance.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnNew.Appearance.Options.UseFont = true;
            btnNew.ImageOptions.Image = global::RimageMedicalSystemV2.Properties.Resources.icons8_c_drive_32;
            btnNew.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            btnNew.Location = new System.Drawing.Point(3, 90);
            btnNew.Name = "btnDrive";
            btnNew.Size = new System.Drawing.Size(106, 81);
            btnNew.TabIndex = idx;
            btnNew.Text = di.Name;
            btnNew.Tag = di;
            btnNew.Click += new System.EventHandler(this.btnDrive_Click);

            return btnNew;
        }

        private void btnDrive_Click(object sender, EventArgs e)
        {

        }

        private void btnStartCopy_Click(object sender, EventArgs e)
        {

        }
    }
}