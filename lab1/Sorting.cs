
using System;

namespace ASD
{

//
// Za kazdy z algorytmow jest 1 pkt.
//

public class SortingMethods: MarshalByRefObject
    {


    public int Partition(int l, int r, ref int[] A)
        {

            int v = A[l];
            int i = l;
            int j = r + 1;
            do
            {
                do
                {
                    i++;
                } while (A[i] < v);
                do
                {
                    j--;
                } while (A[j] > v);
                if (i < j)
                {
                    int tmp = A[i];
                    A[i] = A[j];
                    A[j] = tmp;
                }
            } while (i < j);
            A[l] = A[j];
            A[j] = v;

            return j;
        }

        public int Partition2(int l, int r, ref int[] A)
        {
            int tmp;
            int v = A[r];
            int i = l - 1;
            
            for(int j = l; j <= r-1; j++)
            {
                if(A[j] <= v)
                {
                    i++;
                    tmp = A[i];
                    A[i] = A[j];
                    A[j] = tmp;
                }
            }

                tmp = A[i + 1];
                A[i + 1] = A[r];
                A[r] = tmp;
               

            

            return i+1;
        }

        public void QuickSort (int l, int r, ref int[] tab)
        {
            int j = Partition2(l, r, ref tab);
            if(j-1 > l)
            {
                QuickSort(l, j - 1, ref tab);
            }
            if(j+1 < r)
            {
                QuickSort(j + 1, r, ref tab);
            }
        }



    public int[] QuickSort(int[] tab)
        {
            if (tab.Length <= 1)
            {
                return tab;
            }   
            QuickSort(0, tab.Length - 1, ref tab);


            
            return tab;
        }

    public int[] ShellSort(int[] tab)
        {

            int n = tab.Length;
            int h = 2;
            while (h-1 < n / 2)
            {
                h *= 2;
            }
            h--;
            while (h >= 1)
            {
                for (int j=h+1; j < n; j++)
                {
                    int v = tab[j];
                    int i = j - h;
                    while (i>0 && tab[i] > v)
                    {
                        tab[i + h] = tab[i];
                        i -= h;
                    }
                    tab[i + h] = v;
                }
                h = ((h + 1) / 2) - 1;
            }


            for (int i = 0; i < tab.Length - 1; i++)
            {
                int tmp;
                tmp = tab[i];
                if (tab[i] > tab[i + 1])
                {
                    tmp = tab[i + 1];
                    tab[i + 1] = tab[i];
                    tab[i] = tmp;
                }

            }
            return tab;
        }

    public int[] HeapSort(int[] tab)
        {
        //
        // TODO
        //
        return tab;
        }

        public int[] Merge(int l, int m, int r, ref int[] tab)
            //int[] tab1, int[] tab2)
        {

            int[] tab1 = new int[m-l + 1];
            int[] tab2 = new int[r - m];

            for (int i = 0; i<tab1.Length; i++)
            {
                tab1[i] = tab[i + l];
            }
            for (int i = 0; i < tab1.Length; i++)
            {
                tab2[i] = tab[i + m + 1];
            }
            int[] new_tab = new int[tab1.Length + tab2.Length ];

            int a = 0;
            int b = 0;

            for(int i = 0; i<new_tab.Length; i++)
            {
                if(a < tab1.Length && b < tab2.Length)
                {
                    if (tab1[a] > tab2[b])
                    {
                        new_tab[i] = tab2[b];
                        b++;
                    }
                    else
                    {
                        new_tab[i] = tab1[a];
                        a++;
                    }
                }else if(a == tab1.Length)
                {
                    new_tab[new_tab.Length - 1] = tab2[tab2.Length - 1];
                }
                else
                {
                    new_tab[new_tab.Length - 1] = tab1[tab1.Length - 1];
                }

            }

            for (int i = 0; i < tab1.Length; i++)
            {
                tab[i + l] = tab1[i];
            }
            for (int i = 0; i < tab2.Length; i++)
            {
               tab[i + m + 1] = tab2[i];
            }
            
            return tab;
        }

        public void MergeSort(int l, int r, ref int[] tab)
        {
            if (l == r) return;

            int m = (l + r) / 2;
            MergeSort(l, m, ref tab);
            MergeSort(m+1, r, ref tab);

             Merge(l, m, r,ref tab);

        }

    public int[] MergeSort(int[] tab)
        {
            MergeSort(1, tab.Length - 1, ref tab);
            return tab;
        }

    }

}
