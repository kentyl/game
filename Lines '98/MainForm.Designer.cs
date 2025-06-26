using System.ComponentModel;
using System.Windows.Forms;

namespace Lines98
{
    partial class MainForm
    {
        private MainMenu mainMenu;
        private MenuItem menuItemNew;
        private MenuItem menuItemExit;
        private MenuItem menuItemHelp;
        private MenuItem menuItemAbout;
        private MenuItem menuItemDelimiter1;
        private MenuItem menuItemRecords;
        private MenuItem menuItemGame;
        private MenuItem menuItemOptions;
        private MenuItem menuItemRefresh;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemGame = new System.Windows.Forms.MenuItem();
            this.menuItemNew = new System.Windows.Forms.MenuItem();
            this.menuItemRecords = new System.Windows.Forms.MenuItem();
            this.menuItemDelimiter1 = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.menuItemOptions = new System.Windows.Forms.MenuItem();
            this.menuItemRefresh = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemHelp = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this._ballJump = new System.Windows.Forms.Timer(this.components);
            this._gameOverWatchDog = new System.Windows.Forms.Timer(this.components);
            this._gameTimer = new System.Windows.Forms.Timer(this.components);
            this._gameColors = new System.Windows.Forms.ColorDialog();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemGame,
            this.menuItemOptions,
            this.menuItemHelp});
            // 
            // menuItemGame
            // 
            this.menuItemGame.Index = 0;
            this.menuItemGame.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemNew,
            this.menuItemRecords,
            this.menuItemDelimiter1,
            this.menuItemExit});
            this.menuItemGame.Text = "&Game";
            // 
            // menuItemNew
            // 
            this.menuItemNew.Index = 0;
            this.menuItemNew.Text = "&New game";
            this.menuItemNew.Click += new System.EventHandler(this.menuItemNew_Click);
            // 
            // menuItemRecords
            // 
            this.menuItemRecords.Index = 1;
            this.menuItemRecords.Text = "&High Scores";
            this.menuItemRecords.Click += new System.EventHandler(this.menuItemRecords_Click);
            // 
            // menuItemDelimiter1
            // 
            this.menuItemDelimiter1.Index = 2;
            this.menuItemDelimiter1.Text = "-";
            // 
            // menuItemExit
            // 
            this.menuItemExit.Index = 3;
            this.menuItemExit.Text = "E&xit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuItemOptions
            // 
            this.menuItemOptions.Index = 1;
            this.menuItemOptions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemRefresh,
            this.menuItem1});
            this.menuItemOptions.Text = "&Options";
            // 
            // menuItemRefresh
            // 
            this.menuItemRefresh.Index = 0;
            this.menuItemRefresh.Text = "&Refresh view";
            this.menuItemRefresh.Click += new System.EventHandler(this.menuItemRefresh_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.Text = "&Change colour";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.Index = 2;
            this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAbout});
            this.menuItemHelp.Text = "&Help";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Index = 0;
            this.menuItemAbout.Text = "&About";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // _ballJump
            // 
            this._ballJump.Enabled = true;
            this._ballJump.Tick += new System.EventHandler(this.ballJump_Tick);
            // 
            // _gameOverWatchDog
            // 
            this._gameOverWatchDog.Enabled = true;
            this._gameOverWatchDog.Tick += new System.EventHandler(this.gameOverWatchDog_Tick);
            // 
            // _gameTimer
            // 
            this._gameTimer.Interval = 1000;
            this._gameTimer.Tick += new System.EventHandler(this._gameTimer_Tick);
            // 
            // MainForm
            // 
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(442, 804);
            this.MaximizeBox = false;
            this.Menu = this.mainMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lines \'98";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private Timer _gameTimer;
        private MenuItem menuItem1;
        private ColorDialog _gameColors;
    }
}

