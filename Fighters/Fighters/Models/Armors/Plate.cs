namespace Fighters.Models.Armors
{
    public class Plate : IArmor
    {
        public string Name => "Пластинчатая броня";
        public int Armor => 10;
        public float CritChanceBonus => -0.05f;
        public int InitiativeBonus => -4;
    }
}
