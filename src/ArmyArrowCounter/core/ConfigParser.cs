using System;
using System.Collections.Generic;
using System.Xml;

namespace ArmyArrowCounter {
    /** Turns config XML into settings plus a list of complaints.
     *
     *  Every malformed field falls back to its default rather than failing the
     *  load, because a mod that refuses to start is worse than one that ignores
     *  a typo. Malformed XML is the exception: it throws, since there is nothing
     *  left to read.
     */
    static class ConfigParser {
        internal static readonly CounterType DEFAULT_COUNTER_TYPE = CounterType.EXACT_FRACTION;
        internal static readonly string DEFAULT_PREFIX = "Army arrows: ";

        private static readonly string COUNTER_TYPE_XML_NAME = "CounterType";
        private static readonly string PREFIX_XML_NAME = "Prefix";

        /** Throws XmlException if the document cannot be read at all. */
        internal static ParsedConfig Parse(string xml) {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            CounterType counterType = DEFAULT_COUNTER_TYPE;
            string prefix = DEFAULT_PREFIX;
            List<string> warnings = new List<string>();
            bool foundCounterType = false;
            bool foundPrefix = false;

            foreach (XmlNode node in doc.DocumentElement) {
                if (node.Name == COUNTER_TYPE_XML_NAME) {
                    foundCounterType = true;
                    try {
                        counterType = (CounterType)Enum.Parse(typeof(CounterType), node.InnerText);
                    } catch (ArgumentException) {
                        warnings.Add(String.Format(
                            "Invalid {0}: '{1}'. The counter will display as {2} instead. Valid values are listed in the comment beside the tag.",
                            COUNTER_TYPE_XML_NAME, node.InnerText, DEFAULT_COUNTER_TYPE));
                    }
                } else if (node.Name == PREFIX_XML_NAME) {
                    foundPrefix = true;
                    prefix = node.InnerText;
                }
            }

            if (!foundCounterType) {
                warnings.Add(String.Format(
                    "No '{0}' tag in the config file. The counter will display as {1}. Add the tag to choose a different style.",
                    COUNTER_TYPE_XML_NAME, DEFAULT_COUNTER_TYPE));
            }
            if (!foundPrefix) {
                warnings.Add(String.Format(
                    "No '{0}' tag in the config file. The counter will be labelled '{1}'. Add the tag to change it.",
                    PREFIX_XML_NAME, DEFAULT_PREFIX));
            }

            return new ParsedConfig(counterType, prefix, warnings);
        }
    }
}
