using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabyrinthTask.Labirynth
{
	public struct Point : IEquatable<Point>
	{
		public Point(int x, int y, int g, int h) : this()
		{
			X = x;
			Y = y;
			G = g;
			H = h;
		}

		public Point(int x, int y)
			: this(x, y, 0, 0)
		{
		}

		public int X { get; private set; }
		public int Y { get; private set; }
		public int G { get; private set; }
		public int H { get; private set; }

		public bool Equals(Point other)
		{
			return this.X == other.X && this.Y == other.Y;
		}

		public override bool Equals(Object obj)
		{
			if (obj == null) 
				return false;

			if (!(obj is Point))
				return false;

			return Equals((Point)obj);   
		}   

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + X.GetHashCode();
				hash = hash * 23 + Y.GetHashCode();
				return hash;
			}
		}

		public static bool operator == (Point point1, Point point2)
		{
			return point1.Equals(point2);
		}

		public static bool operator != (Point point1, Point point2)
		{
			return ! point1.Equals(point2);
		}

		public override string ToString()
		{
			return string.Format("({0},{1})", X, Y);
		}
	}
}
