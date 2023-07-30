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

        /// <summary>
        /// EditList
        /// </summary>
        public List<string> EditList { get; set; }

        /// <summary>
        /// 복사해야할 폴더,파일 목록
        /// </summary>
        public List<string> IncList { get; set; }
        
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
                if (GlobalVar.configEntity.FolderPattern == "9")
                {
                    //// Compumedics ProFusion EEG 경우에는 통째로 복사
                    this.EditList = FileControl.CopyFolderAndFilesAll(this.SourceDirectory, this.TargetDirectory, null);
                }
                else
                {
                    //// 톰텍뷰어폴더의 파일들을 환자폴더로 이동한다.
                    this.EditList = FileControl.CopyFolderAndFiles(this.SourceDirectory, this.TargetDirectory, this.SourceDirectory, null, this.IncList);
                }
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