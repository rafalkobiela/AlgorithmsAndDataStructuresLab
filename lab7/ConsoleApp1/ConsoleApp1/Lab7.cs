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
            else /*budujemy obwodnicê*/
            {
                int minCity1 = int.MaxValue;
                int minCity2 = int.MaxValue;
                double time = double.MaxValue;
                Edge[] shortestPath = { };
                int byPassCity = -1;
                PathsInfo[] tmpTmpPathInfo = { };
                PathsInfo[] tmpPathInfo = { };
                int counter = 0;

                //if (g_cloned.VerticesCount == 12 * 12)
                //{
                //    GraphExport ge = new GraphExport();
                //    ge.Export(g_cloned);
                //}

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


                for (int i = 0; i < g_cloned.VerticesCount; i++) // zerujemy jakieœ miasta
                {

                    //if (g_cloned.VerticesCount > 100 && counter > 20)
                    //    break;

                    foreach (var potentialByPass in g_cloned.OutEdges(i))
                    {
                        g_cloned.ModifyEdgeWeight(i, potentialByPass.To, -0.5 * passThroughCityTimes[i]);
                    }


                    foreach (var start in nominatedCities)
                    {

                        double minDistTmp = double.MaxValue;
                        int minCityTmp = -1;
                        int byPassCityTmp = -1;
                        Edge[] shortestPathTmp = { };

                        //if (time - passThroughCityTimes[i] > 0)
                        //{
                        //    continue;
                        //}


                        g_cloned.DijkstraShortestPaths(start, out PathsInfo[] d);

                        foreach (var end in nominatedCities)
                        {
                            counter++;
                            if (start != end && i != start && i != end)
                            {


                                d[end].Dist -= passThroughCityTimes[end] / 2 + passThroughCityTimes[start] / 2;
                                if (d[end].Dist < time && d[end].Dist < minDistTmp)
                                {

                                    minDistTmp = d[end].Dist;
                                    minCityTmp = end;

                                    tmpTmpPathInfo = d;
                                    //shortestPathTmp = PathsInfo.ConstructPath(start, end, d);
                                    byPassCityTmp = i;
                                    //Console.WriteLine($"start: {start}, end: {end}, byPass: {i}");
                                }

                            }
                        }



                        if (minCityTmp != -1)
                        {
                            if (minDistTmp < time)
                            {
                                time = minDistTmp;
                                minCity1 = start;
                                minCity2 = minCityTmp;
                                shortestPath = shortestPathTmp;
                                byPassCity = byPassCityTmp;
                                tmpPathInfo = tmpTmpPathInfo;
                            }
                        }
                    }

                    foreach (var potentialByPass in g_cloned.OutEdges(i))
                    {
                        g_cloned.ModifyEdgeWeight(i, potentialByPass.To, 0.5 * passThroughCityTimes[i]);
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

                if (byPassCity == -1)
                {
                    return (minCity1, minCity2, null, time, shortestPath);
                }
                return (minCity1, minCity2, byPassCity, time, shortestPath);



            }
        }

    }

}

