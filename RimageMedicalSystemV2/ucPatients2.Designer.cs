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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucPatients2));
            this.gcPatientlist = new DevExpress.XtraGrid.GridControl();
            this.gvPatientlist = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColBurnDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColPatientNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColPatientName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColGender = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColMedia = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSize = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColCancel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPatientlist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPatientlist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // gcPatientlist
            // 
            this.gcPatientlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcPatientlist.Location = new System.Drawing.Point(0, 0);
            this.gcPatientlist.MainView = this.gvPatientlist;
            this.gcPatientlist.Name = "gcPatientlist";
            this.gcPatientlist.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.repositoryItemComboBox2,
            this.repositoryItemButtonEdit1});
            this.gcPatientlist.Size = new System.Drawing.Size(817, 208);
            this.gcPatientlist.TabIndex = 3;
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
            this.gridQuantity,
            this.gridColMedia,
            this.gridColSize,
            this.gridColCancel});
            this.gvPatientlist.GridControl = this.gcPatientlist;
            this.gvPatientlist.Name = "gvPatientlist";
            this.gvPatientlist.OptionsBehavior.Editable = false;
            this.gvPatientlist.OptionsBehavior.ReadOnly = true;
            this.gvPatientlist.OptionsCustomization.AllowColumnMoving = false;
            this.gvPatientlist.OptionsCustomization.AllowFilter = false;
            this.gvPatientlist.OptionsCustomization.AllowSort = false;
            this.gvPatientlist.OptionsView.ColumnAutoWidth = false;
            this.gvPatientlist.OptionsView.ShowGroupPanel = false;
            this.gvPatientlist.OptionsView.ShowIndicator = false;
            this.gvPatientlist.RowHeight = 25;
            // 
            // gridColBurnDate
            // 
            this.gridColBurnDate.Caption = "날짜";
            this.gridColBurnDate.Name = "gridColBurnDate";
            this.gridColBurnDate.Visible = true;
            this.gridColBurnDate.VisibleIndex = 0;
            this.gridColBurnDate.Width = 100;
            // 
            // gridColPatientNo
            // 
            this.gridColPatientNo.Caption = "환자번호";
            this.gridColPatientNo.Name = "gridColPatientNo";
            this.gridColPatientNo.Visible = true;
            this.gridColPatientNo.VisibleIndex = 1;
            this.gridColPatientNo.Width = 120;
            // 
            // gridColPatientName
            // 
            this.gridColPatientName.Caption = "환자명";
            this.gridColPatientName.FieldName = "WareHousingDate";
            this.gridColPatientName.Name = "gridColPatientName";
            this.gridColPatientName.Visible = true;
            this.gridColPatientName.VisibleIndex = 2;
            this.gridColPatientName.Width = 150;
            // 
            // gridColGender
            // 
            this.gridColGender.Caption = "성별";
            this.gridColGender.FieldName = "BrandName";
            this.gridColGender.Name = "gridColGender";
            this.gridColGender.Visible = true;
            this.gridColGender.VisibleIndex = 3;
            // 
            // gridQuantity
            // 
            this.gridQuantity.Caption = "수량";
            this.gridQuantity.ColumnEdit = this.repositoryItemComboBox1;
            this.gridQuantity.Name = "gridQuantity";
            this.gridQuantity.Visible = true;
            this.gridQuantity.VisibleIndex = 4;
            // 
            // gridColMedia
            // 
            this.gridColMedia.Caption = "미디어";
            this.gridColMedia.ColumnEdit = this.repositoryItemComboBox2;
            this.gridColMedia.Name = "gridColMedia";
            this.gridColMedia.Visible = true;
            this.gridColMedia.VisibleIndex = 5;
            this.gridColMedia.Width = 100;
            // 
            // gridColSize
            // 
            this.gridColSize.Caption = "크기";
            this.gridColSize.Name = "gridColSize";
            this.gridColSize.Visible = true;
            this.gridColSize.VisibleIndex = 6;
            this.gridColSize.Width = 110;
            // 
            // gridColCancel
            // 
            this.gridColCancel.Caption = "취소";
            this.gridColCancel.ColumnEdit = this.repositoryItemButtonEdit1;
            this.gridColCancel.Name = "gridColCancel";
            this.gridColCancel.Visible = true;
            this.gridColCancel.VisibleIndex = 7;
            this.gridColCancel.Width = 55;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.Appearance.Options.UseTextOptions = true;
            this.repositoryItemButtonEdit1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit1.ContextImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit1.ContextImageOptions.Image")));
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            // 
            // ucPatients2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gcPatientlist);
            this.LookAndFeel.SkinName = "Sharp Plus";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "ucPatients2";
            this.Size = new System.Drawing.Size(817, 208);
            ((System.ComponentModel.ISupportInitialize)(this.gcPatientlist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPatientlist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcPatientlist;
        private DevExpress.XtraGrid.Views.Grid.GridView gvPatientlist;
        private DevExpress.XtraGrid.Columns.GridColumn gridColBurnDate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColPatientNo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColPatientName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColGender;
        private DevExpress.XtraGrid.Columns.GridColumn gridQuantity;
        private DevExpress.XtraGrid.Columns.GridColumn gridColMedia;
        private DevExpress.XtraGrid.Columns.GridColumn gridColSize;
        private DevExpress.XtraGrid.Columns.GridColumn gridColCancel;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
    }
}
