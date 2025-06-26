using System.Drawing;

namespace Lines98.Core
{
    class PathFinder
    {
        public static readonly int PosFree = 0;
        public static readonly int PosOccupied = -1;
        public static readonly int PosStart = -2;
        public static readonly int PosEnd = 1;

        private readonly int[,] _matrix;

        public PathFinder(int[,] matrix)
        {
            _matrix = matrix;
            MaxPathLength = (Width + Height) * 2;
        }

        public int MaxPathLength { set; get; }

        public int Width
        {
            get { return _matrix.GetLength(0); }
        }

        public int Height
        {
            get { return _matrix.GetLength(1); }
        }

        public void SetPosValue(int x, int y, int val)
        {
            _matrix[x, y] = val;
        }

        protected void SetStartPos(int x, int y)
        {
            SetPosValue(x, y, PosStart);
        }

        protected void SetEndPos(int x, int y)
        {
            SetPosValue(x, y, PosEnd);
        }

        public Point[] FindPath(Point start, Point end)
        {
            SetStartPos(start.X, start.Y);
            SetEndPos(end.X, end.Y);

            var found = false;
            var current = PosEnd;
            while ((current < (MaxPathLength + PosEnd)) && (!found))
            {
                for (var i = 0; i < Width; i++)
                {
                    for (var j = 0; j < Height; j++)
                    {
                        if (_matrix[i, j] != current) continue;
                        if (CheckNeighbour(i - 1, j, current + 1))
                        {
                            found = true;
                        }
                        if (CheckNeighbour(i + 1, j, current + 1))
                        {
                            found = true;
                        }
                        if (CheckNeighbour(i, j - 1, current + 1))
                        {
                            found = true;
                        }
                        if (CheckNeighbour(i, j + 1, current + 1))
                        {
                            found = true;
                        }
                    }
                }
                current++;
            }

            if (!found) return null;
            // Build the path
            var result = new Point[current - PosEnd];
            var pos = start;
            var index = 0;
            while (pos != end)
            {
                pos = GetMinNearest(pos);
                result[index++] = pos;
            }

            return result;
        }

        private bool CheckNeighbour(int x, int y, int val)
        {
            if (!IsInBounds(x, y)) return false;
            if (_matrix[x, y] == PosStart)
            {
                return true;
            }
            if (_matrix[x, y] == PosFree)
            {
                _matrix[x, y] = val;
            }
            return false;
        }

        private bool IsInBounds(int x, int y)
        {
            return (x >= 0) && (y >= 0) && (x < Width) && (y < Height);
        }

        private Point GetMinNearest(Point pos)
        {
            var min = _matrix[pos.X, pos.Y];

            var result = pos;

            var x = pos.X - 1;
            var y = pos.Y;
            if (IsInBounds(x, y) && (_matrix[x, y] > 0))
            {
                if ((min < 0) || (_matrix[x, y] < min))
                {
                    min = _matrix[x, y];
                    result = new Point(x, y);
                }
            }
            x = pos.X + 1;
            y = pos.Y;
            if (IsInBounds(x, y) && (_matrix[x, y] > 0))
            {
                if ((min < 0) || (_matrix[x, y] < min))
                {
                    min = _matrix[x, y];
                    result = new Point(x, y);
                }
            }
            x = pos.X;
            y = pos.Y - 1;
            if (IsInBounds(x, y) && (_matrix[x, y] > 0))
            {
                if ((min < 0) || (_matrix[x, y] < min))
                {
                    min = _matrix[x, y];
                    result = new Point(x, y);
                }
            }
            x = pos.X;
            y = pos.Y + 1;
            if (!IsInBounds(x, y) || (_matrix[x, y] <= 0)) return result;
            if ((min >= 0) && (_matrix[x, y] >= min)) return result;
            result = new Point(x, y);
            return result;
        }
    }
}
