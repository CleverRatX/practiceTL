using Fighters.Models.Fighters;

namespace Fighters.Fight
{
    public class AttackManager
    {
        private readonly IRandomProvider _random;
        public AttackManager( IRandomProvider random )
        {
            _random = random ?? throw new ArgumentNullException( nameof( random ) );
        }

        public AttackResult Attack( IFighter attacking, IFighter attacked )
        {
            AttackResult attack = CalculateAttack( attacking, attacked );
            attacked.TakeDamage( attack.Damage );
            return attack;
        }

        private AttackResult CalculateAttack( IFighter attacking, IFighter attacked )
        {
            ArgumentNullException.ThrowIfNull( attacking );
            ArgumentNullException.ThrowIfNull( attacked );

            float randomNum = ( float )_random.Next( 1, 101 ) / 100f;
            float scatterMultiplier = 1f + ( ( float )_random.Next( -20, 21 ) / 100f );
            bool isCritical;
            float damage;
            if ( randomNum <= attacking.GetCritChance() )
            {
                damage = attacking.GetBasicDamage() * attacking.GetCritDamageMultiplier();
                isCritical = true;
            }
            else
            {
                damage = attacking.GetBasicDamage();
                isCritical = false;
            }

            damage = damage * scatterMultiplier - attacked.GetArmor();

            if ( damage < 0 )
            {
                damage = 0;
            }

            AttackResult result = new( ( int )damage, isCritical );
            return result;
        }
    }
}
