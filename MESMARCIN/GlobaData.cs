using System;
using System.IO;

namespace MESMARCIN
{
    public static class GlobalData
    {
        public static readonly double H;
        public static readonly double L;
        public static readonly int mH;
        public static readonly int mL;
        public static readonly int NodesCount;
        public static readonly int ElementsCount;
        private const string Path = @"C:\Users\marci\Desktop\daneDoMes.txt";

        static GlobalData()
        {
            var readFile = File.ReadAllText(Path).Split('\n');
            H = Convert.ToDouble(readFile[0].Split('=')[1]);
            L = Convert.ToDouble(readFile[1].Split('=')[1]);
            mH = Convert.ToInt32(readFile[2].Split('=')[1]);
            mL = Convert.ToInt32(readFile[3].Split('=')[1]);
            NodesCount = mH * mL;
            ElementsCount = (mH - 1) * (mH - 1);
        }
    }
}
