using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Net;
using System.Diagnostics;
using DevExpress.XtraEditors;

using RimageKorea;

namespace RimageConfigSetting
{
    public partial class ConfigSetting : DevExpress.XtraEditors.XtraForm
    {
        string HostIP;
        string HostName;
        string HostPort;
        string LocalShareFolder;
        string CDLabelFile;
        string DVDLabelFile;
        string DVDDLLabelFile;
        string MergeFileFolder;
        string MergeFileServerFolder;
        string ServerNetworkDrive;
        string DicomDownloadFolder;
        string myIP;
        string HospitalName;
        string LogFilePath;
        string LabelPrint;
        string MergePrint;
        string CDMaxSize;
        string LocalSourceFolder;
        string SourceFolder;
        string programType;
        string startProgram;
        string CDPrintYN;
        string MsgBoxShow;
        string CDRemainQty;
        string RibbonRemainQty;
        string BinCheckTime;
        string FolderPattern;
        string RecorderCnt;
        string ReportView;
        string DelType;
        string AutoCloseType;
        string DeleteAfterBurn;
        string DownloadCheckbyFileName;
        string DelFolderWhenClose;
        string RetentionPeriod;
        string ServerListFile;
        DataSet dsServerList;
        string ServerType;
        string CDLabelFolder;
        string DVDLabelFolder;
        string DVDDLLabelFolder;
        string AutoExecute;
        string AutoExecuteHookingType;
        string ProgramCaption;
        string BurnPatientKind;
        //2015.09.06 add
        string PopUpAlamYN;
        string PopUpKeepTime;
        string PopUpSelPatInfoYN;
        string HookSleepTime1;
        string HookSleepTime2;
        string ManualExeBtnView;
        string CloseConfirm;
        string SoundAlam;
        //2016.07.09 add
        string ExistDicomDir;
        string FileCopyYN;
        string ErrorPupupUse;
        string AutoPrintApp;
        string BottomImagePath;
        //2016.12.24 add
        string DeleteInJobFolder;
        //2018.02.15 add
        string DownCheckFileName;
        //2018.02.20 add
        string EnableBoxMultiPatient;
        /// <summary>
        /// 폴더사이즈 체크 시간(초)
        /// </summary>
        string FolderSizeCheckTime;
        /// <summary>
        /// DVD MaxSize
        /// </summary>
        string DvdMaxSize;
        /// <summary>
        /// USB 복사 사용
        /// </summary>
        string UseUSBCopy;
        /// <summary>
        /// 서버명을 IP로 표시
        /// </summary>
        string DisplayServeIP;
        /// <summary>
        /// 조회시 다운로드 폴더 용량 체크하기
        /// </summary>
        string IsSizeCheck;

        /// <summary>
        /// 1:다른 프로그램에서 실행
        /// 2:자체 실행
        /// </summary>
        public string comeWhere
        {
            get;
            set;
        }
        public object sender
        {
            get;
            set;
        }
        public EventArgs eventArgs
        {
            get;
            set;
        }

        /// <summary>
        /// 생성자
        /// </summary>
        public ConfigSetting()
        {
            InitializeComponent();

            label17.Text = "· 로고이미지 경로\r\n    (165*180)";
            ServerListFile = Path.Combine(Application.StartupPath, "ServerList.xml");

            KillProcess.DelProcess(GlobalVar.RIMAGE_ENTERANCE);

            //설정값 가져오기
            this.getConfig();

            this.dsServerList = new DataSet();
            //서버목록 가져오기
            this.getServerList();

            this.textBox_HostIP.Focus();
            this.textBox_HostIP.SelectAll();
        }

        /// <summary>
        /// Load 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigSetting_Load(object sender, EventArgs e)
        {
            if (File.Exists(GlobalVar.CheckFileList) == true)
            {
                this.label_CheckFiles.Text = "(체크할 파일 있음)";
                this.label_CheckFiles.ForeColor = Color.Red;
            }
            else
            {
                this.label_CheckFiles.Text = "(체크할 파일 없음)";
                this.label_CheckFiles.ForeColor = Color.Blue;
            }

            this.Text = GlobalVar.PRODUCT_NAME + " 환경설정";
        }

