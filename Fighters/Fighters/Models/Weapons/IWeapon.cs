namespace Fighters.Models.Weapons
{
    public interface IWeapon
    {
        public string Name { get; }
        public int Damage { get; }
        public float CritChance { get; }
        public float CritDamage { get; }
    }
}
