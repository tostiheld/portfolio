using System;
using Server.Simulation;

namespace Server
{
    class Program
    {
        static void Main()
        {
            using (Engine e = new Engine())
            {
                e.Run();
            }
        }
    }
}
