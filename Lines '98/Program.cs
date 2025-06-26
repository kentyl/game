using System;
using System.Windows.Forms;

namespace Lines98
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var workingArea = Screen.PrimaryScreen.WorkingArea;

            var mainForm = new MainForm {Bounds = workingArea};

            Application.Run(mainForm);
        }
    }
}
