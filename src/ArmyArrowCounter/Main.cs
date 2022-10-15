using TaleWorlds.MountAndBlade;

namespace ArmyArrowCounter {
    public class Main : MBSubModuleBase {
        private bool IsLoaded;

        protected override void OnBeforeInitialModuleScreenSetAsRoot() {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            if (!IsLoaded) {
                Initialize();
                Utils.Log("Mod loaded: Army Arrow Counter v1.6.1 (Nexus)");
                IsLoaded = true;
            }
        }

        public override void OnMissionBehaviorInitialize(Mission mission) {

            if (mission == null) {
                return;
            }

            mission.AddMissionBehavior(new AacMissionBehavior());
        }

        private void Initialize() {
            Config.Instance();
        }
    }
}
