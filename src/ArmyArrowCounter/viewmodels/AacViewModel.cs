using TaleWorlds.Library;

namespace ArmyArrowCounter {
    class AacViewModel : ViewModel {
        private readonly ArrowCounter ArrowCounter;
        private readonly ArrowTextFormatter Formatter;

        public AacViewModel(ArrowCounter arrowCounter, ArrowTextFormatter formatter) {
            ArrowCounter = arrowCounter;
            Formatter = formatter;
            arrowCounter.RemainingArrowsUpdateEvent += OnArrowCountUpdated;
            arrowCounter.MaxArrowsUpdateEvent += OnArrowCountUpdated;
        }

        private void OnArrowCountUpdated(int ignored) {
            base.OnPropertyChanged("ArrowCounterText");
        }

        [DataSourceProperty]
        public string ArrowCounterText {
            get => Formatter.Format(ArrowCounter.RemainingArrows, ArrowCounter.MaxArrows, Config.Instance().Prefix);
            set {
            }
        }
    }
}
