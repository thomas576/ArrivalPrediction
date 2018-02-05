using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArrivalPrediction.Tests
{
	[TestClass]
	public class BasicTests
	{
		[TestMethod]
		public void TestArrivalPredictionDownload()
		{
			TflConnectionSettings tflConnectionSettings = new TflConnectionSettings() {
				AppId = @"24f6effc",
				AppKey = @"b07dcb8449db2934ff23297f4004b0e1",
				HttpsBaseAddress = @"https://api.tfl.gov.uk/"
			};
			ReferenceData.TryFindLine(@"jubilee");
			//IEnumerable<ArrivalPrediction> returnedList = new ArrivalPredictionDataMapper(tflConnectionSettings).GetAllArrivalPredictions();
		}
	}
}
