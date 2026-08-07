namespace Fighters.Models.Weapons
{
    public class Spear : IWeapon
    {
        public string Name => "Копье";
        public int Damage => 21;
        public float CritChance => 0.08f;
        public float CritDamage => 0.80f;
    }
}
