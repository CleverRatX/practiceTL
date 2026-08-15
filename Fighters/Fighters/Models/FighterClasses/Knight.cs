namespace Fighters.Models.FighterClasses
{
    public class Knight : IFighterClass
    {
        public string Name { get; } = "Рыцарь";
        public int Health { get; } = 60;
        public int Damage { get; } = 10;
        public int Initiative { get; } = 10;
        public float CritChance { get; } = 0f;
        public float CritDamage { get; } = 0.10f;
    }
}
