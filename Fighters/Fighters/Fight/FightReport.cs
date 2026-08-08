using Fighters.Models.Fighters;

namespace Fighters.Fight
{
    public class FightReport
    {
        public IReadOnlyList<RoundReport> Rounds { get; }
        public IFighter? Winner { get; }
        public bool IsDraw => Winner is null;

        public FightReport( IReadOnlyList<RoundReport> rounds, IFighter? winner )
        {
            Rounds = rounds ?? throw new ArgumentNullException( nameof( rounds ) );
            Winner = winner;
        }
    }
}