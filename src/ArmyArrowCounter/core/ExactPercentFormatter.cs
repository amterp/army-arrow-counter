using System;

namespace ArmyArrowCounter {
    class ExactPercentFormatter : ArrowTextFormatter {
        private static readonly string REPORT_FORMAT = "{0}{1:P0}";

        public string Format(int remainingArrows, int maxArrows, string prefix) {
            if (maxArrows == 0) {
                return String.Format(REPORT_FORMAT, prefix, 0f);
            }

            return String.Format(REPORT_FORMAT, prefix, remainingArrows / (float)maxArrows);
        }
    }
}
