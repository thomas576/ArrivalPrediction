using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArrivalPrediction
{
	public class GaussianDistribution
	{
		#region Private fields
		private static readonly int NUMBER_OF_DATAPOINTS = 10000;
		private static readonly double ABSOLUTE_MIN_X_OR_MAX_X = 4.0;
		private static readonly Point2D[] _NormalDistributionArray = new Point2D[GaussianDistribution.NUMBER_OF_DATAPOINTS];
		#endregion

		#region Properties
		public double Mean { get; set; }
		public double StandardDeviation { get; set; }
		#endregion

		#region Constructors
		static GaussianDistribution()
		{
			double OneBySqrtTwoPi = 1.0 / (Math.Sqrt(2.0 * Math.PI));
			double xIncrement = GaussianDistribution.ABSOLUTE_MIN_X_OR_MAX_X * 2.0 / (GaussianDistribution.NUMBER_OF_DATAPOINTS - 1);
			for (int i = 0; i < GaussianDistribution.NUMBER_OF_DATAPOINTS; i++)
			{
				double x = -1.0 * GaussianDistribution.ABSOLUTE_MIN_X_OR_MAX_X + i * xIncrement;
				double y = OneBySqrtTwoPi * Math.Exp(-0.5 * x * x);
				GaussianDistribution._NormalDistributionArray[i] = new Point2D(x, y);
			}
		}

		public GaussianDistribution()
		{
			this.Mean = 0.0;
			this.StandardDeviation = 1.0;
		}

		public GaussianDistribution(double mean, double standardDeviation)
		{
			this.Mean = mean;
			this.StandardDeviation = standardDeviation;
		}
		#endregion

		#region Methods
		public double GetValue(double x)
		{
			double zeroMeanX = x - this.Mean;
			double OneByStandardDeviation = 1.0 / this.StandardDeviation;
			double scaledZeroMeanX = zeroMeanX * OneByStandardDeviation;
			if (Math.Abs(scaledZeroMeanX) <= GaussianDistribution.ABSOLUTE_MIN_X_OR_MAX_X)
			{
				double doubleIndex = ((scaledZeroMeanX - (-1.0 * GaussianDistribution.ABSOLUTE_MIN_X_OR_MAX_X)) / (2.0 * GaussianDistribution.ABSOLUTE_MIN_X_OR_MAX_X)) * (GaussianDistribution.NUMBER_OF_DATAPOINTS - 1);
				int indexBefore = (int)Math.Floor(doubleIndex);
				int indexAfter = (int)Math.Ceiling(doubleIndex);
				return OneByStandardDeviation * Point2D.LinearInterpolation(GaussianDistribution._NormalDistributionArray[indexBefore], GaussianDistribution._NormalDistributionArray[indexAfter], scaledZeroMeanX).Y;
			}
			else
			{
				return 0.0;
			}
		}

		public Point2D GetPoint2D(double x)
		{
			return new Point2D(x, this.GetValue(x));
		}
		#endregion
	}
}
