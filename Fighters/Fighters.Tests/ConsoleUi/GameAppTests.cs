using Fighters.ConsoleUi;
using Fighters.Fight;
using Fighters.Models;
using Fighters.Models.Armors;
using Fighters.Models.FighterClasses;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Fighters.Tests.Common;
using Moq;
using Moq.Language;
using Xunit;

namespace Fighters.Tests.ConsoleUi
{
    public class GameAppTests
    {
        private const string CommandTitle = "Введите команду:";
        private const string NameTitle = "Введите имя персонажа:";
        private const string CommandsHeader = "Доступные команды:";

        private readonly Mock<IFightManager> _fightManager = new();
        private readonly Mock<IFighterFactory> _fighterFactory = new();
        private readonly Mock<IConsoleInput> _input = new();
        private readonly Mock<IConsole> _console = new();

        public GameAppTests()
        {
            SetupFightResult( winner: null );
        }

        [Fact]
        public void Run_ExitCommand_StopsWithoutAnyOtherAction()
        {
            SetupCommands( "exit" );

            CreateGameApp().Run();

            _fighterFactory.VerifyNoOtherCalls();
            _fightManager.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData( "EXIT" )]
        [InlineData( "Exit" )]
        [InlineData( "eXiT" )]
        public void Run_CommandIsInUpperCase_IsStillRecognized( string command )
        {
            SetupCommands( command );

            CreateGameApp().Run();

            _console.Verify( console => console.WriteLine( It.Is<string>( message => message.StartsWith( "Неизвестная команда" ) ) ), Times.Never );
        }

        [Fact]
        public void Run_Always_GreetsUserAndPrintsCommandsAtStart()
        {
            SetupCommands( "exit" );

            CreateGameApp().Run();

            _console.Verify( console => console.WriteLine( "Добро пожаловать на арену бойцов!" ), Times.Once );
            _console.Verify( console => console.WriteLine( CommandsHeader ), Times.Once );
        }

        [Fact]
        public void Run_UnknownCommand_PrintsErrorAndCommandsListAgain()
        {
            SetupCommands( "123", "exit" );

            CreateGameApp().Run();

            _console.Verify( console => console.WriteLine( "Неизвестная команда: 123" ), Times.Once );
            _console.Verify( console => console.WriteLine( CommandsHeader ), Times.Exactly( 2 ) );
        }

        [Fact]
        public void Run_HelpCommand_PrintsCommandsListAgain()
        {
            SetupCommands( "help", "exit" );

            CreateGameApp().Run();

            _console.Verify( console => console.WriteLine( CommandsHeader ), Times.Exactly( 2 ) );
        }

        [Fact]
        public void Run_ListCommandOnEmptyArena_PrintsHintInsteadOfFighters()
        {
            SetupCommands( "list", "exit" );

            CreateGameApp().Run();

            _console.Verify(
                console => console.WriteLine( "На арене пока никого нет, добавьте бойца командой add-fighter." ),
                Times.Once );
        }

        [Fact]
        public void Run_AddFighterCommand_CreatesFighterFromChosenCatalogItems()
        {
            SetupCommands( "add-fighter", "exit" );
            SetupFightersToAdd( "Боец1" );
            SetupChosenOptions( 1, 1, 2, 0 );

            CreateGameApp().Run();

            _fighterFactory.Verify(
                factory => factory.Create(
                    "Боец1",
                    FighterCatalog.Races[ 1 ],
                    FighterCatalog.Classes[ 1 ],
                    FighterCatalog.Weapons[ 2 ],
                    FighterCatalog.Armors[ 0 ] ),
                Times.Once );
        }

        [Fact]
        public void Run_AddFighterCommand_PrintsCreatedFighter()
        {
            SetupCommands( "add-fighter", "exit" );
            SetupFightersToAdd( "Боец1" );

            CreateGameApp().Run();

            _console.Verify( console => console.WriteLine( "Боец добавлен на арену:" ), Times.Once );
            _console.Verify( console => console.WriteLine( It.Is<string>( message => message.Contains( "Боец1" ) ) ), Times.AtLeastOnce );
        }

        [Fact]
        public void Run_ListCommandWithFighters_PrintsAllOfThem()
        {
            SetupCommands( "add-fighter", "add-fighter", "list", "exit" );
            SetupFightersToAdd( "Боец1", "Боец2" );

            CreateGameApp().Run();

            _console.Verify( console => console.WriteLine( "Бойцы на арене (2):" ), Times.Once );
            _console.Verify( console => console.WriteLine( It.Is<string>( message => message.Contains( "Боец2" ) ) ), Times.AtLeastOnce );
        }

