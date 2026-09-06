using System.IO;
using System.Reflection;
using System.Xml;

namespace ArmyArrowCounter {
    class Config {
        private static Config _Config = null;

        public CounterType CounterType { get; private set; }
        public string Prefix { get; private set; }

        public static Config Instance() {
            if (_Config == null) {
                _Config = Load();
            }

            return _Config;
        }

        private Config(CounterType counterType, string prefix) {
            CounterType = counterType;
            Prefix = prefix;
        }

        private static Config Defaults() {
            return new Config(ConfigParser.DEFAULT_COUNTER_TYPE, ConfigParser.DEFAULT_PREFIX);
        }

        private static Config Load() {
            string path = ModulePaths.ConfigFilePathFor(Assembly.GetExecutingAssembly().Location);

            if (!File.Exists(path)) {
                Utils.Log("Army Arrow Counter: no config file at '{0}', using default settings.", path);
                return Defaults();
            }

            ParsedConfig parsed;
            try {
                parsed = ConfigParser.Parse(File.ReadAllText(path));
            } catch (XmlException e) {
                Utils.LogWithColor(Utils.RED,
                    "AAC ERROR: Config file '{0}' is not valid XML ({1}). All settings fall back to defaults. "
                    + "Fix or delete the file - an unclosed tag is the usual cause.",
                    path, e.Message);
                return Defaults();
            } catch (IOException e) {
                Utils.LogWithColor(Utils.RED,
                    "AAC ERROR: Could not read config file '{0}' ({1}). All settings fall back to defaults. "
                    + "Check the file's permissions.",
                    path, e.Message);
                return Defaults();
            }

            foreach (string warning in parsed.Warnings) {
                Utils.LogWithColor(Utils.RED, "AAC ERROR: {0}", warning);
            }

            return new Config(parsed.CounterType, parsed.Prefix);
        }
    }
}
