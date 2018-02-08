using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArrivalPrediction;
using System.Diagnostics;

namespace ArrivalPrediction.Tests
{
	/// <summary>
	/// Summary description for GaussianMathTests
	/// </summary>
	[TestClass]
	public class GaussianMathTests
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
		public void TestPoint2D()
		{
			Point2D p1 = new Point2D(0.0, 0.0);
			Point2D p2 = new Point2D(1.0, 1.0);
			Assert.AreEqual(new Point2D(0.5, 0.5), Point2D.LinearInterpolation(p1, p2, 0.5));
		}

		[TestMethod]
		public void TestStandardGaussianValues()
		{
			GaussianDistribution standardGauss = new GaussianDistribution();
			Assert.IsTrue(Math.Abs(standardGauss.GetValue(0.0) - 0.398942) < 0.001);
			Assert.IsTrue(Math.Abs(standardGauss.GetValue(1.0) - 0.241971) < 0.001);
			Assert.IsTrue(Math.Abs(standardGauss.GetValue(-1.0) - 0.241971) < 0.001);
			for (int i = -9; i < 10; i++)
			{
				//Trace.TraceInformation(standardGauss.GetPoint2D(i * 0.2).ToString());
			}

			GaussianDistribution otherGauss = new GaussianDistribution(-1.5, 4.0);
			for (int i = -9; i < 10; i++)
			{
				//Trace.TraceInformation(otherGauss.GetPoint2D(-1.5 + i * 0.2).ToString());
			}
			Assert.IsTrue(Math.Abs(otherGauss.GetValue(0.0) - 0.0929638) < 0.001);
			Assert.IsTrue(Math.Abs(otherGauss.GetValue(1.0) - 0.0820402) < 0.001);
			Assert.IsTrue(Math.Abs(otherGauss.GetValue(-1.0) - 0.0989594) < 0.001);

			otherGauss = new GaussianDistribution(2.2, 0.6);
			for (int i = -9; i < 10; i++)
			{
				//Trace.TraceInformation(otherGauss.GetPoint2D(-1.5 + i * 0.2).ToString());
			}
			Assert.IsTrue(Math.Abs(otherGauss.GetValue(0.0) - 0.000800451) < 0.001);
			Assert.IsTrue(Math.Abs(otherGauss.GetValue(1.0) - 0.0899849) < 0.001);
			Assert.IsTrue(Math.Abs(otherGauss.GetValue(2.0) - 0.628972) < 0.001);
		}
	}
}
