using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Engine.GauntletUI;

namespace ArmyArrowCounter
{
    class AacUiApplier
    {
        private AacVM AacVM;
        private GauntletLayer GauntletLayer;

        public AacUiApplier(AacMissionBehavior aacMissionBehavior, AacVM aacVM)
        {
            AacVM = aacVM;
            aacMissionBehavior.BattleStartEvent += OnBattleStart;
            aacMissionBehavior.PlayerKilledEvent += OnPlayerKilled;
            GauntletLayer = new GauntletLayer(100);
            GauntletLayer.LoadMovie("ArmyArrowCounter", AacVM);
        }

        private void OnBattleStart()
        {
            ScreenManager.TopScreen.AddLayer(GauntletLayer);
        }

        private void OnPlayerKilled()
        {
            ScreenManager.TopScreen.RemoveLayer(GauntletLayer);
        }
    }
}
