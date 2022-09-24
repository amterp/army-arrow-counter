using System;

namespace ArmyArrowCounter {
    class NearestXPercentViewModel : AacViewModel {
        private static readonly string REPORT_FORMAT = "~{0}%";
        private static readonly int TO_PERCENT = 100;

        private readonly int RoundTo;

        public NearestXPercentViewModel(ArrowCounter arrowCounter, int roundTo) : base(arrowCounter) {
            RoundTo = roundTo;
        }

        protected override string GetArrowCounterText() {
            if (ArrowCounter.MaxArrows == 0) {
                return String.Format(REPORT_FORMAT, 0);
            }

            float proportionRemainingArrows = ArrowCounter.RemainingArrows / (float)ArrowCounter.MaxArrows * TO_PERCENT;
            int roundedPercent = (int)Math.Round(proportionRemainingArrows / RoundTo) * RoundTo;

            return String.Format(REPORT_FORMAT, roundedPercent);
        }
    }
}
