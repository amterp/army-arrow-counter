using System;

namespace ArmyArrowCounter
{
	class ExactPercentViewModel : AacViewModel
	{
		private static readonly string REPORT_FORMAT = "{0:P0}";

		public ExactPercentViewModel(ArrowCounter arrowCounter) : base(arrowCounter)
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
