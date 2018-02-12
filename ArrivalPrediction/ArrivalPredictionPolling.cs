using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace ArrivalPrediction
{
	public class ArrivalPredictionPolling
	{
		#region Private fields
		private object _Lock = new object();
		private readonly IArrivalPredictionDataMapper _ArrivalPredictionDataMapper;
		private readonly int _PeriodInMilliseconds;
		private readonly int _MinimumGapInMilliseconds;
		private volatile bool _IsPollingActive;
		#endregion

		#region Properties
		public ArrivalPredictionRepository ArrivalPredictionRepository { get; set; }
		public int PeriodInMilliseconds
		{
			get
			{
				return this._PeriodInMilliseconds;
			}
		}
		public int MinimumGapInMilliseconds
		{
			get
			{
				return this._MinimumGapInMilliseconds;
			}
		}
		#endregion

		#region Constructors
		public ArrivalPredictionPolling(IArrivalPredictionDataMapper arrivalPredictionDataMapper, int periodInMilliseconds, int minimumGapInMilliseconds)
		{
			this.ArrivalPredictionRepository = new ArrivalPredictionRepository();
			this._ArrivalPredictionDataMapper = arrivalPredictionDataMapper;
			this._PeriodInMilliseconds = periodInMilliseconds;
			this._MinimumGapInMilliseconds = minimumGapInMilliseconds;
		}
		#endregion

		#region Methods
		public void StartPollingInNewThread()
		{
			this._IsPollingActive = true;
			Task.Run(() => this.PollThreadSafe());
		}

		public void StopPollingThread()
		{
			this._IsPollingActive = false;
		}

		protected virtual void PollThreadSafe()
		{
			while (this._IsPollingActive)
			{
				Stopwatch watch = new Stopwatch();
				watch.Start();

				lock (this._Lock)
				{
					Trace.TraceInformation(@"PollThreadSafe starting...");
					HashSet<ArrivalPrediction> returnedArrivalPredictions = (HashSet<ArrivalPrediction>)this._ArrivalPredictionDataMapper.GetAllArrivalPredictions();
					this.ArrivalPredictionRepository.AddArrivalPredictions(returnedArrivalPredictions);
				}

				watch.Stop();
				int elapsedTimeMilliseconds = (int)watch.ElapsedMilliseconds;
				if (elapsedTimeMilliseconds > this._PeriodInMilliseconds - this._MinimumGapInMilliseconds)
				{
					Trace.TraceWarning(@"PollThreadSafe had an execution that lasted longer than the set period minus the min gap, it lasted {0:#.###}s", elapsedTimeMilliseconds / 1000.0);
					Trace.TraceInformation(@"Forcing a {0:#.###}s wait in order to allow other threads to read data...", this._MinimumGapInMilliseconds / 1000.0);
					Thread.Sleep(this._MinimumGapInMilliseconds);
					Trace.TraceInformation(@"{0:#.###}s wait is over, continuing...", this._MinimumGapInMilliseconds / 1000.0);
				}
				else
				{
					Trace.TraceInformation(@"PollThreadSafe completed, it took {0:#.###}s", elapsedTimeMilliseconds / 1000.0);
					Trace.TraceInformation(@"Waiting {0:#.###}s...", (this._PeriodInMilliseconds - elapsedTimeMilliseconds) / 1000.0);
					Thread.Sleep(this._PeriodInMilliseconds - elapsedTimeMilliseconds);
					Trace.TraceInformation(@"{0:#.###}s wait is over, continuing...", (this._PeriodInMilliseconds - elapsedTimeMilliseconds) / 1000.0);
				}
			}
		}

		public IDictionary<DateTime, Image> ConvertToImagesThreadSafe(Route route, DateTime since, int minutesBetweenStops, int pixelsPerMinute, int pixelsStopHeight, int minutesBeforeFirstStop, double gaussianStandardDeviation)
		{
			IDictionary<DateTime, Image> imageDictionary = new Dictionary<DateTime, Image>();

			lock (this._Lock)
			{
				IList<StopPoint> stopPointsInOrder = route.GetStopPointsInOrder();
				int imageWidth = (stopPointsInOrder.Count - 1) * minutesBetweenStops * pixelsPerMinute + minutesBeforeFirstStop * pixelsPerMinute;
				int imageHeight = stopPointsInOrder.Count * pixelsStopHeight;
				Trace.TraceInformation(@"Converting to an image, all images will be {0}x{1} pixels.", imageWidth, imageHeight);

				foreach (var keyValue in this.ArrivalPredictionRepository.ArrivalPredictionsByTimeStamp)
				{
					if (keyValue.Key >= since)
					{
						DateTime currentImageDateTime = keyValue.Key;
						Trace.TraceInformation(@"Converting to an image for timestamp = {0}.", currentImageDateTime);
						ICollection<ArrivalPrediction> currentArrivalPredictions = keyValue.Value;
						IEnumerable<ArrivalPrediction> arrivalPredictionsForRoute = currentArrivalPredictions.Where(a => (a.Routes.Contains(route)));
						Trace.TraceInformation(@"Converting to an image, route contains {0} arrival predictions for this timestamp.", arrivalPredictionsForRoute.Count());

						double[,] bitmapDouble = new double[imageWidth, imageHeight];
						bool[,] bitmapBoolStop = new bool[imageWidth, imageHeight];
						for (int s = 0; s < stopPointsInOrder.Count; s++)
						{
							StopPoint stop = stopPointsInOrder[s];
							int yOffsetStopPoint = s * pixelsStopHeight;
							int yMaxStopPoint = (s + 1) * pixelsStopHeight - 1;
							int offsetInMinutesStopPoint = minutesBeforeFirstStop + s * minutesBetweenStops;
							int xOffsetStopPoint = offsetInMinutesStopPoint * pixelsPerMinute;
							IEnumerable<ArrivalPrediction> arrivalPredictionsForStop = arrivalPredictionsForRoute.Where(a => a.StopPoint == stop);
							foreach (ArrivalPrediction arrivalPrediction in arrivalPredictionsForStop)
							{
								double timeToStationInMinutes = arrivalPrediction.TimeToStation / 60.0;
								double gaussianMeanInMinutes = (double)offsetInMinutesStopPoint - timeToStationInMinutes;
								GaussianDistribution gaussianDistribution = new GaussianDistribution(gaussianMeanInMinutes, gaussianStandardDeviation);
								for (int x = 0; x < xOffsetStopPoint; x++)
								{
									double xInMinutes = (double)x / (double)pixelsPerMinute;
									double gaussianValueAtX = gaussianDistribution.GetValue(xInMinutes);
									for (int y = yOffsetStopPoint; y <= yMaxStopPoint; y++)
									{
										bitmapDouble[x, y] = bitmapDouble[x, y] + gaussianValueAtX;
									}
								}
								for (int y = yOffsetStopPoint; y <= yMaxStopPoint; y++)
								{
									if (xOffsetStopPoint < imageWidth)
									{
										bitmapBoolStop[xOffsetStopPoint, y] = true;
									}
								}
							}
						}

						double maxValue = 0;
						for (int x = 0; x < imageWidth; x++)
						{
							for (int y = 0; y < imageHeight; y++)
							{
								if (bitmapDouble[x, y] > maxValue)
								{
									maxValue = bitmapDouble[x, y];
								}
							}
						}

						int[,] bitmapInt = new int[imageWidth, imageHeight];
						double grayScaleFactor = (maxValue == 0.0) ? 0.0 : 255.0 / maxValue;
						for (int x = 0; x < imageWidth; x++)
						{
							for (int y = 0; y < imageHeight; y++)
							{
								bitmapInt[x, y] = (int)Math.Round(bitmapDouble[x, y] * grayScaleFactor);
							}
						}

						Stopwatch watch = new Stopwatch();
						watch.Start();
						Bitmap bitmap = new Bitmap(imageWidth, imageHeight);
						for (int x = 0; x < imageWidth; x++)
						{
							for (int y = 0; y < imageHeight; y++)
							{
								Color pixelColor;
								if (bitmapBoolStop[x, y])
								{
									pixelColor = Color.Yellow;
								}
								else
								{
									pixelColor = Color.FromArgb(bitmapInt[x, y], bitmapInt[x, y], bitmapInt[x, y]);
								}
								bitmap.SetPixel(x, y, pixelColor);
							}
						}
						watch.Stop();
						Trace.TraceInformation(@"Converting to an image, bitmap created in {0:#.###}s.", watch.ElapsedMilliseconds / 1000.0);
						imageDictionary.Add(currentImageDateTime, bitmap);
					}
				}
			}

			return imageDictionary;
		}

		public IDictionary<DateTime, Image> ConvertToImagesWithColorThreadSafe(Route route, DateTime since, int minutesBetweenStops, int pixelsPerMinute, int pixelsStopHeight, int minutesBeforeFirstStop, double gaussianStandardDeviation)
		{
			IDictionary<DateTime, Image> imageDictionary = new Dictionary<DateTime, Image>();

			lock (this._Lock)
			{
				IList<StopPoint> stopPointsInOrder = route.GetStopPointsInOrder();
				int imageWidth = (stopPointsInOrder.Count - 1) * minutesBetweenStops * pixelsPerMinute + minutesBeforeFirstStop * pixelsPerMinute;
				int imageHeight = stopPointsInOrder.Count * pixelsStopHeight;
				Trace.TraceInformation(@"Converting to an image, all images will be {0}x{1} pixels.", imageWidth, imageHeight);

				foreach (var keyValue in this.ArrivalPredictionRepository.ArrivalPredictionsByTimeStamp)
				{
					if (keyValue.Key >= since)
					{
						DateTime currentImageDateTime = keyValue.Key;
						Trace.TraceInformation(@"Converting to an image for timestamp = {0}.", currentImageDateTime);
						ICollection<ArrivalPrediction> currentArrivalPredictions = keyValue.Value;
						IEnumerable<ArrivalPrediction> arrivalPredictionsForRoute = currentArrivalPredictions.Where(a => (a.Routes.Contains(route)));
						Trace.TraceInformation(@"Converting to an image, route contains {0} arrival predictions for this timestamp.", arrivalPredictionsForRoute.Count());

						IColorScaleable[,] bitmapColors = new IColorScaleable[imageWidth, imageHeight];
						for (int s = 0; s < stopPointsInOrder.Count; s++)
						{
							StopPoint stop = stopPointsInOrder[s];
							int yOffsetStopPoint = s * pixelsStopHeight;
							int yMaxStopPoint = (s + 1) * pixelsStopHeight - 1;
							int offsetInMinutesStopPoint = minutesBeforeFirstStop + s * minutesBetweenStops;
							int xOffsetStopPoint = offsetInMinutesStopPoint * pixelsPerMinute;
							IEnumerable<ArrivalPrediction> arrivalPredictionsForStop = arrivalPredictionsForRoute.Where(a => a.StopPoint == stop);
							foreach (ArrivalPrediction arrivalPrediction in arrivalPredictionsForStop)
							{
								double timeToStationInMinutes = arrivalPrediction.TimeToStation / 60.0;
								double gaussianMeanInMinutes = (double)offsetInMinutesStopPoint - timeToStationInMinutes;
								GaussianDistribution gaussianDistribution = new GaussianDistribution(gaussianMeanInMinutes, gaussianStandardDeviation);
								int vehicleIdHashCode = Math.Abs((string.IsNullOrEmpty(arrivalPrediction.VehicleId)) ? 0 : arrivalPrediction.VehicleId.GetHashCode());
								double vehicleIdDouble = (vehicleIdHashCode % 256) / 255.0;
								for (int x = 0; x < xOffsetStopPoint; x++)
								{
									double xInMinutes = (double)x / (double)pixelsPerMinute;
									double gaussianValueAtX = gaussianDistribution.GetValue(xInMinutes);
									for (int y = yOffsetStopPoint; y <= yMaxStopPoint; y++)
									{
										if (bitmapColors[x, y] == null)
										{
											bitmapColors[x, y] = new HLColor(vehicleIdDouble, gaussianValueAtX);
										}
										else
										{
											bitmapColors[x, y] = bitmapColors[x, y].AddTo(new HLColor(vehicleIdDouble, gaussianValueAtX));
										}
									}
								}
							}
							for (int y = yOffsetStopPoint; y <= yMaxStopPoint; y++)
							{
								if (xOffsetStopPoint < imageWidth)
								{
									bitmapColors[xOffsetStopPoint, y] = new HSLColor(0.0, 1.0, 1.0);
								}
							}
						}

						double maxLuminosityRawValue = 0;
						for (int x = 0; x < imageWidth; x++)
						{
							for (int y = 0; y < imageHeight; y++)
							{
								HLColor hlColor = bitmapColors[x, y] as HLColor;
								if (hlColor != null && hlColor.LuminosityRawValue > maxLuminosityRawValue)
								{
									maxLuminosityRawValue = hlColor.LuminosityRawValue;
								}
							}
						}

						Stopwatch watch = new Stopwatch();
						watch.Start();
						Bitmap bitmap = new Bitmap(imageWidth, imageHeight);
						for (int x = 0; x < imageWidth; x++)
						{
							for (int y = 0; y < imageHeight; y++)
							{
								Color color = (bitmapColors[x, y] == null) ? Color.Black : bitmapColors[x, y].GetWinformsColor(maxLuminosityRawValue);
								bitmap.SetPixel(x, y, color);
							}
						}
						watch.Stop();
						Trace.TraceInformation(@"Converting to an image, bitmap created in {0:#.###}s.", watch.ElapsedMilliseconds / 1000.0);
						imageDictionary.Add(currentImageDateTime, bitmap);
					}
				}
			}

			return imageDictionary;
		}
		#endregion
	}
}