        /// <summary>
        /// 설정값 가져오기
        /// </summary>
        public void getConfig()
        {
            Config cf = new Config(Application.StartupPath);

            if (cf._Message == "정상")
            {
                HostIP = cf._HostIP;
                HostName = cf._HostName;
                HostPort = cf._HostPort;
                LocalShareFolder = cf._LocalShareFolder;
                CDLabelFile = cf._CDLabelFile;
                DVDLabelFile = cf._DVDLabelFile;
                DVDDLLabelFile = cf._DVDDLLabelFile;

                MergeFileFolder = cf._MergeFileFolder;
                MergeFileServerFolder = cf._MergeFileServerFolder;
                ServerNetworkDrive = cf._ServerNetworkDrive;
                DicomDownloadFolder = cf._DicomDownloadFolder;
                HospitalName = cf._HospitalName;
                LogFilePath = cf._LogFilePath;
                BottomImagePath = cf._BottomImagePath;
                LabelPrint = cf._LabelPrint;
                MergePrint = cf._MergePrint;
                if (this.MergePrint.Length != 7)
                {
                    if (this.MergePrint.Length == 6)
                        this.MergePrint += "N";
                    else
                        this.MergePrint = "YYNYNNN";
                }

                CDMaxSize = cf._CDMaxSize;
                LocalSourceFolder = cf._LocalSourceFolder;
                SourceFolder = cf._SourceFolder;
                programType = cf._programType;
                startProgram = cf._startProgram;
                CDPrintYN = cf._CDPrintYN;
                MsgBoxShow = cf._MsgBoxShow;
                if (MsgBoxShow == "Y" || string.IsNullOrWhiteSpace(MsgBoxShow))
                    MsgBoxShow = "3";
                CDRemainQty = (string.IsNullOrWhiteSpace(cf._CDRemainQty)) ? "10" : cf._CDRemainQty;
                RibbonRemainQty = (string.IsNullOrWhiteSpace(cf._RibbonRemainQty)) ? "10" : cf._RibbonRemainQty;
                BinCheckTime = (string.IsNullOrWhiteSpace(cf._BinCheckTime)) ? "3" : cf._BinCheckTime;
                FolderPattern = (string.IsNullOrWhiteSpace(cf._FolderPattern)) ? "0" : cf._FolderPattern;
                RecorderCnt = (string.IsNullOrWhiteSpace(cf._RecorderCnt)) ? "2" : cf._RecorderCnt;
                AutoExecute = (string.IsNullOrWhiteSpace(cf._AutoExecute)) ? "0" : cf._AutoExecute;
                ReportView = (string.IsNullOrWhiteSpace(cf._ReportView)) ? "Y" : cf._ReportView;
                DelType = (string.IsNullOrWhiteSpace(cf._DelType)) ? "0" : cf._DelType;
                AutoCloseType = (string.IsNullOrWhiteSpace(cf._AutoCloseType)) ? "N" : cf._AutoCloseType;
                DeleteAfterBurn = (string.IsNullOrWhiteSpace(cf._DeleteAfterBurn)) ? "0" : cf._DeleteAfterBurn;
                DownloadCheckbyFileName = (string.IsNullOrWhiteSpace(cf._DownloadCheckbyFileName)) ? "N" : cf._DownloadCheckbyFileName;
                DelFolderWhenClose = (string.IsNullOrWhiteSpace(cf._DelFolderWhenClose)) ? "Y" : cf._DelFolderWhenClose;
                RetentionPeriod = (string.IsNullOrWhiteSpace(cf._RetentionPeriod)) ? "1" : cf._RetentionPeriod;
                ServerType = (string.IsNullOrWhiteSpace(cf._ServerType)) ? "R" : cf._ServerType;
                AutoExecuteHookingType = (string.IsNullOrWhiteSpace(cf._AutoExecuteHookingType)) ? "0" : cf._AutoExecuteHookingType;

                ProgramCaption = cf._ProgramCaption;
                BurnPatientKind = (string.IsNullOrWhiteSpace(cf._BurnPatientKind)) ? "NORM" : cf._BurnPatientKind;

                PopUpAlamYN = (string.IsNullOrWhiteSpace(cf._PopUpAlamYN)) ? "Y" : cf._PopUpAlamYN;
                PopUpKeepTime = (string.IsNullOrWhiteSpace(cf._PopUpKeepTime)) ? "20" : cf._PopUpKeepTime;
                PopUpSelPatInfoYN = (string.IsNullOrWhiteSpace(cf._PopUpSelPatInfoYN)) ? "Y" : cf._PopUpSelPatInfoYN;

                HookSleepTime1 = (string.IsNullOrWhiteSpace(cf._HookSleepTime1)) ? "3000" : cf._HookSleepTime1;
                HookSleepTime2 = (string.IsNullOrWhiteSpace(cf._HookSleepTime2)) ? "6000" : cf._HookSleepTime2;

                ManualExeBtnView = (string.IsNullOrWhiteSpace(cf._ManualExeBtnView)) ? "N" : cf._ManualExeBtnView;

                CloseConfirm = (string.IsNullOrWhiteSpace(cf._CloseConfirm)) ? "N" : cf._CloseConfirm;
                SoundAlam = (string.IsNullOrWhiteSpace(cf._SoundAlam)) ? "N" : cf._SoundAlam;

                ExistDicomDir = (string.IsNullOrWhiteSpace(cf._ExistDicomDir)) ? "Y" : cf._ExistDicomDir;
                FileCopyYN = (string.IsNullOrWhiteSpace(cf._FileCopyYN)) ? "N" : cf._FileCopyYN;
                ErrorPupupUse = (string.IsNullOrWhiteSpace(cf._ErrorPupupUse)) ? "Y" : cf._ErrorPupupUse;
                AutoPrintApp = (string.IsNullOrWhiteSpace(cf._AutoPrintApp)) ? "N" : cf._AutoPrintApp;

                DeleteInJobFolder = (string.IsNullOrWhiteSpace(cf._DeleteInJobFolder)) ? "N" : cf._DeleteInJobFolder;
                DownCheckFileName = (string.IsNullOrWhiteSpace(cf._DownCheckFileName)) ? "end.txt" : cf._DownCheckFileName;
                EnableBoxMultiPatient = (string.IsNullOrWhiteSpace(cf._DisableMultiPatient)) ? "N" : cf._DisableMultiPatient;

                FolderSizeCheckTime = (string.IsNullOrWhiteSpace(cf._FolderSizeCheckTime)) ? "5" : cf._FolderSizeCheckTime;
                DvdMaxSize = (string.IsNullOrWhiteSpace(cf._DvdMaxSize)) ? "4831838208" : cf._DvdMaxSize;
                UseUSBCopy = (string.IsNullOrWhiteSpace(cf._UseUSBCopy)) ? "N" : cf._UseUSBCopy;
                DisplayServeIP = (string.IsNullOrWhiteSpace(cf._DisplayServeIP)) ? "N" : cf._DisplayServeIP;

                IsSizeCheck = (string.IsNullOrWhiteSpace(cf._IsSizeCheck)) ? "N" : cf._IsSizeCheck;

                myIP = GetMyIP.MyIP();
                setControl();
            }
            else
            {
                DialogResult dr = MessageBox.Show(cf._Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (dr == DialogResult.OK)
                    this.Close();
            }
        }

        /// <summary>
        /// 가져온 값 컨트롤에 입력
        /// </summary>
        public void setControl()
        {
            this.textBox_HostIP.Text = HostIP;
            this.textBox_HostName.Text = HostName;
            this.textBox_HostPort.Text = HostPort;
            this.textBox_LocalShareFolder.Text = LocalShareFolder;

            int index1 = CDLabelFile.LastIndexOf("\\");
            int index2 = DVDLabelFile.LastIndexOf("\\");
            int index3 = DVDDLLabelFile.LastIndexOf("\\");

            if (index1 > 0)
            {
                this.textBox_CDLabelFile.Text = CDLabelFile.Substring(index1 + 1);
                this.textBox_CDLabelFolder.Text = CDLabelFile.Substring(0, index1 + 1);
            }
            else
            {
                this.textBox_CDLabelFile.Text = "";
                this.textBox_CDLabelFolder.Text = "\\\\ServerIP\\Rimage\\Labels\\";
            }

            if (index2 > 0)
            {
                this.textBox_DVDLabelFile.Text = DVDLabelFile.Substring(index1 + 1);
                this.textBox_DVDLabelFolder.Text = DVDLabelFile.Substring(0, index1 + 1);
            }
            else
            {
                this.textBox_DVDLabelFile.Text = "";
                this.textBox_DVDLabelFolder.Text = "\\\\ServerIP\\Rimage\\Labels\\";
            }

            if (index3 > 0)
            {
                this.textBox_DVD_DLLabelFile.Text = DVDDLLabelFile.Substring(index1 + 1);
                this.textBox_DVD_DLLabelFolder.Text = DVDDLLabelFile.Substring(0, index1 + 1);
            }
            else
            {
                this.textBox_DVD_DLLabelFile.Text = "";
                this.textBox_DVD_DLLabelFolder.Text = "\\\\ServerIP\\Rimage\\Labels\\";
            }

            this.CDLabelFolder = textBox_CDLabelFolder.Text;
            this.DVDLabelFolder = textBox_DVDLabelFolder.Text;
            this.DVDDLLabelFolder = textBox_DVD_DLLabelFolder.Text;

            this.textBox_MergeFileServerFolder.Text = MergeFileServerFolder;
            this.textBox_ServerNetworkDrive.Text = ServerNetworkDrive;
            this.textBox_DicomDownloadFolder.Text = DicomDownloadFolder;
            this.textBox_HospitalName.Text = HospitalName;

            if (!string.IsNullOrWhiteSpace(this.LogFilePath) && File.Exists(this.GetImageFolderPath(this.LogFilePath)))
            {
                this.pictureLogoFile.Image = Image.FromFile(this.GetImageFolderPath(this.LogFilePath));
                this.pictureLogoFile.Tag = this.LogFilePath;
            }
            if (!string.IsNullOrWhiteSpace(this.BottomImagePath) && File.Exists(this.GetImageFolderPath(this.BottomImagePath)))
            {
                this.pictureAppBottomImage.Image = Image.FromFile(this.GetImageFolderPath(this.BottomImagePath));
                this.pictureAppBottomImage.Tag = this.BottomImagePath;
            }

            if (this.CDMaxSize.Length < 4 && this.CDMaxSize != "")
            {
                this.CDMaxSize = (Convert.ToInt32(this.CDMaxSize) * 1024 * 1024).ToString();
            }
            this.textBox_MaxSize.Text = CDMaxSize;
            this.textBox_ServerFolder.Text = LocalSourceFolder;
            this.textBox_ShareFolder.Text = SourceFolder;

            this.textBox_CDRemain.Text = CDRemainQty;
            this.textBox_RibbonRemain.Text = RibbonRemainQty;
            this.textBox_BinCheckTime.Text = BinCheckTime;
            this.comboBox_FolderPattern.SelectedIndex = Convert.ToInt16(FolderPattern);
            this.comboBox_RecorderCnt.SelectedItem = RecorderCnt;
            this.comboBox_AutoExec.SelectedIndex = Convert.ToInt16(AutoExecute);
            this.comboBoxMessageAlert.SelectedIndex = Convert.ToInt32(MsgBoxShow);
            this.comboBox_AutoExecuteHookingType.SelectedIndex = Convert.ToInt16(AutoExecuteHookingType);

            this.textBox_HookSleepTime1.Text = (Convert.ToInt32(this.HookSleepTime1) / 1000).ToString();
            this.textBox_HookSleepTime2.Text = (Convert.ToInt32(this.HookSleepTime2) / 1000).ToString();

            this.checkBox_LabelPrint.Checked = this.LabelPrint.Equals("Y");
            this.checkBox_CDPrintYN.Checked = this.CDPrintYN.Equals("Y");

            if (this.MergePrint.Length == 7)
            {
                this.checkBox_merge1.Checked = this.MergePrint.Substring(0, 1).Equals("Y");
                this.checkBox_merge2.Checked = this.MergePrint.Substring(1, 1).Equals("Y");
                this.checkBox_merge3.Checked = this.MergePrint.Substring(2, 1).Equals("Y");
                this.checkBox_merge4.Checked = this.MergePrint.Substring(3, 1).Equals("Y");
                this.checkBox_merge5.Checked = this.MergePrint.Substring(4, 1).Equals("Y");
                this.checkBox_merge6.Checked = this.MergePrint.Substring(5, 1).Equals("Y");
                this.checkBox_merge7.Checked = this.MergePrint.Substring(6, 1).Equals("Y");
            }
            else
            {
                this.checkBox_merge1.Checked = true;
                this.checkBox_merge2.Checked = true;
                this.checkBox_merge3.Checked = false;
                this.checkBox_merge4.Checked = true;
                this.checkBox_merge5.Checked = false;
                this.checkBox_merge6.Checked = false;
                this.checkBox_merge7.Checked = false;
            }

            if (this.programType.Equals("1"))
                this.comboBox_ProgramType.SelectedIndex = 0;
            else
                this.comboBox_ProgramType.SelectedIndex = 1;

            if (this.startProgram.Equals("2"))
            {
                this.comboBox_StartProgram.SelectedIndex = 1;
            }
            else if (startProgram.Equals("3"))
            {
                this.comboBox_StartProgram.SelectedIndex = 2;
            }
            else
            {
                this.comboBox_StartProgram.SelectedIndex = 0;
            }

            this.checkBox_ReportView.Checked = ReportView.Equals("Y");

            if (this.DelType.Equals("0"))
            {
                this.rdoDelType0.Checked = true;
                this.rdoDelType1.Checked = false;
            }
            else if (this.DelType.Equals("1"))
            {
                this.rdoDelType0.Checked = false;
                this.rdoDelType1.Checked = true;
            }
            else
            {
                this.rdoDelType0.Checked = false;
                this.rdoDelType1.Checked = false;
            }

            this.checkBoxAutoCloseType.Checked = AutoCloseType.Equals("Y");
            this.checkDownloadCheckbyFileName.Checked = DownloadCheckbyFileName.Equals("Y");
            this.checkBox_DelFolderWhenClose.Checked = DelFolderWhenClose.Equals("Y");

            this.comboBox_DeleteAfterBurn.SelectedIndex = Convert.ToInt16(DeleteAfterBurn);
            this.textBox_RetentionPeriod.Text = RetentionPeriod;

            this.txtProgramCaption.Text = this.ProgramCaption;

            if (this.BurnPatientKind.Equals("MULTY"))
            {
                this.rdoBurnPatientOnlyOne.Checked = false;
                this.rdoBurnPatientMulty.Checked = true;
            }
            else
            {
                this.rdoBurnPatientOnlyOne.Checked = true;
                this.rdoBurnPatientMulty.Checked = false;
            }

            this.checkBoxPopUpAlamYN.Checked = (this.PopUpAlamYN.Equals("Y"));
            this.textBoxPopUpKeepTime.Text = this.PopUpKeepTime;
            this.checkBoxPopUpSelPatInfoYN.Checked = (this.PopUpSelPatInfoYN.Equals("Y"));
            this.checkBox_ManualExeBtnView.Checked = this.ManualExeBtnView.Equals("Y");
            this.checkBox_ConfirmPopup.Checked = this.CloseConfirm.Equals("Y");
            this.checkBox_AlamSound.Checked = this.SoundAlam.Equals("Y");
            this.checkBoxExistDicomDir.Checked = this.ExistDicomDir.Equals("Y");
            this.checkBoxFileCopyYN.Checked = this.FileCopyYN.Equals("Y");
            this.checkBoxErrorPupupUse.Checked = this.ErrorPupupUse.Equals("Y");
            this.checkBoxAutoPrintApp.Checked = this.AutoPrintApp.Equals("Y");
            this.checkBoxDeleteInJobFolder.Checked = this.DeleteInJobFolder.Equals("Y");

            this.textBox_DownCheckFileName.Text = this.DownCheckFileName;
            this.checkBoxDisableMultiPatient.Checked = this.EnableBoxMultiPatient.Equals("Y");

            this.textBoxFolderSizeCheckTime.Text = this.FolderSizeCheckTime;
            this.textBox_DvdMaxSize.Text = this.DvdMaxSize;
            this.checkBoxUseUSBCopy.Checked = this.UseUSBCopy.Equals("Y");
            this.checkBoxDisplayServeIP.Checked = this.DisplayServeIP.Equals("Y");

            this.checkboxIsSizeCheck.Checked = this.IsSizeCheck.Equals("Y");

            this.SetServerType();
            this.SetControlByServerType();
        }

        /// <summary>
        /// 서버 목록 가져오기
        /// </summary>
        private void getServerList()
        {
            dataGridViewServerList.DataSource = null;

            if (File.Exists(this.ServerListFile))
            {
                this.dsServerList.ReadXml(this.ServerListFile);

                //if (this.dsServerList.Tables[0].Rows.Count == 0)
                //{
                //    DataRow dr = this.dsServerList.Tables[0].NewRow();
                //    dr["No"] = "1";
                //    dr["IP"] = this.HostIP;
                //    dr["Name"] = this.HostName;
                //    dr["Port"] = this.HostPort;
                //    this.dsServerList.Tables[0].Rows.Add(dr);
                //}
            }
            else
            {
                //파일생성한다.
                DataTable dt = new DataTable("List");
                dt.Columns.Add("No", typeof(System.String));
                dt.Columns.Add("Name", typeof(System.String));
                dt.Columns.Add("IP", typeof(System.String));
                dt.Columns.Add("Port", typeof(System.String));

                DataRow dr = dt.NewRow();
                dr["No"] = "1";
                dr["IP"] = this.HostIP;
                dr["Name"] = this.HostName;
                dr["Port"] = this.HostPort;
                dt.Rows.Add(dr);

                this.dsServerList.Tables.Add(dt);
                this.SaveServerListFile();
                this.SetControlByServerType();
            }

            dataGridViewServerList.DataSource = this.dsServerList.Tables[0];
        }

        /// <summary>
        /// 닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_cancel_Click(object sender, EventArgs e)
        {
            if (this.comeWhere == "1")
            {
                //MessageBox.Show("\"서버연결\" 버튼을 클릭하여 시스템에 다시 연결하세요.", "Rimage Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string labelPrint = "N";
            string mergePrint = "";
            string progType = "1";
            string startProg = "1";
            string cdPrint = "Y";
            string msgBoxShowYN = "Y";
            string folderPattern = "0";
            string delType = "0";

            try
            {
                if (checkValue())
                {
                    //IP 체크
                    //if (textBox_HostIP_PingCheck())
                    //{
                    Config cf = new Config(Application.StartupPath);

                    if (checkBox_LabelPrint.Checked)
                        labelPrint = "Y";
                    else
                        labelPrint = "N";

                    //Merge 프린트할 항목
                    if (checkBox_merge1.Checked)
                        mergePrint += "Y";
                    else
                        mergePrint += "N";

                    if (checkBox_merge2.Checked)
                        mergePrint += "Y";
                    else
                        mergePrint += "N";

                    if (checkBox_merge3.Checked)
                        mergePrint += "Y";
                    else
                        mergePrint += "N";

                    if (checkBox_merge4.Checked)
                        mergePrint += "Y";
                    else
                        mergePrint += "N";

                    if (checkBox_merge5.Checked)
                        mergePrint += "Y";
                    else
                        mergePrint += "N";

                    if (checkBox_merge6.Checked)
                        mergePrint += "Y";
                    else
                        mergePrint += "N";

                    if (checkBox_merge7.Checked)
                        mergePrint += "Y";
                    else
                        mergePrint += "N";
                    //////////////////////////////////////////


                    if (checkBox_CDPrintYN.Checked)
                        cdPrint = "Y";
                    else
                        cdPrint = "N";

                    msgBoxShowYN = comboBoxMessageAlert.SelectedIndex.ToString();
                    folderPattern = comboBox_FolderPattern.SelectedIndex.ToString();
                    ServerNetworkDrive = textBox_ServerNetworkDrive.Text;
                    MergeFileServerFolder = textBox_MergeFileServerFolder.Text;

                    if (!ServerNetworkDrive.EndsWith("\\"))
                        ServerNetworkDrive += "\\";
                    if (!MergeFileServerFolder.EndsWith("\\"))
                        MergeFileServerFolder += "\\";

                    MergeFileFolder = MergeFileServerFolder;

                    if (comboBox_ProgramType.SelectedIndex == 0)
                        progType = "1";
                    else
                        progType = "2";

                    startProg = (comboBox_StartProgram.SelectedIndex + 1).ToString();

                    if (rdoDelType0.Checked)
                        delType = "0";
                    else
                        delType = "1";

                    //등록된 서버중 첫번째 정보를 환경설정 파일에 저장
                    foreach (DataRow dr in this.dsServerList.Tables[0].Rows)
                    {
                        cf._HostIP = dr["IP"].ToString();
                        cf._HostName = dr["Name"].ToString();
                        cf._HostPort = dr["Port"].ToString();

                        break;
                    }

                    cf._LocalShareFolder = textBox_LocalShareFolder.Text;
                    cf._CDLabelFile = this.textBox_CDLabelFolder.Text + textBox_CDLabelFile.Text;
                    cf._DVDLabelFile = this.textBox_DVDLabelFolder.Text + textBox_DVDLabelFile.Text;
                    cf._DVDDLLabelFile = this.textBox_DVD_DLLabelFolder.Text + textBox_DVD_DLLabelFile.Text;
                    cf._MergeFileFolder = MergeFileFolder;
                    cf._MergeFileServerFolder = MergeFileServerFolder;
                    cf._ServerNetworkDrive = ServerNetworkDrive;
                    cf._DicomDownloadFolder = textBox_DicomDownloadFolder.Text;
                    cf._HospitalName = textBox_HospitalName.Text;
                    cf._LogFilePath = (this.pictureLogoFile.Tag == null) ? string.Empty : this.pictureLogoFile.Tag.ToString();
                    cf._BottomImagePath = (this.pictureAppBottomImage.Tag == null) ? string.Empty : this.pictureAppBottomImage.Tag.ToString();
                    cf._LabelPrint = labelPrint;
                    cf._MergePrint = mergePrint;
                    cf._CDMaxSize = (textBox_MaxSize.Text == "") ? "730000000" : textBox_MaxSize.Text;
                    cf._LocalSourceFolder = textBox_ServerFolder.Text;
                    cf._SourceFolder = textBox_ShareFolder.Text;
                    cf._programType = progType;
                    cf._startProgram = startProg;
                    cf._CDPrintYN = cdPrint;
                    cf._MsgBoxShow = msgBoxShowYN;
                    cf._CDRemainQty = (textBox_CDRemain.Text == "") ? "10" : textBox_CDRemain.Text;
                    cf._RibbonRemainQty = (textBox_RibbonRemain.Text == "") ? "10" : textBox_RibbonRemain.Text;
                    cf._BinCheckTime = (textBox_BinCheckTime.Text == "") ? "3" : textBox_BinCheckTime.Text;
                    cf._FolderPattern = folderPattern;
                    cf._RecorderCnt = (comboBox_RecorderCnt.SelectedItem.ToString() == "") ? "2" : comboBox_RecorderCnt.SelectedItem.ToString();
                    cf._AutoExecute = comboBox_AutoExec.SelectedIndex.ToString();
                    cf._AutoExecuteHookingType = comboBox_AutoExecuteHookingType.SelectedIndex.ToString();
                    cf._ReportView = (checkBox_ReportView.Checked) ? "Y" : "N";
                    cf._DelType = delType;
                    cf._AutoCloseType = (checkBoxAutoCloseType.Checked) ? "Y" : "N";
                    cf._DownloadCheckbyFileName = (checkDownloadCheckbyFileName.Checked) ? "Y" : "N";
                    cf._DeleteAfterBurn = comboBox_DeleteAfterBurn.SelectedIndex.ToString();
                    cf._DelFolderWhenClose = (checkBox_DelFolderWhenClose.Checked) ? "Y" : "N";
                    cf._RetentionPeriod = String.IsNullOrWhiteSpace(textBox_RetentionPeriod.Text) ? "1" : textBox_RetentionPeriod.Text;
                    cf._ProgramCaption = txtProgramCaption.Text;

                    cf._HookSleepTime1 = (Convert.ToInt32((String.IsNullOrWhiteSpace(textBox_HookSleepTime1.Text) ? "3" : textBox_HookSleepTime1.Text)) * 1000).ToString();
                    cf._HookSleepTime2 = (Convert.ToInt32((String.IsNullOrWhiteSpace(textBox_HookSleepTime2.Text) ? "5" : textBox_HookSleepTime2.Text)) * 1000).ToString();

                    cf._ManualExeBtnView = (checkBox_ManualExeBtnView.Checked) ? "Y" : "N";

                    if (this.rdoBurnPatientMulty.Checked == true)
                        cf._BurnPatientKind = "MULTY";
                    else
                        cf._BurnPatientKind = "NORM";

                    cf._PopUpAlamYN = (this.checkBoxPopUpAlamYN.Checked) ? "Y" : "N";
                    cf._PopUpKeepTime = this.textBoxPopUpKeepTime.Text;
                    cf._PopUpSelPatInfoYN = (this.checkBoxPopUpSelPatInfoYN.Checked) ? "Y" : "N";
                    cf._SoundAlam = (this.checkBox_AlamSound.Checked) ? "Y" : "N";
                    cf._CloseConfirm = (this.checkBox_ConfirmPopup.Checked) ? "Y" : "N";
                    cf._ExistDicomDir = (this.checkBoxExistDicomDir.Checked) ? "Y" : "N";
                    cf._FileCopyYN = (this.checkBoxFileCopyYN.Checked) ? "Y" : "N";
                    cf._ErrorPupupUse = (this.checkBoxErrorPupupUse.Checked) ? "Y" : "N";
                    cf._AutoPrintApp = (this.checkBoxAutoPrintApp.Checked) ? "Y" : "N";
                    cf._DeleteInJobFolder = (this.checkBoxDeleteInJobFolder.Checked) ? "Y" : "N";

                    cf._DownCheckFileName = this.textBox_DownCheckFileName.Text;
                    cf._DisableMultiPatient = (this.checkBoxDisableMultiPatient.Checked) ? "Y" : "N";

                    cf._FolderSizeCheckTime = this.textBoxFolderSizeCheckTime.Text;
                    cf._DvdMaxSize = (string.IsNullOrWhiteSpace(this.textBox_DvdMaxSize.Text)) ? "4831838208" : this.textBox_DvdMaxSize.Text;

                    cf._UseUSBCopy = (this.checkBoxUseUSBCopy.Checked) ? "Y" : "N";
                    cf._DisplayServeIP = (this.checkBoxDisplayServeIP.Checked) ? "Y" : "N";

                    cf._IsSizeCheck = (this.checkboxIsSizeCheck.Checked) ? "Y" : "N";

                    cf.setConfig("all");
                    cf.setServerType(this.ServerType);

                    getConfig();

                    DialogResult ask = MessageBox.Show("프로그램을 다시 실행해야 변경된 설정이 적용됩니다.", "Rimage Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (ask.ToString().ToLower().Equals("ok"))
                    {
                        Application.Exit();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 필수입력 사항 체크
        /// </summary>
        /// <returns></returns>
        private bool checkValue()
        {
            bool result = true;

            if (this.dsServerList.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("서버 정보를 한 개 이상 등록하세요.");
                this.xtraTabControl1.SelectedTabPage = this.xtraTabPageBaseInfo;
                textBox_HostIP.Focus();
                result = false;
            }
            
            if (textBox_DicomDownloadFolder.Text == "")
            {
                MessageBox.Show("다운로드 폴더경로를 입력하세요.");
                this.xtraTabControl1.SelectedTabPage = this.xtraTabPageSetFolder;
                textBox_DicomDownloadFolder.Focus();
                result = false;
            }

            if (textBox_LocalShareFolder.Text == "")
            {
                MessageBox.Show("공유폴더 로컬경로를 입력하세요.");
                this.xtraTabControl1.SelectedTabPage = this.xtraTabPageSetFolder;
                textBox_LocalShareFolder.Focus();
                result = false;
            }
            if (textBox_ServerNetworkDrive.Text == "")
            {
                MessageBox.Show("공유폴더 공유경로를 입력하세요.");
                this.xtraTabControl1.SelectedTabPage = this.xtraTabPageSetFolder;
                textBox_ServerNetworkDrive.Focus();
                result = false;
            }
            if (textBox_CDLabelFile.Text == "")
            {
                MessageBox.Show("CD Label 파일 경로를 입력하세요.");
                this.xtraTabControl1.SelectedTabPage = this.xtraTabPageSetFolder;
                textBox_CDLabelFile.Focus();
                result = false;
            }
            if (textBox_DVDLabelFile.Text == "")
            {
                MessageBox.Show("DVD Label 파일 경로를 입력하세요.");
                this.xtraTabControl1.SelectedTabPage = this.xtraTabPageSetFolder; 
                textBox_DVDLabelFile.Focus();
                result = false;
            }
            if (textBox_DVD_DLLabelFile.Text == "")
            {
                MessageBox.Show("DVD-DL Label 파일 경로를 입력하세요.");
                this.xtraTabControl1.SelectedTabPage = this.xtraTabPageSetFolder;
                textBox_DVD_DLLabelFile.Focus();
                result = false;
            }
            if (textBox_MergeFileServerFolder.Text == "")
            {
                MessageBox.Show("Merge 파일경로(서버)를 입력하세요.");
                this.xtraTabControl1.SelectedTabPage = this.xtraTabPageSetFolder;
                textBox_MergeFileServerFolder.Focus();
                result = false;
            }
            if (textBox_HospitalName.Text == "")
            {
                MessageBox.Show("병원명을 입력하세요.");
                this.xtraTabControl1.SelectedTabPage = this.xtraTabPageBaseInfo;
                textBox_HospitalName.Focus();
                result = false;
            }
            if (textBox_ServerFolder.Text == "")
            {
                MessageBox.Show("읽은데이터 저장폴더 경로를 입력하세요.");
                this.xtraTabControl1.SelectedTabPage = this.xtraTabPageSetFolder;
                textBox_ServerFolder.Focus();
                result = false;
            }
            if (textBox_ShareFolder.Text == "")
            {
                MessageBox.Show("공유폴더명을 입력하세요.");
                this.xtraTabControl1.SelectedTabPage = this.xtraTabPageSetFolder;
                textBox_ShareFolder.Focus();
                result = false;
            }

            return result;
        }

        private void textBox_HostIP_Leave(object sender, EventArgs e)
        {
            if (this.ServerType.Equals("R"))
            {
                this.textBox_CDLabelFolder.Text = "\\\\" + Path.Combine("ServerIP", "Rimage\\Labels") + "\\";
                this.textBox_DVDLabelFolder.Text = "\\\\" + Path.Combine("ServerIP", "Rimage\\Labels") + "\\";
                this.textBox_DVD_DLLabelFile.Text = "\\\\" + Path.Combine("ServerIP", "Rimage\\Labels") + "\\";
                this.textBox_MergeFileServerFolder.Text = "\\\\" + Path.Combine("ServerIP", "Rimage\\Merge") + "\\";
            }
        }

        private bool textBox_HostIP_PingCheck()
        {
            bool blResult = false;
            if (CheckPing.TestPing(textBox_HostIP.Text))
            {
                blResult = true;
            }
            else
            {
                //DialogResult dr = MessageBox.Show("입력한 IP는 네트워크 연결이 되지 않습니다.\r\n다시 확인 후 입력하세요.", "IP테스트", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //if (dr.ToString().ToLower().Equals("ok"))
                //{
                //    textBox_HostIP.Select();
                //    textBox_HostIP.Focus();
                //}
            }

            return blResult;
        }

        /// <summary>
        /// 병원 로고 파일 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_logfile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string imgFilePath = openFileDialog1.FileName;
                FileInfo fi = new FileInfo(imgFilePath);
                string imgName = fi.Name;

                string destFolder = this.GetImageFolderPath(imgName);

                //// 실행폴더\Image 폴더로 복사한다.
                if (!string.IsNullOrWhiteSpace(FileControl.CopyFile(imgFilePath, destFolder)))
                {
                    this.pictureLogoFile.Image = Image.FromFile(destFolder);
                    this.pictureLogoFile.Tag = imgName;
                }
                else
                {
                    MessageBox.Show("이미지 등록에 실패하였습니다.");
                }
            }
        }

        /// <summary>
        /// 로그 파일 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_logfileDelete_Click(object sender, EventArgs e)
        {
            this.pictureLogoFile.Image.Dispose();
            this.pictureLogoFile.Image = null;

            string filePath = Utils.CheckNull(this.pictureLogoFile.Tag);
            if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(this.GetImageFolderPath(filePath)))
            {
                try
                {
                    File.Delete(this.GetImageFolderPath(filePath));
                }
                catch { }
            }

            this.pictureLogoFile.Tag = null;
        }

        /// <summary>
        /// 이미지 폴더 경로 반환
        /// </summary>
        /// <returns></returns>
        private string GetImageFolderPath(string fileName)
        {
            string destFolder = Path.Combine(Application.StartupPath, GlobalVar.IMG_FOLDER);

            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }

            return Path.Combine(destFolder, fileName);
        }

        /// <summary>
        /// 다운로드 폴더 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_folderBrowser1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox_DicomDownloadFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        /// <summary>
        /// 공유폴더 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_folderBrowser2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string fldPath = folderBrowserDialog1.SelectedPath;
                this.textBox_LocalShareFolder.Text = fldPath;

                if (this.ServerType.Equals("R"))
                {
                    int iIndex = fldPath.LastIndexOf("\\");
                    string strTmp = "";

                    if (iIndex > 0)
                    {
                        strTmp = fldPath.Substring(iIndex + 1, fldPath.Length - iIndex - 1);
                    }

                    if (strTmp == "" && fldPath.Length > 0)
                    {
                        strTmp = fldPath.Substring(0, 1);
                    }

                    this.textBox_ServerNetworkDrive.Text = "\\\\" + myIP + "\\" + strTmp + "\\";
                }
                else
                {
                    this.textBox_ServerNetworkDrive.Text = this.textBox_LocalShareFolder.Text;
                }
            }
        }

        private void ConfigSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dsServerList.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("서버 정보를 한 개 이상 등록해야 합니다.");
                e.Cancel = true;
            }
            else
            {
                if (this.comeWhere == "1")
                {
                    MessageBox.Show("\"서버연결\" 버튼을 클릭하여 시스템에 다시 연결하세요.", "Rimage Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void textBox_HostIP_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.ServerType.Equals("R"))
            {
                this.textBox_CDLabelFolder.Text = "\\\\" + Path.Combine("ServerIP", "Rimage\\Labels") + "\\";
                this.textBox_DVDLabelFolder.Text = "\\\\" + Path.Combine("ServerIP", "Rimage\\Labels") + "\\";
                this.textBox_DVD_DLLabelFolder.Text = "\\\\" + Path.Combine("ServerIP", "Rimage\\Labels") + "\\";
                this.textBox_MergeFileServerFolder.Text = "\\\\" + Path.Combine("ServerIP", "Rimage\\Merge") + "\\";
            }

            //this.textBox_CDLabelFolder.Text = "\\\\" + Path.Combine(this.textBox_HostIP.Text, "Rimage\\Labels") + "\\";
            //this.textBox_DVDLabelFolder.Text = "\\\\" + Path.Combine(this.textBox_HostIP.Text, "Rimage\\Labels") + "\\";
            //this.textBox_MergeFileServerFolder.Text = "\\\\" + Path.Combine(this.textBox_HostIP.Text, "Rimage\\Merge") + "\\";
        }

        private void dataGridViewServerList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewServerList.SelectedRows.Count == 1)
            {
                string No = this.dataGridViewServerList.SelectedRows[0].Cells["ColumnNo"].Value.ToString();
                string IP = this.dataGridViewServerList.SelectedRows[0].Cells["ColumnServerIP"].Value.ToString();
                string Name = this.dataGridViewServerList.SelectedRows[0].Cells["ColumnServerName"].Value.ToString();
                string Port = this.dataGridViewServerList.SelectedRows[0].Cells["ColumnServerPort"].Value.ToString();

                this.textBox_HostIP.Text = IP;
                this.textBox_HostName.Text = Name;
                this.textBox_HostPort.Text = Port;
                this.textBox_HostIP.Tag = No;
            }
        }

