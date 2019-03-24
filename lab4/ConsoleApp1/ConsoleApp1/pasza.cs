using System;
namespace ASD
{
    public enum TaxAction2
    {
        Empty,
        TakeMoney,
        TakeCarrots
    }

    public class TaxCollectorManager2 : MarshalByRefObject
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
        public int CollectMaxTax(int[] dist, int[] money, int[] carrots, int maxCarrots, int startingCarrots, out TaxAction[] collectingPlan)
        {

            collectingPlan = null;
            int max = -2;

            //int dist.Length = dist.Length;
            int[,] tab = new int[maxCarrots + 1, dist.Length];

            for (int i = 0; i < maxCarrots + 1; i++)
            {
                for (int j = 0; j < dist.Length; j++)
                {
                    tab[i, j] = -1;
                }
            }

            tab[Math.Min(startingCarrots + carrots[0], maxCarrots), 0] = 0;
            tab[startingCarrots, 0] = money[0];

            for (int i = 1; i < dist.Length; i++)
            {
                int vm = money[i];
                int vd = dist[i];
                int vc = carrots[i];
                for (int j = vd; j <= maxCarrots; j++)
                {
                    if (tab[j, i - 1] >= 0)
                    {
                        tab[j - vd, i] = Math.Max(tab[j - vd, i], tab[j, i - 1] + vm);

                        tab[Math.Min(maxCarrots, j - vd + vc), i] = Math.Max(tab[Math.Min(maxCarrots, j - vd + vc), i], tab[j, i - 1]);
                    }
                }
            }
            max = -1;
            int final_carrots = -1;
            for (int i = startingCarrots; i < maxCarrots + 1; i++)
            {
                if (tab[i, dist.Length - 1] > max)
                {
                    max = tab[i, dist.Length - 1];
                    final_carrots = i;
                }
            }

            print(tab);
            
            if (final_carrots >= 0)
            {
                int indexMax = final_carrots;
                int act_mon = max;
                collectingPlan = new TaxAction[dist.Length];


                for (int i = dist.Length - 1; i > 0; i--)
                {
                    if (indexMax + dist[i] <= maxCarrots &&
                        act_mon - money[i] >= 0 &&
                        tab[indexMax + dist[i], i - 1] == act_mon - money[i])
                    {
                        collectingPlan[i] = TaxAction.TakeMoney;
                        act_mon -= money[i];
                        indexMax = Math.Min(indexMax + dist[i], maxCarrots);
                    }
                    else
                    {
                        collectingPlan[i] = TaxAction.TakeCarrots;
                        if (indexMax == maxCarrots)
                        {
                            int min_car = Math.Max(indexMax - carrots[i] + dist[i], 0);
                            for (int j = Math.Min(maxCarrots, indexMax + dist[i] - 1); j >= min_car; j--)
                            {
                                if (tab[j, i - 1] == act_mon)
                                {
                                    indexMax = j;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            indexMax -= carrots[i];
                            indexMax = Math.Min(indexMax + dist[i], maxCarrots);
                        }
                    }
                }

                if (indexMax + dist[0] <= maxCarrots &&
                        act_mon - money[0] >= 0 &&
                        0 == act_mon - money[0])
                {
                    collectingPlan[0] = TaxAction.TakeMoney;
                }
                else
                {
                    collectingPlan[0] = TaxAction.TakeCarrots;
                }
            }
            return max;
        }

    }
}