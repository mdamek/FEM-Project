using System;
using System.Security.AccessControl;

namespace MesMarcin
{
    public class UniversalElement
    {
        public double[] wspC { get; }
        public double[] weightsC { get; }
        public double[,] dNdE { get; }
        public double[,] dNdN { get; }
        public double[,] N { get; }
        public double[,] NOutside { get; }
        public int nN { get; }

        public UniversalElement()
        {
            this.nN = 4;
            this.wspC = new double[GlobalData.NPc];
            this.weightsC = new double[GlobalData.NPc];
            this.dNdE = new double[GlobalData.NPc * GlobalData.NPc, nN];
            this.dNdN = new double[GlobalData.NPc * GlobalData.NPc, nN];
            this.N = new double[GlobalData.NPc * GlobalData.NPc, nN];
            this.NOutside = new double[8,nN];
            this.SetUpWspCAndWeightsC();
            this.SetUpdNdE();
            this.SetUpdNdN();
            this.SetUpN();
            this.SetUpNOutside();
        }

        private void SetUpN()
        {
            var math = new DifferentialFormulas();
            switch (GlobalData.NPc)
            {
                case 2:
                {
                    for (var i = 0; i < nN; i++)
                    {
                        N[0, i] = math.NFormulas[i].Invoke(wspC[0], wspC[0]);
                        N[1, i] = math.NFormulas[i].Invoke(wspC[1], wspC[0]);
                        N[2, i] = math.NFormulas[i].Invoke(wspC[1], wspC[1]);
                        N[3, i] = math.NFormulas[i].Invoke(wspC[0], wspC[1]);
                    }
                    break;
                }
                case 3:
                {
                    for (var i = 0; i < nN; i++)
                    {
                        N[0, i] = math.NFormulas[i].Invoke(wspC[0],wspC[0]);
                        N[1, i] = math.NFormulas[i].Invoke(wspC[1],wspC[0]);
                        N[2, i] = math.NFormulas[i].Invoke(wspC[2],wspC[0]);
                        N[3, i] = math.NFormulas[i].Invoke(wspC[2],wspC[1]);
                        N[4, i] = math.NFormulas[i].Invoke(wspC[2],wspC[2]);
                        N[5, i] = math.NFormulas[i].Invoke(wspC[1],wspC[2]);
                        N[6, i] = math.NFormulas[i].Invoke(wspC[0],wspC[2]);
                        N[7, i] = math.NFormulas[i].Invoke(wspC[0],wspC[1]);
                        N[8, i] = math.NFormulas[i].Invoke(wspC[1], wspC[1]);
                    }
                    break;
                }
            }
        }

        private void SetUpdNdN()
        {
            var math = new DifferentialFormulas();
            switch (GlobalData.NPc)
            {
                case 2:
                {
                    for (var i = 0; i < nN; i++)
                    {
                        dNdN[0, i] = math.dNFormulas[i].Invoke(wspC[0]);
                        dNdN[1, i] = math.dNFormulas[i].Invoke(wspC[0]);
                        dNdN[2, i] = math.dNFormulas[i].Invoke(wspC[1]);
                        dNdN[3, i] = math.dNFormulas[i].Invoke(wspC[1]);
                    }
                    break;
                }
                case 3:
                {
                    for (var i = 0; i < nN; i++)
                    {
                        dNdN[0, i] = math.dNFormulas[i].Invoke(wspC[0]);
                        dNdN[1, i] = math.dNFormulas[i].Invoke(wspC[0]);
                        dNdN[2, i] = math.dNFormulas[i].Invoke(wspC[0]);
                        dNdN[3, i] = math.dNFormulas[i].Invoke(wspC[1]);
                        dNdN[4, i] = math.dNFormulas[i].Invoke(wspC[2]);
                        dNdN[5, i] = math.dNFormulas[i].Invoke(wspC[2]);
                        dNdN[6, i] = math.dNFormulas[i].Invoke(wspC[2]);
                        dNdN[7, i] = math.dNFormulas[i].Invoke(wspC[1]);
                        dNdN[8, i] = math.dNFormulas[i].Invoke(wspC[1]);
                    }
                    break;
                }
            }
        }

