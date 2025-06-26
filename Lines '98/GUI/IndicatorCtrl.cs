using System.Drawing;
using System.Windows.Forms;

namespace Lines98.GUI
{
    class IndicatorCtrl : GenericCtrl
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            // Draw frame
            var g = e.Graphics;
            var box = new Rectangle(0, 0, Width - 1, Height - 1);
            g.FillRectangle(new SolidBrush(Preferences.IndicatorCtrlBgColor), box);

            // Exit if Game isn't defined
            if (Game == null) return;

            var fieldPen = new Pen(Preferences.IndicatorFgColor);
            var padding = (Width - GetCellSize() * Game.BallsPerStep) / 2;

            // Balls
            var cellRect = new Rectangle(padding, 1, GetCellSize(), GetCellSize());
            var ballRect = cellRect;
            ballRect.Inflate(-2, -2);

            if (Game.VirtualBalls != null)
            {
                var balls = Game.VirtualBalls;
                foreach (var t in balls)
                {
                    g.FillRectangle(new SolidBrush(Preferences.IndicatorBgColor), cellRect);
                    g.DrawRectangle(fieldPen, cellRect);
                    g.FillEllipse(new SolidBrush(Preferences.GetBallColor(t.Color)), ballRect);
                    g.DrawEllipse(new Pen(Preferences.GetBallBorderColor(t.Color)), ballRect);

                    cellRect.Offset(GetCellSize(), 0);
                    ballRect.Offset(GetCellSize(), 0);
                }
            }

            // Step indicator
            float fontSizePx = GetCellSize() - 5;
            var fontSizePt = fontSizePx * AppSettings.Instance.FactorPxToPt;
            Brush textBrush = new SolidBrush(Preferences.IndicatorTextColor);
            var font = new Font(FontFamily.GenericSansSerif, fontSizePt, FontStyle.Bold);
            var textRect = new RectangleF(3, 4, padding - 6, Height - 2);
            g.DrawString("Step: " + Game.StepCount, font, textBrush, textRect);

            // Score indicator
            textRect = new RectangleF(Width - 200, 4, padding - 6, Height - 2);
            g.DrawString("Score: " + Game.Score, font, textBrush, textRect);
        }

        private int GetCellSize()
        {
            return Height - 5;
        }
    }
}
