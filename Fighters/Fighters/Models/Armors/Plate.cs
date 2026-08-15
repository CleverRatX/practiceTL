namespace Fighters.Models.Armors
{
    public class Plate : IArmor
    {
        public string Name { get; } = "Пластинчатая броня";
        public int Armor { get; } = 10;
        public int Initiative { get; } = -4;
        public float CritChance { get; } = -0.05f;
    }
}
