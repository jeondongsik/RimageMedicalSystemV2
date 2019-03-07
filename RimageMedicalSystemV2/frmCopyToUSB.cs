using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using RimageKorea;
using System.Management;
using System.Runtime.InteropServices;

namespace RimageMedicalSystemV2
{
    /// <summary>
    /// 환자정보를 USB로 복사한다.
    /// </summary>
    public partial class frmCopyToUSB : DevExpress.XtraEditors.XtraForm
    {
        BurnOrderedInfoEntity _orderInfo;
        /// <summary>
        /// 환자 정보
        /// </summary>
        public BurnOrderedInfoEntity OrderInfo
        {
            get { return this._orderInfo; }
            set { this._orderInfo = value; }
        }

        MainForm _mainForm;
        /// <summary>
        /// 부모폼
        /// </summary>
        public MainForm MyOwnerForm
        {
            get { return this._mainForm; }
            set { this._mainForm = value; }
        }

        List<DriveInfo> usbList;
        List<DriveInfo> others;
        DriveInfo seletedUSB;
        long copyFileLen = 0;
        bool isCopying = false;

        public frmCopyToUSB()
        {
            InitializeComponent();
        }

        private void frmCopyToUSB_Load(object sender, EventArgs e)
        {
            GlobalVar.isCopyingToUSB = true;
            this.timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;

            this.FindUsb();
        }

        /// <summary>
        /// USB 드라이버 찾기
        /// </summary>
        public void FindUsb()
        {
            try
            {
                this.usbList = new List<DriveInfo>();
                this.others = new List<DriveInfo>();

                ////foreach (var di in DriveInfo.GetDrives())
                ////{
                ////    if (di.IsReady && di.DriveType == DriveType.Removable)
                ////    {
                ////        this.usbList.Add(di);
                ////    }
                ////    else
                ////    {
                ////        this.others.Add(di);
                ////    }
                ////}

                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive WHERE InterfaceType='USB'");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    foreach (ManagementObject b in queryObj.GetRelated("Win32_DiskPartition"))
                    {
                        foreach (ManagementBaseObject c in b.GetRelated("Win32_LogicalDisk"))
                        {
                            ////Console.WriteLine(String.Format("{0}" + "\\", c["Name"].ToString())); // here it will print USB drive letter
                            this.txtProgressView.AppendText(String.Format("USB Drive => {0}\\ 준비되었습니다.\r\n", c["Name"].ToString()));

                            DriveInfo driveInfo = new DriveInfo(c["Name"].ToString());
                            this.usbList.Add(driveInfo);
                        }
                    }

                }

                if (this.usbList.Count > 0)
                {
                    int i = 0;
                    foreach (var di in this.usbList)
                    {
                        this.flpDrives.Controls.Add(this.CreateUsbButton(i, di));
                        i++;
                    }

                    if (i == 1)
                    {
                        //// 바로 복사 시작
                        this.seletedUSB = this.usbList[0];
                        this.CopyStart();
                    }
                }
                else
                {
                    this.txtProgressView.AppendText("인식할 수 있는 USB 드라이브가 없습니다.\r\nUSB를 컴퓨터에 연결하세요.\r\n");
                }

                ////if (this.others.Count > 0)
                ////{
                ////    int j = 11;
                ////    foreach (var di in this.others)
                ////    {
                ////        this.flpDrives.Controls.Add(this.CreateDriveButton(j, di));
                ////        j++;
                ////    }
                ////}
            }
            catch { }
        }

        /// <summary>
        /// USB 드라이버 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUSBDrive_Click(object sender, EventArgs e)
        {
            try
            {
                this.seletedUSB = ((sender as SimpleButton).Tag) as DriveInfo;
            }
            catch { }
        }

