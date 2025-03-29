using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RimageKorea;

namespace RimageConfigSetting
{
    public partial class frmCheckEnc : Form
    {
        public frmCheckEnc()
        {
            InitializeComponent();
        }

        private void btnXmlEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                Config cf = new Config(Application.StartupPath);
                cf.EncryptXml("config.xml");

                this.DialogResult = DialogResult.OK;
            }
            catch 
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}
