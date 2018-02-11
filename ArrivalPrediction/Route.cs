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
			if (stop1 == stop2)
			{
				Trace.TraceError(@"Route cannot determine if it can go from stop1 to stop2 if both are the same and equal to {0}", stop1.Name);
			}
			else
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
			}
			return false;
		}

		public StopPoint GetTerminatingStop()
		{
			StopPoint terminatingStop = null;
			IEnumerable<StopPointOrder> stopOrdersOnRoute = ReferenceData.AllStopPointOrders.Where(o => o.Route == this);
			int countStopOrdersOnroute = stopOrdersOnRoute.Count();
			if (countStopOrdersOnroute > 0)
			{
				terminatingStop = stopOrdersOnRoute.Where(o => o.ZeroBasedOrder == (countStopOrdersOnroute - 1)).First().StopPoint;
			}
			else
			{
				Trace.TraceError(@"No stops defined on route ({0}, {1})", (this.Line == null) ? @"null" : this.Line.Name, this.LineDirection);
			}
			return terminatingStop;
		}

		public IList<StopPoint> GetStopPointsInOrder()
		{
			IList<StopPoint> stopPointsInOrder = new List<StopPoint>();
			IEnumerable<StopPointOrder> stopOrdersOnRoute = ReferenceData.AllStopPointOrders.Where(o => o.Route == this).OrderBy(s => s.ZeroBasedOrder);
			foreach (StopPointOrder order in stopOrdersOnRoute)
			{
				stopPointsInOrder.Add(order.StopPoint);
			}
			return stopPointsInOrder;
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

