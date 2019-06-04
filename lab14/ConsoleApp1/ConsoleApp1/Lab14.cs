using System;
using System.Collections.Generic;

namespace ASD
{




    /// <summary>
    /// Klasa drzewa prefiksowego z możliwością wyszukiwania słów w zadanej odległości edycyjnej
    /// </summary>
    public class Lab14_Trie : System.MarshalByRefObject
    {

        private void print(TrieNode a)
        {
            foreach (var i in a.childs.Keys)
            {
                Console.Write($"{i} , ");
            }
            Console.WriteLine($" {a.IsWord}, {a.WordCount}");

        }
        // klasy TrieNode NIE WOLNO ZMIENIAĆ!
        private class TrieNode
        {
            public SortedDictionary<char, TrieNode> childs = new SortedDictionary<char, TrieNode>();
            public bool IsWord = false;
            public int WordCount = 0;
        }

        private TrieNode root;

        public Lab14_Trie()
        {
            root = new TrieNode();
        }

        /// <summary>
        /// Zwraca liczbę przechowywanych słów
        /// Ma działać w czasie stałym - O(1)
        /// </summary>
        public int Count { get { return -1; } }

        /// <summary>
        /// Zwraca liczbę przechowywanych słów o zadanym prefiksie
        /// Ma działać w czasie O(len(startWith))
        /// </summary>
        /// <param name="startWith">Prefiks słów do zliczenia</param>
        /// <returns>Liczba słów o zadanym prefiksie</returns>
        public int CountPrefix(string startWith)
        {

            TrieNode currNode = root;


            if (startWith == "")
            {
                return root.WordCount;
            }
            //Console.WriteLine();


            for (int i = 0; i < startWith.Length; i++)
            {
                //Console.Write(i);
                if (currNode.childs.ContainsKey(startWith[i]))
                {

                    //Console.Write(currNode.WordCount);
                    currNode = currNode.childs[startWith[i]];
                    if (i == startWith.Length - 1)
                    {

                        return currNode.WordCount;
                    }
                }
                else
                {
                    return 0;
                }
            }
            return 0;

        }

        /// <summary>
        /// Dodaje słowo do słownika
        /// Ma działać w czasie O(len(newWord))
        /// </summary>
        /// <param name="newWord">Słowo do dodania</param>
        /// <returns>True jeśli słowo udało się dodać, false jeśli słowo już istniało</returns>
        public bool AddWord(string newWord)
        {
            TrieNode currNode = root;

            bool sucess = true;

            for (int i = 0; i < newWord.Length; i++)
            {
                //Console.Write(i);
                if (currNode.childs.ContainsKey(newWord[i]))
                {
                    currNode = currNode.childs[newWord[i]];
                    //currNode.WordCount++;
                    //Console.WriteLine(currNode.WordCount);
                    if (i == newWord.Length - 1 && currNode.IsWord)
                    {
                        sucess = false;
                        return false;
                    }
                    if (i == newWord.Length - 1)
                    {
                        currNode.IsWord = true;
                    }

                }
                else
                {
                    currNode.childs.Add(newWord[i], new TrieNode());
                    currNode = currNode.childs[newWord[i]];
                    //currNode.WordCount++;
                    //Console.WriteLine(currNode.WordCount);
                    if (i == newWord.Length - 1)
                    {
                        currNode.IsWord = true;
                    }

                }
            }

            //Console.WriteLine();

            currNode = root;
            currNode.WordCount++;
            if (sucess)
            {
                for (int i = 0; i < newWord.Length; i++)
                {
                    currNode = currNode.childs[newWord[i]];
                    currNode.WordCount++;
                    //if (i == newWord.Length - 1)
                    //{
                    //    currNode.WordCount++;
                    //}
                }

            }


            return sucess;
        }

