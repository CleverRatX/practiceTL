using Fighters.Models.Armors;
using Fighters.Models.FighterClasses;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Moq;
using Moq.Language;

namespace Fighters.Tests.Common
{
    public static class FighterMock
    {
        public const string DefaultName = "Боец";

        public static Mock<IRace> CreateRace(
            string name = "Раса",
            int health = 100,
            int damage = 10,
            int armor = 5,
            int initiative = 10 )
        {
            Mock<IRace> race = new();
            race.SetupGet( item => item.Name ).Returns( name );
            race.SetupGet( item => item.Health ).Returns( health );
            race.SetupGet( item => item.Damage ).Returns( damage );
            race.SetupGet( item => item.Armor ).Returns( armor );
            race.SetupGet( item => item.Initiative ).Returns( initiative );

            return race;
        }

        public static Mock<IFighterClass> CreateClass(
            string name = "Класс",
            int health = 20,
            int damage = 5,
            int initiative = 3,
            float critChance = 0.1f,
            float critDamage = 0.2f )
        {
            Mock<IFighterClass> fighterClass = new();
            fighterClass.SetupGet( item => item.Name ).Returns( name );
            fighterClass.SetupGet( item => item.Health ).Returns( health );
            fighterClass.SetupGet( item => item.Damage ).Returns( damage );
            fighterClass.SetupGet( item => item.Initiative ).Returns( initiative );
            fighterClass.SetupGet( item => item.CritChance ).Returns( critChance );
            fighterClass.SetupGet( item => item.CritDamage ).Returns( critDamage );

            return fighterClass;
        }

        public static Mock<IWeapon> CreateWeapon(
            string name = "Оружие",
            int damage = 7,
            float critChance = 0.05f,
            float critDamage = 0.3f )
        {
            Mock<IWeapon> weapon = new();
            weapon.SetupGet( item => item.Name ).Returns( name );
            weapon.SetupGet( item => item.Damage ).Returns( damage );
            weapon.SetupGet( item => item.CritChance ).Returns( critChance );
            weapon.SetupGet( item => item.CritDamage ).Returns( critDamage );

            return weapon;
        }

        public static Mock<IArmor> CreateArmor(
            string name = "Броня",
            int armor = 4,
            int initiative = -2,
            float critChance = 0f )
        {
            Mock<IArmor> armorMock = new();
            armorMock.SetupGet( item => item.Name ).Returns( name );
            armorMock.SetupGet( item => item.Armor ).Returns( armor );
            armorMock.SetupGet( item => item.Initiative ).Returns( initiative );
            armorMock.SetupGet( item => item.CritChance ).Returns( critChance );

            return armorMock;
        }

        public static Mock<IFighter> CreateFighter(
            string name = DefaultName,
            int maxHealth = 100,
            int currentHealth = 100,
            int damage = 10,
            int armor = 0,
            int initiative = 0,
            float critChance = 0f,
            float critDamageMultiplier = 1f,
            bool isAlive = true )
        {
            Mock<IFighter> fighter = new();
            fighter.SetupGet( item => item.Name ).Returns( name );
            fighter.SetupGet( item => item.RaceName ).Returns( "Раса" );
            fighter.SetupGet( item => item.ClassName ).Returns( "Класс" );
            fighter.SetupGet( item => item.WeaponName ).Returns( "Оружие" );
            fighter.SetupGet( item => item.ArmorName ).Returns( "Броня" );
            fighter.SetupGet( item => item.MaxHealth ).Returns( maxHealth );
            fighter.SetupGet( item => item.CurrentHealth ).Returns( currentHealth );
            fighter.SetupGet( item => item.Damage ).Returns( damage );
            fighter.SetupGet( item => item.Armor ).Returns( armor );
            fighter.SetupGet( item => item.Initiative ).Returns( initiative );
            fighter.SetupGet( item => item.CritChance ).Returns( critChance );
            fighter.SetupGet( item => item.CritDamageMultiplier ).Returns( critDamageMultiplier );
            fighter.SetupGet( item => item.IsAlive ).Returns( isAlive );

            return fighter;
        }

        public static Mock<IFighter> CreateFighterWithAliveSequence(
            string name,
            int initiative,
            params bool[] aliveValues )
        {
            Mock<IFighter> fighter = new();
            fighter.SetupGet( item => item.Name ).Returns( name );
            fighter.SetupGet( item => item.Initiative ).Returns( initiative );

            ISetupSequentialResult<bool> sequence = fighter.SetupSequence( item => item.IsAlive );

            foreach ( bool isAlive in aliveValues )
            {
                sequence = sequence.Returns( isAlive );
            }

            return fighter;
        }

        public static Fighter CreateRealFighter(
            string name = DefaultName,
            IRace? race = null,
            IFighterClass? fighterClass = null,
            IWeapon? weapon = null,
            IArmor? armor = null )
        {
            return new Fighter(
                name,
                race ?? CreateRace().Object,
                fighterClass ?? CreateClass().Object,
                weapon ?? CreateWeapon().Object,
                armor ?? CreateArmor().Object );
        }
    }
}
