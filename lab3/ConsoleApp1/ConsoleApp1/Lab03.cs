using ASD.Graphs;
using System;

namespace ASD
{
    public class Lab03 : MarshalByRefObject
    {
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


            //Array.Sort(tab);
            //Array.Reverse(tab);

            //tab = new int[] { 0, 0, 2,1,4,5,1,4 };
            //Console.WriteLine();
            //Console.WriteLine("----------");
            //Console.WriteLine();
            //for (int i = 0; i<tab.Length; i++)
            //{
            //    Console.Write(tab[i].ToString() + ", ");
            //}
            //Console.WriteLine();
            //Console.WriteLine("----------");
            //Console.WriteLine();
            //InsertionSort(ref tab, 2);
            //for (int i = 0; i < tab.Length; i++)
            //{
            //    Console.Write(tab[i].ToString() + ", ");
            //}
            //Console.WriteLine();
            //Console.WriteLine("----------");


            InsertionSort(ref tab);


            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] < 0)
                {
                    return false;
                }

            }

            int start = 0;
            while (tab[start] > 0)
            {

                int tmp = tab[start];
                tab[start] = 0;

                for (int i = start + 1; i <= tmp + start; i++)
                {
                    //PrintArr(tab);
                    if (i == tab.Length || --tab[i] < 0)
                    {
                        return false;
                    }

                }
                start++;

                //Array.Sort(tab);
                //Array.Reverse(tab);
                InsertionSort(ref tab, start);
            }

            return true;
        }

        //Część 2
        // Konstruowanie grafu na podstawie podanego ciągu grafowego
        // 1.5 pkt
        public Graph ConstructGraph(int[] sequence)
        {

            if (!IsGraphic(sequence))
            {
                return null;
            }

            var newGraph = new AdjacencyListsGraph<AVLAdjacencyList>(false, sequence.Length);
            var tab = (int[])sequence.Clone();

            Array.Sort(tab);
            Array.Reverse(tab);
            //bool isAllZero = true;

            //for (int i = 0; i < tab.Length; i++)
            //{
            //    if (tab[i] < 0)
            //    {
            //        isAllZero = false;
            //    }

            //}

            //if (!isAllZero)
            //{
            //    return null;
            //}

            //if (isAllZero)
            //{
            //    return new AdjacencyListsGraph<AVLAdjacencyList>(false, sequence.Length); 
            //}


            for (int i = 0; i < tab.Length; i++)
            {
                int tmp = tab[i];
                tab[i] = 0;
                for (int j = i + 1; j <= i + tmp; j++)
                {

                    if (i == tab.Length - 1 && j > tab.Length - 1)
                    {
                        newGraph.AddEdge(i, i);
                    }
                    else if (tab[j] == 0 && tmp == 2)
                    {
                        newGraph.AddEdge(i, i);
                    }
                    else if (tab[j] == 0 && tmp == 1)
                    {
                        newGraph.AddEdge(j + 1, i);
                        tab[j + 1]--;

                    }

                    if (j < tab.Length)
                    {
                        if (tab[j] > 0)
                        {
                            tab[j]--;
                            newGraph.AddEdge(j, i);
                        }
                    }
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

            PriorityQueue<Edge, double> edgeQueue = new PriorityQueue<Edge, double>((e1, e2) => e1.Value < e2.Value);
            graph.GeneralSearchAll<EdgesQueue>(null, null, e => edgeQueue.Put(e, e.Weight), out int cc);

            min_weight = 0;
            Graph kruskalTree = graph.IsolatedVerticesGraph();


            UnionFind uf = new UnionFind(graph.VerticesCount);
            while (!edgeQueue.Empty)
            {
                Edge e = edgeQueue.Get();

                if (uf.Find(e.From) != uf.Find(e.To))
                {
                    min_weight += e.Weight;
                    uf.Union(e.From, e.To);
                    kruskalTree.AddEdge(e);
                }
            }

            return kruskalTree;
        }
    }
}