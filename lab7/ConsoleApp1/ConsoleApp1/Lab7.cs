using ASD.Graphs;
using System;

namespace Lab7
{
    public class BestCitiesSolver : MarshalByRefObject
    {



        public void print(PathsInfo[] a)
        {

            foreach (var i in a)
            {
                Console.WriteLine(i.Last);
            }

        }

        public (int c1, int c2, int? bypass, double time, Edge[] path)? FindBestCitiesPair(Graph times, double[] passThroughCityTimes, int[] nominatedCities, bool buildBypass)
        {
            var g_cloned = times.Clone();

            int n = g_cloned.VerticesCount;

            for (int v = 0; v < n; v++)
            {
                foreach (Edge e in g_cloned.OutEdges(v))
                {
                    g_cloned.ModifyEdgeWeight(v, e.To, passThroughCityTimes[v] / 2);
                }
            }

            if (!buildBypass)
            {

                int minCity1 = int.MaxValue;
                int minCity2 = int.MaxValue;
                double time = double.MaxValue;
                PathsInfo[] tmpTmpPathInfo = { };
                PathsInfo[] tmpPathInfo = { };
                Edge[] shortestPath = { };

                foreach (var start in nominatedCities)
                {


                    double minDistTmp = double.MaxValue;
                    int minCityTmp = -1;
                    Edge[] shortestPathTmp = { };

                    g_cloned.DijkstraShortestPaths(start, out PathsInfo[] d);


                    foreach (var end in nominatedCities)
                    {

                        if (start != end)
                        {

                            d[end].Dist -= passThroughCityTimes[end] / 2 + passThroughCityTimes[start] / 2;

                            if (d[end].Dist < time && d[end].Dist < minDistTmp)
                            {
                                minDistTmp = d[end].Dist;
                                minCityTmp = end;
                                tmpTmpPathInfo = d;

                            }
                        }
                    }
                    if (minCityTmp != -1)
                    {
                        if (time > minDistTmp)
                        {
                            time = minDistTmp;
                            minCity1 = start;
                            minCity2 = minCityTmp;
                            tmpPathInfo = tmpTmpPathInfo;
                        }
                    }
                }

                if (time == double.MaxValue)
                {
                    return null;
                }
                else
                {
                    shortestPath = PathsInfo.ConstructPath(minCity1, minCity2, tmpPathInfo);
                }

                return (minCity1, minCity2, null, time, shortestPath);
            }
            else /*budujemy obwodnicę*/
            {
                int minCity1 = int.MaxValue;
                int minCity2 = int.MaxValue;
                double time = double.MaxValue;
                Edge[] shortestPath = { };
                int byPassCity = -1;
                PathsInfo[] tmpTmpPathInfo = { };
                PathsInfo[] tmpPathInfo = { };
                int counter = 0;

                //var pathsToAllVertices = new PathsInfo[nominatedCities.Length][];

                var pathsToAllVertices = new HashTable<int, PathsInfo[]>();

                //if (g_cloned.VerticesCount == 12 * 12)
                //{
                //GraphExport ge = new GraphExport();
                //ge.Export(g_cloned);
                //}

                foreach (var start in nominatedCities)
                {

                    double minDistTmp = double.MaxValue;
                    int minCityTmp = -1;
                    Edge[] shortestPathTmp = { };

                    g_cloned.DijkstraShortestPaths(start, out PathsInfo[] d);


                    pathsToAllVertices.Insert(start, d);

                    foreach (var end in nominatedCities)
                    {
                        if (start != end)
                        {

                            d[end].Dist -= passThroughCityTimes[end] / 2 + passThroughCityTimes[start] / 2;

                            if (d[end].Dist < minDistTmp && d[end].Dist < time)
                            {
                                minDistTmp = d[end].Dist;
                                minCityTmp = end;
                                tmpTmpPathInfo = d;
                                //shortestPathTmp = PathsInfo.ConstructPath(start, end, d);
                            }
                        }
                    }
                    if (minCityTmp != -1)
                    {
                        if (time > minDistTmp)
                        {
                            time = minDistTmp;
                            minCity1 = start;
                            minCity2 = minCityTmp;
                            tmpPathInfo = tmpTmpPathInfo;
                            //shortestPath = shortestPathTmp;
                        }
                    }
                }


                foreach (var start in nominatedCities)
                {
                    double minDistTmp = double.MaxValue;
                    int minCityTmp1 = -1;
                    int minCityTmp2 = -1;
                    int byPassCityTmp = -1;

                    foreach (var end in nominatedCities)
                    {
                        
                        for (int i = 0; i < g_cloned.VerticesCount; i++)
                        {

                            if (start != end && i != start && i != end)
                            {

                                double timeThroughICity = pathsToAllVertices[start][i].Dist + pathsToAllVertices[end][i].Dist - passThroughCityTimes[i] -
                                       0.5 * passThroughCityTimes[start] - 0.5 * passThroughCityTimes[end];

                                //if(timeThroughICity < 10)
                                //    Console.WriteLine($"{timeThroughICity}, city{i}, c1: {start}, c2: {end} ");

                                if (timeThroughICity < time && timeThroughICity >= 0)
                                {

                                    byPassCity = i;
                                    time = timeThroughICity;
                                    minCity1 = start;
                                    minCity2 = end;
                                }
                            }
                        }
                    }
                }

                //Console.WriteLine("");
                //Console.WriteLine($"c1: {minCity1}, c2: {minCity2}, byp: {byPassCity}");

                if (time == double.MaxValue)
                {
                    return null;
                }
                else if (byPassCity != -1)
                {




                    var path1 = PathsInfo.ConstructPath(minCity1, byPassCity, pathsToAllVertices[minCity1]);
                    var path2 = PathsInfo.ConstructPath(minCity2, byPassCity, pathsToAllVertices[minCity2]);
                    Array.Reverse(path2);

                    for(int i = 0; i < path2.Length; i++)
                    {
                        path2[i] = new Edge(path2[i].To, path2[i].From);
                        //(path2[i].To, path2[i].From) = (path2[i].From, path2[i].To);
                    }

                    shortestPath = new Edge[path1.Length + path2.Length];
                    Array.Copy(path1, shortestPath, path1.Length);
                    Array.Copy(path2, 0, shortestPath, path1.Length, path2.Length);


                    //foreach (var i in shortestPath)
                    //{
                    //    Console.Write($"{i.ToString()}, ");
                    //}
                    //Console.WriteLine();
                    //Console.WriteLine($"len {shortestPath.Length}");

                    //foreach (var i in g_cloned.OutEdges(byPassCity))
                    //{
                    //    g_cloned.ModifyEdgeWeight(byPassCity, i.To, -0.5 * passThroughCityTimes[byPassCity]);
                    //}

                    //var t1 = DateTime.Now;
                    //g_cloned.DijkstraShortestPaths(minCity1, out PathsInfo[] d);
                    //var t2 = DateTime.Now;



                    //Console.WriteLine(t2 - t1);



                    //shortestPath = PathsInfo.ConstructPath(minCity1, minCity2, d);
                    //time = d[minCity2].Dist - passThroughCityTimes[minCity1] * 0.5 - passThroughCityTimes[minCity2] * 0.5;
                }
                else
                {
                    shortestPath = PathsInfo.ConstructPath(minCity1, minCity2, tmpPathInfo);
                }


                if (byPassCity == -1)
                {
                    return (minCity1, minCity2, null, time, shortestPath);
                }
                return (minCity1, minCity2, byPassCity, time, shortestPath);

            }
        }

    }

}

