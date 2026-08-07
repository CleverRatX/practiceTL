namespace Fighters.Models.Weapons
{
    public class Fists : IWeapon
    {
        public string Name => "Кулаки";
        public int Damage => 8;
        public float CritChance => 0.10f;
        public float CritDamage => 0.20f;
    }
}
