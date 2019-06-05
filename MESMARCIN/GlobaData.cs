using System;
using System.IO;

namespace MESMARCIN
{
    public static class GlobalData
    {
        public static readonly double Height;
        public static readonly double Length;
        public static readonly int NodesHeightNumber; // ilosc nodow
        public static readonly int NodesLengthNumber; // ilosc nodow
        public static readonly int NodesCount;
        public static readonly int ElementsCount;
        public static readonly int nPc; // schemat calkowania
        public static readonly double InitialTemperature;
        public static readonly double AmbientTemperature;
        public static readonly double SimulationTime;
        private const string Path = @"C:\Users\marci\Desktop\daneDoMes.txt";

        static GlobalData()
        {
            var readFile = File.ReadAllText(Path).Split('\n');
            Height = Convert.ToDouble(readFile[0].Split('=')[1]);
            Length = Convert.ToDouble(readFile[1].Split('=')[1]);
            NodesHeightNumber = Convert.ToInt32(readFile[2].Split('=')[1]);
            NodesLengthNumber = Convert.ToInt32(readFile[3].Split('=')[1]);
            nPc = Convert.ToInt32(readFile[4].Split('=')[1]);
            InitialTemperature = Convert.ToDouble(readFile[5].Split('=')[1]);
            AmbientTemperature = Convert.ToDouble(readFile[6].Split('=')[1]);
            SimulationTime = Convert.ToDouble(readFile[7].Split('=')[1]);
            NodesCount = NodesHeightNumber * NodesLengthNumber;
            ElementsCount = (NodesHeightNumber - 1) * (NodesLengthNumber - 1);
        }
    }
}
