using Fighters.Models.Armors;
using Fighters.Models.FighterClasses;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Fighters.Tests.Common;
using Xunit;

namespace Fighters.Tests.Models.Fighters
{
    public class FighterTests
    {
        private const int FloatPrecision = 4;

        [Fact]
        public void Constructor_ValidArguments_SetsName()
        {
            // Arrange & Act
            Fighter fighter = FighterMock.CreateRealFighter( name: "Боец1" );

            // Assert
            Assert.Equal( "Боец1", fighter.Name );
        }

        [Fact]
        public void Constructor_ValidArguments_SetsCurrentHealthToMaxHealth()
        {
            // Arrange & Act
            Fighter fighter = CreateFighterWithHealth( raceHealth: 200, classHealth: 60 );

            // Assert
            Assert.Equal( 260, fighter.MaxHealth );
            Assert.Equal( 260, fighter.CurrentHealth );
        }

        [Theory]
        [InlineData( null )]
        [InlineData( "" )]
        [InlineData( " " )]
        [InlineData( "    " )]
        public void Constructor_NameIsNullOrWhiteSpace_ThrowsArgumentException( string? name )
        {
            // Act & Assert
            Assert.Throws<ArgumentException>( () => FighterMock.CreateRealFighter( name: name! ) );
        }

        [Theory]
        [InlineData( true, false, false, false )]
        [InlineData( false, true, false, false )]
        [InlineData( false, false, true, false )]
        [InlineData( false, false, false, true )]
        public void Constructor_OneOfDependenciesIsNull_ThrowsArgumentNullException(
            bool isRaceNull,
            bool isClassNull,
            bool isWeaponNull,
            bool isArmorNull )
        {
            // Arrange
            IRace? race = isRaceNull ? null : FighterMock.CreateRace().Object;
            IFighterClass? fighterClass = isClassNull ? null : FighterMock.CreateClass().Object;
            IWeapon? weapon = isWeaponNull ? null : FighterMock.CreateWeapon().Object;
            IArmor? armor = isArmorNull ? null : FighterMock.CreateArmor().Object;

            // Act & Assert
            Assert.Throws<ArgumentNullException>( () => new Fighter(
                FighterMock.DefaultName,
                race!,
                fighterClass!,
                weapon!,
                armor! ) );
        }

        [Fact]
        public void RaceName_Always_IsTakenFromRace()
        {
            // Arrange
            Fighter fighter = FighterMock.CreateRealFighter( race: FighterMock.CreateRace( name: "Орк" ).Object );

            // Act
            string raceName = fighter.RaceName;

            // Assert
            Assert.Equal( "Орк", raceName );
        }

        [Fact]
        public void ClassName_Always_IsTakenFromFighterClass()
        {
            // Arrange
            Fighter fighter = FighterMock.CreateRealFighter( fighterClass: FighterMock.CreateClass( name: "Рыцарь" ).Object );

            // Act
            string className = fighter.ClassName;

            // Assert
            Assert.Equal( "Рыцарь", className );
        }

        [Fact]
        public void WeaponName_Always_IsTakenFromWeapon()
        {
            // Arrange
            Fighter fighter = FighterMock.CreateRealFighter( weapon: FighterMock.CreateWeapon( name: "Меч" ).Object );

            // Act
            string weaponName = fighter.WeaponName;

            // Assert
            Assert.Equal( "Меч", weaponName );
        }

        [Fact]
        public void ArmorName_Always_IsTakenFromArmor()
        {
            // Arrange
            Fighter fighter = FighterMock.CreateRealFighter( armor: FighterMock.CreateArmor( name: "Кольчужная броня" ).Object );

            // Act
            string armorName = fighter.ArmorName;

            // Assert
            Assert.Equal( "Кольчужная броня", armorName );
        }

        [Theory]
        [InlineData( 220, 60, 280 )]
        [InlineData( 190, 15, 205 )]
        [InlineData( 190, -15, 175 )]
        [InlineData( 100, 0, 100 )]
        public void MaxHealth_SumIsPositive_IsSumOfRaceAndClassHealth( int raceHealth, int classHealth, int expected )
        {
            // Arrange
            Fighter fighter = CreateFighterWithHealth( raceHealth, classHealth );

            // Act
            int maxHealth = fighter.MaxHealth;

            // Assert
            Assert.Equal( expected, maxHealth );
        }

        [Theory]
        [InlineData( 0, 0 )]
        [InlineData( 100, -100 )]
        [InlineData( 100, -500 )]
        [InlineData( -50, -50 )]
        public void MaxHealth_SumIsNotPositive_ReturnsMinHealth( int raceHealth, int classHealth )
        {
            // Arrange
            Fighter fighter = CreateFighterWithHealth( raceHealth, classHealth );

            // Act
            int maxHealth = fighter.MaxHealth;

            // Assert
            Assert.Equal( Fighter.MinHealth, maxHealth );
        }

