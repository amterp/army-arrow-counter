using TaleWorlds.Library;

namespace ArmyArrowCounter {
    class AacViewModelFactory {
        public static ViewModel Create(ArrowCounter arrowCounter) {
            return new AacViewModel(arrowCounter, ArrowTextFormatters.For(Config.Instance().CounterType));
        }
    }
}
