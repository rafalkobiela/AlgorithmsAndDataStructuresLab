using System;
using System.Collections.Generic;

namespace ASD
{
    public enum TaxAction
    {
        Empty,
        TakeMoney,
        TakeCarrots
    }

    public class TaxCollectorManager : MarshalByRefObject
    {
        private void print(int[,] tab)
        {
            Console.WriteLine("begin print --------------------");
            for (int i = 0; i < tab.GetLength(0); i++)
            {
                for (int j = 0; j < tab.GetLength(1); j++)
                {
                    Console.Write(tab[i, j] + ", ");
                }
                Console.WriteLine("");
            }

            Console.WriteLine("End print ----------------------------");
        }


        private void PrintPlan(TaxAction[] plan)
        {
            Console.WriteLine("---------------");
            foreach (var i in plan)
            {
                Console.Write(i.ToString() + ", ");
            }
            Console.WriteLine("");
            Console.WriteLine("---------------");
        }

        public int CollectMaxTax(int[] dist, int[] money, int[] carrots, int maxCarrots, int startingCarrots, out TaxAction[] collectingPlan)
        {

            bool ifPrint = false;

            collectingPlan = null;
            int max = 0;

            var tab = new int[dist.Length, maxCarrots + 1];

            List<TaxAction>[,] plan = new List<TaxAction>[dist.Length, maxCarrots + 1];




            for (int i = 0; i < maxCarrots + 1; i++)
            {
                tab[0, i] = -1;
                plan[0, i] = new List<TaxAction>();
            }

            plan[0, startingCarrots].Add(TaxAction.TakeMoney);
            plan[0, Math.Min(maxCarrots, startingCarrots + carrots[0])].Add(TaxAction.TakeCarrots);

            tab[0, startingCarrots] = money[0];
            tab[0, Math.Min(maxCarrots, startingCarrots + carrots[0])] = 0;




            for (int i = 1; i < dist.Length; i++) // miasta
            {
                bool hajs = false;
                bool czyscilem_liste = false;
                for (int j = 0; j < maxCarrots + 1; j++) // marchewki
                {
                    tab[i, j] = -1;


                }

                for (int j = 0; j < maxCarrots + 1; j++) // marchewki
                {

                    if (j + dist[i] < maxCarrots + 1) // bierzemy hajs
                    {
                        if (tab[i - 1, j + dist[i]] >= 0)
                        {
                            plan[i, j] = new List<TaxAction>(plan[i - 1, j + dist[i]]);

                            tab[i, j] = tab[i - 1, j + dist[i]] + money[i];
                            plan[i, j].Add(TaxAction.TakeMoney);
                            hajs = true;
                        }

                    }


                    if (j - carrots[i] > -dist[i] && j + dist[i] - carrots[i] < maxCarrots + 1 && j - carrots[i] >= 0) // tu nie wiem czy ostatni warunek ok                     
                    {
                        if (tab[i - 1, j + dist[i] - carrots[i]] >= 0) // bierzemy marchewki
                        {
                            if (tab[i - 1, j + dist[i] - carrots[i]] > tab[i, j])
                            {

                                if (tab[i, j] != -1)
                                {
                                    plan[i, j] = new List<TaxAction>(plan[i - 1, j + dist[i] - carrots[i]]);
                                    plan[i, j].Add(TaxAction.TakeCarrots);
                                }
                                else
                                {
                                    plan[i, j] = new List<TaxAction>(plan[i - 1, j + dist[i] - carrots[i]]);
                                    plan[i, j].Add(TaxAction.TakeCarrots);
                                }
                                tab[i, j] = tab[i - 1, j + dist[i] - carrots[i]];
                            }

                        }
                    }


                    if (j == maxCarrots)
                    {

                        for (int k = j - carrots[i] + dist[i]; k <= maxCarrots; k++)
                        {
                            if (k >= 0 && dist[i] <= k)
                            {

                                if (tab[i - 1, k] > tab[i, j])
                                {

                                    plan[i, j] = new List<TaxAction>(plan[i - 1, k]);
                                    plan[i, j].Add(TaxAction.TakeCarrots);
                                    tab[i, j] = tab[i - 1, k];
                                }

                            }
                        }
                    }

                    if (ifPrint)
                    {
                        Console.WriteLine("");
                        if (tab[i, j] != -1)
                        {
                            Console.WriteLine($"Current city: {i}, Current carrots: {j}, Czy wzialem hajs: {hajs}, czyscilem list: {czyscilem_liste}");
                            hajs = false;
                            PrintPlan(plan[i, j].ToArray());
                            print(tab);

                        }

                    }
                }

            }
            if (ifPrint)
            {
                print(tab);

            }

            max = -1;
            int index = -1;
            for (int i = startingCarrots; i < maxCarrots + 1; i++)
            {
                if (max < tab[dist.Length - 1, i])
                {
                    max = tab[dist.Length - 1, i];
                    index = i;
                }
            }


            if (index >= 0 && max != -1)
            {
                collectingPlan = plan[plan.GetLength(0) - 1, index].ToArray();
            }
            if (ifPrint && max != -1)
            {
                PrintPlan(collectingPlan);
                Console.WriteLine($"Final result: {max}");

            }
            return max;
        }

    }
}
