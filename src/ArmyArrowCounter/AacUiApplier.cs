using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace ArmyArrowCounter {
    class AacUiApplier {
        private ViewModel ViewModel;
        private GauntletLayer GauntletLayer;
        private bool IsShown;

        public AacUiApplier(AacMissionBehavior aacMissionBehavior, ViewModel viewModel) {
            ViewModel = viewModel;
            // Which of these fires depends on how the mission reaches Battle mode. A field battle
            // with a deployment phase arrives via Deployment -> Battle, not StartUp -> Battle, so
            // subscribing to the battle event alone left the counter off screen in most fights.
            aacMissionBehavior.BattleStartEvent += Show;
            aacMissionBehavior.SiegeBattleStartEvent += Show;
            aacMissionBehavior.HideoutBattleStartEvent += Show;
            // The layer name argument was added in place of a "GauntletLayer" default the game
            // used to supply itself; passing it keeps this layer in the same category as vanilla ones.
            GauntletLayer = new GauntletLayer("GauntletLayer", 100);
            GauntletLayer.LoadMovie("ArmyArrowCounter", ViewModel);
        }

        // More than one of the start events can fire in a single mission, and the layer must only
        // ever be added once.
        private void Show() {
            if (IsShown) {
                return;
            }

            ScreenManager.TopScreen.AddLayer(GauntletLayer);
            IsShown = true;
        }
    }
}
