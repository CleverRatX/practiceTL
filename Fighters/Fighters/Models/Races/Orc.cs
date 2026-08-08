namespace Fighters.Models.Races
{
    public class Orc : IRace
    {
        public string Name => "Орк";

        public int Strength => 33;

        public int Health => 210;

        public int Armor => 3;

        public int Initiative => 6;
    }
}
