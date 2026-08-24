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
            Assert.Throws<ArgumentNullException>( () => new AttackManager( null! ) );
        }

        [Fact]
        public void Attack_AttackingIsNull_ThrowsArgumentNullException()
        {
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, AverageScatterRoll ) );

            Assert.Throws<ArgumentNullException>( () => attackManager.Attack( null!, FighterMock.CreateFighterMock().Object ) );
        }

        [Fact]
        public void Attack_AttackedIsNull_ThrowsArgumentNullException()
        {
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, AverageScatterRoll ) );

            Assert.Throws<ArgumentNullException>( () => attackManager.Attack( FighterMock.CreateFighterMock().Object, null! ) );
        }

        [Fact]
        public void Attack_NoCritAndAverageScatter_ReturnsDamageReducedByArmor()
        {
            Mock<IFighter> attacking = FighterMock.CreateFighterMock( damage: 100, critChance: 0.1f );
            Mock<IFighter> attacked = FighterMock.CreateFighterMock( armor: 30 );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, AverageScatterRoll ) );

            AttackResult result = attackManager.Attack( attacking.Object, attacked.Object );

            Assert.Equal( 100, result.RawDamage );
            Assert.Equal( 70, result.DealtDamage );
            Assert.False( result.IsCritical );
        }

        [Fact]
        public void Attack_RollIsLessThanCritChance_MultipliesDamageByCritMultiplier()
        {
            Mock<IFighter> attacking = FighterMock.CreateFighterMock( damage: 100, critChance: 0.2f, critDamageMultiplier: 2f );
            Mock<IFighter> attacked = FighterMock.CreateFighterMock( armor: 0 );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( critRoll: 0.1f, scatterRoll: AverageScatterRoll ) );

            AttackResult result = attackManager.Attack( attacking.Object, attacked.Object );

            Assert.True( result.IsCritical );
            Assert.Equal( 200, result.RawDamage );
            Assert.Equal( 200, result.DealtDamage );
        }

        [Fact]
        public void Attack_RollEqualsCritChance_IsNotCritical()
        {
            Mock<IFighter> attacking = FighterMock.CreateFighterMock( damage: 100, critChance: 0.2f, critDamageMultiplier: 2f );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( critRoll: 0.2f, scatterRoll: AverageScatterRoll ) );

            AttackResult result = attackManager.Attack( attacking.Object, FighterMock.CreateFighterMock().Object );

            Assert.False( result.IsCritical );
            Assert.Equal( 100, result.RawDamage );
        }

        [Fact]
        public void Attack_CritChanceIsZero_IsNeverCriticalEvenOnMinimalRoll()
        {
            Mock<IFighter> attacking = FighterMock.CreateFighterMock( damage: 100, critChance: 0f, critDamageMultiplier: 2f );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( critRoll: 0f, scatterRoll: AverageScatterRoll ) );

            AttackResult result = attackManager.Attack( attacking.Object, FighterMock.CreateFighterMock().Object );

            Assert.False( result.IsCritical );
            Assert.Equal( 100, result.RawDamage );
        }

        [Fact]
        public void Attack_CritChanceIsOne_IsAlwaysCritical()
        {
            Mock<IFighter> attacking = FighterMock.CreateFighterMock( damage: 100, critChance: 1f, critDamageMultiplier: 2f );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( critRoll: 0.999f, scatterRoll: AverageScatterRoll ) );

            AttackResult result = attackManager.Attack( attacking.Object, FighterMock.CreateFighterMock().Object );

            Assert.True( result.IsCritical );
            Assert.Equal( 200, result.RawDamage );
        }

        [Fact]
        public void Attack_MinimalScatterRoll_UsesMinimalScatterMultiplier()
        {
            Mock<IFighter> attacking = FighterMock.CreateFighterMock( damage: 100 );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, scatterRoll: 0f ) );

            AttackResult result = attackManager.Attack( attacking.Object, FighterMock.CreateFighterMock().Object );

            Assert.Equal( 80, result.RawDamage );
        }

        [Fact]
        public void Attack_MaximalScatterRoll_UsesMaximalScatterMultiplier()
        {
            Mock<IFighter> attacking = FighterMock.CreateFighterMock( damage: 100 );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, scatterRoll: 1f ) );

            AttackResult result = attackManager.Attack( attacking.Object, FighterMock.CreateFighterMock().Object );

            Assert.Equal( 120, result.RawDamage );
        }

        [Fact]
        public void Attack_DamageIsFractional_TruncatesItToWholeNumber()
        {
            Mock<IFighter> attacking = FighterMock.CreateFighterMock( damage: 10 );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, scatterRoll: 0.125f ) );

            AttackResult result = attackManager.Attack( attacking.Object, FighterMock.CreateFighterMock().Object );

            Assert.Equal( 8, result.RawDamage );
        }

        [Fact]
        public void Attack_ArmorIsGreaterThanRawDamage_DealtDamageIsZero()
        {
            Mock<IFighter> attacking = FighterMock.CreateFighterMock( damage: 10 );
            Mock<IFighter> attacked = FighterMock.CreateFighterMock( armor: 50 );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, AverageScatterRoll ) );

            AttackResult result = attackManager.Attack( attacking.Object, attacked.Object );

            Assert.Equal( 10, result.RawDamage );
            Assert.Equal( 0, result.DealtDamage );
        }

        [Fact]
        public void Attack_ArmorEqualsRawDamage_DealtDamageIsZero()
        {
            Mock<IFighter> attacking = FighterMock.CreateFighterMock( damage: 10 );
            Mock<IFighter> attacked = FighterMock.CreateFighterMock( armor: 10 );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, AverageScatterRoll ) );

            AttackResult result = attackManager.Attack( attacking.Object, attacked.Object );

            Assert.Equal( 0, result.DealtDamage );
        }

        [Fact]
        public void Attack_Always_PassesDealtDamageToAttackedFighterOnce()
        {
            Mock<IFighter> attacking = FighterMock.CreateFighterMock( damage: 100 );
            Mock<IFighter> attacked = FighterMock.CreateFighterMock( armor: 25 );
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, AverageScatterRoll ) );

            AttackResult result = attackManager.Attack( attacking.Object, attacked.Object );

            attacked.Verify( fighter => fighter.TakeDamage( result.DealtDamage ), Times.Once );
            attacked.Verify( fighter => fighter.TakeDamage( It.IsAny<int>() ), Times.Once );
        }

        [Fact]
        public void Attack_Always_DoesNotDamageAttackingFighter()
        {
            Mock<IFighter> attacking = FighterMock.CreateFighterMock( damage: 100 );
            Mock<IFighter> attacked = FighterMock.CreateFighterMock();
            AttackManager attackManager = CreateAttackManager( CreateRandomMock( NoCritRoll, AverageScatterRoll ) );

            attackManager.Attack( attacking.Object, attacked.Object );

            attacking.Verify( fighter => fighter.TakeDamage( It.IsAny<int>() ), Times.Never );
        }

        [Fact]
        public void Attack_Always_AsksRandomProviderExactlyTwice()
        {
            Mock<IRandomProvider> random = CreateRandomMock( NoCritRoll, AverageScatterRoll );
            AttackManager attackManager = CreateAttackManager( random );

            attackManager.Attack( FighterMock.CreateFighterMock().Object, FighterMock.CreateFighterMock().Object );

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
