using System;

namespace MesMarcin
{
    public static class MatrixOperations
    {
        public static double[,] TranspositionAndMultiplication(double[] vector)
        {
            var matrix = new double[vector.Length, vector.Length];
            for (var i = 0; i < vector.Length; i++)
            {
                for (var j = 0; j < vector.Length; j++)
                {
                    matrix[i, j] = vector[i] * vector[j];
                }
            }
            return matrix;
        }

        public static double[,] Add(double[,] matrix1, double[,] matrix2)
        {
            var answerMatrix = new double[matrix1.GetLength(0), matrix1.GetLength(0)];
            for (var i = 0; i < matrix1.GetLength(0); i++)
            {
                for (var j = 0; j < matrix1.GetLength(0); j++)
                {
                    answerMatrix[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }
            return answerMatrix;
        }

        public static double[,] ScalarMultiplication(double[,] matrix, double scalar)
        {
            var answerMatrix = new double[matrix.GetLength(0), matrix.GetLength(0)];
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(0); j++)
                {
                    answerMatrix[i, j] = matrix[i, j] * scalar;
                }
            }
            return answerMatrix;
        }

        public static void Print(double[,] matrix)
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

        public static double[] WithVectorMultiplication(double[,] matrix, double[] vector)
        {
            var answer = new double[vector.Length];
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < vector.Length; j++)
                {
                    answer[i] += matrix[i, j] * vector[j];
                }
            }
            return answer;
        }
    }
}
