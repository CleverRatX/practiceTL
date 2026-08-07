namespace Fighters.Fight
{
    public class RandomProvider : IRandomProvider
    {
        private readonly Random _random = new();

        public int Next( int minInclusive, int maxExclusive ) => _random.Next( minInclusive, maxExclusive );
    }
}
