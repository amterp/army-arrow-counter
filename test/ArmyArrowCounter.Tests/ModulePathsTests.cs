using System.IO;
using Xunit;

namespace ArmyArrowCounter.Tests {
    public class ModulePathsTests {
        [Fact]
        public void FindsTheConfigInANexusStyleInstall() {
            string dll = Join("C:", "Bannerlord", "Modules", "ArmyArrowCounter",
                              "bin", "Win64_Shipping_Client", "ArmyArrowCounter.dll");

            Assert.Equal(Join("C:", "Bannerlord", "Modules", "ArmyArrowCounter", "config", "config.xml"),
                ModulePaths.ConfigFilePathFor(dll));
        }

        [Fact]
        public void FindsTheConfigInAWorkshopInstallOutsideTheGameFolder() {
            // The reason the old hardcoded Modules/ path needed a Steam build flag.
            string dll = Join("C:", "steamapps", "workshop", "content", "261550", "2875690459",
                              "bin", "Win64_Shipping_Client", "ArmyArrowCounter.dll");

            Assert.Equal(Join("C:", "steamapps", "workshop", "content", "261550", "2875690459",
                              "config", "config.xml"),
                ModulePaths.ConfigFilePathFor(dll));
        }

        private static string Join(params string[] parts) {
            return Path.GetFullPath(Path.Combine(parts));
        }
    }
}
