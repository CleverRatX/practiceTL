namespace Fighters.Models.Armors
{
    public class Chainmail : IArmor
    {
        public string Name { get; } = "Кольчужная броня";
        public int Armor { get; } = 5;
        public int Initiative { get; } = 2;
        public float CritChance { get; } = 0f;
    }
}
