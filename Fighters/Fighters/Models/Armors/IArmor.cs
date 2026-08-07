namespace Fighters.Models.Armors
{
    public interface IArmor
    {
        public string Name { get; }
        public int Armor { get; }
        public float CritChanceBonus { get; }
        public int InitiativeBonus { get; }
    }
}
