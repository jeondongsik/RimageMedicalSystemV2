namespace RimageMedicalSystemV2
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.grpPatInfo = new DevExpress.XtraEditors.GroupControl();
            this.ucPatients11 = new RimageMedicalSystemV2.ucPatients1();
            this.ucPatients21 = new RimageMedicalSystemV2.ucPatients2();
            this.grpServerList = new DevExpress.XtraEditors.GroupControl();
            this.Server5 = new DevExpress.XtraEditors.LabelControl();
            this.Server4 = new DevExpress.XtraEditors.LabelControl();
            this.Server3 = new DevExpress.XtraEditors.LabelControl();
            this.Server2 = new DevExpress.XtraEditors.LabelControl();
            this.Server1 = new DevExpress.XtraEditors.LabelControl();
            this.btnSearch = new DevExpress.XtraEditors.SimpleButton();
            this.btnBurn = new DevExpress.XtraEditors.SimpleButton();
            this.btnClear = new DevExpress.XtraEditors.SimpleButton();
            this.btnPatientDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnOrderedList = new DevExpress.XtraEditors.SimpleButton();
            this.btnReport = new DevExpress.XtraEditors.SimpleButton();
            this.grpBurningList = new DevExpress.XtraEditors.GroupControl();
            this.gcBurninglist = new DevExpress.XtraGrid.GridControl();
            this.gvBurninglist = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColBurnDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColPatientNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColPatientName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColProgress = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColOrderID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColMedia = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSize = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColServer = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.txtStatusView = new System.Windows.Forms.TextBox();
            this.buttonRetry = new DevExpress.XtraEditors.SimpleButton();
            this.txtMessages = new DevExpress.XtraEditors.MemoEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblDriveStatus = new DevExpress.XtraEditors.LabelControl();
            this.grpServerState = new DevExpress.XtraEditors.GroupControl();
            this.txtBin2Discs = new DevExpress.XtraEditors.TextEdit();
            this.txtBin4Discs = new DevExpress.XtraEditors.TextEdit();
            this.txtBin1Discs = new DevExpress.XtraEditors.TextEdit();
            this.txtBin3Discs = new DevExpress.XtraEditors.TextEdit();
            this.txtRetansferRibbons = new DevExpress.XtraEditors.TextEdit();
            this.txtCMYRibbons = new DevExpress.XtraEditors.TextEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.tmrBinCheker = new System.Windows.Forms.Timer(this.components);
            this.tmrHookChecker = new System.Windows.Forms.Timer(this.components);
            this.tmrDownloadChecker = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker4 = new System.ComponentModel.BackgroundWorker();
            this.btnCancelBurning = new DevExpress.XtraEditors.SimpleButton();
            this.btnUSBCopy = new DevExpress.XtraEditors.SimpleButton();
            this.hyperLinkEdit1 = new DevExpress.XtraEditors.HyperLinkEdit();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tmrConnectServer = new System.Windows.Forms.Timer(this.components);
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.grpPatInfo)).BeginInit();
            this.grpPatInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpServerList)).BeginInit();
            this.grpServerList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBurningList)).BeginInit();
            this.grpBurningList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcBurninglist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBurninglist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessages.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpServerState)).BeginInit();
            this.grpServerState.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBin2Discs.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBin4Discs.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBin1Discs.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBin3Discs.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRetansferRibbons.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCMYRibbons.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hyperLinkEdit1.Properties)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpPatInfo
            // 
            this.grpPatInfo.AppearanceCaption.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grpPatInfo.AppearanceCaption.ForeColor = System.Drawing.Color.Black;
            this.grpPatInfo.AppearanceCaption.Options.UseFont = true;
            this.grpPatInfo.AppearanceCaption.Options.UseForeColor = true;
            this.grpPatInfo.CaptionImageOptions.Image = global::RimageMedicalSystemV2.Properties.Resources.forward_16x16;
            this.grpPatInfo.Controls.Add(this.ucPatients11);
            this.grpPatInfo.Controls.Add(this.ucPatients21);
            this.grpPatInfo.Location = new System.Drawing.Point(12, 57);
            this.grpPatInfo.LookAndFeel.SkinName = "Money Twins";
            this.grpPatInfo.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpPatInfo.Name = "grpPatInfo";
            this.grpPatInfo.Size = new System.Drawing.Size(804, 232);
            this.grpPatInfo.TabIndex = 0;
            this.grpPatInfo.Text = "환자 정보";
            // 
            // ucPatients11
            // 
            this.ucPatients11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucPatients11.Location = new System.Drawing.Point(2, 23);
            this.ucPatients11.LookAndFeel.SkinName = "Sharp Plus";
            this.ucPatients11.LookAndFeel.UseDefaultLookAndFeel = false;
            this.ucPatients11.Name = "ucPatients11";
            this.ucPatients11.OrderInfo = null;
            this.ucPatients11.Size = new System.Drawing.Size(800, 207);
            this.ucPatients11.TabIndex = 1;
            this.ucPatients11.TabStop = false;
            this.ucPatients11.Visible = false;
            // 
            // ucPatients21
            // 
            this.ucPatients21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucPatients21.FrmParent = null;
            this.ucPatients21.Location = new System.Drawing.Point(2, 23);
            this.ucPatients21.LookAndFeel.SkinName = "Sharp Plus";
            this.ucPatients21.LookAndFeel.UseDefaultLookAndFeel = false;
            this.ucPatients21.Name = "ucPatients21";
            this.ucPatients21.PatientInfoList = null;
            this.ucPatients21.Size = new System.Drawing.Size(800, 207);
            this.ucPatients21.TabIndex = 0;
            this.ucPatients21.TabStop = false;
            this.ucPatients21.Visible = false;
            // 
            // grpServerList
            // 
            this.grpServerList.AppearanceCaption.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grpServerList.AppearanceCaption.Options.UseFont = true;
            this.grpServerList.CaptionImageOptions.Image = global::RimageMedicalSystemV2.Properties.Resources.documentmap_16x16;
            this.grpServerList.Controls.Add(this.Server5);
            this.grpServerList.Controls.Add(this.Server4);
            this.grpServerList.Controls.Add(this.Server3);
            this.grpServerList.Controls.Add(this.Server2);
            this.grpServerList.Controls.Add(this.Server1);
            this.grpServerList.Location = new System.Drawing.Point(822, 57);
            this.grpServerList.LookAndFeel.SkinName = "Money Twins";
            this.grpServerList.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpServerList.Name = "grpServerList";
            this.grpServerList.Size = new System.Drawing.Size(174, 232);
            this.grpServerList.TabIndex = 2;
            this.grpServerList.Text = "서버 목록";
            // 
            // Server5
            // 
            this.Server5.Appearance.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Server5.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Server5.Appearance.Image = global::RimageMedicalSystemV2.Properties.Resources.add_16x16;
            this.Server5.Appearance.Options.UseFont = true;
            this.Server5.Appearance.Options.UseForeColor = true;
            this.Server5.Appearance.Options.UseImage = true;
            this.Server5.AppearanceHovered.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Server5.AppearanceHovered.Options.UseBorderColor = true;
            this.Server5.AppearancePressed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Server5.AppearancePressed.Options.UseBorderColor = true;
            this.Server5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.Server5.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.Server5.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.Server5.Location = new System.Drawing.Point(8, 189);
            this.Server5.Name = "Server5";
            this.Server5.Size = new System.Drawing.Size(159, 34);
            this.Server5.TabIndex = 4;
            this.Server5.Click += new System.EventHandler(this.ServerButton_Click);
            // 
            // Server4
            // 
            this.Server4.Appearance.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Server4.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Server4.Appearance.Image = global::RimageMedicalSystemV2.Properties.Resources.add_16x16;
            this.Server4.Appearance.Options.UseFont = true;
            this.Server4.Appearance.Options.UseForeColor = true;
            this.Server4.Appearance.Options.UseImage = true;
            this.Server4.AppearanceHovered.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Server4.AppearanceHovered.Options.UseBorderColor = true;
            this.Server4.AppearancePressed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Server4.AppearancePressed.Options.UseBorderColor = true;
            this.Server4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.Server4.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.Server4.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.Server4.Location = new System.Drawing.Point(8, 149);
            this.Server4.Name = "Server4";
            this.Server4.Size = new System.Drawing.Size(159, 34);
            this.Server4.TabIndex = 3;
            this.Server4.Click += new System.EventHandler(this.ServerButton_Click);
            // 
            // Server3
            // 
            this.Server3.Appearance.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Server3.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Server3.Appearance.Image = global::RimageMedicalSystemV2.Properties.Resources.add_16x16;
            this.Server3.Appearance.Options.UseFont = true;
            this.Server3.Appearance.Options.UseForeColor = true;
            this.Server3.Appearance.Options.UseImage = true;
            this.Server3.AppearanceHovered.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Server3.AppearanceHovered.Options.UseBorderColor = true;
            this.Server3.AppearancePressed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Server3.AppearancePressed.Options.UseBorderColor = true;
            this.Server3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.Server3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.Server3.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.Server3.Location = new System.Drawing.Point(8, 109);
            this.Server3.Name = "Server3";
            this.Server3.Size = new System.Drawing.Size(159, 34);
            this.Server3.TabIndex = 2;
            this.Server3.Click += new System.EventHandler(this.ServerButton_Click);
            // 
            // Server2
            // 
            this.Server2.Appearance.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Server2.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Server2.Appearance.Image = global::RimageMedicalSystemV2.Properties.Resources.add_16x16;
            this.Server2.Appearance.Options.UseFont = true;
            this.Server2.Appearance.Options.UseForeColor = true;
            this.Server2.Appearance.Options.UseImage = true;
            this.Server2.AppearanceHovered.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Server2.AppearanceHovered.Options.UseBorderColor = true;
            this.Server2.AppearancePressed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Server2.AppearancePressed.Options.UseBorderColor = true;
            this.Server2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.Server2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.Server2.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.Server2.Location = new System.Drawing.Point(8, 69);
            this.Server2.Name = "Server2";
            this.Server2.Size = new System.Drawing.Size(159, 34);
            this.Server2.TabIndex = 1;
            this.Server2.Click += new System.EventHandler(this.ServerButton_Click);
            // 
            // Server1
            // 
            this.Server1.Appearance.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Server1.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Server1.Appearance.Image = global::RimageMedicalSystemV2.Properties.Resources.add_16x16;
            this.Server1.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Server1.Appearance.Options.UseFont = true;
            this.Server1.Appearance.Options.UseForeColor = true;
            this.Server1.Appearance.Options.UseImage = true;
            this.Server1.Appearance.Options.UseImageAlign = true;
            this.Server1.AppearanceHovered.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Server1.AppearanceHovered.Options.UseBorderColor = true;
            this.Server1.AppearancePressed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Server1.AppearancePressed.Options.UseBorderColor = true;
            this.Server1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.Server1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.Server1.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.Server1.Location = new System.Drawing.Point(8, 29);
            this.Server1.Name = "Server1";
            this.Server1.Size = new System.Drawing.Size(159, 34);
            this.Server1.TabIndex = 0;
            this.Server1.Click += new System.EventHandler(this.ServerButton_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.AllowFocus = false;
            this.btnSearch.Appearance.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Appearance.Options.UseFont = true;
            this.btnSearch.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.ImageOptions.Image")));
            this.btnSearch.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(3, 3);
            this.btnSearch.LookAndFeel.SkinName = "Lilian";
            this.btnSearch.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 43);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "조회";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnBurn
            // 
            this.btnBurn.AllowFocus = false;
            this.btnBurn.Appearance.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBurn.Appearance.Options.UseFont = true;
            this.btnBurn.ImageOptions.Image = global::RimageMedicalSystemV2.Properties.Resources.dvd_burn1;
            this.btnBurn.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnBurn.Location = new System.Drawing.Point(109, 3);
            this.btnBurn.LookAndFeel.SkinName = "Lilian";
            this.btnBurn.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnBurn.Name = "btnBurn";
            this.btnBurn.Size = new System.Drawing.Size(100, 43);
            this.btnBurn.TabIndex = 6;
            this.btnBurn.Text = "굽기";
            this.btnBurn.Click += new System.EventHandler(this.btnBurn_Click);
            // 
            // btnClear
            // 
            this.btnClear.AllowFocus = false;
            this.btnClear.Appearance.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClear.Appearance.Options.UseFont = true;
            this.btnClear.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.ImageOptions.Image")));
            this.btnClear.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnClear.Location = new System.Drawing.Point(336, 3);
            this.btnClear.LookAndFeel.SkinName = "Lilian";
            this.btnClear.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(115, 43);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "목록정리";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnPatientDelete
            // 
            this.btnPatientDelete.AllowFocus = false;
            this.btnPatientDelete.Appearance.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPatientDelete.Appearance.Options.UseFont = true;
            this.btnPatientDelete.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnPatientDelete.ImageOptions.Image")));
            this.btnPatientDelete.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnPatientDelete.Location = new System.Drawing.Point(457, 3);
            this.btnPatientDelete.LookAndFeel.SkinName = "Lilian";
            this.btnPatientDelete.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnPatientDelete.Name = "btnPatientDelete";
            this.btnPatientDelete.Size = new System.Drawing.Size(159, 43);
            this.btnPatientDelete.TabIndex = 10;
            this.btnPatientDelete.Text = "대기환자 삭제";
            this.btnPatientDelete.Visible = false;
            this.btnPatientDelete.Click += new System.EventHandler(this.btnPatientDelete_Click);
            // 
            // btnOrderedList
            // 
            this.btnOrderedList.AllowFocus = false;
            this.btnOrderedList.Appearance.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOrderedList.Appearance.Options.UseFont = true;
            this.btnOrderedList.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnOrderedList.ImageOptions.Image")));
            this.btnOrderedList.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnOrderedList.Location = new System.Drawing.Point(131, 3);
            this.btnOrderedList.LookAndFeel.SkinName = "Lilian";
            this.btnOrderedList.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnOrderedList.Name = "btnOrderedList";
            this.btnOrderedList.Size = new System.Drawing.Size(100, 43);
            this.btnOrderedList.TabIndex = 11;
            this.btnOrderedList.Text = "재굽기";
            this.btnOrderedList.Click += new System.EventHandler(this.btnOrderedList_Click);
            // 
            // btnReport
            // 
            this.btnReport.AllowFocus = false;
            this.btnReport.Appearance.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnReport.Appearance.Options.UseFont = true;
            this.btnReport.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnReport.ImageOptions.Image")));
            this.btnReport.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnReport.Location = new System.Drawing.Point(237, 3);
            this.btnReport.LookAndFeel.SkinName = "Lilian";
            this.btnReport.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(100, 43);
            this.btnReport.TabIndex = 8;
            this.btnReport.Text = "보고서";
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // grpBurningList
            // 
            this.grpBurningList.AppearanceCaption.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grpBurningList.AppearanceCaption.ForeColor = System.Drawing.Color.Black;
            this.grpBurningList.AppearanceCaption.Options.UseFont = true;
            this.grpBurningList.AppearanceCaption.Options.UseForeColor = true;
            this.grpBurningList.CaptionImageOptions.Image = global::RimageMedicalSystemV2.Properties.Resources.forward_16x16;
            this.grpBurningList.Controls.Add(this.gcBurninglist);
            this.grpBurningList.Location = new System.Drawing.Point(12, 295);
            this.grpBurningList.LookAndFeel.SkinName = "Money Twins";
            this.grpBurningList.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpBurningList.Name = "grpBurningList";
            this.grpBurningList.Size = new System.Drawing.Size(984, 243);
            this.grpBurningList.TabIndex = 1;
            this.grpBurningList.Text = "진행 상태 목록";
            // 
            // gcBurninglist
            // 
            this.gcBurninglist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcBurninglist.Location = new System.Drawing.Point(2, 23);
            this.gcBurninglist.LookAndFeel.SkinName = "VS2010";
            this.gcBurninglist.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcBurninglist.MainView = this.gvBurninglist;
            this.gcBurninglist.Name = "gcBurninglist";
            this.gcBurninglist.Size = new System.Drawing.Size(980, 218);
            this.gcBurninglist.TabIndex = 2;
            this.gcBurninglist.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvBurninglist});
            // 
            // gvBurninglist
            // 
            this.gvBurninglist.Appearance.HeaderPanel.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gvBurninglist.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvBurninglist.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvBurninglist.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvBurninglist.Appearance.Row.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gvBurninglist.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvBurninglist.Appearance.Row.Options.UseFont = true;
            this.gvBurninglist.Appearance.Row.Options.UseForeColor = true;
            this.gvBurninglist.Appearance.Row.Options.UseTextOptions = true;
            this.gvBurninglist.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvBurninglist.Appearance.ViewCaption.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gvBurninglist.Appearance.ViewCaption.Options.UseFont = true;
            this.gvBurninglist.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColNo,
            this.gridColBurnDate,
            this.gridColPatientNo,
            this.gridColPatientName,
            this.gridColState,
            this.gridColProgress,
            this.gridColStatus,
            this.gridColOrderID,
            this.gridQuantity,
            this.gridColMedia,
            this.gridColSize,
            this.gridColServer});
            this.gvBurninglist.GridControl = this.gcBurninglist;
            this.gvBurninglist.Name = "gvBurninglist";
            this.gvBurninglist.OptionsBehavior.Editable = false;
            this.gvBurninglist.OptionsBehavior.ReadOnly = true;
            this.gvBurninglist.OptionsCustomization.AllowColumnMoving = false;
            this.gvBurninglist.OptionsCustomization.AllowFilter = false;
            this.gvBurninglist.OptionsCustomization.AllowSort = false;
            this.gvBurninglist.OptionsDetail.EnableMasterViewMode = false;
            this.gvBurninglist.OptionsView.ColumnAutoWidth = false;
            this.gvBurninglist.OptionsView.ShowDetailButtons = false;
            this.gvBurninglist.OptionsView.ShowGroupPanel = false;
            this.gvBurninglist.OptionsView.ShowIndicator = false;
            this.gvBurninglist.RowHeight = 25;
            this.gvBurninglist.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvBurninglist_RowCellStyle);
            // 
            // gridColNo
            // 
            this.gridColNo.Caption = "번호";
            this.gridColNo.FieldName = "No";
            this.gridColNo.Name = "gridColNo";
            this.gridColNo.Visible = true;
            this.gridColNo.VisibleIndex = 0;
            this.gridColNo.Width = 50;
            // 
            // gridColBurnDate
            // 
            this.gridColBurnDate.Caption = "날짜";
            this.gridColBurnDate.FieldName = "patDate";
            this.gridColBurnDate.Name = "gridColBurnDate";
            this.gridColBurnDate.Visible = true;
            this.gridColBurnDate.VisibleIndex = 1;
            this.gridColBurnDate.Width = 90;
            // 
            // gridColPatientNo
            // 
            this.gridColPatientNo.Caption = "환자번호";
            this.gridColPatientNo.FieldName = "patNo";
            this.gridColPatientNo.Name = "gridColPatientNo";
            this.gridColPatientNo.Visible = true;
            this.gridColPatientNo.VisibleIndex = 2;
            this.gridColPatientNo.Width = 100;
            // 
            // gridColPatientName
            // 
            this.gridColPatientName.Caption = "환자명";
            this.gridColPatientName.FieldName = "patName";
            this.gridColPatientName.Name = "gridColPatientName";
            this.gridColPatientName.Visible = true;
            this.gridColPatientName.VisibleIndex = 3;
            this.gridColPatientName.Width = 130;
            // 
            // gridColState
            // 
            this.gridColState.Caption = "진행상태";
            this.gridColState.FieldName = "Progress";
            this.gridColState.Name = "gridColState";
            this.gridColState.Visible = true;
            this.gridColState.VisibleIndex = 4;
            this.gridColState.Width = 230;
            // 
            // gridColProgress
            // 
            this.gridColProgress.Caption = "진행율(%)";
            this.gridColProgress.FieldName = "ProcessingRate";
            this.gridColProgress.Name = "gridColProgress";
            this.gridColProgress.Visible = true;
            this.gridColProgress.VisibleIndex = 5;
            this.gridColProgress.Width = 80;
            // 
            // gridColStatus
            // 
            this.gridColStatus.Caption = "상태";
            this.gridColStatus.FieldName = "BurnState";
            this.gridColStatus.Name = "gridColStatus";
            this.gridColStatus.Width = 65;
            // 
            // gridColOrderID
            // 
            this.gridColOrderID.Caption = "출력번호";
            this.gridColOrderID.FieldName = "OrderId";
            this.gridColOrderID.Name = "gridColOrderID";
            this.gridColOrderID.Width = 120;
            // 
            // gridQuantity
            // 
            this.gridQuantity.Caption = "수량";
            this.gridQuantity.FieldName = "copies";
            this.gridQuantity.Name = "gridQuantity";
            this.gridQuantity.Visible = true;
            this.gridQuantity.VisibleIndex = 6;
            this.gridQuantity.Width = 55;
            // 
            // gridColMedia
            // 
            this.gridColMedia.Caption = "미디어";
            this.gridColMedia.FieldName = "mediType";
            this.gridColMedia.Name = "gridColMedia";
            this.gridColMedia.Visible = true;
            this.gridColMedia.VisibleIndex = 7;
            // 
            // gridColSize
            // 
            this.gridColSize.Caption = "크기";
            this.gridColSize.FieldName = "mediSize";
            this.gridColSize.Name = "gridColSize";
            this.gridColSize.Visible = true;
            this.gridColSize.VisibleIndex = 8;
            this.gridColSize.Width = 85;
            // 
            // gridColServer
            // 
            this.gridColServer.Caption = "서버";
            this.gridColServer.FieldName = "ServerNo";
            this.gridColServer.Name = "gridColServer";
            this.gridColServer.Visible = true;
            this.gridColServer.VisibleIndex = 9;
            this.gridColServer.Width = 50;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.txtStatusView);
            this.panelControl2.Controls.Add(this.buttonRetry);
            this.panelControl2.Controls.Add(this.txtMessages);
            this.panelControl2.Location = new System.Drawing.Point(12, 544);
            this.panelControl2.LookAndFeel.SkinName = "Money Twins";
            this.panelControl2.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(475, 186);
            this.panelControl2.TabIndex = 3;
            // 
            // txtStatusView
            // 
            this.txtStatusView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(241)))));
            this.txtStatusView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStatusView.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtStatusView.ForeColor = System.Drawing.Color.Black;
            this.txtStatusView.Location = new System.Drawing.Point(5, 51);
            this.txtStatusView.MaxLength = 2147483647;
            this.txtStatusView.Multiline = true;
            this.txtStatusView.Name = "txtStatusView";
            this.txtStatusView.ReadOnly = true;
            this.txtStatusView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatusView.Size = new System.Drawing.Size(465, 130);
            this.txtStatusView.TabIndex = 54;
            this.txtStatusView.TabStop = false;
            // 
            // buttonRetry
            // 
            this.buttonRetry.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("buttonRetry.ImageOptions.Image")));
            this.buttonRetry.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.buttonRetry.Location = new System.Drawing.Point(5, 6);
            this.buttonRetry.Name = "buttonRetry";
            this.buttonRetry.Size = new System.Drawing.Size(20, 39);
            this.buttonRetry.TabIndex = 2;
            this.buttonRetry.Click += new System.EventHandler(this.buttonRetry_Click);
            // 
            // txtMessages
            // 
            this.txtMessages.Location = new System.Drawing.Point(31, 6);
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.Properties.Appearance.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtMessages.Properties.Appearance.Options.UseFont = true;
            this.txtMessages.Properties.ReadOnly = true;
            this.txtMessages.Size = new System.Drawing.Size(439, 39);
            this.txtMessages.TabIndex = 0;
            this.txtMessages.TabStop = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.lblDriveStatus);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 736);
            this.panelControl1.LookAndFeel.SkinName = "Money Twins";
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1008, 25);
            this.panelControl1.TabIndex = 13;
            // 
            // lblDriveStatus
            // 
            this.lblDriveStatus.Appearance.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDriveStatus.Appearance.ForeColor = System.Drawing.Color.MediumVioletRed;
            this.lblDriveStatus.Appearance.Options.UseFont = true;
            this.lblDriveStatus.Appearance.Options.UseForeColor = true;
            this.lblDriveStatus.Location = new System.Drawing.Point(15, 7);
            this.lblDriveStatus.Name = "lblDriveStatus";
            this.lblDriveStatus.Size = new System.Drawing.Size(138, 12);
            this.lblDriveStatus.TabIndex = 17;
            this.lblDriveStatus.Text = "Current Save Drive : ";
            // 
            // grpServerState
            // 
            this.grpServerState.AppearanceCaption.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grpServerState.AppearanceCaption.ForeColor = System.Drawing.Color.Black;
            this.grpServerState.AppearanceCaption.Options.UseFont = true;
            this.grpServerState.AppearanceCaption.Options.UseForeColor = true;
            this.grpServerState.CaptionImageOptions.Image = global::RimageMedicalSystemV2.Properties.Resources.documentmap_16x16;
            this.grpServerState.Controls.Add(this.txtBin2Discs);
            this.grpServerState.Controls.Add(this.txtBin4Discs);
            this.grpServerState.Controls.Add(this.txtBin1Discs);
            this.grpServerState.Controls.Add(this.txtBin3Discs);
            this.grpServerState.Controls.Add(this.txtRetansferRibbons);
            this.grpServerState.Controls.Add(this.txtCMYRibbons);
            this.grpServerState.Controls.Add(this.label7);
            this.grpServerState.Controls.Add(this.label5);
            this.grpServerState.Controls.Add(this.label4);
            this.grpServerState.Controls.Add(this.label3);
            this.grpServerState.Controls.Add(this.label2);
            this.grpServerState.Controls.Add(this.label1);
            this.grpServerState.Location = new System.Drawing.Point(491, 544);
            this.grpServerState.LookAndFeel.SkinName = "Money Twins";
            this.grpServerState.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpServerState.Name = "grpServerState";
            this.grpServerState.Size = new System.Drawing.Size(505, 135);
            this.grpServerState.TabIndex = 4;
            this.grpServerState.Text = "서버 상태 정보";
            // 
            // txtBin2Discs
            // 
            this.txtBin2Discs.EditValue = "0";
            this.txtBin2Discs.Location = new System.Drawing.Point(410, 35);
            this.txtBin2Discs.Name = "txtBin2Discs";
            this.txtBin2Discs.Properties.Appearance.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtBin2Discs.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtBin2Discs.Properties.Appearance.Options.UseFont = true;
            this.txtBin2Discs.Properties.Appearance.Options.UseForeColor = true;
            this.txtBin2Discs.Properties.Appearance.Options.UseTextOptions = true;
            this.txtBin2Discs.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtBin2Discs.Properties.ReadOnly = true;
            this.txtBin2Discs.Size = new System.Drawing.Size(84, 18);
            this.txtBin2Discs.TabIndex = 135;
            this.txtBin2Discs.TabStop = false;
            // 
            // txtBin4Discs
            // 
            this.txtBin4Discs.EditValue = "0";
            this.txtBin4Discs.Location = new System.Drawing.Point(410, 68);
            this.txtBin4Discs.Name = "txtBin4Discs";
            this.txtBin4Discs.Properties.Appearance.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtBin4Discs.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtBin4Discs.Properties.Appearance.Options.UseFont = true;
            this.txtBin4Discs.Properties.Appearance.Options.UseForeColor = true;
            this.txtBin4Discs.Properties.Appearance.Options.UseTextOptions = true;
            this.txtBin4Discs.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtBin4Discs.Properties.ReadOnly = true;
            this.txtBin4Discs.Size = new System.Drawing.Size(84, 18);
            this.txtBin4Discs.TabIndex = 134;
            this.txtBin4Discs.TabStop = false;
            // 
            // txtBin1Discs
            // 
            this.txtBin1Discs.EditValue = "0";
            this.txtBin1Discs.Location = new System.Drawing.Point(169, 34);
            this.txtBin1Discs.Name = "txtBin1Discs";
            this.txtBin1Discs.Properties.Appearance.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtBin1Discs.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtBin1Discs.Properties.Appearance.Options.UseFont = true;
            this.txtBin1Discs.Properties.Appearance.Options.UseForeColor = true;
            this.txtBin1Discs.Properties.Appearance.Options.UseTextOptions = true;
            this.txtBin1Discs.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtBin1Discs.Properties.ReadOnly = true;
            this.txtBin1Discs.Size = new System.Drawing.Size(84, 18);
            this.txtBin1Discs.TabIndex = 133;
            this.txtBin1Discs.TabStop = false;
            // 
            // txtBin3Discs
            // 
            this.txtBin3Discs.EditValue = "0";
            this.txtBin3Discs.Location = new System.Drawing.Point(169, 68);
            this.txtBin3Discs.Name = "txtBin3Discs";
            this.txtBin3Discs.Properties.Appearance.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtBin3Discs.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtBin3Discs.Properties.Appearance.Options.UseFont = true;
            this.txtBin3Discs.Properties.Appearance.Options.UseForeColor = true;
            this.txtBin3Discs.Properties.Appearance.Options.UseTextOptions = true;
            this.txtBin3Discs.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtBin3Discs.Properties.ReadOnly = true;
            this.txtBin3Discs.Size = new System.Drawing.Size(84, 18);
            this.txtBin3Discs.TabIndex = 132;
            this.txtBin3Discs.TabStop = false;
            // 
            // txtRetansferRibbons
            // 
            this.txtRetansferRibbons.EditValue = "0";
            this.txtRetansferRibbons.Location = new System.Drawing.Point(410, 102);
            this.txtRetansferRibbons.Name = "txtRetansferRibbons";
            this.txtRetansferRibbons.Properties.Appearance.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtRetansferRibbons.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtRetansferRibbons.Properties.Appearance.Options.UseFont = true;
            this.txtRetansferRibbons.Properties.Appearance.Options.UseForeColor = true;
            this.txtRetansferRibbons.Properties.Appearance.Options.UseTextOptions = true;
            this.txtRetansferRibbons.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtRetansferRibbons.Properties.ReadOnly = true;
            this.txtRetansferRibbons.Size = new System.Drawing.Size(84, 18);
            this.txtRetansferRibbons.TabIndex = 131;
            this.txtRetansferRibbons.TabStop = false;
            // 
            // txtCMYRibbons
            // 
            this.txtCMYRibbons.EditValue = "0";
            this.txtCMYRibbons.Location = new System.Drawing.Point(169, 104);
            this.txtCMYRibbons.Name = "txtCMYRibbons";
            this.txtCMYRibbons.Properties.Appearance.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtCMYRibbons.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtCMYRibbons.Properties.Appearance.Options.UseFont = true;
            this.txtCMYRibbons.Properties.Appearance.Options.UseForeColor = true;
            this.txtCMYRibbons.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCMYRibbons.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtCMYRibbons.Properties.ReadOnly = true;
            this.txtCMYRibbons.Size = new System.Drawing.Size(84, 18);
            this.txtCMYRibbons.TabIndex = 130;
            this.txtCMYRibbons.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(268, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(134, 12);
            this.label7.TabIndex = 129;
            this.label7.Text = "· Bin4 Discs Remaining";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(261, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(142, 12);
            this.label5.TabIndex = 128;
            this.label5.Text = "· ITS Ribbons Remaining";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(9, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 12);
            this.label4.TabIndex = 127;
            this.label4.Text = "· CMY Ribbons Remaining";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(9, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 12);
            this.label3.TabIndex = 126;
            this.label3.Text = "· Bin3 Discs Remaining";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(268, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 12);
            this.label2.TabIndex = 125;
            this.label2.Text = "· Bin2 Discs Remaining";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(9, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 12);
            this.label1.TabIndex = 124;
            this.label1.Text = "· Bin1 Discs Remaining";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(493, 693);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(265, 31);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // labelControl11
            // 
            this.labelControl11.Appearance.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl11.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.labelControl11.Appearance.Options.UseFont = true;
            this.labelControl11.Appearance.Options.UseForeColor = true;
            this.labelControl11.Location = new System.Drawing.Point(788, 690);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(205, 13);
            this.labelControl11.TabIndex = 16;
            this.labelControl11.Text = "(주)나시스 기술지원 ☎ 02-466-0745";
            // 
            // tmrBinCheker
            // 
            this.tmrBinCheker.Interval = 180000;
            this.tmrBinCheker.Tick += new System.EventHandler(this.tmrBinCheker_Tick);
            // 
            // tmrHookChecker
            // 
            this.tmrHookChecker.Interval = 300;
            this.tmrHookChecker.Tick += new System.EventHandler(this.tmrHookChecker_Tick);
            // 
            // tmrDownloadChecker
            // 
            this.tmrDownloadChecker.Interval = 5000;
            this.tmrDownloadChecker.Tick += new System.EventHandler(this.tmrDownloadChecker_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.WorkerSupportsCancellation = true;
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // backgroundWorker3
            // 
            this.backgroundWorker3.WorkerReportsProgress = true;
            this.backgroundWorker3.WorkerSupportsCancellation = true;
            this.backgroundWorker3.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker3_DoWork);
            this.backgroundWorker3.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker3_ProgressChanged);
            this.backgroundWorker3.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker3_RunWorkerCompleted);
            // 
            // backgroundWorker4
            // 
            this.backgroundWorker4.WorkerReportsProgress = true;
            this.backgroundWorker4.WorkerSupportsCancellation = true;
            this.backgroundWorker4.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker4_DoWork);
            this.backgroundWorker4.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker4_RunWorkerCompleted);
            // 
            // btnCancelBurning
            // 
            this.btnCancelBurning.AllowFocus = false;
            this.btnCancelBurning.Appearance.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancelBurning.Appearance.Options.UseFont = true;
            this.btnCancelBurning.ImageOptions.Image = global::RimageMedicalSystemV2.Properties.Resources.cancel_32x32;
            this.btnCancelBurning.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnCancelBurning.Location = new System.Drawing.Point(215, 3);
            this.btnCancelBurning.LookAndFeel.SkinName = "Lilian";
            this.btnCancelBurning.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCancelBurning.Name = "btnCancelBurning";
            this.btnCancelBurning.Size = new System.Drawing.Size(115, 43);
            this.btnCancelBurning.TabIndex = 9;
            this.btnCancelBurning.Text = "굽기취소";
            this.btnCancelBurning.Click += new System.EventHandler(this.btnCancelBurning_Click);
            // 
            // btnUSBCopy
            // 
            this.btnUSBCopy.AllowFocus = false;
            this.btnUSBCopy.Appearance.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUSBCopy.Appearance.Options.UseFont = true;
            this.btnUSBCopy.ImageOptions.Image = global::RimageMedicalSystemV2.Properties.Resources.icons8_usb_2_321;
            this.btnUSBCopy.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnUSBCopy.Location = new System.Drawing.Point(25, 3);
            this.btnUSBCopy.LookAndFeel.SkinName = "Lilian";
            this.btnUSBCopy.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnUSBCopy.Name = "btnUSBCopy";
            this.btnUSBCopy.Size = new System.Drawing.Size(100, 43);
            this.btnUSBCopy.TabIndex = 18;
            this.btnUSBCopy.Text = "USB";
            this.btnUSBCopy.Visible = false;
            this.btnUSBCopy.Click += new System.EventHandler(this.btnUSBCopy_Click);
            // 
            // hyperLinkEdit1
            // 
            this.hyperLinkEdit1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.hyperLinkEdit1.EditValue = "원격지원 http://www.nasis.co.kr";
            this.hyperLinkEdit1.Location = new System.Drawing.Point(796, 710);
            this.hyperLinkEdit1.Name = "hyperLinkEdit1";
            this.hyperLinkEdit1.Properties.AllowFocused = false;
            this.hyperLinkEdit1.Properties.Appearance.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.hyperLinkEdit1.Properties.Appearance.Options.UseFont = true;
            this.hyperLinkEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.hyperLinkEdit1.Size = new System.Drawing.Size(198, 18);
            this.hyperLinkEdit1.TabIndex = 19;
            this.hyperLinkEdit1.TabStop = false;
            this.hyperLinkEdit1.OpenLink += new DevExpress.XtraEditors.Controls.OpenLinkEventHandler(this.hyperLinkEdit1_OpenLink);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnReport);
            this.flowLayoutPanel1.Controls.Add(this.btnOrderedList);
            this.flowLayoutPanel1.Controls.Add(this.btnUSBCopy);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(659, 5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(340, 50);
            this.flowLayoutPanel1.TabIndex = 20;
            // 
            // tmrConnectServer
            // 
            this.tmrConnectServer.Interval = 300;
            this.tmrConnectServer.Tick += new System.EventHandler(this.tmrConnectServer_Tick);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnSearch);
            this.flowLayoutPanel2.Controls.Add(this.btnBurn);
            this.flowLayoutPanel2.Controls.Add(this.btnCancelBurning);
            this.flowLayoutPanel2.Controls.Add(this.btnClear);
            this.flowLayoutPanel2.Controls.Add(this.btnPatientDelete);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(9, 5);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(642, 50);
            this.flowLayoutPanel2.TabIndex = 21;
            // 
            // MainForm
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1008, 761);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.hyperLinkEdit1);
            this.Controls.Add(this.labelControl11);
            this.Controls.Add(this.grpServerState);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.grpBurningList);
            this.Controls.Add(this.grpServerList);
            this.Controls.Add(this.grpPatInfo);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "VS2010";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rimage Medical Disc System";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grpPatInfo)).EndInit();
            this.grpPatInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpServerList)).EndInit();
            this.grpServerList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpBurningList)).EndInit();
            this.grpBurningList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcBurninglist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBurninglist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessages.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpServerState)).EndInit();
            this.grpServerState.ResumeLayout(false);
            this.grpServerState.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBin2Discs.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBin4Discs.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBin1Discs.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBin3Discs.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRetansferRibbons.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCMYRibbons.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hyperLinkEdit1.Properties)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl grpPatInfo;
        private DevExpress.XtraEditors.GroupControl grpServerList;
        private DevExpress.XtraEditors.SimpleButton btnSearch;
        private DevExpress.XtraEditors.SimpleButton btnBurn;
        private DevExpress.XtraEditors.SimpleButton btnClear;
        private DevExpress.XtraEditors.SimpleButton btnPatientDelete;
        private DevExpress.XtraEditors.SimpleButton btnOrderedList;
        private DevExpress.XtraEditors.SimpleButton btnReport;
        private DevExpress.XtraEditors.GroupControl grpBurningList;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl Server5;
        private DevExpress.XtraEditors.LabelControl Server4;
        private DevExpress.XtraEditors.LabelControl Server3;
        private DevExpress.XtraEditors.LabelControl Server2;
        private DevExpress.XtraEditors.LabelControl Server1;
        private DevExpress.XtraGrid.GridControl gcBurninglist;
        private DevExpress.XtraGrid.Views.Grid.GridView gvBurninglist;
        private DevExpress.XtraGrid.Columns.GridColumn gridColNo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColBurnDate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColPatientNo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColPatientName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColState;
        private DevExpress.XtraGrid.Columns.GridColumn gridColProgress;
        private DevExpress.XtraGrid.Columns.GridColumn gridColStatus;
        private DevExpress.XtraGrid.Columns.GridColumn gridColOrderID;
        private DevExpress.XtraGrid.Columns.GridColumn gridQuantity;
        private DevExpress.XtraGrid.Columns.GridColumn gridColMedia;
        private DevExpress.XtraGrid.Columns.GridColumn gridColSize;
        private DevExpress.XtraGrid.Columns.GridColumn gridColServer;
        private DevExpress.XtraEditors.SimpleButton buttonRetry;
        private DevExpress.XtraEditors.GroupControl grpServerState;
        private DevExpress.XtraEditors.TextEdit txtBin2Discs;
        private DevExpress.XtraEditors.TextEdit txtBin4Discs;
        private DevExpress.XtraEditors.TextEdit txtBin1Discs;
        private DevExpress.XtraEditors.TextEdit txtBin3Discs;
        private DevExpress.XtraEditors.TextEdit txtRetansferRibbons;
        private DevExpress.XtraEditors.TextEdit txtCMYRibbons;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.LabelControl lblDriveStatus;
        private ucPatients2 ucPatients21;
        private ucPatients1 ucPatients11;
        public System.Windows.Forms.TextBox txtStatusView;
        private System.Windows.Forms.Timer tmrBinCheker;
        private System.Windows.Forms.Timer tmrHookChecker;
        private System.Windows.Forms.Timer tmrDownloadChecker;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.ImageList imageList1;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.ComponentModel.BackgroundWorker backgroundWorker4;
        public DevExpress.XtraEditors.MemoEdit txtMessages;
        private DevExpress.XtraEditors.SimpleButton btnCancelBurning;
        private DevExpress.XtraEditors.SimpleButton btnUSBCopy;
        private DevExpress.XtraEditors.HyperLinkEdit hyperLinkEdit1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Timer tmrConnectServer;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    }
}

