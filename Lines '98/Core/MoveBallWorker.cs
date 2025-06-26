using System.Drawing;
using System.Threading;

namespace Lines98.Core
{
    class MoveBallWorker
    {
        private readonly Game _game;
        private readonly Ball _ball;
        private readonly Point[] _path;

        private readonly int _pause = AppSettings.Instance.PauseMoveBall;

        public MoveBallWorker(Game game, Ball ball, Point[] path)
        {
            _game = game;
            _ball = ball;
            _path = path;
        }

        public void Execute()
        {
            foreach (var t in _path)
            {
                var oldPos = _ball.Position;
                var newPos = t;

                _game.MoveBall(oldPos, newPos);

                Thread.Sleep(_pause);
            }
            _game.FireBallMoved(_ball);
        }
    }
}
