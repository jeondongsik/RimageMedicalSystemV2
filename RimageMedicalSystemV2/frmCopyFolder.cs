using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using RimageKorea;

namespace RimageMedicalSystemV2
{
    public partial class frmCopyFolder : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 복사할 원 디렉토리
        /// </summary>
        public string SourceDirectory { get; set; }
        /// <summary>
        /// 복사할 대상 디렉토리
        /// </summary>
        public string TargetDirectory { get; set; }

        public frmCopyFolder()
        {
            InitializeComponent();
        }

        private void frmCopyFolder_Load(object sender, EventArgs e)
        {
            try
            {
                this.backgroundWorker1.RunWorkerAsync();
            }
            catch { }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //// 톰텍뷰어폴더의 파일들을 환자폴더로 이동한다.
                FileControl.CopyFolderAndFiles(this.SourceDirectory, this.TargetDirectory, this.SourceDirectory);
            }
            catch { }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch { }
        }
    }
}