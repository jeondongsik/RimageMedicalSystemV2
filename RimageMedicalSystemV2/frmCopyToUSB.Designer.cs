﻿namespace RimageMedicalSystemV2
{
    partial class frmCopyToUSB
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCopyToUSB));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnReFind = new DevExpress.XtraEditors.SimpleButton();
            this.progressBarControl1 = new DevExpress.XtraEditors.ProgressBarControl();
            this.btnStartCopy = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.txtProgressView = new System.Windows.Forms.TextBox();
            this.grpDrives = new DevExpress.XtraEditors.GroupControl();
            this.flpDrives = new System.Windows.Forms.FlowLayoutPanel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpDrives)).BeginInit();
            this.grpDrives.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnReFind);
            this.panelControl1.Controls.Add(this.progressBarControl1);
            this.panelControl1.Controls.Add(this.btnStartCopy);
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Controls.Add(this.txtProgressView);
            this.panelControl1.Controls.Add(this.grpDrives);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(358, 519);
            this.panelControl1.TabIndex = 0;
            // 
            // btnReFind
            // 
            this.btnReFind.AllowFocus = false;
            this.btnReFind.Appearance.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnReFind.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnReFind.Appearance.Options.UseFont = true;
            this.btnReFind.Appearance.Options.UseForeColor = true;
            this.btnReFind.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnReFind.ImageOptions.Image")));
            this.btnReFind.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnReFind.Location = new System.Drawing.Point(12, 173);
            this.btnReFind.LookAndFeel.SkinName = "Lilian";
            this.btnReFind.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnReFind.Name = "btnReFind";
            this.btnReFind.Size = new System.Drawing.Size(94, 29);
            this.btnReFind.TabIndex = 59;
            this.btnReFind.Text = "Refresh";
            this.btnReFind.Click += new System.EventHandler(this.btnReFind_Click);
            // 
            // progressBarControl1
            // 
            this.progressBarControl1.Location = new System.Drawing.Point(12, 208);
            this.progressBarControl1.Name = "progressBarControl1";
            this.progressBarControl1.Size = new System.Drawing.Size(341, 18);
            this.progressBarControl1.TabIndex = 58;
            // 
            // btnStartCopy
            // 
            this.btnStartCopy.AllowFocus = false;
            this.btnStartCopy.Appearance.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnStartCopy.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnStartCopy.Appearance.Options.UseFont = true;
            this.btnStartCopy.Appearance.Options.UseForeColor = true;
            this.btnStartCopy.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnStartCopy.ImageOptions.Image")));
            this.btnStartCopy.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnStartCopy.Location = new System.Drawing.Point(112, 173);
            this.btnStartCopy.LookAndFeel.SkinName = "Lilian";
            this.btnStartCopy.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnStartCopy.Name = "btnStartCopy";
            this.btnStartCopy.Size = new System.Drawing.Size(241, 29);
            this.btnStartCopy.TabIndex = 57;
            this.btnStartCopy.Text = "복사 시작";
            this.btnStartCopy.Click += new System.EventHandler(this.btnStartCopy_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.labelControl11);
            this.panelControl2.Location = new System.Drawing.Point(12, 12);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(341, 42);
            this.panelControl2.TabIndex = 56;
            // 
            // labelControl11
            // 
            this.labelControl11.Appearance.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl11.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelControl11.Appearance.Options.UseFont = true;
            this.labelControl11.Appearance.Options.UseForeColor = true;
            this.labelControl11.Location = new System.Drawing.Point(4, 9);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(334, 24);
            this.labelControl11.TabIndex = 17;
            this.labelControl11.Text = "* USB가 인식될 경우 자동으로 선택됩니다.\r\n* 그렇지 않은 경우 USB를 선택한 후 복사 버튼을 클릭하세요.";
            // 
            // txtProgressView
            // 
            this.txtProgressView.BackColor = System.Drawing.Color.White;
            this.txtProgressView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProgressView.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtProgressView.ForeColor = System.Drawing.Color.Black;
            this.txtProgressView.Location = new System.Drawing.Point(12, 232);
            this.txtProgressView.MaxLength = 2147483647;
            this.txtProgressView.Multiline = true;
            this.txtProgressView.Name = "txtProgressView";
            this.txtProgressView.ReadOnly = true;
            this.txtProgressView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProgressView.Size = new System.Drawing.Size(341, 275);
            this.txtProgressView.TabIndex = 55;
            this.txtProgressView.TabStop = false;
            // 
            // grpDrives
            // 
            this.grpDrives.AppearanceCaption.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpDrives.AppearanceCaption.ForeColor = System.Drawing.Color.Black;
            this.grpDrives.AppearanceCaption.Options.UseFont = true;
            this.grpDrives.AppearanceCaption.Options.UseForeColor = true;
            this.grpDrives.Controls.Add(this.flpDrives);
            this.grpDrives.Location = new System.Drawing.Point(12, 60);
            this.grpDrives.LookAndFeel.SkinName = "Money Twins";
            this.grpDrives.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpDrives.Name = "grpDrives";
            this.grpDrives.Size = new System.Drawing.Size(341, 109);
            this.grpDrives.TabIndex = 2;
            this.grpDrives.Text = "Drive";
            // 
            // flpDrives
            // 
            this.flpDrives.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpDrives.Location = new System.Drawing.Point(2, 22);
            this.flpDrives.Name = "flpDrives";
            this.flpDrives.Size = new System.Drawing.Size(337, 85);
            this.flpDrives.TabIndex = 58;
            // 
            // timer1
            // 
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // frmCopyToUSB
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(358, 519);
            this.Controls.Add(this.panelControl1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Lilian";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximizeBox = false;
            this.Name = "frmCopyToUSB";
            this.Text = "USB로 복사하기";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCopyToUSB_FormClosing);
            this.Load += new System.EventHandler(this.frmCopyToUSB_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpDrives)).EndInit();
            this.grpDrives.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl grpDrives;
        public System.Windows.Forms.TextBox txtProgressView;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.SimpleButton btnStartCopy;
        private System.Windows.Forms.FlowLayoutPanel flpDrives;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private DevExpress.XtraEditors.ProgressBarControl progressBarControl1;
        private DevExpress.XtraEditors.SimpleButton btnReFind;
    }
}