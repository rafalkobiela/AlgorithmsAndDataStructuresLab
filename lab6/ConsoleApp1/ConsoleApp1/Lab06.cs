using System;

namespace ASD
{
    public class Squares : MarshalByRefObject
    {
        /// <param name="n">Długość boku działki, którą dzielimy</param>
        /// <param name="sizes">Dopuszczalne długości wynikowych działek</param>
        /// <param name="solution">Tablica n*n z znalezionym podziałem, każdy element to unikalny dodatni identyfikator kwadratu</param>
        /// <returns>Liczba kwadratów na jakie została podzielona działka lub 0 jeśli poprawny podział nie istnieje </returns>
        public int FindDisivion(int n, int[] sizes, out int[,] solution)
        {

            solution = new int[n,n];

            int[,] tmp_solution = new int[n, n];
            int[,] best_solution = new int[n, n];

            int index = 0;
            bool ret = false;
            FindDivisionHelper(n, sizes, ref solution, ref ret, index);


            if (ret)
            {
                return 1;
            }
            else
            {

                return 0;
            }

            
           
        }

        public void Fill(ref int[,] tab, int x, int y, int len, int number)
        {
            for (int i = x; i < x + len; i++)
            {
                for (int j = x; j < x + len; j++)
                {
                    tab[i, j] = number;
                }
            }
        }

        public void UnFill(ref int[,] tab, int x, int y, int len, int number)
        {
            for (int i = x; i < x + len; i++)
            {
                for (int j = x; j < x + len; j++)
                {
                    tab[i, j] = 0;
                }
            }
        }

        public bool isPossible(int[,] tab, int x, int y, int len)
        {
            if(x + len >= tab.GetLength(0) || y + len >= tab.GetLength(0))
            {
                return false;
            }



            for (int i = x; i < x + len; i++)
            {
                for (int j = x; j < x + len; j++)
                {
                    if (tab[i, j] != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        public void print(int[,] tab)
        {
            for (int i = 0; i < tab.GetLength(0); i++)
            {
                for (int j = 0; j < tab.GetLength(0); j++)
                {
                    Console.Write($"{tab[i, j]}, ");
                }
                Console.WriteLine("");
            }
        }

        public bool allFilled(int [,] tab)
        {
            for (int i = 0; i < tab.GetLength(0); i++)
            {
                for (int j = 0; j < tab.GetLength(0); j++)
                {
                    if(tab[i,j] != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool FindDivisionHelper(int n, int[] sizes, ref int[,] solution, ref bool ret, int currentNumber = 0, int x = 0, int y = 0)
        {



            if (ret)
            {
                return ret;
            }


                for (int i = x; i < n; i++)
                {
                    for (int j = y; j < n; j++)
                    {
                        for (int k = sizes.Length - 1; k >= 0; k--)
                        {

                            if (isPossible(solution, i, j, k))
                            {

                                Fill(ref solution, i, j, k, currentNumber + 1);
                                
                                if (allFilled(solution))
                                {
                                    ret = true;
                                }

                                FindDivisionHelper(n, sizes, ref solution, ref ret, i + 1, j + 1, currentNumber + 1);

                                if (!ret)
                                {
                                    UnFill(ref solution, i, j, k, currentNumber + 1);
                                }
                            }
                    }
                }
            }

            return ret;

        }



    }
}