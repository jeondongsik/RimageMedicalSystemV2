namespace RimageConfigSetting
{
    partial class frmCheckEnc
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnXmlEncrypt = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMessage.ForeColor = System.Drawing.Color.Blue;
            this.lblMessage.Location = new System.Drawing.Point(83, 52);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(385, 60);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Config.xml 암호화가 필요합니다.\r\n\r\n아래 버튼을 눌러 암호화를 실행하세요.";
            // 
            // btnXmlEncrypt
            // 
            this.btnXmlEncrypt.Appearance.Font = new System.Drawing.Font("Tahoma", 11F);
            this.btnXmlEncrypt.Appearance.Options.UseFont = true;
            this.btnXmlEncrypt.Location = new System.Drawing.Point(190, 205);
            this.btnXmlEncrypt.Name = "btnXmlEncrypt";
            this.btnXmlEncrypt.Size = new System.Drawing.Size(174, 65);
            this.btnXmlEncrypt.TabIndex = 1;
            this.btnXmlEncrypt.Text = "설정파일 암호화";
            this.btnXmlEncrypt.Click += new System.EventHandler(this.btnXmlEncrypt_Click);
            // 
            // frmCheckEnc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 358);
            this.Controls.Add(this.btnXmlEncrypt);
            this.Controls.Add(this.lblMessage);
            this.Name = "frmCheckEnc";
            this.Text = "설정파일 업데이트 확인";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private DevExpress.XtraEditors.SimpleButton btnXmlEncrypt;
    }
}