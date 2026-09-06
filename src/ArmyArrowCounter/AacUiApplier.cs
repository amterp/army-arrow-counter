using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace ArmyArrowCounter {
    class AacUiApplier {
        private ViewModel ViewModel;
        private GauntletLayer GauntletLayer;

        public AacUiApplier(AacMissionBehavior aacMissionBehavior, ViewModel viewModel) {
            ViewModel = viewModel;
            aacMissionBehavior.BattleStartEvent += OnBattleStart;
            // The layer name argument was added in place of a "GauntletLayer" default the game
            // used to supply itself; passing it keeps this layer in the same category as vanilla ones.
            GauntletLayer = new GauntletLayer("GauntletLayer", 100);
            GauntletLayer.LoadMovie("ArmyArrowCounter", ViewModel);
        }

        private void OnBattleStart() {
            ScreenManager.TopScreen.AddLayer(GauntletLayer);
        }
    }
}
