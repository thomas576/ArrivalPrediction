using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArrivalPrediction
{
	[DebuggerDisplay(@"Id={Id}, Name={Name}, StopPoints=({StopPoints.Count} items)")]
	public class Line : AReferenceData
	{
		#region Private fields
		private readonly ICollection<StopPoint> _StopPoints = new HashSet<StopPoint>();
		#endregion

		#region Properties
		public string Name { get; set; }
		public ICollection<StopPoint> StopPoints
		{
			get
			{
				return this._StopPoints;
			}
		}
		#endregion

		#region Constructors

		#endregion

		#region Methods
		#endregion
	}

	public enum LineDirectionsEnum : int
	{
		JubileeStanmoreToStratford,
		JubileeStratfordToStanmore
	}
}
