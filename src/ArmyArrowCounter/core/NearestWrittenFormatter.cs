using System;

namespace ArmyArrowCounter {
    class NearestWrittenFormatter : ArrowTextFormatter {
        private static readonly string REPORT_FORMAT = "{0} ammunition remaining.";

        public string Format(int remainingArrows, int maxArrows, string prefix) {
            string sentence = String.Format(REPORT_FORMAT, DescribeAmount(remainingArrows, maxArrows));

            if (String.IsNullOrEmpty(prefix)) {
                return char.ToUpper(sentence[0]) + sentence.Substring(1);
            }

            return prefix + sentence;
        }

        private static string DescribeAmount(int remainingArrows, int maxArrows) {
            if (maxArrows == 0) {
                return "no";
            }

            if (remainingArrows == maxArrows) {
                return "all";
            }

            float proportionRemaining = remainingArrows / (float)maxArrows;
            if (proportionRemaining >= 0.875) {
                return "almost all";
            } else if (proportionRemaining >= 0.71) {
                return "about three quarters";
            } else if (proportionRemaining >= 0.585) {
                return "about two thirds";
            } else if (proportionRemaining >= 0.415) {
                return "about half";
            } else if (proportionRemaining >= 0.29) {
                return "about one third";
            } else if (proportionRemaining >= 0.125) {
                return "about one quarter";
            } else if (proportionRemaining > 0) {
                return "almost no";
            } else {
                return "no";
            }
        }
    }
}
