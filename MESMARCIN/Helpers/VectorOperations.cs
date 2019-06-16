using System;
using System.Linq;

namespace MESMARCIN.Helpers
{
    public static class VectorOperations
    {
        public static double[] WithScalarMultiplication(double[] vector, double scalar)
        {
            var answerVector = new double[vector.Length];
            for (var i = 0; i < answerVector.Length; i++)
            {
                answerVector[i] = vector[i] * scalar;
            }
            return answerVector;
        }

        public static double[] Add(double[] vector1, double[] vector2)
        {
            var answerVector = new double[vector1.Length];
            for (var i = 0; i < answerVector.Length; i++)
            {
                answerVector[i] = vector1[i] + vector2[i];
            }
            return answerVector;
        }

        public static void Print(double[] vector)
        {
            foreach (var value in vector)
            {
                Console.Write(value + " ");
            }
        }

        public static (double min, double max) FindMinAndMax(double[] vector) => (vector.Min(), vector.Max());
    }
}
