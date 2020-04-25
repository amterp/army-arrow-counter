using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using System;

namespace ArmyArrowCounter
{
    class AacMissionBehavior : MissionBehaviour
    {
        public event Action PlayerBuiltEvent;
        public event Action<Agent> AllyAgentBuiltEvent;
        public event Action<Agent> AllyAgentRemovedEvent;
        public event Action BattleStartEvent;
        public event Action SiegeBattleStartEvent;
        public event Action HideoutBattleStartEvent;
        public event Action<Agent> AllyFiredMissileEvent;
        public event Action<Agent, SpawnedItemEntity> OnAllyPickedUpAmmoEvent;

        public Agent PlayerAgent { private set; get; } = null;

        private bool IsActivated = false;

        private ArrowCounter ArrowCounter;
        private ArrowRecountTriggerer ArrowRecountTriggerer;
        private AacUiApplier AacUiApplier;
        //private EventLogger EventLogger;

        public AacMissionBehavior() {
            ArrowCounter = new ArrowCounter(this);
            ArrowRecountTriggerer = new ArrowRecountTriggerer(this, ArrowCounter);
            AacUiApplier = new AacUiApplier(this, AacVmFactory.Create(ArrowCounter));
            //EventLogger = new EventLogger(this, ArrowCounter);
        }

        public override MissionBehaviourType BehaviourType => MissionBehaviourType.Other;

        public override void OnMissionModeChange(MissionMode oldMissionMode, bool atStart)
        {
            base.OnMissionModeChange(oldMissionMode, atStart);

            if (Utils.IsStartOfBattle(oldMissionMode, Mission.Mode))
            {
                BattleStartEvent?.Invoke();
                IsActivated = true;
            }
            else if (Utils.IsStartofSiege(oldMissionMode, Mission.Mode))
            {
                PlayerAgent = Agent.Main;
                IsActivated = true;
                SiegeBattleStartEvent?.Invoke();
            }
            else if (Utils.IsStartofHideoutBattle(oldMissionMode, Mission.Mode))
            {
                PlayerAgent = Agent.Main;
                IsActivated = true;
                HideoutBattleStartEvent?.Invoke();
            }
            else if (Utils.IsEndOfHideoutConversation(oldMissionMode, Mission.Mode))
            {
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

            if (PlayerAgent == null)
            {
                PlayerAgent = Agent.Main;
                PlayerBuiltEvent?.Invoke();
                return;
            }

            if (PlayerAgent != null && Utils.IsPlayerAlly(agent, PlayerAgent))
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

            if (PlayerAgent != null && Utils.IsPlayerAlly(affectedAgent, PlayerAgent))
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
            if (PlayerAgent != null && Utils.IsPlayerAlly(shooterAgent, PlayerAgent))
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

            if (PlayerAgent != null && Utils.IsPlayerAlly(agent, PlayerAgent))
            {
                OnAllyPickedUpAmmoEvent?.Invoke(agent, item);
            }
        }
    }
}