        [Fact]
        public void Run_PlayCommandWithoutFighters_DoesNotStartFight()
        {
            SetupCommands( "play", "exit" );

            CreateGameApp().Run();

            _fightManager.Verify( manager => manager.Fight( It.IsAny<IReadOnlyList<IFighter>>() ), Times.Never );
            _console.Verify(
                console => console.WriteLine( "Для боя нужно минимум два бойца, добавьте их командой add-fighter." ),
                Times.Once );
        }

        [Fact]
        public void Run_PlayCommandWithSingleFighter_DoesNotStartFight()
        {
            SetupCommands( "add-fighter", "play", "exit" );
            SetupFightersToAdd( "Боец1" );

            CreateGameApp().Run();

            _fightManager.Verify( manager => manager.Fight( It.IsAny<IReadOnlyList<IFighter>>() ), Times.Never );
        }

        [Fact]
        public void Run_PlayCommandWithTwoFighters_StartsFightWithBothOfThem()
        {
            SetupCommands( "add-fighter", "add-fighter", "play", "exit" );
            List<Mock<IFighter>> fighters = SetupFightersToAdd( "Боец1", "Боец2" );

            CreateGameApp().Run();

            _fightManager.Verify(
                manager => manager.Fight( It.Is<IReadOnlyList<IFighter>>( list =>
                    list.Count == 2
                    && list.Contains( fighters[ 0 ].Object )
                    && list.Contains( fighters[ 1 ].Object ) ) ),
                Times.Once );
        }

        [Fact]
        public void Run_PlayCommand_RestoresHealthOfEveryFighterAfterFight()
        {
            SetupCommands( "add-fighter", "add-fighter", "play", "exit" );
            List<Mock<IFighter>> fighters = SetupFightersToAdd( "Боец1", "Боец2" );

            CreateGameApp().Run();

            foreach ( Mock<IFighter> fighter in fighters )
            {
                fighter.Verify( item => item.RestoreHealth(), Times.Once );
            }
        }

        [Fact]
        public void Run_PlayCommandWithWinner_PrintsHim()
        {
            SetupCommands( "add-fighter", "add-fighter", "play", "exit" );
            List<Mock<IFighter>> fighters = SetupFightersToAdd( "Боец1", "Боец2" );
            SetupFightResult( winner: fighters[ 0 ].Object );

            CreateGameApp().Run();

            _console.Verify(
                console => console.WriteLine( It.Is<string>( message => message.Contains( "Боец1" ) && message.Contains( "побеждает" ) ) ),
                Times.Once );
        }

        [Fact]
        public void Run_PlayCommandWithoutWinner_PrintsDraw()
        {
            SetupCommands( "add-fighter", "add-fighter", "play", "exit" );
            SetupFightersToAdd( "Боец1", "Боец2" );
            SetupFightResult( winner: null );

            CreateGameApp().Run();

            _console.Verify( console => console.WriteLine( "Бой окончен. Ничья." ), Times.Once );
        }

        [Fact]
        public void Run_PlayCommand_PrintsEveryRoundWithItsAttacks()
        {
            SetupCommands( "add-fighter", "add-fighter", "play", "exit" );
            SetupFightersToAdd( "Боец1", "Боец2" );
            SetupFightResult(
                winner: null,
                CreateRound( 1, CreateAttack( dealtDamage: 30, rawDamage: 30, attackedHealthLeft: 70 ) ),
                CreateRound( 2, CreateAttack( dealtDamage: 40, rawDamage: 40, attackedHealthLeft: 30 ) ) );

            CreateGameApp().Run();

            _console.Verify( console => console.WriteLine( "--- Раунд 1 ---" ), Times.Once );
            _console.Verify( console => console.WriteLine( "--- Раунд 2 ---" ), Times.Once );
            _console.Verify(
                console => console.WriteLine( "Боец1 наносит 30 урона бойцу Боец2. У бойца Боец2 осталось 70 здоровья" ),
                Times.Once );
        }

        [Fact]
        public void Run_PlayCommandAndFighterIsDefeated_PrintsDeathMessage()
        {
            SetupCommands( "add-fighter", "add-fighter", "play", "exit" );
            SetupFightersToAdd( "Боец1", "Боец2" );
            SetupFightResult( winner: null, CreateRound( 1, CreateAttack( attackedHealthLeft: 0 ) ) );

            CreateGameApp().Run();

            _console.Verify( console => console.WriteLine( "Боец2 погибает!" ), Times.Once );
        }

