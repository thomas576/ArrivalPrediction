using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrivalPrediction
{
	public class StopPointOrder : IEquatable<StopPointOrder>
	{
		#region Private fields

		#endregion

		#region Properties
		public StopPoint StopPoint { get; set; }
		public Line Line { get; set; }
		public LineDirectionsEnum LineDirection { get; set; }
		public int ZeroBasedOrder { get; set; }
		#endregion

		#region Constructors

		#endregion

		#region Methods
		public bool Equals(StopPointOrder other)
		{
			if (other == null)
			{
				return false;
			}
			else
			{
				
			}
		}
		#endregion
	}
}
