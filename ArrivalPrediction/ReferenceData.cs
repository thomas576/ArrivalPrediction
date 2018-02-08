using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace ArrivalPrediction
{
	public static class ReferenceData
	{
		#region Private fields
		#endregion

		#region Properties
		public static Dictionary<string, Line> AllLines { get; set; }
		public static Dictionary<string, StopPoint> AllStopPoints { get; set; }
		public static HashSet<StopPointOrder> AllStopPointOrders { get; set; }
		public static HashSet<Route> AllRoutes { get; set; }
		#endregion

		#region Constructors
		static ReferenceData()
		{
			string assemblyPath = Assembly.GetExecutingAssembly().Location;
			string assemblyDirectory = Path.GetDirectoryName(assemblyPath);

			ReferenceData.AllLines = new Dictionary<string, Line>();
			string lines = File.ReadAllText(Path.Combine(assemblyDirectory, @"Line.json.txt"), Encoding.UTF8);
			JArray arrayLines = JArray.Parse(lines);
			foreach (JToken item in arrayLines)
			{
				ReferenceData.AllLines.Add((string)item[@"id"], new Line() { Id = (string)item[@"id"], Name = (string)item[@"name"] });
			}

			ReferenceData.AllStopPoints = new Dictionary<string, StopPoint>();
			string stopPoints = File.ReadAllText(Path.Combine(assemblyDirectory, @"StopPoint.json.txt"), Encoding.UTF8);
			JArray arrayStopPoints = JArray.Parse(stopPoints);
			foreach (JToken item in arrayStopPoints)
			{
				if (((JArray)item[@"modes"]).Where(j => (string)j == @"tube").Count() > 0)
				{
					StopPoint stop = new StopPoint();
					stop.Id = (string)item[@"naptanId"];
					stop.Name = (string)item[@"commonName"];
					foreach (JToken lineItem in item[@"lines"])
					{
						Line line;
						if (ReferenceData.AllLines.ContainsKey((string)lineItem[@"id"]) && ReferenceData.TryFindLine((string)lineItem[@"id"], out line))
						{
							stop.Lines.Add(line);
							line.StopPoints.Add(stop);
						}
					}
					ReferenceData.AllStopPoints.Add((string)item[@"naptanId"], stop);
				}
			}

			ReferenceData.AllRoutes = new HashSet<Route>();
			ReferenceData.AllRoutes.Add(new Route() { Line = ReferenceData.FindLine(@"jubilee"), LineDirection = LineDirectionsEnum.JubileeStanmoreToStratford });
			ReferenceData.AllRoutes.Add(new Route() { Line = ReferenceData.FindLine(@"jubilee"), LineDirection = LineDirectionsEnum.JubileeStratfordToStanmore });

			ReferenceData.AllStopPointOrders = new HashSet<StopPointOrder>();
			List<StopPoint> listOfStops = new List<StopPoint>();
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Stanmore Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Canons Park Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Queensbury Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Kingsbury Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Wembley Park Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Neasden Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Dollis Hill Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Willesden Green Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Kilburn Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"West Hampstead Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Finchley Road Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Swiss Cottage Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"St. John's Wood Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Baker Street Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Bond Street Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Green Park Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Westminster Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Waterloo Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Southwark Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"London Bridge Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Bermondsey Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Canada Water Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Canary Wharf Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"North Greenwich Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Canning Town Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"West Ham Underground Station").First().Value);
			listOfStops.Add(ReferenceData.AllStopPoints.Where(keyvalue => keyvalue.Value != null && keyvalue.Value.Name == @"Stratford Underground Station").First().Value);
			for (int i = 0; i < listOfStops.Count; i++)
			{
				ReferenceData.AllStopPointOrders.Add(new StopPointOrder() {
					StopPoint = listOfStops[i],
					Route = ReferenceData.FindRoute(@"jubilee", LineDirectionsEnum.JubileeStanmoreToStratford),
					ZeroBasedOrder = i
				});
			}
			for (int i = listOfStops.Count - 1, j = 0; i >= 0; i--, j++)
			{
				ReferenceData.AllStopPointOrders.Add(new StopPointOrder()
				{
					StopPoint = listOfStops[i],
					Route = ReferenceData.FindRoute(@"jubilee", LineDirectionsEnum.JubileeStratfordToStanmore),
					ZeroBasedOrder = j
				});
			}

			foreach (StopPoint stop in ReferenceData.AllStopPoints.Values)
			{
				IEnumerable<StopPointOrder> ordersForStopPoint = ReferenceData.AllStopPointOrders.Where(o => o.StopPoint == stop);
				foreach (StopPointOrder order in ordersForStopPoint)
				{
					IEnumerable<StopPointOrder> allNextOrders = ReferenceData.AllStopPointOrders.Where(o => o.Route == order.Route && o.ZeroBasedOrder == order.ZeroBasedOrder + 1);
					IEnumerable<StopPointOrder> allPreviousOrders = ReferenceData.AllStopPointOrders.Where(o => o.Route == order.Route && o.ZeroBasedOrder == order.ZeroBasedOrder - 1);
					foreach (StopPointOrder nextOrder in allNextOrders)
					{
						stop.NextStopPoints.Add(nextOrder.Route, nextOrder.StopPoint);
					}
					foreach (StopPointOrder previousOrder in allPreviousOrders)
					{
						stop.PreviousStopPoints.Add(previousOrder.Route, previousOrder.StopPoint);
					}
				}
			}
		}
		#endregion

		#region Methods
		public static bool TryFindLine(string id, out Line line)
		{
			try
			{
				line = ReferenceData.AllLines[id];
				return true;
			}
			catch (Exception)
			{
				line = null;
				Trace.TraceError(@"Could not find a line with key = '{0}'.", id);
			}
			return false;
		}

		public static Line FindLine(string id)
		{
			return ReferenceData.AllLines[id];
		}

		public static bool TryFindStopPoint(string id, out StopPoint stop)
		{
			try
			{
				stop = ReferenceData.AllStopPoints[id];
				return true;
			}
			catch (Exception)
			{
				stop = null;
				Trace.TraceError(@"Could not find a stoppoint with key = '{0}'.", id);
			}
			return false;
		}

		public static StopPoint FindStopPoint(string id)
		{
			return ReferenceData.AllStopPoints[id];
		}

		public static bool TryFindRoute(string lineId, LineDirectionsEnum lineDirection, out Route route)
		{
			try
			{
				route = ReferenceData.AllRoutes.Where(r => r.Line == ReferenceData.FindLine(lineId) && r.LineDirection == lineDirection).First();
				return true;
			}
			catch (Exception)
			{
				route = null;
				Trace.TraceError(@"Could not find a route with lineId = '{0}' and lineDirection = {1}.", lineId, lineDirection);
			}
			return false;
		}

		public static Route FindRoute(string lineId, LineDirectionsEnum lineDirection)
		{
			return ReferenceData.AllRoutes.Where(r => r.Line == ReferenceData.FindLine(lineId) && r.LineDirection == lineDirection).First();
		}
		#endregion
	}
}
