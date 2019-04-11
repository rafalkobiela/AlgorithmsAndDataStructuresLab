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

                for (int i = 0; i < nominatedCities.Length; i++)
                //foreach (var start in nominatedCities)
                {

                    int start = nominatedCities[i];

                    double minDistTmp = double.MaxValue;
                    int minCityTmp = -1;
                    Edge[] shortestPathTmp = { };

                    g_cloned.DijkstraShortestPaths(start, out PathsInfo[] d);

                    for (int j = i; j < nominatedCities.Length; j++)
                    //foreach (var end in nominatedCities)
                    {

                        int end = nominatedCities[j];
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
                PathsInfo[] tmpPathInfo = { };
                int i1 = -1;
                int j1 = -1;

                //var pathsToAllVertices = new HashTable<int, PathsInfo[]>();

                var pathsToAllVerticesTab = new PathsInfo[nominatedCities.Length][];

                for (int i = 0; i < nominatedCities.Length ; i++)

                {
                    int start = nominatedCities[i];

                    g_cloned.DijkstraShortestPaths(start, out PathsInfo[] d);
                    //pathsToAllVertices.Insert(start, d);
                    pathsToAllVerticesTab[i] = d;
                    for (int j = i; j < nominatedCities.Length; j++)

                    {
                        int end = nominatedCities[j];
                        if (start != end)
                        {

                            d[end].Dist -= passThroughCityTimes[end] / 2 + passThroughCityTimes[start] / 2;

                            if (d[end].Dist < time)
                            {
                                time = d[end].Dist;
                                minCity1 = start;
                                minCity2 = end;
                                tmpPathInfo = d;
                                i1 = i;
                                j1 = j;
                            }
                        }
                    }
                }

                for (int i = 0; i < nominatedCities.Length; i++)
                {
                    int start = nominatedCities[i];

                    for (int j = i; j < nominatedCities.Length; j++)

                    {
                        int end = nominatedCities[j];
                        for (int ii = 0; ii < g_cloned.VerticesCount; ii++)
                        {

                            if (start != end && ii != start && ii != end)
                            {

                                double timeThroughICity = pathsToAllVerticesTab[i][ii].Dist + pathsToAllVerticesTab[j][ii].Dist - passThroughCityTimes[ii] -
                                       0.5 * passThroughCityTimes[start] - 0.5 * passThroughCityTimes[end];

                                if (timeThroughICity < time && timeThroughICity >= 0)
                                {

                                    byPassCity = ii;
                                    time = timeThroughICity;
                                    minCity1 = start;
                                    minCity2 = end;
                                    i1 = i;
                                    j1 = j;
                                }
                            }
                        }
                    }
                }


                if (time == double.MaxValue)
                {
                    return null;
                }
                else if (byPassCity != -1)
                {

                    var path1 = PathsInfo.ConstructPath(minCity1, byPassCity, pathsToAllVerticesTab[i1]);
                    var path2 = PathsInfo.ConstructPath(minCity2, byPassCity, pathsToAllVerticesTab[j1]);
                    Array.Reverse(path2);

                    for (int i = 0; i < path2.Length; i++)
                    {
                        path2[i] = new Edge(path2[i].To, path2[i].From);
                    }

                    shortestPath = new Edge[path1.Length + path2.Length];
                    Array.Copy(path1, shortestPath, path1.Length);
                    Array.Copy(path2, 0, shortestPath, path1.Length, path2.Length);


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

