namespace RimageEnterance
{
    partial class showMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(showMessage));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label_Msg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 111);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(247, 43);
            this.button1.TabIndex = 1;
            this.button1.Text = "환경설정으로 가기";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(297, 111);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(223, 43);
            this.button2.TabIndex = 2;
            this.button2.Text = "프로그램 종료";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label_Msg
            // 
            this.label_Msg.BackColor = System.Drawing.Color.Snow;
            this.label_Msg.ForeColor = System.Drawing.Color.Black;
            this.label_Msg.Location = new System.Drawing.Point(12, 9);
            this.label_Msg.Name = "label_Msg";
            this.label_Msg.Size = new System.Drawing.Size(508, 89);
            this.label_Msg.TabIndex = 3;
            // 
            // showMessage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(532, 168);
            this.Controls.Add(this.label_Msg);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "showMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "     Rimage Medical Disc System  - Message";
            this.Load += new System.EventHandler(this.showMessage_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label_Msg;
    }
}