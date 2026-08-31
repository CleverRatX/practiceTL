using Fighters.ConsoleUi;
using Fighters.Fight;
using Fighters.Models.Fighters;
using Fighters.Utils;

namespace Fighters
{
    public class Program
    {
        public static void Main()
        {
            IRandomProvider randomProvider = new RandomProvider();
            IAttackManager attackManager = new AttackManager( randomProvider );
            IFightManager fightManager = new FightManager( attackManager, randomProvider );

            IConsole console = new SystemConsole();
            IConsoleInput consoleInput = new ConsoleInput( console );
            IFighterFactory fighterFactory = new FighterFactory();

            GameApp game = new( fightManager, fighterFactory, consoleInput, console );
            game.Run();
        }
    }
}
