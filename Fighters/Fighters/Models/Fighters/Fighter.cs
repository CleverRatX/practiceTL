using Fighters.Models.Armors;
using Fighters.Models.FighterClasses;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.Models.Fighters
{
    public class Fighter : IFighter
    {
        public const int MinHealth = 1;

        private readonly IRace _race;
        private readonly IFighterClass _class;
        private readonly IWeapon _weapon;
        private readonly IArmor _armor;

        private int _currentHealth;

        public Fighter( string name, IRace race, IFighterClass fighterClass, IWeapon weapon, IArmor armor )
        {
            if ( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ArgumentException( "Имя бойца не может быть пустым.", nameof( name ) );
            }

            ArgumentNullException.ThrowIfNull( race );
            ArgumentNullException.ThrowIfNull( fighterClass );
            ArgumentNullException.ThrowIfNull( weapon );
            ArgumentNullException.ThrowIfNull( armor );

            Name = name;
            _race = race;
            _class = fighterClass;
            _weapon = weapon;
            _armor = armor;

            _currentHealth = MaxHealth;
        }

        public string Name { get; }

        public string RaceName => _race.Name;

        public string ClassName => _class.Name;

        public string WeaponName => _weapon.Name;

        public string ArmorName => _armor.Name;

        public int MaxHealth => Math.Max( _race.Health + _class.Health, MinHealth );

        public int CurrentHealth => _currentHealth;

        public int Damage => _race.Damage + _class.Damage + _weapon.Damage;

        public int Armor => _race.Armor + _armor.Armor;

        public int Initiative => _race.Initiative + _class.Initiative + _armor.Initiative;

        public float CritChance => Math.Clamp( _class.CritChance + _weapon.CritChance + _armor.CritChance, 0f, 1f );

        public float CritDamageMultiplier => 1f + _class.CritDamage + _weapon.CritDamage;

        public bool IsAlive => _currentHealth > 0;

        public void TakeDamage( int damage )
        {
            if ( damage <= 0 )
            {
                return;
            }

            _currentHealth = Math.Max( _currentHealth - damage, 0 );
        }

        public void RestoreHealth()
        {
            _currentHealth = MaxHealth;
        }
    }
}