        /// <summary>
        /// 서버 추가하기
        /// </summary>
        private void AddServer()
        {
            bool CheckData = false;

            foreach (DataRow dr in this.dsServerList.Tables[0].Rows)
            {
                if (dr["Name"].ToString() == this.textBox_HostName.Text.Trim() &&
                    dr["IP"].ToString() == this.textBox_HostIP.Text.Trim() &&
                    dr["Port"].ToString() == this.textBox_HostPort.Text.Trim())
                {
                    CheckData = true;
                    break;
                }
            }

            if (CheckData == true)
            {
                DialogResult dr = MessageBox.Show("동일한 정보의 서버가 이미 등록되어 있습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (this.dsServerList.Tables[0].Rows.Count >= 7)
                {
                    MessageBox.Show("서버는 최대 7개까지만 등록 가능합니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataRow nRow = this.dsServerList.Tables[0].NewRow();
                nRow["No"] = this.MaxNumber() + 1;
                nRow["IP"] = this.textBox_HostIP.Text.Trim();
                nRow["Name"] = this.textBox_HostName.Text.Trim();
                nRow["Port"] = this.textBox_HostPort.Text.Trim();
                this.dsServerList.Tables[0].Rows.Add(nRow);

                this.dataGridViewServerList.DataSource = this.dsServerList.Tables[0];
                this.SaveServerListFile();
                //this.getServerList();
            }
        }

        /// <summary>
        /// 서버목록 저장
        /// </summary>
        private void SaveServerListFile()
        {
            dsServerList.Tables[0].WriteXml(this.ServerListFile);
        }

        private int MaxNumber()
        {
            int i = 0;

            foreach (DataRow dr in this.dsServerList.Tables[0].Rows)
            {
                if (Convert.ToInt32(dr["No"]) > i)
                {
                    i = Convert.ToInt32(dr["No"]);
                }
            }

            return i;
        }

        /// <summary>
        /// 원격서버 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_RemoteServer_Click(object sender, EventArgs e)
        {
            DialogResult ret = MessageBox.Show("원격서버로 설정합니다.", "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (ret.ToString().ToUpper().Equals("OK"))
            {
                this.SaveServerType("R");
                this.SetServerType();
                this.SetControlByServerType();

                this.textBox_HostName.Text = this.HostIP;
                this.textBox_HostName.Text = this.HostName;
            }
        }

        /// <summary>
        /// 로컬서버 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_LocalServer_Click(object sender, EventArgs e)
        {
            DialogResult ret = MessageBox.Show("로컬서버로 설정합니다.", "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            this.SaveServerType("L");
            this.SetServerType();
            this.SetControlByServerType();

            this.textBox_HostIP.Text = GetMyIP.MyIP();
            this.textBox_HostName.Text = GetMyIP.MyHostName();
        }

        /// <summary>
        /// 서버유형에 따른 버튼 글자색깔 변경
        /// </summary>
        private void SetServerType()
        {
            if (this.ServerType.Equals("L"))
            {
                this.button_RemoteServer.ForeColor = Color.Gray;
                this.button_LocalServer.ForeColor = Color.Red;

                this.textBox_CDLabelFolder.Text = this.CDLabelFolder;
                this.textBox_DVDLabelFolder.Text = this.DVDLabelFolder;
                this.textBox_DVD_DLLabelFolder.Text = this.DVDDLLabelFolder;
                this.textBox_MergeFileServerFolder.Text = this.MergeFileFolder;
            }
            else
            {
                this.button_RemoteServer.ForeColor = Color.Red;
                this.button_LocalServer.ForeColor = Color.Gray;

                this.textBox_CDLabelFolder.Text = "\\\\ServerIP\\Rimage\\Labels\\";
                this.textBox_DVDLabelFolder.Text = "\\\\ServerIP\\Rimage\\Labels\\";
                this.textBox_DVD_DLLabelFolder.Text = "\\\\ServerIP\\Rimage\\Labels\\";
                this.textBox_MergeFileServerFolder.Text = "\\\\ServerIP\\rimage\\Merge\\";
            }
        }

        /// <summary>
        /// 서버유형 저장
        /// </summary>
        /// <param name="srvType"></param>
        private void SaveServerType(string srvType)
        {
            this.ServerType = srvType;
            Config cf = new Config(Application.StartupPath);
            cf.setServerType(this.ServerType);
        }

        private void SetControlByServerType()
        {
            if (this.ServerType.Equals("L"))
            {
                this.buttonCDLabelSearch.Enabled = true;
                this.buttonDVDLabelSearch.Enabled = true;
                this.buttonDVDDLLabelSearch.Enabled = true;
                this.buttonMergeFolderSearch.Enabled = true;

                this.buttonCDLabelSearch.Visible = true;
                this.buttonDVDLabelSearch.Visible = true;
                this.buttonDVDDLLabelSearch.Visible = true;
                this.buttonMergeFolderSearch.Visible = true;
            }
            else
            {
                this.buttonCDLabelSearch.Enabled = false;
                this.buttonDVDLabelSearch.Enabled = false;
                this.buttonDVDDLLabelSearch.Enabled = false;
                this.buttonMergeFolderSearch.Enabled = false;

                this.buttonCDLabelSearch.Visible = false;
                this.buttonDVDLabelSearch.Visible = false;
                this.buttonDVDDLLabelSearch.Visible = false;
                this.buttonMergeFolderSearch.Visible = false;
            }
        }

        /// <summary>
        /// CD Label 파일 폴더 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCDLabelSearch_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string cdLabel = this.openFileDialog2.FileName;
                FileInfo fi = new FileInfo(cdLabel);

                this.textBox_CDLabelFolder.Text = fi.DirectoryName + "\\";
                this.textBox_CDLabelFile.Text = fi.Name;
            }
        }

        /// <summary>
        /// DVD Label 파일 폴더 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDVDLabelSearch_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string dvdLabel = this.openFileDialog2.FileName;
                FileInfo fi = new FileInfo(dvdLabel);

                this.textBox_DVDLabelFolder.Text = fi.DirectoryName + "\\";
                this.textBox_DVDLabelFile.Text = fi.Name;
            }
        }

        /// <summary>
        /// 머지 폴더 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonMergeFolderSearch_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox_MergeFileServerFolder.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnAutoExeType_Click(object sender, EventArgs e)
        {
            frmHelp frm = new frmHelp();
            frm.ShowDialog();
        }

        private void button_RegCheckFiles_Click(object sender, EventArgs e)
        {
            frmCheckFiles frm = new frmCheckFiles();
            DialogResult dr = frm.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                if (frm.FileExist == true)
                {
                    this.label_CheckFiles.Text = "(체크할 파일 있음)";
                    this.label_CheckFiles.ForeColor = Color.Red;
                }
                else
                {
                    this.label_CheckFiles.Text = "(체크할 파일 없음)";
                    this.label_CheckFiles.ForeColor = Color.Blue;
                }
            }
        }



