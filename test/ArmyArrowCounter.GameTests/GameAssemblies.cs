using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ArmyArrowCounter.GameTests {
    // The game's managed assemblies are not redistributable and are not copied into the test
    // output; they are resolved in place, out of the installed game. Nothing here touches a
    // TaleWorlds type, so this class can be JITted before the hook is installed.
    static class GameAssemblies {
        private static readonly string[] CommonInstallPaths = {
            @"C:\Program Files (x86)\Steam\steamapps\common\Mount & Blade II Bannerlord",
            @"C:\Program Files\Epic Games\Mount & Blade II Bannerlord",
            @"F:\Program Files (x86)\Steam\steamapps\common\Mount & Blade II Bannerlord",
        };

        private static bool Hooked;

        /** Set BANNERLORD_BIN to the Win64_Shipping_Client directory to point these
         *  somewhere the guesses below do not cover.
         */
        public static string Bin {
            get {
                string configured = Environment.GetEnvironmentVariable("BANNERLORD_BIN");
                if (!string.IsNullOrEmpty(configured)) {
                    return configured;
                }

                return CommonInstallPaths
                    .Select(root => Path.Combine(root, "bin", "Win64_Shipping_Client"))
                    .FirstOrDefault(Directory.Exists) ?? CommonInstallPaths[0];
            }
        }

        public static bool Available => Directory.Exists(Bin);

        public static void Hook() {
            if (Hooked) {
                return;
            }

            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => {
                string file = Path.Combine(Bin, new AssemblyName(args.Name).Name + ".dll");
                return File.Exists(file) ? Assembly.LoadFrom(file) : null;
            };
            Hooked = true;
        }
    }
}
