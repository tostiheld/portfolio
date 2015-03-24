using System;
using Server.Simulation;

namespace Server
{
    class Program
    {
        static void Main()
        {
            using (Engine e = new Engine(new Zone()))
            {
                e.Run();
            }
        }
    }
}