        /// <summary>
        /// 숫자만 입력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != 8 && e.KeyChar != 45 && e.KeyChar != 46)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 서버 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonAdd_Click_1(object sender, EventArgs e)
        {
            if (textBox_HostIP.Text.Length < 8)
            {
                MessageBox.Show("서버 IP를 입력하세요.");
                textBox_HostIP.Focus();
                return;
            }
            if (textBox_HostName.Text == "")
            {
                MessageBox.Show("서버명을 입력하세요.");
                textBox_HostName.Focus();
                return;
            }
            if (textBox_HostPort.Text == "")
            {
                MessageBox.Show("접속포트를 입력하세요.");
                textBox_HostPort.Focus();
                return;
            }

            this.AddServer();
        }

        /// <summary>
        /// 서버 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewServerList.SelectedRows.Count == 1)
            {
                string No = this.dataGridViewServerList.SelectedRows[0].Cells["ColumnNo"].Value.ToString();
                string IP = this.dataGridViewServerList.SelectedRows[0].Cells["ColumnServerIP"].Value.ToString();
                string Name = this.dataGridViewServerList.SelectedRows[0].Cells["ColumnServerName"].Value.ToString();
                string Port = this.dataGridViewServerList.SelectedRows[0].Cells["ColumnServerPort"].Value.ToString();

                //DataRow delDr = new DataRow();
                foreach (DataRow dr in this.dsServerList.Tables[0].Rows)
                {
                    if (dr["No"].ToString() == No && dr["Name"].ToString() == Name &&
                        dr["IP"].ToString() == IP && dr["Port"].ToString() == Port)
                    {
                        this.dsServerList.Tables[0].Rows.Remove(dr);
                        break;
                    }
                }

                this.dataGridViewServerList.DataSource = this.dsServerList.Tables[0];
                this.SaveServerListFile();
                //this.getServerList();

                if (this.dataGridViewServerList.Rows.Count > 0)
                {
                    this.dataGridViewServerList.Rows[0].Selected = true;

                    this.textBox_HostIP.Text = this.dataGridViewServerList.SelectedRows[0].Cells["ColumnServerIP"].Value.ToString();
                    this.textBox_HostName.Text = this.dataGridViewServerList.SelectedRows[0].Cells["ColumnServerName"].Value.ToString();
                    this.textBox_HostPort.Text = this.dataGridViewServerList.SelectedRows[0].Cells["ColumnServerPort"].Value.ToString();
                    this.textBox_HostIP.Tag = this.dataGridViewServerList.SelectedRows[0].Cells["ColumnNo"].Value.ToString();
                }
            }
        }

