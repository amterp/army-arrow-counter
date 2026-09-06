using System;
using System.Collections.Generic;

namespace ArmyArrowCounter {
    /** Running totals of the army's ammunition, tracked per agent.
     *
     *  Every total is derived from what was recorded when an agent joined, never
     *  from re-reading the agent later. An agent that has died, fled, or been
     *  torn down no longer has readable equipment, so recomputing at removal
     *  subtracts the wrong amount and the totals drift.
     */
    class ArrowLedger {
        public event Action<int> RemainingChanged;
        public event Action<int> MaxChanged;

        public int Remaining { get; private set; }
        public int Max { get; private set; }

        private readonly Dictionary<int, TrackedAmmo> Tracked = new Dictionary<int, TrackedAmmo>();

        public bool IsTracking(int agentId) {
            return Tracked.ContainsKey(agentId);
        }

        public void Add(int agentId, int maxAmmo, int remainingAmmo) {
            if (Tracked.ContainsKey(agentId)) {
                return;
            }

            Tracked.Add(agentId, new TrackedAmmo(maxAmmo, remainingAmmo));
            AddToMax(maxAmmo);
            AddToRemaining(remainingAmmo);
        }

        public void Remove(int agentId) {
            TrackedAmmo tracked;
            if (!Tracked.TryGetValue(agentId, out tracked)) {
                return;
            }

            Tracked.Remove(agentId);
            AddToRemaining(-tracked.Remaining);
            AddToMax(-tracked.Max);
        }

        /** An untracked agent's shot is ignored rather than subtracted from the
         *  army total, which would leave a decrement no removal can ever match.
         */
        public void RecordShot(int agentId) {
            TrackedAmmo tracked;
            if (!Tracked.TryGetValue(agentId, out tracked)) {
                return;
            }

            Tracked[agentId] = tracked.WithRemaining(tracked.Remaining - 1);
            AddToRemaining(-1);
        }

        /** For ammunition picked up off the ground, where only the resulting total
         *  is observable and the difference has to be inferred.
         */
        public void RecordObservedAmmo(int agentId, int observedRemaining) {
            TrackedAmmo tracked;
            if (!Tracked.TryGetValue(agentId, out tracked)) {
                return;
            }

            Tracked[agentId] = tracked.WithRemaining(observedRemaining);
            AddToRemaining(observedRemaining - tracked.Remaining);
        }

        public void Clear() {
            Tracked.Clear();
            AddToRemaining(-Remaining);
            AddToMax(-Max);
        }

        private void AddToRemaining(int delta) {
            Remaining += delta;
            RemainingChanged?.Invoke(Remaining);
        }

        private void AddToMax(int delta) {
            Max += delta;
            MaxChanged?.Invoke(Max);
        }
    }
}
