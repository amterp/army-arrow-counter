using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;

namespace ArmyArrowCounter
{
	class NearestWrittenVM : AacVM
	{
		private static readonly string REPORT_FORMAT = "Your army has {0} ammunition remaining.";

		public NearestWrittenVM(ArrowCounter arrowCounter) : base(arrowCounter)
		{
		}

		protected override string GetArrowCounterText()
		{
			if (ArrowCounter.MaxArrows == 0)
			{
				return "Your army has no ammunition.";
			}

			float proportionRemainingArrows = ArrowCounter.RemainingArrows / (float) ArrowCounter.MaxArrows;
			if (ArrowCounter.RemainingArrows == ArrowCounter.MaxArrows) {
				return String.Format(REPORT_FORMAT, "all");
			}
			else if (proportionRemainingArrows >= 0.875)
			{
				return String.Format(REPORT_FORMAT, "almost all");
			}
			else if (proportionRemainingArrows >= 0.71)
			{
				return String.Format(REPORT_FORMAT, "about three quarters");
			}
			else if (proportionRemainingArrows >= 0.585)
			{
				return String.Format(REPORT_FORMAT, "about two thirds");
			}
			else if (proportionRemainingArrows >= 0.415)
			{
				return String.Format(REPORT_FORMAT, "about half");
			}
			else if (proportionRemainingArrows >= 0.29)
			{
				return String.Format(REPORT_FORMAT, "about one third");
			}
			else if (proportionRemainingArrows >= 0.125)
			{
				return String.Format(REPORT_FORMAT, "about one quarter");
			}
			else if (proportionRemainingArrows > 0)
			{
				return String.Format(REPORT_FORMAT, "almost no");
			}
			else
			{
				return String.Format(REPORT_FORMAT, "no");
			}
		}
	}
}
