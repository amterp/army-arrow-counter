using System;

namespace ArmyArrowCounter {
    class NearestXPercentFormatter : ArrowTextFormatter {
        private static readonly string REPORT_FORMAT = "{0}~{1}%";
        private static readonly int TO_PERCENT = 100;

        private readonly int RoundTo;

        public NearestXPercentFormatter(int roundTo) {
            RoundTo = roundTo;
        }

        public string Format(int remainingArrows, int maxArrows, string prefix) {
            if (maxArrows == 0) {
                return String.Format(REPORT_FORMAT, prefix, 0);
            }

            double percentRemaining = (double)remainingArrows / maxArrows * TO_PERCENT;
            int roundedPercent = (int)Math.Round(percentRemaining / RoundTo, MidpointRounding.AwayFromZero) * RoundTo;

            return String.Format(REPORT_FORMAT, prefix, roundedPercent);
        }
    }
}
