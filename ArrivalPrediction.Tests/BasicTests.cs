using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading;
using System.Drawing;

namespace ArrivalPrediction.Tests
{
	[TestClass]
	public class BasicTests
	{
		[TestInitialize]
		public void TestInit()
		{
			foreach (TraceListener traceListener in Trace.Listeners)
			{
				TextWriterTraceListener txtWriterListener = traceListener as TextWriterTraceListener;
				if (txtWriterListener != null)
				{
					System.IO.StreamWriter streamWriter = txtWriterListener.Writer as System.IO.StreamWriter;
					if (streamWriter != null)
					{
						System.IO.FileStream fileStream = streamWriter.BaseStream as System.IO.FileStream;
						if (fileStream != null)
						{
							Process.Start(fileStream.Name);
						}
					}
				}
			}
		}

		[TestMethod]
		public void TestReferenceData()
		{
			Line jubilee;
			bool found = ReferenceData.TryFindLine(@"jubilee", out jubilee);
			Assert.IsTrue(found);
			Assert.IsTrue(jubilee.Name == @"Jubilee");
			StopPoint canaryWharf = jubilee.StopPoints.Where(s => s.Name.Contains(@"Canary")).First(); // Canary Wharf security guards are a bunch of twats btw
			Assert.IsTrue(canaryWharf.NextStopPoints.Values.Where(next => next.Name.Contains(@"Canada")).Count() > 0);
			Route routeJubileeToStratford;
			bool foundRoute = ReferenceData.TryFindRoute(@"jubilee", LineDirectionsEnum.JubileeStanmoreToStratford, out routeJubileeToStratford);
			Assert.IsTrue(foundRoute);
			StopPoint canadaWater = ReferenceData.AllStopPoints.Where(s => s.Value.Name.Contains(@"Canada Water")).First().Value;
			Assert.IsTrue(routeJubileeToStratford.CanGoFromStopToStop(canadaWater, canaryWharf));
		}

		[TestMethod]
		public void TestArrivalPredictionDownload()
		{
			TflConnectionSettings tflConnectionSettings = new TflConnectionSettings()
			{
				AppId = @"24f6effc",
				AppKey = @"b07dcb8449db2934ff23297f4004b0e1",
				HttpsBaseAddress = @"https://api.tfl.gov.uk/"
			};
			ICollection<ArrivalPrediction> returnedList = new ArrivalPredictionDataMapper(tflConnectionSettings).GetAllArrivalPredictions();

			Line jubilee;
			bool found = ReferenceData.TryFindLine(@"jubilee", out jubilee);
			IEnumerable<ArrivalPrediction> jubileePredictions = returnedList.Where(a => a.Line == jubilee);
			IList<StopPoint> stopsFromStanmore = ReferenceData.FindRoute(@"jubilee", LineDirectionsEnum.JubileeStanmoreToStratford).GetStopPointsInOrder();
			foreach (StopPoint stop in stopsFromStanmore)
			{
				IEnumerable<ArrivalPrediction> arrivalsForStop = jubileePredictions.Where(a => a.StopPoint == stop);
				Trace.TraceInformation(@"There are {0} predictions for {1}", arrivalsForStop.Count(), stop.Name);
			}
		}

		[TestMethod]
		public void TestPolling()
		{
			TflConnectionSettings tflConnectionSettings = new TflConnectionSettings()
			{
				AppId = @"24f6effc",
				AppKey = @"b07dcb8449db2934ff23297f4004b0e1",
				HttpsBaseAddress = @"https://api.tfl.gov.uk/"
			};
			ArrivalPredictionDataMapper arrivalPredictionDataMapper = new ArrivalPredictionDataMapper(tflConnectionSettings);
			ArrivalPredictionPolling polling = new ArrivalPredictionPolling(arrivalPredictionDataMapper, 55000, 10000);
			polling.StartPollingInNewThread();
			Thread.Sleep(240000);
			polling.StopPollingThread();
			Thread.Sleep(55000);
			IDictionary<DateTime, Image> imageDictionary = polling.ConvertToImagesThreadSafe(
				ReferenceData.FindRoute(@"jubilee", LineDirectionsEnum.JubileeStanmoreToStratford),
				new DateTime(1900, 1, 1),
				2,
				20,
				10,
				5,
				0.3);
			foreach (var keyvalue in imageDictionary)
			{
				keyvalue.Value.Save(string.Format(@"C:\Users\Administrator\Desktop\bitmap_export\bmp_{0:yyyyMMdd_HHmmss}.bmp", keyvalue.Key));
			}
		}
	}
}
