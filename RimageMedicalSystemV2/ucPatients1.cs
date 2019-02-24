using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using RimageKorea;
using DicomReader;
using System.IO;

namespace RimageMedicalSystemV2
{
    /// <summary>
    /// 굽기 타입 1
    /// </summary>
    public partial class ucPatients1 : DevExpress.XtraEditors.XtraUserControl
    {        
        BurnOrderedInfoEntity _orderInfo;
        /// <summary>
        /// 굽기 명령 정보
        /// </summary>
        public BurnOrderedInfoEntity OrderInfo
        {
            get { return this._orderInfo; }
            set { this._orderInfo = value; }
        }

        public ucPatients1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 컨트럴 초기화
        /// </summary>
        public void Clear()
        {
            this.txtPatientID.EditValue = "";
            this.txtPatientName.EditValue = "";
            this.txtDataLength.EditValue = "";
            this.cbCopies.SelectedIndex = 0;
            this.cbMedia.SelectedIndex = 0;
            this.treeView1.Nodes.Clear();

            //// this._orderInfo = null;
        }

        /// <summary>
        /// 환자 정보 보여주기
        /// </summary>
        public void ShowPatientInfo()
        {
            try
            {
                if (this._orderInfo == null)
                    return;
                if (string.IsNullOrWhiteSpace(this._orderInfo.patNo))
                    return;

                this.txtPatientID.Text = this._orderInfo.patNo;
                this.txtPatientName.Text = this._orderInfo.patName;

                this.txtDataLength.Text = this._orderInfo.mediSize;
                this.txtDataLength.Tag = this._orderInfo.FolderSize;

                if (this._orderInfo.mediType == "DVDR-DL")
                {
                    this.cbMedia.SelectedIndex = 2;     //DVDR-DL
                }
                else if (this._orderInfo.mediType == "DVDR")
                {
                    this.cbMedia.SelectedIndex = 1;     //DVDR
                }
                else
                {
                    this.cbMedia.SelectedIndex = 0;
                }

                //// Dicom 이미지 정보를 화면에 보여준다.
                if (this._orderInfo.DicomImgList != null)
                {
                    this.DisplayPatientInfo(this._orderInfo.DicomImgList);
                }
                else
                {
                    this.GetPatientDicomImages(this._orderInfo.PatInfor);
                }
            }
            catch { }
        }

        /// <summary>
        /// 이미지들의 목록을 보여준다.
        /// </summary>
        /// <param name="lstFiles"></param>
        private void DisplayPatientInfo(List<string> lstFiles)
        {
            DicomInfomation di = new DicomInfomation();
            FileInfo fi = null;

            try
            {
                DataTable dtImages = new DataTable();
                DataTable dt = new DataTable();

                if (lstFiles != null || lstFiles.Count > 0)
                {
                    int n = 0;
                    foreach (string dcm in lstFiles)
                    {
                        n++;
                        di._DicomFilePath = dcm;
                        fi = new FileInfo(dcm);
                        di._DicomFileName = fi.Name;

                        dt = di.PatientDicomImageInfo(n);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (dtImages.Rows.Count == 0)
                            {
                                dtImages = dt.Clone();
                            }

                            foreach (DataRow dr in dt.Rows)
                            {
                                dr["imageNumber"] = n;
                                dr["imagePath"] = dcm.Replace(this._orderInfo.DicomCDFolder, "");
                                dtImages.ImportRow(dr);
                            }
                        }
                    }

                    this.DisplayPatientDicomInfo(dtImages);
                }
            }
            catch { }
        }

