﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using RimageKorea;
using System.IO;

namespace RimageMedicalSystemV2
{
    public partial class ucPatients2 : DevExpress.XtraEditors.XtraUserControl
    {
        List<BurnOrderedInfoEntity> _patInfoList = new List<BurnOrderedInfoEntity>();
        /// <summary>
        /// 조회된 환자 목록
        /// </summary>
        public List<BurnOrderedInfoEntity> PatientInfoList
        {
            get { return this._patInfoList; }
            set { this._patInfoList = value; }
        }

        public ucPatients2()
        {
            InitializeComponent();

            this.gcPatientlist.DataSource = this._patInfoList;
        }

        /// <summary>
        /// 이미 조회한 환자가 존재하는지 체크
        /// </summary>
        /// <returns></returns>
        public bool ExistSameInfoInGrid(string patFolder)
        {
            try
            {
                //// 존재하면 true
                if (this._patInfoList.Any(s => s.patFolder == patFolder))
                    return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 환자정보 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvPatientlist_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            //// 더블클릭
            if (e.Clicks == 2)
            {
                if (e.RowHandle < 0)
                    return;

                try
                {
                    if (e.Column == this.gridColPatientNo || e.Column == this.gridColPatientName)
                    {
                        //// 굽기 실행

                    }

                    if (e.Column == this.gridColCancel)
                    {
                        //// 삭제
                        if (MessageBox.Show("선택한 환자의 작업을 취소하겠습니까?", "Rimage Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            int idx = this.gvPatientlist.GetDataSourceRowIndex(e.RowHandle);

                            //// 환자 폴더 삭제
                            this.DeletePatFolder(this._patInfoList[idx].patFolderFullPath);

                            this._patInfoList.RemoveAt(idx);
                            this.gcPatientlist.RefreshDataSource();
                        }
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 환자 폴더 삭제
        /// </summary>
        /// <param name="target"></param>
        public void DeletePatFolder(string target)
        {
            try
            {
                DirectoryInfo sourceDir = new DirectoryInfo(target);

                if (sourceDir.Exists)
                {
                    if (GlobalVar.configEntity.DelType == "0")
                    {
                        FileControl.DeleteFolder(sourceDir.FullName, false);
                    }
                    else
                    {
                        FileControl.DeleteFolder(sourceDir.FullName, true);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 선택한 환자 가져오기
        /// </summary>
        /// <returns></returns>
        public BurnOrderedInfoEntity GetSelectedPatient()
        {
            if (this.gvPatientlist.SelectedRowsCount > 0)
            {
                return this.GetSelectedPatient(this.gvPatientlist.FocusedRowHandle);
            }

            return null;
        }

        /// <summary>
        /// 해당 인덱스의 환자 정보 반환
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public BurnOrderedInfoEntity GetSelectedPatient(int rowIndex)
        {
            if (this._patInfoList != null && this._patInfoList.Count > 0)
            {
                return this._patInfoList[rowIndex];
            }

            return null;
        }

        /// <summary>
        /// 선택한 Row의 인덱스 배열 반환
        /// </summary>
        /// <returns></returns>
        public int[] GetSelectedRows()
        {
            if (this.gvPatientlist.SelectedRowsCount > 0)
            {
                return this.gvPatientlist.GetSelectedRows();
            }

            return null;
        }

        /// <summary>
        /// 조회된 폴더목록에서 삭제한다.
        /// </summary>
        /// <param name="idx"></param>
        public void RemoveAtList(int idx)
        {
            this._patInfoList.RemoveAt(idx);
            this.gcPatientlist.RefreshDataSource();
        }

        /// <summary>
        /// 조회된 환자중 선택한 환자폴더를 삭제한다.
        /// </summary>
        /// <param name="mainForm"></param>
        public void DeletePatientFiles(MainForm mainForm)
        {

        }
    }
}
