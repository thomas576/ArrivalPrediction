using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArrivalPrediction;
using System.Threading;
using System.Diagnostics;

namespace ArrivalPrediction.UI
{
	public partial class Form1 : Form
	{
		private ArrivalPredictionPolling _ArrivalPredictionPolling;

		public Form1()
		{
			InitializeComponent();

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

			TflConnectionSettings tflConnectionSettings = new TflConnectionSettings()
			{
				AppId = @"24f6effc",
				AppKey = @"b07dcb8449db2934ff23297f4004b0e1",
				HttpsBaseAddress = @"https://api.tfl.gov.uk/"
			};
			ArrivalPredictionDataMapper arrivalPredictionDataMapper = new ArrivalPredictionDataMapper(tflConnectionSettings);
			this._ArrivalPredictionPolling = new ArrivalPredictionPolling(arrivalPredictionDataMapper, 55000, 10000);
			this._ArrivalPredictionPolling.StartPollingInNewThread();
		}

		private void buttonStopPolling_Click(object sender, EventArgs e)
		{
			this._ArrivalPredictionPolling.StopPollingThread();
			this.labelInfo.Text = @"Polling stop requested, waiting 60s...";
			Task.Run(() =>
			{
				Thread.Sleep(60000);
				this.buttonPlay.BeginInvoke(new Action(() => { this.buttonPlay.Enabled = true; }));
				this.labelInfo.BeginInvoke(new Action(() => { this.labelInfo.Text = @"Polling is now stopped, data ready."; }));
			});
		}

		private void buttonPlay_Click(object sender, EventArgs e)
		{
			this.buttonPlay.Enabled = false;
			IDictionary<DateTime, Image> imageDictionary = this._ArrivalPredictionPolling.ConvertToImagesThreadSafe(
				ReferenceData.FindRoute(@"jubilee", LineDirectionsEnum.JubileeStanmoreToStratford),
				new DateTime(1900, 1, 1),
				2,
				20,
				15,
				5,
				0.3);

			if (imageDictionary.Any())
			{
				int imageWidth = imageDictionary.Values.First().Width;
				int imageHeight = imageDictionary.Values.First().Height;
				this.pictureBox1.Width = imageWidth;
				this.pictureBox1.Height = imageHeight;
				IList<DateTime> dateTimesSorted = imageDictionary.Keys.OrderBy(d => d).ToList();
				int numberOfDateTimes = dateTimesSorted.Count();
				int i = 0;
				while (true)
				{
					if (i == numberOfDateTimes)
					{
						i = 0;
						Thread.Sleep(1000);
					}
					DateTime dateTime = dateTimesSorted[i];
					this.pictureBox1.Image = imageDictionary[dateTime];
					this.labelInfo.Text = string.Format(@"Displaying image for {0}", dateTime.ToLongTimeString());
					Application.DoEvents();
					Thread.Sleep(75);
					i++;
				}
			}
			this.buttonPlay.Enabled = true;
		}
	}
}
