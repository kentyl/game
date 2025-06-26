using System.Drawing;

namespace Lines98.GUI
{
    class Preferences
    {
        public Color GetBallColor(int color)
        {
            switch (color)
            {
                case 1:
                    return Color.Red;
                case 2:
                    return Color.Green;
                case 3:
                    return Color.Blue;
                case 4:
                    return Color.Yellow;
                case 5:
                    return Color.Orange;
                case 6:
                    return Color.Brown;
                case 7:
                    return Color.Cyan;
                case 8:
                    return Color.Violet;
                default:
                    return Color.Black;
            }
        }

        public Color GetBallBorderColor(int color)
        {
            switch (color)
            {
                case 1:
                    return Color.DarkRed;
                case 2:
                    return Color.DarkGreen;
                case 3:
                    return Color.DarkBlue;
                case 4:
                    return Color.Gold;
                case 5:
                    return Color.DarkOrange;
                case 6:
                    return Color.Black;
                case 7:
                    return Color.DarkCyan;
                case 8:
                    return Color.DarkViolet;
                default:
                    return Color.Black;
            }
        }

        public Color FieldFgColor
        {
            get { return Color.Black; }
        }

        public Color IndicatorBgColor
        {
            get { return Color.WhiteSmoke; }
        }

        public Color IndicatorFgColor
        {
            get { return Color.Black; }
        }

        public Color IndicatorTextColor
        {
            get { return Color.PaleGreen; }
        }

        public Color IndicatorCtrlBgColor
        {
            get { return Color.DarkSlateGray; }
        }
    }
}
