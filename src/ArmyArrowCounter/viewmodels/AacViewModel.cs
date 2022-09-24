using System;
using TaleWorlds.Library;

namespace ArmyArrowCounter {
    abstract class AacViewModel : ViewModel {
        protected ArrowCounter ArrowCounter;

        public AacViewModel(ArrowCounter arrowCounter) {
            ArrowCounter = arrowCounter;
            arrowCounter.RemainingArrowsUpdateEvent += OnArrowCountUpdated;
            arrowCounter.MaxArrowsUpdateEvent += OnArrowCountUpdated;
        }

        private void OnArrowCountUpdated(int ignored) {
            base.OnPropertyChanged("ArrowCounterText");
        }

        protected abstract string GetArrowCounterText();

        [DataSourceProperty]
        public string ArrowCounterText {
            get => String.Format("{0}{1}", Config.Instance().Prefix, GetArrowCounterText());
            set {
            }
        }
    }
}
