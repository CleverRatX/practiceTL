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
        private readonly FightManager _fightManager;

        public GameApp( FightManager fightManager )
        {
            _fightManager = fightManager;
        }

        public void Run()
        {
            Console.WriteLine( "Добро пожаловать на арену бойцов!" );
            PrintCommands();

            bool isRunning = true;

            while ( isRunning )
            {
                Console.WriteLine();

                string command = ConsoleInput.ReadString( "Введите команду:" ).ToLowerInvariant();

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
                        Console.WriteLine( $"Неизвестная команда: {command}" );
                        PrintCommands();
                        break;
                }
            }
        }

        private void AddFighter()
        {
            string name = ConsoleInput.ReadString( "Введите имя персонажа:" );

            IRace race = ChooseOne( "Выберите расу из списка ниже:", FighterCatalog.Races, item => item.Name );
            IFighterClass fighterClass = ChooseOne( "Выберите класс из списка ниже:", FighterCatalog.Classes, item => item.Name );
            IWeapon weapon = ChooseOne( "Выберите оружие из списка ниже:", FighterCatalog.Weapons, item => item.Name );
            IArmor armor = ChooseOne( "Выберите броню из списка ниже:", FighterCatalog.Armors, item => item.Name );

            Fighter fighter = new( name, race, fighterClass, weapon, armor );
            _fighters.Add( fighter );

            Console.WriteLine( "Боец добавлен на арену:" );
            PrintFighter( fighter );
        }

        private static T ChooseOne<T>( string title, IReadOnlyList<T> options, Func<T, string> getName )
        {
            List<string> names = options
                .Select( getName )
                .ToList();

            return options[ ConsoleInput.ChooseOption( title, names ) ];
        }

        private void PrintFighters()
        {
            if ( _fighters.Count == 0 )
            {
                Console.WriteLine( $"На арене пока никого нет, добавьте бойца командой {AddFighterCommand}." );
                return;
            }

            Console.WriteLine( $"Бойцы на арене ({_fighters.Count}):" );

            foreach ( IFighter fighter in _fighters )
            {
                PrintFighter( fighter );
            }
        }

        private void Play()
        {
            if ( _fighters.Count < 2 )
            {
                Console.WriteLine( $"Для боя нужно минимум два бойца, добавьте их командой {AddFighterCommand}." );
                return;
            }

            Console.WriteLine();
            Console.WriteLine( "Бой начинается! Первыми бьют бойцы с большей инициативой." );

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

            Console.WriteLine();
            Console.WriteLine( $"Бойцы залечили раны и остались на арене. Команда {ClearCommand} — очистить арену." );
        }

        private void ClearArena()
        {
            _fighters.Clear();

            Console.WriteLine( "Арена очищена, можно набирать новых бойцов." );
        }

        private static void PrintFightReport( FightReport fightReport )
        {
            foreach ( RoundReport round in fightReport.Rounds )
            {
                Console.WriteLine();
                Console.WriteLine( $"--- Раунд {round.Number} ---" );

                foreach ( AttackReport attack in round.Attacks )
                {
                    PrintAttack( attack );
                }
            }

            PrintFightOutcome( fightReport );
        }

        private static void PrintAttack( AttackReport attack )
        {
            string criticalText = attack.IsCritical ? " Крит!" : string.Empty;
            string blockedText = attack.RawDamage > attack.DealtDamage
                ? $" Броня погасила {attack.RawDamage - attack.DealtDamage}."
                : string.Empty;

            Console.WriteLine( $"{attack.AttackingName} наносит {attack.DealtDamage} урона бойцу {attack.AttackedName}.{criticalText}{blockedText} " +
                $"У бойца {attack.AttackedName} осталось {attack.AttackedHealthLeft} здоровья" );

            if ( attack.IsAttackedDefeated )
            {
                Console.WriteLine( $"{attack.AttackedName} погибает!" );
            }
        }

        private static void PrintFightOutcome( FightReport fightReport )
        {
            Console.WriteLine();

            if ( fightReport.Winner is null )
            {
                Console.WriteLine( "Бой окончен. Ничья." );
                return;
            }

            Console.WriteLine( $"Бой окончен. {fightReport.Winner.Name} остаётся последним и побеждает! " +
                $"Здоровья осталось: {fightReport.Winner.CurrentHealth}" );
        }

        private static void PrintFighter( IFighter fighter )
        {
            Console.WriteLine( $"  {fighter.Name} - {fighter.RaceName}, {fighter.ClassName}, {fighter.WeaponName}, {fighter.ArmorName}" );
            Console.WriteLine( $"    здоровье {fighter.CurrentHealth}/{fighter.MaxHealth}, урон {fighter.Damage}, броня {fighter.Armor}, " +
                $"инициатива {fighter.Initiative}, шанс крита {fighter.CritChance * 100:0}%" );
        }

        private static void PrintCommands()
        {
            Console.WriteLine( "Доступные команды:" );
            Console.WriteLine( $"  {AddFighterCommand} - добавить нового бойца на арену" );
            Console.WriteLine( $"  {ListCommand} - показать бойцов на арене" );
            Console.WriteLine( $"  {PlayCommand} - начать битву" );
            Console.WriteLine( $"  {ClearCommand} - убрать всех бойцов с арены" );
            Console.WriteLine( $"  {HelpCommand} - показать список команд" );
            Console.WriteLine( $"  {ExitCommand} - выйти из игры" );
        }
    }
}
