using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lines98
{
    public partial class AboutBox : Form
    {
        public AboutBox(Color color)
        {
            InitializeComponent();
            textEmail.BackColor = color;
            textWeb.BackColor = color;
            BackColor = color;

            if (!AppSettings.Instance.IsPocketPc) return;
            menuStub = new MainMenu();
            Menu = menuStub;

            var size = TextRenderer.MeasureText(textWeb.Text, textWeb.Font);
            textWeb.Width = textEmail.Width = size.Width;
            textWeb.Height = textEmail.Height = size.Height;
            Width = size.Width + 100;
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

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
