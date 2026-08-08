namespace Fighters.Models.Weapons
{
    public class Sword : IWeapon
    {
        public string Name => "Меч";
        public int Damage => 20;
        public float CritChance => 0.15f;
        public float CritDamage => 0.50f;
    }
}
