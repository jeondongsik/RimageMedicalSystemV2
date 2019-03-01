namespace RimageMedicalSystemV2
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
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpDrives)).BeginInit();
            this.grpDrives.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
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
            // btnStartCopy
            // 
            this.btnStartCopy.AllowFocus = false;
            this.btnStartCopy.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartCopy.Appearance.Options.UseFont = true;
            this.btnStartCopy.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnStartCopy.ImageOptions.Image")));
            this.btnStartCopy.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnStartCopy.Location = new System.Drawing.Point(135, 59);
            this.btnStartCopy.Name = "btnStartCopy";
            this.btnStartCopy.Size = new System.Drawing.Size(218, 24);
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
            this.labelControl11.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl11.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.labelControl11.Appearance.Options.UseFont = true;
            this.labelControl11.Appearance.Options.UseForeColor = true;
            this.labelControl11.Location = new System.Drawing.Point(6, 6);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(330, 30);
            this.labelControl11.TabIndex = 17;
            this.labelControl11.Text = "* USB가 인식될 경우 자동으로 선택됩니다.\r\n* 그렇지 않은 경우 USB를 선택한 후 복사 버튼을 클릭하세요.";
            // 
            // txtProgressView
            // 
            this.txtProgressView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(84)))));
            this.txtProgressView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProgressView.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProgressView.ForeColor = System.Drawing.Color.White;
            this.txtProgressView.Location = new System.Drawing.Point(135, 89);
            this.txtProgressView.MaxLength = 2147483647;
            this.txtProgressView.Multiline = true;
            this.txtProgressView.Name = "txtProgressView";
            this.txtProgressView.ReadOnly = true;
            this.txtProgressView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProgressView.Size = new System.Drawing.Size(218, 418);
            this.txtProgressView.TabIndex = 55;
            this.txtProgressView.TabStop = false;
            // 
            // grpDrives
            // 
            this.grpDrives.Controls.Add(this.flpDrives);
            this.grpDrives.Location = new System.Drawing.Point(12, 60);
            this.grpDrives.Name = "grpDrives";
            this.grpDrives.Size = new System.Drawing.Size(117, 447);
            this.grpDrives.TabIndex = 2;
            this.grpDrives.Text = "Drive";
            // 
            // flpDrives
            // 
            this.flpDrives.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpDrives.Location = new System.Drawing.Point(2, 23);
            this.flpDrives.Name = "flpDrives";
            this.flpDrives.Size = new System.Drawing.Size(113, 422);
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
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(358, 519);
            this.Controls.Add(this.panelControl1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Sharp Plus";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximizeBox = false;
            this.Name = "frmCopyToUSB";
            this.Text = "USB로 복사하기";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCopyToUSB_FormClosing);
            this.Load += new System.EventHandler(this.frmCopyToUSB_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
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
    }
}