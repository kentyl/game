using System.Drawing;
using System.Threading;
using Lines98.GUI;
using Lines98.Utils;

namespace Lines98.Core
{
    class Game
    {
        public delegate void BallEventHandler(Game game, Ball ball);
        public delegate void BallsEventHandler(Game game, Ball[] balls);
        public delegate void BallMoveEventHandler(Game game, Ball ball, Point oldPos, Point newPos);

        public event BallEventHandler BallAdd;
        public event BallEventHandler BallDelete;
        public event BallEventHandler BallMoved;
        public event BallMoveEventHandler BallMove;
        public event BallsEventHandler BallsDelete;

        private bool _isGameOver;
        private int _ballsPerStep;
        private int _ballsInLine;
        private bool _oneByOneDestruction = true;
        private readonly HiScoreList _hiScore;
        private string _recordsFilename;

        public Board Board { get; private set; }

        public int BallsPerStep
        {
            get { return _ballsPerStep; }
            set { _ballsPerStep = (value > 0) ? value : _ballsPerStep; }
        }

        public int BallsInLine
        {
            get { return _ballsInLine; }
            set { _ballsInLine = (value > 2) ? value : _ballsInLine; }
        }

        public int DestroyBallPause { get; set; }

        public bool OneByOneDestruction
        {
            get { return _oneByOneDestruction; }
            set { _oneByOneDestruction = value; }
        }

        public int StepCount { get; private set; }

        public int Score { get; private set; }

        public int PreviousScore { get; private set; }

        public string Time { get; set; }

        public Ball[] VirtualBalls { get; private set; }

        public Ball[,] PreviousBalls { get; private set; }

        public string RecordsFilename
        {
            get { return _recordsFilename; }
            set
            {
                _recordsFilename = value;
                _hiScore.Load(_recordsFilename);
            }
        }

        public bool IsLocked { get; private set; }

        public bool IsRecord
        {
            get { return Records.IsRecord(Score, StepCount, Time); }
        }

        public bool IsGameOverEvent
        {
            get
            {
                if (!_isGameOver) return false;
                _isGameOver = false;
                return true;
            }
        }

        public HiScoreList Records
        {
            get { return _hiScore; }
        }

        public Game()
        {
            _ballsPerStep = AppSettings.Instance.BallsPerStep;
            _ballsInLine = AppSettings.Instance.MinBallsInLine;
            DestroyBallPause = AppSettings.Instance.PauseDestroyBall;

            _hiScore = new HiScoreList(AppSettings.Instance.HiScoreListSize);
            _recordsFilename = PathHelper.GetAppFile(AppSettings.Instance.HiScoreListFile);

            // Load records from file
            _hiScore.Load(_recordsFilename);
        }

        public void NewGame()
        {
            // Create new game
            Board = new Board();

            // Generate first set of balls
            GenerateNextBalls();
            NextStep();

            // Reset step counter
            StepCount = 0;
            // Reset score
            Score = 0;
            // Reset time
            Time = "00:00";

            // New game
            _isGameOver = false;

            // Unlock in any case
            UnlockBoard();
        }

        public void UndoMove(BoardCtrl boardCtrl)
        {
            VirtualBalls = new Ball[0];

            DrawPreviousBalls(boardCtrl);
            GenerateNextBalls();

            StepCount++;
            Score = PreviousScore;

            _isGameOver = false;
            // Unlock in any case
            UnlockBoard();
        }

        public void SaveResults(string player, string time)
        {
            // Save in the high score table
            if (!IsRecord) return;
            Records.AddRecord(player, Score, StepCount, time);
            Records.Save(RecordsFilename);
        }

        public void GenerateNextBalls()
        {
            VirtualBalls = Board.GenerateVirtualBalls(_ballsPerStep);
        }

        public void DrawPreviousBalls(BoardCtrl boardCtrl)
        {
            for (var i = 0; i < 9; i++)
                for (var j = 0; j < 9; j++)
                {
                    if (Board.Balls[i, j] != null && PreviousBalls[i, j] == null)
                    {
                        boardCtrl.RemoveBall(Board.Balls[i, j]);
                        Board.Balls[i, j] = null;
                    }
                    if (PreviousBalls[i, j] == null || Board.Balls[i, j] != null) continue;
                    boardCtrl.AddNewBall(PreviousBalls[i, j]);
                    Board.Balls[i, j] = PreviousBalls[i, j];
                }
        }

