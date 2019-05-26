using System;
using System.Linq;
using ASD.Graphs;
using System.Collections.Generic;
using System.Collections;

namespace Lab13
{
    public class ProgramPlanning : MarshalByRefObject
    {

        public void print(IEnumerable a)
        {
            Console.WriteLine();
            foreach (var i in a)
            {
                Console.Write($"{i}, ");
            }
            Console.WriteLine();
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="taskGraph">Graf opisujący zależności procedur</param>
        /// <param name="taskTimes">Tablica długości czasów procedur</param>
        /// <param name="startTimes">Parametr wyjśćiowy z najpóźniejszymi możliwymi startami procedur przy optymalnym czasie całości</param>
        /// <param name="startTimes">Parametr wyjśćiowy z dowolna wybraną ścieżką krytyczną</param>
        /// <returns>Najkrótszy czas w jakim można wykonać cały program</returns>
        public double CalculateTimesLatestPossible(Graph taskGraph, double[] taskTimes, out double[] startTimes, out int[] criticalPath)
        {
            startTimes = null;
            criticalPath = null;

            int n = taskGraph.VerticesCount;

            var startingVertices = new List<int>();



            for (int i = 0; i < n; i++)
            {
                if (taskGraph.InDegree(i) == 0)
                {
                    startingVertices.Add(i);
                }
            }

            var endingVertices = new List<int>();


            for (int i = 0; i < n; i++)
            {
                if (taskGraph.OutDegree(i) == 0)
                {
                    endingVertices.Add(i);
                }
            }

            var reversedGraph = taskGraph.Reverse();

            double currMax = 0;
            double cur = 0;

            foreach (var i in endingVertices)
            {
                var res = reversedGraph.GeneralSearchFrom<EdgesStack>(i
                    , v =>
                     {
                         cur += taskTimes[v];
                         if (startingVertices.Contains(v))
                         {
                             if (currMax < cur)
                                 currMax = cur;
                         }
                         return true;
                     }
                    , v =>
                    {
                        cur -= taskTimes[v];
                        return true;
                    }
                    , null);
            }


            var currMaxTab = new double[n];
            var currTab = new double[n];

            startTimes = new double[n];

            foreach (int u in endingVertices)
            {
                currMaxTab[u] = 0;
            }

            foreach (var i in endingVertices)
            {

                var res = reversedGraph.GeneralSearchFrom<EdgesStack>(i
                    ,
                    null
                    //v =>
                    //{
                    //    if(prev != i && start)
                    //    {
                    //        start = false;
                    //        currTab[v] = currTab[prev] + taskTimes[prev];
                    //        if (startingVertices.Contains(v))
                    //        {
                    //            if (currTab[v] > currMaxTab[v])
                    //                currMaxTab[v] = currTab[v];
                    //        }
                    //    }
                    //    Console.WriteLine($"curr: {v}, prev: {prev}");
                    //    prev = v;
                    //    print(currTab);
                    //    return true;
                    //}
                    , null
                    //, v =>
                    //{
                    //    //currTab[v] = currTab[prev] + taskTimes[prev];
                    //    prev = v;
                    //    //cur -= taskTimes[v];
                    //    return true;
                    //}
                    ,
                    e =>
                    {

                        currTab[e.From] = currTab[e.To] + taskTimes[e.To];
                        if (startingVertices.Contains(e.To))
                        {
                            if (currTab[e.To] > currMaxTab[e.To])
                                currMaxTab[e.To] = currTab[e.To];
                        }

                        //Console.WriteLine($"curr: {e.To}, prev: {e.From}");
                        //print(currTab);
                        return true;
                    },
                    null);
            }


            //Console.WriteLine("end:----------------------");
            //print(currMaxTab);
            startTimes = currMaxTab;


            while drugim trzeba liczyć od końca 


            return currMax;







        }
    }
}
