using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RimageKorea
{
    public partial class FrmCheckPatientForMerge : Form
    {
        Dictionary<string, string> dicPatList = null;

        public Dictionary<string, string> PatientList
        {
            get { return this.dicPatList; }
            set { this.dicPatList = value; }
        }

        public FrmCheckPatientForMerge()
        {
            InitializeComponent();
        }

        public void SetPatientList(Dictionary<string, string> dicPatList)
        {
            try
            {
                if (dicPatList == null) return;

                int i = 1;
                foreach (KeyValuePair<string, string> kvp in dicPatList)
                {
                    //this.checkedListBoxPatient.Items.Add(string.Format("{0}:{1}", kvp.Key.PadRight(15, ' '), kvp.Value), true);
                    if (i <= 36)
                        this.SetControlData(i, kvp.Key, kvp.Value);

                    i++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("에러 발생!!" + ex.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = System.Windows.Forms.DialogResult.No;
            }
        }

        public void SetControlData(int idx, string id, string name)
        {
            string chkName = "checkBox" + idx;
            string txIDName = "textBoxID" + idx;
            string txNmName = "textBoxName" + idx;

            try
            {
                ((CheckBox)this.panel2.Controls[chkName]).Checked = true;
                this.panel2.Controls[txIDName].Text = id;
                this.panel2.Controls[txNmName].Text = name;
            }
            catch { }
        }

        //private void FrmCheckPatientForMerge_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    this.DialogResult = MessageBox.Show("굽기를 취소할까요?\r\n굽기를 취소할 시 \"" + System.Windows.Forms.DialogResult.Yes.ToString() + "\" 버튼을\r\n환자선택없이 굽기를 진행하려면 \"" + System.Windows.Forms.DialogResult.No.ToString() + "\" 버튼을 클릭하세요.", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //}

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.dicPatList = new Dictionary<string, string>();

            for (int i = 1; i <= 36; i++)
            {
                string chkName = "checkBox" + i;
                string txIDName = "textBoxID" + i;
                string txNmName = "textBoxName" + i;

                if (((CheckBox)this.panel2.Controls[chkName]).Checked == true)
                {
                    string id = this.panel2.Controls[txIDName].Text;
                    string nm = this.panel2.Controls[txNmName].Text;

                    if (string.IsNullOrWhiteSpace(id) == false && string.IsNullOrWhiteSpace(nm) == false)
                        this.dicPatList.Add(this.panel2.Controls[txIDName].Text, this.panel2.Controls[txNmName].Text);
                }
            }

            if (this.dicPatList.Count == 0)
            {
                MessageBox.Show("선택한 환자가 한명도 없습니다.", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = MessageBox.Show("굽기를 취소할까요?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (this.DialogResult == System.Windows.Forms.DialogResult.Yes)
                this.DialogResult = System.Windows.Forms.DialogResult.No;
            else
                return;
        }
    }
}
