namespace ArmyArrowCounter {
    struct TrackedAmmo {
        public int Max { get; }
        public int Remaining { get; }

        public TrackedAmmo(int max, int remaining) {
            Max = max;
            Remaining = remaining;
        }

        public TrackedAmmo WithRemaining(int remaining) {
            return new TrackedAmmo(Max, remaining);
        }
    }
}
