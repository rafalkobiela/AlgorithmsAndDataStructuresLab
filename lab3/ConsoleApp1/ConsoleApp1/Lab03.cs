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

        public bool IsGreaterThanZero(int[] tab)
        {

            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void InsertionSort(ref int[] tab, int start = 0)
        {
            for (int i = start; i < tab.Length - 1; i++)
            {
                for (int j = i + 1; j > start; j--)
                {
                    if (tab[j - 1] < tab[j])
                    {
                        int temp = tab[j - 1];
                        tab[j - 1] = tab[j];
                        tab[j] = temp;
                    }
                }
            }
        }

        public int FindFirstNonZero(int[] tab)
        {

            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] > 0)
                {
                    return i;
                }
            }

            return -1;
        }

        public void PrintArr(int[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                Console.Write(a[i].ToString() + ", ");
            }
            Console.WriteLine();
            Console.WriteLine("------------------");
        }

        public int[] SortAsIWant(int[] tab)
        {
            return tab;
        }


        public bool IsGraphic(int[] sequence)
        {
            var tab = (int[])sequence.Clone();

            IComparer revComparer = new ReverseComparer();
            Array.Sort(tab, 0 , tab.Length, revComparer);
            //Array.Reverse(tab);
            //PrintArr(tab);
            int degSum = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] < 0 )
                {
                    return false;
                }
                degSum += tab[i];
            }

            if (degSum % 2 != 0)
            {
                return false;
            }


            //Pierwsze rozw

                //int start = 0;
                //while (tab[start] > 0)
                //{

                //    int tmp = tab[start];
                //    tab[start] = 0;

                //    for (int i = start + 1; i <= tmp + start; i++)
                //    {
                //        if (i == tab.Length || --tab[i] < 0)
                //        {
                //            return false;
                //        }
                //    }

                //    int nextZero = 0;
                //    for (int i = 1; i < tab.Length - start; i++)
                //    {
                //        if (start + i + 1 < tab.Length)
                //        {
                //            if (tab[start + i] == 0)
                //            {
                //                nextZero++;
                //            }
                //            else
                //            {
                //                break;
                //            }

                //        }
                //    }
                //    start += nextZero + 1;
                //    Array.Sort(tab, start, Math.Min(tmp + start, tab.Length - start), revComparer);
                //    //PrintArr(tab);
                //}

                //return true;


                //Drugie rozw

                //int n = tab.Length;
                //for (int i = 0; i < n; i++)
                //{

                //    int tmp = tab[i];
                //    for (int j = i + 1; j < n; j++)
                //    {
                //        if (tab[i] == 0)
                //        {
                //            break;
                //        }

                //        if (tab[i] > 0 && tab[j] > 0)
                //        {

                //            //PrintArr(tab);
                //            tab[i]--;
                //            tab[j]--;
                //            if (tab[i] == 0)
                //            {
                //                break;
                //            }

                //        }
                //    }
                //}


                //for (int i = 0; i < tab.Length; i++)
                //{
                //    if (i != 0)
                //    {
                //        if (tab[i]%2 == 0 && tab[i] > 0)
                //        {
                //            tab[i] -= 2;
                //            i--;
                //            //newGraph.AddEdge(i, i);
                //            //break;
                //        }
                //    }
                //}

                ////PrintArr(tab);
                //for(int i = 0; i< tab.Length; i++)
                //{
                //    if (tab[i] > 0)
                //    {
                //        return false;
                //    }
                //}

                //return true;
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

                        //PrintArr(tab);
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
                if (/*u.Find(e.From) != u.Find(e.To) */ u.Union(e.From, e.To))
                {
                    //u.Union(e.From, e.To);
                    kurskalTree.AddEdge(e);
                    min_weight += e.Weight;
                }
            }
            return kurskalTree;  // zmienić

        }
    }
}