        public void SetPreviousBalls(Ball ball, Point oldPos, Point newPos)
        {
            var board = new Board();
            PreviousBalls = (Ball[,])Board.Balls.Clone();
            for (var i = 0; i < 9; i++)
                for (var j = 0; j < 9; j++)
                    if (PreviousBalls[i, j] != null)
                    {
                        board = PreviousBalls[i, j].Owner;
                        break;
                    }
            PreviousBalls[newPos.X, newPos.Y] = null;
            PreviousBalls[oldPos.X, oldPos.Y] = new Ball(board, ball.Color, oldPos.X, oldPos.Y);
        }

        public bool NextStep()
        {
            // Remove lines that have reached a limit ballsInLine
            if (!DestroyLines())
            {
                // Materialize virtual balls :)
                foreach (var ball in VirtualBalls)
                {
                    if (Board.GetBallAt(ball.X, ball.Y) != null)
                    {
                        var newPos = Board.GetRandomEmptySquare();
                        ball.MoveTo(newPos.X, newPos.Y);
                    }
                    Board.SetBallAt(ball, ball.X, ball.Y);
                    FireBallAdd(ball);
                }

                VirtualBalls = null;

                // Collapse those that appeared after materializing virtual balls
                DestroyLines();

                // Generate following up set of balls
                GenerateNextBalls();
            }

            // Increment step counter
            StepCount++;

            // Each step gives one point to score
            Score++;

            if (VirtualBalls != null) return true;
            _isGameOver = true;
            return false;
        }

        public bool MoveTo(Point pos)
        {
            var destination = Board.GetBallAt(pos.X, pos.Y);
            var ball = Board.GetSelectedBall();
            if ((destination != null) || (ball == null)) return false;
            var route = Board.CreateRoute(ball.Position, pos);
            if (route == null) return false;
            // Fire move event and then move the ball to appropriate pos.
            ball.Select(false);
            AnimateMove(ball, route);
            // ball.MoveTo(pos.X, pos.Y);
            return true;
        }

        protected void AnimateMove(Ball ball, Point[] route)
        {
            var worker = new MoveBallWorker(this, ball, route);
            var moveThread = new Thread(worker.Execute);
            moveThread.Start();
        }

        public void MoveBall(Point from, Point to)
        {
            var ball = Board.GetBallAt(from.X, from.Y);
            if (ball == null) return;
            Board.MoveBall(@from, to);
            FireBallMove(ball, @from, to);
        }

        public bool DestroyLines()
        {
            var ballsToDestroy = Board.FindBallsToDestroy(_ballsInLine);
            var hasBallsToDestroy = ballsToDestroy.Length > 0;
            UpdateScore(ballsToDestroy.Length);

            if (_oneByOneDestruction)
            {
                foreach (var ball in ballsToDestroy)
                {
                    FireBallDelete(ball);
                    Board.DeleteBallAt(ball.X, ball.Y);
                    Thread.Sleep(DestroyBallPause);
                }
            }
            else if (hasBallsToDestroy)
            {
                FireBallsDelete(ballsToDestroy);
                foreach (var ball in ballsToDestroy)
                {
                    Board.DeleteBallAt(ball.X, ball.Y);
                }
            }

            return hasBallsToDestroy;
        }

        public void UpdateScore(int destroyedBalls)
        {
            PreviousScore = Score;

            if (destroyedBalls < _ballsInLine) return;
            // Give one for each ball from minimum
            Score += _ballsInLine;

            // Each extra ball is valued with weight of previous + 2 starting from 2
            var extraValue = 2;
            destroyedBalls -= _ballsInLine;
            while (destroyedBalls > 0)
            {
                Score += extraValue;
                extraValue += 2;
                destroyedBalls--;
            }
        }

        public void FireBallAdd(Ball ball)
        {
            if (BallAdd != null) BallAdd(this, ball);
        }

        public void FireBallDelete(Ball ball)
        {
            if (BallDelete != null) BallDelete(this, ball);
        }

        public void FireBallsDelete(Ball[] balls)
        {
            if (BallsDelete != null) BallsDelete(this, balls);
        }

        public void FireBallMove(Ball ball, Point oldPos, Point newPos)
        {
            if (BallMove != null) BallMove(this, ball, oldPos, newPos);
        }

        public void FireBallMoved(Ball ball)
        {
            if (BallMoved != null) BallMoved(this, ball);
        }

        public bool LockBoard()
        {
            if (IsLocked)
                return false;

            IsLocked = true;
            return true;
        }

        public void UnlockBoard()
        {
            IsLocked = false;
        }
    }
}
