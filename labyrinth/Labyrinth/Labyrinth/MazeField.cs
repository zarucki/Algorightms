using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabyrinthTask.Labirynth
{
	enum MazeField
	{
		[DisplayAs(".")]
		[FieldCanBeEntered]
		EmptyField = 0,

		[DisplayAs("#")]
		Wall = 1,

		[DisplayAs("O")]
		[FieldCanBeEntered]
		StartPoint = 2,

		[DisplayAs("G")]
		[FieldCanBeEntered]
		Goal = 3,

		//Teleport = <7, 99>
	}

	class DisplayAsAttribute : Attribute
	{
		public DisplayAsAttribute(string displayString) {
			DisplayString = displayString;
		}

		public string DisplayString { get; set; }
	}

	class FieldCanBeEnteredAttribute : Attribute { }

	static class MazeFieldExtensions
	{
		public static string ToDisplayString(this MazeField field)
		{
			var displayAsAtt = field.GetAttribute<DisplayAsAttribute>();
			return displayAsAtt != null ? displayAsAtt.DisplayString : field.ToString();
		}

		public static bool CanEnter(this MazeField field)
		{
			var canBeEnterdAttr = field.GetAttribute<FieldCanBeEnteredAttribute>();
			return canBeEnterdAttr != null;
		}

		public static T GetAttribute<T>(this Enum enumeration)
			where T : Attribute
		{
			return enumeration
				.GetType()
				.GetMember(enumeration.ToString())[0]
				.GetCustomAttributes(typeof(T), false)
				.Cast<T>()
				.SingleOrDefault();
		}	
	}
}
