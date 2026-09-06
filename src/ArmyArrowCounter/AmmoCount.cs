using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace ArmyArrowCounter {
    /** How much ammunition a set of equipment holds, and how much it could hold.
     *
     *  Takes MissionEquipment rather than Agent so the arithmetic can be exercised
     *  against the real game assemblies without an engine; Agent is native-backed and
     *  cannot be constructed outside a running mission.
     */
    static class AmmoCount {
        // Weapon0..Weapon3 plus the extra slot are the only ones that can hold ammo.
        private static readonly EquipmentIndex[] AmmoBearingSlots = {
            EquipmentIndex.Weapon0,
            EquipmentIndex.Weapon1,
            EquipmentIndex.Weapon2,
            EquipmentIndex.Weapon3,
            EquipmentIndex.ExtraWeaponSlot,
        };

        /** Both totals are asked of the same slots, and both are answered by the game.
         *
         *  Asking per slot matters: the game attributes a bow's arrow capacity to the bow's
         *  slot and reports nothing for the quiver holding them, so summing over every slot
         *  is what makes the two totals describe the same arrows.
         */
        internal static int Max(MissionEquipment equipment) {
            return Total(equipment, (eq, slot) => eq.GetMaxAmmo(slot));
        }

        internal static int Remaining(MissionEquipment equipment) {
            return Total(equipment, (eq, slot) => eq.GetAmmoAmount(slot));
        }

        private static int Total(MissionEquipment equipment, System.Func<MissionEquipment, EquipmentIndex, int> ammoInSlot) {
            int total = 0;
            foreach (EquipmentIndex slot in AmmoBearingSlots) {
                MissionWeapon weapon = equipment[slot];
                // IsEmpty first: it is the game's own null check, and IsShield reads a list
                // that an empty slot does not have. Both game calls below assume an occupied
                // slot and dereference without checking.
                if (weapon.IsEmpty || weapon.IsShield()) {
                    continue;
                }
                total += ammoInSlot(equipment, slot);
            }
            return total;
        }
    }
}
