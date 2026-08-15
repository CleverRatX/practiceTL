namespace Fighters.ConsoleUi
{
    public static class ConsoleInput
    {
        public static string ReadString( string title )
        {
            Console.WriteLine( title );

            string? input = Console.ReadLine()?.Trim();

            while ( string.IsNullOrWhiteSpace( input ) )
            {
                Console.WriteLine( "Значение не может быть пустым, попробуйте ещё раз:" );

                input = Console.ReadLine()?.Trim();
            }

            return input;
        }

        public static int ChooseOption( string title, IReadOnlyList<string> options )
        {
            Console.WriteLine( title );

            for ( int i = 0; i < options.Count; i++ )
            {
                Console.WriteLine( $"  {i} - {options[ i ]}" );
            }

            int option = 0;
            bool isChoosing = true;

            while ( isChoosing )
            {
                string? input = Console.ReadLine()?.Trim();

                if ( int.TryParse( input, out option ) && option >= 0 && option < options.Count )
                {
                    isChoosing = false;
                }
                else
                {
                    Console.WriteLine( $"Нужно ввести число от 0 до {options.Count - 1}, попробуйте ещё раз:" );
                }
            }

            return option;
        }
    }
}
