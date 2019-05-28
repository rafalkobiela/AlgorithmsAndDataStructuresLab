using ASD.Graphs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

            bool allIsolated = true;
            int maxV = 0;
            double maxVal = 0;
            for (int i = 0; i < n; i++)
            {
                if (taskGraph.OutEdges(i).Count() > 0)
                {

                    allIsolated = false;
                    break;
                }
                if(maxVal < taskTimes[i])
                {
                    maxV = i;
                    maxVal = taskTimes[i];
                }
            }

            if (allIsolated)
            {
                startTimes = new double[n];
                for (int i = 0; i < n; i++)
                {
                    startTimes[i] = maxVal- taskTimes[i];
                }

                criticalPath = new int[] { maxV };

                return maxVal;
            }

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

            Graph g = taskGraph.IsolatedVerticesGraph(true, n + 2);


            for (int i = 0; i < n; i++)
            {
                foreach (var v in reversedGraph.OutEdges(i))
                {
                    g.AddEdge(i, v.To, -taskTimes[i]);

                }
            }

            //n - zrodlo
            //n+1 ujscie

            foreach (var i in endingVertices)
            {
                g.AddEdge(n, i, 0);
            }

            foreach (var i in startingVertices)
            {
                g.AddEdge(i, n + 1, -taskTimes[i]);
            }


            g.DAGShortestPaths(n, out PathsInfo[] d);
            double programTime = -d[n + 1].Dist;

            startTimes = new double[n];

            for (int i = 0; i < n; i++)
            {
                startTimes[i] = programTime + d[i].Dist - taskTimes[i];
            }

            int startigVertexForCriticalPath = 0;

            var longestPath = PathsInfo.ConstructPath(n, n + 1, d);

            startigVertexForCriticalPath = longestPath[longestPath.Length - 1].From;


            Edge[] path = PathsInfo.ConstructPath(n, startigVertexForCriticalPath, d);


            criticalPath = new int[path.Length];


            for (int i = 0; i < criticalPath.Length; i++)
            {
                criticalPath[i] = path[i].To;
            }

            Array.Reverse(criticalPath);


            return -d[n + 1].Dist;


        }
    }
}
