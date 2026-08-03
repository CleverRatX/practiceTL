public class Program
{
    public static void Main( string[] args )
    {
        Casino casino = new();
        casino.RunCasino();
    }
}

public class Casino
{
    private int _balance = 0;
    private bool _isGameStarts = true;
    private const int Multiplicator = 3;
    private const int MinNum = 18;

    public void RunCasino()
    {
        bool askingForBalance = true;
        while ( askingForBalance )
        {
            Console.Write( "Введите начальный баланс: " );
            string? input = Console.ReadLine();
            if ( TryParseAndSetBalance( input ) )
            {
                askingForBalance = false;
            }
        }

        PrintTitle();
        PrintMenu();

        while ( _isGameStarts )
        {
            Console.WriteLine();
            Console.Write( "Введите команду: " );
            HandleInput();
        }
    }

    private bool TryParseAndSetBalance( string? inputBal )
    {
        if ( int.TryParse( inputBal, out int bal ) && bal > 0 )
        {
            _balance = bal;
            return true;
        }
        else
        {
            Console.WriteLine( "Ошибка: некорректный баланс" );
            return false;
        }
    }

    private bool TryParseAndAddToBalance( string? inputAdd )
    {
        if ( int.TryParse( inputAdd, out int addition ) && addition > 0 )
        {
            _balance += addition;
            return true;
        }
        else
        {
            Console.WriteLine( "Ошибка: некорректная сумма пополнения" );
            return false;
        }
    }

    private void EndGame()
    {
        _isGameStarts = false;
    }

    private void PrintBalance()
    {
        Console.WriteLine( $"Ваш баланс: {_balance}" );
    }

    private void PrintTitle()
    {
        Console.WriteLine();
        Console.WriteLine( " ####    ##    ####  # #    #  #### " );
        Console.WriteLine( "#    #  #  #  #      # ##   # #    #" );
        Console.WriteLine( "#      #    #  ####  # # #  # #    #" );
        Console.WriteLine( "#      ######      # # #  # # #    #" );
        Console.WriteLine( "#    # #    # #    # # #   ## #    #" );
        Console.WriteLine( " ####  #    #  ####  # #    #  #### " );
        Console.WriteLine();
    }

    private void PrintMenu()
    {
        Console.WriteLine( "Доступные команды:" );
        Console.WriteLine( "deposit [сумма пополнения] - пополнить баланс" );
        Console.WriteLine( "bet [сумма ставки] - поставить и запустить" );
        Console.WriteLine( "bal - вывести сумму баланса" );
        Console.WriteLine( "menu - вывести список команд" );
        Console.WriteLine( "exit - выход" );
    }

    private void RunGameIteration( int betSum )
    {
        Console.WriteLine( $"Игра началась, ваша ставка: {betSum}" );

        int randomNum = Random.Shared.Next( 1, 21 );
        Console.WriteLine( $"Выпало число: {randomNum}" );

        if ( randomNum >= MinNum )
        {
            int winSum = betSum * ( 1 + Multiplicator * ( randomNum % ( MinNum - 1 ) ) );
            Console.WriteLine( $"Вы выиграли!!! Сумма выигрыша: {winSum}" );
            _balance += winSum;
            PrintBalance();
        }
        else
        {
            Console.WriteLine( "Вы проиграли..." );
            _balance -= betSum;
            PrintBalance();
        }
    }

    private void TryHandleBet( string? inputBetSum )
    {
        if ( !int.TryParse( inputBetSum, out int betSum ) )
        {
            Console.WriteLine( "Ошибка: некорректная сумма ставки" );
            return;
        }

        if ( betSum > _balance )
        {
            Console.WriteLine( "Ошибка: сумма ставки превышает баланс" );
            return;
        }

        if ( betSum <= 0 )
        {
            Console.WriteLine( "Ошибка: сумма ставки должна быть > 0" );
            return;
        }

        RunGameIteration( betSum );
    }

    private void HandleInput()
    {
        string? input = Console.ReadLine();

        if ( string.IsNullOrWhiteSpace( input ) )
        {
            Console.WriteLine( "Ошибка: пустой ввод" );
            return;
        }

        string[] parts = input.Split( ' ' );

        if ( parts.Length > 2 )
        {
            Console.WriteLine( "Ошибка: слишком много аргументов в вводе" );
            return;
        }

        switch ( parts[ 0 ] )
        {
            case "bal":
                {
                    PrintBalance();
                }
                break;
            case "menu":
                {
                    PrintMenu();
                }
                break;
            case "exit":
                {
                    EndGame();
                }
                break;
            case "deposit":
                {
                    if ( parts.Length < 2 )
                    {
                        Console.WriteLine( "Ошибка: отсутствует аргумент для команды \"deposit\"" );
                        return;
                    }

                    if ( TryParseAndAddToBalance( parts[ 1 ] ) )
                    {
                        PrintBalance();
                    }
                }
                break;
            case "bet":
                {
                    if ( parts.Length < 2 )
                    {
                        Console.WriteLine( "Ошибка: отсутствует аргумент для команды \"bet\"" );
                        return;
                    }

                    TryHandleBet( parts[ 1 ] );
                }
                break;
            default:
                Console.WriteLine( "Ошибка: некорректная команда" );
                break;
        }
    }
}
