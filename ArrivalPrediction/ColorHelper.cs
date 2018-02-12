using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrivalPrediction
{
	public static class ColorHelper
	{
		/// <summary>
		/// Creates a color well-suited to be used as a bcackground on the buttons.
		/// </summary>
		/// <param name="hue">
		/// A double between 0.0 and 1.0 where 1.0 corresponds to 360°.
		/// </param>
		/// <returns>
		/// A Wpf color
		/// </returns>
		public static System.Windows.Media.Color CreateWpfBackgroundColorFromHue(double hue)
		{
			return (System.Windows.Media.Color)(new HSLColorConversion(hue, 0.4, 0.75));
		}

		/// <summary>
		/// Creates a grayscale color with the given luminosity.
		/// </summary>
		/// <param name="luminosity">
		/// A double between 0.0 and 1.0.
		/// </param>
		/// <returns>
		/// A Wpf color
		/// </returns>
		public static System.Windows.Media.Color CreateWpfGrayscaleColorFromLuminosity(double luminosity)
		{
			return (System.Windows.Media.Color)(new HSLColorConversion(0.0, 0.0, luminosity));
		}

		/// <summary>
		/// Creates a fully parameterized color from the given hue, saturation and luminosity
		/// </summary>
		/// <param name="hue">A double between 0.0 and 1.0 where 1.0 corresponds to 360°.</param>
		/// <param name="saturation">A double between 0.0 and 1.0 where 1.0 corresponds to maximum saturation</param>
		/// <param name="luminosity">A double between 0.0 and 1.0 where 1.0 corresponds to maximum luminosity (white)</param>
		/// <returns>A Wpf color</returns>
		public static System.Windows.Media.Color CreateWpfColorFromHueSatLum(double hue, double saturation, double luminosity)
		{
			return (System.Windows.Media.Color)(new HSLColorConversion(hue, saturation, luminosity));
		}

		/// <summary>
		/// Creates a fully parameterized color from the given hue, saturation and luminosity
		/// </summary>
		/// <param name="hue">A double between 0.0 and 1.0 where 1.0 corresponds to 360°.</param>
		/// <param name="saturation">A double between 0.0 and 1.0 where 1.0 corresponds to maximum saturation</param>
		/// <param name="luminosity">A double between 0.0 and 1.0 where 1.0 corresponds to maximum luminosity (white)</param>
		/// <returns>A winforms color</returns>
		public static System.Drawing.Color CreateColorFromHueSatLum(double hue, double saturation, double luminosity)
		{
			System.Windows.Media.Color wpfColor = ColorHelper.CreateWpfColorFromHueSatLum(hue, saturation, luminosity);
			return System.Drawing.Color.FromArgb(wpfColor.R, wpfColor.G, wpfColor.B);
		}

		/// <summary>
		/// Creates a grayscale color with the given luminosity.
		/// </summary>
		/// <param name="luminosity">A double between 0.0 and 1.0.</param>
		/// <returns>A winforms color</returns>
		public static System.Drawing.Color CreateGrayscaleColorFromLuminosity(double luminosity)
		{
			int luminosityInteger = (int)Math.Round(luminosity * 255.0);
			return System.Drawing.Color.FromArgb(luminosityInteger, luminosityInteger, luminosityInteger);
		}
	}
}
