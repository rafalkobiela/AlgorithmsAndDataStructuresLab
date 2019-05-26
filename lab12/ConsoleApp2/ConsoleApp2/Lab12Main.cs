﻿using ASD;
using System;
using System.Collections.Generic;
using System.Text;
using GraphX;

namespace CSG
{
    public class MakeIntersectionPointsTestCase : TestCase
    {
        private Polygon source;
        private Polygon clip;
        private Polygon expectedSource;
        private Polygon expectedClip;
        private Polygon resultedSource;
        private Polygon resultedClip;

        public MakeIntersectionPointsTestCase(double timelimit, string description, Polygon source, Polygon clip, Polygon expectedSource, Polygon expectedClip) 
            : base(timelimit, null, description)
        {
            this.source = source;
            this.clip = clip;
            this.expectedClip = expectedClip;
            this.expectedSource = expectedSource;
        }

        protected override void PerformTestCase(object prototypeObject)
        {
            (resultedSource, resultedClip) = ((Clipper)prototypeObject).MakeIntersectionPoints(source, clip);
        }
        
        protected override (Result resultCode, string message) VerifyTestCase(object settings)
        {
            String sourceMessage;
            String clipMessage;

            if(resultedSource == null || resultedClip == null)
                return (Result.WrongResult, $"X  At least one of returned polygons is null");

            bool sourceOK = expectedSource.IsTheSame(resultedSource, false, out sourceMessage);
            bool clipOK = expectedClip.IsTheSame(resultedClip, false, out clipMessage);

            if (sourceOK && clipOK)
                return (Result.Success, $"OK, {PerformanceTime:#0.00}");
            else if(sourceOK && !clipOK)
                return (Result.WrongResult, $"X  clip is not as expected: {clipMessage}");
            else if (!sourceOK && clipOK)
                return (Result.WrongResult, $"X  source is not as expected: {sourceMessage}");
            else
                return (Result.WrongResult, $"X  source and clip is not as expected. Clip: {clipMessage}. Source: {sourceMessage}");
        }
    }

    public class MarkEntryExitPointsTestCase : TestCase
    {
        private Polygon source;
        private Polygon clip;
        private Polygon expectedSource;
        private Polygon expectedClip;
        private Polygon resultedSource;
        private Polygon resultedClip;
        public MarkEntryExitPointsTestCase(double timelimit, string description, Polygon source, Polygon clip, Polygon expectedSource, Polygon expectedClip)
            : base(timelimit, null, description)
        {
            this.source = source;
            this.clip = clip;
            this.expectedClip = expectedClip;
            this.expectedSource = expectedSource;
        }

        protected override void PerformTestCase(object prototypeObject)
        {
            (resultedSource, resultedClip) = ((Clipper)prototypeObject).MarkEntryExitPoints(source, clip);
        }

        protected override (Result resultCode, string message) VerifyTestCase(object settings)
        {
            String sourceMessage;
            String clipMessage;

            if (resultedSource == null || resultedClip == null)
                return (Result.WrongResult, $"X  At least one of returned polygons is null");

            bool sourceOK = expectedSource.IsTheSame(resultedSource, true, out sourceMessage);
            bool clipOK = expectedClip.IsTheSame(resultedClip, true, out clipMessage);

            if (sourceOK && clipOK)
                return (Result.Success, $"OK, {PerformanceTime:#0.00}");
            else if (sourceOK && !clipOK)
                return (Result.WrongResult, $"X  clip is not as expected: {clipMessage}");
            else if (!sourceOK && clipOK)
                return (Result.WrongResult, $"X  source is not as expected: {sourceMessage}");
            else
                return (Result.WrongResult, $"X  source and clip is not as expected. Clip: {clipMessage}. Source: {sourceMessage}");
        }
    }

    public class ReturnClippedPolygonsTestCase : TestCase
    {
        private readonly Polygon source;
        private readonly Polygon clip;
        private readonly List<Polygon> expectedResult;

        private List<Polygon> result;
        public ReturnClippedPolygonsTestCase(double timelimit, string description, Polygon source, Polygon clip, List<Polygon> expectedResult)
            : base(timelimit, null, description)
        {
            this.source = source;
            this.clip = clip;
            this.expectedResult = expectedResult;
        }

        protected override void PerformTestCase(object prototypeObject)
        {
            result = ((Clipper)prototypeObject).ReturnClippedPolygons(source, clip);
        }

