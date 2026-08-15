namespace Fighters.Models.Armors
{
    public interface IArmor
    {
        string Name { get; }
        int Armor { get; }
        int Initiative { get; }
        float CritChance { get; }
    }
}
