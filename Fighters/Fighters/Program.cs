using Fighters.Utils;
using Fighters.ConsoleUi;
using Fighters.Fight;

namespace Fighters
{
    public class Program
    {
        public static void Main()
        {
            IRandomProvider randomProvider = new RandomProvider();
            AttackManager attackManager = new( randomProvider );
            FightManager fightManager = new( attackManager, randomProvider );

            GameApp game = new( fightManager );
            game.Run();
        }
    }
}
