namespace MESMARCIN
{
    public static class EquationHelper
    {
        public static double[] Solve(double[,] matrix, double[] vector)
        {
            var n = vector.Length;
            var x = new double[n];
            var tmpA = new double[n, n + 1];
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    tmpA[i, j] = matrix[i, j];
                }
                tmpA[i, n] = vector[i];
            }
            double tmp;
            for (var k = 0; k < n - 1; k++)
            {
                for (var i = k + 1; i < n; i++)
                {
                    tmp = tmpA[i, k] / tmpA[k, k];
                    for (var j = k; j < n + 1; j++)
                    {
                        tmpA[i, j] -= tmp * tmpA[k, j];
                    }
                }
            }
            for (var k = n - 1; k >= 0; k--)
            {
                tmp = 0;
                for (var j = k + 1; j < n; j++)
                {
                    tmp += tmpA[k, j] * x[j];
                }
                x[k] = (tmpA[k, n] - tmp) / tmpA[k, k];
            }
            return x;
        }
    }
}
