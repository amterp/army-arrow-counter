using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace ArmyArrowCounter
{
    class ArrowTracker
    {
        internal int RemainingArrows { get; private set; }
        internal int MaxArrows { get; private set; }

        internal void AddToRemainingArrows(int deltaRemainingArrows)
        {
            RemainingArrows += deltaRemainingArrows;
        }

        internal void AddToMaxArrows(int deltaMaxArrows)
        {
            MaxArrows += deltaMaxArrows;
        }

        internal bool AddAgent(Agent agent)
        {
            short spawnAmmo = CalculateMaxAmmo(agent);
            AddToRemainingArrows(spawnAmmo);
            AddToMaxArrows(spawnAmmo);
            return spawnAmmo != 0;
        }

        internal bool RemoveAgent(Agent agent)
        {
            short remainingAmmo = CalculateRemainingAmmo(agent);
            short maxAmmo = CalculateMaxAmmo(agent);
            AddToRemainingArrows(-remainingAmmo);
            AddToMaxArrows(-maxAmmo);
            return maxAmmo != 0;
        }

        private static short CalculateRemainingAmmo(Agent agent)
        {
            MissionWeapon weaponFromSlot0 = agent.Equipment[EquipmentIndex.Weapon0];
            short ammoFromSlot0 = weaponFromSlot0.Equals(MissionWeapon.Invalid) || weaponFromSlot0.IsShield() ? (short)0 : weaponFromSlot0.Amount;
            MissionWeapon weaponFromSlot1 = agent.Equipment[EquipmentIndex.Weapon1];
            short ammoFromSlot1 = weaponFromSlot1.Equals(MissionWeapon.Invalid) || weaponFromSlot1.IsShield() ? (short)0 : weaponFromSlot1.Amount;
            MissionWeapon weaponFromSlot2 = agent.Equipment[EquipmentIndex.Weapon2];
            short ammoFromSlot2 = weaponFromSlot2.Equals(MissionWeapon.Invalid) || weaponFromSlot2.IsShield() ? (short)0 : weaponFromSlot2.Amount;
            MissionWeapon weaponFromSlot3 = agent.Equipment[EquipmentIndex.Weapon3];
            short ammoFromSlot3 = weaponFromSlot3.Equals(MissionWeapon.Invalid) || weaponFromSlot3.IsShield() ? (short)0 : weaponFromSlot3.Amount;
            MissionWeapon weaponFromSlot4 = agent.Equipment[EquipmentIndex.Weapon4];
            short ammoFromSlot4 = weaponFromSlot4.Equals(MissionWeapon.Invalid) || weaponFromSlot4.IsShield() ? (short)0 : weaponFromSlot4.Amount;

            return (short)(ammoFromSlot0 + ammoFromSlot1 + ammoFromSlot2 + ammoFromSlot3 + ammoFromSlot4);
        }

        private static short CalculateMaxAmmo(Agent agent)
        {
            short ammoFromSlot0 = agent.SpawnEquipment[EquipmentIndex.Weapon0].Ammo;
            short ammoFromSlot1 = agent.SpawnEquipment[EquipmentIndex.Weapon1].Ammo;
            short ammoFromSlot2 = agent.SpawnEquipment[EquipmentIndex.Weapon2].Ammo;
            short ammoFromSlot3 = agent.SpawnEquipment[EquipmentIndex.Weapon3].Ammo;
            short ammoFromSlot4 = agent.SpawnEquipment[EquipmentIndex.Weapon4].Ammo;

            return (short)(ammoFromSlot0 + ammoFromSlot1 + ammoFromSlot2 + ammoFromSlot3 + ammoFromSlot4);
        }
    }
}
