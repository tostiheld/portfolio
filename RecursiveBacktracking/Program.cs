using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveBacktracking
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] data = new int[10, 10];
            Grid grid = new Grid(10, 10);
            grid.CarvePassagesFrom(0, 0, ref data);
            grid.Print(data);

            Console.ReadKey();
        }
    }
}
