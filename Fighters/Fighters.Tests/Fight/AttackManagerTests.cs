using Fighters.Fight;
using Fighters.Models.Fighters;
using Fighters.Tests.Common;
using Fighters.Utils;
using Moq;
using Xunit;

namespace Fighters.Tests.Fight
{
    public class AttackManagerTests
    {
        private const float AverageScatterRoll = 0.5f;
        private const float NoCritRoll = 0.99f;

        [Fact]
        public void Constructor_RandomIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>( () => new AttackManager( null! ) );
        }

        [Fact]
        public void Attack_AttackingIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, AverageScatterRoll ) );

            // Act & Assert
            Assert.Throws<ArgumentNullException>( () => attackManager.Attack( null!, FighterMock.CreateFighter().Object ) );
        }

        [Fact]
        public void Attack_AttackedIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, AverageScatterRoll ) );

            // Act & Assert
            Assert.Throws<ArgumentNullException>( () => attackManager.Attack( FighterMock.CreateFighter().Object, null! ) );
        }

        [Fact]
        public void Attack_NoCritAndAverageScatter_ReturnsDamageReducedByArmor()
        {
            // Arrange
            Mock<IFighter> attacking = FighterMock.CreateFighter( damage: 100, critChance: 0.1f );
            Mock<IFighter> attacked = FighterMock.CreateFighter( armor: 30 );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, AverageScatterRoll ) );

            // Act
            AttackResult result = attackManager.Attack( attacking.Object, attacked.Object );

            // Assert
            Assert.Equal( 100, result.RawDamage );
            Assert.Equal( 70, result.DealtDamage );
            Assert.False( result.IsCritical );
        }

        [Fact]
        public void Attack_RollIsLessThanCritChance_MultipliesDamageByCritMultiplier()
        {
            // Arrange
            Mock<IFighter> attacking = FighterMock.CreateFighter( damage: 100, critChance: 0.2f, critDamageMultiplier: 2f );
            Mock<IFighter> attacked = FighterMock.CreateFighter( armor: 0 );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( critRoll: 0.1f, scatterRoll: AverageScatterRoll ) );

            // Act
            AttackResult result = attackManager.Attack( attacking.Object, attacked.Object );

            // Assert
            Assert.True( result.IsCritical );
            Assert.Equal( 200, result.RawDamage );
            Assert.Equal( 200, result.DealtDamage );
        }

        [Fact]
        public void Attack_RollEqualsCritChance_IsNotCritical()
        {
            // Arrange
            Mock<IFighter> attacking = FighterMock.CreateFighter( damage: 100, critChance: 0.2f, critDamageMultiplier: 2f );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( critRoll: 0.2f, scatterRoll: AverageScatterRoll ) );

            // Act
            AttackResult result = attackManager.Attack( attacking.Object, FighterMock.CreateFighter().Object );

            // Assert
            Assert.False( result.IsCritical );
            Assert.Equal( 100, result.RawDamage );
        }

        [Fact]
        public void Attack_CritChanceIsZero_IsNeverCriticalEvenOnMinimalRoll()
        {
            // Arrange
            Mock<IFighter> attacking = FighterMock.CreateFighter( damage: 100, critChance: 0f, critDamageMultiplier: 2f );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( critRoll: 0f, scatterRoll: AverageScatterRoll ) );

            // Act
            AttackResult result = attackManager.Attack( attacking.Object, FighterMock.CreateFighter().Object );

            // Assert
            Assert.False( result.IsCritical );
            Assert.Equal( 100, result.RawDamage );
        }

        [Fact]
        public void Attack_CritChanceIsOne_IsAlwaysCritical()
        {
            // Arrange
            Mock<IFighter> attacking = FighterMock.CreateFighter( damage: 100, critChance: 1f, critDamageMultiplier: 2f );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( critRoll: 0.999f, scatterRoll: AverageScatterRoll ) );

            // Act
            AttackResult result = attackManager.Attack( attacking.Object, FighterMock.CreateFighter().Object );

            // Assert
            Assert.True( result.IsCritical );
            Assert.Equal( 200, result.RawDamage );
        }

        [Fact]
        public void Attack_MinimalScatterRoll_UsesMinimalScatterMultiplier()
        {
            // Arrange
            Mock<IFighter> attacking = FighterMock.CreateFighter( damage: 100 );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, scatterRoll: 0f ) );

            // Act
            AttackResult result = attackManager.Attack( attacking.Object, FighterMock.CreateFighter().Object );

            // Assert
            Assert.Equal( 80, result.RawDamage );
        }

        [Fact]
        public void Attack_MaximalScatterRoll_UsesMaximalScatterMultiplier()
        {
            // Arrange
            Mock<IFighter> attacking = FighterMock.CreateFighter( damage: 100 );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, scatterRoll: 1f ) );

            // Act
            AttackResult result = attackManager.Attack( attacking.Object, FighterMock.CreateFighter().Object );

            // Assert
            Assert.Equal( 120, result.RawDamage );
        }

        [Fact]
        public void Attack_DamageIsFractional_TruncatesItToWholeNumber()
        {
            // Arrange
            Mock<IFighter> attacking = FighterMock.CreateFighter( damage: 10 );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, scatterRoll: 0.125f ) );

            // Act
            AttackResult result = attackManager.Attack( attacking.Object, FighterMock.CreateFighter().Object );

            // Assert
            Assert.Equal( 8, result.RawDamage );
        }

        [Fact]
        public void Attack_ArmorIsGreaterThanRawDamage_DealtDamageIsZero()
        {
            // Arrange
            Mock<IFighter> attacking = FighterMock.CreateFighter( damage: 10 );
            Mock<IFighter> attacked = FighterMock.CreateFighter( armor: 50 );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, AverageScatterRoll ) );

            // Act
            AttackResult result = attackManager.Attack( attacking.Object, attacked.Object );

            // Assert
            Assert.Equal( 10, result.RawDamage );
            Assert.Equal( 0, result.DealtDamage );
        }

        [Fact]
        public void Attack_ArmorEqualsRawDamage_DealtDamageIsZero()
        {
            // Arrange
            Mock<IFighter> attacking = FighterMock.CreateFighter( damage: 10 );
            Mock<IFighter> attacked = FighterMock.CreateFighter( armor: 10 );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, AverageScatterRoll ) );

            // Act
            AttackResult result = attackManager.Attack( attacking.Object, attacked.Object );

            // Assert
            Assert.Equal( 0, result.DealtDamage );
        }

        [Fact]
        public void Attack_Always_PassesDealtDamageToAttackedFighterOnce()
        {
            // Arrange
            Mock<IFighter> attacking = FighterMock.CreateFighter( damage: 100 );
            Mock<IFighter> attacked = FighterMock.CreateFighter( armor: 25 );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, AverageScatterRoll ) );

            // Act
            AttackResult result = attackManager.Attack( attacking.Object, attacked.Object );

            // Assert
            attacked.Verify( fighter => fighter.TakeDamage( result.DealtDamage ), Times.Once );
            attacked.Verify( fighter => fighter.TakeDamage( It.IsAny<int>() ), Times.Once );
        }

        [Fact]
        public void Attack_Always_DoesNotDamageAttackingFighter()
        {
            // Arrange
            Mock<IFighter> attacking = FighterMock.CreateFighter( damage: 100 );
            Mock<IFighter> attacked = FighterMock.CreateFighter();
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, AverageScatterRoll ) );

            // Act
            attackManager.Attack( attacking.Object, attacked.Object );

            // Assert
            attacking.Verify( fighter => fighter.TakeDamage( It.IsAny<int>() ), Times.Never );
        }

        [Fact]
        public void Attack_Always_AsksRandomProviderExactlyTwice()
        {
            // Arrange
            Mock<IRandomProvider> random = CreateRandomMock( NoCritRoll, AverageScatterRoll );
            AttackManager attackManager = CreateAttackManager( random );

            // Act
            attackManager.Attack( FighterMock.CreateFighter().Object, FighterMock.CreateFighter().Object );

            // Assert
            random.Verify( provider => provider.NextFloat(), Times.Exactly( 2 ) );
        }

        private static AttackManager CreateAttackManager( Mock<IRandomProvider> random )
        {
            return new AttackManager( random.Object );
        }

        private static Mock<IRandomProvider> CreateRandomMock( float critRoll, float scatterRoll )
        {
            Mock<IRandomProvider> random = new();
            random
                .SetupSequence( provider => provider.NextFloat() )
                .Returns( critRoll )
                .Returns( scatterRoll );

            return random;
        }
    }
}
