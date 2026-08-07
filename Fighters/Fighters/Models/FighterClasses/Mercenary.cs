namespace Fighters.Models.FighterClasses
{
    public class Mercenary : IFighterClass
    {
        public string Name => "Наёмник";
        public float CritChanceBonus => 0.12f;
        public float CritDamageBonus => 0.30f;
        public int HealthBonus => 15;
        public int DamageBonus => 12;
        public int InitiativeBonus => 0;
    }
}
