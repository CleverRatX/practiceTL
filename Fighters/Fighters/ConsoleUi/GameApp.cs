using Fighters.Fight;
using Fighters.Models;
using Fighters.Models.Armors;
using Fighters.Models.FighterClasses;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.ConsoleUi
{
    public class GameApp
    {
        private const string AddFighterCommand = "add-fighter";
        private const string ListCommand = "list";
        private const string PlayCommand = "play";
        private const string ClearCommand = "clear";
        private const string HelpCommand = "help";
        private const string ExitCommand = "exit";

        private readonly List<IFighter> _fighters = new();

        private readonly IFightManager _fightManager;
        private readonly IFighterFactory _fighterFactory;
        private readonly IConsoleInput _input;
        private readonly IConsole _console;

        public GameApp(
            IFightManager fightManager,
            IFighterFactory fighterFactory,
            IConsoleInput input,
            IConsole console )
        {
            ArgumentNullException.ThrowIfNull( fightManager );
            ArgumentNullException.ThrowIfNull( fighterFactory );
            ArgumentNullException.ThrowIfNull( input );
            ArgumentNullException.ThrowIfNull( console );

            _fightManager = fightManager;
            _fighterFactory = fighterFactory;
            _input = input;
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine( "Добро пожаловать на арену бойцов!" );
            PrintCommands();

            bool isRunning = true;

            while ( isRunning )
            {
                _console.WriteLine();

                string command = _input.ReadString( "Введите команду:" ).ToLowerInvariant();

                switch ( command )
                {
                    case AddFighterCommand:
                        AddFighter();
                        break;
                    case ListCommand:
                        PrintFighters();
                        break;
                    case PlayCommand:
                        Play();
                        break;
                    case ClearCommand:
                        ClearArena();
                        break;
                    case HelpCommand:
                        PrintCommands();
                        break;
                    case ExitCommand:
                        isRunning = false;
                        break;
                    default:
                        _console.WriteLine( $"Неизвестная команда: {command}" );
                        PrintCommands();
                        break;
                }
            }
        }

        private void AddFighter()
        {
            string name = _input.ReadString( "Введите имя персонажа:" );

            IRace race = ChooseOne( "Выберите расу из списка ниже:", FighterCatalog.Races, item => item.Name );
            IFighterClass fighterClass = ChooseOne( "Выберите класс из списка ниже:", FighterCatalog.Classes, item => item.Name );
            IWeapon weapon = ChooseOne( "Выберите оружие из списка ниже:", FighterCatalog.Weapons, item => item.Name );
            IArmor armor = ChooseOne( "Выберите броню из списка ниже:", FighterCatalog.Armors, item => item.Name );

            IFighter fighter = _fighterFactory.Create( name, race, fighterClass, weapon, armor );
            _fighters.Add( fighter );

            _console.WriteLine( "Боец добавлен на арену:" );
            PrintFighter( fighter );
        }

        private T ChooseOne<T>( string title, IReadOnlyList<T> options, Func<T, string> getName )
        {
            List<string> names = options
                .Select( getName )
                .ToList();

            return options[ _input.ChooseOption( title, names ) ];
        }

        private void PrintFighters()
        {
            if ( _fighters.Count == 0 )
            {
                _console.WriteLine( $"На арене пока никого нет, добавьте бойца командой {AddFighterCommand}." );
                return;
            }

            _console.WriteLine( $"Бойцы на арене ({_fighters.Count}):" );

            foreach ( IFighter fighter in _fighters )
            {
                PrintFighter( fighter );
            }
        }

        private void Play()
        {
            if ( _fighters.Count < 2 )
            {
                _console.WriteLine( $"Для боя нужно минимум два бойца, добавьте их командой {AddFighterCommand}." );
                return;
            }

            _console.WriteLine();
            _console.WriteLine( "Бой начинается! Первыми бьют бойцы с большей инициативой." );

            FightReport fightReport = _fightManager.Fight( _fighters );
            PrintFightReport( fightReport );

            RestoreFighters();
        }

        private void RestoreFighters()
        {
            foreach ( IFighter fighter in _fighters )
            {
                fighter.RestoreHealth();
            }

            _console.WriteLine();
            _console.WriteLine( $"Бойцы залечили раны и остались на арене. Команда {ClearCommand} — очистить арену." );
        }

        private void ClearArena()
        {
            _fighters.Clear();

            _console.WriteLine( "Арена очищена, можно набирать новых бойцов." );
        }

        private void PrintFightReport( FightReport fightReport )
        {
            foreach ( RoundReport round in fightReport.Rounds )
            {
                _console.WriteLine();
                _console.WriteLine( $"--- Раунд {round.Number} ---" );

                foreach ( AttackReport attack in round.Attacks )
                {
                    PrintAttack( attack );
                }
            }

            PrintFightOutcome( fightReport );
        }

        private void PrintAttack( AttackReport attack )
        {
            string criticalText = attack.IsCritical ? " Крит!" : string.Empty;
            string blockedText = attack.RawDamage > attack.DealtDamage
                ? $" Броня погасила {attack.RawDamage - attack.DealtDamage}."
                : string.Empty;

            _console.WriteLine( $"{attack.AttackingName} наносит {attack.DealtDamage} урона бойцу {attack.AttackedName}.{criticalText}{blockedText} " +
                $"У бойца {attack.AttackedName} осталось {attack.AttackedHealthLeft} здоровья" );

            if ( attack.IsAttackedDefeated )
            {
                _console.WriteLine( $"{attack.AttackedName} погибает!" );
            }
        }

        private void PrintFightOutcome( FightReport fightReport )
        {
            _console.WriteLine();

            if ( fightReport.Winner is null )
            {
                _console.WriteLine( "Бой окончен. Ничья." );
                return;
            }

            _console.WriteLine( $"Бой окончен. {fightReport.Winner.Name} остаётся последним и побеждает! " +
                $"Здоровья осталось: {fightReport.Winner.CurrentHealth}" );
        }

        private void PrintFighter( IFighter fighter )
        {
            _console.WriteLine( $"  {fighter.Name} - {fighter.RaceName}, {fighter.ClassName}, {fighter.WeaponName}, {fighter.ArmorName}" );
            _console.WriteLine( $"    здоровье {fighter.CurrentHealth}/{fighter.MaxHealth}, урон {fighter.Damage}, броня {fighter.Armor}, " +
                $"инициатива {fighter.Initiative}, шанс крита {fighter.CritChance * 100:0}%" );
        }

        private void PrintCommands()
        {
            _console.WriteLine( "Доступные команды:" );
            _console.WriteLine( $"  {AddFighterCommand} - добавить нового бойца на арену" );
            _console.WriteLine( $"  {ListCommand} - показать бойцов на арене" );
            _console.WriteLine( $"  {PlayCommand} - начать битву" );
            _console.WriteLine( $"  {ClearCommand} - убрать всех бойцов с арены" );
            _console.WriteLine( $"  {HelpCommand} - показать список команд" );
            _console.WriteLine( $"  {ExitCommand} - выйти из игры" );
        }
    }
}
