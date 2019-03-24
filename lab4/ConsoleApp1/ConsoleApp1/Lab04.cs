using System;

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

        private void PrintAllPlans(TaxAction[,][] tab)
        {

            for (int i = 0; i < tab.GetLength(0); i++)
            {
                for (int j = 0; j < tab.GetLength(1); j++)
                {
                    foreach (var z in tab[i, j])
                    {
                        Console.Write(z.ToString() + ", ");
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("");
            }

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

        private int WhichTab(int i)
        {
            if (i % 2 == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public int CollectMaxTax(int[] dist, int[] money, int[] carrots, int maxCarrots, int startingCarrots, out TaxAction[] collectingPlan)
        {

            bool ifPrint = false;

            collectingPlan = null;
            int max = 0;
            int curr = 0;
            int prev = 0;
            var tab = new int[dist.Length, maxCarrots + 1];

            TaxAction[] plan = new TaxAction[dist.Length];




            for (int i = 0; i < maxCarrots + 1; i++)
            {
                tab[0, i] = -1;
            }



            tab[0, startingCarrots] = money[0];
            tab[0, Math.Min(maxCarrots, startingCarrots + carrots[0])] = 0;



            for (int miasto = 1; miasto < dist.Length; miasto++) // miasta
            {

                for (int j = 0; j < maxCarrots + 1; j++) // marchewki
                {
                    tab[miasto, j] = -1;
                }

                curr = WhichTab(miasto);
                prev = Math.Abs(curr - 1);

                for (int marchewka = 0; marchewka < maxCarrots + 1; marchewka++) // marchewki
                {

                    if (marchewka + dist[miasto] < maxCarrots + 1) // bierzemy hajs
                    {
                        if (tab[miasto - 1, marchewka + dist[miasto]] >= 0)
                        {
                            //Array.Copy(plan[prev, j + dist[i]], 0, plan[curr, j], 0, i);
                            tab[miasto, marchewka] = tab[miasto - 1, marchewka + dist[miasto]] + money[miasto];
                            //plan[curr, j][i] = TaxAction.TakeMoney;
                            //plan2[i] = TaxAction.TakeMoney;

                        }
                    }


                    if (marchewka + dist[miasto] - carrots[miasto] < maxCarrots + 1 && marchewka - carrots[miasto] >= 0)
                    {
                        if (tab[miasto - 1, marchewka + dist[miasto] - carrots[miasto]] > tab[miasto, marchewka])
                        {
                            //Array.Copy(plan[prev, j + dist[i] - carrots[i]], 0, plan[curr, j], 0, i);
                            //plan[curr, j][i] = TaxAction.TakeCarrots;
                            tab[miasto, marchewka] = tab[miasto - 1, marchewka + dist[miasto] - carrots[miasto]];
                            //plan2[i] = TaxAction.TakeCarrots;
                        }
                    }


                    if (marchewka == maxCarrots)
                    {
                        for (int k = marchewka - carrots[miasto] + dist[miasto]; k <= maxCarrots; k++)
                        {
                            if (k >= 0 && dist[miasto] <= k)
                            {
                                if (tab[miasto - 1, k] > tab[miasto, marchewka])
                                {
                                    //Array.Copy(plan[prev, k], 0, plan[curr, j], 0, i);
                                    //plan[curr, j][i] = TaxAction.TakeCarrots;
                                    tab[miasto, marchewka] = tab[miasto - 1, k];

                                    //plan2[i] = TaxAction.TakeCarrots;
                                }
                            }
                        }
                    }
                }

            }


            max = -1;
            int indexMax = -1;
            for (int i = startingCarrots; i < maxCarrots + 1; i++)
            {
                if (max < tab[dist.Length - 1, i])
                {
                    max = tab[dist.Length - 1, i];
                    indexMax = i;
                }
            }


            if (indexMax >= 0)
            {

                for (int miasto = dist.Length - 1; miasto > 0; miasto--)
                {
                    if (indexMax + dist[miasto] <= maxCarrots)
                    {
                        if (tab[miasto - 1, indexMax + dist[miasto]] + money[miasto] == tab[miasto, indexMax] && tab[miasto - 1, indexMax + dist[miasto]] != -1)
                        {
                            plan[miasto] = TaxAction.TakeMoney;
                            indexMax = indexMax + dist[miasto];
                        }
                        else
                        {

                            if (indexMax < maxCarrots)
                            {
                                indexMax = indexMax + dist[miasto] - carrots[miasto];
                                plan[miasto] = TaxAction.TakeCarrots;
                            }
                            else
                            {
                                for (int k = indexMax - carrots[miasto] + dist[miasto]; k < indexMax + dist[miasto] - 1; k++)
                                {
                                    if (k >= 0)
                                    {
                                        if (tab[miasto - 1, k] == tab[miasto, indexMax])
                                        {
                                            indexMax = k;
                                            plan[miasto] = TaxAction.TakeCarrots;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (indexMax < maxCarrots)
                        {
                            indexMax = indexMax + dist[miasto] - carrots[miasto];
                            plan[miasto] = TaxAction.TakeCarrots;
                        }
                        else
                        {
                            for (int k = indexMax + dist[miasto] - 1; k >= indexMax - carrots[miasto] + dist[miasto]; k--)
                            {
                                if (k >= 0 && k <= maxCarrots)
                                {
                                    if (tab[miasto - 1, k] == tab[miasto, indexMax])
                                    {
                                        indexMax = k;
                                        plan[miasto] = TaxAction.TakeCarrots;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                }


                if (tab[0, indexMax] == money[0])
                {
                    plan[0] = TaxAction.TakeMoney;
                }
                else
                {
                    plan[0] = TaxAction.TakeCarrots;
                }
                //PrintPlan(collectingPlan);
            }

            //plan2 = collectingPlan;
            if (ifPrint)
            {
                PrintPlan(plan);
            }
            if (max >= 0)
            {
                collectingPlan = plan;
            }
            return max;
        }

    }
}
