namespace Fighters.Utils
{
    public interface IRandomProvider
    {
        int Next( int minInclusive, int maxExclusive );
        float NextFloat();
    }

    public class RandomProvider : IRandomProvider
    {
        private readonly Random _random = new();

        public int Next( int minInclusive, int maxExclusive )
        {
            return _random.Next( minInclusive, maxExclusive );
        }

        public float NextFloat()
        {
            return _random.NextSingle();
        }
    }
}
