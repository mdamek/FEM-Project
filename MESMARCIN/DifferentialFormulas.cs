using System;
using System.Collections.Generic;

namespace MESMARCIN
{
    public class DifferentialFormulas
    {
        public readonly List<Func<double,double>> dEFormulas = new List<Func<double, double>>()
        {
            n => -0.25 * (1 - n),
            n => 0.25 * (1 - n),
            n => 0.25 * (1 + n),
            n => -0.25 * (1 + n)
        };

        public readonly List<Func<double, double>> dNFormulas = new List<Func<double, double>>()
        {
            e => -0.25 * (1 - e),
            e => -0.25 * (1 + e),
            e => 0.25 * (1 + e),
            e => 0.25 * (1 - e)
        };

        public readonly List<Func<double, double, double>> NFormulas = new List<Func<double, double, double>>()
        {
            (e, n) => 0.25 * (1 - e) * (1-n),
            (e, n) => 0.25 * (1 + e) * (1-n),
            (e, n) => 0.25 * (1 + e) * (1+n),
            (e, n) => 0.25 * (1 - e) * (1+n)
        };
    }
}
