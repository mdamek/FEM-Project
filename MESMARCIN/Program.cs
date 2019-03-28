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

            var universalElement = new UniversalElement();
            Console.ReadKey();
        }
    }
}
