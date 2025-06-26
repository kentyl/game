using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Lines98.Core;

namespace Lines98.GUI
{
    class BoardCtrl : GenericCtrl
    {
        public delegate void FildEventHandler(BoardCtrl board);
        public delegate void CellEventHandler(BoardCtrl board, Point cell);
        public delegate void BallEventHandler(BoardCtrl board, BallCtrl ball);

        public event CellEventHandler CellClick;
        public event BallEventHandler BallClick;

        private readonly ArrayList _balls = new ArrayList();

        public BoardCtrl()
        {
            MouseDown += OnMouseDown;
        }

        public int BallCount
        {
            get
            {
                return _balls.Count;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            if (e.ClipRectangle.Width < ((int)GetCellSize() + 1))
            {
                // Redraw clip rectangle only

                // Draw field lines (has to be done)
                //DrawFieldLines(g);

                // Draw balls
                g.FillRectangle(new SolidBrush(Color.Transparent), e.ClipRectangle);
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < _balls.Count; i++)
                {
                    var ball = (BallCtrl)_balls[i];
                    if (e.ClipRectangle.IntersectsWith(ball.Bounds))
                    {
                        ball.OnPaint(e);
                    }
                }

                // Draw virtual balls
                if (Game.VirtualBalls == null) return;
                foreach (var t in from t in Game.VirtualBalls let rect = GetCellRect(t.Position) where e.ClipRectangle.IntersectsWith(rect) select t)
                {
                    DrawVirtualBall(g, t);
                }
            }
            else
            {
                // Draw frame
                var frame = new Rectangle(0, 0, Width - 1, Height - 1);
                g.DrawRectangle(new Pen(Color.DimGray), frame); // Color.RosyBrown

                // Draw field frame
                var fieldPen = new Pen(Preferences.FieldFgColor);
                var fieldRect = GetFieldRect();
                g.FillRectangle(new SolidBrush(Color.Transparent), fieldRect);
                g.DrawRectangle(fieldPen, fieldRect);

                // Draw field lines
                DrawFieldLines(g);

                // Draw balls
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i=0; i < _balls.Count; i++)
                {
                    ((BallCtrl)_balls[i]).OnPaint(e);
                }

                // Draw virtual balls
                if (Game.VirtualBalls == null) return;
                foreach (var t in Game.VirtualBalls)
                {
                    DrawVirtualBall(g, t);
                }
            }
        }

        private void DrawFieldLines(Graphics g)
        {
            var gameSize = Game.Board.BoardSize;
            var fieldRect = GetFieldRect();
            var fieldPen = new Pen(Preferences.FieldFgColor);
            var size = GetCellSize();

            // Draw horizontal lines
            for (var i = 0; i < gameSize.Width; i++)
            {
                g.DrawLine(
                    fieldPen,
                    (int)(i * size) + fieldRect.Left,
                    fieldRect.Top,
                    (int)(i * size) + fieldRect.Left,
                    fieldRect.Bottom);
            }
            // Draw vertical lines
            for (var i = 0; i < gameSize.Height; i++)
            {
                g.DrawLine(
                    fieldPen,
                    fieldRect.Left,
                    (int)(i * size) + fieldRect.Top,
                    fieldRect.Right,
                    (int)(i * size) + fieldRect.Top);
            }
        }

        private void DrawVirtualBall(Graphics g, Ball ball)
        {
            var rect = GetCellRect(ball.Position);
            rect.Inflate(-rect.Width / 3, -rect.Height / 3);
            g.FillEllipse(new SolidBrush(Preferences.GetBallColor(ball.Color)), rect);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // recalculate ball positions
            foreach (var ballCtrl in _balls.Cast<BallCtrl>())
            {
                ballCtrl.Bounds = GetCellRect(ballCtrl.Ball.Position);
            }
        }

