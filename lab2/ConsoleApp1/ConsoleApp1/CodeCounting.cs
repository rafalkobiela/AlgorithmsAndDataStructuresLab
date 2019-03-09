using System;
using System.Collections.Generic;

namespace ASD
{

    public class CodesCounting : MarshalByRefObject
    {


        public int CountCodes(string text, string[] codes, out int[][] solutions)
        {

            int[] tab = new int[text.Length + 1];
            int k;

            List<List<int>>[] solList = new List<List<int>>[text.Length + 1];


            for (int i = 0; i < solList.Length; i++)
            {
                solList[i] = new List<List<int>>();
            }

            solList[0].Add(new List<int>());
            tab[0] = 1;

            for (int i = 1; i < tab.Length; i++)
            {
                for (int j = 0; j < codes.Length; j++)
                {
                    if (text.Substring(0, i).EndsWith(codes[j]))
                    {
                        if (tab[i - codes[j].Length] != 0)
                        {
                            tab[i] += tab[i - codes[j].Length];
                        }


                        k = i - codes[j].Length;
                        foreach (List<int> z in solList[k])
                        {
                            var tmpList = new List<int>(z);
                            tmpList.Add(j);
                            solList[i].Add(tmpList);
                        }
                    }
                }
            }


            solutions = new int[solList[solList.Length - 1].Count][];

            for (int i = 0; i < solutions.Length; i++)
            {
                solutions[i] = solList[solList.Length - 1][i].ToArray();
            }


            return tab[text.Length];
        }

    }

}