namespace Fighters.Models.Weapons
{
    public class Spear : IWeapon
    {
        public string Name { get; } = "Копьё";
        public int Damage { get; } = 21;
        public float CritChance { get; } = 0.08f;
        public float CritDamage { get; } = 0.80f;
    }
}
