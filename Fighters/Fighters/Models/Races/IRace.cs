namespace Fighters.Models.Races
{
    public interface IRace
    {
        string Name { get; }
        int Health { get; }
        int Damage { get; }
        int Armor { get; }
        int Initiative { get; }
    }
}
