﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArrivalPrediction
{
	public static class HttpHelper
	{
		#region Private fields
		private static HttpClient _HttpsClient;
		#endregion

		#region Properties

		#endregion

		#region Constructors
		static HttpHelper()
		{
			// see https://stackoverflow.com/questions/22251689/make-https-call-using-httpclient/22251844
			System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
			HttpHelper._HttpsClient = new HttpClient();
		}
		#endregion

		#region Methods
		public static string GetHttpsString(string baseAddress, string uri)
		{
			string returnedString;
			try
			{
				HttpHelper._HttpsClient.BaseAddress = new Uri(baseAddress);
				Stopwatch sw = new Stopwatch();
				Trace.TraceInformation(@"HTTPS GET {0}", uri);
				sw.Start();
				returnedString = HttpHelper._HttpsClient.GetStringAsync(uri).Result;
				sw.Stop();
				Trace.TraceInformation(@"HTTPS GET executed in {0:#.###}s", sw.ElapsedMilliseconds / 1000.0);
			}
			catch (Exception ex)
			{
				Trace.TraceError(@"Error when executing HTTPS GET {0}" + Environment.NewLine + @"{1}", uri, ex.Message);
				throw;
			}
			return returnedString;
		}
		#endregion
	}
}
