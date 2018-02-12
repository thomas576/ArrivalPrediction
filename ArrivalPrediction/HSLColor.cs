using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArrivalPrediction
{
	[DebuggerDisplay(@"Hue={Hue}, Saturation={Saturation}, Luminosity={Luminosity}")]
	public class HSLColor : IColorScaleable
	{
		#region Private fields
		private double _Hue;
		private double _Saturation;
		private double _Luminosity;
		#endregion

		#region Properties
		public double Hue
		{
			get { return this._Hue; }
			set
			{
				if (value < 0.0 || value > 1.0)
				{
					throw new ArgumentOutOfRangeException(@"Hue value can only be between 0.0 and 1.0");
				}
				this._Hue = value;
			}
		}
		public double Saturation
		{
			get { return this._Saturation; }
			set
			{
				if (value < 0.0 || value > 1.0)
				{
					throw new ArgumentOutOfRangeException(@"Saturation value can only be between 0.0 and 1.0");
				}
				this._Saturation = value;
			}
		}
		public double Luminosity
		{
			get { return this._Luminosity; }
			set
			{
				if (value < 0.0 || value > 1.0)
				{
					throw new ArgumentOutOfRangeException(@"Luminosity value can only be between 0.0 and 1.0");
				}
				this._Luminosity = value;
			}
		}
		#endregion

		#region Constructors
		public HSLColor(double hue, double saturation, double luminosity)
		{
			this.Hue = hue;
			this.Saturation = saturation;
			this.Luminosity = luminosity;
		}
		#endregion

		#region Methods
		public System.Drawing.Color GetWinformsColor(double maxLuminosity)
		{
			System.Windows.Media.Color wpfColor = (System.Windows.Media.Color)(new HSLColorConversion(this.Hue, this.Saturation, this.Luminosity));
			return System.Drawing.Color.FromArgb(wpfColor.R, wpfColor.G, wpfColor.B);
		}

		public IColorScaleable AddTo(IColorScaleable otherColor)
		{
			if (otherColor == null)
			{
				return this;
			}
			else if (otherColor is HLColor)
			{
				return this;
			}
			else if (otherColor is HSLColor)
			{
				throw new ArgumentException(@"otherColor should not be also of type HSLColor.", @"otherColor");
			}
			else
			{
				return this;
			}
		}
		#endregion
	}
}
