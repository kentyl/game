using System.Drawing;

namespace Lines98.Core
{
    internal class Ball
    {
        private readonly int _color;
        private readonly Board _owner;

        public Ball(int color)
            : this(null, color)
        {
        }

        public Ball(Board owner, int color)
            : this(owner, color, 0, 0)
        {
        }

        public Ball(Board owner, int color, int x, int y)
        {
            _owner = owner;
            _color = color;
            X = x;
            Y = y;
            Selected = false;
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public Point Position
        {
            get { return new Point(X, Y); }
        }

        public int Color
        {
            get { return _color; }
        }

        public Board Owner
        {
            get { return _owner; }
        }

        public bool Selected { get; private set; }

        public void MoveTo(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Select(bool select)
        {
            Selected = select;
        }
    }
}