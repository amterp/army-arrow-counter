using TaleWorlds.MountAndBlade;

namespace ArmyArrowCounter.GameTests {
    // Every touch of a game type lives here, behind a plain bool-returning API.
    static class Probe {
        // How MissionEquipment builds a slot the troop is carrying nothing in.
        private static MissionWeapon EmptySlot() => new MissionWeapon(null, null, null);

        public static bool EmptySlotEqualsInvalid() => EmptySlot().Equals(MissionWeapon.Invalid);
        public static bool EmptySlotIsEmpty() => EmptySlot().IsEmpty;
        public static bool EmptySlotIsShield() => EmptySlot().IsShield();
    }
}
