using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrivalPrediction
{
	public abstract class AReferenceData : IReferenceData, IEquatable<AReferenceData>
	{
		#region Properties
		public virtual string Id { get; set; }
		#endregion

		#region Methods
		public bool Equals(AReferenceData other)
		{
			return (other == null) ? false : this.Id == other.Id;
		}

		public override bool Equals(object obj)
		{
			return (obj == null) ? false : (obj is AReferenceData) ? this.Equals(obj as AReferenceData) : false;
		}

		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}
		#endregion
	}
}
