using System.ComponentModel;
using System.Windows.Forms;

namespace Lines98
{
    partial class HiScoreForm
    {
        private ColumnHeader columnNo;
        private ColumnHeader columnName;
        private ColumnHeader columnScore;
        private ColumnHeader columnSteps;
        private ListView recordList;
        private Button buttonOk;

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
            this.recordList = new System.Windows.Forms.ListView();
            this.columnNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnScore = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSteps = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // recordList
            // 
            this.recordList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnNo,
            this.columnName,
            this.columnScore,
            this.columnSteps,
            this.columnTime});
            this.recordList.FullRowSelect = true;
            this.recordList.Location = new System.Drawing.Point(0, 0);
            this.recordList.Name = "recordList";
            this.recordList.Size = new System.Drawing.Size(279, 184);
            this.recordList.TabIndex = 1;
            this.recordList.UseCompatibleStateImageBehavior = false;
            this.recordList.View = System.Windows.Forms.View.Details;
            // 
            // columnNo
            // 
            this.columnNo.Text = "Nr";
            this.columnNo.Width = 30;
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnName.Width = 75;
            // 
            // columnScore
            // 
            this.columnScore.Text = "Score";
            this.columnScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnScore.Width = 55;
            // 
            // columnSteps
            // 
            this.columnSteps.Text = "Steps";
            this.columnSteps.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnSteps.Width = 55;
            // 
            // columnTime
            // 
            this.columnTime.Text = "Time";
            this.columnTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(98, 190);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(96, 24);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "OK";
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // HiScoreForm
            // 
            this.ClientSize = new System.Drawing.Size(278, 224);
            this.ControlBox = false;
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.recordList);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HiScoreForm";
            this.Text = "High Scores";
            this.ResumeLayout(false);

        }

        #endregion

        private ColumnHeader columnTime;
    }
}