        /// <summary>
        /// Sprawdza czy podane słowo jest przechowywane w słowniku
        /// Ma działać w czasie O(len(word))
        /// </summary>
        /// <param name="word">Słowo do sprawdzenia</param>
        /// <returns>True jeśli słowo znajduje się w słowniku, wpp. false</returns>
        public bool Contains(string word)
        {
            TrieNode currNode = root;

            for (int i = 0; i < word.Length; i++)
            {
                //Console.Write(i);
                if (currNode.childs.ContainsKey(word[i]))
                {

                    currNode = currNode.childs[word[i]];

                    if (i == word.Length - 1 && currNode.IsWord)
                    {
                        return true;
                    }
                    else if (i == word.Length - 1 && !currNode.IsWord)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            //Console.WriteLine();

            return true;
        }

        /// <summary>
        /// Usuwa podane słowo ze słownika
        /// Ma działać w czasie O(len(word))
        /// </summary>
        /// <param name="word">Słowo do usunięcia</param>
        /// <returns>True jeśli udało się słowo usunąć, false jeśli słowa nie było w słowniku</returns>
        public bool Remove(string word)
        {
            //Console.WriteLine();

            //Console.WriteLine($"word: {word}");

            TrieNode currNode = root;

            List<TrieNode> nodesList = new List<TrieNode>();
            nodesList.Add(currNode);

            for (int i = 0; i < word.Length; i++)
            {

                if (currNode.childs.ContainsKey(word[i]))
                {
                    currNode = currNode.childs[word[i]];
                    nodesList.Add(currNode);
                    //Console.Write($"-- {word[i]} ---");
                    //print(currNode);
                }
                else
                {
                    //Console.WriteLine("1");
                    return false;
                }
                if (i == word.Length - 1)
                {
                    if (!currNode.IsWord)
                    {
                        //Console.WriteLine("2");
                        return false;
                    }
                }
            }


            //Console.WriteLine($"list len: {nodesList.Count}, word len: {word.Length}");

            //foreach (var i in nodesList)
            //{
            //    print(i);
            //}

            for (int i = 0; i < nodesList.Count; i++)
            {
                if (nodesList[i].WordCount > 1)
                {
                    nodesList[i].WordCount--;
                    if (i == nodesList.Count - 1)
                    {
                        nodesList[i].IsWord = false;
                        //Console.WriteLine("dupa");
                        break;
                    }
                }
                else if (nodesList[i].WordCount == 1)
                {

                    //Console.Write("Usuwam z tego węzła: ");
                    //print(nodesList[i - 1]);
                    //Console.WriteLine($"usuwanie jak jest 2 {word[i - 1]}, czy zawiera literke: {nodesList[i - 1].childs.ContainsKey(word[i - 1])}");
                    nodesList[i].WordCount--;
                    if (i == nodesList.Count - 1)

                    {
                        nodesList[i].childs.Remove(word[i - 1]);

                    }
                    else
                    {
                        nodesList[i].childs.Remove(word[i]);

                    }


                }

            }
            //Console.WriteLine("3");
            return true;
        }

        /// <summary>
        /// Zwraca wszystkie słowa o podanym prefiksie. 
        /// Dla pustego prefiksu zwraca wszystkie słowa ze słownika.
        /// Wynik jest w porządku alfabetycznym.
        /// Ma działać w czasie O(liczba węzłów w drzewie)
        /// </summary>
        /// <param name="startWith">Prefiks</param>
        /// <returns>Wyliczenie zawierające wszystkie słowa ze słownika o podanym prefiksie</returns>
        public List<string> AllWords(string startWith = "")
        {

            TrieNode currNode = root;

            allWordsWithPrefix = new List<string>();

            for (int i = 0; i < startWith.Length; i++)
            {
                //Console.Write(i);
                if (currNode.childs.ContainsKey(startWith[i]))
                {
                    currNode = currNode.childs[startWith[i]];
                }
                else
                {
                    //Console.WriteLine("a");
                    return allWordsWithPrefix;
                }
                if (i == startWith.Length - 1)
                {
                    if (currNode.IsWord)
                    {
                        allWordsWithPrefix.Add(startWith);
                    }
                }
            }

            currWord = startWith;

            AllWordsRecursiveHelper(currNode);

            return allWordsWithPrefix;
        }

        private List<string> allWordsWithPrefix;
        private string currWord;

        private void AllWordsRecursiveHelper(TrieNode node)
        {

            foreach (var i in node.childs.Keys)
            {
                currWord += i;

                TrieNode tmpNode = node.childs[i];
                if (tmpNode.IsWord)
                {
                    allWordsWithPrefix.Add(currWord);
                }

                AllWordsRecursiveHelper(node.childs[i]);

                currWord = currWord.Substring(0, currWord.Length - 1);
            }
        }


        public void print(List<int[]> tab)
        {
            for (int i = 0; i < tab.Count; i++)
            {
                for (int j = 0; j < tab[i].Length; j++)
                {
                    Console.Write($"{tab[i][j]}, ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }


        public int LevenshteinDistance(string word1, string word2)
        {
            int n1 = word1.Length;
            int n2 = word2.Length;

            //int[,] changesMatrix = new int[n1 + 1, n2 + 1];

            for (int i = 0; i < n1 + 1; i++)
            {
                changesMatrix[i][0] = i;
            }

            for (int i = 0; i < n2 + 1; i++)
            {
                changesMatrix[0][i] = i;
            }

            for (int i = 1; i < n1 + 1; i++)
            {
                for (int j = 1; j < n2 + 1; j++)
                {
                    if (word1[i - 1] == word2[j - 1])
                    {
                        changesMatrix[i][j] = Math.Min(Math.Min(changesMatrix[i - 1][j] + 1, changesMatrix[i][j - 1] + 1), changesMatrix[i - 1][j - 1]);
                    }
                    else
                    {
                        changesMatrix[i][j] = Math.Min(Math.Min(changesMatrix[i - 1][j] + 1, changesMatrix[i][j - 1] + 1), changesMatrix[i - 1][j - 1] + 1);
                    }
                }
            }

            return changesMatrix[n1][n2];
        }


        /// <summary>
        /// Wyszukuje w słowniku wszystkie słowa w podanej odległości edycyjnej od zadanego słowa
        /// Wynik jest w porządku alfabetycznym ze względu na słowa (a nie na odległość).
        /// Ma działać optymalnie - tj. niedozwolone jest wyszukanie wszystkich słów i sprawdzenie ich odległości
        /// Należy przeszukując drzewo odpowiednio odrzucać niektóre z gałęzi.
        /// Złożoność pesymistyczna (gdy wszystkie słowa w słowniku mieszczą się w zadanej odległości)
        /// O(len(word) * (liczba węzłów w drzewie))
        /// </summary>
        /// <param name="word">Słowo</param>
        /// <param name="distance">Odległość edycyjna</param>
        /// <returns>Lista zawierająca pary (słowo, odległość) spełniające warunek odległości edycyjnej</returns>
        public List<(string, int)> Search(string word, int distance = 1)
        {

            allWordsWithDistances = new List<(string, int)>();
            changesMatrix = new List<int[]>();
            int[] tmpTab = new int[word.Length + 1];

            for (int i = 0; i < word.Length + 1; i++)
            {
                tmpTab[i] = i;
            }

            ReallyAllWordsRecursiveHelper(root, word, distance);


            return allWordsWithDistances;
        }

        List<(string, int)> allWordsWithDistances;
        List<int[]> changesMatrix;
        private void ReallyAllWordsRecursiveHelper(TrieNode node, string wordToMeasureDist, int distance)
        {

            foreach (var i in node.childs.Keys)
            {
                currWord += i;

                TrieNode tmpNode = node.childs[i];

                int[] tmpTab = new int[wordToMeasureDist.Length + 1];
                tmpTab[0] = changesMatrix.Count ;
                changesMatrix.Add(tmpTab);
                Console.WriteLine($"Curr word: {currWord}");

                print(changesMatrix);
                for (int j = 1; j < wordToMeasureDist.Length + 1; j++)
                {
                    int n = changesMatrix.Count;
                    if (i == wordToMeasureDist[j - 1])
                    {
                        changesMatrix[n - 1][j] = Math.Min(Math.Min(changesMatrix[n - 1][j] + 1,
                                                                changesMatrix[n - 1][j - 1] + 1),
                                                       changesMatrix[n - 1][j - 1]);
                    }
                    else
                    {
                        changesMatrix[changesMatrix.Count - 1][j] = Math.Min(Math.Min(changesMatrix[n - 1][j] + 1,
                                                                changesMatrix[n - 1][j - 1] + 1),
                                                       changesMatrix[n - 1][j - 1] + 1);
                    }
                }


                if (tmpNode.IsWord)
                {
                    int currDist = changesMatrix[changesMatrix.Count - 1][wordToMeasureDist.Length];
                    if (currDist <= distance)
                    {
                        allWordsWithDistances.Add((currWord, currDist));
                    }
                }

                if (currWord.Length <= distance + wordToMeasureDist.Length)
                {
                    ReallyAllWordsRecursiveHelper(node.childs[i], wordToMeasureDist, distance);
                }

                currWord = currWord.Substring(0, currWord.Length - 1);
                changesMatrix.RemoveAt(changesMatrix.Count - 1);
            }
        }


    }
}