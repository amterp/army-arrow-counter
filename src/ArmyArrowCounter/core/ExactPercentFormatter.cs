using System;

namespace ArmyArrowCounter {
    class ExactPercentFormatter : ArrowTextFormatter {
        private static readonly string REPORT_FORMAT = "{0}{1}%";
        private static readonly int TO_PERCENT = 100;

        public string Format(int remainingArrows, int maxArrows, string prefix) {
            if (maxArrows == 0) {
                return String.Format(REPORT_FORMAT, prefix, 0);
            }

            double percentRemaining = (double)remainingArrows / maxArrows * TO_PERCENT;
            int rounded = (int)Math.Round(percentRemaining, MidpointRounding.AwayFromZero);

            return String.Format(REPORT_FORMAT, prefix, rounded);
        }
    }
}
