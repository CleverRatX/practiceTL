using Fighters.Extensions;
using Fighters.Models.Fighters;

namespace Fighters.Fight
{
    public class FightManager
    {
        private const int MaxRoundsCount = 30;

        private readonly AttackManager _attackManager;
        private readonly IRandomProvider _random;

        public FightManager( AttackManager attackManager, IRandomProvider random )
        {
            _attackManager = attackManager ?? throw new ArgumentNullException( nameof( attackManager ) );
            _random = random ?? throw new ArgumentNullException( nameof( random ) );
        }

        public FightReport Fight( List<IFighter> fighters )
        {
            ArgumentNullException.ThrowIfNull( fighters );

            if ( fighters.Count < 2 )
            {
                throw new ArgumentException( "Ошибка: для боя нужно минимум два бойца.", nameof( fighters ) );
            }

            List<RoundReport> rounds = new();
            int roundNumber = 1;

            while ( ( GetAliveFighters( fighters ).Count > 1 ) && ( roundNumber <= MaxRoundsCount ) )
            {
                rounds.Add( PlayRound( fighters, roundNumber ) );
                roundNumber++;
            }

            return new FightReport( rounds, GetWinner( fighters ) );
        }

        private RoundReport PlayRound( List<IFighter> fighters, int roundNumber )
        {
            List<AttackReport> attacks = new();

            List<IFighter> attackOrder = GetAliveFighters( fighters )
                .OrderByDescending( fighter => fighter.GetInitiative() )
                .ToList();

            foreach ( IFighter attacking in attackOrder )
            {
                if ( !attacking.IsAlive() )
                {
                    continue;
                }

                List<IFighter> targets = fighters
                    .Where( fighter => ( fighter != attacking ) && fighter.IsAlive() )
                    .ToList();

                if ( targets.Count == 0 )
                {
                    break;
                }

                IFighter attacked = targets[ _random.Next( 0, targets.Count ) ];
                AttackResult attackResult = _attackManager.Attack( attacking, attacked );

                attacks.Add( new AttackReport(
                    attacking,
                    attacked,
                    attackResult.Damage,
                    attackResult.IsCritical,
                    attacked.GetCurrentHealth() ) );
            }

            return new RoundReport( roundNumber, attacks );
        }

        private static IFighter? GetWinner( List<IFighter> fighters )
        {
            List<IFighter> aliveFighters = GetAliveFighters( fighters );

            return aliveFighters.Count == 1
                ? aliveFighters[ 0 ]
                : null;
        }

        private static List<IFighter> GetAliveFighters( List<IFighter> fighters )
        {
            return fighters
                .Where( fighter => fighter.IsAlive() )
                .ToList();
        }
    }
}