        [Fact]
        public void Run_PlayCommandAndFighterSurvives_DoesNotPrintDeathMessage()
        {
            SetupCommands( "add-fighter", "add-fighter", "play", "exit" );
            SetupFightersToAdd( "Боец1", "Боец2" );
            SetupFightResult( winner: null, CreateRound( 1, CreateAttack( attackedHealthLeft: 1 ) ) );

            CreateGameApp().Run();

            _console.Verify( console => console.WriteLine( "Боец2 погибает!" ), Times.Never );
        }

        [Fact]
        public void Run_PlayCommandAndArmorAbsorbedPartOfDamage_PrintsBlockedAmount()
        {
            SetupCommands( "add-fighter", "add-fighter", "play", "exit" );
            SetupFightersToAdd( "Боец1", "Боец2" );
            SetupFightResult( winner: null, CreateRound( 1, CreateAttack( rawDamage: 50, dealtDamage: 30 ) ) );

            CreateGameApp().Run();

            _console.Verify(
                console => console.WriteLine( It.Is<string>( message => message.Contains( "Броня погасила 20." ) ) ),
                Times.Once );
        }

        [Fact]
        public void Run_PlayCommandAndAttackIsCritical_PrintsCritMark()
        {
            SetupCommands( "add-fighter", "add-fighter", "play", "exit" );
            SetupFightersToAdd( "Боец1", "Боец2" );
            SetupFightResult( winner: null, CreateRound( 1, CreateAttack( isCritical: true ) ) );

            CreateGameApp().Run();

            _console.Verify(
                console => console.WriteLine( It.Is<string>( message => message.Contains( "Крит!" ) ) ),
                Times.Once );
        }

        [Fact]
        public void Run_ClearCommand_RemovesAllFightersFromArena()
        {
            SetupCommands( "add-fighter", "add-fighter", "clear", "play", "exit" );
            SetupFightersToAdd( "Боец1", "Боец2" );

            CreateGameApp().Run();

            _console.Verify( console => console.WriteLine( "Арена очищена, можно набирать новых бойцов." ), Times.Once );
            _fightManager.Verify( manager => manager.Fight( It.IsAny<IReadOnlyList<IFighter>>() ), Times.Never );
        }

        private GameApp CreateGameApp()
        {
            return new GameApp( _fightManager.Object, _fighterFactory.Object, _input.Object, _console.Object );
        }

        private void SetupCommands( params string[] commands )
        {
            ISetupSequentialResult<string> sequence = _input.SetupSequence( input => input.ReadString( CommandTitle ) );

            foreach ( string command in commands )
            {
                sequence = sequence.Returns( command );
            }
        }

        private void SetupChosenOptions( params int[] options )
        {
            ISetupSequentialResult<int> sequence = _input.SetupSequence(
                input => input.ChooseOption( It.IsAny<string>(), It.IsAny<IReadOnlyList<string>>() ) );

            foreach ( int option in options )
            {
                sequence = sequence.Returns( option );
            }
        }

        private List<Mock<IFighter>> SetupFightersToAdd( params string[] names )
        {
            ISetupSequentialResult<string> nameSequence = _input.SetupSequence( input => input.ReadString( NameTitle ) );
            List<Mock<IFighter>> fighters = new();

            foreach ( string name in names )
            {
                nameSequence = nameSequence.Returns( name );

                Mock<IFighter> fighter = FighterMock.CreateFighterMock( name: name );
                fighters.Add( fighter );

                _fighterFactory
                    .Setup( factory => factory.Create(
                        name,
                        It.IsAny<IRace>(),
                        It.IsAny<IFighterClass>(),
                        It.IsAny<IWeapon>(),
                        It.IsAny<IArmor>() ) )
                    .Returns( fighter.Object );
            }

            return fighters;
        }

        private void SetupFightResult( IFighter? winner, params RoundReport[] rounds )
        {
            _fightManager
                .Setup( manager => manager.Fight( It.IsAny<IReadOnlyList<IFighter>>() ) )
                .Returns( new FightReport( rounds, winner ) );
        }

        private static RoundReport CreateRound( int number, params AttackReport[] attacks )
        {
            return new RoundReport( number, attacks );
        }

        private static AttackReport CreateAttack(
            string attackingName = "Боец1",
            string attackedName = "Боец2",
            int rawDamage = 30,
            int dealtDamage = 30,
            bool isCritical = false,
            int attackedHealthLeft = 70 )
        {
            return new AttackReport( attackingName, attackedName, rawDamage, dealtDamage, isCritical, attackedHealthLeft );
        }
    }
}
