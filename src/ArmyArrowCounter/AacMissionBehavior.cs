using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ArmyArrowCounter
{
    class AacMissionBehavior : MissionBehaviour
    {
        private readonly ArrowTracker ArrowTracker;
        private readonly Logger Logger;

        private bool IsActivated = false;
        private bool PlayerAgentAlive = false;
        private short NumberOfPlayerAlliedSoldiersWithRangedAmmo = 0;

        public AacMissionBehavior()
        {
            ArrowTracker = new ArrowTracker();
            Logger = new Logger(ArrowTracker);
        }

        public override MissionBehaviourType BehaviourType => MissionBehaviourType.Other;

        public override void OnMissionModeChange(MissionMode oldMissionMode, bool atStart)
        {
            base.OnMissionModeChange(oldMissionMode, atStart);
            if (oldMissionMode == MissionMode.StartUp && Mission.Mode == MissionMode.Battle)
            {
                Utils.Log("Army Arrow Counter active.");
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
                PerformInitialCount();
                return;
            }

            if (agent.IsFriendOf(Agent.Main))
            {
                if (ArrowTracker.AddAgent(agent))
                {
                    NumberOfPlayerAlliedSoldiersWithRangedAmmo++;
                }
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
                PlayerAgentAlive = false;
                return;
            }

            if (IsPlayerAlly(affectedAgent))
            {
                if (ArrowTracker.RemoveAgent(affectedAgent))
                {
                    NumberOfPlayerAlliedSoldiersWithRangedAmmo--;
                }
            }
        }

        public override void OnAgentShootMissile(Agent shooterAgent, EquipmentIndex weaponIndex, Vec3 position, Vec3 velocity, Mat3 orientation, bool hasRigidBody, int forcedMissileIndex)
        {
            base.OnAgentShootMissile(shooterAgent, weaponIndex, position, velocity, orientation, hasRigidBody, forcedMissileIndex);

            if (!IsActivated)
            {
                return;
            }

            if (PlayerAgentAlive && IsPlayerAlly(shooterAgent))
            {
                ArrowTracker.AddToRemainingArrows(-1);
                Logger.OnAgentShootMissile(NumberOfPlayerAlliedSoldiersWithRangedAmmo);
            }
        }

        private void PerformInitialCount()
        {
            foreach (Agent agent in Mission.Agents)
            {
                if (IsPlayerAlly(agent))
                {
                    if (ArrowTracker.AddAgent(agent))
                    {
                        NumberOfPlayerAlliedSoldiersWithRangedAmmo++;
                    }
                }
            }
        }

        private static bool IsPlayerAlly(Agent agent)
        {
            return agent.IsFriendOf(Agent.Main) && !agent.IsMine;
        }
    }
}
