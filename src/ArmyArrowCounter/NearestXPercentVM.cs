using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyArrowCounter
{
    class NearestXPercentVM : AacVM
    {
		private static readonly string REPORT_FORMAT = "Army remaining arrows: ~{0}%";
		private static readonly int TO_PERCENT = 100;

		private readonly int RoundTo;

		public NearestXPercentVM(ArrowCounter arrowCounter, int roundTo) : base(arrowCounter)
		{
			RoundTo = roundTo;
		}

		protected override string GetArrowCounterText()
		{
			if (ArrowCounter.MaxArrows == 0)
			{
				return String.Format(REPORT_FORMAT, 0);
			}

			float proportionRemainingArrows = ArrowCounter.RemainingArrows / (float) ArrowCounter.MaxArrows * TO_PERCENT;
			int roundedPercent = (int) Math.Round(proportionRemainingArrows / RoundTo) * RoundTo;

			return String.Format(REPORT_FORMAT, roundedPercent);
		}
	}
}
