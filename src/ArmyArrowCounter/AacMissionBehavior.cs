using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using System;

namespace ArmyArrowCounter
{
    class AacMissionBehavior : MissionBehaviour
    {
        public event Action PlayerBuiltEvent;
        public event Action PlayerKilledEvent;
        public event Action<Agent> AllyAgentBuiltEvent;
        public event Action<Agent> AllyAgentRemovedEvent;
        public event Action BattleStartEvent;
        public event Action SiegeBattleStartEvent;
        public event Action HideoutBattleStartEvent;
        public event Action<Agent> AllyFiredMissileEvent;
        public event Action<Agent, SpawnedItemEntity> OnAllyPickedUpAmmoEvent;

        private bool IsActivated = false;
        private bool PlayerAgentAlive = false;

        private ArrowCounter ArrowCounter;
        private AacUiApplier AacUiApplier;
        //private EventLogger EventLogger;

        public AacMissionBehavior() {
            ArrowCounter = new ArrowCounter(this);
            AacUiApplier = new AacUiApplier(this, AacVmFactory.Create(ArrowCounter));
            //EventLogger = new EventLogger(this, ArrowCounter);
        }

        public override MissionBehaviourType BehaviourType => MissionBehaviourType.Other;

        public override void OnMissionModeChange(MissionMode oldMissionMode, bool atStart)
        {
            base.OnMissionModeChange(oldMissionMode, atStart);
            if (oldMissionMode == MissionMode.StartUp && Mission.Mode == MissionMode.Battle)
            {
                BattleStartEvent?.Invoke();
                IsActivated = true;
            }
            else if (oldMissionMode == MissionMode.Deployment && Mission.Mode == MissionMode.Battle)
            {
                SiegeBattleStartEvent?.Invoke();
                PlayerAgentAlive = true;
                IsActivated = true;
            }
            else if (oldMissionMode == MissionMode.Battle && Mission.Mode == MissionMode.Stealth)
            {
                HideoutBattleStartEvent?.Invoke();
                PlayerAgentAlive = true;
                IsActivated = true;
            }
            else
            {
                IsActivated = false;
            }
        }

        public override void OnAgentBuild(Agent agent, Banner banner)
        {
            base.OnAgentBuild(agent, banner);

            if (!IsActivated)
            {
                return;
            }

            if (Agent.Main == null)
            {
                return;
            }

            if (!PlayerAgentAlive)
            {
                PlayerAgentAlive = true;
                PlayerBuiltEvent?.Invoke();
                return;
            }

            if (Utils.IsPlayerAlly(agent))
            {
                AllyAgentBuiltEvent(agent);
            }
        }

        public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
        {
            base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);

            if (!IsActivated)
            {
                return;
            }

            if (Agent.Main == null || affectedAgent.IsMine)
            {
                PlayerKilledEvent?.Invoke();
                PlayerAgentAlive = false;
                return;
            }

            if (Utils.IsPlayerAlly(affectedAgent))
            {
                AllyAgentRemovedEvent?.Invoke(affectedAgent);
            }
        }

        public override void OnAgentShootMissile(Agent shooterAgent, EquipmentIndex weaponIndex, Vec3 position, Vec3 velocity, Mat3 orientation, bool hasRigidBody, int forcedMissileIndex)
        {
            base.OnAgentShootMissile(shooterAgent, weaponIndex, position, velocity, orientation, hasRigidBody, forcedMissileIndex);
            
            if (!IsActivated)
            {
                return;
            }

            if (PlayerAgentAlive && Utils.IsPlayerAlly(shooterAgent))
            {
                AllyFiredMissileEvent?.Invoke(shooterAgent);
            }
        }

        public override void OnItemPickup(Agent agent, SpawnedItemEntity item)
        {
            base.OnItemPickup(agent, item);
            
            if (!IsActivated)
            {
                return;
            }

            if (!Utils.IsAmmo(item))
            {
                return;
            }

            if (PlayerAgentAlive && Utils.IsPlayerAlly(agent))
            {
                OnAllyPickedUpAmmoEvent?.Invoke(agent, item);
            }
        }
    }
}
