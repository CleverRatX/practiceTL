using Fighters.Fight;
using Xunit;

namespace Fighters.Tests.Fight
{
    public class AttackReportTests
    {
        [Fact]
        public void Constructor_Always_StoresAllPassedValues()
        {
            AttackReport report = CreateReport(
                attackingName: "Боец1",
                attackedName: "Боец2",
                rawDamage: 50,
                dealtDamage: 30,
                isCritical: true,
                attackedHealthLeft: 70 );

            Assert.Equal( "Боец1", report.AttackingName );
            Assert.Equal( "Боец2", report.AttackedName );
            Assert.Equal( 50, report.RawDamage );
            Assert.Equal( 30, report.DealtDamage );
            Assert.True( report.IsCritical );
            Assert.Equal( 70, report.AttackedHealthLeft );
        }

        [Fact]
        public void IsAttackedDefeated_HealthLeftIsZero_ReturnsTrue()
        {
            AttackReport report = CreateReport( attackedHealthLeft: 0 );

            Assert.True( report.IsAttackedDefeated );
        }

        [Theory]
        [InlineData( 1 )]
        [InlineData( 50 )]
        [InlineData( 1000 )]
        public void IsAttackedDefeated_HealthLeftIsPositive_ReturnsFalse( int attackedHealthLeft )
        {
            AttackReport report = CreateReport( attackedHealthLeft: attackedHealthLeft );

            Assert.False( report.IsAttackedDefeated );
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