        /// <summary>
        /// 복사 신청서 하단 이미지 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAppBottomImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string imgFilePath = openFileDialog1.FileName;
                FileInfo fi = new FileInfo(imgFilePath);
                string imgName = fi.Name;

                string destFolder = this.GetImageFolderPath(imgName);

                //// 실행폴더\Image 폴더로 복사한다.
                if (!string.IsNullOrWhiteSpace(FileControl.CopyFile(imgFilePath, destFolder)))
                {
                    this.pictureAppBottomImage.Image = Image.FromFile(destFolder);
                    this.pictureAppBottomImage.Tag = imgName;
                }
                else
                {
                    MessageBox.Show("이미지 등록에 실패하였습니다.");
                }
            }
        }

        /// <summary>
        /// 복사 신청서 하단 이미지 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAppBottomImageDelete_Click(object sender, EventArgs e)
        {
            this.pictureAppBottomImage.Image.Dispose();
            this.pictureAppBottomImage.Image = null;

            string filePath = Utils.CheckNull(this.pictureAppBottomImage.Tag);
            if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(this.GetImageFolderPath(filePath)))
            {
                try
                {
                    File.Delete(this.GetImageFolderPath(filePath));
                }
                catch { }
            }

            this.pictureAppBottomImage.Tag = null;
        }

        /// <summary>
        /// DVD-DL 파일 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDVDDLLabelSearch_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string dvdLabel = this.openFileDialog2.FileName;
                FileInfo fi = new FileInfo(dvdLabel);

                this.textBox_DVD_DLLabelFolder.Text = fi.DirectoryName + "\\";
                this.textBox_DVD_DLLabelFile.Text = fi.Name;
            }
        }
    }
}
