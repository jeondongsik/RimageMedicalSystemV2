using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Net;

using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.LookAndFeel;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraPrintingLinks;

using Newtonsoft.Json;
using RimageKorea;

namespace RimageReport
{
    /// <summary>
    /// 굽기 보고서
    /// </summary>
    public partial class xFormReport : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 서버목록
        /// </summary>
        private List<ServerInfo> _serverList;
        /// <summary>
        /// RimageWeb 주소
        /// </summary>
        private string _webAddress = string.Empty;
        /// <summary>
        /// 현재 선택된 서버
        /// </summary>
        private ServerInfo _nowSelectedServer;

        /// <summary>
        /// 웹주소
        /// </summary>
        public string WebAddress
        {
            get { return _webAddress; }
            set { _webAddress = value; }
        }
        /// <summary>
        /// 등록된 서버 목록
        /// </summary>
        public List<ServerInfo> ServerList
        {
            get { return _serverList; }
            set { _serverList = value; }
        }
        /// <summary>
        /// 현재 선택된 서버
        /// </summary>
        public ServerInfo NowServerInfo
        {
            get { return _nowSelectedServer; }
            set { _nowSelectedServer = value; }
        }

        public xFormReport()
        {
            InitializeComponent();
        }

        /// <summary>
        /// load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xFormReport_Load(object sender, EventArgs e)
        {
            this.dateEditFrom.EditValue = DateTime.Now.Date;
            this.dateEditTo.EditValue = DateTime.Now.Date;

            this.gridMonth.Tag = DateTime.Now.Year.ToString();
            this.gridDays.Tag = DateTime.Now.Month.ToString().PadLeft(2, '0');

            this.BindingServerIPs();

            ////this._webAddress = string.Format("http://{0}/RimageWeb", this._nowSelectedServer.IP);

            ////this.BindingData(this.GetData());
        }

        /// <summary>
        /// 서버 아이피 목록 콤보박스에 바인딩
        /// </summary>
        private void BindingServerIPs()
        {
            if (this._serverList != null)
            {
                if (this._serverList.Count > 0)
                {
                    int idx = 0;
                    for (int i = 0; i < this._serverList.Count; i++)
                    {
                        this.cbServerIPs.Properties.Items.Add(this._serverList[i].IP);

                        if (this._serverList[i].IP == this._nowSelectedServer.IP)
                        {
                            idx = i;
                        }
                    }

                    this.cbServerIPs.SelectedIndex = idx;
                }
            }
        }

