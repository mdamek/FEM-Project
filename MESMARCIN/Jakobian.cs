using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESMARCIN
{
    public class Jakobian
    {
        public double [,] Value { get; }
        public double Det { get; }
        public double[,] ValueT { get; }


        public Jakobian(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, int whichIntegralPoint, UniversalElement universalElement)
        {
            this.Value = CalculateValue(x1, y1, x2, y2, x3, y3, x4, y4, whichIntegralPoint, universalElement);
            this.Det = CalculateDet(this.Value);
            this.ValueT = CalculateTValue(this.Value);
        }

        public double[,] CalculateValue(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, int whichIntegralPoint, UniversalElement universalElement)
        {
            
            var value = new double[2,2];
            value[0, 0] = universalElement.dNdE[whichIntegralPoint, 0] * x1 + universalElement.dNdE[whichIntegralPoint, 1] * x2 +
                          universalElement.dNdE[whichIntegralPoint, 2] * x3 + universalElement.dNdE[whichIntegralPoint, 3] * x4;
                                            
            value[0, 1] = universalElement.dNdE[whichIntegralPoint, 0] * y1 + universalElement.dNdE[whichIntegralPoint, 1] * y2 +
                          universalElement.dNdE[whichIntegralPoint, 2] * y3 + universalElement.dNdE[whichIntegralPoint, 3] * y4;
                                             
            value[1, 0] = universalElement.dNdN[whichIntegralPoint, 0] * x1 + universalElement.dNdN[whichIntegralPoint, 1] * x2 +
                          universalElement.dNdN[whichIntegralPoint, 2] * x3 + universalElement.dNdN[whichIntegralPoint, 3] * x4;
                                        
            value[1, 1] = universalElement.dNdN[whichIntegralPoint, 0] * y1 + universalElement.dNdN[whichIntegralPoint, 1] * y2 +
                          universalElement.dNdN[whichIntegralPoint, 2] * y3 + universalElement.dNdN[whichIntegralPoint, 3] * y4;
            return value;
        }

        public double CalculateDet(double[,] value)
        {
            return value[0, 0] * value[1, 1] - value[0, 1] * value[1, 0];
        }

        public double[,] CalculateTValue(double[,] value)
        {
            var tValue = new double[2, 2];
            tValue[0, 0] = value[1, 1] / this.Det;
            tValue[1, 1] = value[0, 0] / this.Det;
            tValue[1, 0] = -value[1, 0] / this.Det;
            tValue[0, 1] = -value[0, 1] / this.Det;
            return tValue;
        }
    }
}
