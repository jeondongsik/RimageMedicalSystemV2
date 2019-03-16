using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RimageWebDBCopy
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSqliteFileSearch_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtSqliteFilePath.Text = this.openFileDialog1.FileName;
            }
        }

        private void btnCopyStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtSqliteFilePath.Text))
            {
                MessageBox.Show("SQLite DB 파일을 선택하세요.");
                this.txtSqliteFilePath.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(this.txtFrom.Text))
            {
                MessageBox.Show("복사할 기간을 숫자로 입력하세요.");
                this.txtFrom.Focus();
                return;
            }

            this.CopyStart();
        }

        private void CopyStart()
        {
            try
            {
                DataTable dt = null;
                MsSqlData mssql = new MsSqlData();

                int addYears = 2;

                if (!int.TryParse(this.txtFrom.Text, out addYears))
                {
                    MessageBox.Show("복사할 기간을 숫자로만 입력하세요.");
                    this.txtFrom.Focus();
                    return;
                }

                string fromDate = string.Format("{0}000000", DateTime.Now.AddYears(addYears * -1).ToString("yyyyMMdd"));
                this.label2.Text = string.Format("{0} 이후의 데이터만 복사합니다.", DateTime.Now.AddYears(addYears * -1).ToString("yyyyMMdd"));
                
                dt = mssql.GetData(fromDate);

                if (dt != null)
                {
                    this.label3.Text = string.Format("총 {0}건", dt.Rows.Count);
                    this.progressBar1.Maximum = dt.Rows.Count;
                    this.progressBar1.Value = 0;

                    if (!this.backgroundWorker1.IsBusy)
                    {
                        this.backgroundWorker1.RunWorkerAsync(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {                
                DataTable dt = e.Argument as DataTable;
                int cnt = 0;

                if (dt != null)
                {
                    Sqlite slite = new Sqlite(this.txtSqliteFilePath.Text);
                    string msg = string.Empty;

                    foreach (DataRow dr in dt.Rows)
                    {
                        string result = slite.Insert(
                            dr["Idx"].ToString(),
                            dr["startDateTime"].ToString(),
                            dr["endDateTime"].ToString(),
                            dr["PatientID"].ToString(),
                            dr["PatientName"].ToString(),
                            Convert.ToInt32(dr["Copies"]),
                            dr["MediaType"].ToString(),
                            dr["FilesLength"].ToString(),
                            dr["IPAddress"].ToString(),
                            dr["Status"].ToString(),
                            dr["StudyDesc"].ToString()
                            );

                        if (string.IsNullOrWhiteSpace(result))
                            msg = string.Format("{0}-{1}-{2}-{3} >> 완료\r\n", dr["Idx"].ToString(), dr["startDateTime"].ToString(), dr["PatientID"].ToString(), dr["PatientName"].ToString());
                        else
                            msg = string.Format("{0} >> 실패\r\n", result);

                        this.backgroundWorker1.ReportProgress(cnt, msg);

                        cnt++;
                    }

                    ////MessageBox.Show("데이터 복사 완료!!");
                }
            }
            catch { }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                string result = e.UserState.ToString();
                this.txtResultProgress.AppendText(result);

                this.progressBar1.Value = e.ProgressPercentage;
                ////this.label4.Text = e.ProgressPercentage.ToString();
            }
            catch { }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("데이터 복사 완료!!");
        }
    }
}
