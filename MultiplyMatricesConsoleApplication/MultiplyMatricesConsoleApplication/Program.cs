using System;
using System.Threading.Tasks;

namespace MultiplyMatricesConsoleApplication
{
    class Program
    {
        static void Main()
        {
            int[,] matrixA = new int[10, 10];
            int[,] matrixB = new int[10, 10];

            Random randomGenerator = new Random();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    matrixA[i, j] = randomGenerator.Next(10);
                    matrixB[i, j] = randomGenerator.Next(0, 10);
                }
            }

            int[,] result = new int[10,10];
            MultiplyMatricesParralel(matrixA, matrixB, result);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write("{0} ", result[i,j]);
                }
                Console.WriteLine();
            }

            Console.ReadLine();
        }

        static void MultiplyMatricesParralel(int[,] matrixA, int[,] matrixB, int[,] result)
        {
            int matrixACollumns = matrixA.GetLength(1);
            int matrixBCollumns = matrixB.GetLength(1);
            int rows = matrixA.GetLength(0);

            Parallel.For(0, rows, i =>
            {
                    for (int j = 0; j < matrixBCollumns; j++)
                    {
                        for (int k = 0; k < matrixACollumns; k++)
                        {
                            result[i, j] += matrixA[i, k] * matrixB[k, j];
                        }
                    }
            });
        }
    }
}
