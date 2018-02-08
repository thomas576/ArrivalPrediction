using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArrivalPrediction
{
	[DebuggerDisplay(@"Id={Id}, VehicleId={VehicleId}, StopPoint={StopPoint.Name}, Line={Line.Name}, TimeToStation={TimeToStation}, DestinationStopPoint={DestinationStopPoint.Name}, TimeStamp={TimeStamp}")]
	public class ArrivalPrediction
	{
		#region Private fields
		#endregion

		#region Properties
		public string Id { get; set; }
		public string VehicleId { get; set; }
		public StopPoint StopPoint { get; set; }
		public Line Line { get; set; }
		public DateTime TimeStamp { get; set; }
		public int TimeToStation { get; set; }
		public StopPoint DestinationStopPoint { get; set; }
		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion
	}
}