        [Fact]
        public void Constructor_RaceAndClassHealthSumIsNotPositive_FighterIsStillAlive()
        {
            // Arrange & Act
            Fighter fighter = CreateFighterWithHealth( raceHealth: 10, classHealth: -100 );

            // Assert
            Assert.Equal( Fighter.MinHealth, fighter.CurrentHealth );
            Assert.True( fighter.IsAlive );
        }

        [Theory]
        [InlineData( 26, 10, 20, 56 )]
        [InlineData( 33, 12, 8, 53 )]
        [InlineData( 26, -10, 20, 36 )]
        [InlineData( 0, 0, 0, 0 )]
        public void Damage_Always_IsSumOfRaceClassAndWeaponDamage( int raceDamage, int classDamage, int weaponDamage, int expected )
        {
            // Arrange
            Fighter fighter = FighterMock.CreateRealFighter(
                race: FighterMock.CreateRace( damage: raceDamage ).Object,
                fighterClass: FighterMock.CreateClass( damage: classDamage ).Object,
                weapon: FighterMock.CreateWeapon( damage: weaponDamage ).Object );

            // Act
            int damage = fighter.Damage;

            // Assert
            Assert.Equal( expected, damage );
        }

        [Theory]
        [InlineData( 8, 10, 18 )]
        [InlineData( 3, 0, 3 )]
        [InlineData( 8, -3, 5 )]
        [InlineData( 0, 0, 0 )]
        public void Armor_Always_IsSumOfRaceAndArmorValues( int raceArmor, int armorValue, int expected )
        {
            // Arrange
            Fighter fighter = FighterMock.CreateRealFighter(
                race: FighterMock.CreateRace( armor: raceArmor ).Object,
                armor: FighterMock.CreateArmor( armor: armorValue ).Object );

            // Act
            int armor = fighter.Armor;

            // Assert
            Assert.Equal( expected, armor );
        }

        [Theory]
        [InlineData( 14, 10, 8, 32 )]
        [InlineData( 6, 0, -4, 2 )]
        [InlineData( 2, 0, -10, -8 )]
        public void Initiative_Always_IsSumOfRaceClassAndArmorInitiative(
            int raceInitiative,
            int classInitiative,
            int armorInitiative,
            int expected )
        {
            // Arrange
            Fighter fighter = FighterMock.CreateRealFighter(
                race: FighterMock.CreateRace( initiative: raceInitiative ).Object,
                fighterClass: FighterMock.CreateClass( initiative: classInitiative ).Object,
                armor: FighterMock.CreateArmor( initiative: armorInitiative ).Object );

            // Act
            int initiative = fighter.Initiative;

            // Assert
            Assert.Equal( expected, initiative );
        }

        [Theory]
        [InlineData( 0.12f, 0.15f, 0.05f, 0.32f )]
        [InlineData( 0f, 0.10f, -0.05f, 0.05f )]
        [InlineData( 0f, 0f, 0f, 0f )]
        public void CritChance_SumIsInsideAllowedRange_ReturnsSum(
            float classCritChance,
            float weaponCritChance,
            float armorCritChance,
            float expected )
        {
            // Arrange
            Fighter fighter = CreateFighterWithCritChances( classCritChance, weaponCritChance, armorCritChance );

            // Act
            float critChance = fighter.CritChance;

            // Assert
            Assert.Equal( expected, critChance, FloatPrecision );
        }

        [Fact]
        public void CritChance_SumIsNegative_ReturnsZero()
        {
            // Arrange
            Fighter fighter = CreateFighterWithCritChances( classCritChance: 0f, weaponCritChance: 0f, armorCritChance: -0.05f );

            // Act
            float critChance = fighter.CritChance;

            // Assert
            Assert.Equal( 0f, critChance, FloatPrecision );
        }

        [Fact]
        public void CritChance_SumIsGreaterThanOne_ReturnsOne()
        {
            // Arrange
            Fighter fighter = CreateFighterWithCritChances( classCritChance: 0.6f, weaponCritChance: 0.6f, armorCritChance: 0.2f );

            // Act
            float critChance = fighter.CritChance;

            // Assert
            Assert.Equal( 1f, critChance, FloatPrecision );
        }

        [Fact]
        public void CritChance_SumIsExactlyOne_ReturnsOne()
        {
            // Arrange
            Fighter fighter = CreateFighterWithCritChances( classCritChance: 0.5f, weaponCritChance: 0.5f, armorCritChance: 0f );

            // Act
            float critChance = fighter.CritChance;

            // Assert
            Assert.Equal( 1f, critChance, FloatPrecision );
        }

