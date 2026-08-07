using Fighters.Fight;
using Fighters.Models.Armors;
using Fighters.Models.FighterClasses;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.ConsoleUi
{
    public class GameApp
    {
        private static readonly List<IRace> _races = new()
        {
            new Human(),
            new Orc(),
            new Vampire()
        };

        private static readonly List<IFighterClass> _classes = new()
        {
            new Knight(),
            new Mercenary()
        };

        private static readonly List<IWeapon> _weapons = new()
        {
            new Fists(),
            new Sword(),
            new Spear()
        };

        private static readonly List<IArmor> _armors = new()
        {
            new NoArmor(),
            new Chainmail(),
            new Plate()
        };

        private readonly List<IFighter> _fighters = new();
        private readonly FightManager _fightManager;

        public GameApp( FightManager fightManager )
        {
            _fightManager = fightManager ?? throw new ArgumentNullException( nameof( fightManager ) );
        }

        public void Run()
        {
            Console.WriteLine( "Добро пожаловать на арену бойцов!" );
            PrintCommands();

            bool isRunning = true;

            while ( isRunning )
            {
                Console.WriteLine();
                string command = ConsoleInput.ReadNotEmptyString( "Введите команду:" ).ToLowerInvariant();

                switch ( command )
                {
                    case "add-fighter":
                        AddFighter();
                        break;
                    case "list":
                        PrintFighters();
                        break;
                    case "play":
                        Play();
                        break;
                    case "help":
                        PrintCommands();
                        break;
                    case "exit":
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine( $"Неизвестная команда: {command}" );
                        PrintCommands();
                        break;
                }
            }

            Console.WriteLine( "До новых боёв!" );
        }

        private void AddFighter()
        {
            string name = ConsoleInput.ReadNotEmptyString( "Введите имя персонажа:" );

            List<string> raceNames = _races.Select( race => race.Name ).ToList();
            int raceIndex = ConsoleInput.ChooseIndex( "Выберите расу из списка ниже:", raceNames );

            List<string> classNames = _classes.Select( fighterClass => fighterClass.Name ).ToList();
            int classIndex = ConsoleInput.ChooseIndex( "Выберите класс из списка ниже:", classNames );

            List<string> weaponNames = _weapons.Select( weapon => weapon.Name ).ToList();
            int weaponIndex = ConsoleInput.ChooseIndex( "Выберите оружие из списка ниже:", weaponNames );

            List<string> armorNames = _armors.Select( armor => armor.Name ).ToList();
            int armorIndex = ConsoleInput.ChooseIndex( "Выберите броню из списка ниже:", armorNames );

            Fighter fighter = new( name, _races[ raceIndex ], _classes[ classIndex ] );
            fighter.SetWeapon( _weapons[ weaponIndex ] );
            fighter.SetArmor( _armors[ armorIndex ] );

            _fighters.Add( fighter );

            Console.WriteLine( "Боец добавлен на арену:" );
            PrintFighter( fighter );
        }

        private void PrintFighters()
        {
            if ( _fighters.Count == 0 )
            {
                Console.WriteLine( "На арене пока никого нет, добавьте бойца командой add-fighter." );
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
                Console.WriteLine( "Для боя нужно минимум два бойца, добавьте их командой add-fighter." );
                return;
            }

            Console.WriteLine();
            Console.WriteLine( "Бой начинается! Первыми бьют бойцы с большей инициативой." );

            FightReport fightReport = _fightManager.Fight( _fighters );
            PrintFightReport( fightReport );

            _fighters.Clear();

            Console.WriteLine();
            Console.WriteLine( "Арена очищена, можно набирать новых бойцов." );
        }

        private static void PrintFightReport( FightReport fightReport )
        {
            foreach ( RoundReport round in fightReport.Rounds )
            {
                Console.WriteLine();
                Console.WriteLine( $" --- Раунд {round.Number} --- " );

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

            Console.WriteLine( $"{attack.Attacking.Name} наносит {attack.Damage} урона бойцу {attack.Attacked.Name}.{criticalText} " +
                $"У бойца {attack.Attacked.Name} осталось {attack.AttackedHealthLeft} здоровья" );

            if ( attack.IsAttackedDefeated )
            {
                Console.WriteLine( $"{attack.Attacked.Name} погибает!" );
            }
        }

        private static void PrintFightOutcome( FightReport fightReport )
        {
            Console.WriteLine();

            if ( fightReport.Winner is null )
            {
                Console.WriteLine( "Бой окончен. Ничья" );
                return;
            }

            Console.WriteLine( $"Бой окончен. {fightReport.Winner.Name} остаётся последним на арене и побеждает! " +
                $"Здоровья осталось: {fightReport.Winner.GetCurrentHealth()}" );
        }

        private static void PrintFighter( IFighter fighter )
        {
            Console.WriteLine( $"  {fighter.Name}: здоровье {fighter.GetCurrentHealth()}/{fighter.GetMaxHealth()}, " +
                $"урон {fighter.GetBasicDamage()}, броня {fighter.GetArmor()}, инициатива {fighter.GetInitiative()}, " +
                $"шанс крита {fighter.GetCritChance() * 100:0}%" );
        }

        private static void PrintCommands()
        {
            Console.WriteLine( "Доступные команды:" );
            Console.WriteLine( "  add-fighter - добавить нового бойца на арену" );
            Console.WriteLine( "  list - показать бойцов на арене" );
            Console.WriteLine( "  play - начать битву" );
            Console.WriteLine( "  help - показать список команд" );
            Console.WriteLine( "  exit - выйти из игры" );
        }
    }
}