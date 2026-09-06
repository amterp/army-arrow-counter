namespace ArmyArrowCounter {
    /** Renders the counter's numbers as the line the player reads.
     *
     *  The prefix is passed in rather than prepended by the caller because
     *  NearestWrittenFormatter capitalizes its sentence only when there is no
     *  prefix in front of it.
     */
    interface ArrowTextFormatter {
        string Format(int remainingArrows, int maxArrows, string prefix);
    }
}
