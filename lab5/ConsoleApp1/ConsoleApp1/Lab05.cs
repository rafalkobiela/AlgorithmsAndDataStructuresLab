using System;
using System.Collections.Generic;
using ASD.Graphs;
using System.Linq;

namespace ASD
{
    public class RoutePlanner : MarshalByRefObject
    {
        public static void print(int[] tab)
        {
            foreach(var i in tab)
            {
                Console.Write($"{i}, ");
            }
            Console.WriteLine();
        }

        private int[] subsetArray(int[] tab, int fromElem)
        {
            int findIndex = -1;

            findIndex = Array.FindIndex(tab, s => { return fromElem == s; } );


            //for(int i =0; i<tab.Length; i++)
            //{
            //    if(tab[i] == fromElem)
            //    {
            //        findIndex = i;
            //    }
            //}

            //var tmp = new int[tab.Length - findIndex];

            //for (int i = 0; i < tab.Length - findIndex; i++)
            //{
            //        tmp[i] = tab[i + findIndex];
            //}
            return tab.Skip(findIndex).ToArray();
        }
        // mozna dodawac metody pomocnicze

        /// <summary>
        /// Znajduje dowolny cykl skierowany w grafie lub stwierdza, że go nie ma.
        /// </summary>
        /// <param name="g">Graf wejściowy</param>
        /// <returns>Kolejne wierzchołki z cyklu lub null, jeśli cyklu nie ma.</returns>
        public int[] FindCycle(Graph g)
        {

            Graph g_cloned = g;
            var cycle_stack = new Stack<int>();

            //if (g_cloned.TopologicalSort(out int[] z, out int[] z1))
            //{
            //    return null;
            //}
            //Graph g2 = g.Clone();

            //g2.FindNegativeCostCycle();


            bool all = true;
            bool found_cycle = false;
            int[] cycle_tab;
            int last = -1;

            var vertexInCycle = new bool[g.VerticesCount];

            //g_cloned.DFSearchAll(v_from =>
            //{
            //    vertexInCycle[v_from] = true;
            //    cycle_stack.Push(v_from);

            //    //foreach(var e in g_cloned.OutEdges(v_from))
            //    //{
            //    //    //    int v_to = e.To;

            //    //    //    if (vertexInCycle[v_to]) // to zmienić na tablice
            //    //    //    {
            //    //    //        found_cycle = true;
            //    //    //        last = v_to;
            //    //    //        return false;
            //    //    //    }
            //    //    //    else
            //    //    //    {

            //    //    //        while (cycle_stack.Count > 0 && cycle_stack.First() != v_from)
            //    //    //        {
            //    //    //            int tmp = cycle_stack.Pop();
            //    //    //            vertexInCycle[tmp] = false;
            //    //    //        }
            //    //    //        if (cycle_stack.Count == 0)
            //    //    //        {
            //    //    //            vertexInCycle[v_from] = true;
            //    //    //            cycle_stack.Push(v_from);
            //    //    //        }
            //    //    //        vertexInCycle[v_from] = true;
            //    //    //        cycle_stack.Push(v_from);


            //    //    //    }
            //    //}

            //    Console.WriteLine($"Current v: {v_from}");

            //    return true;
            //}, null, out int ccc);

            g_cloned.GeneralSearchAll<EdgesStack>(null, null,
                e =>
                {
                    //print(cycle_stack.ToArray());
                    int v_from = e.From;
                    int v_to = e.To;
                    if (cycle_stack.Count == 0)
                    {
                        vertexInCycle[v_from] = true;
                        cycle_stack.Push(v_from);
                    }
                    
                    if (vertexInCycle[v_to]) // to zmienić na tablice
                    {
                        found_cycle = true;
                        last = v_to;
                        return false;
                    }
                    else
                    {
                        while(cycle_stack.Count>0 && cycle_stack.First() != v_from)
                        {
                            int tmp = cycle_stack.Pop();
                            vertexInCycle[tmp] = false;
                        }
                        if(cycle_stack.Count == 0)
                        {
                            vertexInCycle[v_from] = true;
                            cycle_stack.Push(v_from);
                        }
                        vertexInCycle[v_to] = true;
                        cycle_stack.Push(v_to);
                    }

                    return true;
                }
                , out int cc);

            

            int[] tmp2 = null;
            if (found_cycle)
            {
                //var edges = new HashSet<int[]>();
                //all = g.GeneralSearchAll<EdgesStack>(null, null, e => { edges.Add(new int[]{e.From, e.To }); return true; }, out int ccc);

                tmp2 = cycle_stack.ToArray();
                Array.Reverse(tmp2);
                tmp2 = subsetArray(tmp2, last);

                for (int i = 0; i < tmp2.Length; i++)
                {
                    //if (!edges.Contains(new int[] { tmp2[i], tmp2[i + 1] }))
                    //{
                    //    return null;
                    //}
                    if (i < tmp2.Length - 1)
                    {
                        if (g.GetEdgeWeight(tmp2[i], tmp2[i + 1]).IsNaN())
                        {
                            return null;
                        }
                    }
                    else
                    {
                        //Console.WriteLine($"({tmp2[i]}, {tmp2[0]})");
                        if (g_cloned.GetEdgeWeight(tmp2[i], tmp2[0]).IsNaN())
                        {
                            return null;
                        }
                    }
                }
            }

            if (found_cycle && cycle_stack.Count > 1)
            {
                return tmp2;
            }
            return null;
        }

