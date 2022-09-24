using TaleWorlds.MountAndBlade;

namespace ArmyArrowCounter {
    /** For debugging. Intended use is to add the following line to AacMissionBehavior's constructor and to make this a property of it:
     *  EventLogger = new EventLogger(this, ArrowCounter);
     *  
     *  Comment in/out whatever logs you want in this class's constructor.
     */
    class EventLogger {

        public EventLogger(AacMissionBehavior aacMissionBehavior, ArrowCounter arrowCounter) {
            //aacMissionBehavior.AllyAgentBuiltEvent += OnAllyAgentBuilt;
            //aacMissionBehavior.AllyAgentRemovedEvent += OnAllyAgentRemoved;
            //aacMissionBehavior.AllyFiredMissileEvent += OnAllyFiredMissile;
            //aacMissionBehavior.BattleStartEvent += OnBattleStart;
            //aacMissionBehavior.SiegeBattleStartEvent += OnSiegeBattleStart;
            //aacMissionBehavior.PlayerBuiltEvent += OnPlayerBuilt;
            //arrowCounter.RemainingArrowsUpdateEvent += OnRemainingArrowsUpdate;
            //arrowCounter.MaxArrowsUpdateEvent += OnMaxArrowsUpdate;
        }

        private void OnAllyAgentBuilt(Agent agent) {
            Utils.Log("Agent built");
        }

        private void OnAllyAgentRemoved(Agent agent) {
            Utils.Log("Agent removed");
        }

        private void OnAllyFiredMissile() {
            Utils.Log("Ally fired missile");
        }

        private void OnBattleStart() {
            Utils.Log("Battle started");
        }

        private void OnSiegeBattleStart() {
            Utils.Log("Siege battle started");
        }

        private void OnPlayerBuilt() {
            Utils.Log("Player built");
        }

        private void OnRemainingArrowsUpdate(int newRemainingArrows) {
            Utils.Log("New remaining arrows: {0}", newRemainingArrows);
        }

        private void OnMaxArrowsUpdate(int newMaxArrows) {
            Utils.Log("New max arrows: {0}", newMaxArrows);
        }
    }
}
