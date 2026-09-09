using Domain.Exceptions;

namespace Domain.Validation
{
    internal static class Validator
    {
        private const int CurrencyCodeLength = 3;

        public static string NormalizedText( string? value, string message )
        {
            if ( string.IsNullOrWhiteSpace( value ) )
            {
                throw new DomainValidationException( message );
            }

            return value.Trim();
        }

        public static string NormalizedCurrencyCode( string? value )
        {
            string currency = NormalizedText( value, "Валюта не может быть пустой." );

            if ( currency.Length != CurrencyCodeLength || !currency.All( char.IsAsciiLetter ) )
            {
                throw new DomainValidationException( "Валюта задаётся трёхбуквенным кодом, например RUB." );
            }

            return currency.ToUpperInvariant();
        }

        public static decimal Positive( decimal value, string message )
        {
            if ( value <= 0 )
            {
                throw new DomainValidationException( message );
            }

            return value;
        }

        public static decimal NotNegative( decimal value, string message )
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
