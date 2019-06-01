using System;
using System.Collections.Generic;

namespace ASD
{
    /// <summary>
    /// Klasa drzewa prefiksowego z możliwością wyszukiwania słów w zadanej odległości edycyjnej
    /// </summary>
    public class Lab14_Trie : System.MarshalByRefObject
    {

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
            if (sucess)
            {
                for (int i = 0; i < newWord.Length; i++)
                {
                    currNode.WordCount++;
                    currNode = currNode.childs[newWord[i]];
                    if(i == newWord.Length - 1)
                    {
                        currNode.WordCount++;
                    }
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

            if(Contains(word))
            {

                TrieNode currNode = root;

                allWordsWithPrefix = new List<string>();

                for (int i = 0; i < word.Length; i++)
                {
                    //Console.Write(i);
                    if (currNode.childs.ContainsKey(word[i]))
                    {
                        if(currNode.WordCount <= 2)
                        currNode = currNode.childs[word[i]];
                        {
                            currNode.childs.Remove(word[i]);
                            return true;
                        }
                    }
                }


                return true;
            }

            return false;
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
                if(i == startWith.Length - 1)
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


        List<string> allWordsWithPrefix;
        string currWord;

        private void AllWordsRecursiveHelper(TrieNode node)
        {

            foreach(var i in node.childs.Keys)
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
            return null;
        }

    }
}