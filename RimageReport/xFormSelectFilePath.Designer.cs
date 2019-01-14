namespace RimageReport
{
    partial class xFormSelectFilePath
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(xFormSelectFilePath));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.textEditFolderName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.textEditFileName = new DevExpress.XtraEditors.TextEdit();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnFolder = new DevExpress.XtraEditors.SimpleButton();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.textEditFolderName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditFileName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(23, 22);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(70, 17);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "저장할 폴더";
            // 
            // textEditFolderName
            // 
            this.textEditFolderName.Location = new System.Drawing.Point(99, 19);
            this.textEditFolderName.Name = "textEditFolderName";
            this.textEditFolderName.Size = new System.Drawing.Size(320, 20);
            this.textEditFolderName.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Location = new System.Drawing.Point(23, 48);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(39, 17);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "파일명";
            // 
            // textEditFileName
            // 
            this.textEditFileName.Location = new System.Drawing.Point(99, 45);
            this.textEditFileName.Name = "textEditFileName";
            this.textEditFileName.Size = new System.Drawing.Size(320, 20);
            this.textEditFileName.TabIndex = 3;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.Appearance.Options.UseFont = true;
            this.btnConfirm.Image = ((System.Drawing.Image)(resources.GetObject("btnConfirm.Image")));
            this.btnConfirm.Location = new System.Drawing.Point(134, 113);
            this.btnConfirm.LookAndFeel.SkinName = "Money Twins";
            this.btnConfirm.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(124, 46);
            this.btnConfirm.TabIndex = 4;
            this.btnConfirm.Text = "저장";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(264, 113);
            this.btnCancel.LookAndFeel.SkinName = "Office 2010 Blue";
            this.btnCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(124, 46);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "취소";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnFolder
            // 
            this.btnFolder.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFolder.Appearance.Options.UseFont = true;
            this.btnFolder.Image = ((System.Drawing.Image)(resources.GetObject("btnFolder.Image")));
            this.btnFolder.Location = new System.Drawing.Point(425, 18);
            this.btnFolder.LookAndFeel.SkinName = "Office 2010 Silver";
            this.btnFolder.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(89, 47);
            this.btnFolder.TabIndex = 6;
            this.btnFolder.Text = "폴더선택";
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl3.Location = new System.Drawing.Point(106, 71);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(190, 17);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "(파일확장자는 입력하지 마세요.)";
            // 
            // xFormSelectFilePath
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(526, 171);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.btnFolder);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.textEditFileName);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.textEditFolderName);
            this.Controls.Add(this.labelControl1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "xFormSelectFilePath";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "저장할 폴더와 파일명을 입력하세요.";
            ((System.ComponentModel.ISupportInitialize)(this.textEditFolderName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditFileName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit textEditFolderName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit textEditFileName;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}