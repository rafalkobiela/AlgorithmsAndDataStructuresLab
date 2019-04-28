using ASD.Graphs;
using System;

namespace Lab9
{
    public class DistributionFinder : MarshalByRefObject
    {
        private void print(int[] a)
        {
            foreach (var i in a)
            {
                Console.Write($"{i}, ");
            }
            Console.WriteLine();
        }

        public (int satisfactionLevel, int[] bestDistribution) FindBestDistribution(int[] limits, int[][] preferences, bool[] isSportActivity)
        {

            int n = limits.Length + preferences.Length + 2;
            //n - 2 - żródło
            //n - 1 - ujście

            //0 - limits.Length - 1 - zajęcia
            //limits.Length - limits.Length + preferences.Length - studenty


            Graph g = new AdjacencyListsGraph<HashTableAdjacencyList>(true, n);

            for (int i = 0; i < limits.Length; i++)
            {
                g.AddEdge(i, n - 2, limits[i]);
            }

            for (int i = 0; i < preferences.Length; i++)
            {
                g.AddEdge(n - 1, limits.Length + i, 1);
            }

            for (int i = 0; i < preferences.Length; i++)
            {
                for (int j = 0; j < preferences[i].Length; j++)
                {
                    g.AddEdge(i + limits.Length, preferences[i][j], 1);
                }
            }

            //GraphExport a = new GraphExport();

            //a.Export(g);


            (double satisf, Graph initialFlow) = g.FordFulkersonDinicMaxFlow(n - 1, n - 2, MaxFlowGraphExtender.DFSBlockingFlow);

            var bestDistribution = new int[preferences.Length];

            for (int i = 0; i < bestDistribution.Length; i++)
            {
                bestDistribution[i] = -1;
            }


            var part = new int[limits.Length];

            for (int i = 0; i < preferences.Length; i++)
            {
                for (int j = 0; j < preferences[i].Length; j++)
                {
                    if (initialFlow.GetEdgeWeight(i + limits.Length, preferences[i][j]) == 1)
                    {
                        bestDistribution[i] = preferences[i][j];
                        part[preferences[i][j]]++;
                        break;
                    }
                }
            }
            //print(bestDistribution);
            //return ((int)satisf, null);

            //print(part);


            return ((int)satisf, bestDistribution);



        }

    }
}
