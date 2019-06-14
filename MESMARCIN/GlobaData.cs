using System;
using System.IO;

namespace MESMARCIN
{
    public static class GlobalData
    {
        public static readonly double Height;
        public static readonly double Width;
        public static readonly int NodesHeightNumber;
        public static readonly int NodesLengthNumber;
        public static readonly int NodesCount;
        public static readonly int ElementsCount;
        public static readonly int NPc;
        public static readonly double InitialTemperature;
        public static readonly double AmbientTemperature;
        public static readonly double SimulationTime;
        public static readonly double Dt;
        public static readonly int Conductivity;
        public static readonly double SpecificHeat;
        public static readonly double Density;
        public static readonly double Alfa;
        private const string Path = @"C:\Users\marci\Desktop\daneDoMes.txt";

        static GlobalData()
        {
            var readFile = File.ReadAllText(Path).Split('\n');
            Height = Convert.ToDouble(readFile[0].Split('=')[1]);
            Width = Convert.ToDouble(readFile[1].Split('=')[1]);
            NodesHeightNumber = Convert.ToInt32(readFile[2].Split('=')[1]);
            NodesLengthNumber = Convert.ToInt32(readFile[3].Split('=')[1]);
            NPc = Convert.ToInt32(readFile[4].Split('=')[1]);
            InitialTemperature = Convert.ToDouble(readFile[5].Split('=')[1]);
            AmbientTemperature = Convert.ToDouble(readFile[6].Split('=')[1]);
            SimulationTime = Convert.ToDouble(readFile[7].Split('=')[1]);
            Dt = Convert.ToDouble(readFile[8].Split('=')[1]);
            Conductivity = Convert.ToInt32(readFile[9].Split('=')[1]);
            SpecificHeat = Convert.ToDouble(readFile[10].Split('=')[1]);
            Density = Convert.ToDouble(readFile[11].Split('=')[1]);
            Alfa = Convert.ToDouble(readFile[12].Split('=')[1]);
            NodesCount = NodesHeightNumber * NodesLengthNumber;
            ElementsCount = (NodesHeightNumber - 1) * (NodesLengthNumber - 1);
        }
    }
}
