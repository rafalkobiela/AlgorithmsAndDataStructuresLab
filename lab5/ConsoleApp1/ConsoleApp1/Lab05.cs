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

            return tab.Skip(findIndex).ToArray();
        }
        // mozna dodawac metody pomocnicze

        /// <summary>
        /// Znajduje dowolny cykl skierowany w grafie lub stwierdza, że go nie ma.
        /// </summary>
        /// <param name="g">Graf wejściowy</param>
        /// <returns>Kolejne wierzchołki z cyklu lub null, jeśli cyklu nie ma.</returns>
        /// 
        public int[] FindCycle(Graph g)
        {

            Graph g_cloned = g;
            var cycle_stack = new Stack<int>();


            bool found_cycle = false;

            int last = -1;

            var vertexInCycle = new bool[g.VerticesCount];

  

            g_cloned.GeneralSearchAll<EdgesStack>(
                 v =>
                 {

                     cycle_stack.Push(v);
                     vertexInCycle[v] = true;

                     return true;
                 }
                , 
                 v =>
                 {

                     var popped = cycle_stack.Pop();
                     vertexInCycle[popped] = false;
  
                     return true;
                 }
                ,
                e =>
                {
                    if (vertexInCycle[e.To])
                    {
                        last = e.To;
                        found_cycle = true;
                        return false;
                    }


                    return true;
                }
                , out int cc);

            

            int[] tmp2 = null;
            if (found_cycle)
            {

                tmp2 = cycle_stack.ToArray();
                Array.Reverse(tmp2);
                tmp2 = subsetArray(tmp2, last);
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


  

            for(int i = 0; i < short_cycles.Length; i++)
            {
                for (int j = 0; j < short_cycles.Length; j++)
                {


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
