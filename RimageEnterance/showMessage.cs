using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using RimageKorea;

namespace RimageEnterance
{
    public partial class showMessage : Form
    {
        public showMessage()
        {
            InitializeComponent();

            string noti = "\r\n  연결가능한 서버가 없습니다.\r\n\r\n  Rimage 시스템이 정상동작 인지 확인하여 주십시오.\r\n\r\n"
                        + "  시스템 전원, 네트워크 연결, 환경설정 상의 문제일 수 있습니다.";

            label_Msg.Text = noti;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string programName = GlobalVar.RIMAGE_CONFIG_SETTING + ".exe";

                Process.Start(programName);
                Thread.Sleep(500);
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Rimage Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Thread.Sleep(500);
                Application.Exit();
            }
        }

        private void showMessage_Load(object sender, EventArgs e)
        {
            this.Text = "     " + GlobalVar.PRODUCT_NAME + "  - Message";
        }
    }
}
