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
                    if(isSportActivity == null || isSportActivity[preferences[i][j]])
                        g.AddEdge(i + limits.Length, preferences[i][j], 1);
                }
            }


            (double satisf, Graph initialFlow) = g.FordFulkersonDinicMaxFlow(n - 1, n - 2, MaxFlowGraphExtender.DFSBlockingFlow);

            var bestDistribution = new int[preferences.Length];

            for (int i = 0; i < bestDistribution.Length; i++)
            {
                bestDistribution[i] = -1;
            }


            var participants = new int[limits.Length];

            for (int i = 0; i < preferences.Length; i++)
            {
                for (int j = 0; j < preferences[i].Length; j++)
                {
                    if (initialFlow.GetEdgeWeight(i + limits.Length, preferences[i][j]) == 1)
                    {
                        bestDistribution[i] = preferences[i][j];
                        participants[preferences[i][j]]++;
                        break;
                    }
                }
            }


            if (isSportActivity == null)
            {
                return ((int)satisf, bestDistribution);
            }
            else
            {

                bool possible = true;

                for(int i = 0; i<limits.Length; i++)
                {
                    if(isSportActivity[i] && limits[i] != participants[i])
                    {
                        possible = false;
                    }
                }

                if (possible)
                {
                    return (1, bestDistribution);
                }
                else
                {
                    return (0, null);
                }
            }


        }

    }
}
