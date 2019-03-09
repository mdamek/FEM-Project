using System;

namespace MESMARCIN
{
    class Program
    {
        static void Main(string[] args)
        {
            var grid = new Grid();
            grid.ShowNodesCornerValues();
            grid.ShowElementsCornerValues();
            Console.ReadKey();
        }
    }
}
