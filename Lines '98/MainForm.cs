using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Lines98.Core;
using Lines98.GUI;

namespace Lines98
{
    public partial class MainForm : Form
    {
        private IndicatorCtrl _indicatorCtrl;
        private BoardCtrl _boardCtrl;
        private Timer _ballJump;
        private Timer _gameOverWatchDog;
        private Label _timerLabel;
        private Label _ballCountLabel;
        private Label _undoLabel;

        private readonly Game _game = new Game();
        private readonly Preferences _preferences = new Preferences();
        private readonly int _menuSize;
        private bool _enableUndo;
        private bool _previousBallsSet;
        private Color _color;

        public MainForm()
        {
            _menuSize = AppSettings.Instance.MagicPadding;
            ExtraInitializeComponent();
            InitializeComponent();
            _color = BackColor = _gameColors.Color = Properties.Settings.Default.BackgroundColor;

            _indicatorCtrl.Game = _game;
            _indicatorCtrl.Preferences = _preferences;
            _boardCtrl.Game = _game;
            _boardCtrl.Preferences = _preferences;

            _game.BallAdd += game_BallAdd;
            _game.BallDelete += game_BallDelete;
            _game.BallsDelete += game_BallsDelete;
            _game.BallMove += game_BallMove;
            _game.BallMoved += game_BallMoved;

            _game.NewGame();
            _gameTimer.Start();

            // Additional initialization from properties
            _game.OneByOneDestruction = AppSettings.Instance.OneByOneDestroy;

            _game.DestroyBallPause = 0;
        }

        public override sealed Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        private void ExtraInitializeComponent()
        {
            _boardCtrl = new BoardCtrl();
            _indicatorCtrl = new IndicatorCtrl();
            _timerLabel = new Label();
            _ballCountLabel = new Label();
            _undoLabel = new Label {Text = @"Undo", Enabled = false};
            _undoLabel.Click += undoLabel_Click;
            _enableUndo = false;

            var workingArea = Screen.PrimaryScreen.WorkingArea;
            var minDimension = workingArea.Height < workingArea.Width ? workingArea.Height : workingArea.Width;
            _indicatorCtrl.Location = new Point(0, 0);
            _indicatorCtrl.Height = minDimension * 5 / 100; // 5% of min. dimension

            _boardCtrl.Location = new Point(0, _indicatorCtrl.Height);
            _boardCtrl.CellClick += boardCtrl_CellClick;
            _boardCtrl.BallClick += boardCtrl_BallClick;

            Controls.Add(_indicatorCtrl);
            Controls.Add(_boardCtrl);
            Controls.Add(_timerLabel);
            Controls.Add(_ballCountLabel);
            Controls.Add(_undoLabel);
        }

        private void undoLabel_Click(object sender, EventArgs e)
        {
            _game.UndoMove(_boardCtrl);
            RefreshView();
            _enableUndo = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Width = _boardCtrl.Width / 2;
            Height = Height - 100;
            // Redraw complete area, there was a certain problem encountered.
            RefreshView();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // Working area
            var width = Width - 2;
            var height = Height - 5 * _menuSize - 1;

            _indicatorCtrl.Width = width;
            _boardCtrl.Size = new Size(width, height - _indicatorCtrl.Height);

            _timerLabel.Location = new Point(width / 2 - 50, _boardCtrl.Location.Y + _boardCtrl.Height + 10);
            _timerLabel.Height = 30;
            _timerLabel.Font = new Font("Microsoft Sans Serif", 20F);

            _ballCountLabel.Location = new Point(20, _boardCtrl.Location.Y + _boardCtrl.Height + 10);
            _ballCountLabel.Height = 30;
            _ballCountLabel.Width = 120;
            _ballCountLabel.Font = new Font("Microsoft Sans Serif", 20F);

            _undoLabel.Location = new Point(width - 110, _boardCtrl.Location.Y + _boardCtrl.Height + 10);
            _undoLabel.Height = 30;
            _undoLabel.Font = new Font("Microsoft Sans Serif", 20F);
        }

        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            AppSettings.Instance.Save();
            GameOver();
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            var workingArea = Screen.PrimaryScreen.WorkingArea;

            var dialog = new AboutBox(_color);
            dialog.Left = (workingArea.Width - dialog.Width) / 2;
            dialog.Top = (workingArea.Height - dialog.Height) / 2;

            dialog.ShowDialog();
        }

        private void menuItemNew_Click(object sender, EventArgs e)
        {
            GameOver();
        }

