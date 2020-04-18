using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;

namespace ArmyArrowCounter
{
    class ExactFractionVM : AacVM
	{
		public ExactFractionVM(ArrowCounter arrowCounter) : base(arrowCounter)
		{
		}

		protected override string GetArrowCounterText()
		{
			return String.Format("{0} / {1}", ArrowCounter.RemainingArrows, ArrowCounter.MaxArrows);
		}
	}
}
