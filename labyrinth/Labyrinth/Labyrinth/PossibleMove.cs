using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabyrinthTask.Labirynth
{
	enum PossibleMove
	{
		Up = 1,
		Down = 2,
		Left = 3,
		Right = 4
	}

	static class PossibleMoveHelpers {
		static private Dictionary<PossibleMove, PossibleMove> moveReversalMapping = 
			new Dictionary<PossibleMove, PossibleMove>()
		{
			{ PossibleMove.Up, PossibleMove.Down },
			{ PossibleMove.Down, PossibleMove.Up },
			{ PossibleMove.Left, PossibleMove.Right },
			{ PossibleMove.Right, PossibleMove.Left }
		};

		public static IEnumerable<PossibleMove> GetAllPossibleMoves()
		{
			return (PossibleMove[]) Enum.GetValues(typeof(PossibleMove));
		}

		public static PossibleMove GetReverseMove(this PossibleMove move)
		{
			return moveReversalMapping[move];
		}
	}
}