        protected override (Result resultCode, string message) VerifyTestCase(object settings)
        {
            if(result == null)
                return (Result.WrongResult, $"X  result is null");

            if (expectedResult.Count != result.Count)
                return (Result.WrongResult, $"X  number of polygons in result is wrong: {result.Count} (expected: {expectedResult.Count})");

            bool[] foundInExpectedResult = new bool[expectedResult.Count];
            bool[] foundInResult = new bool[result.Count];
            int foundInExpectedResultTrueCount = 0;
            int foundInResultTrueCount = 0;

            string message;

            for (int i = 0; i < expectedResult.Count; i++)
            {
                for (int j = 0; j < result.Count; j++)
                {

                    if(!foundInExpectedResult[i] && !foundInResult[j])
                        if(expectedResult[i].IsTheSame(result[j], true, out message))
                        {
                            foundInExpectedResult[i] = true;
                            foundInResult[j] = true;
                            foundInExpectedResultTrueCount++;
                            foundInResultTrueCount++;
                        }
                }
            }

            if (foundInResultTrueCount == result.Count && foundInExpectedResultTrueCount == expectedResult.Count)
                return (Result.Success, $"OK, {PerformanceTime:#0.00}");
            else
            {
                StringBuilder studentPolygons = new StringBuilder();
                StringBuilder expectedPolygons = new StringBuilder();

                for (int i = 0; i < result.Count; i++)
                {
                    if (!foundInResult[i])
                        studentPolygons.Append(result[i]);
                }

                for (int i = 0; i < expectedResult.Count; i++)
                {
                    if (!foundInExpectedResult[i])
                        expectedPolygons.Append(expectedResult[i]);
                }

                return (Result.Success, $"X  There are {result.Count - foundInResultTrueCount} polygon(s) in your solution, which are not in expected result: {studentPolygons.ToString()}. And there are {expectedResult.Count - foundInExpectedResultTrueCount} polygon(s) in expected result, which are not found in yours: {expectedPolygons.ToString()}");
            }
        }
    }

    class Lab12TestModule : TestModule
    {
        public override void PrepareTestSets()
        {
            PrepareLabTestSets();
        }

        private void PrepareLabTestSets()
        {
            PrepareMakeIntersectionPointsLabTestSets();
            PrepareMarkEntryExitPointsLabTestSets();
            PrepareReturnClippedPolygonsLabTestSets();
        }

