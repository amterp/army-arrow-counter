using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;

namespace ArmyArrowCounter {
    class ArrowCounter {
        public event Action<int> RemainingArrowsUpdateEvent {
            add { Ledger.RemainingChanged += value; }
            remove { Ledger.RemainingChanged -= value; }
        }
        public event Action<int> MaxArrowsUpdateEvent {
            add { Ledger.MaxChanged += value; }
            remove { Ledger.MaxChanged -= value; }
        }
        public int RemainingArrows => Ledger.Remaining;
        public int MaxArrows => Ledger.Max;

        private readonly AacMissionBehavior AacMissionBehavior;
        private readonly ArrowLedger Ledger = new ArrowLedger();

        public ArrowCounter(AacMissionBehavior aacMissionBehavior) {
            AacMissionBehavior = aacMissionBehavior;
            aacMissionBehavior.SiegeBattleStartEvent += OnSiegeBattleStart;
            aacMissionBehavior.HideoutBattleStartEvent += OnHideoutBattleStart;
            aacMissionBehavior.PlayerBuiltEvent += OnPlayerBuilt;
            aacMissionBehavior.AllyAgentBuiltEvent += OnAllyAgentBuilt;
            aacMissionBehavior.AllyAgentRemovedEvent += OnAllyAgentRemoved;
            aacMissionBehavior.AllyFiredMissileEvent += OnAllyFiredMissile;
            aacMissionBehavior.OnAllyPickedUpAmmoEvent += OnAllyPickedUpAmmo;
        }

        private void OnSiegeBattleStart() {
            CountAllAlliedAgents();
        }

        private void OnHideoutBattleStart() {
            CountAllAlliedAgents();
        }

        private void OnPlayerBuilt() {
            CountAllAlliedAgents();
        }

        private void OnAllyAgentBuilt(Agent agent) {
            AddAgent(agent);
        }

        private void OnAllyAgentRemoved(Agent agent) {
            RemoveAgent(agent);
        }

        private void OnAllyFiredMissile(Agent agent) {
            Ledger.RecordShot(agent.Index);
        }

        private void OnAllyPickedUpAmmo(Agent agent, SpawnedItemEntity item) {
            Ledger.RecordObservedAmmo(agent.Index, AmmoCount.Remaining(agent.Equipment));
        }

        internal void CountAllAlliedAgents(bool countRemainingArrows = false) {
            foreach (Agent agent in AacMissionBehavior.Mission.Agents) // todo: can instead maybe get player's MBTeam an iterate through friendly agents directly
            {
                if (Utils.IsPlayerAlly(agent, AacMissionBehavior.PlayerAgent)) {
                    AddAgent(agent, countRemainingArrows);
                }
            }
        }

        internal void ForgetState() {
            Ledger.Clear();
        }

        internal void RecountAllAlliedAgents() {
            ForgetState();
            CountAllAlliedAgents(true);
        }

        internal void AddAgent(Agent agent, bool countRemaining = false) {
            if (agent.Equipment == null) {
                return;
            }

            int maxAmmo = AmmoCount.Max(agent.Equipment);
            int remainingAmmo = countRemaining ? AmmoCount.Remaining(agent.Equipment) : maxAmmo;
            Ledger.Add(agent.Index, maxAmmo, remainingAmmo);
        }

        internal void RemoveAgent(Agent agent) {
            Ledger.Remove(agent.Index);
        }

    }
}
