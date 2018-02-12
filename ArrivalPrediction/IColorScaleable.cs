using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrivalPrediction
{
	public interface IColorScaleable
	{
		System.Drawing.Color GetWinformsColor(double maxLuminosity);
		IColorScaleable AddTo(IColorScaleable otherColor);
	}
}
