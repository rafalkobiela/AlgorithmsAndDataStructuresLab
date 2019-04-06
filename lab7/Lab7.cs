using System;
using System.Linq;
using ASD.Graphs;

namespace Lab7
{

public class BestCitiesSolver : MarshalByRefObject
    {

    public (int c1, int c2, int? bypass, double time, Edge[] path)? FindBestCitiesPair(Graph times, int[] passThroughCityTimes, int[] nominatedCities, bool buildBypass)
        {
        return (-1,-1, null, -1, null);
        }

    }

}

