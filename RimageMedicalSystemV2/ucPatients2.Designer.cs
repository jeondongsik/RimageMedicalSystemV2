namespace RimageMedicalSystemV2
{
    partial class ucPatients2
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
            this.gcPatientlist = new DevExpress.XtraGrid.GridControl();
            this.gvPatientlist = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColBurnDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColPatientNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColPatientName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColGender = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repoQuantity = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColMedia = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repoMediatype = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColSize = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColCancel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repoPicDel = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPatientlist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPatientlist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repoQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repoMediatype)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repoPicDel)).BeginInit();
            this.SuspendLayout();
            // 
            // gcPatientlist
            // 
            this.gcPatientlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcPatientlist.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcPatientlist.Location = new System.Drawing.Point(0, 0);
            this.gcPatientlist.LookAndFeel.SkinName = "VS2010";
            this.gcPatientlist.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcPatientlist.MainView = this.gvPatientlist;
            this.gcPatientlist.Name = "gcPatientlist";
            this.gcPatientlist.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repoQuantity,
            this.repoMediatype,
            this.repoPicDel});
            this.gcPatientlist.Size = new System.Drawing.Size(802, 208);
            this.gcPatientlist.TabIndex = 3;
            this.gcPatientlist.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPatientlist});
            // 
            // gvPatientlist
            // 
            this.gvPatientlist.Appearance.HeaderPanel.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gvPatientlist.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvPatientlist.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvPatientlist.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvPatientlist.Appearance.Row.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gvPatientlist.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvPatientlist.Appearance.Row.Options.UseFont = true;
            this.gvPatientlist.Appearance.Row.Options.UseForeColor = true;
            this.gvPatientlist.Appearance.Row.Options.UseTextOptions = true;
            this.gvPatientlist.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvPatientlist.Appearance.ViewCaption.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gvPatientlist.Appearance.ViewCaption.Options.UseFont = true;
            this.gvPatientlist.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColBurnDate,
            this.gridColPatientNo,
            this.gridColPatientName,
            this.gridColGender,
            this.gridQuantity,
            this.gridColMedia,
            this.gridColSize,
            this.gridColCancel});
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
            this.gvPatientlist.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gvPatientlist_RowCellClick);
            // 
            // gridColBurnDate
            // 
            this.gridColBurnDate.Caption = "날짜";
            this.gridColBurnDate.FieldName = "patDate";
            this.gridColBurnDate.Name = "gridColBurnDate";
            this.gridColBurnDate.OptionsColumn.AllowEdit = false;
            this.gridColBurnDate.OptionsColumn.ReadOnly = true;
            this.gridColBurnDate.Visible = true;
            this.gridColBurnDate.VisibleIndex = 0;
            this.gridColBurnDate.Width = 100;
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
            this.gridColPatientNo.Width = 115;
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
            this.gridColPatientName.Width = 145;
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
            this.gridColGender.Width = 80;
            // 
            // gridQuantity
            // 
            this.gridQuantity.Caption = "수량";
            this.gridQuantity.ColumnEdit = this.repoQuantity;
            this.gridQuantity.FieldName = "copies";
            this.gridQuantity.Name = "gridQuantity";
            this.gridQuantity.Visible = true;
            this.gridQuantity.VisibleIndex = 4;
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
            // gridColMedia
            // 
            this.gridColMedia.Caption = "미디어";
            this.gridColMedia.ColumnEdit = this.repoMediatype;
            this.gridColMedia.FieldName = "mediType";
            this.gridColMedia.Name = "gridColMedia";
            this.gridColMedia.Visible = true;
            this.gridColMedia.VisibleIndex = 5;
            this.gridColMedia.Width = 110;
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
            // gridColSize
            // 
            this.gridColSize.Caption = "크기";
            this.gridColSize.FieldName = "mediSize";
            this.gridColSize.Name = "gridColSize";
            this.gridColSize.OptionsColumn.AllowEdit = false;
            this.gridColSize.OptionsColumn.ReadOnly = true;
            this.gridColSize.Visible = true;
            this.gridColSize.VisibleIndex = 6;
            this.gridColSize.Width = 110;
            // 
            // gridColCancel
            // 
            this.gridColCancel.Caption = "취소";
            this.gridColCancel.ColumnEdit = this.repoPicDel;
            this.gridColCancel.FieldName = "DeleteIcon";
            this.gridColCancel.Name = "gridColCancel";
            this.gridColCancel.OptionsColumn.AllowEdit = false;
            this.gridColCancel.OptionsColumn.ReadOnly = true;
            this.gridColCancel.Visible = true;
            this.gridColCancel.VisibleIndex = 7;
            this.gridColCancel.Width = 40;
            // 
            // repoPicDel
            // 
            this.repoPicDel.Appearance.Image = global::RimageMedicalSystemV2.Properties.Resources.close_16x161;
            this.repoPicDel.Appearance.Options.UseImage = true;
            this.repoPicDel.AppearanceReadOnly.Image = global::RimageMedicalSystemV2.Properties.Resources.close_16x16;
            this.repoPicDel.AppearanceReadOnly.Options.UseImage = true;
            this.repoPicDel.Name = "repoPicDel";
            // 
            // ucPatients2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gcPatientlist);
            this.DoubleBuffered = true;
            this.LookAndFeel.SkinName = "Lilian";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "ucPatients2";
            this.Size = new System.Drawing.Size(802, 208);
            ((System.ComponentModel.ISupportInitialize)(this.gcPatientlist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPatientlist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repoQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repoMediatype)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repoPicDel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraGrid.Columns.GridColumn gridColBurnDate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColPatientNo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColPatientName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColGender;
        private DevExpress.XtraGrid.Columns.GridColumn gridQuantity;
        private DevExpress.XtraGrid.Columns.GridColumn gridColMedia;
        private DevExpress.XtraGrid.Columns.GridColumn gridColSize;
        private DevExpress.XtraGrid.Columns.GridColumn gridColCancel;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repoQuantity;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repoMediatype;
        public DevExpress.XtraGrid.GridControl gcPatientlist;
        public DevExpress.XtraGrid.Views.Grid.GridView gvPatientlist;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repoPicDel;
    }
}
