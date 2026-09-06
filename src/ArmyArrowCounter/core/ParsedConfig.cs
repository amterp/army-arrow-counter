using System.Collections.Generic;

namespace ArmyArrowCounter {
    class ParsedConfig {
        public CounterType CounterType { get; }
        public string Prefix { get; }

        /** Problems worth telling the player about. Each one already names what was
         *  wrong and what was used instead, so a caller can log it as-is.
         */
        public IReadOnlyList<string> Warnings { get; }

        public ParsedConfig(CounterType counterType, string prefix, IReadOnlyList<string> warnings) {
            CounterType = counterType;
            Prefix = prefix;
            Warnings = warnings;
        }
    }
}
