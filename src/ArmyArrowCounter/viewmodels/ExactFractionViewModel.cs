using System;

namespace ArmyArrowCounter {
    class ExactFractionViewModel : AacViewModel {
        public ExactFractionViewModel(ArrowCounter arrowCounter) : base(arrowCounter) {
        }

        protected override string GetArrowCounterText() {
            return String.Format("{0} / {1}", ArrowCounter.RemainingArrows, ArrowCounter.MaxArrows);
        }
    }
}
