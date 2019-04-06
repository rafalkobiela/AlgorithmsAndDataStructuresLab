using ASD.Graphs;
using System;

namespace Lab7
{

    public class BestCitiesSolver : MarshalByRefObject
    {

        public (int c1, int c2, int? bypass, double time, Edge[] path)? FindBestCitiesPair(Graph times, int[] passThroughCityTimes, int[] nominatedCities, bool buildBypass)
        {


            var g_cloned = times.Clone();

            int n = g_cloned.VerticesCount;

            for (int v = 0; v < n; v++)
            {
                foreach (Edge e in g_cloned.OutEdges(v))
                {
                    g_cloned.ModifyEdgeWeight(v, e.To, (double)passThroughCityTimes[v] / 2);
                }
            }

            if (!buildBypass)
            {

                int minCity1 = int.MaxValue;
                int minCity2 = int.MaxValue;
                double time = double.MaxValue;
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

                            d[end].Dist -= (double)passThroughCityTimes[end] / 2 + (double)passThroughCityTimes[start] / 2;

                            if (d[end].Dist < minDistTmp)
                            {

                                minDistTmp = d[end].Dist;
                                minCityTmp = end;
                                shortestPathTmp = PathsInfo.ConstructPath(start, end, d);

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
                            shortestPath = shortestPathTmp;
                        }
                    }
                }

                if (time == double.MaxValue)
                {
                    return null;
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

                foreach (var start in nominatedCities)
                {


                    double minDistTmp = double.MaxValue;
                    int minCityTmp = -1;
                    int byPassCityTmp = -1;
                    Edge[] shortestPathTmp = { };

                    g_cloned.DijkstraShortestPaths(start, out PathsInfo[] d);


                    foreach (var end in nominatedCities)
                    {
                        if (start != end)
                        {

                            d[end].Dist -= (double)passThroughCityTimes[end] / 2 + (double)passThroughCityTimes[start] / 2;

                            if (d[end].Dist < minDistTmp)
                            {

                                minDistTmp = d[end].Dist;
                                minCityTmp = end;
                                shortestPathTmp = PathsInfo.ConstructPath(start, end, d);

                            }


                            for (int i = 0; i < g_cloned.VerticesCount; i++) // zerujemy jakieœ miasta
                            {
                                if (i != start && i != end)
                                {
                                    foreach (var potentialByPass in g_cloned.OutEdges(i))
                                    {
                                        g_cloned.ModifyEdgeWeight(i, potentialByPass.To, -0.5 * (double)passThroughCityTimes[i]);
                                    }

                                    //teraz dijkstra
                                    g_cloned.DijkstraShortestPaths(start, out PathsInfo[] dByPass);

                                    dByPass[end].Dist -= (double)passThroughCityTimes[end] / 2 + (double)passThroughCityTimes[start] / 2;

                                    if (dByPass[end].Dist < minDistTmp)
                                    {

                                        minDistTmp = dByPass[end].Dist;
                                        minCityTmp = end;
                                        shortestPathTmp = PathsInfo.ConstructPath(start, end, dByPass);
                                        byPassCityTmp = i;                                        

                                    }


                                    foreach (var potentialByPass in g_cloned.OutEdges(i))
                                    {
                                        g_cloned.ModifyEdgeWeight(i, potentialByPass.To, 0.5 * (double)passThroughCityTimes[i]);
                                    }

                                }
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
                            shortestPath = shortestPathTmp;
                            byPassCity = byPassCityTmp;
                        }
                    }
                }

                if (time == double.MaxValue)
                {
                    return null;
                }

                if(byPassCity == -1)
                {
                    return (minCity1, minCity2, null, time, shortestPath);
                }
                return (minCity1, minCity2, byPassCity, time, shortestPath);



            }

        }

    }

}

