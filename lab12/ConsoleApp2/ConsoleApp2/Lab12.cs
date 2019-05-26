using GraphX;
using System;
using System.Collections.Generic;

namespace CSG
{
    public class Clipper : MarshalByRefObject
    {
        /// <summary>
        /// Metoda znajdowania przecięcia odcinków s1s2 i c1c2. Użyj jej w etapie 1.
        /// Metoda wypełnia pole Distance w zwracanych wierzchołkach, czyli względną pozycję wierzchołka na odcinku.
        /// </summary>
        /// <param name="s1">Początek pierwszego odcinka</param>
        /// <param name="s2">Koniec pierwszego odcinka</param>
        /// <param name="c1">Początek drugiego odcinka</param>
        /// <param name="c2">Koniec drugiego odcinka</param>
        /// <returns>Zwraca dwa wierzchołki, które mają te same współrzędne, 
        /// ale różnią się względną pozycją na swoim odcinku (pierwszy zwracany wierzchołek leży na s1s2,
        /// a drugi na c1c2). Gdy odcinki się nie przecinają, zwracany jest null.</returns>
        public (Vertex, Vertex)? GetIntersectionPoints(Vertex s1, Vertex s2, Vertex c1, Vertex c2)
        {
            double d = (c2.Y - c1.Y) * (s2.X - s1.X) - (c2.X - c1.X) * (s2.Y - s1.Y);
            if (d == 0.0)
            {
                return null;
            }

            double toSource = ((c2.X - c1.X) * (s1.Y - c1.Y) - (c2.Y - c1.Y) * (s1.X - c1.X)) / d;
            double toClip = ((s2.X - s1.X) * (s1.Y - c1.Y) - (s2.Y - s1.Y) * (s1.X - c1.X)) / d;

            Vertex s = new Vertex(s1.X + toSource * (s2.X - s1.X), s1.Y + toSource * (s2.Y - s1.Y))
            {
                Distance = toSource,
                IsIntersection = true
            };
            Vertex c = new Vertex(s1.X + toSource * (s2.X - s1.X), s1.Y + toSource * (s2.Y - s1.Y))
            {
                Distance = toClip,
                IsIntersection = true
            };

            if ((0 < toSource && toSource < 1) && (0 < toClip && toClip < 1))
            {
                return (s, c);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Metoda sprawdzająca, czy dany wierzchołek v znajduje się w danym wielokącie p. Użyj w etapach 2.
        /// </summary>
        /// <param name="v">Wierzchołek</param>
        /// <param name="p">Wielokąt</param>
        /// <returns>Prawda, jeśli wierzchołek znajduje się wewnątrz wielokąta, fałsz w przeciwnym wypadku</returns>
        public bool IsInside(LinkedListNode<Vertex> v, Polygon p)
        {
            // funkcja strzela nieskończoną prostą w lewo (ujemne X-y) od punktu v
            // jeśli na swojej drodze napotka nieparzystą liczbę boków, to znaczy, że jest w środku wielokąta p
            // w praktyce odbywa się to tak, że sprawdzamy, czy dany bok przecina Y-ową współrzędną punktu,
            // jednocześnie (używając interpolacji liniowej) czy to miejsce w odcinku, na który pada rzut punktu v na oś OY
            // jest po lewej (po ludzku: czy odcinek jest na lewo od punktu). 
            // Jeśli tak, to jest to bok przecinający się z nieskończoną prostą idącą w lewo. Inne boki nas nie interesują

            bool oddNodes = false;
            double x = v.Value.X;
            double y = v.Value.Y;

            // znów nie da się sprytnie foreachem, bo trzeba mieć dostęp także do następnego
            for (LinkedListNode<Vertex> LLvertex = p.Vertices.First; LLvertex != null; LLvertex = LLvertex.Next)
            {
                Vertex vertex;
                Vertex next;

                //if ma zapewnić, że sprawdzimy także bok łączący ostatni wierzchołek z pierwszym 
                if (LLvertex.Next == null)
                {
                    vertex = LLvertex.Value;
                    next = p.Vertices.First.Value;
                }
                else
                {
                    vertex = LLvertex.Value;
                    next = LLvertex.Next.Value;
                }

                // czy odcinek przecina Y-ową współrzędną punktu?
                // czy choć jedna współrzędna X-owa odcinka jest po lewej?
                if ((vertex.Y < y && next.Y >= y ||
                       next.Y < y && vertex.Y >= y) &&
                    (vertex.X <= x || next.X <= x))
                {
                    // jeśli tak, to czy odcinek jest na lewo od punktu?
                    oddNodes ^= vertex.X + (y - vertex.Y) / (next.Y - vertex.Y) * (next.X - vertex.X) < x;
                }
            }

            return oddNodes;
        }

        /// <summary>
        /// Metoda znajdowania punktów, gdzie przecinają się dwa wielokąty ze sobą. 
        /// Argumenty nie są modyfikowane, zmodyfikowane wersje są zwracane jako wynik.
        /// </summary>
        /// <param name="source">Pierwszy z przecinanych wielokątów</param>
        /// <param name="clip">Drugi z przecinanych wielokątów</param>
        /// <returns>Zwraca zmodyfikowane kopie wielokątów wejściowych</returns>
        public (Polygon, Polygon) MakeIntersectionPoints(Polygon source, Polygon clip)
        {
            Polygon sourceCopy = new Polygon(source);
            Polygon clipCopy = new Polygon(clip);


            //Console.WriteLine(source);
            //Console.WriteLine(clip);
            //Console.WriteLine();
            for (LinkedListNode<Vertex> LLvertex = sourceCopy.Vertices.First; LLvertex != null; LLvertex = LLvertex.Next)
            {


                for (LinkedListNode<Vertex> LLvertex2 = clipCopy.Vertices.First; LLvertex2 != null; LLvertex2 = LLvertex2.Next)
                {
                    Vertex s1 = LLvertex.Value;
                    Vertex s2;
                    LinkedListNode<Vertex> s2Node = LLvertex.Next;
                    if (s2Node == null)
                    {
                        s2 = LLvertex.List.First.Value;
                    }
                    else
                    {
                        s2 = s2Node.Value;
                    }

                    Vertex c1 = LLvertex2.Value;
                    Vertex c2;
                    LinkedListNode<Vertex> c2Node = LLvertex2.Next;
                    if (c2Node == null)
                    {
                        c2 = LLvertex2.List.First.Value;
                    }
                    else
                    {
                        c2 = c2Node.Value;
                    }

                    //Console.WriteLine($"({s1.ToString()},{s2.ToString()}), ({c1.ToString()},{c2.ToString()})");

                    (Vertex, Vertex)? intersectionPoints = GetIntersectionPoints(s1, s2, c1, c2);

                    if (intersectionPoints != null)
                    {
                        //Console.WriteLine();
                        //Console.Write(intersectionPoints.Value.Item1);
                        //Console.WriteLine();
                        sourceCopy.Vertices.AddAfter(LLvertex, intersectionPoints.Value.Item1);
                        clipCopy.Vertices.AddAfter(LLvertex2, intersectionPoints.Value.Item2);

                    }

                }
            }


            //Console.WriteLine();
            //Console.WriteLine(sourceCopy);
            //Console.WriteLine(clipCopy);

            return (sourceCopy, clipCopy);
        }

        /// <summary>
        /// Metoda oznaczająca wierzchołki jako wejściowe lub wyjściowe.
        /// Argumenty nie są modyfikowane, zmodyfikowane wersje są zwracane jako wynik.
        /// </summary>
        /// <param name="source">Pierwszy z przecinanych wielokątów</param>
        /// <param name="clip">Drugi z przecinanych wielokątów</param>
        /// <returns>Zwraca zmodyfikowane kopie wielokątów wejściowych</returns>
        public (Polygon, Polygon) MarkEntryExitPoints(Polygon source, Polygon clip)
        {
            (Polygon sourceCopy, Polygon clipCopy) = MakeIntersectionPoints(source, clip);

            bool isFirstInside = IsInside(sourceCopy.Vertices.First, sourceCopy);

            for (LinkedListNode<Vertex> LLvertex = sourceCopy.Vertices.First; LLvertex != null; LLvertex = LLvertex.Next)
            {
                if (LLvertex.Value.IsIntersection)
                {

                    LLvertex.Value.IsEntry = !isFirstInside;
                    isFirstInside = !isFirstInside;
                }
            }

            isFirstInside = IsInside(clipCopy.Vertices.First, clipCopy);

            for (LinkedListNode<Vertex> LLvertex = clipCopy.Vertices.First; LLvertex != null; LLvertex = LLvertex.Next)
            {
                if (LLvertex.Value.IsIntersection)
                {
                    LLvertex.Value.IsEntry = isFirstInside;
                    isFirstInside = !isFirstInside;
                }
            }

            return (sourceCopy, clipCopy);
        }

        /// <summary>
        /// Metoda zwracająca wynik operacji logicznej na dwóch wielokątach.
        /// </summary>
        /// <param name="source">Pierwszy z przecinanych wielokątów</param>
        /// <param name="clip">Drugi z przecinanych wielokątów</param>
        /// <returns>Lista wynikowych wielokątów</returns>
        public List<Polygon> ReturnClippedPolygons(Polygon source, Polygon clip)
        {
            (Polygon sourceCopy, Polygon clipCopy) = MarkEntryExitPoints(source, clip);

            int numberOfIntersections = 0;
            int numberOfIntersectionsProcessed = 0;
            for (LinkedListNode<Vertex> LLvertex = clipCopy.Vertices.First; LLvertex != null; LLvertex = LLvertex.Next)
            {
                if (LLvertex.Value.IsIntersection)
                {
                    numberOfIntersections++;
                }
            }
            Console.WriteLine($"numberOfIntersections: {numberOfIntersections}");

            List<Polygon> result = new List<Polygon>();
            List<Vertex> tmpPolygon = new List<Vertex>();

            List<Vertex> sourceNormalList = new List<Vertex>(sourceCopy.Vertices);
            List<Vertex> clipNormalList = new List<Vertex>(clipCopy.Vertices);

            bool[] sourceVerticesProcessed = new bool[sourceNormalList.Count];
            bool[] clipVerticesProcessed = new bool[clipNormalList.Count];

            int currentIndex = findFirstUnprocessedIntersection(sourceNormalList, sourceVerticesProcessed);
            List<Vertex> currentNewPolygon = new List<Vertex>();
            bool isSourceCurrentlyProcessed = true;

            while (numberOfIntersections > numberOfIntersectionsProcessed)
            {

                numberOfIntersectionsProcessed++;
            }

            //if(numberOfIntersections > 0)
            //{
            //    Console.WriteLine($"intersection1: { sourceNormalList[findFirstUnprocessedIntersection(sourceNormalList, sourceVerticesProcessed)]   }");
            //    Console.WriteLine($"intersection2: { clipNormalList[findIndexOfIntersectionInSecondPolygon(sourceNormalList[findFirstUnprocessedIntersection(sourceNormalList, sourceVerticesProcessed)], clipNormalList)]   }");
            //}

            return null;
        }

        private int findFirstUnprocessedIntersection(List<Vertex> p, bool[] processedVertices)
        {

            for (int i = 0; i < p.Count; i++)
            {
                if (p[i].IsIntersection && !processedVertices[i])
                {
                    return i;
                }
            }
            return -1;
        }

        private int findIndexOfIntersectionInSecondPolygon(Vertex v, List<Vertex> p)
        {

            for (int i = 0; i < p.Count; i++)
            {
                if (v.X == p[i].X && v.Y == p[i].Y )
                {
                    return i;
                }
            }

            return -1;
        }

    }
}
