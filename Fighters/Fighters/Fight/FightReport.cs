using Fighters.Models.Fighters;

namespace Fighters.Fight
{
    public class FightReport
    {
        public FightReport( IReadOnlyList<RoundReport> rounds, IFighter? winner )
        {
            Rounds = rounds;
            Winner = winner;
        }

        public IReadOnlyList<RoundReport> Rounds { get; }
        public IFighter? Winner { get; }
    }
}
