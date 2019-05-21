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
            return Math.Sqrt((p1.Item1 - p2.Item1) * (p1.Item1 - p2.Item1) + (p1.Item2 - p2.Item2) * (p1.Item2 - p2.Item2));
        }




        public (double, double)[] mergeArraysDown((double, double)[] t1, (double, double)[] t2)
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
                    if (t1[t1_iter].Item1 == t2[t2_iter].Item1)
                    {
                        if (t1[t1_iter].Item2 > t2[t2_iter].Item2)
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
                    else if (t1[t1_iter].Item1 > t2[t2_iter].Item1)
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


        public (double, double)[] mergeArraysUp((double, double)[] t1, (double, double)[] t2)
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
                    if (t1[t1_iter].Item1 == t2[t2_iter].Item1)
                    {
                        if (t1[t1_iter].Item2 < t2[t2_iter].Item2)
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
                    else if (t1[t1_iter].Item1 < t2[t2_iter].Item1)
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

            for (int k = 1; k < n - 1; k++)
            {
                while (convexHull.Count >= 2 && Cross(convexHull[convexHull.Count - 2], convexHull[convexHull.Count - 1], sortedPoints[k]) <= 0)
                    convexHull.RemoveAt(convexHull.Count - 1);

                convexHull.Add(sortedPoints[k]);
            }

            return convexHull.ToArray();
        }


        // Etap 2
        // oblicza otoczkę dwóch wielokątów wypukłych
        public (double, double)[] ConvexHullOfTwo((double, double)[] poly1, (double, double)[] poly2)
        {


            int n1 = poly1.Length;
            int n2 = poly2.Length;

            //ma być najbardziej po lewo dół
            (double, double) startingPoint1 = poly1[0];
            (double, double) startingPoint2 = poly2[0];
            int startingIndex1 = 0;
            int startingIndex2 = 0;

            //ma być nabardziej po prawo na góra
            (double, double) endingPoint1 = poly1[0];
            (double, double) endingPoint2 = poly2[0];
            int endingIndex1 = 0;
            int endingIndex2 = 0;


            for (int i = 1; i < n1; i++)
            {
                if (startingPoint1.Item1 == poly1[i].Item1 && startingPoint1.Item2 > poly1[i].Item2)
                {
                    startingPoint1 = poly1[i];
                    startingIndex1 = i;
                }
                if (startingPoint1.Item1 > poly1[i].Item1)
                {
                    startingPoint1 = poly1[i];
                    startingIndex1 = i;
                }

                if (endingPoint1.Item1 == poly1[i].Item1 && endingPoint1.Item2 < poly1[i].Item2)
                {
                    endingPoint1 = poly1[i];
                    endingIndex1 = i;
                }
                if (endingPoint1.Item1 < poly1[i].Item1)
                {
                    endingPoint1 = poly1[i];
                    endingIndex1 = i;
                }
            }


            for (int i = 1; i < n2; i++)
            {
                if (startingPoint2.Item1 == poly2[i].Item1 && startingPoint2.Item2 > poly2[i].Item2)
                {
                    startingPoint2 = poly2[i];
                    startingIndex2 = i;
                }
                if (startingPoint2.Item1 > poly2[i].Item1)
                {
                    startingPoint2 = poly2[i];
                    startingIndex2 = i;
                }

                if (endingPoint2.Item1 == poly2[i].Item1 && endingPoint2.Item2 < poly2[i].Item2)
                {
                    endingPoint2 = poly2[i];
                    endingIndex2 = i;
                }
                if (endingPoint2.Item1 < poly2[i].Item1)
                {
                    endingPoint2 = poly2[i];
                    endingIndex2 = i;
                }
            }


            //Dzielenie na górę i dół

            var upper1 = new List<(double, double)>();
            var down1 = new List<(double, double)>();

            down1.Add(startingPoint1);
            bool afterEnding = false;
            for (int i = 1; i <= n1; i++)
            {
                if (poly1[(i + startingIndex1) % n1].Equals(endingPoint1))
                {
                    afterEnding = true;
                    down1.Add(poly1[(i + startingIndex1) % n1]);
                }
                if (afterEnding)
                {
                    upper1.Add(poly1[(i + startingIndex1) % n1]);
                }
                else
                {
                    down1.Add(poly1[(i + startingIndex1) % n1]);
                }
            }

            var upper2 = new List<(double, double)>();
            var down2 = new List<(double, double)>();

            down2.Add(startingPoint2);
            afterEnding = false;

            for (int i = 1; i <= n2; i++)
            {
                if (poly2[(i + startingIndex2) % n2].Equals(endingPoint2))
                {
                    afterEnding = true;
                    down2.Add(poly2[(i + startingIndex2) % n2]);
                }
                if (afterEnding)
                {
                    upper2.Add(poly2[(i + startingIndex2) % n2]);
                }
                else
                {
                    down2.Add(poly2[(i + startingIndex2) % n2]);
                }
            }


            //Łączenie tablic 

            var down = mergeArraysDown(down1.ToArray(), down2.ToArray());
            var up = mergeArraysUp(upper1.ToArray(), upper2.ToArray());


            List<(double, double)> convexHullDown = new List<(double, double)>();
            convexHullDown.Add(down[0]);
            convexHullDown.Add(down[1]);

            for (int k = 2; k < down.Length; k++)
            {
                while (convexHullDown.Count >= 2 &&
                        Cross(convexHullDown[convexHullDown.Count - 2], convexHullDown[convexHullDown.Count - 1], down[k]) <= 0)
                    convexHullDown.RemoveAt(convexHullDown.Count - 1);

                convexHullDown.Add(down[k]);
            }

            List<(double, double)> convexHullUp = new List<(double, double)>();
            convexHullUp.Add(up[0]);
            convexHullUp.Add(up[1]);

            for (int k = 2; k < up.Length; k++)
            {

                while (convexHullUp.Count >= 2 &&
                        Cross(convexHullUp[convexHullUp.Count - 2], convexHullUp[convexHullUp.Count - 1], up[k]) <= 0)
                    convexHullUp.RemoveAt(convexHullUp.Count - 1);

                convexHullUp.Add(up[k]);
            }


            List<(double, double)> convexHull = new List<(double, double)>();

            foreach(var i in convexHullDown)
            {
                convexHull.Add(i);
            }

            for(int i = 1; i< convexHullUp.Count - 1; i++)
            {
                convexHull.Add(convexHullUp[i]);
            }

            return convexHull.ToArray();

        }

    }
}
