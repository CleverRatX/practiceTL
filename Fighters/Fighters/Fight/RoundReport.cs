namespace Fighters.Fight
{
    public class RoundReport
    {
        public int Number { get; }
        public IReadOnlyList<AttackReport> Attacks { get; }

        public RoundReport( int number, IReadOnlyList<AttackReport> attacks )
        {
            Number = number;
            Attacks = attacks ?? throw new ArgumentNullException( nameof( attacks ) );
        }
    }
}