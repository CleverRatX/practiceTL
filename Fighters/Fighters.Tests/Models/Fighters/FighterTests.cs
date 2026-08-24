using Fighters.Models.Fighters;
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
            Fighter fighter = FighterMock.CreateFighter( name: "Боец1" );

            Assert.Equal( "Боец1", fighter.Name );
        }

        [Fact]
        public void Constructor_ValidArguments_SetsCurrentHealthToMaxHealth()
        {
            Fighter fighter = CreateFighterWithHealth( raceHealth: 200, classHealth: 60 );

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
            Assert.Throws<ArgumentException>( () => FighterMock.CreateFighter( name: name! ) );
        }

        [Fact]
        public void Constructor_RaceIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>( () => new Fighter(
                FighterMock.DefaultName,
                null!,
                FighterMock.CreateClassMock().Object,
                FighterMock.CreateWeaponMock().Object,
                FighterMock.CreateArmorMock().Object ) );
        }

        [Fact]
        public void Constructor_FighterClassIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>( () => new Fighter(
                FighterMock.DefaultName,
                FighterMock.CreateRaceMock().Object,
                null!,
                FighterMock.CreateWeaponMock().Object,
                FighterMock.CreateArmorMock().Object ) );
        }

        [Fact]
        public void Constructor_WeaponIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>( () => new Fighter(
                FighterMock.DefaultName,
                FighterMock.CreateRaceMock().Object,
                FighterMock.CreateClassMock().Object,
                null!,
                FighterMock.CreateArmorMock().Object ) );
        }

        [Fact]
        public void Constructor_ArmorIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>( () => new Fighter(
                FighterMock.DefaultName,
                FighterMock.CreateRaceMock().Object,
                FighterMock.CreateClassMock().Object,
                FighterMock.CreateWeaponMock().Object,
                null! ) );
        }

        [Fact]
        public void RaceName_Always_IsTakenFromRace()
        {
            Fighter fighter = FighterMock.CreateFighter( race: FighterMock.CreateRaceMock( name: "Орк" ).Object );

            Assert.Equal( "Орк", fighter.RaceName );
        }

        [Fact]
        public void ClassName_Always_IsTakenFromFighterClass()
        {
            Fighter fighter = FighterMock.CreateFighter( fighterClass: FighterMock.CreateClassMock( name: "Рыцарь" ).Object );

            Assert.Equal( "Рыцарь", fighter.ClassName );
        }

        [Fact]
        public void WeaponName_Always_IsTakenFromWeapon()
        {
            Fighter fighter = FighterMock.CreateFighter( weapon: FighterMock.CreateWeaponMock( name: "Меч" ).Object );

            Assert.Equal( "Меч", fighter.WeaponName );
        }

        [Fact]
        public void ArmorName_Always_IsTakenFromArmor()
        {
            Fighter fighter = FighterMock.CreateFighter( armor: FighterMock.CreateArmorMock( name: "Кольчужная броня" ).Object );

            Assert.Equal( "Кольчужная броня", fighter.ArmorName );
        }

        [Theory]
        [InlineData( 220, 60, 280 )]
        [InlineData( 190, 15, 205 )]
        [InlineData( 100, 0, 100 )]
        public void MaxHealth_Always_IsSumOfRaceAndClassHealth( int raceHealth, int classHealth, int expected )
        {
            Fighter fighter = CreateFighterWithHealth( raceHealth, classHealth );

            Assert.Equal( expected, fighter.MaxHealth );
        }

        [Theory]
        [InlineData( 26, 10, 20, 56 )]
        [InlineData( 33, 12, 8, 53 )]
        [InlineData( 0, 0, 0, 0 )]
        public void Damage_Always_IsSumOfRaceClassAndWeaponDamage( int raceDamage, int classDamage, int weaponDamage, int expected )
        {
            Fighter fighter = FighterMock.CreateFighter(
                race: FighterMock.CreateRaceMock( damage: raceDamage ).Object,
                fighterClass: FighterMock.CreateClassMock( damage: classDamage ).Object,
                weapon: FighterMock.CreateWeaponMock( damage: weaponDamage ).Object );

            Assert.Equal( expected, fighter.Damage );
        }

        [Theory]
        [InlineData( 8, 10, 18 )]
        [InlineData( 3, 0, 3 )]
        [InlineData( 0, 0, 0 )]
        public void Armor_Always_IsSumOfRaceAndArmorValues( int raceArmor, int armorValue, int expected )
        {
            Fighter fighter = FighterMock.CreateFighter(
                race: FighterMock.CreateRaceMock( armor: raceArmor ).Object,
                armor: FighterMock.CreateArmorMock( armor: armorValue ).Object );

            Assert.Equal( expected, fighter.Armor );
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
            Fighter fighter = FighterMock.CreateFighter(
                race: FighterMock.CreateRaceMock( initiative: raceInitiative ).Object,
                fighterClass: FighterMock.CreateClassMock( initiative: classInitiative ).Object,
                armor: FighterMock.CreateArmorMock( initiative: armorInitiative ).Object );

            Assert.Equal( expected, fighter.Initiative );
        }

        [Theory]
        [InlineData( 0.12f, 0.15f, 0.05f, 0.32 )]
        [InlineData( 0f, 0.10f, -0.05f, 0.05 )]
        [InlineData( 0f, 0f, 0f, 0 )]
        public void CritChance_SumIsInsideAllowedRange_ReturnsSum(
            float classCritChance,
            float weaponCritChance,
            float armorCritChance,
            double expected )
        {
            Fighter fighter = CreateFighterWithCritChances( classCritChance, weaponCritChance, armorCritChance );

            Assert.Equal( expected, fighter.CritChance, FloatPrecision );
        }

        [Fact]
        public void CritChance_SumIsNegative_ReturnsZero()
        {
            Fighter fighter = CreateFighterWithCritChances( classCritChance: 0f, weaponCritChance: 0f, armorCritChance: -0.05f );

            Assert.Equal( 0d, fighter.CritChance, FloatPrecision );
        }

        [Fact]
        public void CritChance_SumIsGreaterThanOne_ReturnsOne()
        {
            Fighter fighter = CreateFighterWithCritChances( classCritChance: 0.6f, weaponCritChance: 0.6f, armorCritChance: 0.2f );

            Assert.Equal( 1d, fighter.CritChance, FloatPrecision );
        }

        [Fact]
        public void CritChance_SumIsExactlyOne_ReturnsOne()
        {
            Fighter fighter = CreateFighterWithCritChances( classCritChance: 0.5f, weaponCritChance: 0.5f, armorCritChance: 0f );

            Assert.Equal( 1d, fighter.CritChance, FloatPrecision );
        }

        [Theory]
        [InlineData( 0.30f, 0.50f, 1.80 )]
        [InlineData( 0f, 0f, 1.00 )]
        [InlineData( 0.10f, 0.80f, 1.90 )]
        public void CritDamageMultiplier_Always_IsOnePlusClassAndWeaponCritDamage(
            float classCritDamage,
            float weaponCritDamage,
            double expected )
        {
            Fighter fighter = FighterMock.CreateFighter(
                fighterClass: FighterMock.CreateClassMock( critDamage: classCritDamage ).Object,
                weapon: FighterMock.CreateWeaponMock( critDamage: weaponCritDamage ).Object );

            Assert.Equal( expected, fighter.CritDamageMultiplier, FloatPrecision );
        }

        [Fact]
        public void TakeDamage_DamageIsLessThanCurrentHealth_ReducesCurrentHealth()
        {
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );

            fighter.TakeDamage( 30 );

            Assert.Equal( 70, fighter.CurrentHealth );
            Assert.True( fighter.IsAlive );
        }

        [Fact]
        public void TakeDamage_CalledSeveralTimes_AccumulatesDamage()
        {
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );

            fighter.TakeDamage( 30 );
            fighter.TakeDamage( 25 );
            fighter.TakeDamage( 5 );

            Assert.Equal( 40, fighter.CurrentHealth );
        }

        [Fact]
        public void TakeDamage_DamageEqualsCurrentHealth_HealthBecomesZeroAndFighterDies()
        {
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );

            fighter.TakeDamage( 100 );

            Assert.Equal( 0, fighter.CurrentHealth );
            Assert.False( fighter.IsAlive );
        }

        [Fact]
        public void TakeDamage_DamageIsGreaterThanCurrentHealth_HealthIsNotNegative()
        {
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );

            fighter.TakeDamage( 1000 );

            Assert.Equal( 0, fighter.CurrentHealth );
            Assert.False( fighter.IsAlive );
        }

        [Theory]
        [InlineData( 0 )]
        [InlineData( -1 )]
        [InlineData( -500 )]
        public void TakeDamage_DamageIsNotPositive_HealthDoesNotChange( int damage )
        {
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );

            fighter.TakeDamage( damage );

            Assert.Equal( 100, fighter.CurrentHealth );
        }

        [Fact]
        public void TakeDamage_FighterIsAlreadyDead_HealthStaysZero()
        {
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );
            fighter.TakeDamage( 100 );

            fighter.TakeDamage( 50 );

            Assert.Equal( 0, fighter.CurrentHealth );
            Assert.False( fighter.IsAlive );
        }

        [Fact]
        public void IsAlive_HealthIsOne_ReturnsTrue()
        {
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );

            fighter.TakeDamage( 99 );

            Assert.Equal( 1, fighter.CurrentHealth );
            Assert.True( fighter.IsAlive );
        }

        [Fact]
        public void RestoreHealth_AfterDamage_RestoresHealthToMaximum()
        {
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );
            fighter.TakeDamage( 60 );

            fighter.RestoreHealth();

            Assert.Equal( fighter.MaxHealth, fighter.CurrentHealth );
        }

        [Fact]
        public void RestoreHealth_FighterIsDead_ReturnsHimToLife()
        {
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );
            fighter.TakeDamage( 150 );

            fighter.RestoreHealth();

            Assert.Equal( 100, fighter.CurrentHealth );
            Assert.True( fighter.IsAlive );
        }

        [Fact]
        public void RestoreHealth_HealthIsFull_DoesNotChangeAnything()
        {
            Fighter fighter = CreateFighterWithHealth( raceHealth: 90, classHealth: 10 );

            fighter.RestoreHealth();

            Assert.Equal( 100, fighter.CurrentHealth );
        }

        private static Fighter CreateFighterWithHealth( int raceHealth, int classHealth )
        {
            return FighterMock.CreateFighter(
                race: FighterMock.CreateRaceMock( health: raceHealth ).Object,
                fighterClass: FighterMock.CreateClassMock( health: classHealth ).Object );
        }

        private static Fighter CreateFighterWithCritChances( float classCritChance, float weaponCritChance, float armorCritChance )
        {
            return FighterMock.CreateFighter(
                fighterClass: FighterMock.CreateClassMock( critChance: classCritChance ).Object,
                weapon: FighterMock.CreateWeaponMock( critChance: weaponCritChance ).Object,
                armor: FighterMock.CreateArmorMock( critChance: armorCritChance ).Object );
        }
    }
}
