using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;

namespace ArmyArrowCounter
{
    class AacVM : ViewModel
	{
		private ArrowCounter ArrowCounter;

		public AacVM(ArrowCounter arrowCounter)
		{
			ArrowCounter = arrowCounter;
			arrowCounter.RemainingArrowsUpdateEvent += OnArrowCountUpdated;
			arrowCounter.MaxArrowsUpdateEvent += OnArrowCountUpdated;
		}

		private void OnArrowCountUpdated(int ignored)
		{
			base.OnPropertyChanged("ArrowCounterText");
		}

		[DataSourceProperty]
		public string ArrowCounterText
		{
			get
			{
				return String.Format("{0} / {1}", ArrowCounter.RemainingArrows, ArrowCounter.MaxArrows);
			}
			set
			{
			}
		}
	}
}
