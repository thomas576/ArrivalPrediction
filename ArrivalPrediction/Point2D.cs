using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrivalPrediction
{
	public struct Point2D
	{
		#region Private fields

		#endregion

		#region Properties
		public double X;
		public double Y;
		#endregion

		#region Constructors
		public Point2D(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}
		#endregion

		#region Methods
		public static Point2D LinearInterpolation(Point2D point1, Point2D point2, double x)
		{
			if (point1.X == point2.X)
			{
				if (point1.Y == point2.Y)
				{
					return point1;
				}
				else
				{
					throw new ArgumentException(@"point1 and point2 shared the same x-coordinate but not the same y-coordinates");
				}
			}
			else
			{
				return new Point2D(x, point1.Y + ((x - point1.X)/(point2.X - point1.X)) * (point2.Y - point1.Y));
			}
		}
		#endregion
	}
}
