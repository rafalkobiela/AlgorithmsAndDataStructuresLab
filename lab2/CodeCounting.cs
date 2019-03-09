
using System;
using System.Collections.Generic;

namespace ASD
{

public class CodesCounting : MarshalByRefObject
    {

    public int CountCodes(string text, string[] codes, out int[][] solutions)
        {

            int[] tab = new int[text.Length + 1];

            var solList = new List<int[]>[text.Length + 1];

            tab[0] = 1;

            for (int i = 1; i < tab.Length; i++)
            {
                foreach(var code in codes)
                {
                    if (text.Substring(0, i).EndsWith(code))
                    {
                        if(tab[i - code.Length] != 0)
                        {
                            tab[i] += tab[i - code.Length]; 
                        }
                    }
                }
            }

            //var solList = new List<int>[tab[text.Length + 1]];
            //var solList = new List<int>[tab[text.Length] + 1, ];  // 3 wymiary , dla każdego znaku, dla każdego sposobu kodowania, dla każdej składowej kodowania

            


            for (int i = 1; i < tab.Length; i++)
            {
                for(int j=0; j<codes.Length; j++)
                {
                    if (text.Substring(0, i).EndsWith(codes[j]))
                    {

                        foreach (var lista in solList[i - codes.Length])
                        {
                            int[] new_tab = new int[lista.Length + 1];
                            for (int k = 0; k <= lista.Length; k++)
                            {
                                new_tab[k] = lista[j];
                                new_tab[new_tab.Length - 1] = k;
                                solList[i].Add(new_tab);
                            }
                        }
                    }
                }
            }

            


            solutions = null;  // do policzenia w czesci drugiej
        return tab[text.Length];
        }

    }

}
