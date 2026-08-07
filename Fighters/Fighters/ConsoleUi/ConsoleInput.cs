namespace Fighters.ConsoleUi
{
    public static class ConsoleInput
    {
        public static string ReadNotEmptyString( string title )
        {
            Console.WriteLine( title );

            string input = "";
            bool isReading = true;

            while ( isReading )
            {
                input = Console.ReadLine()?.Trim() ?? string.Empty;

                if ( !string.IsNullOrWhiteSpace( input ) )
                {
                    isReading = false;
                }
                else
                {
                    Console.WriteLine( "Значение не может быть пустым, попробуйте ещё раз:" );
                }
            }

            return input;
        }

        public static int ChooseIndex( string title, List<string> options )
        {
            Console.WriteLine( title );

            for ( int i = 0; i < options.Count; i++ )
            {
                Console.WriteLine( $"  {i} - {options[ i ]}" );
            }

            int index = 0;
            bool isChoosing = true;

            while ( isChoosing )
            {
                string input = Console.ReadLine()?.Trim() ?? string.Empty;

                if ( int.TryParse( input, out index ) && ( index >= 0 ) && ( index < options.Count ) )
                {
                    isChoosing = false;
                }
                else
                {
                    Console.WriteLine( $"Нужно ввести число от 0 до {options.Count - 1}, попробуйте ещё раз:" );
                }
            }

            return index;
        }
    }
}
