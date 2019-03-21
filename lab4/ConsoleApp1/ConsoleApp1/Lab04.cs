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
      


        public int CollectMaxTax(int[] dist, int[] money, int[] carrots, int maxCarrots, int startingCarrots, out TaxAction[] collectingPlan)
        {
            collectingPlan = null;
            int max = 0;

            var tab = new int[dist.Length , maxCarrots + 1];

            for(int i = 0; i < maxCarrots + 1; i++)
            {
                tab[0, i] = -1;
            }
            tab[0, startingCarrots] = money[0];
            tab[0, Math.Min(maxCarrots, startingCarrots + carrots[0])] = 0;

            for (int i = 1; i< dist.Length; i++) // miasta
            {
                for(int j = 0; j < maxCarrots + 1; j++) // marchewki
                {
                    tab[i, j] = -1;
                }
                for(int j = 0; j < maxCarrots + 1; j++) // marchewki
                {
                    if(dist[i] + j <= maxCarrots && tab[i-1, dist[i] + 1] >= 0)
                    {

                        tab[i, j] = tab[i - 1, dist[i] + j] + money[i];

                    }
                    
                    if(j>= carrots[i] && j - carrots[i] + dist[i] <= maxCarrots)
                    {
                        tab[i, j] = Math.Max(tab[i, j], tab[i - 1, j - carrots[i] + dist[i]]);
                    }

                    if(j == maxCarrots && j - carrots[i] + dist[i] >=0)
                    {
                        for(int k = j - carrots[i] + dist[i]; k <= maxCarrots; k++)
                        {
                            tab[i, j] = Math.Max(tab[i - 1, k], tab[i-1 , k]);
                        }
                    }

                    //tab[i, j] = 1;
                }

            }


            max = -1;
            for (int i = 0; i < maxCarrots + 1; i++)
            {
                if(max < tab[dist.Length - 1, i])
                {
                    max =  tab[dist.Length - 1, i];
                }
            }

            return max;
        }

    }
}
