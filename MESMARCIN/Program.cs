using System;

namespace MESMARCIN
{
    class Program
    {
        static void Main(string[] args)
        {
            var grid = new Grid();
            var deltaX = GlobalData.H / (GlobalData.mH - 1);
            var deltaY = GlobalData.L / (GlobalData.mL - 1);
            var k = 0;
            while(k < GlobalData.NodesCount)
            {
                for (var i = 0; i < GlobalData.mL; i++)
                {
                    for (var j = 0; j < GlobalData.mH; j++)
                    {
                        grid.Nodes[k].X = i * deltaX;
                        grid.Nodes[k].Y = j * deltaY;
                        k++;
                    }
                }
            }
            var actualElement = 0;
            for (var i = 1; i < GlobalData.ElementsCount + GlobalData.mL - 1; i++)
            {
                if (i % (GlobalData.mH) == 0)
                {
                    continue;
                }
                grid.Elements[actualElement].Id[0] = i - 1;
                grid.Elements[actualElement].Id[1] = i;
                grid.Elements[actualElement].Id[2] = GlobalData.mH + i;
                grid.Elements[actualElement].Id[3] = GlobalData.mH + i - 1;
                actualElement++;
            }
            grid.ShowNodesCornerValues();
            grid.ShowElementsCornerValues();
            Console.ReadKey();
        }
    }
}
