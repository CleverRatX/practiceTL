namespace Fighters.ConsoleUi
{
    public interface IConsoleInput
    {
        string ReadString( string title );

        int ChooseOption( string title, IReadOnlyList<string> options );
    }

    public class ConsoleInput : IConsoleInput
    {
        private readonly IConsole _console;

        public ConsoleInput( IConsole console )
        {
            ArgumentNullException.ThrowIfNull( console );

            _console = console;
        }

        public string ReadString( string title )
        {
            _console.WriteLine( title );

            string? input = _console.ReadLine()?.Trim();

            while ( string.IsNullOrWhiteSpace( input ) )
            {
                _console.WriteLine( "Значение не может быть пустым, попробуйте ещё раз:" );

                input = _console.ReadLine()?.Trim();
            }

            return input;
        }

        public int ChooseOption( string title, IReadOnlyList<string> options )
        {
            ArgumentNullException.ThrowIfNull( options );

            if ( options.Count == 0 )
            {
                throw new ArgumentException( "Список вариантов не может быть пустым.", nameof( options ) );
            }

            _console.WriteLine( title );

            for ( int i = 0; i < options.Count; i++ )
            {
                _console.WriteLine( $"  {i} - {options[ i ]}" );
            }

            int option = 0;
            bool isChoosing = true;

            while ( isChoosing )
            {
                string? input = _console.ReadLine()?.Trim();

                if ( int.TryParse( input, out option ) && option >= 0 && option < options.Count )
                {
                    isChoosing = false;
                }
                else
                {
                    _console.WriteLine( $"Нужно ввести число от 0 до {options.Count - 1}, попробуйте ещё раз:" );
                }
            }

            return option;
        }
    }
}
