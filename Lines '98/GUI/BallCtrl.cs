using System.Drawing;
using System.Windows.Forms;
using Lines98.Core;

namespace Lines98.GUI
{
    class BallCtrl
    {
        private Rectangle _bounds = new Rectangle(0, 0, 0, 0);
        private readonly Ball _ball;
        private int _jumpPosition;
        private readonly Preferences _preferences;

        public Ball Ball
        {
            get { return _ball; }
        }

        public int Width
        {
            get { return _bounds.Width; }
            set { _bounds.Width = value; }
        }

        public int Height
        {
            get { return _bounds.Height; }
            set { _bounds.Height = value; }
        }

        public Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        public BallCtrl(Ball ball, Preferences preferences)
        {
            _ball = ball;
            _preferences = preferences;
        }

        public void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var rect = new Rectangle(_bounds.X + 5, _bounds.Y + 5, _bounds.Width - 10, _bounds.Height - 10);
            Brush brush = new SolidBrush(_preferences.GetBallColor(Ball.Color));
            var pen = new Pen(_preferences.GetBallBorderColor(Ball.Color));

            // Background
            g.FillRectangle(new SolidBrush(Color.Transparent), rect);

            // Ball itself
            rect.Inflate(-3, -3);

            // Draw the ball depending on the select status
            if (_ball.Selected)
            {
                switch (_jumpPosition)
                {
                    case 0:
                        break;
                    case 1:
                        rect.Inflate(0, -1);
                        rect.Offset(0, -1);
                        break;
                    case 2:
                        rect.Inflate(0, -1);
                        rect.Offset(0, -2);
                        break;
                    case 3:
                        rect.Inflate(0, 0);
                        rect.Offset(0, -1);
                        break;
                    case 4:
                        rect.Inflate(0, -1);
                        rect.Offset(0, +1);
                        break;
                    case 5:
                        rect.Inflate(0, -1);
                        rect.Offset(0, +2);
                        break;
                    case 6:
                        rect.Inflate(0, -1);
                        rect.Offset(0, +1);
                        break;
                }
            }

            g.FillEllipse(brush, rect);
            g.DrawEllipse(pen, rect);
        }

        public void NexJumpPosition()
        {
            _jumpPosition++;
            _jumpPosition = (_jumpPosition > 5) ? 0 : _jumpPosition + 1;
        }
    }
}
