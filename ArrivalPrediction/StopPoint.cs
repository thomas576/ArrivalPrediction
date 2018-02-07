using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArrivalPrediction
{
	[DebuggerDisplay(@"Id={Id}, Name={Name}, Lines=({Lines.Count} items)")]
	public class StopPoint : AReferenceData
	{
		#region Private fields
		private readonly ICollection<Line> _Lines = new HashSet<Line>();
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
		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion
	}
}
