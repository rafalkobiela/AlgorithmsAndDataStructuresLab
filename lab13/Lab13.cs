using System;
using System.Linq;
using ASD.Graphs;
using System.Collections.Generic;

namespace Lab13
{
public class ProgramPlanning  : MarshalByRefObject
    {

        /// <summary>
        ///
        /// </summary>
        /// <param name="taskGraph">Graf opisujący zależności procedur</param>
        /// <param name="taskTimes">Tablica długości czasów procedur</param>
        /// <param name="startTimes">Parametr wyjśćiowy z najpóźniejszymi możliwymi startami procedur przy optymalnym czasie całości</param>
        /// <param name="startTimes">Parametr wyjśćiowy z dowolna wybraną ścieżką krytyczną</param>
        /// <returns>Najkrótszy czas w jakim można wykonać cały program</returns>
        public double CalculateTimesLatestPossible(Graph taskGraph, double[] taskTimes, out double[] startTimes, out int[] criticalPath)
            {
            startTimes = null;
            criticalPath = null;
            return -1;
            }
    }
}
