using Fighters.Models.Fighters;
using Fighters.Utils;

namespace Fighters.Fight
{
    public interface IAttackManager
    {
        AttackResult Attack( IFighter attacking, IFighter attacked );
    }

    public class AttackManager : IAttackManager
    {
        public const float MinScatterMultiplier = 0.8f;
        public const float MaxScatterMultiplier = 1.2f;

        private readonly IRandomProvider _random;

        public AttackManager( IRandomProvider random )
        {
            ArgumentNullException.ThrowIfNull( random );

            _random = random;
        }

        public AttackResult Attack( IFighter attacking, IFighter attacked )
        {
            ArgumentNullException.ThrowIfNull( attacking );
            ArgumentNullException.ThrowIfNull( attacked );

            AttackResult result = CalculateAttack( attacking, attacked );
            attacked.TakeDamage( result.DealtDamage );

            return result;
        }

        private AttackResult CalculateAttack( IFighter attacking, IFighter attacked )
        {
            float damage = attacking.Damage;
            bool isCritical = _random.NextFloat() < attacking.CritChance;

            if ( isCritical )
            {
                damage *= attacking.CritDamageMultiplier;
            }

            float scatterMultiplier = MinScatterMultiplier + ( _random.NextFloat() * ( MaxScatterMultiplier - MinScatterMultiplier ) );

            int rawDamage = ( int )( damage * scatterMultiplier );
            int dealtDamage = Math.Max( rawDamage - attacked.Armor, 0 );

            return new AttackResult( rawDamage, dealtDamage, isCritical );
        }
    }
}
