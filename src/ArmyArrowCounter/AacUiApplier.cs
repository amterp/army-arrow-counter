using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;

namespace ArmyArrowCounter
{
    class AacUiApplier
    {
        private ViewModel ViewModel;
        private GauntletLayer GauntletLayer;

        public AacUiApplier(AacMissionBehavior aacMissionBehavior, ViewModel viewModel)
        {
            ViewModel = viewModel;
            aacMissionBehavior.BattleStartEvent += OnBattleStart;
            GauntletLayer = new GauntletLayer(100);
            GauntletLayer.LoadMovie("ArmyArrowCounter", ViewModel);
        }

        private void OnBattleStart()
        {
            ScreenManager.TopScreen.AddLayer(GauntletLayer);
        }
    }
}
