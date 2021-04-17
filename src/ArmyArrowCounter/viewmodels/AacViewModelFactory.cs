using TaleWorlds.Library;

namespace ArmyArrowCounter
{
    class AacViewModelFactory
    {
        public static ViewModel Create(ArrowCounter arrowCounter)
        {
            switch (Config.Instance().CounterType)
            {
                case CounterType.NEAREST_WRITTEN:
                    return new NearestWrittenViewModel(arrowCounter);
                case CounterType.EXACT_PERCENT:
                    return new ExactPercentViewModel(arrowCounter);
                case CounterType.NEAREST_10_PERCENT:
                    return new NearestXPercentViewModel(arrowCounter, 10);
                case CounterType.NEAREST_20_PERCENT:
                    return new NearestXPercentViewModel(arrowCounter, 20);
                case CounterType.NEAREST_25_PERCENT:
                    return new NearestXPercentViewModel(arrowCounter, 25);
                case CounterType.EXACT_FRACTION:
                default:
                    return new ExactFractionViewModel(arrowCounter);
            }
        }
    }
}
