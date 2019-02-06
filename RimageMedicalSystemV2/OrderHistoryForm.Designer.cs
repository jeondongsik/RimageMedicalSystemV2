namespace RimageMedicalSystemV2
{
    partial class OrderHistoryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderHistoryForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new DevExpress.XtraEditors.SimpleButton();
            this.grpBurningList = new DevExpress.XtraEditors.GroupControl();
            this.gcPatientlist = new DevExpress.XtraGrid.GridControl();
            this.gvPatientlist = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColBurnDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColPatientNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColPatientName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColGender = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColAge = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSize = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColMedia = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repoMediatype = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repoQuantity = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColResult = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColRetry = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColDelete = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repoPicDel = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBurningList)).BeginInit();
            this.grpBurningList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcPatientlist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPatientlist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repoMediatype)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repoQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repoPicDel)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.btnSearch);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(984, 70);
            this.panelControl1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label3.Location = new System.Drawing.Point(12, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(418, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "* 다시 버닝을 할 때 수량/미디어를 수정하여 다시굽기할 수 있습니다.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label2.Location = new System.Drawing.Point(12, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(369, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "* 성공/실패와 상관없이 명령을 보낸 내역이 모두 보여집니다.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(282, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "* CD/DVD 버닝 명령을 보낸 모든 내역입니다. ";
            // 
            // btnSearch
            // 
            this.btnSearch.Appearance.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Appearance.Options.UseFont = true;
            this.btnSearch.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.ImageOptions.Image")));
            this.btnSearch.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(833, 14);
            this.btnSearch.LookAndFeel.SkinName = "London Liquid Sky";
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(139, 43);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "조회하기";
            // 
            // grpBurningList
            // 
            this.grpBurningList.AppearanceCaption.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBurningList.AppearanceCaption.Options.UseFont = true;
            this.grpBurningList.Controls.Add(this.gcPatientlist);
            this.grpBurningList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpBurningList.Location = new System.Drawing.Point(0, 76);
            this.grpBurningList.Name = "grpBurningList";
            this.grpBurningList.Size = new System.Drawing.Size(984, 585);
            this.grpBurningList.TabIndex = 2;
            this.grpBurningList.Text = "CD/DVD 버닝 목록";
            // 
            // gcPatientlist
            // 
            this.gcPatientlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcPatientlist.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcPatientlist.Location = new System.Drawing.Point(2, 22);
            this.gcPatientlist.MainView = this.gvPatientlist;
            this.gcPatientlist.Name = "gcPatientlist";
            this.gcPatientlist.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repoQuantity,
            this.repoMediatype,
            this.repoPicDel});
            this.gcPatientlist.Size = new System.Drawing.Size(980, 561);
            this.gcPatientlist.TabIndex = 4;
            this.gcPatientlist.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPatientlist});
            // 
            // gvPatientlist
            // 
            this.gvPatientlist.Appearance.HeaderPanel.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvPatientlist.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvPatientlist.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvPatientlist.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvPatientlist.Appearance.Row.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvPatientlist.Appearance.Row.Options.UseFont = true;
            this.gvPatientlist.Appearance.Row.Options.UseTextOptions = true;
            this.gvPatientlist.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvPatientlist.Appearance.ViewCaption.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvPatientlist.Appearance.ViewCaption.Options.UseFont = true;
            this.gvPatientlist.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColBurnDate,
            this.gridColPatientNo,
            this.gridColPatientName,
            this.gridColGender,
            this.gridColAge,
            this.gridColSize,
            this.gridColMedia,
            this.gridQuantity,
            this.gridColResult,
            this.gridColRetry,
            this.gridColDelete});
            this.gvPatientlist.GridControl = this.gcPatientlist;
            this.gvPatientlist.Name = "gvPatientlist";
            this.gvPatientlist.OptionsCustomization.AllowColumnMoving = false;
            this.gvPatientlist.OptionsCustomization.AllowColumnResizing = false;
            this.gvPatientlist.OptionsCustomization.AllowFilter = false;
            this.gvPatientlist.OptionsCustomization.AllowGroup = false;
            this.gvPatientlist.OptionsCustomization.AllowSort = false;
            this.gvPatientlist.OptionsDetail.EnableMasterViewMode = false;
            this.gvPatientlist.OptionsDetail.ShowDetailTabs = false;
            this.gvPatientlist.OptionsDetail.SmartDetailExpand = false;
            this.gvPatientlist.OptionsSelection.MultiSelect = true;
            this.gvPatientlist.OptionsView.ColumnAutoWidth = false;
            this.gvPatientlist.OptionsView.ShowDetailButtons = false;
            this.gvPatientlist.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gvPatientlist.OptionsView.ShowGroupPanel = false;
            this.gvPatientlist.OptionsView.ShowIndicator = false;
            this.gvPatientlist.RowHeight = 25;
            // 
            // gridColBurnDate
            // 
            this.gridColBurnDate.Caption = "실행 일시";
            this.gridColBurnDate.FieldName = "patDate";
            this.gridColBurnDate.Name = "gridColBurnDate";
            this.gridColBurnDate.OptionsColumn.AllowEdit = false;
            this.gridColBurnDate.OptionsColumn.ReadOnly = true;
            this.gridColBurnDate.Visible = true;
            this.gridColBurnDate.VisibleIndex = 0;
            this.gridColBurnDate.Width = 120;
            // 
            // gridColPatientNo
            // 
            this.gridColPatientNo.Caption = "환자번호";
            this.gridColPatientNo.FieldName = "patNo";
            this.gridColPatientNo.Name = "gridColPatientNo";
            this.gridColPatientNo.OptionsColumn.AllowEdit = false;
            this.gridColPatientNo.OptionsColumn.ReadOnly = true;
            this.gridColPatientNo.Visible = true;
            this.gridColPatientNo.VisibleIndex = 1;
            this.gridColPatientNo.Width = 90;
            // 
            // gridColPatientName
            // 
            this.gridColPatientName.Caption = "환자명";
            this.gridColPatientName.FieldName = "patName";
            this.gridColPatientName.Name = "gridColPatientName";
            this.gridColPatientName.OptionsColumn.AllowEdit = false;
            this.gridColPatientName.OptionsColumn.ReadOnly = true;
            this.gridColPatientName.Visible = true;
            this.gridColPatientName.VisibleIndex = 2;
            this.gridColPatientName.Width = 130;
            // 
            // gridColGender
            // 
            this.gridColGender.Caption = "성별";
            this.gridColGender.FieldName = "patSex";
            this.gridColGender.Name = "gridColGender";
            this.gridColGender.OptionsColumn.AllowEdit = false;
            this.gridColGender.OptionsColumn.ReadOnly = true;
            this.gridColGender.Visible = true;
            this.gridColGender.VisibleIndex = 3;
            this.gridColGender.Width = 70;
            // 
            // gridColAge
            // 
            this.gridColAge.Caption = "나이";
            this.gridColAge.Name = "gridColAge";
            this.gridColAge.OptionsColumn.AllowEdit = false;
            this.gridColAge.OptionsColumn.ReadOnly = true;
            this.gridColAge.Visible = true;
            this.gridColAge.VisibleIndex = 4;
            this.gridColAge.Width = 65;
            // 
            // gridColSize
            // 
            this.gridColSize.Caption = "크기";
            this.gridColSize.FieldName = "mediSize";
            this.gridColSize.Name = "gridColSize";
            this.gridColSize.OptionsColumn.AllowEdit = false;
            this.gridColSize.OptionsColumn.ReadOnly = true;
            this.gridColSize.Visible = true;
            this.gridColSize.VisibleIndex = 5;
            this.gridColSize.Width = 80;
            // 
            // gridColMedia
            // 
            this.gridColMedia.Caption = "미디어";
            this.gridColMedia.ColumnEdit = this.repoMediatype;
            this.gridColMedia.FieldName = "mediType";
            this.gridColMedia.Name = "gridColMedia";
            this.gridColMedia.Visible = true;
            this.gridColMedia.VisibleIndex = 6;
            this.gridColMedia.Width = 100;
            // 
            // repoMediatype
            // 
            this.repoMediatype.AutoHeight = false;
            this.repoMediatype.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repoMediatype.Items.AddRange(new object[] {
            "CDR",
            "DVDR",
            "DVDR-DL"});
            this.repoMediatype.Name = "repoMediatype";
            this.repoMediatype.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // gridQuantity
            // 
            this.gridQuantity.Caption = "수량";
            this.gridQuantity.ColumnEdit = this.repoQuantity;
            this.gridQuantity.FieldName = "copies";
            this.gridQuantity.Name = "gridQuantity";
            this.gridQuantity.Visible = true;
            this.gridQuantity.VisibleIndex = 7;
            // 
            // repoQuantity
            // 
            this.repoQuantity.AutoHeight = false;
            this.repoQuantity.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repoQuantity.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7"});
            this.repoQuantity.Name = "repoQuantity";
            this.repoQuantity.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // gridColResult
            // 
            this.gridColResult.Caption = "결과";
            this.gridColResult.Name = "gridColResult";
            this.gridColResult.OptionsColumn.AllowEdit = false;
            this.gridColResult.OptionsColumn.ReadOnly = true;
            this.gridColResult.Visible = true;
            this.gridColResult.VisibleIndex = 8;
            this.gridColResult.Width = 90;
            // 
            // gridColRetry
            // 
            this.gridColRetry.Caption = "다시굽기";
            this.gridColRetry.Name = "gridColRetry";
            this.gridColRetry.OptionsColumn.AllowEdit = false;
            this.gridColRetry.OptionsColumn.ReadOnly = true;
            this.gridColRetry.Visible = true;
            this.gridColRetry.VisibleIndex = 9;
            this.gridColRetry.Width = 65;
            // 
            // gridColDelete
            // 
            this.gridColDelete.Caption = "폴더삭제";
            this.gridColDelete.ColumnEdit = this.repoPicDel;
            this.gridColDelete.FieldName = "DeleteIcon";
            this.gridColDelete.Name = "gridColDelete";
            this.gridColDelete.OptionsColumn.AllowEdit = false;
            this.gridColDelete.OptionsColumn.ReadOnly = true;
            this.gridColDelete.Visible = true;
            this.gridColDelete.VisibleIndex = 10;
            this.gridColDelete.Width = 65;
            // 
            // repoPicDel
            // 
            this.repoPicDel.Appearance.Image = global::RimageMedicalSystemV2.Properties.Resources.close_16x161;
            this.repoPicDel.Appearance.Options.UseImage = true;
            this.repoPicDel.AppearanceReadOnly.Image = global::RimageMedicalSystemV2.Properties.Resources.close_16x16;
            this.repoPicDel.AppearanceReadOnly.Options.UseImage = true;
            this.repoPicDel.Name = "repoPicDel";
            // 
            // OrderHistoryForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.grpBurningList);
            this.Controls.Add(this.panelControl1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "OrderHistoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "굽기 명령 전송 내역";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBurningList)).EndInit();
            this.grpBurningList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcPatientlist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPatientlist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repoMediatype)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repoQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repoPicDel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl grpBurningList;
        public DevExpress.XtraGrid.GridControl gcPatientlist;
        public DevExpress.XtraGrid.Views.Grid.GridView gvPatientlist;
        private DevExpress.XtraGrid.Columns.GridColumn gridColBurnDate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColPatientNo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColPatientName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColGender;
        private DevExpress.XtraGrid.Columns.GridColumn gridColAge;
        private DevExpress.XtraGrid.Columns.GridColumn gridColSize;
        private DevExpress.XtraGrid.Columns.GridColumn gridColMedia;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repoMediatype;
        private DevExpress.XtraGrid.Columns.GridColumn gridQuantity;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repoQuantity;
        private DevExpress.XtraGrid.Columns.GridColumn gridColResult;
        private DevExpress.XtraGrid.Columns.GridColumn gridColRetry;
        private DevExpress.XtraGrid.Columns.GridColumn gridColDelete;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repoPicDel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton btnSearch;
    }
}