using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Lines98.Core;

namespace Lines98
{
    public partial class HiScoreForm : Form
    {
        public HiScoreForm(Color color)
        {
            InitializeComponent();
            recordList.BackColor = color;
            BackColor = color;
            MinimumSize = Size;
            MaximumSize = Size;
        }

        public override sealed Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        public override sealed Size MaximumSize
        {
            get { return base.MaximumSize; }
            set { base.MaximumSize = value; }
        }

        public override sealed Size MinimumSize
        {
            get { return base.MinimumSize; }
            set { base.MinimumSize = value; }
        }

        public void UpdateRecordList(HiScoreList records)
        {
            recordList.Items.Clear();
            int[] pos = {1};
            foreach (var item in from HiScoreList.PlayerScore playerScore in records where playerScore != null select new ListViewItem(
                new[] {
                    pos[0].ToString(),
                    playerScore.Name,
                    playerScore.Score.ToString(),
                    playerScore.Steps.ToString(),
                    playerScore.Time
                }))
            {
                recordList.Items.Add(item);
                pos[0]++;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
