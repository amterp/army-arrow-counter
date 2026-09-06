namespace ArmyArrowCounter {
    static class ArrowTextFormatters {
        internal static ArrowTextFormatter For(CounterType counterType) {
            switch (counterType) {
                case CounterType.NEAREST_WRITTEN:
                    return new NearestWrittenFormatter();
                case CounterType.EXACT_PERCENT:
                    return new ExactPercentFormatter();
                case CounterType.NEAREST_10_PERCENT:
                    return new NearestXPercentFormatter(10);
                case CounterType.NEAREST_20_PERCENT:
                    return new NearestXPercentFormatter(20);
                case CounterType.NEAREST_25_PERCENT:
                    return new NearestXPercentFormatter(25);
                case CounterType.EXACT_FRACTION:
                default:
                    return new ExactFractionFormatter();
            }
        }
    }
}
