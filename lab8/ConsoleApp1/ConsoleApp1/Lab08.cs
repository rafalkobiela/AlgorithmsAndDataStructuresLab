using System;
using System.Collections.Generic;
using System.Linq;
using ASD.Graphs;

namespace ASD
{
    public class Lab08 : MarshalByRefObject
    {
        /// <summary>
        /// Znajduje cykl rozpoczynający się w stolicy, który dla wybranych miast,
        /// przez które przechodzi ma największą sumę liczby ludności w tych wybranych
        /// miastach oraz minimalny koszt.
        /// </summary>
        /// <param name="cities">
        /// Graf miast i połączeń między nimi.
        /// Waga krawędzi jest kosztem przejechania między dwoma miastami.
        /// Koszty transportu między miastami są nieujemne.
        /// </param>
        /// <param name="citiesPopulation">Liczba ludności miast</param>
        /// <param name="meetingCosts">
        /// Koszt spotkania w każdym z miast.
        /// Dla części pierwszej koszt spotkania dla każdego miasta wynosi 0.
        /// Dla części drugiej koszty są nieujemne.
        /// </param>
        /// <param name="budget">Budżet do wykorzystania przez kandydata.</param>
        /// <param name="capitalCity">Numer miasta będącego stolicą, z której startuje kandydat.</param>
        /// <param name="path">
        /// Tablica dwuelementowych krotek opisująca ciąg miast, które powinen odwiedzić kandydat.
        /// Pierwszy element krotki to numer miasta do odwiedzenia, a drugi element decyduje czy
        /// w danym mieście będzie organizowane spotkanie wyborcze.
        /// 
        /// Pierwszym miastem na tej liście zawsze będzie stolica (w której można, ale nie trzeba
        /// organizować spotkania).
        /// 
        /// Zakładamy, że po odwiedzeniu ostatniego miasta na liście kandydat wraca do stolicy
        /// (na co musi mu starczyć budżetu i połączenie między tymi miastami musi istnieć).
        /// 
        /// Jeżeli kandydat nie wyjeżdża ze stolicy (stolica jest jedynym miastem, które odwiedzi),
        /// to lista `path` powinna zawierać jedynie jeden element: stolicę (wraz z informacją
        /// czy będzie tam spotkanie czy nie). Nie są wtedy ponoszone żadne koszty podróży.
        /// 
        /// W pierwszym etapie drugi element krotki powinien być zawsze równy `true`.
        /// </param>
        /// <returns>
        /// Liczba mieszkańców, z którymi spotka się kandydat.
        /// </returns>
        /// 


        int capital;
        int currentBest = 0;
        (int, bool)[] bestPath = new (int, bool)[0];
        int currentCity;
        int[] population;
        List<int> currentPath = new List<int>();
        Graph g;
        bool[] isInCurrentPath;
        double CurrentCost;
        double budget;
        double bestBudget;
        int currentPopulation;

        public void Print(List<int> a)
        {
            Console.Write("path: ");
            foreach (var i in a)
            {
                Console.Write($"{i}, ");
            }
            Console.WriteLine();
        }

        public int ComputeElectionCampaignPath(Graph cities, int[] citiesPopulation,
            double[] meetingCosts, double budget, int capitalCity, out (int, bool)[] path)
        {

            capital = capitalCity;
            currentCity = capitalCity;
            g = cities.Clone();
            population = citiesPopulation;
            currentPath.Add(capitalCity);
            isInCurrentPath = new bool[g.VerticesCount];
            CurrentCost = 0;
            this.budget = budget;
            bestBudget = double.MaxValue;
            currentPopulation = population[capitalCity];

            if (g.VerticesCount == 1)
            {
                
                path = new (int, bool)[1] { (capital, true) };
                
                currentPath.Clear();
                CurrentCost = 0;

                return citiesPopulation[capital];
                
            }
            else
            {
                bestPath = new (int, bool)[1] { (capital, true) };
                currentBest = citiesPopulation[capital];
                ComputeElectionCampaignPath();
                path = bestPath;        
            }
            
            //foreach(var z in g.OutEdges(2115))
            //{
            //    Console.Write($"{z} ");
            //}
            //Console.WriteLine();
            int i = 0;
            //g.GeneralSearchAll<EdgesStack>(v => { Console.WriteLine($"{v}, {i}"); i++; return true; }, null, null, out int cc);


            currentPath.Clear();
            CurrentCost = 0;
            return currentBest;
        }


        

        public void ComputeElectionCampaignPath()
        {


            int tmpCity = currentCity;


            if (currentCity == capital && currentPath.Count > 1)
            {
                int sum = 0;

                foreach (var c in currentPath)
                {
                    sum += population[c];
                }

                sum -= population[capital];
                currentPopulation -= population[capital];
                //Console.WriteLine($"sum: {sum}, pop: {currentPopulation}");
                if (sum >= currentBest)
                {
                    

                    if(!(sum == currentBest && CurrentCost >= bestBudget)){
                        currentBest = sum;
                        bestPath = new (int, bool)[currentPath.Count - 1];

                        for (int i = 0; i < currentPath.Count - 1; i++)
                        {
                            bestPath[i] = (currentPath[i], true);
                        }
                        bestBudget = CurrentCost;
                    }

                }
            }
            else
            {
                foreach (var neigh in g.OutEdges(tmpCity))
                {


                    if (isInCurrentPath[neigh.To])
                    {
                        continue;
                    }

                    if (CurrentCost + neigh.Weight > budget)
                    {
                        continue;
                    }


                    currentPath.Add(neigh.To);
                    isInCurrentPath[neigh.To] = true;
                    currentCity = neigh.To;
                    CurrentCost += neigh.Weight;
                    currentPopulation += population[neigh.To];

                    ComputeElectionCampaignPath();

                    currentCity = tmpCity;
                    currentPath.RemoveAt(currentPath.Count - 1);
                    isInCurrentPath[neigh.To] = false;
                    CurrentCost -= neigh.Weight;
                    currentPopulation -= population[neigh.To];

                }
            }
        }



    }

}
