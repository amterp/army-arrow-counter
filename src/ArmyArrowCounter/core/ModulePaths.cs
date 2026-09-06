using System.IO;

namespace ArmyArrowCounter {
    /** Locates the mod's own files without knowing where the module is installed.
     *
     *  A Nexus install lives under Modules/ArmyArrowCounter and a Steam Workshop
     *  install lives under the workshop content directory, so an absolute path
     *  built from the game root only works for one of them.
     */
    static class ModulePaths {
        /** Given <moduleRoot>/bin/<platform>/ArmyArrowCounter.dll, finds the config file. */
        internal static string ConfigFilePathFor(string modAssemblyPath) {
            string platformBinDir = Path.GetDirectoryName(modAssemblyPath);
            string moduleRoot = Path.GetFullPath(Path.Combine(platformBinDir, "..", ".."));
            return Path.Combine(moduleRoot, "config", "config.xml");
        }
    }
}
