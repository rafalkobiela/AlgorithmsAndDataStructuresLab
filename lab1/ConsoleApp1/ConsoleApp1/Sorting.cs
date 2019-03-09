
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

            for (int j = l; j <= r - 1; j++)
            {
                if (A[j] <= v)
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




            return i + 1;
        }

        public void QuickSort(int l, int r, ref int[] tab)
        {
            int j = Partition2(l, r, ref tab);
            if (j - 1 > l)
            {
                QuickSort(l, j - 1, ref tab);
            }
            if (j + 1 < r)
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
        private void BuildHeap(int[] arr)
        {
            heapSize = arr.Length - 1;
            for (int i = heapSize / 2; i >= 0; i--)
            {
                Heap(arr, i);
            }
        }
        private void Heap(int[] arr, int index)
        {
            int left = 2 * index;
            int right = 2 * index + 1;
            int largest = index;

            if (left <= heapSize && arr[left] > arr[index])
            {
                largest = left;
            }

            if (right <= heapSize && arr[right] > arr[largest])
            {
                largest = right;
            }

            if (largest != index)
            {

                int tmp = arr[index];
                arr[index] = arr[largest];
                arr[largest] = tmp;
                Heap(arr, largest);
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

        public int[] merge(int[] left, int[] right)
        {
            int resultLength = right.Length + left.Length;
            int[] result = new int[resultLength];

            int indexLeft = 0, indexRight = 0, indexResult = 0;

            while (indexLeft < left.Length || indexRight < right.Length)
            {

                if (indexLeft < left.Length && indexRight < right.Length)
                {

                    if (left[indexLeft] <= right[indexRight])
                    {
                        result[indexResult] = left[indexLeft];
                        indexLeft++;
                        indexResult++;
                    }

                    else
                    {
                        result[indexResult] = right[indexRight];
                        indexRight++;
                        indexResult++;
                    }
                }

                else if (indexLeft < left.Length)
                {
                    result[indexResult] = left[indexLeft];
                    indexLeft++;
                    indexResult++;
                }

                else if (indexRight < right.Length)
                {
                    result[indexResult] = right[indexRight];
                    indexRight++;
                    indexResult++;
                }

            }
            return result;
        }


        public int[] MergeSort(int[] tab)
        {


            int[] left;
            int[] right;
            int[] result = new int[tab.Length];


            if (tab.Length <= 1)
            {
                return tab;
            }

            int midPoint = tab.Length / 2;


            left = new int[midPoint];


            if (tab.Length % 2 == 0)
            {
                right = new int[midPoint];
            }

            else
            {
                right = new int[midPoint + 1];
            }

            for (int i = 0; i < midPoint; i++)
            {
                left[i] = tab[i];
            }

            int x = 0;


            for (int i = midPoint; i < tab.Length; i++)
            {
                right[x] = tab[i];
                x++;
            }


            left = MergeSort(left);

            right = MergeSort(right);

            result = merge(left, right);

            return result;

        }

    }

}
