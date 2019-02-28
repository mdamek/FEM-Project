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

        public void ShowElementsCornerValues()
        {
            Console.WriteLine("Elements");
            Console.WriteLine(ShowElement(Elements[GlobalData.mH - 2]) + "---------" + ShowElement(Elements[GlobalData.ElementsCount - 1]));
            Console.WriteLine(ShowElement(Elements[0]) + "---------" + ShowElement(Elements[GlobalData.ElementsCount - (GlobalData.mH - 1)]));
        }

        private string ShowElement(Element element)
        {
            return  element.Id[0] +"|"+ element.Id[1] + "|" + element.Id[2] + "|" + element.Id[3];
        }
    }
}
