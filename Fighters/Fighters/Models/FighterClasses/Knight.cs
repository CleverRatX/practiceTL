namespace Fighters.Models.FighterClasses
{
    public class Knight : IFighterClass
    {
        public string Name => "Рыцарь";
        public float CritChanceBonus => 0f;
        public float CritDamageBonus => 0.10f;
        public int HealthBonus => 60;
        public int DamageBonus => 10;
        public int InitiativeBonus => 10;
    }
}
