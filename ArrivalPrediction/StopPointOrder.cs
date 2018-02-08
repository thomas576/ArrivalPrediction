using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArrivalPrediction
{
	[DebuggerDisplay(@"StopPoint={StopPoint.Name}, Route=(Line={Route.Line.Name}, LineDirection={Route.LineDirection}), ZeroBasedOrder={ZeroBasedOrder}")]
	public class StopPointOrder : IEquatable<StopPointOrder>
	{
		#region Private fields

		#endregion

		#region Properties
		public StopPoint StopPoint { get; set; }
		public Route Route { get; set; }
		public int ZeroBasedOrder { get; set; }
		#endregion

		#region Constructors

		#endregion

		#region Methods
		public static bool operator ==(StopPointOrder obj1, StopPointOrder obj2)
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

		public static bool operator !=(StopPointOrder obj1, StopPointOrder obj2)
		{
			return !(obj1 == obj2);
		}

		public bool Equals(StopPointOrder other)
		{
			return (other != null 
					&& this.ZeroBasedOrder == other.ZeroBasedOrder 
					&& this.Route == other.Route
					&& this.StopPoint == other.StopPoint);
		}

		public override bool Equals(object obj)
		{
			return (obj == null) ? false : this.Equals(obj as StopPointOrder);
		}

		public override int GetHashCode()
		{
			return ((this.StopPoint == null) ? 0 : this.StopPoint.GetHashCode())
				+ ((this.Route == null) ? 0 : this.Route.GetHashCode())
				+ this.ZeroBasedOrder;
		}
		#endregion
	}
}
