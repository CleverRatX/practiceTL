namespace Fighters.Models.Armors
{
    public class NoArmor : IArmor
    {
        public string Name => "Без брони";
        public int Armor => 0;
        public float CritChanceBonus => 0.05f;
        public int InitiativeBonus => 8;
    }
}
