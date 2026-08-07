using Fighters.Models.Fighters;

namespace Fighters.Fight
{
    public class AttackReport
    {
        public IFighter Attacking { get; }
        public IFighter Attacked { get; }
        public int Damage { get; }
        public bool IsCritical { get; }
        public int AttackedHealthLeft { get; }
        public bool IsAttackedDefeated => AttackedHealthLeft <= 0;

        public AttackReport( IFighter attacking, IFighter attacked, int damage, bool isCritical, int attackedHealthLeft )
        {
            Attacking = attacking ?? throw new ArgumentNullException( nameof( attacking ) );
            Attacked = attacked ?? throw new ArgumentNullException( nameof( attacked ) );
            Damage = damage;
            IsCritical = isCritical;
            AttackedHealthLeft = attackedHealthLeft;
        }
    }
}