using Domain.Exceptions;

namespace Domain.Validation
{
    internal static class Validated
    {
        private const int CurrencyCodeLength = 3;

        public static string Text( string? value, string message )
        {
            if ( string.IsNullOrWhiteSpace( value ) )
            {
                throw new DomainValidationException( message );
            }

            return value.Trim();
        }

        public static string Currency( string? value )
        {
            string currency = Text( value, "Валюта не может быть пустой." );

            if ( currency.Length != CurrencyCodeLength )
            {
                throw new DomainValidationException( "Валюта задаётся трёхбуквенным кодом, например RUB." );
            }

            return currency.ToUpperInvariant();
        }

        public static decimal PositiveAmount( decimal value, string message )
        {
            if ( value <= 0 )
            {
                throw new DomainValidationException( message );
            }

            return value;
        }

        public static decimal NotNegativeAmount( decimal value, string message )
        {
            if ( value < 0 )
            {
                throw new DomainValidationException( message );
            }

            return value;
        }

        public static int PositiveCount( int value, string message )
        {
            if ( value <= 0 )
            {
                throw new DomainValidationException( message );
            }

            return value;
        }

        public static double Coordinate( double value, double limit, string message )
        {
            if ( double.IsNaN( value ) || Math.Abs( value ) > limit )
            {
                throw new DomainValidationException( message );
            }

            return value;
        }
    }
}
