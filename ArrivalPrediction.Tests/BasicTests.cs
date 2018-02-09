using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading;

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
		public void TestArrivalPredictionDownload()
		{
			TflConnectionSettings tflConnectionSettings = new TflConnectionSettings()
			{
				AppId = @"24f6effc",
				AppKey = @"b07dcb8449db2934ff23297f4004b0e1",
				HttpsBaseAddress = @"https://api.tfl.gov.uk/"
			};
			Line jubilee;
			bool found = ReferenceData.TryFindLine(@"jubilee", out jubilee);
			foreach (StopPoint s in jubilee.StopPoints)
			{
				Debug.WriteLine(s.Name);
			}
			//IEnumerable<ArrivalPrediction> returnedList = new ArrivalPredictionDataMapper(tflConnectionSettings).GetAllArrivalPredictions();
			//IEnumerable<ArrivalPrediction> jubileePredictions = returnedList.Where(ap => ap.LineId == @"jubilee").OrderBy(ap => ap.TimeToStation);
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
			ArrivalPredictionPolling polling = new ArrivalPredictionPolling(tflConnectionSettings, 55000, 10000);
			polling.StartPollingInNewThread();
			Thread.Sleep(240000);
			polling.StopPollingThread();
			Thread.Sleep(55000);
			ArrivalPredictionRepository repo = polling.ArrivalPredictionRepository;
		}
	}
}