        /// <summary>
        /// Rozwiązanie wariantu 1.
        /// </summary>
        /// <param name="g">Graf połączeń, które trzeba zrealizować</param>
        /// <returns>Lista tras autobusów lub null, jeśli zadanie nie ma rozwiązania</returns>
        public int[][] FindShortRoutes(Graph g)
        {


            var g_cloned = g.Clone();
            int[] tmp_cycle;
            //var cycles_list = new List<int[]>();
            var cycles_list = new int[ Math.Max( g_cloned.EdgesCount, g_cloned.VerticesCount) + 1 ][];
            int idx = 0;

            do
            {

                tmp_cycle = FindCycle(g_cloned);

                if (tmp_cycle != null)
                {

                    for (int i = 0; i < tmp_cycle.Length; i++)
                    {
                        if(i != tmp_cycle.Length - 1)
                        {
                            g_cloned.DelEdge(tmp_cycle[i], tmp_cycle[i + 1]);
                        }else
                        {
                            g_cloned.DelEdge(tmp_cycle[i], tmp_cycle[0]);
                        }
                    }

                    cycles_list[idx] = tmp_cycle;
                    idx++;
                }

            }while(tmp_cycle != null && g_cloned.EdgesCount >= 1);

            

            if (cycles_list.Length > 0 && g_cloned.EdgesCount == 0)
            {
                return cycles_list.Take(idx).ToArray();
            }

            return null;
        }

        public int[] MergeCycles(int[] c1, int[] c2, int commonElement)
        {

            
            int indexC1 = Array.IndexOf(c1, commonElement);
            int indexC2 = Array.IndexOf(c2, commonElement);

            var new_cycle = new int[c1.Length + c2.Length];


            int currentC1 = indexC1;
            int currentC2 = indexC2;
            bool firstCycle = true;
            for (int i = 0; i< new_cycle.Length; i++)
            {
                if (firstCycle)
                {
                    new_cycle[i] = c1[currentC1];
                    currentC1 = (currentC1 + 1) % c1.Length;
                    if(i == c1.Length - 1)
                    {
                        firstCycle = false;
                    }
                }
                else
                {
                    new_cycle[i] = c2[currentC2];
                    currentC2 = (currentC2 + 1) % c2.Length;
                }
                //Console.WriteLine(i);
            }



            return new_cycle; 
        }

        /// <summary>
        /// Rozwiązanie wariantu 2.
        /// </summary>
        /// <param name="g">Graf połączeń, które trzeba zrealizować</param>
        /// <returns>Lista tras autobusów lub null, jeśli zadanie nie ma rozwiązania</returns>
        /// </summary>
        public int[][] FindLongRoutes(Graph g)
        {
            int[][] short_cycles = FindShortRoutes(g);
            if(short_cycles == null || short_cycles.Length == 0)
            {
                return null;
            }

            int numberOfFinalCycles = short_cycles.Length;

            if(short_cycles.Length == 1)
            {
                return short_cycles;
            }


            //Console.WriteLine(short_cycles.GetLength(0));

            for(int i = 0; i < short_cycles.Length; i++)
            {
                for (int j = 0; j < short_cycles.Length; j++)
                {

                    //Console.WriteLine($"Current i: {i}, j: {j}, get {short_cycles.GetLength(0)}");

                    if ( i == j || short_cycles[j] == null || short_cycles[i] == null)
                    {
                        continue;
                    }
                    IEnumerable<int> common = short_cycles[i].Intersect(short_cycles[j]);
                    if (common.Count() > 0)
                    {
                        short_cycles[i] = MergeCycles(short_cycles[i], short_cycles[j], common.First() );
                        short_cycles[j] = null;
                        numberOfFinalCycles--;
                    }
                }
            }

            if (short_cycles != null)
            {
                //Console.WriteLine("Solution");
                //foreach (var i in short_cycles)
                //{
                //    if(i != null)
                //    {
                //        print(i);
                //    }
                //}

                //return cyclesList.ToArray();

            }

            //var finish = new List<int[]>();
            var finish = new int[numberOfFinalCycles][];

            int idx = 0;
            foreach (var i in short_cycles)
            {
                if(i != null)
                {
                    finish[idx++] = i;
                }
            }


            return finish;

        }

    }

}
