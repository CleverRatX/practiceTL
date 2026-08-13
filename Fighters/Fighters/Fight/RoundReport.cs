namespace Fighters.Fight
{
    public class RoundReport
    {
        public RoundReport( int number, IReadOnlyList<AttackReport> attacks )
        {
            Number = number;
            Attacks = attacks;
        }

        public int Number { get; }
        public IReadOnlyList<AttackReport> Attacks { get; }
    }
}
