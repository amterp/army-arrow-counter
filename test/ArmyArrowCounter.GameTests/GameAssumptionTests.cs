using Xunit;

namespace ArmyArrowCounter.GameTests {
    // These run against the INSTALLED game assemblies, with no engine, no window and no save,
    // in about a second. They pin down the assumptions the mod makes about game types, which is
    // exactly the class of thing that changes silently under a game update.
    //
    // Test bodies delegate to Probe so that no TaleWorlds type is resolved until after the
    // constructor has installed the assembly resolver.
    public class GameAssumptionTests {
        public GameAssumptionTests() {
            Skip.IfNot(GameAssemblies.Available, "Bannerlord not found at " + GameAssemblies.Bin + " - set BANNERLORD_BIN to override");
            GameAssemblies.Hook();
        }

        [SkippableFact]
        public void AnEmptySlotDoesNotCompareEqualToMissionWeaponInvalid() {
            // The v1.8.0 crash, in one assertion: the old guard assumed this was true.
            Assert.False(Probe.EmptySlotEqualsInvalid());
        }

        [SkippableFact]
        public void AnEmptySlotIsReportedEmptyByTheGame() {
            // ...and this is the check that actually works, which the fix switched to.
            Assert.True(Probe.EmptySlotIsEmpty());
        }

        [SkippableFact]
        public void IsShieldIsSafeToCallOnAnEmptySlot() {
            // IsShield() reads _weapons.Count, so the emptiness check must short-circuit ahead of it.
            Assert.False(Probe.EmptySlotIsShield());
        }
    }
}
