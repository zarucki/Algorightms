using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using LabyrinthTask.Extensions;
using LabyrinthTask.Labirynth;

namespace LabyrinthTask 
{
	class Program
	{
		static void Main(string[] args)
		{
			int mazeCount = int.Parse(Console.ReadLine().Trim());

			while (mazeCount-- > 0) {
				var maze = ParseMazeDefintion();

				var penalities = new Penalities(stepPenality: 1, teleportPenality: 1);

				var heuristic = Heuristics.PrepareHeuristicByDynamicProgramming(maze, penalities);

				var shortestPathPointList = AStar.ShortestPath(maze, penalities, heuristic);

				Console.WriteLine(FormatOutput(shortestPathPointList, maze.Height));
			}
		}

		static Maze ParseMazeDefintion()
		{
			var dimensions = Console.ReadLine()
				.SplitBySpaces()
				.Select(o => int.Parse(o));

			int width = dimensions.ElementAt(0);
			int height = dimensions.ElementAt(1);

			var mazeLayout = new string[height];
			for (int row = 0; row < height; row++)
			{
				mazeLayout[row] = Console.ReadLine();
			}

			return Maze.ParseMaze(width, height, mazeLayout);
		}

		static string FormatOutput(IEnumerable<Point> shortestPathPointList, int height)
		{
			var resultTransposedToOutputFormat = shortestPathPointList
				.Select(o => new Point(o.X, height - 1 - o.Y));

			return string.Join("", resultTransposedToOutputFormat.Select(o => o.ToString()));
		}
	}
}