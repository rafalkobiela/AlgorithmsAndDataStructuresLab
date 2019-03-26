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
            for(int i =0; i<tab.Length; i++)
            {
                if(tab[i] == fromElem)
                {
                    findIndex = i;
                }
            }

            var tmp = new int[tab.Length - findIndex];

            for (int i = 0; i < tab.Length - findIndex; i++)
            {
                try
                {
                    tmp[i] = tab[i + findIndex];
                }
                catch
                {
                    Console.WriteLine("Actual tab;");
                    print(tmp);
                    Console.WriteLine($"Current i {i}, indexTab {findIndex}");
                    

                    tmp[i] = tab[i + findIndex];

                }
            }
            return tmp;
            
        }
        // mozna dodawac metody pomocnicze

        /// <summary>
        /// Znajduje dowolny cykl skierowany w grafie lub stwierdza, że go nie ma.
        /// </summary>
        /// <param name="g">Graf wejściowy</param>
        /// <returns>Kolejne wierzchołki z cyklu lub null, jeśli cyklu nie ma.</returns>
        public int[] FindCycle(Graph g)
        {

            Graph g_cloned = g.Clone();

            var cycle = new List<int>();
            var cycle_set = new HashSet<int>();
            var cycle_stack = new Stack<int>();
            var cycle_queue = new Queue<int>();

            bool all = true;
            bool found_cycle = false;
            int[] cycle_tab;
            int last = -1;

            g_cloned.GeneralSearchAll<EdgesStack>(null, null,
                e =>
                {
                    //print(cycle_stack.ToArray());
                    int v_from = e.From;
                    int v_to = e.To;
                    if (cycle_stack.Count() == 0)
                    {
                        cycle_stack.Push(v_from);
                    }
                    

                    if (cycle_stack.Contains(v_to)) // to zmienić na tablice
                    {
                        //while (cycle_stack.Count > 0 && cycle_stack.First() != v_from)
                        //{
                        //    cycle_stack.Pop();
                        //}
                        found_cycle = true;
                        last = v_to;
                        return false;
                    }
                    else
                    {
                        while(cycle_stack.Count>0 && cycle_stack.First() != v_from)
                        {
                            cycle_stack.Pop();
                        }
                        if(cycle_stack.Count == 0)
                        {
                            cycle_stack.Push(v_from);
                        }
                        cycle_stack.Push(v_to);
                    }

                    return true;
                }
                , out int cc);

            if (found_cycle)
            {
                var tmp = cycle_stack.ToArray();
                tmp.Reverse();
                Array.Reverse(tmp);
                return subsetArray(tmp, last);
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
            var cycles_list = new List<int[]>();


            do
            {

                tmp_cycle = FindCycle(g_cloned);
                //Console.WriteLine($"CUrrent edges {g_cloned.EdgesCount}");

                if (tmp_cycle != null)
                {
                    //List<int> tmp_list = tmp_cycle.ToList();

                    //print(tmp_list.ToArray());

                    //tmp_list.Append(tmp_cycle[0]);

                    //print(tmp_list.ToArray());

                    for (int i = 0; i < tmp_cycle.Length; i++)
                    {
                        //Console.WriteLine(i);
                        try
                        {
                            g_cloned.DelEdge(tmp_cycle[i], tmp_cycle[i + 1]);
                        }
                        catch
                        {
                            g_cloned.DelEdge(tmp_cycle[i], tmp_cycle[0]);
                        }
                    }
                    
                    //RoutePlanner.print(tmp_cycle);
                    //Console.WriteLine($"CUrrent edges {g_cloned.EdgesCount}, Current number of lists {cycles_list.Count()}");
                    cycles_list.Add(tmp_cycle);

                }

            }while(tmp_cycle != null && g_cloned.EdgesCount >= 1);

            //Console.WriteLine($"CUrrent edges {g_cloned.EdgesCount}, Current number of lists {cycles_list.Count()}");

            if (cycles_list.Count > 0 && g_cloned.EdgesCount == 0)
            {
                return cycles_list.ToArray();
            }

            return null;
        }

        /// <summary>
        /// Rozwiązanie wariantu 2.
        /// </summary>
        /// <param name="g">Graf połączeń, które trzeba zrealizować</param>
        /// <returns>Lista tras autobusów lub null, jeśli zadanie nie ma rozwiązania</returns>
        /// </summary>
        public int[][] FindLongRoutes(Graph g)
        {
            return new int[][] { new int[] { -1 } };
        }

    }

}
