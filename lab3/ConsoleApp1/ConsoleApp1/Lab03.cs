using ASD.Graphs;
using System;
using System.Collections;

namespace ASD
{



    public class Lab03 : MarshalByRefObject
    {

        public class ReverseComparer : IComparer
        {
            // Call CaseInsensitiveComparer.Compare with the parameters reversed.
            public int Compare(Object x, Object y)
            {
                return -((IComparable)x).CompareTo(y);
            }

        }

        // Część 1
        //  Sprawdzenie czy podany ciąg stopni jest grafowy
        //  0.5 pkt


        public bool IsGraphic(int[] sequence)
        {
            var tab = (int[])sequence.Clone();

            IComparer revComparer = new ReverseComparer();
            Array.Sort(tab, 0, tab.Length, revComparer);
            int degSum = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] < 0)
                {
                    return false;
                }
                degSum += tab[i];
            }

            if (degSum % 2 != 0)
            {
                return false;
            }

            int n = tab.Length;
            for (int i = 0; i < n; i++)
            {
                int tmp = tab[i];
                for (int j = i + 1; j < n; j++)
                {
                    if (tab[i] == 0)
                    {
                        break;
                    }

                    if (tab[i] > 0 && tab[j] > 0)
                    {
                        tab[i]--;
                        tab[j]--;
                        if (tab[i] == 0)
                        {
                            break;
                        }
                    }
                }
                if (i == tab.Length - 1 && tab.Length > 1)
                {
                    if (tab[i] == 1 && tab[i - 1] == 1)
                    {
                        tab[i] = 0;
                        tab[i - 1] = 0;

                    }
                }
            }

            for (int i = 0; i < tab.Length; i++)
            {

                if (tab[i] >= 2 && i != 0)
                {
                    tab[i] -= 2;
                    i--;
                }
            }

            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] != 0)
                {
                    return false;
                }
            }

            return true;
        }

        //Część 2
        // Konstruowanie grafu na podstawie podanego ciągu grafowego
        // 1.5 pkt
        public Graph ConstructGraph(int[] sequence)
        {


            int degSum = 0;
            for (int i = 0; i < sequence.Length; i++)
            {
                if (sequence[i] < 0)
                {
                    return null;
                }
                degSum += sequence[i];
            }

            if (degSum % 2 != 0)
            {
                return null;
            }


            var newGraph = new AdjacencyListsGraph<AVLAdjacencyList>(false, sequence.Length);
            var tab = (int[])sequence.Clone();

            Array.Sort(tab);
            Array.Reverse(tab);
            int n = tab.Length;
           

            for (int i = 0; i < n; i++)
            {

                for (int j = i + 1; j < n; j++)
                {
                    if (tab[i] == 0)
                    {
                        break;
                    }

                    if (tab[i] > 0 && tab[j] > 0)
                    {
                        tab[i]--;
                        tab[j]--;
                        newGraph.AddEdge(i, j);
                        if (tab[i] == 0)
                        {
                            break;
                        }

                    }
                }
            }

            for (int i = tab.Length - 1; i >= 0; i--)
            {
                if (tab[i] > 1)
                {
                    tab[i] -= 2;
                    newGraph.AddEdge(i, i);
                    break;
                }
                if (tab[i] > 0)
                {
                    return null;
                }
            }

            return newGraph;
        }

        //Część 3
        // Wyznaczanie minimalnego drzewa (bądź lasu) rozpinającego algorytmem Kruskala
        // 2 pkt 
        public Graph MinimumSpanningTree(Graph graph, out double min_weight)
        {

            if (graph.Directed)
            {
                throw new ArgumentException();
            }

            UnionFind u = new UnionFind(graph.VerticesCount);
            EdgesMinPriorityQueue q = new EdgesMinPriorityQueue();
            Graph kurskalTree = graph.IsolatedVerticesGraph();

            for (int i = 0; i < graph.VerticesCount; i++)
            {
                foreach (var e in graph.OutEdges(i))
                {
                    if (e.To >= i)
                    {
                        q.Put(e);
                    }
                }
            }
            min_weight = 0;       // zmienić
            while (!q.Empty)
            {
                Edge e = q.Get();
                if (u.Union(e.From, e.To))
                {
                    
                    kurskalTree.AddEdge(e);
                    min_weight += e.Weight;
                }
            }
            return kurskalTree;  // zmienić

   

        }

    }
}