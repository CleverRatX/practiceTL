namespace Fighters.Models.Armors
{
    public class Chainmail : IArmor
    {
        public string Name => "Кольчужная броня";
        public int Armor => 5;
        public float CritChanceBonus => 0f;
        public int InitiativeBonus => 2;
    }
}