        public void AddNewBall(Ball ball)
        {
            var ballCtrl = FindBall(ball);
            if (ballCtrl == null)
            {
                ballCtrl = new BallCtrl(ball, Preferences) {Bounds = GetCellRect(ball.Position)};
                _balls.Add(ballCtrl);
            }
            InvalidateCell(ball.Position);
            //ballCtrl.Invalidate();
        }

        public void RemoveBall(Ball ball)
        {
            var ballCtrl = FindBall(ball);
            if (ballCtrl != null)
            {
                _balls.Remove(ballCtrl);
            }
            InvalidateCell(ball.Position);
        }

        public void RemoveBalls(Ball[] balls)
        {
            foreach (var ball in balls)
            {
                RemoveBall(ball);
            }
        }

        public void RemoveAllBalls()
        {
            _balls.Clear();
        }

        private float GetCellSize()
        {
            var gameSize = Game.Board.BoardSize;

            var sizeX = (float)(Width - 1) / gameSize.Width / 1.4f;
            var sizeY = (float)(Height - 1) / gameSize.Height / 1.4f;

            return (sizeX > sizeY) ? sizeY : sizeX;
        }

        private Rectangle GetFieldRect()
        {
            var gameSize = Game.Board.BoardSize;
            var size = GetCellSize();
            var fieldRect = new Rectangle(0, 0, (int)(size * gameSize.Width), (int)(size * gameSize.Height));
            fieldRect.Offset((Width - 1 - fieldRect.Width) / 2, (Height - 1 - fieldRect.Height) / 2);
            return fieldRect;
        }

        public Rectangle GetCellRect(Point cell)
        {
            var fieldRect = GetFieldRect();
            var size = GetCellSize();
            var cellRect = new Rectangle(
                    (int)(cell.X * size + fieldRect.Left + 1),
                    (int)(cell.Y * size + fieldRect.Top + 1),
                    (int)size - 1,
                    (int)size - 1
                );
            return cellRect;
        }

        public void InvalidateCell(Point cell)
        {
            Invalidate(GetCellRect(cell));
        }

        protected void OnMouseDown(object sender, MouseEventArgs e)
        {
            var cell = GetCellFromLocation(e.X, e.Y);
            if ((cell.X < 0) || (cell.Y < 0)) return;
            var ball = FindBall(cell);
            if (ball != null)
            {
                FireBallClick(ball);
            }
            else
            {
                FireCellClick(cell);
            }
        }

        protected Point GetCellFromLocation(int x, int y)
        {
            var gameSize = Game.Board.BoardSize;
            var size = GetCellSize();

            var field = new Rectangle(0, 0, (int)(size * gameSize.Width), (int)(size * gameSize.Height));
            field.Offset((Width - 1 - field.Width) / 2, (Height - 1 - field.Height) / 2);

            if (field.Contains(x, y))
            {
                return new Point(
                    (int)((x - field.Left) / size),
                    (int)((y - field.Top) / size));
            }

            return new Point(-1, -1);
        }

        public void DrawJumpingBall()
        {
            // Find and repaint Jumping ball
            var ball = Game.Board.GetSelectedBall();
            if (ball == null) return;
            var ballCtrl = FindBall(ball);
            if (ballCtrl == null) return;
            ballCtrl.NexJumpPosition();
            InvalidateCell(ball.Position);
        }

        public BallCtrl FindBall(Point pos)
        {
            return _balls.Cast<BallCtrl>().FirstOrDefault(ball => ball.Ball.Position == pos);
        }

        public BallCtrl FindBall(Ball ball)
        {
            return FindBall(ball.Position);
        }

        public void MoveBall(Ball ball, Point oldPos, Point newPos)
        {
            // Move
            var ballCtrl = FindBall(ball);
            ballCtrl.Bounds = GetCellRect(newPos);

            // Redraw
            InvalidateCell(oldPos);
            InvalidateCell(newPos);
        }

        public void FireCellClick(Point cell)
        {
            if (CellClick != null) CellClick(this, cell);
        }

        public void FireBallClick(BallCtrl ball)
        {
            if (BallClick != null) BallClick(this, ball);
        }
    }
}
