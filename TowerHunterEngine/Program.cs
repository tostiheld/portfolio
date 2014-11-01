#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows.Forms;
#endregion

namespace TowerHunterEngine
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using (Engine game = new Engine())
            {
                Application.EnableVisualStyles();
                Form gameForm = (Form)Form.FromHandle(game.Window.Handle);
                gameForm.FormBorderStyle = FormBorderStyle.None;
                gameForm.Location = Properties.Settings.Default.Position;
                gameForm.Size = new System.Drawing.Size(
                    Properties.Settings.Default.Resolution.X,
                    Properties.Settings.Default.Resolution.Y);

                game.Run();
            }
        }
    }
#endif
}
