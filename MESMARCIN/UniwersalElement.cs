namespace MESMARCIN
{
    public class UniwersalElement
    {
        public double[] wspC { get; set; }
        public double[] weightsC { get; set; }
        public double[,] dNdE { get; set; }
        public double[,] dNdN { get; set; }
        public double[,] N { get; set; }

        public UniwersalElement()
        {
            int nN = 4;
            this.wspC = new double[GlobalData.nPc];
            this.weightsC = new double[GlobalData.nPc];
            this.dNdE = new double[GlobalData.nPc * GlobalData.nPc, nN];
            this.dNdN = new double[GlobalData.nPc * GlobalData.nPc, nN];
            this.N = new double[GlobalData.nPc * GlobalData.nPc, nN];
            this.SetUpWspCAndWeightsC();
            this.SetUpdNdE();
        }

        public void SetUpWspCAndWeightsC()
        {
            if(GlobalData.nPc == 2)
            {
                this.wspC[0] = - 0.577;
                this.wspC[1] = 0.577;
                this.weightsC[0] = 0;
                this.weightsC[1] = 0;
            }
            else if (GlobalData.nPc == 3)
            {
                this.wspC[0] = -0.77;
                this.wspC[1] = 0;
                this.wspC[2] = 0.77;
                this.weightsC[0] = 5 / 9;
                this.weightsC[1] = 8 / 9;
                this.weightsC[2] = 5 / 9;
            }
        }
        public void SetUpdNdE()
        {

        }
    }
}
