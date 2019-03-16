namespace RimageWebDBCopy
{
    partial class frmMain
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSqliteFileSearch = new System.Windows.Forms.Button();
            this.txtSqliteFilePath = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtResultProgress = new System.Windows.Forms.TextBox();
            this.btnCopyStart = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSqliteFileSearch);
            this.groupBox1.Controls.Add(this.txtSqliteFilePath);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(551, 56);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "대상 Database (SQLite DB File 위치)";
            // 
            // btnSqliteFileSearch
            // 
            this.btnSqliteFileSearch.Location = new System.Drawing.Point(473, 24);
            this.btnSqliteFileSearch.Name = "btnSqliteFileSearch";
            this.btnSqliteFileSearch.Size = new System.Drawing.Size(68, 21);
            this.btnSqliteFileSearch.TabIndex = 1;
            this.btnSqliteFileSearch.Text = "파일찾기";
            this.btnSqliteFileSearch.UseVisualStyleBackColor = true;
            this.btnSqliteFileSearch.Click += new System.EventHandler(this.btnSqliteFileSearch_Click);
            // 
            // txtSqliteFilePath
            // 
            this.txtSqliteFilePath.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtSqliteFilePath.Location = new System.Drawing.Point(12, 24);
            this.txtSqliteFilePath.Name = "txtSqliteFilePath";
            this.txtSqliteFilePath.Size = new System.Drawing.Size(455, 21);
            this.txtSqliteFilePath.TabIndex = 0;
            this.txtSqliteFilePath.Text = "C:\\inetpub\\wwwroot\\WebRsm\\RimageWeb\\App_Data\\RimageDB.sqlite";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "전체|*.*";
            this.openFileDialog1.InitialDirectory = "C:\\inetpub\\wwwroot";
            // 
            // txtResultProgress
            // 
            this.txtResultProgress.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtResultProgress.Location = new System.Drawing.Point(12, 167);
            this.txtResultProgress.MaxLength = 3276700;
            this.txtResultProgress.Multiline = true;
            this.txtResultProgress.Name = "txtResultProgress";
            this.txtResultProgress.ReadOnly = true;
            this.txtResultProgress.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResultProgress.Size = new System.Drawing.Size(551, 324);
            this.txtResultProgress.TabIndex = 1;
            // 
            // btnCopyStart
            // 
            this.btnCopyStart.Location = new System.Drawing.Point(485, 123);
            this.btnCopyStart.Name = "btnCopyStart";
            this.btnCopyStart.Size = new System.Drawing.Size(78, 21);
            this.btnCopyStart.TabIndex = 2;
            this.btnCopyStart.Text = "복사 시작";
            this.btnCopyStart.UseVisualStyleBackColor = true;
            this.btnCopyStart.Click += new System.EventHandler(this.btnCopyStart_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.White;
            this.progressBar1.Location = new System.Drawing.Point(12, 123);
            this.progressBar1.Maximum = 1000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(467, 21);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 3;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtFrom);
            this.groupBox2.Location = new System.Drawing.Point(12, 74);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(551, 43);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "복사 대상 데이터";
            // 
            // txtFrom
            // 
            this.txtFrom.Location = new System.Drawing.Point(12, 16);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(65, 21);
            this.txtFrom.TabIndex = 0;
            this.txtFrom.Text = "2";
            this.txtFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(81, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "년전 이후의 데이터";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(328, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 12);
            this.label2.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(422, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 12);
            this.label3.TabIndex = 5;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 503);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnCopyStart);
            this.Controls.Add(this.txtResultProgress);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "MSQLS -> SQLite";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSqliteFilePath;
        private System.Windows.Forms.Button btnSqliteFileSearch;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtResultProgress;
        private System.Windows.Forms.Button btnCopyStart;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

