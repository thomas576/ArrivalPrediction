﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

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
			Line l = ReferenceData.TryFindLine(@"jubilee");
			IEnumerable<ArrivalPrediction> returnedList = new ArrivalPredictionDataMapper(tflConnectionSettings).GetAllArrivalPredictions();
		}
	}
}