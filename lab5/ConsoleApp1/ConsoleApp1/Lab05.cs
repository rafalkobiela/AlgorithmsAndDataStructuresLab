using System;
using System.Collections.Generic;
using ASD.Graphs;
using System.Linq;

namespace ASD
{
    public class RoutePlanner : MarshalByRefObject
    {

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

            bool all = true;
            bool found_cycle = false;
            for (int i = 0; i < g.VerticesCount; i++)
            {

                cycle.Add(i);
                
                 all = g_cloned.GeneralSearchFrom<EdgesStack>(i,

                    v =>
                    {
                        //Console.WriteLine(v);
                        foreach (Edge j in g_cloned.OutEdges(v))
                        {
                            if (j.To == i)
                            {
                                //cycle.Add(j.To);
                                found_cycle = true;
                                return false;
                            }
                        }

                        return true;
                    }
                    ,
                     null
                    ,
                    e =>
                    {

                        cycle.Add(e.To);
                        return true;
                    }
                    );
                if (found_cycle)
                {
                    break;
                }
                else
                {
                    cycle.Clear();
                }

            }

            
            if (found_cycle)
            {
                //Console.WriteLine("");
                //Console.WriteLine(" Current cycle: ");
                //foreach (var z in cycle)
                //{
                //    Console.Write(z + ", ");
                //}
                //Console.WriteLine("");

                return cycle.ToArray();
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



            var list_of_cycles = new List < int[] > ();


            Graph g_cloned = g.Clone();

            var cycle = new List<int>();
            //Console.WriteLine("Current number of edges: " + g_cloned.EdgesCount.ToString());
            bool all = true;
            bool found_cycle = false;
            for (int i = 0; i < g.VerticesCount; i++)
            {

                cycle.Add(i);

                all = g_cloned.GeneralSearchFrom<EdgesStack>(i,

                   v =>
                   {
                       //Console.WriteLine(v);
                       foreach (Edge j in g_cloned.OutEdges(v))
                       {
                           if (j.To == i)
                           {
                               
                                //cycle.Add(j.To);
                                found_cycle = true;
                                return false;

                           }
                       }

                       return true;
                   }
                   ,
                    null
                   ,
                   e =>
                   {

                       int current_enter = -1;
                       bool found = false;
                       for(int j = 0; j<cycle.Count; j++)
                       {
                           if(e.From == cycle[j])
                           {
                               current_enter = j;
                               found = true;
                           } 
                       }

                       if (found)
                       {

                           cycle.RemoveRange(current_enter, cycle.Count - current_enter - 1);
                           //for(int j = cycle.Count - 1; j >= current_enter; j++)
                           //{    
                           //    cycle.RemoveAt(j);
                           //}

                       }

                       cycle.Add(e.To);
                       return true;

                   }
                   );

                if (found_cycle)
                {

                    list_of_cycles.Add(cycle.ToArray());
                    cycle.Add(i);

                    //Console.WriteLine(" Current cycle: ");
                    //for (int z = 0; z < cycle.Count; z++)
                    //{
                    //    Console.Write(cycle[z].ToString() + ", ");
                    //}

                    //Console.WriteLine("");

                    for (int k = 0; k<cycle.Count - 1; k++)
                    {
                        g_cloned.DelEdge(cycle[k], cycle[k + 1]);
                    }

                    cycle.Clear();

                    //Console.WriteLine("Current number of edges: " + g_cloned.EdgesCount.ToString() + " Current v: " + i.ToString());
                    if(g_cloned.EdgesCount == 0)
                    {
                        break;
                    }

                }
                else
                {
                    cycle.Clear();
                }

            }
            
            if(g_cloned.EdgesCount == 0)
            {
                return list_of_cycles.ToArray();
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
