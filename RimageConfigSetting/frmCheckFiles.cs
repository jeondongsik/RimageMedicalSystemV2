using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using RimageKorea;

namespace RimageConfigSetting
{
    public partial class frmCheckFiles : DevExpress.XtraEditors.XtraForm
    {
        string fileName;
        bool fileExist = false;

        public bool FileExist
        {
            get { return this.fileExist; }
            set { this.fileExist = value; }
        }

        public frmCheckFiles()
        {
            InitializeComponent();

            this.fileName = GlobalVar.CheckFileList;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            StreamWriter sw = null;

            try
            {
                if (File.Exists(this.fileName))
                    File.Delete(this.fileName);

                if (string.IsNullOrWhiteSpace(this.textBox_Files.Text) == false)
                {
                    sw = new StreamWriter(this.fileName, true, System.Text.Encoding.Default);

                    sw.WriteLine(this.textBox_Files.Text);
                    sw.Flush();

                    this.fileExist = true;
                }
                else
                {
                    this.fileExist = false;
                }
                                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (sw != null) sw.Close();
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCheckFiles_Load(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(this.fileName))
                {
                    this.textBox_Files.Text = File.ReadAllText(this.fileName, System.Text.Encoding.Default);
                }
                else
                {
                    this.textBox_Files.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
