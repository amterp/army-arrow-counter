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
        private static readonly string COUNTER_TYPE_XML_NAME = "CounterType";
        private static readonly string CONFIG_FILE_SUB_PATH = "Modules/ArmyArrowCounter/config/config.xml";
        private static readonly string CONFIG_FILE_FULL_PATH = BasePath.Name + CONFIG_FILE_SUB_PATH;

        private static Config Config;

        public static ViewModel Create(ArrowCounter arrowCounter)
        {
            switch(Config.CounterType)
            {
                case CounterType.EXACT_FRACTION:
                default:
                    return new AacVM(arrowCounter);
            }
        }

        internal static void Initialize()
        {
            CounterType counterType = CounterType.EXACT_FRACTION;

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(CONFIG_FILE_FULL_PATH);
            } catch (FileNotFoundException e)
            {
                Utils.LogWithColor(Utils.RED, "AAC ERROR: Expected to find config file located at '{0}', but could not. Defaulting counter type to {1}.", CONFIG_FILE_FULL_PATH, counterType);
                Config = new Config(counterType);
                return;
            }
            
            bool foundNode = false;
            foreach (XmlNode node in doc.DocumentElement)
            {
                if (node.Name == COUNTER_TYPE_XML_NAME)
                {
                    foundNode = true;
                    try
                    {
                        counterType = (CounterType)Enum.Parse(typeof(CounterType), node.InnerText);
                    } catch (ArgumentException e)
                    {
                        Utils.LogWithColor(Utils.RED, "AAC ERROR: Invalid counter type: '{0}'. Defaulting to {1}.", node.InnerText, counterType);
                    }
                    break;
                }
            }

            if (!foundNode)
            {
                Utils.LogWithColor(Utils.RED, "AAC ERROR: Failed to find '{0}' tag in config file. Defaulting counter type to {1}.", COUNTER_TYPE_XML_NAME, counterType);
            }

            Config = new Config(counterType);
        }
    }
}
