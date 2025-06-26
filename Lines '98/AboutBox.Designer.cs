using System.ComponentModel;
using System.Windows.Forms;

namespace Lines98
{
    partial class AboutBox
    {
        private Label labelName;
        private Label labelCopyright;
        private Label labelEmail;
        private Label labelWeb;
        private TextBox textEmail;
        private TextBox textWeb;
        private readonly MainMenu menuStub;
        private Button buttonOK;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.labelName = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.labelWeb = new System.Windows.Forms.Label();
            this.textEmail = new System.Windows.Forms.TextBox();
            this.textWeb = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelName.Location = new System.Drawing.Point(91, 11);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(192, 16);
            this.labelName.TabIndex = 7;
            this.labelName.Text = "Lines \'98";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(146, 108);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(64, 24);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelCopyright
            // 
            this.labelCopyright.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelCopyright.Location = new System.Drawing.Point(92, 28);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(192, 24);
            this.labelCopyright.TabIndex = 5;
            this.labelCopyright.Text = "Copyright © Christmas 2015";
            this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelEmail
            // 
            this.labelEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelEmail.Location = new System.Drawing.Point(9, 57);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(48, 16);
            this.labelEmail.TabIndex = 4;
            this.labelEmail.Text = "E-mail:";
            // 
            // labelWeb
            // 
            this.labelWeb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelWeb.Location = new System.Drawing.Point(9, 81);
            this.labelWeb.Name = "labelWeb";
            this.labelWeb.Size = new System.Drawing.Size(48, 16);
            this.labelWeb.TabIndex = 3;
            this.labelWeb.Text = "Web:";
            // 
            // textEmail
            // 
            this.textEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.textEmail.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textEmail.Location = new System.Drawing.Point(63, 55);
            this.textEmail.Name = "textEmail";
            this.textEmail.ReadOnly = true;
            this.textEmail.Size = new System.Drawing.Size(340, 20);
            this.textEmail.TabIndex = 2;
            this.textEmail.Text = "hajbok.robert@gmail.com";
            // 
            // textWeb
            // 
            this.textWeb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.textWeb.Location = new System.Drawing.Point(63, 79);
            this.textWeb.Name = "textWeb";
            this.textWeb.ReadOnly = true;
            this.textWeb.Size = new System.Drawing.Size(340, 20);
            this.textWeb.TabIndex = 1;
            this.textWeb.Text = "https://www.linkedin.com/in/robert-hajbok-b872a6102";
            // 
            // AboutBox
            // 
            this.ClientSize = new System.Drawing.Size(409, 144);
            this.ControlBox = false;
            this.Controls.Add(this.textWeb);
            this.Controls.Add(this.textEmail);
            this.Controls.Add(this.labelWeb);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.labelCopyright);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}