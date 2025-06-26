using System.Drawing;
using Lines98.Utils;

namespace Lines98
{
    class AppSettings
    {
        private const string PropertiesFilename = "settings.xml";
        private const int BallsperStep = 3;
        private const int MinBallsinLine = 5;
        private const int Boardwidth = 9;
        private const int Boardheight = 9;
        private const int ColorAmount = 6;
        private const int HighScoreListSize = 10;
        private const string HiScoreFilename = "hiscore.xml";
        private const int MoveBallPause = 50;
        private const int DestroyBallPause = 1000;
        private const int Magicpadding = 24;
        private const float FactorPxtoPt = 1f / 96 * 64;
        private static AppSettings _instance;

        private AppSettings()
        {
            Load();
        }

        public static AppSettings Instance
        {
            get { return _instance ?? (_instance = new AppSettings()); }
        }

        private readonly Utils.Properties _props = new Utils.Properties();

        public void Load()
        {
            _props.Load(PathHelper.GetAppFile(PropertiesFilename));
        }

        public void Save()
        {
            _props.Save(PathHelper.GetAppFile(PropertiesFilename));
        }

        public int BallsPerStep
        {
            get { return _props.GetProperty("ballsPerStep", BallsperStep); }
            set { _props.SetProperty("ballsPerStep", value); }
        }

        public int MinBallsInLine
        {
            get { return _props.GetProperty("minBallsInLine", MinBallsinLine); }
            set { _props.SetProperty("minBallsInLine", value); }
        }

        public int HiScoreListSize
        {
            get { return _props.GetProperty("hiScoreListSize", HighScoreListSize); }
            set { _props.SetProperty("hiScoreListSize", value); }
        }

        public string HiScoreListFile
        {
            get { return _props.GetProperty("hiScoreListFile", HiScoreFilename); }
            set { _props.SetProperty("hiScoreListFile", value); }
        }

        public int BoardWidth
        {
            get { return _props.GetProperty("boardWidth", Boardwidth); }
            set { _props.SetProperty("boardWidth", value); }
        }

        public int BoardHeight
        {
            get { return _props.GetProperty("boardHeight", Boardheight); }
            set { _props.SetProperty("boardHeight", value); }
        }

        public int Colors
        {
            get { return _props.GetProperty("colors", ColorAmount); }
            set { _props.SetProperty("colors", value); }
        }

        public float FactorPxToPt
        {
            get { return FactorPxtoPt; }
        }

        public int MagicPadding
        {
            get { return _props.GetProperty("magicPadding", Magicpadding); }
        }

        public int PauseDestroyBall
        {
            get { return _props.GetProperty("pauseDestroyBall", DestroyBallPause); }
            set { _props.SetProperty("pauseDestroyBall", value); }
        }

        public int PauseMoveBall
        {
            get { return _props.GetProperty("pauseMoveBall", MoveBallPause); }
            set { _props.SetProperty("pauseMoveBall", value); }
        }

        public bool OneByOneDestroy
        {
            get { return _props.GetProperty("oneByOneDestroy", true); }
            set { _props.SetProperty("oneByOneDestroy", value); }
        }

        public bool IsPocketPc
        {
            get { return _props.GetProperty("isPocketPC", true); }
        }
    }
}
