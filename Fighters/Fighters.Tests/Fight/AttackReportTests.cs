using Fighters.Fight;
using Xunit;

namespace Fighters.Tests.Fight
{
    public class AttackReportTests
    {
        [Fact]
        public void IsAttackedDefeated_HealthLeftIsZero_ReturnsTrue()
        {
            // Arrange
            AttackReport report = CreateReport( attackedHealthLeft: 0 );

            // Act
            bool isAttackedDefeated = report.IsAttackedDefeated;

            // Assert
            Assert.True( isAttackedDefeated );
        }

        [Theory]
        [InlineData( 1 )]
        [InlineData( 50 )]
        [InlineData( 1000 )]
        public void IsAttackedDefeated_HealthLeftIsPositive_ReturnsFalse( int attackedHealthLeft )
        {
            // Arrange
            AttackReport report = CreateReport( attackedHealthLeft: attackedHealthLeft );

            // Act
            bool isAttackedDefeated = report.IsAttackedDefeated;

            // Assert
            Assert.False( isAttackedDefeated );
        }

        private static AttackReport CreateReport(
            string attackingName = "Атакующий",
            string attackedName = "Атакованный",
            int rawDamage = 10,
            int dealtDamage = 10,
            bool isCritical = false,
            int attackedHealthLeft = 10 )
        {
            return new AttackReport( attackingName, attackedName, rawDamage, dealtDamage, isCritical, attackedHealthLeft );
        }
    }
}
