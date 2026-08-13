namespace Fighters.Models.Races
{
    public class Human : IRace
    {
        public string Name { get; } = "Человек";
        public int Health { get; } = 220;
        public int Damage { get; } = 26;
        public int Armor { get; } = 8;
        public int Initiative { get; } = 14;
    }
}
