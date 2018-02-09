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
		public static bool operator ==(AReferenceData obj1, AReferenceData obj2)
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

		public static bool operator !=(AReferenceData obj1, AReferenceData obj2)
		{
			return !(obj1 == obj2);
		}

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
			return (this.Id == null) ? 0 : this.Id.GetHashCode();
		}
		#endregion
	}
}
