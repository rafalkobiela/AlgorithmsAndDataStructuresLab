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


            int verticesCount = sellPrices.Length + sellPrices.Length + 1;
            Graph g = new AdjacencyListsGraph<AVLAdjacencyList>(true, verticesCount);
            Graph costsGraph = new AdjacencyListsGraph<AVLAdjacencyList>(true, verticesCount);


            g.AddEdge(verticesCount - 1, 0, lemonadeAmount);
            costsGraph.AddEdge(verticesCount - 1, 0, 0);

            //Przed granicą

            foreach (var i in possibleChangesBeforeBorder)
            {
                g.AddEdge(i.from, i.to, i.limit);
                costsGraph.AddEdge(i.from, i.to, i.cost);
            }


            int afterBorder = sellPrices.Length;

            //Za granicą
            foreach (var i in possibleChangesAfterBorder)
            {
                g.AddEdge(i.from + afterBorder, i.to + afterBorder, i.limit);
                costsGraph.AddEdge(i.from + afterBorder, i.to + afterBorder, i.cost);
            }

            //Cło przy granicy


            for (int i = 0; i < sellPrices.Length; i++)
            {
                g.AddEdge(i, i + afterBorder, double.PositiveInfinity);
                costsGraph.AddEdge(i, i + afterBorder, sellPrices[i] * 0.5);
            }


            //Z lemionady na lemioniade
            g.AddEdge(0, afterBorder, double.PositiveInfinity);
            costsGraph.AddEdge(0, afterBorder, sellPrices[0] * 0.5);


            (double value, double cost, Graph flow) res = g.MinCostFlow(costsGraph, verticesCount - 1, afterBorder);



            changesBeforeBorder = new List<(int from, int to, int amount)>();
            changesAfterBorder = new List<(int from, int to, int amount)>();


            foreach (var i in possibleChangesBeforeBorder)
            {
                if ((int)res.flow.GetEdgeWeight(i.from, i.to) > 0)
                {
                    changesBeforeBorder.Add((i.from, i.to, (int)res.flow.GetEdgeWeight(i.from, i.to)));
                }
            }



            foreach (var i in possibleChangesAfterBorder)
            {
                if ((int)res.flow.GetEdgeWeight(i.from + afterBorder, i.to + afterBorder) > 0)
                {
                    changesAfterBorder.Add((i.from, i.to, (int)res.flow.GetEdgeWeight(i.from + afterBorder, i.to + afterBorder)));
                }
            }





            return res.value * sellPrices[0] - res.cost;
        }

    }
}