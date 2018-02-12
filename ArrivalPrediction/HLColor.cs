using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArrivalPrediction
{
	[DebuggerDisplay(@"Hue={Hue}, LuminosityRawValue={LuminosityRawValue}")]
	public class HLColor : IColorScaleable
	{
		#region Private fields
		private double _Hue;
		private double _LuminosityRawValue;
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
		public double LuminosityRawValue
		{
			get { return this._LuminosityRawValue; }
			set
			{
				if (value < 0.0)
				{
					throw new ArgumentOutOfRangeException(@"LuminosityRawValue value cannot be negative");
				}
				this._LuminosityRawValue = value;
			}
		}
		#endregion

		#region Constructors
		public HLColor(double hue, double luminosityRaw)
		{
			this.Hue = hue;
			this.LuminosityRawValue = luminosityRaw;
		}
		#endregion

		#region Methods
		public static HLColor HLColorsAddition(HLColor color1, HLColor color2)
		{
			if (color2.LuminosityRawValue + color1.LuminosityRawValue == 0.0)
			{
				return new HLColor((color1.Hue + color2.Hue)/2.0, 0.0);
			}
			else
			{
				return new HLColor((color1.Hue * color1.LuminosityRawValue + color2.Hue * color2.LuminosityRawValue) / (color1.LuminosityRawValue + color2.LuminosityRawValue), color1.LuminosityRawValue + color2.LuminosityRawValue);
			}
		}

		public IColorScaleable AddTo(IColorScaleable otherColor)
		{
			if (otherColor == null)
			{
				return this;
			}
			else if (otherColor is HLColor)
			{
				return HLColor.HLColorsAddition(this, (HLColor)otherColor);
			}
			else if (otherColor is HSLColor)
			{
				return otherColor;
			}
			else
			{
				return this;
			}
		}

		public System.Drawing.Color GetWinformsColor(double maxLuminosity)
		{
			System.Windows.Media.Color wpfColor = (System.Windows.Media.Color)(new HSLColorConversion(this.Hue, 1.0, this.LuminosityRawValue / maxLuminosity));
			return System.Drawing.Color.FromArgb(wpfColor.R, wpfColor.G, wpfColor.B);
		}
		#endregion
	}
}
