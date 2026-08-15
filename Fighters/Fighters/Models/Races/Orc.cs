namespace Fighters.Models.Races
{
    public class Orc : IRace
    {
        public string Name { get; } = "Орк";
        public int Health { get; } = 265;
        public int Damage { get; } = 33;
        public int Armor { get; } = 3;
        public int Initiative { get; } = 6;
    }
}
