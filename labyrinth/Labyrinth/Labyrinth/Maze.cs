using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using LabyrinthTask.Extensions;

namespace LabyrinthTask.Labirynth
{
	class Maze : IMazeContainingTeleports
	{
		private Maze() { }

		public int [][] Layout { get; private set; }

		public int Width { get; private set;}
		public int Height { get; private set; }
		public Point Start { get; set; }
		public Point Goal { get; set; }

		public static Maze ParseMaze(int width, int height, string [] mazeLayout) {
			var newMaze = new Maze {
				Width = width,
				Height = height,
				Layout = new int[height][]
			};

			for (int i = 0; i < newMaze.Height; i++)
			{
				var parsedRow = mazeLayout[i]
					.SplitBySpaces()
					.Select(o => int.Parse(o))
					.ToList();

				int index = parsedRow.IndexOf((int)MazeField.StartPoint);
				if (index != -1)
					newMaze.Start = new Point(index, i);
				index = parsedRow.IndexOf((int)MazeField.Goal);
				if (index != -1)
					newMaze.Goal = new Point(index, i);

				newMaze.Layout[i] = parsedRow.ToArray();

				Debug.Assert(newMaze.Layout[i].Count() == newMaze.Width);
			}

			return newMaze;
		}

		public bool IsValidDestination(Point destinationCoordinates)
		{
			return IsNotOutsideMaze(destinationCoordinates) && IsEmptyField(destinationCoordinates);
		}

		public bool IsFieldTeleport(Point mazeCoordinate)
		{
			return IsTeleport(GetMazeValueAtPoint(mazeCoordinate));
		}

		public Point EnterTeleport(Point point)
		{
			return FindTeleportExit(point);
		}

		public IEnumerable<PossibleMove> GetAvaialableMoves(Point fromCoordinate)
		{
			return PossibleMoveHelpers
				.GetAllPossibleMoves()
				.Where(move => IsValidDestination(MoveFromPosition(fromCoordinate, move)));
		}

		public Point Move(Point pointToExpand, PossibleMove move)
		{
			var newPosition = MoveFromPosition(pointToExpand, move);

			if (!IsValidDestination(newPosition)) {
				throw new InvalidOperationException();
			}

			return newPosition;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			foreach (var row in Layout) {
				var rowAsString = string.Join(" ", row.Select(o => {
					if (IsTeleport(o))
						return o.ToString();

					return ((MazeField)o).ToDisplayString();
				}));

				sb.AppendLine(rowAsString);
			}

			return sb.ToString();
		}

		private Point FindTeleportExit(Point teleportEntrance)
		{
			var teleportNumber = GetMazeValueAtPoint(teleportEntrance);

			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					var teleportExitCandidate = new Point(x, y);

					if (teleportExitCandidate != teleportEntrance 
						&& GetMazeValueAtPoint(teleportExitCandidate) == teleportNumber)
					{
						return teleportExitCandidate;
					}
				}
			}

			throw new InvalidOperationException(string.Format("No teleport {0} exit!", teleportNumber));
		}

		private int GetMazeValueAtPoint(Point coordinate)
		{
			return Layout[coordinate.Y][coordinate.X];
		}

		private bool IsTeleport(int value)
		{
			return value > 6 && value < 100;
		}

		private bool IsEmptyField(Point mazeCoordinate)
		{
			var fieldValue = GetMazeValueAtPoint(mazeCoordinate);
			return IsTeleport(fieldValue) || ((MazeField)fieldValue).CanEnter();
		}

		private Point MoveFromPosition(Point mazeCoordinate, PossibleMove move)
		{
			var moveDelta = moveToDeltaMapping[move];
			return new Point(mazeCoordinate.X + moveDelta.X, mazeCoordinate.Y + moveDelta.Y);
		}

		private bool IsNotOutsideMaze(Point mazeCoordinate)
		{
			return 
				mazeCoordinate.X >= 0 && mazeCoordinate.X < Width 
				&& mazeCoordinate.Y >= 0 && mazeCoordinate.Y < Height;
		}

		static private Dictionary<PossibleMove, Point> moveToDeltaMapping = new Dictionary<PossibleMove, Point>() 
		{
			{ PossibleMove.Up, new Point(0, -1) },
			{ PossibleMove.Down, new Point(0, 1) },
			{ PossibleMove.Left, new Point(-1, 0) },
			{ PossibleMove.Right, new Point(1, 0) },
		};
	}

	struct Penalities
	{
		public readonly int StepPenality;
		public readonly int TeleportPenality;

		public Penalities(int stepPenality = 1, int teleportPenality = 1)
		{
			StepPenality = stepPenality;
			TeleportPenality = teleportPenality;
		}
	}
}