        private void PrepareMakeIntersectionPointsLabTestSets()
        {
            TestSets["MakeIntersectionPoints"] = new TestSet(new Clipper(), "Część 1. -- MakeIntersectionPoints() (1 p.)\n");

            Vertex[] square1 = { new Vertex(-1, -1), new Vertex(-1, 1), new Vertex(1, 1), new Vertex(1, -1) };
            Vertex[] square2 = { new Vertex(0, 0), new Vertex(0, 2), new Vertex(2, 2), new Vertex(2, 0) };
            Polygon sq1 = new Polygon(square1);
            Polygon sq2 = new Polygon(square2);
            Vertex[] square1_expected = { new Vertex(-1, -1), new Vertex(-1, 1), new Vertex(0, 1), new Vertex(1, 1), new Vertex(1, 0), new Vertex(1, -1) };
            Vertex[] square2_expected = { new Vertex(0, 0), new Vertex(0, 1), new Vertex(0, 2), new Vertex(2, 2), new Vertex(2, 0), new Vertex(1, 0) };
            Polygon sq1_expected = new Polygon(square1_expected);
            Polygon sq2_expected = new Polygon(square2_expected);
            TestSets["MakeIntersectionPoints"].TestCases.Add(new MakeIntersectionPointsTestCase(1, "Two squares", sq1, sq2, sq1_expected, sq2_expected));

            Vertex[] square1_2 = { new Vertex(0, 0), new Vertex(0, 2), new Vertex(5, 2), new Vertex(5, 0) };
            Vertex[] square2_2 = { new Vertex(1, 1), new Vertex(2, 1), new Vertex(2, 3), new Vertex(3, 3), new Vertex(3, 1), new Vertex(4, 1), new Vertex(4, 4), new Vertex(1, 4) };
            Polygon sq1_2 = new Polygon(square1_2);
            Polygon sq2_2 = new Polygon(square2_2);
            Vertex[] square1_expected_2 = { new Vertex(0, 0), new Vertex(0, 2), new Vertex(1, 2), new Vertex(2, 2), new Vertex(3, 2), new Vertex(4, 2), new Vertex(5, 2), new Vertex(5, 0) };
            Vertex[] square2_expected_2 = { new Vertex(1, 1), new Vertex(2, 1), new Vertex(2, 2), new Vertex(2, 3), new Vertex(3, 3), new Vertex(3, 2), new Vertex(3, 1), new Vertex(4, 1), new Vertex(4, 2), new Vertex(4, 4), new Vertex(1, 4), new Vertex(1, 2) };
            Polygon sq1_expected_2 = new Polygon(square1_expected_2);
            Polygon sq2_expected_2 = new Polygon(square2_expected_2);
            TestSets["MakeIntersectionPoints"].TestCases.Add(new MakeIntersectionPointsTestCase(1, "Two intersections", sq1_2, sq2_2, sq1_expected_2, sq2_expected_2));

            Vertex[] square1_5 = { new Vertex(0, 0), new Vertex(0, 2), new Vertex(5, 2), new Vertex(5, 0) };
            Vertex[] square2_5 = { new Vertex(1, 1), new Vertex(1, 4), new Vertex(4, 4), new Vertex(4, 1), new Vertex(3, 1), new Vertex(3, 3), new Vertex(2, 3), new Vertex(2, 1) };
            Polygon sq1_5 = new Polygon(square1_5);
            Polygon sq2_5 = new Polygon(square2_5);
            Vertex[] square1_expected_5 = { new Vertex(0, 0), new Vertex(0, 2), new Vertex(1, 2), new Vertex(2, 2), new Vertex(3, 2), new Vertex(4, 2), new Vertex(5, 2), new Vertex(5, 0) };
            Vertex[] square2_expected_5 = { new Vertex(1, 1), new Vertex(2, 1), new Vertex(2, 2), new Vertex(2, 3), new Vertex(3, 3), new Vertex(3, 2), new Vertex(3, 1), new Vertex(4, 1), new Vertex(4, 2), new Vertex(4, 4), new Vertex(1, 4), new Vertex(1, 2) };
            Polygon sq1_expected_5 = new Polygon(square1_expected_2);
            Polygon sq2_expected_5 = new Polygon(square2_expected_2);
            TestSets["MakeIntersectionPoints"].TestCases.Add(new MakeIntersectionPointsTestCase(1, "Two intersections - reversed order", sq1_5, sq2_5, sq1_expected_5, sq2_expected_5));

            Vertex[] square1_3 = { new Vertex(0, 0), new Vertex(0, 3), new Vertex(3, 3), new Vertex(3, 0) };
            Vertex[] square2_3 = { new Vertex(1, 1), new Vertex(2, 1), new Vertex(2, 2), new Vertex(1, 2) };
            Polygon sq1_3 = new Polygon(square1_3);
            Polygon sq2_3 = new Polygon(square2_3);
            Vertex[] square1_expected_3 = { new Vertex(0, 0), new Vertex(0, 3), new Vertex(3, 3), new Vertex(3, 0) };
            Vertex[] square2_expected_3 = { new Vertex(1, 1), new Vertex(2, 1), new Vertex(2, 2), new Vertex(1, 2) };
            Polygon sq1_expected_3 = new Polygon(square1_expected_3);
            Polygon sq2_expected_3 = new Polygon(square2_expected_3);
            TestSets["MakeIntersectionPoints"].TestCases.Add(new MakeIntersectionPointsTestCase(1, "Square in square", sq1_3, sq2_3, sq1_expected_3, sq2_expected_3));

            Vertex[] square1_4 = { new Vertex(0, 0), new Vertex(1, 0), new Vertex(1, 1), new Vertex(0, 1) };
            Vertex[] square2_4 = { new Vertex(3, 0), new Vertex(2, 0), new Vertex(2, 1), new Vertex(3, 1) };
            Polygon sq1_4 = new Polygon(square1_4);
            Polygon sq2_4 = new Polygon(square2_4);
            Vertex[] square1_expected_4 = { new Vertex(0, 0), new Vertex(1, 0), new Vertex(1, 1), new Vertex(0, 1) };
            Vertex[] square2_expected_4 = { new Vertex(3, 0), new Vertex(2, 0), new Vertex(2, 1), new Vertex(3, 1) };
            Polygon sq1_expected_4 = new Polygon(square1_expected_4);
            Polygon sq2_expected_4 = new Polygon(square2_expected_4);
            TestSets["MakeIntersectionPoints"].TestCases.Add(new MakeIntersectionPointsTestCase(1, "Separate squares", sq1_4, sq2_4, sq1_expected_4, sq2_expected_4));

        }

