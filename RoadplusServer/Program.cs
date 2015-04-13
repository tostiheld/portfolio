using System;
using Gtk;
using Roadplus.Server.UI;

using Roadplus.Server.Map;

namespace Roadplus.Server
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Vertex start = new Vertex(new Microsoft.Xna.Framework.Point(0, 0));
            Zone zone = new Zone(start);

            Microsoft.Xna.Framework.Point point = new Microsoft.Xna.Framework.Point(10, 10);
            start.Connect(point);


            /*
            Application.Init();
            MainWindow win = new MainWindow();
            win.Show();
            Application.Run();*/
        }
    }
}
