namespace Fighters.Models.Fighters
{
    public interface IFighter
    {
        string Name { get; }
        string RaceName { get; }
        string ClassName { get; }
        string WeaponName { get; }
        string ArmorName { get; }

        int MaxHealth { get; }
        int CurrentHealth { get; }
        int Damage { get; }
        int Armor { get; }
        int Initiative { get; }
        float CritChance { get; }
        float CritDamageMultiplier { get; }
        bool IsAlive { get; }

        void TakeDamage( int damage );
        void RestoreHealth();
    }
}
