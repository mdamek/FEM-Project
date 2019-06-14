using System;
using System.Runtime.CompilerServices;

namespace MESMARCIN
{
    class Program
    {
        static void Main(string[] args)
        {
            var newGrid = CalculateLocalMatrix(new Grid());
            var finalGrid = AggregateToGlobalMatrix(newGrid);
            var nextT = new double[GlobalData.NodesCount];
            for (var i = 0; i < nextT.Length; i++)
            {
                nextT[i] = finalGrid.Nodes[i].T;
            }

            var interationNumer = GlobalData.SimulationTime /GlobalData.Dt;
            for (var i = 0; i < interationNumer; i++)
            {
                var HPlusCdT = MatrixCalc.AddMatrix(finalGrid.HG, MatrixCalc.MatrixScalarMultiplication(finalGrid.CG, 1.0 / GlobalData.Dt));
                var CdT = MatrixCalc.MatrixScalarMultiplication(finalGrid.CG, 1.0 / GlobalData.Dt);
                var CdTT0 = MatrixCalc.MatrixVectorMultiplication(CdT, nextT);
                var PPlusCtDT0 = MatrixCalc.AddVectors(finalGrid.PG, CdTT0);
                nextT = EquationHelper.Solve(HPlusCdT, PPlusCtDT0);
                finalGrid.SetNodesTemperature(nextT);
                var limes = MatrixCalc.FindMinAndMax(nextT);
                Console.WriteLine("Min: " + limes.min  + " Max: " + limes.max);
            }
            Console.ReadKey();
        }

        private static Grid CalculateLocalMatrix(Grid grid)
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

                    var matrixDx = MatrixCalc.TranspositionAndMultiplication(dNdX);
                    var matrixDy = MatrixCalc.TranspositionAndMultiplication(dNdY);
                    var detAndWeights = jacobian.Det * universalElement.weightsC[0] * universalElement.weightsC[1];
                    matrixDx = MatrixCalc.MatrixScalarMultiplication(matrixDx, detAndWeights);
                    matrixDy = MatrixCalc.MatrixScalarMultiplication(matrixDy, detAndWeights);

                    var sumMatrix = MatrixCalc.AddMatrix(matrixDx, matrixDy);
                    sumMatrix = MatrixCalc.MatrixScalarMultiplication(sumMatrix, GlobalData.Conductivity);
                    elementH = MatrixCalc.AddMatrix(elementH, sumMatrix);
                    
                    for (var k = 0; k < 4; k++)
                    {
                        N[k] = universalElement.N[j, k];
                    }

