namespace Fighters.Models.FighterClasses
{
    public interface IFighterClass
    {
        string Name { get; }
        int Health { get; }
        int Damage { get; }
        int Initiative { get; }
        float CritChance { get; }
        float CritDamage { get; }
    }
}
