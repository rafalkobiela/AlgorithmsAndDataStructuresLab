using System;
using System.Collections.Generic;
using ASD.Graphs;

namespace ASD
{
    public class Lab03 : MarshalByRefObject
    {
        // Część 1
        //  Sprawdzenie czy podany ciąg stopni jest grafowy
        //  0.5 pkt
        public bool IsGraphic(int[] sequence)
        {
            return false;
        }

        //Część 2
        // Konstruowanie grafu na podstawie podanego ciągu grafowego
        // 1.5 pkt
        public Graph ConstructGraph(int[] sequence)
        {
            return null;
        }

        //Część 3
        // Wyznaczanie minimalnego drzewa (bądź lasu) rozpinającego algorytmem Kruskala
        // 2 pkt
        public Graph MinimumSpanningTree(Graph graph, out double min_weight)
        {
            min_weight = 0;
            return null;
        }
    }
}