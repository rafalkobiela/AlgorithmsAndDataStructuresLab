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
            solution = null;
            return 0;
        }

    }
}