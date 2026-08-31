using Fighters.Fight;
using Fighters.Models.Fighters;
using Fighters.Tests.Common;
using Fighters.Utils;
using Moq;
using Xunit;

namespace Fighters.Tests.Fight
{
    public class FightManagerTests
    {
        private readonly Mock<IAttackManager> _attackManager = new();
        private readonly Mock<IRandomProvider> _random = new();

        public FightManagerTests()
        {
            _attackManager
                .Setup( manager => manager.Attack( It.IsAny<IFighter>(), It.IsAny<IFighter>() ) )
                .Returns( new AttackResult( 10, 10, false ) );
        }

        [Fact]
        public void Constructor_AttackManagerIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>( () => new FightManager( null!, _random.Object ) );
        }

        [Fact]
        public void Constructor_RandomIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>( () => new FightManager( _attackManager.Object, null! ) );
        }

        [Fact]
        public void Fight_FightersIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            FightManager fightManager = CreateFightManager();

            // Act & Assert
            Assert.Throws<ArgumentNullException>( () => fightManager.Fight( null! ) );
        }

        [Fact]
        public void Fight_ThereAreNoFighters_ThrowsArgumentException()
        {
            // Arrange
            FightManager fightManager = CreateFightManager();

            // Act & Assert
            Assert.Throws<ArgumentException>( () => fightManager.Fight( new List<IFighter>() ) );
        }

        [Fact]
        public void Fight_ThereIsOnlyOneFighter_ThrowsArgumentException()
        {
            // Arrange
            FightManager fightManager = CreateFightManager();
            List<IFighter> fighters = CreateFighters( FighterMock.CreateFighter() );

            // Act & Assert
            Assert.Throws<ArgumentException>( () => fightManager.Fight( fighters ) );
        }

        [Fact]
        public void Fight_Always_LetsFighterWithHigherInitiativeAttackFirst()
        {
            // Arrange
            List<string> attackingNames = new();
            Mock<IFighter> fast = FighterMock.CreateFighter( name: "Быстрый", initiative: 20 );
            Mock<IFighter> slow = FighterMock.CreateFighter( name: "Медленный", initiative: 5 );

            _attackManager
                .Setup( manager => manager.Attack( It.IsAny<IFighter>(), It.IsAny<IFighter>() ) )
                .Callback<IFighter, IFighter>( ( attacking, attacked ) => attackingNames.Add( attacking.Name ) )
                .Returns( new AttackResult( 10, 10, false ) );

            // Act
            CreateFightManager().Fight( CreateFighters( slow, fast ) );

            // Assert
            Assert.Equal( "Быстрый", attackingNames[ 0 ] );
            Assert.Equal( "Медленный", attackingNames[ 1 ] );
        }

        [Fact]
        public void Fight_Always_DoesNotChangeOrderOfPassedCollection()
        {
            // Arrange
            Mock<IFighter> slow = FighterMock.CreateFighter( name: "Медленный", initiative: 1 );
            Mock<IFighter> fast = FighterMock.CreateFighter( name: "Быстрый", initiative: 50 );
            List<IFighter> fighters = CreateFighters( slow, fast );

            // Act
            CreateFightManager().Fight( fighters );

            // Assert
            Assert.Same( slow.Object, fighters[ 0 ] );
            Assert.Same( fast.Object, fighters[ 1 ] );
        }

        [Fact]
        public void Fight_EveryFighterIsAlive_LetsEachOfThemAttackOncePerRound()
        {
            // Arrange
            Mock<IFighter> first = FighterMock.CreateFighter( name: "Первый", initiative: 20 );
            Mock<IFighter> second = FighterMock.CreateFighter( name: "Второй", initiative: 10 );

            // Act
            FightReport report = CreateFightManager().Fight( CreateFighters( first, second ) );

            // Assert
            Assert.Equal( 2, report.Rounds[ 0 ].Attacks.Count );
        }

        [Fact]
        public void Fight_OnlyOneFighterSurvives_ReturnsHimAsWinner()
        {
            // Arrange
            bool isLoserAlive = true;
            Mock<IFighter> winner = FighterMock.CreateFighter( name: "Победитель", initiative: 20 );
            Mock<IFighter> loser = FighterMock.CreateFighter( name: "Проигравший", initiative: 5 );
            loser.SetupGet( fighter => fighter.IsAlive ).Returns( () => isLoserAlive );

            SetupKillingAttack( winner, loser, () => isLoserAlive = false );

            // Act
            FightReport report = CreateFightManager().Fight( CreateFighters( winner, loser ) );

            // Assert
            Assert.Same( winner.Object, report.Winner );
            Assert.Single( report.Rounds );
        }

        [Fact]
        public void Fight_NobodyDies_StopsAfterMaximumRoundsCountAndReturnsNoWinner()
        {
            // Arrange
            Mock<IFighter> first = FighterMock.CreateFighter( name: "Первый", initiative: 20 );
            Mock<IFighter> second = FighterMock.CreateFighter( name: "Второй", initiative: 10 );

            // Act
            FightReport report = CreateFightManager().Fight( CreateFighters( first, second ) );

            // Assert
            Assert.Equal( FightManager.MaxRoundsCount, report.Rounds.Count );
            Assert.Null( report.Winner );
        }

        [Fact]
        public void Fight_Always_NumbersRoundsStartingFromOne()
        {
            // Arrange
            Mock<IFighter> first = FighterMock.CreateFighter( name: "Первый", initiative: 20 );
            Mock<IFighter> second = FighterMock.CreateFighter( name: "Второй", initiative: 10 );

            // Act
            FightReport report = CreateFightManager().Fight( CreateFighters( first, second ) );

            // Assert
            for ( int i = 0; i < report.Rounds.Count; i++ )
            {
                Assert.Equal( i + 1, report.Rounds[ i ].Number );
            }
        }

        [Fact]
        public void Fight_AllFightersAreDead_ReturnsNoWinnerAndPlaysNoRounds()
        {
            // Arrange
            Mock<IFighter> first = FighterMock.CreateFighter( name: "Первый", isAlive: false );
            Mock<IFighter> second = FighterMock.CreateFighter( name: "Второй", isAlive: false );

            // Act
            FightReport report = CreateFightManager().Fight( CreateFighters( first, second ) );

            // Assert
            Assert.Empty( report.Rounds );
            Assert.Null( report.Winner );
            _attackManager.Verify( manager => manager.Attack( It.IsAny<IFighter>(), It.IsAny<IFighter>() ), Times.Never );
        }

        [Fact]
        public void Fight_FighterIsDeadFromTheStart_NeitherAttacksNorIsAttacked()
        {
            // Arrange
            Mock<IFighter> first = FighterMock.CreateFighter( name: "Первый", initiative: 30 );
            Mock<IFighter> second = FighterMock.CreateFighter( name: "Второй", initiative: 20 );
            Mock<IFighter> dead = FighterMock.CreateFighter( name: "Мёртвый", initiative: 10, isAlive: false );

            // Act
            CreateFightManager().Fight( CreateFighters( first, second, dead ) );

            // Assert
            _attackManager.Verify( manager => manager.Attack( dead.Object, It.IsAny<IFighter>() ), Times.Never );
            _attackManager.Verify( manager => manager.Attack( It.IsAny<IFighter>(), dead.Object ), Times.Never );
        }

        [Fact]
        public void Fight_FighterDiesInTheMiddleOfRound_DoesNotAttackInThisRound()
        {
            // Arrange
            bool isVictimAlive = true;
            Mock<IFighter> attacker = FighterMock.CreateFighter( name: "Атакующий", initiative: 30 );
            Mock<IFighter> victim = FighterMock.CreateFighter( name: "Жертва", initiative: 20 );
            Mock<IFighter> witness = FighterMock.CreateFighter( name: "Свидетель", initiative: 10 );
            victim.SetupGet( fighter => fighter.IsAlive ).Returns( () => isVictimAlive );

            SetupKillingAttack( attacker, victim, () => isVictimAlive = false );

            // Act
            FightReport report = CreateFightManager().Fight( CreateFighters( attacker, victim, witness ) );

            // Assert
            RoundReport firstRound = report.Rounds[ 0 ];

            Assert.Equal( 2, firstRound.Attacks.Count );
            Assert.DoesNotContain( firstRound.Attacks, attack => attack.AttackingName == "Жертва" );
        }

        [Fact]
        public void Fight_Always_ChoosesTargetIndexWithRandomProvider()
        {
            // Arrange
            Mock<IFighter> first = FighterMock.CreateFighter( name: "Первый", initiative: 30 );
            Mock<IFighter> second = FighterMock.CreateFighter( name: "Второй", initiative: 20 );
            Mock<IFighter> third = FighterMock.CreateFighter( name: "Третий", initiative: 10 );

            // Act
            CreateFightManager().Fight( CreateFighters( first, second, third ) );

            // Assert
            _random.Verify( provider => provider.Next( 0, 2 ), Times.AtLeastOnce );
        }

        [Fact]
        public void Fight_RandomReturnsTargetIndex_AttacksFighterAtThisIndex()
        {
            // Arrange
            Mock<IFighter> first = FighterMock.CreateFighter( name: "Первый", initiative: 30 );
            Mock<IFighter> second = FighterMock.CreateFighter( name: "Второй", initiative: 20 );
            Mock<IFighter> third = FighterMock.CreateFighter( name: "Третий", initiative: 10 );

            _random.Setup( provider => provider.Next( 0, 2 ) ).Returns( 1 );

            // Act
            CreateFightManager().Fight( CreateFighters( first, second, third ) );

            // Assert
            _attackManager.Verify( manager => manager.Attack( first.Object, third.Object ), Times.AtLeastOnce );
            _attackManager.Verify( manager => manager.Attack( first.Object, second.Object ), Times.Never );
        }

        [Fact]
        public void Fight_Always_FillsAttackReportFromAttackResultAndAttackedHealth()
        {
            // Arrange
            bool isLoserAlive = true;
            Mock<IFighter> winner = FighterMock.CreateFighter( name: "Боец1", initiative: 20 );
            Mock<IFighter> loser = FighterMock.CreateFighter( name: "Боец2", initiative: 5, currentHealth: 0 );
            loser.SetupGet( fighter => fighter.IsAlive ).Returns( () => isLoserAlive );

            _attackManager
                .Setup( manager => manager.Attack( winner.Object, loser.Object ) )
                .Callback( () => isLoserAlive = false )
                .Returns( new AttackResult( 50, 30, true ) );

            // Act
            FightReport report = CreateFightManager().Fight( CreateFighters( winner, loser ) );

            // Assert
            RoundReport round = Assert.Single( report.Rounds );
            AttackReport attack = Assert.Single( round.Attacks );

            Assert.Equal( "Боец1", attack.AttackingName );
            Assert.Equal( "Боец2", attack.AttackedName );
            Assert.Equal( 50, attack.RawDamage );
            Assert.Equal( 30, attack.DealtDamage );
            Assert.True( attack.IsCritical );
            Assert.Equal( 0, attack.AttackedHealthLeft );
            Assert.True( attack.IsAttackedDefeated );
        }

        [Fact]
        public void Fight_AllTargetsDiedBeforeAttackerMove_RoundHasNoAttacks()
        {
            // Arrange
            Mock<IFighter> attacker = FighterMock.CreateFighter( name: "Одинокий", initiative: 10 );
            Mock<IFighter> target = FighterMock.CreateFighterWithAliveSequence( "Исчезающий", 5, true );

            // Act
            FightReport report = CreateFightManager().Fight( CreateFighters( attacker, target ) );

            // Assert
            RoundReport round = Assert.Single( report.Rounds );

            Assert.Empty( round.Attacks );
            Assert.Same( attacker.Object, report.Winner );
            _attackManager.Verify( manager => manager.Attack( It.IsAny<IFighter>(), It.IsAny<IFighter>() ), Times.Never );
        }

        private FightManager CreateFightManager()
        {
            return new FightManager( _attackManager.Object, _random.Object );
        }

        private void SetupKillingAttack( Mock<IFighter> attacking, Mock<IFighter> attacked, Action kill )
        {
            _attackManager
                .Setup( manager => manager.Attack( attacking.Object, attacked.Object ) )
                .Callback( kill )
                .Returns( new AttackResult( 100, 100, false ) );
        }

        private static List<IFighter> CreateFighters( params Mock<IFighter>[] fighters )
        {
            return fighters
                .Select( fighter => fighter.Object )
                .ToList();
        }
    }
}
