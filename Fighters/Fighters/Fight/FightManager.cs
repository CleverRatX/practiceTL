using Fighters.Models.Fighters;
using Fighters.Utils;

namespace Fighters.Fight
{
    public interface IFightManager
    {
        FightReport Fight( IReadOnlyList<IFighter> fighters );
    }

    public class FightManager : IFightManager
    {
        public const int MaxRoundsCount = 30;

        private readonly IAttackManager _attackManager;
        private readonly IRandomProvider _random;

        public FightManager( IAttackManager attackManager, IRandomProvider random )
        {
            ArgumentNullException.ThrowIfNull( attackManager );
            ArgumentNullException.ThrowIfNull( random );

            _attackManager = attackManager;
            _random = random;
        }

        public FightReport Fight( IReadOnlyList<IFighter> fighters )
        {
            ArgumentNullException.ThrowIfNull( fighters );

            if ( fighters.Count < 2 )
            {
                throw new ArgumentException( "Для боя нужно минимум два бойца.", nameof( fighters ) );
            }

            List<IFighter> attackOrder = fighters
                .OrderByDescending( fighter => fighter.Initiative )
                .ToList();

            List<RoundReport> rounds = new();
            int roundNumber = 1;

            while ( GetAliveCount( attackOrder ) > 1 && roundNumber <= MaxRoundsCount )
            {
                rounds.Add( PlayRound( attackOrder, roundNumber ) );
                roundNumber++;
            }

            return new FightReport( rounds, GetWinner( attackOrder ) );
        }

        private RoundReport PlayRound( List<IFighter> attackOrder, int roundNumber )
        {
            List<AttackReport> attacks = new();

            foreach ( IFighter attacking in attackOrder )
            {
                if ( !attacking.IsAlive )
                {
                    continue;
                }

                List<IFighter> targets = attackOrder
                    .Where( fighter => fighter != attacking && fighter.IsAlive )
                    .ToList();

                if ( targets.Count == 0 )
                {
                    break;
                }

                IFighter attacked = targets[ _random.Next( 0, targets.Count ) ];
                AttackResult attackResult = _attackManager.Attack( attacking, attacked );

                attacks.Add( new AttackReport(
                    attacking.Name,
                    attacked.Name,
                    attackResult.RawDamage,
                    attackResult.DealtDamage,
                    attackResult.IsCritical,
                    attacked.CurrentHealth ) );
            }

            return new RoundReport( roundNumber, attacks );
        }

        private static IFighter? GetWinner( List<IFighter> fighters )
        {
            List<IFighter> aliveFighters = fighters
                .Where( fighter => fighter.IsAlive )
                .ToList();

            return aliveFighters.Count == 1
                ? aliveFighters[ 0 ]
                : null;
        }

        private static int GetAliveCount( List<IFighter> fighters )
        {
            return fighters.Count( fighter => fighter.IsAlive );
        }
    }
}
