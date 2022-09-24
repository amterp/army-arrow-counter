using System;
using System.IO;
using System.Xml;
using TaleWorlds.Library;

namespace ArmyArrowCounter {
    class Config {
        private static readonly string COUNTER_TYPE_XML_NAME = "CounterType";
        private static readonly string PREFIX_XML_NAME = "Prefix";
        private static readonly string CONFIG_FILE_SUB_PATH = "Modules/ArmyArrowCounter/config/config.xml";
        private static readonly string CONFIG_FILE_FULL_PATH = BasePath.Name + CONFIG_FILE_SUB_PATH;
        private static readonly CounterType DEFAULT_COUNTER_TYPE = CounterType.EXACT_FRACTION;
        private static readonly string DEFAULT_PREFIX = "Army arrows: ";

        private static Config _Config = null;

        public CounterType CounterType { get; private set; }
        public string Prefix { get; private set; }

        public static Config Instance() {
            if (_Config == null) {
                Load();
            }

            return _Config;
        }

        private Config() {
            this.CounterType = DEFAULT_COUNTER_TYPE;
            this.Prefix = DEFAULT_PREFIX;
        }

        private static void Load() {
            _Config = new Config();

            XmlDocument doc = new XmlDocument();
            try {
                doc.Load(CONFIG_FILE_FULL_PATH);
            } catch (FileNotFoundException e) {
                Utils.LogWithColor(Utils.RED, "AAC ERROR: Expected to find config file located at '{0}', but could not. Using default config.", CONFIG_FILE_SUB_PATH);
                return;
            } catch (XmlException e) {
                Utils.LogWithColor(Utils.RED, "AAC ERROR: Invalid config file '{0}'. Received exception: {1} Using default config.", CONFIG_FILE_SUB_PATH, e.Message);
                return;
            }

            bool foundCounterType = false;
            bool foundPrefix = false;
            foreach (XmlNode node in doc.DocumentElement) {
                if (node.Name == COUNTER_TYPE_XML_NAME) {
                    foundCounterType = true;
                    try {
                        _Config.CounterType = (CounterType)Enum.Parse(typeof(CounterType), node.InnerText);
                    } catch (ArgumentException) {
                        Utils.LogWithColor(Utils.RED, "AAC ERROR: Invalid {0}: '{1}'. Defaulting to {2}.", COUNTER_TYPE_XML_NAME, node.InnerText, DEFAULT_COUNTER_TYPE);
                    }
                } else if (node.Name == PREFIX_XML_NAME) {
                    foundPrefix = true;
                    _Config.Prefix = node.InnerText;
                }
            }

            if (!foundCounterType) {
                Utils.LogWithColor(Utils.RED, "AAC ERROR: Failed to find '{0}' tag in config file. Defaulting to {1}.", COUNTER_TYPE_XML_NAME, DEFAULT_COUNTER_TYPE);
            }
            if (!foundPrefix) {
                Utils.LogWithColor(Utils.RED, "AAC ERROR: Failed to find '{0}' tag in config file. Defaulting to {1}.", PREFIX_XML_NAME, DEFAULT_PREFIX);
            }
        }
    }

    public enum CounterType {
        EXACT_FRACTION,
        EXACT_PERCENT,
        NEAREST_10_PERCENT,
        NEAREST_20_PERCENT,
        NEAREST_25_PERCENT,
        NEAREST_WRITTEN,
    }
}