        private void PrepareMarkEntryExitPointsLabTestSets()
        {
            TestSets["MarkEntryExitPoints"] = new TestSet(new Clipper(), "Część 2. -- MarkEntryExitPoints() (1 p.)\n");

            Vertex[] square1 = { new Vertex(-1, -1), new Vertex(-1, 1), new Vertex(1, 1), new Vertex(1, -1) };
            Vertex[] square2 = { new Vertex(0, 0), new Vertex(0, 2), new Vertex(2, 2), new Vertex(2, 0) };
            Polygon sq1 = new Polygon(square1);
            Polygon sq2 = new Polygon(square2);
            Vertex[] square1_expected = { new Vertex(-1, -1), new Vertex(-1, 1), new Vertex(0, 1, true), new Vertex(1, 1), new Vertex(1, 0, false), new Vertex(1, -1) };
            Vertex[] square2_expected = { new Vertex(0, 0), new Vertex(0, 1, false), new Vertex(0, 2), new Vertex(2, 2), new Vertex(2, 0), new Vertex(1, 0, true) };
            Polygon sq1_expected = new Polygon(square1_expected);
            Polygon sq2_expected = new Polygon(square2_expected);
            TestSets["MarkEntryExitPoints"].TestCases.Add(new MarkEntryExitPointsTestCase(1, "Two squares", sq1, sq2, sq1_expected, sq2_expected));

            Vertex[] square1_2 = { new Vertex(0, 0), new Vertex(0, 2), new Vertex(5, 2), new Vertex(5, 0) };
            Vertex[] square2_2 = { new Vertex(1, 1), new Vertex(2, 1), new Vertex(2, 3), new Vertex(3, 3), new Vertex(3, 1), new Vertex(4, 1), new Vertex(4, 4), new Vertex(1, 4) };
            Polygon sq1_2 = new Polygon(square1_2);
            Polygon sq2_2 = new Polygon(square2_2);
            Vertex[] square1_expected_2 = { new Vertex(0, 0), new Vertex(0, 2), new Vertex(1, 2, true), new Vertex(2, 2, false), new Vertex(3, 2, true), new Vertex(4, 2, false), new Vertex(5, 2), new Vertex(5, 0) };
            Vertex[] square2_expected_2 = { new Vertex(1, 1), new Vertex(2, 1), new Vertex(2, 2, false), new Vertex(2, 3), new Vertex(3, 3), new Vertex(3, 2, true), new Vertex(3, 1), new Vertex(4, 1), new Vertex(4, 2, false), new Vertex(4, 4), new Vertex(1, 4), new Vertex(1, 2, true) };
            Polygon sq1_expected_2 = new Polygon(square1_expected_2);
            Polygon sq2_expected_2 = new Polygon(square2_expected_2);
            TestSets["MarkEntryExitPoints"].TestCases.Add(new MarkEntryExitPointsTestCase(1, "Two intersections", sq1_2, sq2_2, sq1_expected_2, sq2_expected_2));

            Vertex[] square1_5 = { new Vertex(0, 0), new Vertex(0, 2), new Vertex(5, 2), new Vertex(5, 0) };
            Vertex[] square2_5 = { new Vertex(1, 1), new Vertex(1, 4), new Vertex(4, 4), new Vertex(4, 1), new Vertex(3, 1), new Vertex(3, 3), new Vertex(2, 3), new Vertex(2, 1) };
            Polygon sq1_5 = new Polygon(square1_5);
            Polygon sq2_5 = new Polygon(square2_5);
            Vertex[] square1_expected_5 = { new Vertex(0, 0), new Vertex(0, 2), new Vertex(1, 2, true), new Vertex(2, 2, false), new Vertex(3, 2, true), new Vertex(4, 2, false), new Vertex(5, 2), new Vertex(5, 0) };
            Vertex[] square2_expected_5 = { new Vertex(1, 1), new Vertex(2, 1), new Vertex(2, 2, true), new Vertex(2, 3), new Vertex(3, 3), new Vertex(3, 2, false), new Vertex(3, 1), new Vertex(4, 1), new Vertex(4, 2, true), new Vertex(4, 4), new Vertex(1, 4), new Vertex(1, 2, false) };
            Polygon sq1_expected_5 = new Polygon(square1_expected_5);
            Polygon sq2_expected_5 = new Polygon(square2_expected_5);
            TestSets["MarkEntryExitPoints"].TestCases.Add(new MarkEntryExitPointsTestCase(1, "Two intersections - reversed order", sq1_5, sq2_5, sq1_expected_5, sq2_expected_5));

            Vertex[] square1_3 = { new Vertex(0, 0), new Vertex(0, 3), new Vertex(3, 3), new Vertex(3, 0) };
            Vertex[] square2_3 = { new Vertex(1, 1), new Vertex(2, 1), new Vertex(2, 2), new Vertex(1, 2) };
            Polygon sq1_3 = new Polygon(square1_3);
            Polygon sq2_3 = new Polygon(square2_3);
            Vertex[] square1_expected_3 = { new Vertex(0, 0), new Vertex(0, 3), new Vertex(3, 3), new Vertex(3, 0) };
            Vertex[] square2_expected_3 = { new Vertex(1, 1), new Vertex(2, 1), new Vertex(2, 2), new Vertex(1, 2) };
            Polygon sq1_expected_3 = new Polygon(square1_expected_3);
            Polygon sq2_expected_3 = new Polygon(square2_expected_3);
            TestSets["MarkEntryExitPoints"].TestCases.Add(new MarkEntryExitPointsTestCase(1, "Square in square", sq1_3, sq2_3, sq1_expected_3, sq2_expected_3));

            Vertex[] square1_4 = { new Vertex(0, 0), new Vertex(1, 0), new Vertex(1, 1), new Vertex(0, 1) };
            Vertex[] square2_4 = { new Vertex(3, 0), new Vertex(2, 0), new Vertex(2, 1), new Vertex(3, 1) };
            Polygon sq1_4 = new Polygon(square1_4);
            Polygon sq2_4 = new Polygon(square2_4);
            Vertex[] square1_expected_4 = { new Vertex(0, 0), new Vertex(1, 0), new Vertex(1, 1), new Vertex(0, 1) };
            Vertex[] square2_expected_4 = { new Vertex(3, 0), new Vertex(2, 0), new Vertex(2, 1), new Vertex(3, 1) };
            Polygon sq1_expected_4 = new Polygon(square1_expected_4);
            Polygon sq2_expected_4 = new Polygon(square2_expected_4);
            TestSets["MarkEntryExitPoints"].TestCases.Add(new MarkEntryExitPointsTestCase(1, "Separate squares", sq1_4, sq2_4, sq1_expected_4, sq2_expected_4));
        }

