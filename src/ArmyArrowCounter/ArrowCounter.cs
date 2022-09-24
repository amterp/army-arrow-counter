using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace ArmyArrowCounter {
    class ArrowCounter {
        public event Action<int> RemainingArrowsUpdateEvent;
        public event Action<int> MaxArrowsUpdateEvent;
        public int RemainingArrows { get; private set; }
        public int MaxArrows { get; private set; }

        private readonly AacMissionBehavior AacMissionBehavior;
        private readonly Dictionary<int, short> AgentHashCodeToCurrentArrows = new Dictionary<int, short>();

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
            if (AgentHashCodeToCurrentArrows.ContainsKey(agent.GetHashCode())) {
                AgentHashCodeToCurrentArrows[agent.GetHashCode()]--;
            }
            AddToRemainingArrows(-1);
        }

        private void OnAllyPickedUpAmmo(Agent agent, SpawnedItemEntity item) {
            if (!AgentHashCodeToCurrentArrows.ContainsKey(agent.GetHashCode())) {
                return;
            }

            short lastKnownAmmoOnAgent = AgentHashCodeToCurrentArrows[agent.GetHashCode()];
            short newAmmoOnAgent = CalculateRemainingAmmo(agent);
            short amountPickedUp = (short)(newAmmoOnAgent - lastKnownAmmoOnAgent);
            AgentHashCodeToCurrentArrows[agent.GetHashCode()] += amountPickedUp;
            AddToRemainingArrows(amountPickedUp);
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
            AgentHashCodeToCurrentArrows.Clear();
            AddToRemainingArrows(-RemainingArrows);
            AddToMaxArrows(-MaxArrows);
        }

        internal void RecountAllAlliedAgents() {
            ForgetState();
            CountAllAlliedAgents(true);
        }

        internal void AddToRemainingArrows(int deltaRemainingArrows) {
            RemainingArrows += deltaRemainingArrows;
            RemainingArrowsUpdateEvent?.Invoke(RemainingArrows);
        }

        internal void AddToMaxArrows(int deltaMaxArrows) {
            MaxArrows += deltaMaxArrows;
            MaxArrowsUpdateEvent?.Invoke(MaxArrows);
        }

        internal void AddAgent(Agent agent, bool countRemaining = false) {
            int agentHashCode = agent.GetHashCode();
            if (AgentHashCodeToCurrentArrows.ContainsKey(agentHashCode)) {
                return;
            }

            if (agent.Equipment == null) {
                return;
            }

            short maxAmmo = CalculateMaxAmmo(agent);
            AddToMaxArrows(maxAmmo);

            short remainingAmmo;
            if (countRemaining) {
                remainingAmmo = CalculateRemainingAmmo(agent);
            } else {
                remainingAmmo = maxAmmo;
            }

            AgentHashCodeToCurrentArrows.Add(agentHashCode, remainingAmmo);
            AddToRemainingArrows(remainingAmmo);
        }

        internal void RemoveAgent(Agent agent) {
            int agentHashCode = agent.GetHashCode();
            if (!AgentHashCodeToCurrentArrows.ContainsKey(agentHashCode)) {
                return;
            }

            AgentHashCodeToCurrentArrows.Remove(agentHashCode);
            short remainingAmmo = CalculateRemainingAmmo(agent);
            short maxAmmo = CalculateMaxAmmo(agent);
            AddToRemainingArrows(-remainingAmmo);
            AddToMaxArrows(-maxAmmo);
        }

        private static short CalculateRemainingAmmo(Agent agent) {
            MissionWeapon weaponFromSlot0 = agent.Equipment[EquipmentIndex.Weapon0];
            short ammoFromSlot0 = weaponFromSlot0.Equals(MissionWeapon.Invalid) || weaponFromSlot0.IsShield() ? (short)0 : weaponFromSlot0.Amount;
            MissionWeapon weaponFromSlot1 = agent.Equipment[EquipmentIndex.Weapon1];
            short ammoFromSlot1 = weaponFromSlot1.Equals(MissionWeapon.Invalid) || weaponFromSlot1.IsShield() ? (short)0 : weaponFromSlot1.Amount;
            MissionWeapon weaponFromSlot2 = agent.Equipment[EquipmentIndex.Weapon2];
            short ammoFromSlot2 = weaponFromSlot2.Equals(MissionWeapon.Invalid) || weaponFromSlot2.IsShield() ? (short)0 : weaponFromSlot2.Amount;
            MissionWeapon weaponFromSlot3 = agent.Equipment[EquipmentIndex.Weapon3];
            short ammoFromSlot3 = weaponFromSlot3.Equals(MissionWeapon.Invalid) || weaponFromSlot3.IsShield() ? (short)0 : weaponFromSlot3.Amount;
            MissionWeapon weaponFromSlot4 = agent.Equipment[EquipmentIndex.Weapon4];
            short ammoFromSlot4 = weaponFromSlot4.Equals(MissionWeapon.Invalid) || weaponFromSlot4.IsShield() ? (short)0 : weaponFromSlot4.Amount;

            return (short)(ammoFromSlot0 + ammoFromSlot1 + ammoFromSlot2 + ammoFromSlot3 + ammoFromSlot4);
        }

        private static short CalculateMaxAmmo(Agent agent) {
            int arrowAmmo = agent.Equipment.GetMaxAmmo(WeaponClass.Arrow);
            int boltAmmo = agent.Equipment.GetMaxAmmo(WeaponClass.Bolt);
            int javelinAmmo = agent.Equipment.GetMaxAmmo(WeaponClass.Javelin);
            int axeAmmo = agent.Equipment.GetMaxAmmo(WeaponClass.ThrowingAxe);

            return (short)(arrowAmmo + boltAmmo + javelinAmmo + axeAmmo);
        }
    }
}
