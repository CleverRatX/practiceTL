namespace Fighters.Models.Races
{
    public class Vampire : IRace
    {
        public string Name { get; } = "Вампир";
        public int Health { get; } = 190;
        public int Damage { get; } = 34;
        public int Armor { get; } = 4;
        public int Initiative { get; } = 28;
    }
}
