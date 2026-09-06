using System.Reflection;
using TaleWorlds.MountAndBlade;

namespace ArmyArrowCounter {
    public class Main : MBSubModuleBase {
        private bool IsLoaded;

        protected override void OnBeforeInitialModuleScreenSetAsRoot() {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            if (!IsLoaded) {
                Initialize();
                Utils.Log("Mod loaded: Army Arrow Counter v{0}", ModVersion());
                IsLoaded = true;
            }
        }

        public override void OnMissionBehaviorInitialize(Mission mission) {

            if (mission == null) {
                return;
            }

            mission.AddMissionBehavior(new AacMissionBehavior());
        }

        private static string ModVersion() {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
        }

        private void Initialize() {
            Config.Instance();
        }
    }
}
