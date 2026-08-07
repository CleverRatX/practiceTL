namespace Fighters.Models.Races
{
    public class Vampire : IRace
    {
        public string Name => "Вампир";

        public int Strength => 34;

        public int Health => 190;

        public int Armor => 4;

        public int Initiative => 28;
    }
}
