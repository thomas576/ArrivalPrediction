using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArrivalPrediction
{
	[DebuggerDisplay(@"Line={Line.Name}, LineDirection={LineDirection}")]
	public class Route : IEquatable<Route>
	{
		#region Private fields
		#endregion

		#region Properties
		public Line Line { get; set; }
		public LineDirectionsEnum LineDirection { get; set; }
		#endregion

		#region Constructors
		#endregion

		#region Methods
		public static bool operator ==(Route obj1, Route obj2)
		{
			if (ReferenceEquals(obj1, obj2))
			{
				return true;
			}
			if (ReferenceEquals(obj1, null))
			{
				return false;
			}
			if (ReferenceEquals(obj2, null))
			{
				return false;
			}
			return obj1.Equals(obj2);
		}

		public static bool operator !=(Route obj1, Route obj2)
		{
			return !(obj1 == obj2);
		}

		public bool Equals(Route other)
		{
			return (other != null
					&& this.LineDirection == other.LineDirection
					&& this.Line == other.Line);
		}

		public override bool Equals(object obj)
		{
			return (obj == null) ? false : this.Equals(obj as Route);
		}

		public override int GetHashCode()
		{
			return ((this.Line == null) ? 0 : this.Line.GetHashCode()) + this.LineDirection.GetHashCode();
		}
		#endregion
	}
}

