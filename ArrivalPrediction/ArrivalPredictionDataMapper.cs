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
		public ICollection<ArrivalPrediction> GetAllArrivalPredictions()
		{
			//string uri = string.Format(@"/Mode/tube/Arrivals?count=8&app_id={0}&app_key={1}",
			//	this._TflConnectionSettings.AppId,
			//	this._TflConnectionSettings.AppKey);
			string uri = @"/line/jubilee/arrivals";

			string allArrivalPredictionText = HttpHelper.GetHttpsString(this._TflConnectionSettings.HttpsBaseAddress, uri);
			
			JArray arrivalPredictionArray = JArray.Parse(allArrivalPredictionText);
			HashSet<ArrivalPrediction> arrivalPredictionList = new HashSet<ArrivalPrediction>();
			foreach (JToken item in arrivalPredictionArray)
			{
				ArrivalPrediction arrivalPrediction = new ArrivalPrediction();
				arrivalPrediction.Id = (string)item[@"id"];
				arrivalPrediction.VehicleId = (string)item[@"vehicleId"];
				string stopPointId = (string)item[@"naptanId"];
				string lineId = (string)item[@"lineId"];
				arrivalPrediction.TimeStamp = (DateTime)item[@"timestamp"];
				arrivalPrediction.TimeToStation = (int)(item[@"timeToStation"] ?? 0);
				string destinationStopPointId = (string)item[@"destinationNaptanId"];
				Line line;
				StopPoint stop;
				StopPoint destinationStop;
				if (stopPointId != null && ReferenceData.TryFindStopPoint(stopPointId, out stop) && lineId != null && ReferenceData.TryFindLine(lineId, out line))
				{
					arrivalPrediction.StopPoint = stop;
					arrivalPrediction.Line = line;
					if (destinationStopPointId != null && ReferenceData.TryFindStopPoint(destinationStopPointId, out destinationStop))
					{
						arrivalPrediction.DestinationStopPoint = destinationStop;
						foreach (Route candidateRoute in ReferenceData.AllRoutes.Where(r => r.Line == line))
						{
							// Note that here we won't be able to find the route if we have no destinationStop or if the destinationStop is the same as the stop
							// but that stop is not the terminating point on that route.
							if ((stop != destinationStop && candidateRoute.CanGoFromStopToStop(stop, destinationStop))
								|| (stop == destinationStop && destinationStop == candidateRoute.GetTerminatingStop()))
							{
								arrivalPrediction.Routes.Add(candidateRoute);
							}
						}
					}
					arrivalPredictionList.Add(arrivalPrediction);
				}
				else
				{
					Trace.TraceInformation(@"Could not read:{0}{1}", Environment.NewLine, item);
				}
			}

			Trace.TraceInformation(@"Retrieved {0} arrival predictions from a total of {1} JSON entries.", arrivalPredictionList.Count, arrivalPredictionArray.Count);
			return arrivalPredictionList;
		}
		#endregion
	}
}
