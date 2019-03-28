using System;

namespace MESMARCIN
{
    public class Grid
    {
        public Element[] Elements { get; set; }
        public Node[] Nodes { get; set; }

        public Grid()
        {
            this.Elements = new Element[GlobalData.ElementsCount];
            this.Nodes = new Node[GlobalData.NodesCount];
            for (var i = 0; i < GlobalData.ElementsCount; i++)
            {
                this.Elements[i] = new Element();
            }

            for (var i = 0; i < GlobalData.NodesCount; i++)
            {
                this.Nodes[i] = new Node();
            }

            FullNodesAndElements();
        }

        private void FullNodesAndElements()
        {
            var deltaX = GlobalData.L / (GlobalData.mL - 1);
            var deltaY = GlobalData.H / (GlobalData.mH - 1);
            var k = 0;
            while (k < GlobalData.NodesCount)
            {
                for (var i = 0; i < GlobalData.mL; i++)
                {
                    for (var j = 0; j < GlobalData.mH; j++)
                    {
                        Nodes[k].X = i * deltaX;
                        Nodes[k].Y = j * deltaY;
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
                    Elements[actualElement].Id[0] = i - 1;
                    Elements[actualElement].Id[3] = i;
                    Elements[actualElement].Id[2] = GlobalData.mH + i;
                    Elements[actualElement].Id[1] = GlobalData.mH + i - 1;
                    actualElement++;
            }
        }

        public void ShowNodesCornerValues()
        {
            Console.WriteLine("Nodes");
            Console.WriteLine("X|Y");
            Console.WriteLine(Nodes[GlobalData.mH - 1].X + "|" + Nodes[GlobalData.mH - 1].Y + "---------" +
                              Nodes[GlobalData.NodesCount - 1].X + "|" + Nodes[GlobalData.NodesCount - 1].Y);
            Console.WriteLine(Nodes[0].X + "|" + Nodes[0].Y + "---------" +
                              Nodes[GlobalData.NodesCount - GlobalData.mH].X + "|" +
                              Nodes[GlobalData.NodesCount - GlobalData.mH].Y);
        }

        public void ShowElementsCornerValues(int index)
        {
            //Console.WriteLine("Elements");
            //Console.WriteLine(ShowElement(Elements[GlobalData.mH - 2]) + "---------" + ShowElement(Elements[GlobalData.ElementsCount - 1]));
            //Console.WriteLine(ShowElement(Elements[0]) + "---------" + ShowElement(Elements[GlobalData.ElementsCount - (GlobalData.mH - 1)]));
            Console.WriteLine(Nodes[Elements[index].Id[0]].X);
            Console.WriteLine(Nodes[Elements[index].Id[0]].Y);
            Console.WriteLine(Nodes[Elements[index].Id[1]].X);
            Console.WriteLine(Nodes[Elements[index].Id[1]].Y);
            Console.WriteLine(Nodes[Elements[index].Id[2]].X);
            Console.WriteLine(Nodes[Elements[index].Id[2]].Y);
            Console.WriteLine(Nodes[Elements[index].Id[3]].X);
            Console.WriteLine(Nodes[Elements[index].Id[3]].Y);




        }

        private string ShowElement(Element element)
        {
            return  element.Id[0] +"|"+ element.Id[1] + "|" + element.Id[2] + "|" + element.Id[3];
        }
    }
}
