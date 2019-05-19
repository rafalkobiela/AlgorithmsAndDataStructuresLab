using ASD.Graphs;
using System;
using System.Collections.Generic;

namespace ASD
{
    public class DutyOptimizer : MarshalByRefObject
    {
        /// <summary>
        /// Metoda realizująca zadanie (sprzedaż lemoniady za granicą)
        /// </summary>
        /// <param name="possibleChangesBeforeBorder">Możliwe zamiany przed przekroczeniem granicy</param>
        /// <param name="possibleChangesAfterBorder">Możliwe zamiany po przekroczeniu granicy</param>
        /// <param name="substancesNumber">Liczba wszystkich substancji (identyfikatory substancji to 0 (lemoniada), 1, 2, ..., substancesNumber-1)</param>
        /// <param name="sellPrices">Tablica długości substancesNumber, i-ty element to cena jednostki i-tej substancji</param>
        /// <param name="lemonadeAmount">Liczba jednostek lemoniady do sprzedania za granicą</param>
        /// <param name="changesBeforeBorder">Wynikowe zamiany przed granicą</param>
        /// <param name="changesAfterBorder">Wynikowe zamiany za granicą</param>
        /// <returns>Całkowity zysk (przychód ze sprzedaży - wszystkie poniesione wydatki)</returns>
        public double CreateSimplePlan((int from, int to, int cost, int limit)[] possibleChangesBeforeBorder,
                                       (int from, int to, int cost, int limit)[] possibleChangesAfterBorder,
                                       int substancesNumber, double[] sellPrices, int lemonadeAmount,
                                       out List<(int from, int to, int amount)> changesBeforeBorder,
                                       out List<(int from, int to, int amount)> changesAfterBorder)
        {
            changesBeforeBorder = null;
            changesAfterBorder = null;

            return -1;
        }

    }
}