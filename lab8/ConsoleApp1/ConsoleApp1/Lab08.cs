using ASD.Graphs;
using System;
using System.Collections.Generic;

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


        private int capital;
        private int currentBest = 0;
        private (int, bool)[] bestPath = new (int, bool)[0];
        private int currentCity;
        private int[] population;
        private List<(int, bool)> currentPath = new List<(int, bool)>();
        private Graph g;
        private bool[] isInCurrentPath;
        private double CurrentCost;
        private double budget;
        private double bestCost;
        private int currentPopulation;
        private double[] meetingCosts;

        public void Print(List<(int, bool)> a)
        {
            Console.Write($"cost: {CurrentCost}, path: ");
            foreach (var i in a)
            {
                Console.Write($"({i.Item1}, {i.Item2}), ");
            }
            Console.WriteLine();
        }
        public void Print((int, bool)[] a)
        {
            Console.Write($"cost: {CurrentCost}, path: ");
            foreach (var i in a)
            {
                Console.Write($"({i.Item1}, {i.Item2}), ");
            }
            Console.WriteLine();
        }

        public int ComputeElectionCampaignPath(Graph cities, int[] citiesPopulation,
            double[] meetingCosts, double budget, int capitalCity, out (int, bool)[] path)
        {


            capital = capitalCity;
            g = cities.Clone();
            population = citiesPopulation;
            currentBest = 0;

            this.budget = budget;
            bestCost = double.MaxValue;
            this.meetingCosts = meetingCosts;
            isInCurrentPath = new bool[g.VerticesCount];

            bestPath = new (int, bool)[1] { (capital, false) };
            currentBest = 0;

            if (meetingCosts[capital] <= budget)
            {
                currentCity = capitalCity;
                currentPopulation = population[capitalCity];
                currentPath.Add((capitalCity, true));
                currentBest = population[capitalCity];
                //
                bestPath = new (int, bool)[1] { (capital, true) };
                bestCost = meetingCosts[capital];
                CurrentCost = meetingCosts[capital];
                ComputeElectionCampaignPath();
                
            }

            if(meetingCosts[capital] != 0)
            {
                currentCity = capitalCity;
                currentPath.Clear();
                currentPopulation = 0;
                CurrentCost = 0;

                currentPath.Add((capitalCity, false));
                ComputeElectionCampaignPath();
            }

            path = bestPath;
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

                for (int i = 0; i < currentPath.Count - 1; i++)
                {
                    if (currentPath[i].Item2)
                    {
                        sum += population[currentPath[i].Item1];
                    }
                }

                currentPopulation -= population[capital];
                if (sum >= currentBest)
                {
                    if (sum != currentBest || CurrentCost < bestCost)
                    {
                        currentBest = sum;
                        bestPath = new (int, bool)[currentPath.Count - 1];

                        currentPath.CopyTo(0, bestPath, 0, currentPath.Count - 1);
                        bestCost = CurrentCost;
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


                    isInCurrentPath[neigh.To] = true;
                    currentCity = neigh.To;
                    CurrentCost += neigh.Weight;



                    if (meetingCosts[neigh.To] != 0)
                    {
                        currentPath.Add((neigh.To, false));
                        ComputeElectionCampaignPath();
                        currentPath.RemoveAt(currentPath.Count - 1);
                    }


                    CurrentCost += meetingCosts[neigh.To];
                    currentPopulation += population[neigh.To];
                    currentPath.Add((neigh.To, true));

                    if (CurrentCost <= budget)
                    {
                        ComputeElectionCampaignPath();
                    }


                    CurrentCost -= meetingCosts[neigh.To];
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
