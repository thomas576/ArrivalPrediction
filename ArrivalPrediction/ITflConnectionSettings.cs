using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrivalPrediction
{
	public interface ITflConnectionSettings
	{
		string AppId { get; }
		string AppKey { get; }
		string HttpsBaseAddress { get; }
	}
}
