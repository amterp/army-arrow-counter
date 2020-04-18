using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TaleWorlds.Library;

namespace ArmyArrowCounter
{
    class AacVmFactory
    {
        public static ViewModel Create(ArrowCounter arrowCounter)
        {
            switch(Config.Instance().CounterType)
            {
                case CounterType.NEAREST_WRITTEN:
                    return new NearestWrittenVM(arrowCounter);
                case CounterType.EXACT_PERCENT:
                    return new ExactPercentVM(arrowCounter);
                case CounterType.NEAREST_10_PERCENT:
                    return new NearestXPercentVM(arrowCounter, 10);
                case CounterType.NEAREST_20_PERCENT:
                    return new NearestXPercentVM(arrowCounter, 20);
                case CounterType.NEAREST_25_PERCENT:
                    return new NearestXPercentVM(arrowCounter, 25);
                case CounterType.EXACT_FRACTION:
                default:
                    return new ExactFractionVM(arrowCounter);
            }
        }
    }
}