        /// <summary>
        /// USB 버튼 생성한다.
        /// </summary>
        /// <returns></returns>
        private SimpleButton CreateUsbButton(int idx, DriveInfo di)
        {
            SimpleButton btnNew = new SimpleButton();

            btnNew.Appearance.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnNew.Appearance.Options.UseFont = true;
            btnNew.AppearancePressed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            btnNew.AppearancePressed.Options.UseBorderColor = true;
            btnNew.ImageOptions.Image = global::RimageMedicalSystemV2.Properties.Resources.icons8_usb_2_32;
            btnNew.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            btnNew.Location = new System.Drawing.Point(3, 3);
            btnNew.Name = string.Format("btnUSBDrive{0}", idx.ToString());
            btnNew.Size = new System.Drawing.Size(106, 81);
            btnNew.TabIndex = idx;
            btnNew.Text = di.Name;
            btnNew.Tag = di;
            btnNew.Click += new System.EventHandler(this.btnUSBDrive_Click);

            return btnNew;
        }

        /// <summary>
        /// USB 이외의 드라이브 버튼 생성
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="di"></param>
        /// <returns></returns>
        private SimpleButton CreateDriveButton(int idx, DriveInfo di)
        {
            SimpleButton btnNew = new SimpleButton();

            btnNew.Appearance.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnNew.Appearance.Options.UseFont = true;
            btnNew.ImageOptions.Image = global::RimageMedicalSystemV2.Properties.Resources.icons8_c_drive_32;
            btnNew.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            btnNew.Location = new System.Drawing.Point(3, 90);
            btnNew.Name = "btnDrive";
            btnNew.Size = new System.Drawing.Size(106, 80);
            btnNew.TabIndex = idx;
            btnNew.Text = di.Name;
            btnNew.Tag = di;
            btnNew.Click += new System.EventHandler(this.btnDrive_Click);

            return btnNew;
        }

        /// <summary>
        /// 고정형 하드 드라이브 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDrive_Click(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 복사 시작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartCopy_Click(object sender, EventArgs e)
        {
            try
            {
                this.CopyStart();
            }
            catch { }
        }

        /// <summary>
        /// 드라이브 공간 체크
        /// </summary>
        /// <returns></returns>
        private bool CheckAvailableSpace()
        {
            try
            {
                //// 환자정보가 USB공간보다 클 경우
                if (this._orderInfo.FolderSize > this.seletedUSB.AvailableFreeSpace)
                    return false;
            }
            catch { }

            return true;
        }

        /// <summary>
        /// USB 에 다른 데이터가 존재하는지 체크한다.
        /// </summary>
        /// <returns></returns>
        private bool ExistsUSBData()
        {
            try
            {
                if (this.seletedUSB.RootDirectory.GetFiles().Length > 0 ||
                    this.seletedUSB.RootDirectory.GetDirectories().Length > 2)
                {
                    return true;
                }
            }
            catch { }

            return false;
        }

