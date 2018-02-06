using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArrivalPrediction
{
	[DebuggerDisplay(@"Id={Id}, VehicleId={VehicleId}, StopPointId={StopPointId}, LineId={LineId}, TimeStamp={TimeStamp}, TimeToStation={TimeToStation}")]
	public class ArrivalPrediction
	{
		#region Private fields

		#endregion

		#region Properties
		public string Id { get; set; }
		public string VehicleId { get; set; }
		public string StopPointId { get; set; }
		public string LineId { get; set; }
		public DateTime TimeStamp { get; set; }
		public int TimeToStation { get; set; }
		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion
	}
}
