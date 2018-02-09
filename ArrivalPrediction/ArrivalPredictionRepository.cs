using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArrivalPrediction
{
	[DebuggerDisplay(@"ArrivalPredictionsByTimeStamp=({ArrivalPredictionsByTimeStamp.Count} items)")]
	public class ArrivalPredictionRepository
	{
		#region Private fields
		private readonly IDictionary<DateTime, ICollection<ArrivalPrediction>> _ArrivalPredictionsByTimeStamp = new Dictionary<DateTime, ICollection<ArrivalPrediction>>();
		#endregion

		#region Properties
		public IDictionary<DateTime, ICollection<ArrivalPrediction>> ArrivalPredictionsByTimeStamp
		{
			get
			{
				return this._ArrivalPredictionsByTimeStamp;
			}
		}
		#endregion

		#region Constructors

		#endregion

		#region Methods
		public void AddArrivalPredictions<T>(T arrivalPredictionCollection) where T : ICollection<ArrivalPrediction>, new()
		{
			foreach (ArrivalPrediction arrival in arrivalPredictionCollection)
			{
				if (this.ArrivalPredictionsByTimeStamp.ContainsKey(arrival.TimeStamp))
				{
					ICollection<ArrivalPrediction> dictionaryEntry = this.ArrivalPredictionsByTimeStamp[arrival.TimeStamp];
					if (dictionaryEntry != null)
					{
						dictionaryEntry.Add(arrival);
					}
					else
					{
						T newCollection = new T();
						newCollection.Add(arrival);
						this.ArrivalPredictionsByTimeStamp[arrival.TimeStamp] = newCollection;
					}
				}
				else
				{
					T newCollection = new T();
					newCollection.Add(arrival);
					this.ArrivalPredictionsByTimeStamp.Add(arrival.TimeStamp, newCollection);
				}
			}
		}
		#endregion
	}
}
