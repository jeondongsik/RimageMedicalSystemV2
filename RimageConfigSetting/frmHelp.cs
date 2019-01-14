using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RimageConfigSetting
{
    public partial class frmHelp : DevExpress.XtraEditors.XtraForm
    {
        public frmHelp()
        {
            InitializeComponent();
        }

        private void frmHelp_Load(object sender, EventArgs e)
        {
            this.textBox1.DeselectAll();
            this.textBox1.Select(this.textBox1.Text.Length, 0);
            this.textBox1.Focus();
        }
    }
}