        /// <summary>
        /// Dicomdir 정보를 화면에 보여준다.
        /// </summary>
        /// <param name="dt"></param>
        private void DisplayPatientDicomInfo(DataTable dt)
        {
            try
            {
                string study = "";
                string studydate = "";
                string studytime = "";
                string series = "";
                int imagecount = 0;

                foreach (DataRow row in dt.Rows)
                {
                    imagecount++;
                    studydate = row["StudyDate"].ToString();
                    studytime = row["StudyTime"].ToString();
                    study = row["StudyDesc"].ToString();
                    series = row["SeriesNumber"].ToString() + " : " + row["SeriesModality"].ToString();

                    if (studydate.Length == 8)
                    {
                        studydate = studydate.Substring(0, 4) + "-" + studydate.Substring(4, 2) + "-" + studydate.Substring(6, 2);
                    }
                    if (studytime.Length >= 6)
                    {
                        studytime = studytime.Substring(0, 2) + ":" + studytime.Substring(2, 2) + ":" + studytime.Substring(4, 2);
                    }

                    if (this.checkExistsNode(study, 1, null) != null)
                    {
                        TreeNode studyNode = this.checkExistsNode(study, 1, null);

                        if (this.checkExistsNode(series, 2, studyNode.Nodes) != null)
                        {
                            TreeNode seriesNode = this.checkExistsNode(series, 2, studyNode.Nodes);

                            seriesNode.Nodes.Add(row["imageNumber"].ToString() + " : " + row["imagePath"].ToString());
                        }
                        else
                        {
                            TreeNode seriesNode = new TreeNode();
                            seriesNode.Name = series;
                            seriesNode.Text = series;

                            seriesNode.Nodes.Add(row["imageNumber"].ToString() + " : " + row["imagePath"].ToString());
                            studyNode.Nodes.Add(seriesNode);
                        }
                    }
                    else
                    {
                        TreeNode studyNode = new TreeNode();
                        studyNode.Name = study;
                        studyNode.Text = study + " : " + studydate + " : " + studytime;

                        TreeNode seriesNode = new TreeNode();
                        seriesNode.Name = series;
                        seriesNode.Text = series;

                        seriesNode.Nodes.Add(row["imageNumber"].ToString() + " : " + row["imagePath"].ToString());
                        studyNode.Nodes.Add(seriesNode);
                        this.treeView1.Nodes.Add(studyNode);
                    }
                }

                if (imagecount > 0)
                {
                    this.treeView1.Nodes[0].Expand();
                    this.treeNodeTextChang();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 트리에 노드가 존재하는지 체크
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="step"></param>
        /// <param name="pNode"></param>
        /// <returns></returns>
        private TreeNode checkExistsNode(string nodeName, int step, TreeNodeCollection pNode)
        {
            TreeNode rtNode = null;

            if (step == 1)
            {
                foreach (TreeNode node in this.treeView1.Nodes)
                {
                    if (node.Name == nodeName)
                    {
                        rtNode = node;
                        break;
                    }
                }
            }
            else if (step == 2)
            {
                foreach (TreeNode node in pNode)
                {
                    if (node.Name == nodeName)
                    {
                        rtNode = node;
                        break;
                    }
                }
            }

            return rtNode;
        }

        /// <summary>
        /// 노드별 이미지 갯수로 업데이트
        /// </summary>
        private void treeNodeTextChang()
        {
            int cnt = 0;

            foreach (TreeNode node in this.treeView1.Nodes)
            {
                if (node.Nodes.Count > 0)
                {
                    foreach (TreeNode nd in node.Nodes)
                    {
                        cnt += nd.Nodes.Count;
                    }

                    node.Text += " (" + cnt.ToString() + ")";
                    cnt = 0;
                }
            }
        }

        /// <summary>
        /// Dicomdir 정보를 화면에 보여준다.
        /// </summary>
        private void GetPatientDicomImages(PatientList patList)
        {
            try
            {
                if (patList == null)
                    return;

                if (patList.Base != null)
                {
                    DataTable dt = DicomDirReader.GetImageList(patList.Base);
                    this.DisplayPatientDicomInfo(dt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 조회된 환자 파일 삭제
        /// </summary>
        public void DeletePatientFiles(MainForm mainForm)
        {
            DirectoryInfo directory = new DirectoryInfo(GlobalVar.configEntity.DicomDownloadFolder);

            try
            {
                if (directory.Exists)
                {
                    FileInfo[] files = directory.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        file.Delete();
                        mainForm.txtStatusView.AppendText(file.Name + " deleted.\r\n");
                    }

                    DirectoryInfo[] dirs = directory.GetDirectories();
                    foreach (DirectoryInfo dri in dirs)
                    {
                        dri.Delete(true);
                        mainForm.txtStatusView.AppendText(dri.Name + " deleted.\r\n");
                    }
                }
            }
            catch 
            {
                MessageBox.Show("파일 삭제중 에러가 발생하였습니다.\r\n수동으로 삭제하여 주세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
