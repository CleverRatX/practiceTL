namespace Fighters.Models.Weapons
{
    public class Sword : IWeapon
    {
        public string Name { get; } = "Меч";
        public int Damage { get; } = 20;
        public float CritChance { get; } = 0.15f;
        public float CritDamage { get; } = 0.50f;
    }
}
