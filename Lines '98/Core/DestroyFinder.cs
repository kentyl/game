using System.Collections;
using System.Drawing;
using System.Linq;

namespace Lines98.Core
{
    class DestroyFinder
    {
        private readonly int[,] _matrix;
        private readonly int _ballsInLine;

        public DestroyFinder(int[,] matrix, int ballsInLine)
        {
            _matrix = matrix;
            _ballsInLine = ballsInLine;
        }

        public int Width
        {
            get { return _matrix.GetLength(0); }
        }

        public int Height
        {
            get { return _matrix.GetLength(1); }
        }

        public Point[] FindLocations()
        {
            var locations = new ArrayList();

            // Horizontal lines
            AddUnique(locations, FindHorizontal());
            // Vertical lines
            AddUnique(locations, FindVertical());
            // NW-to-SE lines
            AddUnique(locations, FindDiagonal());
            // NE-to-SW lines
            AddUnique(locations, FindDiagonalBackward());

            var result = new Point[locations.Count];
            for (var i = 0; i < locations.Count; i++)
            {
                result[i] = (Point)locations[i];
            }

            return result;
        }

        private static void AddUnique(IList addTo, IList addFrom)
        {
            foreach (var pos in from Point pos in addFrom let unique = addTo.Cast<object>().All(t1 => ((Point) t1) != pos) where unique select pos)
            {
                addTo.Add(pos);
            }
        }

        private ArrayList FindHorizontal()
        {
            var locations = new ArrayList();

            for (var y = 0; y < Height; y++)
            {
                // Each horizontal line
                var length = 0;
                var x = 0;
                var color = -1;
                while (x < Width)
                {
                    var newColor = _matrix[x, y];
                    if ((newColor > 0) && (newColor == color))
                    {
                        length++;
                    }
                    else
                    {
                        if (length >= _ballsInLine)
                        {
                            for (var i = 0; i < length; i++)
                            {
                                locations.Add(new Point(x - i - 1, y));
                            }
                        }

                        if (newColor > 0)
                        {
                            length = 1;
                            color = newColor;
                        }
                        else
                        {
                            length = 0;
                            color = -1;
                        }
                    }
                    x++;
                }
                if (length < _ballsInLine) continue;
                for (var i = 0; i < length; i++)
                {
                    locations.Add(new Point(x - i - 1, y));
                }
            }

            return locations;
        }

        private ArrayList FindVertical()
        {
            var locations = new ArrayList();

            for (var x = 0; x < Width; x++)
            {
                // Each vertical line
                var length = 0;
                var y = 0;
                var color = -1;
                while (y < Height)
                {
                    var newColor = _matrix[x, y];
                    if ((newColor > 0) && (newColor == color))
                    {
                        length++;
                    }
                    else
                    {
                        if (length >= _ballsInLine)
                        {
                            for (var i = 0; i < length; i++)
                            {
                                locations.Add(new Point(x, y - i - 1));
                            }
                        }

                        if (newColor > 0)
                        {
                            length = 1;
                            color = newColor;
                        }
                        else
                        {
                            length = 0;
                            color = -1;
                        }
                    }
                    y++;
                }
                if (length < _ballsInLine) continue;
                for (var i = 0; i < length; i++)
                {
                    locations.Add(new Point(x, y - i - 1));
                }
            }

            return locations;
        }

        private ArrayList FindDiagonal()
        {
            var locations = new ArrayList();

            for (var line = _ballsInLine - 1; line < (Width + Height - 2); line++)
            {
                // Each diagonal line
                var length = 0;
                var color = -1;
                int x = 0, y = line - x;
                for (; x <= line; x++, y = line - x)
                {
                    if ((x < Width) && (y < Height))
                    {
                        var newColor = _matrix[x, y];
                        if ((newColor > 0) && (newColor == color))
                        {
                            length++;
                        }
                        else
                        {
                            if (length >= _ballsInLine)
                            {
                                for (var i = 0; i < length; i++)
                                {
                                    locations.Add(new Point(x - i - 1, y + i + 1));
                                }
                            }

                            if (newColor > 0)
                            {
                                length = 1;
                                color = newColor;
                            }
                            else
                            {
                                length = 0;
                                color = -1;
                            }
                        }
                    }
                    else if (y < Height)
                    {
                        if (length < _ballsInLine) continue;
                        for (var i = 0; i < length; i++)
                        {
                            locations.Add(new Point(x - i - 1, y + i + 1));
                        }
                        length = 0;
                    }
                }
                if (length < _ballsInLine) continue;
                for (var i = 0; i < length; i++)
                {
                    locations.Add(new Point(x - i - 1, y + i + 1));
                }
            }

            return locations;
        }

        private ArrayList FindDiagonalBackward()
        {
            var locations = new ArrayList();

            for (var line = _ballsInLine - 1; line < (Width + Height - 2); line++)
            {
                // Each backward diagonal line
                var length = 0;
                var color = -1;
                int x = 0, y = Height - 1 - line + x;
                for (; x <= line; x++, y = Height - 1 - line + x)
                {
                    if ((x < Width) && (y >= 0) && (y < Height))
                    {
                        var newColor = _matrix[x, y];
                        if ((newColor > 0) && (newColor == color))
                        {
                            length++;
                        }
                        else
                        {
                            if (length >= _ballsInLine)
                            {
                                for (var i = 0; i < length; i++)
                                {
                                    locations.Add(new Point(x - i - 1, y - i - 1));
                                }
                            }

                            if (newColor > 0)
                            {
                                length = 1;
                                color = newColor;
                            }
                            else
                            {
                                length = 0;
                                color = -1;
                            }
                        }
                    }
                    else if ((y >= 0) && (y < Height))
                    {
                        if (length < _ballsInLine) continue;
                        for (var i = 0; i < length; i++)
                        {
                            locations.Add(new Point(x - i - 1, y - i - 1));
                        }
                        length = 0;
                    }
                }
                if (length < _ballsInLine) continue;
                for (var i = 0; i < length; i++)
                {
                    locations.Add(new Point(x - i - 1, y - i - 1));
                }
            }

            return locations;
        }
    }
}
