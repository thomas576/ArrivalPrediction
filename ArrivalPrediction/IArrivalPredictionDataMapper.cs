using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrivalPrediction
{
	public interface IArrivalPredictionDataMapper
	{
		IEnumerable<ArrivalPrediction> GetAllArrivalPredictions();
	}
}