        /// <summary>
        /// 신청서 인쇄
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApplicationPrint_Click(object sender, EventArgs e)
        {
            if (this.xTabReport.SelectedTabPage == this.xPageResultSearch)
            {
                if (this.gridView1.SelectedRowsCount < 1)
                {
                    MessageBox.Show("인쇄할 환자를 선택하세요.", "Rimage Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (this.gridView1.GetFocusedRow() != null)
                {
                    try
                    {
                        string regID = this.gridView1.GetFocusedRowCellValue("PatientID").ToString();
                        string patName = this.gridView1.GetFocusedRowCellValue("PatientName").ToString();
                        long? rowid = this.gridView1.GetFocusedRowCellValue("rowid") as long?;

                        int rowHandle = this.gridView1.FocusedRowHandle;

                        this.gridView1.ExpandMasterRow(rowHandle);

                        GridView view = this.gridView1.GetDetailView(rowHandle, 0) as GridView;
                        DataView dv = view.DataSource as DataView;
                        DataTable dt = dv.Table;

                        DataRow[] rows = dt.Select("rowid = " + rowid.Value.ToString());

                        //dtD.Columns.Add("rowid", typeof(long));
                        //dtD.Columns.Add("StartDateTime", typeof(string));

                        ArrayList _listDataSource = new ArrayList();
                        int cnt = 0;

                        PatientInfo pInfo = null;

                        foreach (DataRow dr in rows)
                        {
                            cnt++;

                            string studyDate = dr["StudyDate"].ToString();
                            //string yyyy = string.Empty;
                            //string mm = string.Empty;
                            //string dd = string.Empty;

                            //if (studyDate.Length > 7)
                            //{
                            //    yyyy = studyDate.Substring(0, 4);
                            //    mm = studyDate.Substring(4, 2);
                            //    dd = studyDate.Substring(6, 2);

                            //    studyDate = string.Format("{0}-{1}-{2}", yyyy, mm, dd);
                            //}

                            if (cnt % 2 != 0)
                            {
                                pInfo = new PatientInfo(studyDate, dr["StudyDescription"].ToString());
                                _listDataSource.Add(pInfo);
                            }
                            else
                            {
                                int indx = (cnt / 2) - 1;

                                ((PatientInfo)_listDataSource[indx]).StudyDate2 = studyDate;
                                ((PatientInfo)_listDataSource[indx]).StudyDesc2 = dr["StudyDescription"].ToString();
                            }
                        }

                        this.PrintReport(regID, patName, _listDataSource, cnt);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// 보고서 인쇄
        /// </summary>
        /// <param name="regID"></param>
        /// <param name="patName"></param>
        /// <param name="ds"></param>
        private void PrintReport(string regID, string patName, ArrayList _listDataSource, int studyCount)
        {
            //CustomerConfirm report = new CustomerConfirm();
            CopyIssuePaper report = new CopyIssuePaper();
            report.Parameters["paramRegID"].Value = regID;
            report.Parameters["paramPatientName"].Value = patName;
            report.Parameters["paramDatetime"].Value = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            report.Parameters["paramTotalCount"].Value = studyCount.ToString("###, ##0");
            
            report.DataSource = _listDataSource;

            using (ReportPrintTool printTool = new ReportPrintTool(report))
            {
                printTool.PreviewRibbonForm.PrintControl.ExecCommand(PrintingSystemCommand.DocumentMap, new object[] { false });

                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.DocumentMap, CommandVisibility.None);
                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.Open, CommandVisibility.None);
                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.Save, CommandVisibility.None);
                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.Thumbnails, CommandVisibility.None);
                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.Watermark, CommandVisibility.None);
                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.Parameters, CommandVisibility.None);
                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.Background, CommandVisibility.None);
                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.FillBackground, CommandVisibility.None);

                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.ExportCsv, CommandVisibility.None);
                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.ExportGraphic, CommandVisibility.None);
                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.ExportMht, CommandVisibility.None);
                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.ExportRtf, CommandVisibility.None);
                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.ExportXps, CommandVisibility.None);
                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.ExportTxt, CommandVisibility.None);

                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.SendGraphic, CommandVisibility.None);
                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.SendMht, CommandVisibility.None);
                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.SendRtf, CommandVisibility.None);
                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.SendTxt, CommandVisibility.None);
                printTool.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.SendXps, CommandVisibility.None);

                printTool.ShowRibbonPreviewDialog(UserLookAndFeel.Default);
                //printTool.Print();
            }
        }

        /// <summary>
        /// 보고서 다이렉트 인쇄
        /// </summary>
        /// <param name="regID"></param>
        /// <param name="patName"></param>
        /// <param name="_listDataSource"></param>
        public void PrintDirect(string regID, string patName, ArrayList _listDataSource, int studyCount)
        {
            //CustomerConfirm report = new CustomerConfirm();
            CopyIssuePaper report = new CopyIssuePaper();
            report.Parameters["paramRegID"].Value = regID;
            report.Parameters["paramPatientName"].Value = patName;
            report.Parameters["paramDatetime"].Value = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            report.Parameters["paramTotalCount"].Value = studyCount.ToString("###, ##0");

            report.DataSource = _listDataSource;
            report.Print();
        }

        /// <summary>
        /// 결과 조회 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonSearch_Click(object sender, EventArgs e)
        {
            if (this.dateEditFrom.EditValue == null)
            {
                MessageBox.Show("조회 시작일자를 입력하세요.", "Rimage Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dateEditFrom.Focus();
                return;
            }
            if (this.dateEditTo.EditValue == null)
            {
                MessageBox.Show("조회 종료일자를 입력하세요.", "Rimage Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dateEditTo.Focus();
                return;
            }

            DateTime dtFrom = this.dateEditFrom.DateTime.Date;
            DateTime dtTo = this.dateEditTo.DateTime.Date;

            int deff = dtTo.AddMonths(-6).Date.CompareTo(dtFrom);

            if (deff == 1 && string.IsNullOrWhiteSpace(this.textEditPatientID.Text) && string.IsNullOrWhiteSpace(this.textEditPatientName.Text))
            {
                MessageBox.Show("기간으로만 조회시 조회기간은 최대 6개월입니다.\r\n시작일자와 종료일자 간격을 6개월 이내로 입력하세요.", "Rimage Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dateEditTo.Focus();
                return;
            }

            this.BindingData(this.GetData());
        }

        /// <summary>
        /// 웹서버에 연결해서 데이터를 가져온다.
        /// </summary>
        /// <returns></returns>
        private DataSet GetData()
        {
            string response = string.Empty;
            string webaddress = string.Empty;

            //webaddress = "http://" + HostIP + "/RimageWeb/ResultSearch.aspx";
            //webaddress = "http://localhost:4557/RimageWeb/ResultSearch.aspx";
            webaddress = string.Format("{0}/ResultSearch.aspx", this._webAddress);

            this.Cursor = Cursors.WaitCursor;

            try
            {
                using (WebClient webc = new WebClient())
                {
                    NameValueCollection nvcol = new NameValueCollection();

                    string startDate = string.Empty;
                    string endDate = string.Empty;

                    startDate = string.Format("{0}000000", (this.dateEditFrom.EditValue == null) ? DateTime.Now.AddDays(-7).ToString("yyyyMMdd") : Convert.ToDateTime(this.dateEditFrom.EditValue).ToString("yyyyMMdd"));
                    endDate = string.Format("{0}235959", (this.dateEditTo.EditValue == null) ? DateTime.Now.ToString("yyyyMMdd") : Convert.ToDateTime(this.dateEditTo.EditValue).ToString("yyyyMMdd"));

                    nvcol.Add("StartDate", startDate);
                    nvcol.Add("EndDate", endDate);
                    nvcol.Add("PatientID", this.textEditPatientID.Text);
                    nvcol.Add("PatientName", this.textEditPatientName.Text.Replace("'", ""));
                    nvcol.Add("MediaType", (this.comboBoxEditMedia.Text == "전체") ? "" : this.comboBoxEditMedia.Text);

                    byte[] ResponseByte = webc.UploadValues(webaddress, nvcol);
                    response = Encoding.UTF8.GetString(ResponseByte);

                    webc.Dispose();

                    if (string.IsNullOrWhiteSpace(response))
                        return null;

                    if (response.StartsWith("Error"))
                        return null;

                    DataSet ds = JsonConvert.DeserializeObject(response, typeof(DataSet)) as DataSet;

                    return ds;
                }
            }
            catch 
            {

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return null;
        }

        /// <summary>
        /// 그리드에 데이터를 바인딩한다.
        /// </summary>
        /// <param name="ds"></param>
        private void BindingData(DataSet ds)
        {
            try
            {
                this.grdSearchResult.DataSource = null;

                DataSet dsResult = new DataSet();

                DataTable dtH = new DataTable();
                DataTable dtD = new DataTable();
                dtD.Columns.Add("rowid", typeof(long));
                dtD.Columns.Add("StartDateTime", typeof(string));
                //dtD.Columns.Add("Idx", typeof(string));
                dtD.Columns.Add("StudyDate", typeof(string));
                dtD.Columns.Add("StudyDescription", typeof(string));
                dtD.TableName = "Study";
                //dtD.PrimaryKey = new DataColumn[] { dtD.Columns[0], dtD.Columns[1] };
                
                dtH = ds.Tables[0].Copy();
                dtH.TableName = "ResultHeader";

                if (dtH.Rows.Count == 0)
                {
                    this.gridView1.OptionsView.ShowViewCaption = true;
                    this.gridView1.ViewCaption = "조회된 데이터가 없습니다.";
                }
                else
                {
                    this.gridView1.OptionsView.ShowViewCaption = false;

                    foreach (DataRow dr in dtH.Rows)
                    {
                        dr["StartDateTime"] = this.ConverToDateString(Utils.CheckNull(dr["StartDateTime"]), "yyyyMMddHHmmss");
                        dr["EndDateTime"] = this.ConverToDateString(Utils.CheckNull(dr["EndDateTime"]), "yyyyMMddHHmmss");

                        string[] arrStudy = null;

                        if (dr["StudyDesc"].ToString().Contains("^^"))
                        {
                            arrStudy = Utils.SplitByString(dr["StudyDesc"].ToString(), "^^");
                        }
                        else
                        {
                            arrStudy = new string[1];
                            arrStudy[0] = dr["StudyDesc"].ToString();
                        }

                        foreach (string str in arrStudy)
                        {
                            if (!string.IsNullOrWhiteSpace(str))
                            {
                                DataRow newRow = dtD.NewRow();
                                newRow["rowid"] = dr["rowid"];
                                newRow["StartDateTime"] = dr["StartDateTime"];

                                if (str.StartsWith("[") && str.Contains("]"))
                                {
                                    //// 예전 형식이면
                                    newRow["StudyDate"] = this.ConverToDateString(str.Substring(1, str.IndexOf(']') - 1), "yyyyMMdd");
                                    newRow["StudyDescription"] = str.Substring(str.IndexOf(']') + 1);
                                }
                                else if (str.Contains(":"))
                                {
                                    newRow["StudyDate"] = this.ConverToDateString(str.Substring(0, str.IndexOf(':')), "yyyyMMdd");
                                    newRow["StudyDescription"] = str.Substring(str.IndexOf(':') + 2);
                                }
                                else
                                {
                                    newRow["StudyDate"] = "";
                                    newRow["StudyDescription"] = str;
                                }

                                dtD.Rows.Add(newRow);
                            }
                        }

                        dr["StudyDesc"] = dr["StudyDesc"].ToString().Replace("^^", Environment.NewLine);
                    }

                    dsResult.Tables.Add(dtH);
                    dsResult.Tables.Add(dtD);

                    DataColumn keyColumn = dsResult.Tables["ResultHeader"].Columns["rowid"];
                    DataColumn foreignKeyColumn = dsResult.Tables["Study"].Columns["rowid"];
                    dsResult.Relations.Add("BurnInfo", keyColumn, foreignKeyColumn);

                    this.grdSearchResult.DataSource = dsResult.Tables["ResultHeader"];

                    this.grdSearchResult.LevelTree.Nodes.Add("BurnInfo", gridView2);
                }
            }
            catch (Exception ex)
            {
                RimageKorea.ErrorLog.LogWrite(this, ex.ToString(), Environment.CurrentDirectory);
            }
        }

        private string ConverToDateString(string date, string format)
        {
            string ret = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(date) || date.Length < 8)
                    return date;

                if (date.Length < 14)
                    date.PadRight(14, '0');

                if (format == "yyyyMMdd")
                {
                    ret = string.Format("{0}-{1}-{2}", date.Substring(0, 4), date.Substring(4, 2), date.Substring(6, 2));
                }
                else if (format == "yyyyMMddHHmmss")
                {
                    ret = string.Format("{0}-{1}-{2} {3}:{4}:{5}", date.Substring(0, 4), date.Substring(4, 2), date.Substring(6, 2),
                        date.Substring(8, 2), date.Substring(10, 2), date.Substring(12, 2));
                }
                else
                {
                    ret = date;
                }
            }
            catch { return date; }

            return ret;
        }

        /// <summary>
        /// 탭페이지 선택 시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xTabReport_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == this.xPageDailySum)
            {
                this.BindingStatics(this.GetStatics("", ""));

                try
                {
                    this.gridViewYear.FocusedRowHandle = this.gridViewYear.RowCount - 1;
                    this.gridViewMonth.FocusedRowHandle = DateTime.Now.Month - 1;
                    this.gridViewDays.FocusedRowHandle = DateTime.Now.Day - 1;
                }
                catch { }
            }
        }

        /// <summary>
        /// 통계 가져오기
        /// </summary>
        /// <returns></returns>
        private DataSet GetStatics(string year, string month)
        {
            string response = string.Empty;
            string webaddress = string.Empty;

            //webaddress = "http://" + HostIP + "/RimageWeb/ResultStatics.aspx";
            //webaddress = "http://localhost:4557/RimageWeb/ResultStatics.aspx";
            webaddress = string.Format("{0}/ResultStatics.aspx", this._webAddress);

            this.Cursor = Cursors.WaitCursor;

            try
            {
                using (WebClient webc = new WebClient())
                {
                    NameValueCollection nvcol = new NameValueCollection();

                    nvcol.Add("year", year);
                    nvcol.Add("month", month);

                    byte[] ResponseByte = webc.UploadValues(webaddress, nvcol);
                    response = Encoding.UTF8.GetString(ResponseByte);

                    webc.Dispose();

                    if (string.IsNullOrWhiteSpace(response))
                        return null;

                    if (response.StartsWith("Error"))
                        return null;

                    DataSet ds = JsonConvert.DeserializeObject(response, typeof(DataSet)) as DataSet;

                    return ds;
                }
            }
            catch
            {
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return null;
        }

        /// <summary>
        /// 통계 바인딩
        /// </summary>
        /// <param name="ds"></param>
        private void BindingStatics(DataSet ds, string target = "")
        {
            try
            {
                if (target == "")
                {
                    if (ds.Tables.Contains("tblYear"))
                    {
                        this.gridYear.DataSource = ds.Tables["tblYear"];
                    }
                }

                if (target == "" || target == "month")
                {
                    if (ds.Tables.Contains("tblMonth"))
                        this.gridMonth.DataSource = ds.Tables["tblMonth"];
                }

                if (target == "" || target == "month" || target == "day")
                {
                    if (ds.Tables.Contains("tblDay"))
                        this.gridDays.DataSource = ds.Tables["tblDay"];
                }
            }
            catch { }            
        }

        private void gridViewYear_RowClick(object sender, RowClickEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                string year = this.gridViewYear.GetFocusedRowCellValue("Year").ToString();
                this.gridMonth.Tag = year;
                this.BindingStatics(this.GetStatics(year, ""), "month");
            }
        }

        private void gridViewMonth_RowClick(object sender, RowClickEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                string month = this.gridViewMonth.GetFocusedRowCellValue("Code").ToString();
                string year = this.gridMonth.Tag.ToString();
                this.gridDays.Tag = month;
                this.BindingStatics(this.GetStatics(year, month), "day");
            }
        }

        private void gridViewYear_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.DisplayText != "0")
            {
                //e.Appearance.ForeColor = Color.Blue;
                e.Appearance.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            }
        }

        private void gridViewMonth_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.DisplayText != "0")
            {
                //e.Appearance.ForeColor = Color.Blue;
                e.Appearance.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            }
            else
            {
                e.DisplayText = string.Empty;
            }
        }

        private void gridViewDays_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.DisplayText != "0")
            {
                //e.Appearance.ForeColor = Color.Blue;
                e.Appearance.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            }
            else
            {
                e.DisplayText = string.Empty;
            }
        }

        /// <summary>
        /// 인쇄하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {            
            ////https://documentation.devexpress.com/#WindowsForms/CustomDocument3427

            if (this.xTabReport.SelectedTabPage == this.xPageResultSearch)
            {
                this.gridView1.ShowRibbonPrintPreview();
            }
            else
            {
                CompositeLink comosLink = new CompositeLink(new PrintingSystem());

                PrintableComponentLink pcLink1 = new PrintableComponentLink();
                PrintableComponentLink pcLink2 = new PrintableComponentLink();
                PrintableComponentLink pcLink3 = new PrintableComponentLink();

                pcLink1.Component = this.gridYear;
                pcLink2.Component = this.gridMonth;
                pcLink3.Component = this.gridDays;

                comosLink.Links.Add(pcLink1);
                comosLink.Links.Add(pcLink2);
                comosLink.Links.Add(pcLink3);

                this.PrintInit(comosLink.PrintingSystemBase);
                comosLink.ShowRibbonPreviewDialog(this.LookAndFeel);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //this.gridView1.ExportToXlsx(@"D:\test.xlsx");

            xFormSelectFilePath frm = new xFormSelectFilePath();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = frm.FileName;
                if (!fileName.ToLower().EndsWith(".xlsx"))
                    fileName += ".xlsx";

                this.ExportToFile(fileName);
            }
            
            frm.Dispose();
        }

        /// <summary>
        /// 엑셀로 저장
        /// </summary>
        /// <param name="filePath"></param>
        private void ExportToFile(string filePath)
        {
            if (this.xTabReport.SelectedTabPage == this.xPageResultSearch)
            {                
                this.gridView1.ExportToXlsx(filePath);
            }
            else
            {
                CompositeLink comosLink = new CompositeLink(new PrintingSystem());

                PrintableComponentLink pcLink1 = new PrintableComponentLink();
                PrintableComponentLink pcLink2 = new PrintableComponentLink();
                PrintableComponentLink pcLink3 = new PrintableComponentLink();

                pcLink1.Component = this.gridYear;
                pcLink2.Component = this.gridMonth;
                pcLink3.Component = this.gridDays;

                comosLink.Links.Add(pcLink1);
                comosLink.Links.Add(pcLink2);
                comosLink.Links.Add(pcLink3);

                comosLink.ExportToXlsx(filePath);
            }

            MessageBox.Show("저장되었습니다.", "Rimage Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 프린트 미리보기 화면 세팅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            try
            {
                PrintingSystemBase pb = e.PrintingSystem as PrintingSystemBase;
                this.PrintInit(pb);
            }
            catch { }
        }

        /// <summary>
        /// 프린트 미리보기 화면 세팅
        /// </summary>
        /// <param name="pb"></param>
        private void PrintInit(PrintingSystemBase pb)
        {
            try
            {
                pb.SetCommandVisibility(PrintingSystemCommand.Open, CommandVisibility.None);
                pb.SetCommandVisibility(PrintingSystemCommand.Save, CommandVisibility.None);
                pb.SetCommandVisibility(PrintingSystemCommand.Thumbnails, CommandVisibility.None);
                pb.SetCommandVisibility(PrintingSystemCommand.Watermark, CommandVisibility.None);
                pb.SetCommandVisibility(PrintingSystemCommand.Parameters, CommandVisibility.None);
                pb.SetCommandVisibility(PrintingSystemCommand.Background, CommandVisibility.None);
                pb.SetCommandVisibility(PrintingSystemCommand.FillBackground, CommandVisibility.None);

                pb.SetCommandVisibility(PrintingSystemCommand.ExportCsv, CommandVisibility.None);
                pb.SetCommandVisibility(PrintingSystemCommand.ExportGraphic, CommandVisibility.None);
                pb.SetCommandVisibility(PrintingSystemCommand.ExportMht, CommandVisibility.None);
                pb.SetCommandVisibility(PrintingSystemCommand.ExportRtf, CommandVisibility.None);
                pb.SetCommandVisibility(PrintingSystemCommand.ExportXps, CommandVisibility.None);
                pb.SetCommandVisibility(PrintingSystemCommand.ExportTxt, CommandVisibility.None);

                pb.SetCommandVisibility(PrintingSystemCommand.SendFile, CommandVisibility.None);
                pb.SetCommandVisibility(PrintingSystemCommand.MultiplePages, CommandVisibility.None);

                pb.SetCommandVisibility(PrintingSystemCommand.Customize, CommandVisibility.None);
                pb.SetCommandVisibility(PrintingSystemCommand.EditPageHF, CommandVisibility.None);
                pb.SetCommandVisibility(PrintingSystemCommand.Find, CommandVisibility.None);
            }
            catch { }
        }

        /// <summary>
        /// 복사 신청서 인쇄 - 굽기 화면에서 호출
        /// </summary>
        /// <param name="PatientID"></param>
        /// <param name="PatientName"></param>
        /// <param name="StudyDesc"></param>
        public void PrintCopyRequestForm(string PatientID, string PatientName, string StudyDesc)
        {
            try
            {
                if (!StudyDesc.EndsWith("^^"))
                    StudyDesc += "^^";

                string[] arrStudy = Utils.SplitByString(StudyDesc, "^^");

                ArrayList _listDataSource = new ArrayList();
                string sDate = string.Empty;
                string sCont = string.Empty;
                string yyyy = string.Empty;
                string mm = string.Empty;
                string dd = string.Empty;

                int cnt = 0;
                PatientInfo pInfo = null;

                foreach (string study in arrStudy)
                {
                    if (!string.IsNullOrWhiteSpace(study))
                    {
                        cnt++;

                        sDate = study.Substring(0, study.IndexOf(':'));
                        sCont = study.Substring(study.IndexOf(':') + 2);

                        if (sDate.Length > 7)
                        {
                            yyyy = sDate.Substring(0, 4);
                            mm = sDate.Substring(4, 2);
                            dd = sDate.Substring(6, 2);

                            sDate = string.Format("{0}-{1}-{2}", yyyy, mm, dd);
                        }

                        if (cnt % 2 != 0)
                        {
                            pInfo = new PatientInfo(sDate, sCont);
                            _listDataSource.Add(pInfo);
                        }
                        else
                        {
                            int indx = (cnt / 2) - 1;

                            ((PatientInfo)_listDataSource[indx]).StudyDate2 = sDate;
                            ((PatientInfo)_listDataSource[indx]).StudyDesc2 = sCont;
                        }
                    }
                }

                if (_listDataSource.Count > 0)
                {                    
                    this.PrintDirect(PatientID, PatientName, _listDataSource, cnt);
                }
            }
            catch { }
        }

        /// <summary>
        /// 서버아이피 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbServerIPs_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._webAddress = string.Format("http://{0}/RimageWeb", this.cbServerIPs.Text);

            if (this.xTabReport.SelectedTabPage == this.xPageDailySum)
            {
                this.BindingStatics(this.GetStatics("", ""));

                try
                {
                    this.gridViewYear.FocusedRowHandle = this.gridViewYear.RowCount - 1;
                    this.gridViewMonth.FocusedRowHandle = DateTime.Now.Month - 1;
                    this.gridViewDays.FocusedRowHandle = DateTime.Now.Day - 1;
                }
                catch { }
            }
            else
            {
                this.BindingData(this.GetData());
            }
        }
    }
}