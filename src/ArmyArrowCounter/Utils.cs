using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace ArmyArrowCounter
{
    class Utils
    {
        internal static void Log(Object somethingToLog)
        {
            InformationManager.DisplayMessage(new InformationMessage(somethingToLog.ToString()));
        }
        internal static void Log(String stringFormat, params Object[] stringFormatArgs)
        {
            InformationManager.DisplayMessage(new InformationMessage(String.Format(stringFormat, stringFormatArgs)));
        }

        internal static bool IsPlayerAlly(Agent agent)
        {
            return agent.IsFriendOf(Agent.Main) && !agent.IsMine;
        }
    }
}
