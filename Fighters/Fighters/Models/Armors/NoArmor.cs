namespace Fighters.Models.Armors
{
    public class NoArmor : IArmor
    {
        public string Name { get; } = "Без брони";
        public int Armor { get; } = 0;
        public int Initiative { get; } = 8;
        public float CritChance { get; } = 0.05f;
    }
}