        private void PrepareReturnClippedPolygonsLabTestSets()
        {
            TestSets["ReturnClippedPolygons"] = new TestSet(new Clipper(), "Część 3. -- ReturnClippedPolygons() (2 p.)\n");

            Vertex[] square1 = { new Vertex(-1, -1), new Vertex(-1, 1), new Vertex(1, 1), new Vertex(1, -1) };
            Vertex[] square2 = { new Vertex(0, 0), new Vertex(0, 2), new Vertex(2, 2), new Vertex(2, 0) };
            Polygon sq1 = new Polygon(square1);
            Polygon sq2 = new Polygon(square2);
            Vertex[] square1_expected = { new Vertex(0, 0), new Vertex(0, 1), new Vertex(1, 1), new Vertex(1, 0) };
            Polygon sq1_expected = new Polygon(square1_expected);
            List<Polygon> expectedResult = new List<Polygon>();
            expectedResult.Add(sq1_expected);
            TestSets["ReturnClippedPolygons"].TestCases.Add(new ReturnClippedPolygonsTestCase(1, "Two squares", sq1, sq2, expectedResult));

            Vertex[] square1_2 = { new Vertex(0, 0), new Vertex(0, 2), new Vertex(5, 2), new Vertex(5, 0) };
            Vertex[] square2_2 = { new Vertex(1, 1), new Vertex(2, 1), new Vertex(2, 3), new Vertex(3, 3), new Vertex(3, 1), new Vertex(4, 1), new Vertex(4, 4), new Vertex(1, 4) };
            Polygon sq1_2 = new Polygon(square1_2);
            Polygon sq2_2 = new Polygon(square2_2);
            Vertex[] square1_expected_2 = { new Vertex(1, 2), new Vertex(2, 2), new Vertex(2, 1), new Vertex(1, 1) };
            Vertex[] square2_expected_2 = { new Vertex(3, 2), new Vertex(4, 2), new Vertex(4, 1), new Vertex(3, 1) };
            Polygon sq1_expected_2 = new Polygon(square1_expected_2);
            Polygon sq2_expected_2 = new Polygon(square2_expected_2);
            List<Polygon> expectedResult2 = new List<Polygon>();
            expectedResult2.Add(sq1_expected_2);
            expectedResult2.Add(sq2_expected_2);
            TestSets["ReturnClippedPolygons"].TestCases.Add(new ReturnClippedPolygonsTestCase(1, "Two intersections", sq1_2, sq2_2, expectedResult2));

            Vertex[] square1_5 = { new Vertex(0, 0), new Vertex(0, 2), new Vertex(5, 2), new Vertex(5, 0) };
            Vertex[] square2_5 = { new Vertex(1, 1), new Vertex(1, 4), new Vertex(4, 4), new Vertex(4, 1), new Vertex(3, 1), new Vertex(3, 3), new Vertex(2, 3), new Vertex(2, 1) };
            Polygon sq1_5 = new Polygon(square1_5);
            Polygon sq2_5 = new Polygon(square2_5);
            Vertex[] square1_expected_5 = { new Vertex(1, 2), new Vertex(2, 2), new Vertex(2, 1), new Vertex(1, 1) };
            Vertex[] square2_expected_5 = { new Vertex(3, 2), new Vertex(4, 2), new Vertex(4, 1), new Vertex(3, 1) };
            Polygon sq1_expected_5 = new Polygon(square1_expected_5);
            Polygon sq2_expected_5 = new Polygon(square2_expected_5);
            List<Polygon> expectedResult5 = new List<Polygon>();
            expectedResult5.Add(sq1_expected_5);
            expectedResult5.Add(sq2_expected_5);
            TestSets["ReturnClippedPolygons"].TestCases.Add(new ReturnClippedPolygonsTestCase(1, "Two intersections - reversed order", sq1_5, sq2_5, expectedResult5));

            Vertex[] square1_3 = { new Vertex(0, 0), new Vertex(0, 3), new Vertex(3, 3), new Vertex(3, 0) };
            Vertex[] square2_3 = { new Vertex(1, 1), new Vertex(2, 1), new Vertex(2, 2), new Vertex(1, 2) };
            Polygon sq1_3 = new Polygon(square1_3);
            Polygon sq2_3 = new Polygon(square2_3);
            Vertex[] square1_expected_3 = { new Vertex(1, 1), new Vertex(2, 1), new Vertex(2, 2), new Vertex(1, 2) };
            Polygon sq1_expected_3 = new Polygon(square1_expected_3);
            List<Polygon> expectedResult3 = new List<Polygon>();
            expectedResult3.Add(sq1_expected_3);
            TestSets["ReturnClippedPolygons"].TestCases.Add(new ReturnClippedPolygonsTestCase(1, "Square in square", sq1_3, sq2_3, expectedResult3));

            Vertex[] square1_4 = { new Vertex(0, 0), new Vertex(1, 0), new Vertex(1, 1), new Vertex(0, 1) };
            Vertex[] square2_4 = { new Vertex(3, 0), new Vertex(2, 0), new Vertex(2, 1), new Vertex(3, 1) };
            Polygon sq1_4 = new Polygon(square1_4);
            Polygon sq2_4 = new Polygon(square2_4);
            List<Polygon> expectedResult4 = new List<Polygon>();
            TestSets["ReturnClippedPolygons"].TestCases.Add(new ReturnClippedPolygonsTestCase(1, "Separate squares", sq1_4, sq2_4, expectedResult4));
        }
    }

    public class Lab12Main
    {
        public static void Main()
        {
            var testRunner = new Lab12TestModule();
            testRunner.PrepareTestSets();

            foreach (var testSet in testRunner.TestSets)
                testSet.Value.PerformTests(false);
        }
    }
}
