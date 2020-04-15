using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyArrowCounter
{
    class ExactPercentVM : AacVM
    {
		private static readonly string REPORT_FORMAT = "Army remaining arrows: {0:P0}";

		public ExactPercentVM(ArrowCounter arrowCounter) : base(arrowCounter)
		{
		}

		protected override string GetArrowCounterText()
		{
			if (ArrowCounter.MaxArrows == 0)
			{
				return String.Format(REPORT_FORMAT, 0);
			}

			float proportionRemainingArrows = ArrowCounter.RemainingArrows / (float) ArrowCounter.MaxArrows;
			return String.Format(REPORT_FORMAT, proportionRemainingArrows);
		}
	}
}
