using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabyrinthTask.Extensions
{
	public static class StringExtensions
	{
		static public string[] SplitBySpaces(this string input)
		{
			return input
				.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		}
	}
}
