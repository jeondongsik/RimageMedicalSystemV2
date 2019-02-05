﻿namespace RimageMedicalSystemV2
{
    partial class ucPatients1
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlPatInfoArea = new DevExpress.XtraEditors.PanelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.cbMedia = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.cbCopies = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtDataLength = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtPatientName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtPatientID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.pnlImgArea = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.pnlPatInfoArea)).BeginInit();
            this.pnlPatInfoArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbMedia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbCopies.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataLength.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatientName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatientID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlImgArea)).BeginInit();
            this.pnlImgArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPatInfoArea
            // 
            this.pnlPatInfoArea.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlPatInfoArea.Controls.Add(this.labelControl5);
            this.pnlPatInfoArea.Controls.Add(this.cbMedia);
            this.pnlPatInfoArea.Controls.Add(this.labelControl4);
            this.pnlPatInfoArea.Controls.Add(this.cbCopies);
            this.pnlPatInfoArea.Controls.Add(this.txtDataLength);
            this.pnlPatInfoArea.Controls.Add(this.labelControl3);
            this.pnlPatInfoArea.Controls.Add(this.txtPatientName);
            this.pnlPatInfoArea.Controls.Add(this.labelControl2);
            this.pnlPatInfoArea.Controls.Add(this.txtPatientID);
            this.pnlPatInfoArea.Controls.Add(this.labelControl1);
            this.pnlPatInfoArea.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPatInfoArea.Location = new System.Drawing.Point(0, 0);
            this.pnlPatInfoArea.Name = "pnlPatInfoArea";
            this.pnlPatInfoArea.Size = new System.Drawing.Size(817, 31);
            this.pnlPatInfoArea.TabIndex = 128;
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(560, 6);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(43, 15);
            this.labelControl5.TabIndex = 9;
            this.labelControl5.Text = "· 미디어";
            // 
            // cbMedia
            // 
            this.cbMedia.EditValue = "CDR";
            this.cbMedia.Location = new System.Drawing.Point(609, 3);
            this.cbMedia.Name = "cbMedia";
            this.cbMedia.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbMedia.Properties.Appearance.Options.UseFont = true;
            this.cbMedia.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbMedia.Properties.Items.AddRange(new object[] {
            "CDR",
            "DVDR",
            "DVDR-DL"});
            this.cbMedia.Size = new System.Drawing.Size(73, 22);
            this.cbMedia.TabIndex = 8;
            this.cbMedia.TabStop = false;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(438, 6);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(31, 15);
            this.labelControl4.TabIndex = 7;
            this.labelControl4.Text = "· 수량";
            // 
            // cbCopies
            // 
            this.cbCopies.EditValue = "1";
            this.cbCopies.Location = new System.Drawing.Point(475, 3);
            this.cbCopies.Name = "cbCopies";
            this.cbCopies.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbCopies.Properties.Appearance.Options.UseFont = true;
            this.cbCopies.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbCopies.Properties.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7"});
            this.cbCopies.Size = new System.Drawing.Size(72, 22);
            this.cbCopies.TabIndex = 6;
            this.cbCopies.TabStop = false;
            // 
            // txtDataLength
            // 
            this.txtDataLength.EditValue = "";
            this.txtDataLength.Location = new System.Drawing.Point(725, 3);
            this.txtDataLength.Name = "txtDataLength";
            this.txtDataLength.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtDataLength.Properties.Appearance.Options.UseFont = true;
            this.txtDataLength.Properties.Appearance.Options.UseTextOptions = true;
            this.txtDataLength.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtDataLength.Properties.ReadOnly = true;
            this.txtDataLength.Size = new System.Drawing.Size(84, 22);
            this.txtDataLength.TabIndex = 5;
            this.txtDataLength.TabStop = false;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(688, 6);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(31, 15);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "· 크기";
            // 
            // txtPatientName
            // 
            this.txtPatientName.Location = new System.Drawing.Point(254, 3);
            this.txtPatientName.Name = "txtPatientName";
            this.txtPatientName.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPatientName.Properties.Appearance.Options.UseFont = true;
            this.txtPatientName.Size = new System.Drawing.Size(175, 22);
            this.txtPatientName.TabIndex = 3;
            this.txtPatientName.TabStop = false;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(205, 6);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(43, 15);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "· 환자명";
            // 
            // txtPatientID
            // 
            this.txtPatientID.Location = new System.Drawing.Point(87, 3);
            this.txtPatientID.Name = "txtPatientID";
            this.txtPatientID.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPatientID.Properties.Appearance.Options.UseFont = true;
            this.txtPatientID.Size = new System.Drawing.Size(110, 22);
            this.txtPatientID.TabIndex = 1;
            this.txtPatientID.TabStop = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(10, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(71, 15);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "· 환자 아이디";
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(81)))), ((int)(((byte)(97)))));
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.ForeColor = System.Drawing.Color.White;
            this.treeView1.FullRowSelect = true;
            this.treeView1.Location = new System.Drawing.Point(7, 5);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(803, 165);
            this.treeView1.TabIndex = 129;
            // 
            // pnlImgArea
            // 
            this.pnlImgArea.Controls.Add(this.treeView1);
            this.pnlImgArea.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlImgArea.Location = new System.Drawing.Point(0, 31);
            this.pnlImgArea.Name = "pnlImgArea";
            this.pnlImgArea.Padding = new System.Windows.Forms.Padding(5, 3, 5, 5);
            this.pnlImgArea.Size = new System.Drawing.Size(817, 177);
            this.pnlImgArea.TabIndex = 130;
            // 
            // ucPatients1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlImgArea);
            this.Controls.Add(this.pnlPatInfoArea);
            this.LookAndFeel.SkinName = "Sharp Plus";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "ucPatients1";
            this.Size = new System.Drawing.Size(817, 208);
            ((System.ComponentModel.ISupportInitialize)(this.pnlPatInfoArea)).EndInit();
            this.pnlPatInfoArea.ResumeLayout(false);
            this.pnlPatInfoArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbMedia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbCopies.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataLength.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatientName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatientID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlImgArea)).EndInit();
            this.pnlImgArea.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlPatInfoArea;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.TreeView treeView1;
        public DevExpress.XtraEditors.ComboBoxEdit cbCopies;
        public DevExpress.XtraEditors.TextEdit txtPatientName;
        public DevExpress.XtraEditors.TextEdit txtPatientID;
        public DevExpress.XtraEditors.ComboBoxEdit cbMedia;
        public DevExpress.XtraEditors.TextEdit txtDataLength;
        private DevExpress.XtraEditors.PanelControl pnlImgArea;
    }
}
