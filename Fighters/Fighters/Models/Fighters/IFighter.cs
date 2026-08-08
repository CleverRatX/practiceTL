using Fighters.Models.Armors;
using Fighters.Models.Weapons;

namespace Fighters.Models.Fighters
{
    public interface IFighter
    {
        string Name { get; }

        public int GetMaxHealth();
        public int GetCurrentHealth();
        public int GetBasicDamage();
        public int GetArmor();
        public int GetInitiative();
        public float GetCritChance();
        public float GetCritDamageMultiplier();

        public void SetArmor( IArmor armor );
        public void SetWeapon( IWeapon weapon );

        public void TakeDamage( int damage );
    }
}
