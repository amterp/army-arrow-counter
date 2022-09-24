using System;
using TaleWorlds.MountAndBlade;

namespace ArmyArrowCounter {
    class ArrowRecountTriggerer {
        private readonly float PROPORTION_OF_TROOPS__TO_DIE_BEFORE_RECOUNT = 0.1f;

        private AacMissionBehavior aacMissionBehavior;
        private ArrowCounter arrowCounter;

        private short removalsUntilNextRecount = 1;
        private bool active = false;

        public ArrowRecountTriggerer(AacMissionBehavior aacMissionBehavior, ArrowCounter arrowCounter) {
            this.aacMissionBehavior = aacMissionBehavior;
            this.arrowCounter = arrowCounter;

            aacMissionBehavior.AllyFiredMissileEvent += OnAllyFired;
            aacMissionBehavior.AllyAgentRemovedEvent += OnAgentRemoved;
            arrowCounter.RemainingArrowsUpdateEvent += OnRemainingArrowsUpdate;
        }

        private void OnAllyFired(Agent agent) {
            if (active) {
                return;
            }

            removalsUntilNextRecount = CalculateRemovalsUntilNextRecount();
            active = true;
        }

        private void OnAgentRemoved(Agent agent) {
            removalsUntilNextRecount--;

            if (removalsUntilNextRecount <= 0) {
                arrowCounter.RecountAllAlliedAgents();
                arrowCounter.RemoveAgent(agent);
                removalsUntilNextRecount = CalculateRemovalsUntilNextRecount();
            }
        }

        private void OnRemainingArrowsUpdate(int remainingArrows) {
            if (remainingArrows < 0) {
                arrowCounter.RecountAllAlliedAgents();
            }
        }

        private short CalculateRemovalsUntilNextRecount() {
            int? numAllies = aacMissionBehavior.Mission?.PlayerTeam?.TeamAgents?.Count;
            if (!numAllies.HasValue || numAllies == 0) {
                return 1;
            }

            return (short)Math.Max(Math.Ceiling(PROPORTION_OF_TROOPS__TO_DIE_BEFORE_RECOUNT * numAllies.Value), 1);
        }
    }
}
