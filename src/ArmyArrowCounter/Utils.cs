using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ArmyArrowCounter
{
    class Utils
    {
        internal static Color RED = new Color(1f, 0, 0);

        internal static void Log(Object somethingToLog)
        {
            InformationManager.DisplayMessage(new InformationMessage(somethingToLog.ToString()));
        }
        internal static void Log(String stringFormat, params Object[] stringFormatArgs)
        {
            InformationManager.DisplayMessage(new InformationMessage(String.Format(stringFormat, stringFormatArgs)));
        }
        internal static void LogWithColor(Color color, Object somethingToLog)
        {
            InformationManager.DisplayMessage(new InformationMessage(somethingToLog.ToString(), color));
        }
        internal static void LogWithColor(Color color, String stringFormat, params Object[] stringFormatArgs)
        {
            InformationManager.DisplayMessage(new InformationMessage(String.Format(stringFormat, stringFormatArgs), color));
        }

        internal static bool IsPlayerAlly(Agent agent)
        {
            return agent.IsFriendOf(Agent.Main) && !agent.IsMine;
        }

        internal static bool IsAmmo(SpawnedItemEntity item)
        {
            return item.IsStuckMissile() || item.WeaponCopy.IsAnyAmmo();
        }
    }
}
