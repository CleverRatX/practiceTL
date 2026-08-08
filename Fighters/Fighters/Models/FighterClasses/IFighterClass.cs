namespace Fighters.Models.FighterClasses
{
    public interface IFighterClass
    {
        public string Name { get; }
        public float CritChanceBonus { get; }
        public float CritDamageBonus { get; }
        public int HealthBonus { get; }
        public int DamageBonus { get; }
        public int InitiativeBonus { get; }
    }
}