        private void SetUpdNdE()
        {
            var math = new DifferentialFormulas();
            switch (GlobalData.NPc)
            {
                case 2:
                {
                    for (var i = 0; i < nN; i++)
                    {
                        dNdE[0, i] = math.dEFormulas[i].Invoke(wspC[0]);
                        dNdE[1, i] = math.dEFormulas[i].Invoke(wspC[0]);
                        dNdE[2, i] = math.dEFormulas[i].Invoke(wspC[1]);
                        dNdE[3, i] = math.dEFormulas[i].Invoke(wspC[1]);
                    }
                    break;
                }
                case 3:
                {
                    for (var i = 0; i < nN; i++)
                    {
                        dNdE[0, i] = math.dEFormulas[i].Invoke(wspC[0]);
                        dNdE[1, i] = math.dEFormulas[i].Invoke(wspC[0]);
                        dNdE[2, i] = math.dEFormulas[i].Invoke(wspC[0]);
                        dNdE[3, i] = math.dEFormulas[i].Invoke(wspC[1]);
                        dNdE[4, i] = math.dEFormulas[i].Invoke(wspC[2]);
                        dNdE[5, i] = math.dEFormulas[i].Invoke(wspC[2]);
                        dNdE[6, i] = math.dEFormulas[i].Invoke(wspC[2]);
                        dNdE[7, i] = math.dEFormulas[i].Invoke(wspC[1]);
                        dNdE[8, i] = math.dEFormulas[i].Invoke(wspC[1]);
                    }
                    break;
                }
            }
        }

        private void SetUpWspCAndWeightsC()
        {
            switch (GlobalData.NPc)
            {
                case 2:
                    this.wspC[0] = -1/ Math.Sqrt(3);
                    this.wspC[1] = 1 / Math.Sqrt(3);
                    this.weightsC[0] = 1;
                    this.weightsC[1] = 1;
                    break;
                case 3:
                    this.wspC[0] = -0.77;
                    this.wspC[1] = 0;
                    this.wspC[2] = 0.77;
                    this.weightsC[0] = 5 / 9;
                    this.weightsC[1] = 8 / 9;
                    this.weightsC[2] = 5 / 9;
                    break;
            }
        }

        private void SetUpNOutside()
        {
            var math = new DifferentialFormulas();
            switch (GlobalData.NPc)
            {
                case 2:
                {
                    for (var i = 0; i < 4; i++)
                    {
                        NOutside[0, i] = math.NFormulas[i].Invoke(wspC[0], -1);
                        NOutside[1, i] = math.NFormulas[i].Invoke(wspC[1], -1);
                        NOutside[2, i] = math.NFormulas[i].Invoke(1, wspC[0]);
                        NOutside[3, i] = math.NFormulas[i].Invoke(1, wspC[1]);
                        NOutside[4, i] = math.NFormulas[i].Invoke(wspC[1], 1);
                        NOutside[5, i] = math.NFormulas[i].Invoke(wspC[0], 1);
                        NOutside[6, i] = math.NFormulas[i].Invoke(-1, wspC[1]);
                        NOutside[7, i] = math.NFormulas[i].Invoke(-1, wspC[0]);
                        }
                    break;
                }
                case 3:
                {
                    //for (var i = 0; i < nN; i++)
                    //{
                    //    N[0, i] = math.NFormulas[i].Invoke(wspC[0], wspC[0]);
                    //    N[1, i] = math.NFormulas[i].Invoke(wspC[1], wspC[0]);
                    //    N[2, i] = math.NFormulas[i].Invoke(wspC[2], wspC[0]);
                    //    N[3, i] = math.NFormulas[i].Invoke(wspC[2], wspC[1]);
                    //    N[4, i] = math.NFormulas[i].Invoke(wspC[2], wspC[2]);
                    //    N[5, i] = math.NFormulas[i].Invoke(wspC[1], wspC[2]);
                    //    N[6, i] = math.NFormulas[i].Invoke(wspC[0], wspC[2]);
                    //    N[7, i] = math.NFormulas[i].Invoke(wspC[0], wspC[1]);
                    //    N[8, i] = math.NFormulas[i].Invoke(wspC[1], wspC[1]);
                    //}
                    break;
                }
            }
        }
    }
}
