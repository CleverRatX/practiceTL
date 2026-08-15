namespace Fighters.Models.Weapons
{
    public class Fists : IWeapon
    {
        public string Name { get; } = "Кулаки";
        public int Damage { get; } = 8;
        public float CritChance { get; } = 0.10f;
        public float CritDamage { get; } = 0.20f;
    }
}
