namespace Fighters.Fight
{
    public interface IRandomProvider
    {
        int Next( int minInclusive, int maxExclusive );
    }
}
