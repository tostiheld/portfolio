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
            
            int x;
            int y;
            System.Drawing.Point p = new System.Drawing.Point(1280, 0);

            if (args.Length > 0)
            {
                if (Int32.TryParse(args[0], out x) && Int32.TryParse(args[1], out y))
                {
                    p = new System.Drawing.Point(x, y);
                }
            }

            //using (Application a = )

            using (Engine game = new Engine())
            {
                Application.EnableVisualStyles();
                Form gameForm = (Form)Form.FromHandle(game.Window.Handle);
                gameForm.FormBorderStyle = FormBorderStyle.None;
                gameForm.Location = p;
                gameForm.Size = new System.Drawing.Size(1024, 768);

                game.Run();
                
            }
        }
    }
#endif
}
