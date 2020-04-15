using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyArrowCounter
{
    class Config
    {
        public CounterType CounterType { get; private set; }

        public Config(CounterType counterType)
        {
            CounterType = counterType;
        }
    }

    public enum CounterType
    {
        EXACT_FRACTION,
        //NEAREST_100_FRACTION,
        EXACT_PERCENT,
        //NEAREST_10_PERCENT,
        //NEAREST_20_PERCENT,
        //NEAREST_25_PERCENT,
        NEAREST_WRITTEN
    }
}
