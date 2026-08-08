using Fighters.Models.Armors;
using Fighters.Models.FighterClasses;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.Models.Fighters
{
    public class Fighter : IFighter
    {
        public string Name { get; }
        private readonly IRace _race;
        private readonly IFighterClass _class;
        private IArmor _armor = new NoArmor();
        private IWeapon _weapon = new Fists();
        private int _currentHealth;

        public Fighter( string name, IRace race, IFighterClass fighterClass )
        {
            if ( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ArgumentException( "Имя бойца не может быть пустым.", nameof( name ) );
            }

            Name = name;
            _race = race ?? throw new ArgumentNullException( nameof( race ) );
            _class = fighterClass ?? throw new ArgumentNullException( nameof( fighterClass ) );

            _currentHealth = GetMaxHealth();
        }

        public int GetMaxHealth() => _race.Health + _class.HealthBonus;

        public int GetCurrentHealth() => _currentHealth;

        public int GetBasicDamage() => _weapon.Damage + _race.Strength + _class.DamageBonus;

        public int GetArmor() => _armor.Armor + _race.Armor;

        public int GetInitiative() => _race.Initiative + _class.InitiativeBonus + _armor.InitiativeBonus;

        public float GetCritChance()
        {
            float baseCritChance = _weapon.CritChance + _class.CritChanceBonus + _armor.CritChanceBonus;
            return Math.Clamp( baseCritChance, 0f, 1f );
        }

        public float GetCritDamageMultiplier() => 1 + _weapon.CritDamage + _class.CritDamageBonus;

        public void SetArmor( IArmor armor )
        {
            _armor = armor ?? throw new ArgumentNullException( nameof( armor ) );
        }

        public void SetWeapon( IWeapon weapon )
        {
            _weapon = weapon ?? throw new ArgumentNullException( nameof( weapon ) );
        }

        public void TakeDamage( int damage )
        {
            if ( damage < 0 )
            {
                damage = 0;
            }

            int newHealth = _currentHealth - damage;

            if ( newHealth < 0 )
            {
                newHealth = 0;
            }

            _currentHealth = newHealth;
        }
    }
}
