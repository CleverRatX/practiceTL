namespace Fighters.Models.Races
{
    public interface IRace
    {
        public string Name { get; }
        public int Strength { get; }
        public int Health { get; }
        public int Armor { get; }
        public int Initiative { get; }
    }
}
