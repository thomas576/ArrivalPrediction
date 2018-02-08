using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArrivalPrediction
{
	[DebuggerDisplay(@"Id={Id}, Name={Name}, Lines=({Lines.Count} items), NextStopPoints=({NextStopPoints.Count} items), PreviousStopPoints=({PreviousStopPoints.Count} items)")]
	public class StopPoint : AReferenceData
	{
		#region Private fields
		private readonly ICollection<Line> _Lines = new HashSet<Line>();
		private readonly IDictionary<Route, StopPoint> _NextStopPoints = new Dictionary<Route, StopPoint>();
		private readonly IDictionary<Route, StopPoint> _PreviousStopPoints = new Dictionary<Route, StopPoint>();
		#endregion

		#region Properties
		public string Name { get; set; }
		public ICollection<Line> Lines
		{
			get
			{
				return this._Lines;
			}
		}
		public IDictionary<Route, StopPoint> NextStopPoints
		{
			get
			{
				return this._NextStopPoints;
			}
		}
		public IDictionary<Route, StopPoint> PreviousStopPoints
		{
			get
			{
				return this._PreviousStopPoints;
			}
		}
		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion
	}
}
