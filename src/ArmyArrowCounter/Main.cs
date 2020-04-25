using System;
using TaleWorlds.MountAndBlade;

namespace ArmyArrowCounter
{
    public class Main : MBSubModuleBase
    {
        private bool IsLoaded;

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            if (!IsLoaded)
            {
                Initialize();
                Utils.Log("Mod loaded: Army Arrow Counter v1.2.7");
                IsLoaded = true;
            }
        }

        public override void OnMissionBehaviourInitialize(Mission mission)
        {
            
            if (mission == null)
            {
                return;
            }

            mission.AddMissionBehaviour(new AacMissionBehavior());
        }

        private void Initialize()
        {
            Config.Instance();
        }
    }
}
