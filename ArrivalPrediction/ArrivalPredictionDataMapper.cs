using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net;

namespace ArrivalPrediction
{
	public class ArrivalPredictionDataMapper : IArrivalPredictionDataMapper
	{
		#region Private fields
		private readonly ITflConnectionSettings _TflConnectionSettings;
		#endregion

		#region Properties

		#endregion

		#region Constructors
		public ArrivalPredictionDataMapper(ITflConnectionSettings tflConnectionSettings)
		{
			this._TflConnectionSettings = tflConnectionSettings;
		}
		#endregion

		#region Methods
		public IEnumerable<ArrivalPrediction> GetAllArrivalPredictions()
		{
			string uri = string.Format(@"/Mode/tube/Arrivals?count=-1&app_id={0}&app_key={1}",
				this._TflConnectionSettings.AppId,
				this._TflConnectionSettings.AppKey);

			string allArrivalPredictionText = HttpHelper.GetHttpsString(this._TflConnectionSettings.HttpsBaseAddress, uri);
			
			JArray arrivalPredictionArray = JArray.Parse(allArrivalPredictionText);
			List<ArrivalPrediction> arrivalPredictionList = new List<ArrivalPrediction>(arrivalPredictionArray.Count);
			foreach (JToken item in arrivalPredictionArray)
			{

			}

			return arrivalPredictionList;
		}
		#endregion
	}
}
