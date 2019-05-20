using System;
using System.Collections.Generic;
using System.Linq;

namespace ASD
{
    public class Lab11 : System.MarshalByRefObject
    {

        // iloczyn wektorowy
        private int Cross((double, double) o, (double, double) a, (double, double) b)
        {
            double value = (a.Item1 - o.Item1) * (b.Item2 - o.Item2) - (a.Item2 - o.Item2) * (b.Item1 - o.Item1);
            return Math.Abs(value) < 1e-10 ? 0 : value < 0 ? -1 : 1;
        }


        public double Distance((double, double) p1, (double, double) p2)
        {
            double dx, dy;
            dx = p1.Item1 - p2.Item1;
            dy = p1.Item2 - p2.Item2;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public double CrossProduct((double, double) p1, (double, double) p2)
        { return p1.Item1 * p2.Item2 - p2.Item1 * p1.Item2; }



        public (double, double) Diff((double, double) p1, (double, double) p2)
        {
            return (p1.Item1 - p2.Item1, p1.Item2 - p2.Item2);
        }


        public void print((double, double)[] p)
        {
            Console.WriteLine();
            foreach (var i in p)
            {
                Console.Write($"({i.Item1}, {i.Item2})");
            }
            Console.WriteLine();
        }


        public void print(List<(double, double)> p)
        {
            Console.WriteLine();
            foreach (var i in p)
            {
                Console.Write($"({i.Item1}, {i.Item2})");
            }
            Console.WriteLine();
        }


        public (double, double)[] mergeArrays((double, double)[] t1, (double, double)[] t2)
        {

            int t1_iter = 0;
            int t2_iter = 0;

            (double, double)[] t = new (double, double)[t2.Length + t1.Length];

            for (int i = 0; i < t1.Length + t2.Length; i++)
            {

                if (t1_iter == t1.Length)
                {
                    t[i] = t2[t2_iter];
                    t2_iter++;
                }
                else if (t2_iter == t2.Length)
                {
                    t[i] = t1[t1_iter];
                    t1_iter++;
                }
                else
                {
                    if (t1[t1_iter].Item1 > t2[t2_iter].Item1)
                    {
                        t[i] = t2[t2_iter];
                        t2_iter++;
                    }
                    else
                    {
                        t[i] = t1[t1_iter];
                        t1_iter++;
                    }

                }



            }

            return t;
        }

        // Etap 1
        // po prostu otoczka wypukła
        public (double, double)[] ConvexHull((double, double)[] points)
        {

            if (points.Length == 1)
            {
                return points;
            }

            int n = points.Length;
            (double, double) startingPoint = points[0];
            for (int i = 1; i < n; i++)
            {
                if (points[i].Item2 < startingPoint.Item2)
                    startingPoint = points[i];
                else if (points[i].Item2 == startingPoint.Item2 && points[i].Item1 < startingPoint.Item1)
                    startingPoint = points[i];
            }


            List<(double, double)> sortedPoints = new List<(double, double)>(points);
            sortedPoints.Remove(startingPoint);

            sortedPoints.Sort((p1, p2) =>
            {

                int crossProduct = Cross(p1, startingPoint, p2);
                if (crossProduct == 0)
                {
                    return (int)(Distance(startingPoint, p1) - Distance(startingPoint, p2));
                }
                return crossProduct;

            });


            List<(double, double)> convexHull = new List<(double, double)>();
            convexHull.Add(startingPoint);
            convexHull.Add(sortedPoints[0]);

            for (int k = 0; k < n - 1; k++)
            {

                while (convexHull.Count >= 2 && Cross(convexHull[convexHull.Count - 2], convexHull[convexHull.Count - 1], sortedPoints[k]) <= 0)
                    convexHull.RemoveAt(convexHull.Count - 1);

                convexHull.Add(sortedPoints[k]);
            }

            //print(convexHull.ToArray());
            return convexHull.ToArray();
        }


        // Etap 2
        // oblicza otoczkę dwóch wielokątów wypukłych
        public (double, double)[] ConvexHullOfTwo((double, double)[] poly1, (double, double)[] poly2)
        {

            int n = poly1.Length + poly2.Length;

            var poly = new (double, double)[n];

            poly1.CopyTo(poly, 0);
            poly2.CopyTo(poly, poly1.Length);

            int n1 = poly1.Length;
            int n2 = poly2.Length;

            poly = mergeArrays(poly1, poly2);

            //ma być najbardziej po lewo u góry
            (double, double) startingPoint1 = poly1[0];
            (double, double) startingPoint2 = poly2[0];

            //ma być nabardziej po prawo na dole
            (double, double) endingPoint1 = poly1[0];
            (double, double) endingPoint2 = poly2[0];


            for(int i = 1; i<n1; i++)
            {
                if (startingPoint1.Item1 == poly1[i].Item1 && startingPoint1.Item2 < poly1[i].Item2)
                {
                    startingPoint1 = poly1[i];
                }
                if(startingPoint1.Item1 > poly1[i].Item1)
                {
                    startingPoint1 = poly1[i];
                }

                if (endingPoint1.Item1 == poly1[i].Item1 && endingPoint1.Item2 > poly1[i].Item2)
                {
                    endingPoint1 = poly1[i];
                }
                if (endingPoint1.Item1 < poly1[i].Item1)
                {
                    endingPoint1 = poly1[i];
                }
            }


            for (int i = 1; i < n2; i++)
            {
                if (startingPoint2.Item1 == poly2[i].Item1 && startingPoint2.Item2 < poly2[i].Item2)
                {
                    startingPoint2 = poly2[i];
                }
                if (startingPoint2.Item1 > poly2[i].Item1)
                {
                    startingPoint2 = poly2[i];
                }

                if (endingPoint2.Item1 == poly2[i].Item1 && endingPoint2.Item2 > poly2[i].Item2)
                {
                    endingPoint2 = poly2[i];
                }
                if (endingPoint2.Item1 < poly2[i].Item1)
                {
                    endingPoint2 = poly2[i];
                }
            }

            Console.WriteLine();
            Console.WriteLine($"s1: {startingPoint1}");
            Console.WriteLine($"e1: {endingPoint1}");
            Console.WriteLine($"s2: {startingPoint2}");
            Console.WriteLine($"e2: {endingPoint2}");

            //Dzielenie na górę i dół

            var upper1 = new List<(double, double)>();
            var down1 = new List<(double, double)>();

            upper1.Add(startingPoint1);
            down1.Add(startingPoint1);

            foreach(var i in poly1)
            {
                if((i.Item1 == startingPoint1.Item1 && i.Item2 == startingPoint1.Item2) || (i.Item1 == endingPoint1.Item1 && i.Item2 == endingPoint1.Item2))
                {
                    continue;
                }
                else
                {
                    if( Cross(startingPoint1, endingPoint1, i) >=0)
                    {
                        upper1.Add(i);
                    }
                    else
                    {
                        down1.Add(i);
                    }
                }
            }

            upper1.Add(endingPoint1);
            down1.Add(endingPoint1);

            Console.Write("Upper1: ");
            print(upper1);
            Console.Write("Down1: ");
            print(down1);



            var upper2 = new List<(double, double)>();
            var down2 = new List<(double, double)>();

            upper2.Add(startingPoint2);
            down2.Add(startingPoint2);

            foreach (var i in poly2)
            {
                if ((i.Item1 == startingPoint2.Item1 && i.Item2 == startingPoint2.Item2) || (i.Item1 == endingPoint2.Item1 && i.Item2 == endingPoint2.Item2))
                {
                    continue;
                }
                else
                {
                    if (Cross(startingPoint2, endingPoint2, i) >= 0)
                    {
                        upper2.Add(i);
                    }
                    else
                    {
                        down2.Add(i);
                    }
                }
            }

            upper2.Add(endingPoint2);
            down2.Add(endingPoint2);

            Console.Write("Upper2: ");
            print(upper2);
            Console.Write("Down2: ");
            print(down2);

            return ConvexHull(poly);

        }

    }
}
