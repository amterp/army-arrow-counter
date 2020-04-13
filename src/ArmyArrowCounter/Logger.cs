namespace ArmyArrowCounter
{
    class Logger
    {
        private const short NumArrowsPerSoldierForEachLog = 1;

        private readonly ArrowTracker ArrowTracker;

        private int TotalArrowsFired = 0;
        private int NextArrowNumberToLogOn = 1;

        public Logger(ArrowTracker ArrowTracker)
        {
            this.ArrowTracker = ArrowTracker;
        }

        internal void Log()
        {
            Utils.Log("Remaining arrows in army: {0} / {1}", ArrowTracker.RemainingArrows, ArrowTracker.MaxArrows);
        }

        internal void OnAgentShootMissile(short numSoldiersWithAmmo)
        {
            TotalArrowsFired++;

            if (TotalArrowsFired == NextArrowNumberToLogOn)
            {
                Log();
                CalculateNextArrowOnWhichToLog(numSoldiersWithAmmo);
            }
        }

        internal void CalculateNextArrowOnWhichToLog(short numSoldiersWithAmmo)
        {
            NextArrowNumberToLogOn += numSoldiersWithAmmo * NumArrowsPerSoldierForEachLog;
        }
    }
}
