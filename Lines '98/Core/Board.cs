using System;
using System.Drawing;

namespace Lines98.Core
{
    internal class Board
    {
        private readonly int _colors;
        private readonly Random _random;

        public Size BoardSize { get; private set; }

        public int Colors
        {
            get { return _colors; }
        }

        public Ball[,] Balls { get; set; }

        public Ball GetBallAt(int x, int y)
        {
            return Balls[x, y];
        }

        public Ball CreateBallAt(int x, int y, int color)
        {
            var ball = new Ball(this, color, x, y);
            SetBallAt(ball, x, y);

            return ball;
        }

        public void SetBallAt(Ball ball, int x, int y)
        {
            if (GetBallAt(x, y) != null)
            {
                throw new ModelException("The position [" + x + ", " + y + "] is already occupied!");
            }

            Balls[x, y] = ball;
            ball.MoveTo(x, y);
        }

        public void DeleteBallAt(int x, int y)
        {
            Balls[x, y] = null;
        }

        public int GetEmptySquaresCount()
        {
            var result = 0;
            for (var i = 0; i < BoardSize.Width; i++)
            {
                for (var j = 0; j < BoardSize.Height; j++)
                {
                    if (GetBallAt(i, j) == null)
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        public Point GetRandomEmptySquare()
        {
            var pos = _random.Next(1, GetEmptySquaresCount() + 1);
            var curPos = 0;
            for (var i = 0; i < BoardSize.Width; i++)
            {
                for (var j = 0; j < BoardSize.Height; j++)
                {
                    if (GetBallAt(i, j) != null) continue;
                    curPos++;
                    if (curPos == pos)
                    {
                        return new Point(i, j);
                    }
                }
            }
            throw new ModelException("Enable to allocate a new empty position!");
        }

        public Ball[] GenerateVirtualBalls(int amount)
        {
            if (GetEmptySquaresCount() < amount)
            {
                return null;
            }

            var result = new Ball[amount];
            for (var i = 0; i < amount; i++)
            {
                var pos = GetRandomEmptySquare();
                result[i] = CreateBallAt(pos.X, pos.Y, GetRandomColor());
            }
            for (var i = 0; i < amount; i++)
            {
                DeleteBallAt(result[i].X, result[i].Y);
            }

            return result;
        }

        public int GetRandomColor()
        {
            return _random.Next(1, _colors + 1);
        }

        public Ball GetSelectedBall()
        {
            for (var i = 0; i < BoardSize.Width; i++)
            {
                for (var j = 0; j < BoardSize.Height; j++)
                {
                    var ball = GetBallAt(i, j);
                    if ((ball != null) &&
                        (ball.Selected))
                    {
                        return ball;
                    }
                }
            }
            return null;
        }

        public void SelectBall(Ball ball)
        {
            SelectBall(ball.X, ball.Y);
        }

        public void SelectBall(int x, int y)
        {
            // Deselect a ball if one is already selected
            DeselectBall();

            // Select a new ball
            var ball = GetBallAt(x, y);
            if (ball != null)
            {
                ball.Select(true);
            }
        }

        public void DeselectBall()
        {
            var ball = GetSelectedBall();
            if (ball != null)
            {
                ball.Select(false);
            }
        }

        public Point[] CreateRoute(Point from, Point to)
        {
            var matrix = new int[BoardSize.Width, BoardSize.Height];

            for (var i = 0; i < BoardSize.Width; i++)
            {
                for (var j = 0; j < BoardSize.Height; j++)
                {
                    var ball = GetBallAt(i, j);
                    if (ball != null)
                    {
                        matrix[i, j] = PathFinder.PosOccupied;
                    }
                }
            }

            var pathFinder = new PathFinder(matrix);
            return pathFinder.FindPath(from, to);
        }

        public void MoveBall(Point from, Point to)
        {
            var ball = GetBallAt(from.X, from.Y);
            if (ball == null)
            {
                throw new ModelException("There is no ball under position [" + from.X + ", " + from.Y + "]");
            }
            var posTo = GetBallAt(to.X, to.Y);
            if (posTo != null)
            {
                throw new ModelException("The position [" + to.X + ", " + to.Y + "] is already occupied!");
            }

            Balls[from.X, from.Y] = null;
            Balls[to.X, to.Y] = ball;
            ball.MoveTo(to.X, to.Y);
        }

        public Ball[] FindBallsToDestroy(int ballsInLine)
        {
            var matrix = new int[BoardSize.Width, BoardSize.Height];

            for (var x = 0; x < BoardSize.Width; x++)
            {
                for (var y = 0; y < BoardSize.Height; y++)
                {
                    var ball = GetBallAt(x, y);
                    matrix[x, y] = ball != null ? ball.Color : -1;
                }
            }

            var finder = new DestroyFinder(matrix, ballsInLine);
            var locations = finder.FindLocations();

            var result = new Ball[locations.Length];
            var i = 0;
            foreach (var location in locations)
            {
                result[i++] = GetBallAt(location.X, location.Y);
            }

            return result;
        }

        public Board()
            : this(new Size(AppSettings.Instance.BoardWidth, AppSettings.Instance.BoardHeight))
        {
        }

        public Board(Size size)
            : this(size, AppSettings.Instance.Colors)
        {
        }

        public Board(Size size, int colors)
        {
            BoardSize = size;
            Balls = new Ball[size.Width, size.Height];
            _colors = colors;
            _random = new Random((int) DateTime.Now.Ticks);
        }
    }
}