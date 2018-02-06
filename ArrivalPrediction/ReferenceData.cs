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
		public static Dictionary<string, Line> AllLines;
		public static Dictionary<string, StopPoint> AllStopPoints;
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
					ReferenceData.AllStopPoints.Add((string)item[@"naptanId"], stop);
				}
			}
		}
		#endregion

		#region Methods
		public static Line TryFindLine(string id)
		{
			Line l = null;
			try
			{
				l = ReferenceData.AllLines[id];
			}
			catch (Exception)
			{
				Trace.TraceError(@"Could not find a line with key = '{0}'.", id);
			}
			return l;
		}

		public static StopPoint TryFindStopPoint(string id)
		{
			StopPoint s = null;
			try
			{
				s = ReferenceData.AllStopPoints[id];
			}
			catch (Exception)
			{
				Trace.TraceError(@"Could not find a stoppoint with key = '{0}'.", id);
			}
			return s;
		}
		#endregion
	}
}
