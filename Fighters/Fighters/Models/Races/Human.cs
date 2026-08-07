namespace Fighters.Models.Races
{
    public class Human : IRace
    {
        public string Name => "Человек";

        public int Strength => 26;

        public int Health => 220;

        public int Armor => 8;

        public int Initiative => 14;
    }
}