        [Theory]
        [InlineData( 0.30f, 0.50f, 1.80f )]
        [InlineData( 0f, 0f, 1.00f )]
        [InlineData( 0.10f, 0.80f, 1.90f )]
        public void CritDamageMultiplier_Always_IsOnePlusClassAndWeaponCritDamage(
            float classCritDamage,
            float weaponCritDamage,
            float expected )
        {
            // Arrange
            Fighter fighter = FighterMock.CreateRealFighter(
                fighterClass: FighterMock.CreateClass( critDamage: classCritDamage ).Object,
                weapon: FighterMock.CreateWeapon( critDamage: weaponCritDamage ).Object );

            // Act
            float critDamageMultiplier = fighter.CritDamageMultiplier;

            // Assert
            Assert.Equal( expected, critDamageMultiplier, FloatPrecision );
        }

        [Fact]
        public void TakeDamage_DamageIsLessThanCurrentHealth_ReducesCurrentHealth()
        {
            // Arrange
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );

            // Act
            fighter.TakeDamage( 30 );

            // Assert
            Assert.Equal( 70, fighter.CurrentHealth );
            Assert.True( fighter.IsAlive );
        }

        [Fact]
        public void TakeDamage_CalledSeveralTimes_AccumulatesDamage()
        {
            // Arrange
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );

            // Act
            fighter.TakeDamage( 30 );
            fighter.TakeDamage( 25 );
            fighter.TakeDamage( 5 );

            // Assert
            Assert.Equal( 40, fighter.CurrentHealth );
        }

        [Fact]
        public void TakeDamage_DamageEqualsCurrentHealth_HealthBecomesZeroAndFighterDies()
        {
            // Arrange
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );

            // Act
            fighter.TakeDamage( 100 );

            // Assert
            Assert.Equal( 0, fighter.CurrentHealth );
            Assert.False( fighter.IsAlive );
        }

        [Fact]
        public void TakeDamage_DamageIsGreaterThanCurrentHealth_HealthIsNotNegative()
        {
            // Arrange
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );

            // Act
            fighter.TakeDamage( 1000 );

            // Assert
            Assert.Equal( 0, fighter.CurrentHealth );
            Assert.False( fighter.IsAlive );
        }

        [Theory]
        [InlineData( 0 )]
        [InlineData( -1 )]
        [InlineData( -500 )]
        public void TakeDamage_DamageIsNotPositive_HealthDoesNotChange( int damage )
        {
            // Arrange
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );

            // Act
            fighter.TakeDamage( damage );

            // Assert
            Assert.Equal( 100, fighter.CurrentHealth );
        }

        [Fact]
        public void TakeDamage_FighterIsAlreadyDead_HealthStaysZero()
        {
            // Arrange
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );
            fighter.TakeDamage( 100 );

            // Act
            fighter.TakeDamage( 50 );

            // Assert
            Assert.Equal( 0, fighter.CurrentHealth );
            Assert.False( fighter.IsAlive );
        }

        [Fact]
        public void IsAlive_HealthIsOne_ReturnsTrue()
        {
            // Arrange
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );

            // Act
            fighter.TakeDamage( 99 );

            // Assert
            Assert.Equal( 1, fighter.CurrentHealth );
            Assert.True( fighter.IsAlive );
        }

        [Fact]
        public void RestoreHealth_AfterDamage_RestoresHealthToMaximum()
        {
            // Arrange
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );
            fighter.TakeDamage( 60 );

            // Act
            fighter.RestoreHealth();

            // Assert
            Assert.Equal( fighter.MaxHealth, fighter.CurrentHealth );
        }

        [Fact]
        public void RestoreHealth_FighterIsDead_ReturnsHimToLife()
        {
            // Arrange
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );
            fighter.TakeDamage( 150 );

            // Act
            fighter.RestoreHealth();

            // Assert
            Assert.Equal( 100, fighter.CurrentHealth );
            Assert.True( fighter.IsAlive );
        }

        [Fact]
        public void RestoreHealth_HealthIsFull_DoesNotChangeAnything()
        {
            // Arrange
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );

            // Act
            fighter.RestoreHealth();

            // Assert
            Assert.Equal( 100, fighter.CurrentHealth );
        }

        private static Fighter CreateFighterWithHealth( int raceHealth, int classHealth )
        {
            return FighterMock.CreateRealFighter(
                race: FighterMock.CreateRace( health: raceHealth ).Object,
                fighterClass: FighterMock.CreateClass( health: classHealth ).Object );
        }

        private static Fighter CreateFighterWithCritChances( float classCritChance, float weaponCritChance, float armorCritChance )
        {
            return FighterMock.CreateRealFighter(
                fighterClass: FighterMock.CreateClass( critChance: classCritChance ).Object,
                weapon: FighterMock.CreateWeapon( critChance: weaponCritChance ).Object,
                armor: FighterMock.CreateArmor( critChance: armorCritChance ).Object );
        }
    }
}
