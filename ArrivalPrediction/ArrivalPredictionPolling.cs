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
		private readonly ITflConnectionSettings _TflConnectionSettings;
		private readonly int _PeriodInMilliseconds;
		private readonly int _MinimumGapInMilliseconds;
		private volatile bool _IsPollingActive;
		#endregion

		#region Properties
		public ArrivalPredictionRepository ArrivalPredictionRepository { get; set; }
		public int PeriodInMilliseconds {
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
		public ArrivalPredictionPolling(ITflConnectionSettings tflConnectionSettings, int periodInMilliseconds, int minimumGapInMilliseconds)
		{
			this.ArrivalPredictionRepository = new ArrivalPredictionRepository();
			this._TflConnectionSettings = tflConnectionSettings;
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
					HashSet<ArrivalPrediction> returnedArrivalPredictions = (HashSet<ArrivalPrediction>)new ArrivalPredictionDataMapper(this._TflConnectionSettings).GetAllArrivalPredictions();
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

		public IDictionary<DateTime, Image> ConvertToImagesThreadSafe(DateTime since)
		{
			IDictionary<DateTime, Image> imageDictionary = new Dictionary<DateTime, Image>();

			lock (this._Lock)
			{

			}

			return imageDictionary;
		}
		#endregion
	}
}
