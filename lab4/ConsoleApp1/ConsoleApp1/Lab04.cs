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
                    foreach (var z in tab[i,j])
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
            if(i % 2 == 0)
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

            TaxAction[,][] plan = new TaxAction[2, maxCarrots + 1][];

            for (int i = 0; i < plan.GetLength(0); i++)
            {
                for (int j = 0; j < plan.GetLength(1); j++)
                {
                    plan[i, j] = new TaxAction[dist.Length];
                }
            }



            for (int i = 0; i < maxCarrots + 1; i++)
            {
                tab[0, i] = -1;
            }

            plan[0, startingCarrots][0] = TaxAction.TakeMoney;
            plan[0, Math.Min(maxCarrots, startingCarrots + carrots[0])][0] = TaxAction.TakeCarrots;


            tab[0, startingCarrots] = money[0];
            tab[0, Math.Min(maxCarrots, startingCarrots + carrots[0])] = 0;



            for (int i = 1; i < dist.Length; i++) // miasta
            {

                for (int j = 0; j < maxCarrots + 1; j++) // marchewki
                {
                    tab[i, j] = -1;
                }

                curr = WhichTab(i);
                prev = Math.Abs(curr - 1);

                for (int j = 0; j < maxCarrots + 1; j++) // marchewki
                {

                    if (j + dist[i] < maxCarrots + 1) // bierzemy hajs
                    {
                        if (tab[i - 1, j + dist[i]] >= 0)
                        {
                            Array.Copy(plan[prev, j + dist[i]], 0, plan[curr, j], 0, i);
                            tab[i, j] = tab[i - 1, j + dist[i]] + money[i];
                            plan[curr, j][i] = TaxAction.TakeMoney;

                        }
                    }


                    if ( j + dist[i] - carrots[i] < maxCarrots + 1 && j - carrots[i] >= 0)                   
                    {
                            if (tab[i - 1, j + dist[i] - carrots[i]] > tab[i, j])
                            {
                            Array.Copy(plan[prev, j + dist[i] - carrots[i]], 0, plan[curr, j], 0, i);
                            plan[curr, j][i] = TaxAction.TakeCarrots;
                                tab[i, j] = tab[i - 1, j + dist[i] - carrots[i]];
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
                                    Array.Copy(plan[prev, k], 0, plan[curr, j], 0, i);
                                    plan[curr, j][i] = TaxAction.TakeCarrots;
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
                            Console.WriteLine($"Current city: {i}, Current carrots: {j}");

                            PrintAllPlans(plan);
                            //PrintPlan(plan[curr, j]);
                            print(tab);

                        }

                    }
                }

            }
            //if (ifPrint)
            //{
            //    print(tab);

            //}

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
                collectingPlan = plan[curr, index];
            }
            //if (ifPrint && max != -1)
            //{
            //    PrintPlan(collectingPlan);
            //    Console.WriteLine($"Final result: {max}");

            //}
            return max;
        }

    }
}
