
using System;

namespace ASD
{

    //
    // Za kazdy z algorytmow jest 1 pkt.
    //

    public class SortingMethods : MarshalByRefObject
    {

        public int Partition(int l, int r, ref int[] A)
        {

            int mid = (l + r) / 2;

            if (A[mid] < A[l])
            {
                int tmp = A[l];
                A[l] = A[mid];
                A[mid] = tmp;

            }
            if (A[r] < A[l])
            {

                int tmp = A[l];
                A[l] = A[r];
                A[r] = tmp;
            }
            if (A[mid] < A[r])
            {
                int tmp = A[mid];
                A[mid] = A[r];
                A[r] = tmp;

            }
            int pivot = A[r];
            int i = l - 1;
            int j = r + 1;
            while (true)
            {

                do
                {
                    i++;
                } while (A[i] < pivot);
                do
                {
                    j--;
                } while (A[j] > pivot);
                if (i >= j)
                {
                    return j;
                }
                int tmp = A[i];
                A[i] = A[j];
                A[j] = tmp;
            }

        }

        public void QuickSort(int l, int r, ref int[] tab)
        {

            if (l < r)
            {
                int p = Partition(l, r, ref tab);
                QuickSort(l, p, ref tab);
                QuickSort(p + 1, r, ref tab);
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
            while (h - 1 < n / 2)
            {
                h *= 2;
            }
            h--;
            while (h >= 1)
            {
                for (int j = h + 1; j < n; j++)
                {
                    int v = tab[j];
                    int i = j - h;
                    while (i > 0 && tab[i] > v)
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

        private int heapSize;
        private void BuildHeap(int[] tab)
        {
            heapSize = tab.Length - 1;
            for (int i = heapSize / 2; i >= 0; i--)
            {
                Heap(tab, i);
            }
        }
        private void Heap(int[] tab, int i)
        {
            int l = 2 * i;
            int r = 2 * i + 1;
            int largest = i;

            if (l <= heapSize && tab[l] > tab[i])
            {
                largest = l;
            }

            if (r <= heapSize && tab[r] > tab[largest])
            {
                largest = r;
            }

            if (largest != i)
            {
                int tmp = tab[i];
                tab[i] = tab[largest];
                tab[largest] = tmp;
                Heap(tab, largest);
            }
        }
        public int[] HeapSort(int[] tab)
        {

            BuildHeap(tab);
            for (int i = tab.Length - 1; i >= 0; i--)
            {

                int tmp = tab[0];
                tab[0] = tab[i];
                tab[i] = tmp;
                heapSize--;
                Heap(tab, 0);
            }

            return tab;
        }

        public int[] merge(int[] leftTab, int[] rightTab)
        {
            int[] result = new int[rightTab.Length + leftTab.Length];

            int l = 0;
            int r = 0;
            int i = 0;

            while (l < leftTab.Length || r < rightTab.Length)
            {

                if (l < leftTab.Length && r < rightTab.Length)
                {

                    if (leftTab[l] <= rightTab[r])
                    {
                        result[i] = leftTab[l];
                        l++;
                        i++;
                    }

                    else
                    {
                        result[i] = rightTab[r];
                        r++;
                        i++;
                    }
                }

                else if (l < leftTab.Length)
                {
                    result[i] = leftTab[l];
                    l++;
                    i++;
                }

                else if (r < rightTab.Length)
                {
                    result[i] = rightTab[r];
                    r++;
                    i++;
                }

            }
            return result;
        }


        public int[] MergeSort(int[] tab)
        {


            if (tab.Length <= 1)
            {
                return tab;
            }

            int[] leftTab;
            int[] rightTab;

            int mid = tab.Length / 2;
            leftTab = new int[mid];
            rightTab = new int[tab.Length - leftTab.Length];

            for (int i = 0; i < mid; i++)
            {
                leftTab[i] = tab[i];
            }

            for (int i = mid; i < tab.Length; i++)
            {
                rightTab[i - mid] = tab[i];
            }


            return merge(MergeSort(leftTab), MergeSort(rightTab));

        }

    }

}