        /// <summary>
        /// USB 포멧하기
        /// </summary>
        /// <param name="driveLetter"></param>
        /// <param name="fileSystem">FAT32 or NTFS</param>
        /// <param name="quickFormat"></param>
        /// <param name="clusterSize"></param>
        /// <param name="label"></param>
        /// <param name="enableCompression"></param>
        /// <returns></returns>
        public bool FormatUSB(string driveLetter, string fileSystem = "NTFS", bool quickFormat = true,
                                   int clusterSize = 4096, string label = "RIMAGE", bool enableCompression = false)
        {
            //add logic to format Usb drive
            //verify conditions for the letter format: driveLetter[0] must be letter. driveLetter[1] must be ":" and all the characters mustn't be more than 2
            if (driveLetter.Length != 2 || driveLetter[1] != ':' || !char.IsLetter(driveLetter[0]))
                return false;

            //query and format given drive 
            //best option is to use ManagementObjectSearcher

            var files = Directory.GetFiles(driveLetter);
            var directories = Directory.GetDirectories(driveLetter);

            foreach (var item in files)
            {
                try
                {
                    File.Delete(item);
                }
                catch (UnauthorizedAccessException) { }
                catch (IOException) { }
            }

            foreach (var item in directories)
            {
                try
                {
                    Directory.Delete(item);
                }
                catch (UnauthorizedAccessException) { }
                catch (IOException) { }
            }

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"select * from Win32_Volume WHERE DriveLetter = '" + driveLetter + "'");
            foreach (ManagementObject vi in searcher.Get())
            {
                try
                {
                    var completed = false;
                    var watcher = new ManagementOperationObserver();

                    watcher.Completed += (sender, args) =>
                    {
                        ////Console.WriteLine("USB format completed " + args.Status);
                        this.txtProgressView.AppendText(string.Format("USB format completed {0}", args.Status));
                        completed = true;
                    };
                    watcher.Progress += (sender, args) =>
                    {
                        ////Console.WriteLine("USB format in progress " + args.Current);
                        this.txtProgressView.AppendText(string.Format("USB format in progress {0}", args.Current));
                    };

                    vi.InvokeMethod(watcher, "Format", new object[] { fileSystem, quickFormat, clusterSize, label, enableCompression });

                    while (!completed) { System.Threading.Thread.Sleep(1000); }
                }
                catch
                {
                }
            }
            return true;
        }

        /// <summary>
        /// 복사 시작
        /// </summary>
        private void CopyStart()
        {
            if (this.seletedUSB == null)
            {
                return;
            }

            if (!this.seletedUSB.IsReady)
            {
                //// 드라이버가 준비되지 않았을 경우
                MessageBox.Show("준비된 USB 드라이버가 없습니다.\r\n컴퓨터에 제대로 연결되었는지 체크해주세요.", "Rimage Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.ExistsUSBData())
            {
                //// 다른데이터가 존재할 경우
                MessageBox.Show("USB 드라이버에 다른 데이터가 존재합니다.\r\n체크 후 다시 시도해주세요.", "Rimage Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!this.CheckAvailableSpace())
            {
                //// 공간이 안될 경우
                MessageBox.Show("USB 공간이 복사하려는 환자정보의 크기보다 작습니다.\r\n체크 후 다시 시도해주세요.", "Rimage Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DirectoryInfo dirInfo = new DirectoryInfo(this.OrderInfo.patFolderFullPath);

            try
            {
                if (dirInfo.Exists)
                {
                    this.txtProgressView.AppendText(String.Format("{0}\\ 로 복사를 시작합니다.\r\n", this.seletedUSB.Name));
                    this.progressBarControl1.Properties.Maximum = this._orderInfo.ImgFiles.EditList.Count;

                    this.Cursor = Cursors.WaitCursor;

                    if (this.backgroundWorker1.IsBusy == false)
                    {
                        CopyWorker cls = new CopyWorker();
                        cls.srcDir = dirInfo;
                        cls.targetDrive = this.seletedUSB;

                        this.isCopying = true;
                        this._mainForm.ClearSendUSBPatInfo(this._orderInfo.patFolderFullPath);
                        this._mainForm.AddBurningList(this._orderInfo);

                        this.backgroundWorker1.RunWorkerAsync(cls);
                    }
                    else
                    {
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("파일복사가 진행되고 있습니다. 잠시 후 다시 시도하세요.", "Rimage Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("환자폴더가 존재하지 않습니다.\r\n확인 후 다시 시도해주세요.", "Rimage Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                this.isCopying = false;
                MessageBox.Show("굽기 시작 중 에러가 발생했습니다.\r\n" + ex.Message, "Rimage Message : burnCDAfterCopyFiles", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 복사 진행 쓰레드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                CopyWorker cls = e.Argument as CopyWorker;
                e.Result = 0;

                //// 시작시 로그 저장
                WebUtils.InsertResult(this._orderInfo.OrderId,
                          this._orderInfo.StartDateTime,
                          "",
                          this._orderInfo.patNo,
                          this._orderInfo.patName,
                          this._orderInfo.copies.ToString(),
                          this._orderInfo.mediType,
                          this._orderInfo.mediSize,
                          "?",
                          ((this._orderInfo.BurnPatientKind.Equals("Y") || this._orderInfo.patList.Count > 1) ? this._orderInfo.DicomDescription : this._orderInfo.StudyModality),
                          Utils.CheckNull(this._orderInfo.BurnPatientKind, "N"),
                          this._orderInfo.TargetServer.IP,
                          NetInfo.MyIP());

                cls.ExecJob(worker, e);
            }
            catch (Exception ex)
            {
                this.isCopying = false;
                this.txtProgressView.Text = ex.ToString();
                ErrorLog.LogWrite(this, ex.ToString(), Application.StartupPath);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                CurrentState state = e.UserState as CurrentState;
                this.progressBarControl1.EditValue = state.count;
                this.txtProgressView.AppendText(state.retMessage);
            }
            catch { }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.txtProgressView.AppendText("Error: " + e.Error.Message + "\r\n");
            }
            else if (e.Cancelled)
            {
                this.txtProgressView.AppendText("Copying files canceled.\r\n");
            }
            else
            {
                this.txtProgressView.AppendText("Finished copying files.\r\n");
            }

            CurrentState state = e.Result as CurrentState;
            this.copyFileLen = state.fileSize;
            this.backgroundWorker1.CancelAsync();
            this.isCopying = false;

            this.Complet();
        }

        /// <summary>
        /// 복사 종료
        /// </summary>
        private void Complet()
        {
            try
            {
                //// 복사가 제대로 되었는지 체크

                //// 완료 파일 생성한다.
                if (!File.Exists(Path.Combine(this._orderInfo.patFolderFullPath, GlobalVar.BURN_CHK_FL_NM)))
                {
                    FileControl.CreateTextFile(Path.Combine(this._orderInfo.patFolderFullPath, GlobalVar.BURN_CHK_FL_NM));
                }

                //// 결과 저장
                WebUtils.InsertResult(this._orderInfo.OrderId,
                          this._orderInfo.StartDateTime,
                          Utils.GetNowTime(),
                          this._orderInfo.patNo,
                          this._orderInfo.patName,
                          this._orderInfo.copies.ToString(),
                          this._orderInfo.mediType,
                          this._orderInfo.mediSize,
                          "완료",
                          ((this._orderInfo.BurnPatientKind.Equals("Y") || this._orderInfo.patList.Count > 1) ? this._orderInfo.DicomDescription : this._orderInfo.StudyModality),
                          Utils.CheckNull(this._orderInfo.BurnPatientKind, "N"),
                          this._orderInfo.TargetServer.IP,
                          NetInfo.MyIP());

                //// 종료 : 창을 닫고 메인화면에 종료 메시지 전송
                GlobalVar.isCopyingToUSB = false;

                this._orderInfo.Finish = "Y";

                string message = string.Format("[{0} - {1}] USB로 복사되었습니다.\r\n", this._orderInfo.patNo, this._orderInfo.patName);

                DiscStatusForDisplay trace = new DiscStatusForDisplay();
                trace.OrderID = this._orderInfo.OrderId;
                trace.ServerIP = this._orderInfo.TargetServer.IP;
                trace.Status = "FINISHED";
                trace.StatusType = "Coping";
                trace.StateString = "COPY TO USB Complete";
                trace.PercentCompleted = "100";
                trace.ResultMessage = "완료";
                trace.Finish = "Y";
                trace.ResultCode = "2";

                this._mainForm.copyToUSBComplete(message, trace);

                this.Close();
            }
            catch { }
        }

        /// <summary>
        /// 폼이 닫히기 전 복사진행중이라면 창을 못 닫게.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCopyToUSB_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.isCopying || this.backgroundWorker1.IsBusy)
            {
                if (MessageBox.Show("환자정보 파일이 복사중입니다.\r\n그래도 종료할까요?", "Rimage Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else
                {
                    this.backgroundWorker1.CancelAsync();
                }
            }

            GlobalVar.isCopyingToUSB = false;
        }

        protected override void WndProc(ref Message m)
        {
            UInt32 WM_DEVICECHANGE = 0x0219;
            UInt32 DBT_DEVTUP_VOLUME = 0x02;
            UInt32 DBT_DEVICEARRIVAL = 0x8000;
            ////UInt32 DBT_DEVICEREMOVECOMPLETE = 0x8004;

            if ((m.Msg == WM_DEVICECHANGE) && (m.WParam.ToInt32() == DBT_DEVICEARRIVAL))//디바이스 연결
            {
                //int m_Count = 0;
                int devType = Marshal.ReadInt32(m.LParam, 4);

                if (devType == DBT_DEVTUP_VOLUME)
                {
                    this.timer1.Enabled = true;
                }
            }

            ////if ((m.Msg == WM_DEVICECHANGE) && (m.WParam.ToInt32() == DBT_DEVICEREMOVECOMPLETE))  //디바이스 연결 해제
            ////{
            ////    int devType = Marshal.ReadInt32(m.LParam, 4);

            ////    if (devType == DBT_DEVTUP_VOLUME)
            ////    {
            ////        this.txtProgressView.AppendText("디바이스 연결 해제");
            ////    }
            ////}

            base.WndProc(ref m);
        }

        private void btnReFind_Click(object sender, EventArgs e)
        {
            this.FindUsb();
        }
    }

    /// <summary>
    /// 파일복사 실행 Class
    /// </summary>
    public class CopyWorker
    {
        public DirectoryInfo srcDir { get; set; }
        public DriveInfo targetDrive { get; set; }
        string parentFolder;

        /// <summary>
        /// 환자정보 파일 복사하기
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        public void ExecJob(System.ComponentModel.BackgroundWorker worker, System.ComponentModel.DoWorkEventArgs e)
        {
            CurrentState state = new CurrentState();
            state.fileSize = 0;
            state.retCode = 0;
            state.count = 0;
            state.retMessage = "파일복사 시작 >>> \r\n";
            worker.ReportProgress(1, state);

            if (worker.CancellationPending)
            {
                state.retCode = 0;
                state.retMessage = "파일복사가 작업자에 의해 취소되었습니다.\r\n";
                worker.ReportProgress(100, state);
                e.Cancel = true;
            }
            else
            {
                try
                {
                    if (srcDir.Exists)
                    {
                        this.parentFolder = string.Format("{0}\\", srcDir.FullName);

                        //1.폴더생성
                        this.CreatdFolder(srcDir);

                        //2.파일복사
                        this.moveFiles(srcDir, worker, e, state);

                        state.retMessage = "Copying files done.\r\n";
                        worker.ReportProgress(100, state);
                    }
                    else
                    {
                        state.retCode = -1;
                        state.retMessage = "파일복사 중 에러발생.\r\n";
                        worker.ReportProgress(100, state);
                    }
                }
                catch (Exception ex)
                {
                    state.retCode = -1;
                    state.retMessage = "파일복사 중 에러발생.\r\n";
                    worker.ReportProgress(100, state);
                }

                e.Result = state;
            }
        }

        /// <summary>
        /// 복사할 대상 디렉토리 생성
        /// </summary>
        /// <param name="directory"></param>
        public void CreatdFolder(DirectoryInfo directory)
        {
            foreach (DirectoryInfo d in directory.GetDirectories())
            {
                string srcDir = d.FullName.Replace(this.parentFolder, "");
                string targetDir = Path.Combine(this.targetDrive.RootDirectory.Name, srcDir);
                DirectoryInfo tdir = new DirectoryInfo(targetDir);

                if (!tdir.Exists)
                    tdir.Create();

                CreatdFolder(d);
            }
        }

        /// <summary>
        /// 파일이동
        /// </summary>
        /// <param name="directory"></param>
        private void moveFiles(DirectoryInfo directory, System.ComponentModel.BackgroundWorker worker, System.ComponentModel.DoWorkEventArgs e, CurrentState state)
        {
            try
            {
                FileInfo[] files = directory.GetFiles();
                DirectoryInfo[] dirs = directory.GetDirectories();

                foreach (FileInfo file in files)
                {
                    state.fileSize += file.Length;
                    //// 원본 파일의 드라이명을 USB 드라이버로 변경한다.
                    string copyto = file.FullName.Replace(this.parentFolder, this.targetDrive.RootDirectory.Name);
                    file.CopyTo(copyto);

                    state.count++;
                    state.retMessage = string.Format("{0} {1} byte copied.\r\n", file.Name, state.fileSize.ToString());
                    worker.ReportProgress(20, state);
                }

                foreach (DirectoryInfo dri in dirs)
                {
                    moveFiles(dri, worker, e, state);
                }
            }
            catch { }
        }
    }
}