namespace Fighters.Fight
{
    public class AttackReport
    {
        public AttackReport(
            string attackingName,
            string attackedName,
            int rawDamage,
            int dealtDamage,
            bool isCritical,
            int attackedHealthLeft )
        {
            AttackingName = attackingName;
            AttackedName = attackedName;
            RawDamage = rawDamage;
            DealtDamage = dealtDamage;
            IsCritical = isCritical;
            AttackedHealthLeft = attackedHealthLeft;
        }

        public string AttackingName { get; }
        public string AttackedName { get; }
        public int RawDamage { get; }
        public int DealtDamage { get; }
        public bool IsCritical { get; }
        public int AttackedHealthLeft { get; }
        public bool IsAttackedDefeated => AttackedHealthLeft == 0;
    }
}
