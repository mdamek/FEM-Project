using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace MesMarcin
{
    public class Grid
    {
        public Element[] Elements { get; }
        public Node[] Nodes { get; }
        public List<double[]> Snapshots { get; }
        public double [,] HG { get; }
        public double[,] CG { get; }
        public double [] PG { get; }

        public Grid()
        {
            this.Snapshots = new List<double[]>();
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
            this.HG = new double[GlobalData.NodesHeightNumber * GlobalData.NodesLengthNumber, GlobalData.NodesHeightNumber * GlobalData.NodesLengthNumber];
            this.CG = new double[GlobalData.NodesHeightNumber * GlobalData.NodesLengthNumber, GlobalData.NodesHeightNumber * GlobalData.NodesLengthNumber];
            this.PG = new double[GlobalData.NodesCount];
            FullNodesAndElements();

            var initialTemperatureVector = new double[Nodes.Length];
            for (var i = 0; i < initialTemperatureVector.Length; i++)
            {
                initialTemperatureVector[i] = GlobalData.InitialTemperature;
            }
            SetNodesTemperature(initialTemperatureVector);
        }

        private void FullNodesAndElements()
        {
            var deltaX = GlobalData.Width / (GlobalData.NodesLengthNumber - 1);
            var deltaY = GlobalData.Height / (GlobalData.NodesHeightNumber - 1);
            var k = 0;
            while (k < GlobalData.NodesCount)
            {
                for (var i = 0; i < GlobalData.NodesLengthNumber; i++)
                {
                    for (var j = 0; j < GlobalData.NodesHeightNumber; j++)
                    {
                        Nodes[k].IsMarginal = false;
                        Nodes[k].X = i * deltaX;
                        Nodes[k].Y = j * deltaY;
                        if (Nodes[k].X == 0 || Nodes[k].X == GlobalData.Width)
                            Nodes[k].IsMarginal = true;
                        if (Nodes[k].Y == 0 || Nodes[k].Y == GlobalData.Height)
                            Nodes[k].IsMarginal = true;
                        k++;
                    }
                }
            }
            var actualElement = 0;
            for (var i = 1; i < GlobalData.ElementsCount + GlobalData.NodesLengthNumber - 1; i++)
            {
                if (i % (GlobalData.NodesHeightNumber) == 0)
                {
                    continue;
                }
                    Elements[actualElement].Id[0] = i - 1;
                    Elements[actualElement].Id[3] = i;
                    Elements[actualElement].Id[2] = GlobalData.NodesHeightNumber + i;
                    Elements[actualElement].Id[1] = GlobalData.NodesHeightNumber + i - 1;
                    actualElement++;
            }
        }

        public void SetNodesTemperature(double [] temperatures)
        {
            for (var i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].T = temperatures[i];
            }
            MakeTemperaturesSnapshot(temperatures);
        }

        private void MakeTemperaturesSnapshot(double [] temperatures)
        {
            var temperaturesToSnapshot = new double[temperatures.Length];
            for (var i = 0; i < temperatures.Length; i++)
            {
                temperaturesToSnapshot[i] = temperatures[i];
            }
            Snapshots.Add(temperaturesToSnapshot);
        }

        public void SaveToFile()
        {
            var i = 0;
            foreach (var snapshot in Snapshots)
            {
                using (var streamWriter = new StreamWriter($"C:\\Users\\marci\\Desktop\\FilesToMatlab\\plik" + i +".txt"))
                {
                    for (var j = 1; j <= snapshot.Length; j++)
                    {
                        streamWriter.Write(snapshot[j-1].ToString("0.0", CultureInfo.InvariantCulture));
                        streamWriter.Write(j % GlobalData.NodesHeightNumber != 0 ? "," : Environment.NewLine);
                    }
                }
                i++;
            }

        }
    }
}
