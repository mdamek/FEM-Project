using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESMARCIN
{
    public static class MatrixHelper
    {
        public static double[,] TranspositionAndMultipication(double[] vector)
        {
            var matrix = new double[4, 4];
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    matrix[i, j] = vector[i] * vector[j];
                }
            }
            return matrix;
        }

        public static double[,] AddMatrix(double[,] matrix1, double[,] matrix2)
        {
            var answerMatrix = new double[4,4];
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    answerMatrix[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }
            return answerMatrix;
        }

        public static double[,] MatrixScalarMultiplication(double[,] matrix, double scalar)
        {
            var answerMatrix = new double[4, 4];
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    answerMatrix[i, j] = matrix[i, j] * scalar;
                }
            }
            return answerMatrix;
        }

        public static void PrintMatrix(double[,] matrix)
        {
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }

                Console.WriteLine();
            }
        }


    }
}
