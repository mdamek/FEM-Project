using System;

namespace MesMarcin
{
    public static class MesProgram
    {
        static void Main()
        {
            var grid = Aggregator.AggregateToGlobalMatrix(GenerateAllLocalMatricesForGrid(new Grid()));
            var nextT = new double[GlobalData.NodesCount];
            for (var i = 0; i < nextT.Length; i++)
            {
                nextT[i] = grid.Nodes[i].T;
            }

            for (var i = 0; i < GlobalData.SimulationTime / GlobalData.Dt; i++)
            {
                var HPlusCdT = MatrixCalculator.AddMatrix(grid.HG, MatrixCalculator.MatrixScalarMultiplication(grid.CG, 1.0 / GlobalData.Dt));
                var CdT = MatrixCalculator.MatrixScalarMultiplication(grid.CG, 1.0 / GlobalData.Dt);
                var CdTT0 = MatrixCalculator.MatrixVectorMultiplication(CdT, nextT);
                var PPlusCtDT0 = MatrixCalculator.AddVectors(grid.PG, CdTT0);
                nextT = EquationHelper.Solve(HPlusCdT, PPlusCtDT0);
                grid.SetNodesTemperature(nextT);
                var limes = MatrixCalculator.FindMinAndMax(nextT);
                Console.WriteLine("Min: " + limes.min  + " Max: " + limes.max);
            }
            grid.SaveToFile();
        }

        private static Grid GenerateAllLocalMatricesForGrid(Grid grid)
        {
            var universalElement = new UniversalElement();
            foreach (var element in grid.Elements)
            {
                var x1 = grid.Nodes[element.Id[0]].X;
                var y1 = grid.Nodes[element.Id[0]].Y;
                var x2 = grid.Nodes[element.Id[1]].X;
                var y2 = grid.Nodes[element.Id[1]].Y;
                var x3 = grid.Nodes[element.Id[2]].X;
                var y3 = grid.Nodes[element.Id[2]].Y;
                var x4 = grid.Nodes[element.Id[3]].X;
                var y4 = grid.Nodes[element.Id[3]].Y;
                var elementH = new double[,]
                {
                    {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}
                };
                var elementC = new double[,]
                {
                    {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}
                };

                for (var j = 0; j < GlobalData.NPc * GlobalData.NPc; j++)
                {
                    var jacobian = new Jacobian(x1, y1, x2, y2, x3, y3, x4, y4, j, universalElement);

                    var dNdX = new double[4];
                    var dNdY = new double[4];
                    var N = new double[4];

                    for (var k = 0; k < 4; k++)
                    {
                        dNdX[k] = jacobian.ValueT[0, 0] * universalElement.dNdE[j, k] +
                                  jacobian.ValueT[0, 1] * universalElement.dNdN[j, k];
                    }
                    for (var k = 0; k < 4; k++)
                    {
                        dNdY[k] = jacobian.ValueT[1, 0] * universalElement.dNdE[j, k] +
                                  jacobian.ValueT[1, 1] * universalElement.dNdN[j, k];
                    }

                    var matrixDx = MatrixCalculator.TranspositionAndMultiplication(dNdX);
                    var matrixDy = MatrixCalculator.TranspositionAndMultiplication(dNdY);
                    var detAndWeights = jacobian.Det * universalElement.weightsC[0] * universalElement.weightsC[1];
                    matrixDx = MatrixCalculator.MatrixScalarMultiplication(matrixDx, detAndWeights);
                    matrixDy = MatrixCalculator.MatrixScalarMultiplication(matrixDy, detAndWeights);

                    var sumMatrix = MatrixCalculator.AddMatrix(matrixDx, matrixDy);
                    sumMatrix = MatrixCalculator.MatrixScalarMultiplication(sumMatrix, GlobalData.Conductivity);
                    elementH = MatrixCalculator.AddMatrix(elementH, sumMatrix);
                    
                    for (var k = 0; k < 4; k++)
                    {
                        N[k] = universalElement.N[j, k];
                    }

                    var cRazyCT = MatrixCalculator.TranspositionAndMultiplication(N);
                    var actualC = MatrixCalculator.MatrixScalarMultiplication(cRazyCT, detAndWeights * GlobalData.Density * GlobalData.SpecificHeat);
                    elementC = MatrixCalculator.AddMatrix(elementC, actualC);
                }

                var elementToAddToH = new double[,]
                {
                    {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}
                };
                for (var j = 0; j < 8; j++)
                {
                    switch (j)
                    {
                        case 0:
                        case 1:
                        {
                            if (grid.Nodes[element.Id[0]].IsMarginal &&
                                grid.Nodes[element.Id[1]].IsMarginal)
                            {
                                elementToAddToH = SideCalculations(universalElement, j, elementToAddToH, element, Side.Width);
                            }
                            break;
                        }
                        case 2:
                        case 3:
                        {
                            if (grid.Nodes[element.Id[1]].IsMarginal &&
                                grid.Nodes[element.Id[2]].IsMarginal)
                            {
                                elementToAddToH = SideCalculations(universalElement, j, elementToAddToH, element, Side.Height);
                            }
                            break;
                        }
                        case 4:
                        case 5:
                        {
                            if (grid.Nodes[element.Id[2]].IsMarginal &&
                                grid.Nodes[element.Id[3]].IsMarginal)
                            {
                                elementToAddToH = SideCalculations(universalElement, j, elementToAddToH, element, Side.Width);
                            }
                            break;
                        }
                        case 6:
                        case 7:
                        {
                            if (grid.Nodes[element.Id[3]].IsMarginal &&
                                grid.Nodes[element.Id[0]].IsMarginal)
                            {
                                elementToAddToH = SideCalculations(universalElement, j, elementToAddToH, element, Side.Height);
                            }
                            break;
                        }
                    }
                }

                elementToAddToH = MatrixCalculator.MatrixScalarMultiplication(elementToAddToH, GlobalData.Alfa);
                elementH = MatrixCalculator.AddMatrix(elementH, elementToAddToH);
                element.HL = elementH;
                element.CL = elementC;
            }
            return grid;
        }

        private static double[,] SideCalculations(UniversalElement universalElement, int j, double[,] elementToAddToH, Element element, Side side)
        {
            double sideDeterminant;
            if (side == Side.Width)
            {
                sideDeterminant = GlobalData.Width / (GlobalData.NodesLengthNumber - 1) * 0.5;
            }
            else
            {
                sideDeterminant = GlobalData.Height / (GlobalData.NodesHeightNumber - 1) * 0.5;
            }
            var nVector = new double[4];
            for (var k = 0; k < 4; k++)
            {
                nVector[k] = universalElement.NOutside[j, k];
            }

            var pcMatrix = MatrixCalculator.TranspositionAndMultiplication(nVector);
            elementToAddToH = MatrixCalculator.AddMatrix(elementToAddToH, MatrixCalculator.MatrixScalarMultiplication(pcMatrix, sideDeterminant));
            var pcVector =MatrixCalculator.VectorScalarMultiplication(nVector, GlobalData.AmbientTemperature * sideDeterminant * GlobalData.Alfa);
            element.PL = MatrixCalculator.AddVectors(element.PL, pcVector);
            return elementToAddToH;
        }
    }
}
