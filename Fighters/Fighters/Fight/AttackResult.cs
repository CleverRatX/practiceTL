namespace Fighters.Fight
{
    public readonly struct AttackResult
    {
        public int Damage { get; }
        public bool IsCritical { get; }

        public AttackResult( int damage, bool isCritical )
        {
            Damage = damage;
            IsCritical = isCritical;
        }
    }
}
