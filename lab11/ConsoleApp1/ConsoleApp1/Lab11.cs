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
                Console.Write($"({i.Item1},{i.Item2})");
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


            n = poly.Length;
            (double, double) startingPoint = poly[0];
            for (int i = 1; i < n; i++)
            {
                if (poly[i].Item2 < startingPoint.Item2)
                    startingPoint = poly[i];
                else if (poly[i].Item2 == startingPoint.Item2 && poly[i].Item1 < startingPoint.Item1)
                    startingPoint = poly[i];
            }

            poly = mergeArrays(poly1, poly2);



            return ConvexHull(poly);

        }

    }
}
