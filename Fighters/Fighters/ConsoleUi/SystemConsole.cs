namespace Fighters.ConsoleUi
{
    public interface IConsole
    {
        void WriteLine();

        void WriteLine( string message );

        string? ReadLine();
    }

    public class SystemConsole : IConsole
    {
        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine( string message )
        {
            Console.WriteLine( message );
        }

        public string? ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
