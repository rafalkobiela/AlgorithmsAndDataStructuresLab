using System;

namespace ASD
{
    public class Squares : MarshalByRefObject
    {
        private int[,] solution;
        private int[] sizes;
        private int n;
        private int[,] bestSolution;
        private int bestNumber;
        private int fieldsFilled;

        /// <param name="n">Długość boku działki, którą dzielimy</param>
        /// <param name="sizes">Dopuszczalne długości wynikowych działek</param>
        /// <param name="solution">Tablica n*n z znalezionym podziałem, każdy element to unikalny dodatni identyfikator kwadratu</param>
        /// <returns>Liczba kwadratów na jakie została podzielona działka lub 0 jeśli poprawny podział nie istnieje </returns>
        public int FindDisivion(int n, int[] sizes, out int[,] solution)
        {



            this.solution = new int[n, n];
            this.sizes = sizes;
            this.n = n;
            this.bestNumber = n * n + 1;
            this.bestSolution = new int[n, n];

            FindDivisionHelper(0);

            if (bestNumber < n * n + 1)
            {
                solution = bestSolution;
                return 1;
            }

            solution = null;
            return 0;
        }



        public void Fill(int x, int y, int len, int number)
        {
            for (int i = x; i < x + len; i++)
            {
                for (int j = y; j < y + len; j++)
                {
                    solution[i, j] = number;
                }
            }
        }

        public void UnFill(int x, int y, int len)
        {
            for (int i = x; i < x + len; i++)
            {
                for (int j = y; j < y + len; j++)
                {
                    solution[i, j] = 0;
                }
            }
        }


        public bool IsPossibleToFill(int x, int y, int len)
        {
            if (x + len > this.n || y + len > this.n)
            {
                return false;
            }

            for (int i = x; i < x + len; i++)
            {
                for (int j = y; j < y + len; j++)
                {
                    if (solution[i, j] != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        public (int, int) FindFirstEmpty()
        {
            int firstX = -1;
            int firstY = -1;

            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    if (solution[x, y] == 0)
                    {
                        firstX = x;
                        firstY = y;
                        break;
                    }
                }
                if (firstX != -1)
                {
                    break;
                }
            }


            return (firstX, firstY);
        }

        public bool Finished()
        {

            int max = -1;
            for (int i = solution.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = solution.GetLength(0) - 1; j >= 0; j--)
                {
                    if (solution[i, j] == 0)
                    {
                        return false;
                    }
                    if (solution[i, j] > max)
                    {
                        max = solution[i, j];
                    }
                }
            }

            if (max < bestNumber)
            {

                bestSolution = (int[,])solution.Clone();
                bestNumber = max;
            }

            return true;
        }


        public void FindDivisionHelper(int currentNumber)
        {
            int firstX = -1;
            int firstY = -1;

            (firstX, firstY) = FindFirstEmpty();



            for (int i = sizes.Length - 1; i >= 0; i--)
            {
                if (currentNumber + 1 < bestNumber)
                {
                    if (IsPossibleToFill(firstX, firstY, sizes[i]))
                    {

                        Fill(firstX, firstY, sizes[i], currentNumber + 1);
                        if (!Finished())
                        {
                            FindDivisionHelper(currentNumber + 1);
                        }
                        UnFill(firstX, firstY, sizes[i]);
                    }
                }
            }
        }
    }
}