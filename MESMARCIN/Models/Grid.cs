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
        public List<int> NodesInsideChimney { get; }
        public List<int> SwitchOffBoundaryConditionElements { get; }

        public Grid()
        {
            this.NodesInsideChimney =new List<int>
            {
                36, 37, 38, 39, 40, 47, 48, 49, 50, 51, 58, 59, 60, 61,
                62, 69, 70, 71, 72, 73, 80, 81, 82, 83, 84
            };
            this.SwitchOffBoundaryConditionElements = new List<int>
            {
                22, 23, 24, 25, 26, 27, 37, 47, 57, 67, 77, 76, 75, 74, 73, 72, 32, 42, 52, 62, 72

            };
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

        private void SetInsideChimneyNodesTemperature()
        {
            

            for (var i = 0; i < Nodes.Length; i++)
            {
                if (this.NodesInsideChimney.Contains(i))
                {
                    Nodes[i].T = GlobalData.AmbientTemperature;
                }
            }
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
                        if (Nodes[k].X > 0.09 && Nodes[k].X < 0.11 && Nodes[k].Y >= 0.09 && Nodes[k].Y <= 0.41)
                            Nodes[k].IsMarginal = true;

                        if (Nodes[k].X > 0.39 && Nodes[k].X < 0.41 && Nodes[k].Y >= 0.09 && Nodes[k].Y <= 0.41)
                            Nodes[k].IsMarginal = true;

                        if (Nodes[k].Y > 0.09 && Nodes[k].Y < 0.11 && Nodes[k].X >= 0.09 && Nodes[k].X <= 0.41)
                            Nodes[k].IsMarginal = true;

                        if (Nodes[k].Y > 0.39 && Nodes[k].Y < 0.41 && Nodes[k].X >= 0.09 && Nodes[k].X <= 0.41)
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
                if (NodesInsideChimney.Contains(i))
                {
                    Nodes[i].T = GlobalData.AmbientTemperature;
                }  
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
            System.IO.DirectoryInfo di = new DirectoryInfo($"C:\\Users\\marci\\Desktop\\FilesToMatlab");

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            var i = 0;
            foreach (var snapshot in Snapshots)
            {
                using (var streamWriter = new StreamWriter($"C:\\Users\\marci\\Desktop\\FilesToMatlab\\plik" + i + ".txt"))
                {
                    //if (i == Snapshots.Count - 1)
                    //{
                    //    var nodesInsideChimney = new List<int>
                    //    {
                    //        36, 37, 38, 39, 40, 47, 48, 49, 50, 51, 58, 59, 60, 61,
                    //        62, 69, 70, 71, 72, 73, 80, 81, 82, 83, 84
                    //    };

                    //    for (var k = 0; k < Nodes.Length; k++)
                    //    {
                    //        if (nodesInsideChimney.Contains(k))
                    //        {
                    //            Snapshots[i][k] =0;
                    //        }
                    //    }
                    //}
                    for (var j = 1; j <= snapshot.Length; j++)
                    {
                        streamWriter.Write(snapshot[j - 1].ToString("0.0", CultureInfo.InvariantCulture));
                        streamWriter.Write(j % GlobalData.NodesHeightNumber != 0 ? "," : Environment.NewLine);
                    }
                }
                i++;
            }
        }
    }
}
