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
		public bool CanGoFromStopToStop(StopPoint stop1, StopPoint stop2)
		{
			IEnumerable<StopPointOrder> stopOrdersOnRoute = ReferenceData.AllStopPointOrders.Where(o => o.Route == this);
			if (stopOrdersOnRoute.Count() > 0)
			{
				StopPointOrder stop1OrderFound = stopOrdersOnRoute.Where(o => o.StopPoint == stop1).FirstOrDefault();
				StopPointOrder stop2OrderFound = stopOrdersOnRoute.Where(o => o.StopPoint == stop2).FirstOrDefault();
				if (stop1OrderFound != null && stop2OrderFound != null && stop1OrderFound.ZeroBasedOrder < stop2OrderFound.ZeroBasedOrder)
				{
					return true;
				}
			}
			return false;
		}

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

