using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace ArmyArrowCounter
{
	class NearestWrittenVM : AacVM
	{
		private static readonly string REPORT_FORMAT = "{0} ammunition remaining.";

		public NearestWrittenVM(ArrowCounter arrowCounter) : base(arrowCounter)
		{
		}

		protected override string GetArrowCounterText()
		{
			string formattedString;
			if (ArrowCounter.MaxArrows == 0)
			{
				formattedString = String.Format(REPORT_FORMAT, "no");
			}
			else
			{
				float proportionRemainingArrows = ArrowCounter.RemainingArrows / (float) ArrowCounter.MaxArrows;
				if (ArrowCounter.RemainingArrows == ArrowCounter.MaxArrows)
				{
					formattedString = String.Format(REPORT_FORMAT, "all");
				}
				else if (proportionRemainingArrows >= 0.875)
				{
					formattedString = String.Format(REPORT_FORMAT, "almost all");
				}
				else if (proportionRemainingArrows >= 0.71)
				{
					formattedString = String.Format(REPORT_FORMAT, "about three quarters");
				}
				else if (proportionRemainingArrows >= 0.585)
				{
					formattedString = String.Format(REPORT_FORMAT, "about two thirds");
				}
				else if (proportionRemainingArrows >= 0.415)
				{
					formattedString = String.Format(REPORT_FORMAT, "about half");
				}
				else if (proportionRemainingArrows >= 0.29)
				{
					formattedString = String.Format(REPORT_FORMAT, "about one third");
				}
				else if (proportionRemainingArrows >= 0.125)
				{
					formattedString = String.Format(REPORT_FORMAT, "about one quarter");
				}
				else if (proportionRemainingArrows > 0)
				{
					formattedString = String.Format(REPORT_FORMAT, "almost no");
				}
				else
				{
					formattedString = String.Format(REPORT_FORMAT, "no");
				}
			}

			if (Config.Instance().Prefix.IsEmpty())
			{
				return char.ToUpper(formattedString.First()) + formattedString.Substring(1);
			} else
			{
				return formattedString;
			}
		}
	}
}
