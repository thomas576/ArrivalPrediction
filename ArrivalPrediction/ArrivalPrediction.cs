using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArrivalPrediction
{
	[DebuggerDisplay(@"Id={Id}, VehicleId={VehicleId}, StopPoint={StopPoint.Name}, Line={Line.Name}, TimeToStation={TimeToStation}, DestinationStopPoint={DestinationStopPoint.Name}, Routes=({Routes.Count} items), TimeStamp={TimeStamp}")]
	public class ArrivalPrediction : IEquatable<ArrivalPrediction>
	{
		#region Private fields
		private readonly ICollection<Route> _Routes = new HashSet<Route>();
		#endregion

		#region Properties
		public string Id { get; set; }
		public string VehicleId { get; set; }
		public StopPoint StopPoint { get; set; }
		public Line Line { get; set; }
		public DateTime TimeStamp { get; set; }
		public int TimeToStation { get; set; }
		public StopPoint DestinationStopPoint { get; set; }
		public ICollection<Route> Routes
		{
			get
			{
				return this._Routes;
			}
		}
		#endregion

		#region Constructors

		#endregion

		#region Methods
		public static bool operator ==(ArrivalPrediction obj1, ArrivalPrediction obj2)
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

		public static bool operator !=(ArrivalPrediction obj1, ArrivalPrediction obj2)
		{
			return !(obj1 == obj2);
		}

		public bool Equals(ArrivalPrediction other)
		{
			return (other != null
					&& this.TimeStamp == other.TimeStamp
					&& this.TimeToStation == other.TimeToStation
					&& this.VehicleId == other.VehicleId
					&& this.StopPoint == other.StopPoint
					&& this.Line == other.Line
					&& this.DestinationStopPoint == other.DestinationStopPoint);
		}

		public override bool Equals(object obj)
		{
			return (obj == null) ? false : this.Equals(obj as ArrivalPrediction);
		}

		public override int GetHashCode()
		{
			return this.TimeStamp.GetHashCode()
				+ this.TimeToStation
				+ ((this.VehicleId == null) ? 0 : this.VehicleId.GetHashCode())
				+ ((this.StopPoint == null) ? 0 : this.StopPoint.GetHashCode())
				+ ((this.Line == null) ? 0 : this.Line.GetHashCode())
				+ ((this.DestinationStopPoint == null) ? 0 : this.DestinationStopPoint.GetHashCode());
		}
		#endregion
	}
}