                    var cRazyCT = MatrixCalc.TranspositionAndMultiplication(N);
                    var actualC = MatrixCalc.MatrixScalarMultiplication(cRazyCT, detAndWeights * GlobalData.Density * GlobalData.SpecificHeat);
                    elementC = MatrixCalc.AddMatrix(elementC, actualC);
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
                                var sideDeterminant = GlobalData.Width / (GlobalData.NodesLengthNumber - 1) * 0.5;
                                var nVector = new double[4];
                                for (var k = 0; k < 4; k++)
                                {
                                    nVector[k] = universalElement.NOutside[j, k];
                                }
                                var pcMatrix = MatrixCalc.TranspositionAndMultiplication(nVector);
                                elementToAddToH = MatrixCalc.AddMatrix(elementToAddToH,
                                MatrixCalc.MatrixScalarMultiplication(pcMatrix, sideDeterminant));
                                var pcVector = MatrixCalc.VectorScalarMultiplication(nVector, GlobalData.AmbientTemperature * sideDeterminant * GlobalData.Alfa);
                                element.PL = MatrixCalc.AddVectors(element.PL, pcVector);
                            }
                            break;
                        }
                        case 2:
                        case 3:
                        {
                            if (grid.Nodes[element.Id[1]].IsMarginal &&
                                grid.Nodes[element.Id[2]].IsMarginal)
                            {
                                var sideDeterminant = GlobalData.Height / (GlobalData.NodesHeightNumber - 1) * 0.5;
                                var nVector = new double[4];
                                for (var k = 0; k < 4; k++)
                                {
                                    nVector[k] = universalElement.NOutside[j, k];
                                }
                                var pcMatrix = MatrixCalc.TranspositionAndMultiplication(nVector);
                                elementToAddToH = MatrixCalc.AddMatrix(elementToAddToH,
                                    MatrixCalc.MatrixScalarMultiplication(pcMatrix, sideDeterminant));
                                var pcVector = MatrixCalc.VectorScalarMultiplication(nVector, GlobalData.AmbientTemperature * sideDeterminant * GlobalData.Alfa);
                                element.PL = MatrixCalc.AddVectors(element.PL, pcVector);
                            }
                            break;
                        }
                        case 4:
                        case 5:
                        {
                            if (grid.Nodes[element.Id[2]].IsMarginal &&
                                grid.Nodes[element.Id[3]].IsMarginal)
                            {
                                var sideDeterminant = GlobalData.Width / (GlobalData.NodesLengthNumber - 1) * 0.5;
                                var nVector = new double[4];
                                for (var k = 0; k < 4; k++)
                                {
                                    nVector[k] = universalElement.NOutside[j, k];
                                }
                                var pcMatrix = MatrixCalc.TranspositionAndMultiplication(nVector);
                                elementToAddToH = MatrixCalc.AddMatrix(elementToAddToH,
                                    MatrixCalc.MatrixScalarMultiplication(pcMatrix, sideDeterminant));
                                var pcVector = MatrixCalc.VectorScalarMultiplication(nVector, GlobalData.AmbientTemperature * sideDeterminant * GlobalData.Alfa);
                                element.PL = MatrixCalc.AddVectors(element.PL, pcVector);
                            }
                            break;
                        }
                        case 6:
                        case 7:
                        {
                            if (grid.Nodes[element.Id[3]].IsMarginal &&
                                grid.Nodes[element.Id[0]].IsMarginal)
                            {
                                var sideDeterminant = GlobalData.Height / (GlobalData.NodesHeightNumber - 1) * 0.5;
                                var nVector = new double[4];
                                for (var k = 0; k < 4; k++)
                                {
                                    nVector[k] = universalElement.NOutside[j, k];
                                }
                                var pcMatrix = MatrixCalc.TranspositionAndMultiplication(nVector);
                                elementToAddToH = MatrixCalc.AddMatrix(elementToAddToH,
                                    MatrixCalc.MatrixScalarMultiplication(pcMatrix, sideDeterminant));
                                var pcVector = MatrixCalc.VectorScalarMultiplication(nVector, GlobalData.AmbientTemperature * sideDeterminant * GlobalData.Alfa);
                                element.PL = MatrixCalc.AddVectors(element.PL, pcVector);
                            }
                            break;
                        }
                    }
                }

                elementToAddToH = MatrixCalc.MatrixScalarMultiplication(elementToAddToH, GlobalData.Alfa);
                elementH = MatrixCalc.AddMatrix(elementH, elementToAddToH);
                element.HL = elementH;
                element.CL = elementC;
            }

           

            return grid;
        }

        private static Grid AggregateToGlobalMatrix(Grid grid)
        {
            foreach (var actualElement in grid.Elements)
            {
                for (var i = 0; i < 4; i++)
                {
                    for (var j = 0; j < 4; j++)
                    {
                        grid.HG[actualElement.Id[i], actualElement.Id[j]] += actualElement.HL[i, j];
                        grid.CG[actualElement.Id[i], actualElement.Id[j]] += actualElement.CL[i, j];
                    }
                    grid.PG[actualElement.Id[i]] += actualElement.PL[i];
                }
            }
            return grid;
        }

    }
}
