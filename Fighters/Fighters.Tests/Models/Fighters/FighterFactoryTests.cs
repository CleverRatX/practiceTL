using Fighters.Models.Armors;
using Fighters.Models.FighterClasses;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Fighters.Tests.Common;
using Xunit;

namespace Fighters.Tests.Models.Fighters
{
    public class FighterFactoryTests
    {
        private readonly FighterFactory _factory = new();

        [Fact]
        public void Create_Always_BuildsFighterFromPassedDependencies()
        {
            // Arrange
            IRace race = FighterMock.CreateRace( name: "Орк", health: 200, damage: 30, armor: 5, initiative: 6 ).Object;
            IFighterClass fighterClass = FighterMock.CreateClass( name: "Рыцарь", health: 60, damage: 10, initiative: 10 ).Object;
            IWeapon weapon = FighterMock.CreateWeapon( name: "Меч", damage: 25 ).Object;
            IArmor armor = FighterMock.CreateArmor( name: "Латы", armor: 12, initiative: -4 ).Object;

            // Act
            IFighter fighter = Create( race: race, fighterClass: fighterClass, weapon: weapon, armor: armor );

            // Assert
            Assert.Equal( "Орк", fighter.RaceName );
            Assert.Equal( "Рыцарь", fighter.ClassName );
            Assert.Equal( "Меч", fighter.WeaponName );
            Assert.Equal( "Латы", fighter.ArmorName );
            Assert.Equal( 260, fighter.MaxHealth );
            Assert.Equal( 65, fighter.Damage );
            Assert.Equal( 17, fighter.Armor );
            Assert.Equal( 12, fighter.Initiative );
        }

        [Fact]
        public void Create_CalledTwiceWithSameArguments_ReturnsIndependentFighters()
        {
            // Arrange
            IRace race = FighterMock.CreateRace( health: 100 ).Object;
            IFighterClass fighterClass = FighterMock.CreateClass( health: 0 ).Object;

            // Act
            IFighter first = Create( race: race, fighterClass: fighterClass );
            IFighter second = Create( race: race, fighterClass: fighterClass );
            first.TakeDamage( 40 );

            // Assert
            Assert.NotSame( first, second );
            Assert.Equal( 60, first.CurrentHealth );
            Assert.Equal( 100, second.CurrentHealth );
        }

        [Theory]
        [InlineData( null )]
        [InlineData( "" )]
        [InlineData( "   " )]
        public void Create_NameIsNullOrWhiteSpace_ThrowsArgumentException( string? name )
        {
            // Act & Assert
            Assert.Throws<ArgumentException>( () => Create( name: name! ) );
        }

        private IFighter Create(
            string name = FighterMock.DefaultName,
            IRace? race = null,
            IFighterClass? fighterClass = null,
            IWeapon? weapon = null,
            IArmor? armor = null )
        {
            return _factory.Create(
                name,
                race ?? FighterMock.CreateRace().Object,
                fighterClass ?? FighterMock.CreateClass().Object,
                weapon ?? FighterMock.CreateWeapon().Object,
                armor ?? FighterMock.CreateArmor().Object );
        }
    }
}
