using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabyrinthTask.Labirynth 
{
	interface IMazeContainingTeleports
	{
		bool IsValidDestination(Point destinationCoordinates);
		bool IsFieldTeleport(Point mazeCoordinate);

		IEnumerable<PossibleMove> GetAvaialableMoves(Point fromCoordinate);

		Point Move(Point pointToExpand, PossibleMove move);
		Point EnterTeleport(Point point);

		int Width { get; }
		int Height { get; }
		Point Start { get; set; }
		Point Goal { get; set; }
	}
}
