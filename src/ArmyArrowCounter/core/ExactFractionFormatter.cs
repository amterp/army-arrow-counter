using System;

namespace ArmyArrowCounter {
    class ExactFractionFormatter : ArrowTextFormatter {
        public string Format(int remainingArrows, int maxArrows, string prefix) {
            return String.Format("{0}{1} / {2}", prefix, remainingArrows, maxArrows);
        }
    }
}