        private void menuItemRecords_Click(object sender, EventArgs e)
        {
            var workingArea = Screen.PrimaryScreen.WorkingArea;

            var dialog = new HiScoreForm(_color);
            dialog.Left = (workingArea.Width - dialog.Width) / 2;
            dialog.Top = (workingArea.Height - dialog.Height) / 2;

            dialog.UpdateRecordList(_game.Records);

            dialog.ShowDialog();
        }

        private void menuItemRefresh_Click(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void boardCtrl_CellClick(BoardCtrl ctrl, Point cell)
        {
            var ball = _game.Board.GetSelectedBall();
            if ((ball == null) || (!_game.LockBoard())) return;
            // Move ball
            if (!_game.MoveTo(cell))
            {
                // Wasn't able to move
                _game.UnlockBoard();
            }
        }

        private void boardCtrl_BallClick(BoardCtrl ctrl, BallCtrl ballCtrl)
        {
            _game.Board.SelectBall(ballCtrl.Ball);
        }

        private void game_BallAdd(Game game, Ball ball)
        {
            _boardCtrl.AddNewBall(ball);
        }

        private void game_BallDelete(Game game, Ball ball)
        {
            _boardCtrl.RemoveBall(ball);
        }

        private void game_BallsDelete(Game game, Ball[] balls)
        {
            _boardCtrl.RemoveBalls(balls);
        }

        private void game_BallMove(Game game, Ball ball, Point oldPos, Point newPos)
        {
            if (!_previousBallsSet)
            {
                game.SetPreviousBalls(ball, oldPos, newPos);
                _previousBallsSet = true;
            }

            _boardCtrl.MoveBall(ball, oldPos, newPos);
        }

        private void game_BallMoved(Game game, Ball ball)
        {
            // Diselect & Invalidate
            _boardCtrl.InvalidateCell(ball.Position);

            // Next step
            if (!game.NextStep()) return;
            _indicatorCtrl.Invalidate();

            // For optimization purposes we will invalidate only new virtual balls positions
            foreach (var t in game.VirtualBalls)
            {
                _boardCtrl.InvalidateCell(t.Position);
            }
            _enableUndo = true;
            _previousBallsSet = false;
            game.UnlockBoard();
        }

        private void ballJump_Tick(object sender, EventArgs e)
        {
            _boardCtrl.DrawJumpingBall();
        }

        private void NewGame()
        {
            _boardCtrl.RemoveAllBalls();

            _game.NewGame();
            _gameTimer.Start();
            _enableUndo = false;
            RefreshView();
        }

        private void GameOver()
        {
            _gameTimer.Stop();
            MessageBox.Show(@"Game Over!", @"Lines '98", MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            if ((_game.StepCount > 0) && _game.IsRecord)
            {
                var dialog = new InputBox(AppSettings.Instance.IsPocketPc, _color)
                {
                    Text = @"High Score",
                    Message = "What is your name?"
                };
                dialog.ShowDialog();

                _game.SaveResults(dialog.InputText, _timerLabel.Text);
            }
            NewGame();
            _gameTimer.Start();
        }

        private void RefreshView()
        {
            _boardCtrl.Invalidate();
            _indicatorCtrl.Invalidate();

            MaximumSize = Size;
            MinimumSize = Size;
        }

        private void gameOverWatchDog_Tick(object sender, EventArgs e)
        {
            _ballCountLabel.Text = @"Balls: " + _boardCtrl.BallCount;
            _undoLabel.Enabled = _enableUndo;
            if (!_game.IsGameOverEvent) return;
            GameOver();
        }

        private void _gameTimer_Tick(object sender, EventArgs e)
        {
            var minutes = Convert.ToInt32(_game.Time.Split(':')[0]);
            var seconds = Convert.ToInt32(_game.Time.Split(':')[1]);

            if (seconds + 1 == 60)
            {
                seconds = 0;
                minutes++;
            }
            else
            {
                seconds++;
            }

            string minutesString;
            if (minutes < 10)
                minutesString = "0" + minutes;
            else
                minutesString = minutes.ToString();
            string secondsString;
            if (seconds < 10)
                secondsString = "0" + seconds;
            else
                secondsString = seconds.ToString();
            _game.Time = minutesString + ":" + secondsString;

            _timerLabel.Text = _game.Time;
        }

        public override sealed Size MaximumSize
        {
            get { return base.MaximumSize; }
            set { base.MaximumSize = value; }
        }

        public override sealed Size MinimumSize
        {
            get { return base.MinimumSize; }
            set { base.MinimumSize = value; }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            var result = _gameColors.ShowDialog();
            if (result != DialogResult.OK) return;
            _color = BackColor = _gameColors.Color;
            Properties.Settings.Default.BackgroundColor = _color;
            Properties.Settings.Default.Save(); // Saves settings in application configuration file
        }
    }
}
