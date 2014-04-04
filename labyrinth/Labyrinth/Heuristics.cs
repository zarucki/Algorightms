using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LabyrinthTask.Labirynth;

namespace LabyrinthTask
{
    static class Heuristics
    {
        // We need to think in portals, Manhattan distance heuristic won't work with them.
        // This basically returns the solution by dynamic programing
        public static Func<Point, int> PrepareHeuristicByDynamicProgramming(IMazeContainingTeleports maze, Penalities costs)
        {
            int maximumDistance = 
                (costs.StepPenality + costs.TeleportPenality) * maze.Width * maze.Height;

            var valueTable = new int[maze.Width, maze.Height];
            valueTable[maze.Goal.X, maze.Goal.Y] = maximumDistance;
            
            int iterationDelta;
            do {
                iterationDelta = 0;

                for (int y = 0; y < maze.Height; y++)
                {
                    for (int x = 0; x < maze.Width; x++)		
                    {
                        var currentPoint = new Point(x, y);
                        bool usedTeleport = false;

                        if (currentPoint == maze.Goal || !maze.IsValidDestination(currentPoint))
                        {
                            continue;
                        }

                        if (maze.IsFieldTeleport(currentPoint))
                        {
                            usedTeleport = true;
                            currentPoint = maze.EnterTeleport(currentPoint);
                        }

                        var availbleLocations = maze
                            .GetAvaialableMoves(currentPoint)
                            .Select(o => maze.Move(currentPoint, o));

                        if (!availbleLocations.Any())
                            continue;

                        var newValue = availbleLocations.Max(o => valueTable[o.X, o.Y]) 
                            - costs.StepPenality
                            - (usedTeleport ? costs.TeleportPenality : 0);

                        iterationDelta += newValue - valueTable[x, y];

                        valueTable[x, y] = newValue;
                    }
                }
            } while (iterationDelta != 0);

            return o => maximumDistance - valueTable[o.X, o.Y];
        }
    }
}
