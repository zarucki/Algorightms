using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LabyrinthTask.Labirynth;

namespace LabyrinthTask
{
	class AStar
	{
		public static IEnumerable<Point> ShortestPath(
			IMazeContainingTeleports maze, 
			Penalities costs, 
			Func<Point, int> heuristicFunction = null)
		{
			var takenActions = new PossibleMove[maze.Width, maze.Height];

			var expanded = new bool[maze.Width, maze.Height];
			expanded[maze.Start.X, maze.Start.Y] = true;

			var openList = new List<Point>() { maze.Start };

			while (openList.Count > 0)
			{
				var pointToExpand = openList
					.OrderBy(o => o.G + o.H)
					.ThenByDescending(o => o.G)
					.First();

				openList.Remove(pointToExpand);

				if (pointToExpand == maze.Goal) {
					return GetCompletePathThroughMaze(maze, takenActions);
				}

				foreach (var move in maze.GetAvaialableMoves(pointToExpand))
				{
					var newPoint = maze.Move(pointToExpand, move);

					Point? teleportEntrance = null;
					if (maze.IsFieldTeleport(newPoint))
					{
						teleportEntrance = newPoint;
						newPoint = maze.EnterTeleport(newPoint);
					}

					if (expanded[newPoint.X, newPoint.Y]) {
					   continue;
					}

					var newCost = pointToExpand.G 
						+ costs.StepPenality
						+ (teleportEntrance != null ? costs.TeleportPenality : 0);

					var heuristic = 0;
					if (heuristicFunction != null)
					{
						heuristic = teleportEntrance != null ? 
							heuristicFunction(teleportEntrance.Value) : 
							heuristicFunction(newPoint);
					}

					openList.Add(new Point(newPoint.X, newPoint.Y, newCost, heuristic));

					expanded[newPoint.X, newPoint.Y] = true;
					takenActions[newPoint.X, newPoint.Y] = move;
				}
			}

			throw new InvalidOperationException("Have not found goal");
		}

		private static IEnumerable<Point> GetCompletePathThroughMaze(
			IMazeContainingTeleports maze, 
			PossibleMove[,] takenActions)
		{
			var path = new List<Point>() { maze.Goal };

			var currentPoint = maze.Goal;
			var actionTakenToGetToCurrentPoint = takenActions[currentPoint.X, currentPoint.Y];

			while (currentPoint != maze.Start) {
				currentPoint = maze.Move(currentPoint, actionTakenToGetToCurrentPoint.GetReverseMove());
				actionTakenToGetToCurrentPoint = takenActions[currentPoint.X, currentPoint.Y];

				if (maze.IsFieldTeleport(currentPoint))
				{
					path.Add(currentPoint);
					actionTakenToGetToCurrentPoint = takenActions[currentPoint.X, currentPoint.Y];

					currentPoint = maze.EnterTeleport(currentPoint);
				}

				path.Add(currentPoint);
			}

			path.Reverse();
			return path;
		}
	}
}
