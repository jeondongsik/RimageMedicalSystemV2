using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace RimageReport
{
    public partial class xFormSelectFilePath : DevExpress.XtraEditors.XtraForm
    {
        private string _fileName;
        /// <summary>
        /// 선택한 파일명
        /// </summary>
        public string FileName
        {
            get { return this._fileName; }
            set { this._fileName = value; }
        }

        public xFormSelectFilePath()
        {
            InitializeComponent();
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textEditFolderName.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.textEditFolderName.Text))
            {
                MessageBox.Show("저장할 폴더를 선택하세요.");
                this.textEditFolderName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(this.textEditFileName.Text))
            {
                MessageBox.Show("파일명을 입력하세요.");
                this.textEditFileName.Focus();
                return;
            }

            this._fileName = System.IO.Path.Combine(this.textEditFolderName.Text, this.textEditFileName.Text);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}