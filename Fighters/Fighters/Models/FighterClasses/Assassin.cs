namespace Fighters.Models.FighterClasses
{
    public class Assassin : IFighterClass
    {
        public string Name { get; } = "Ассасин";
        public int Health { get; } = 15;
        public int Damage { get; } = 12;
        public int Initiative { get; } = 0;
        public float CritChance { get; } = 0.12f;
        public float CritDamage { get; } = 0.30f;
    }
}
