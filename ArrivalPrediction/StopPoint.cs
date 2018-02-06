using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrivalPrediction
{
	public class StopPoint : IReferenceData
	{
		#region Private fields
		private readonly IList<Line> _Lines = new List<Line>();
		#endregion

		#region Properties
		public string Id { get; set; }
		public string Name { get; set; }
		public IList<Line> Lines
		{
			get
			{
				return this._Lines;
			}
		}
		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion
	}
}
