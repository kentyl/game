using System.Drawing;
using System.Windows.Forms;
using Lines98.Core;

namespace Lines98.GUI
{
    class GenericCtrl : Control
    {
        public Game Game { get; set; }

		public Preferences Preferences { get; set; }

		protected override void OnPaint(PaintEventArgs e)
		{
			/* Draw frame */
			var g = e.Graphics;
			var box = new Rectangle(0, 0, Width - 1, Height - 1);
			g.DrawRectangle(new Pen(Color.Black), box);
		}
    }
}
