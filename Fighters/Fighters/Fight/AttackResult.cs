namespace Fighters.Fight
{
    public readonly struct AttackResult
    {
        public AttackResult( int rawDamage, int dealtDamage, bool isCritical )
        {
            RawDamage = rawDamage;
            DealtDamage = dealtDamage;
            IsCritical = isCritical;
        }

        public int RawDamage { get; }
        public int DealtDamage { get; }
        public bool IsCritical { get; }
    }
}
