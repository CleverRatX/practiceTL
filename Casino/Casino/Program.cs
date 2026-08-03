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
    private bool _isGameStarts = false;
    private const int Multiplicator = 3;
    private const int MinNum = 18;

    public void RunCasino()
    {
        Console.Write( "Начать (да/нет): " );

        string? answer = Console.ReadLine();
        if ( answer != null )
        {
            answer = answer.ToLower();
            if ( answer == "да" || answer == "yes" || answer == "y" )
            {
                StartGame();

                while ( true )
                {
                    Console.Write( "Введите начальный баланс: " );
                    string? input = Console.ReadLine();
                    if ( ParseAndSetBalance( input ) )
                    {
                        break;
                    }
                }

                HandleTitle();
                HandleMenu();

                while ( _isGameStarts )
                {
                    Console.WriteLine();
                    Console.Write( "Введите команду: " );
                    ProcessInput();
                }
            }
        }
    }

    private bool ParseAndSetBalance( string? inputBal )
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

    private bool ParseAndAddToBalance( string? inputAdd )
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

    private void HandleBalance()
    {
        Console.WriteLine( $"Ваш баланс: {_balance}" );
    }

    private void StartGame()
    {
        _isGameStarts = true;
    }

    private void EndGame()
    {
        _isGameStarts = false;
    }

    private void HandleTitle()
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

    private void HandleMenu()
    {
        Console.WriteLine( "Доступные команды:" );
        Console.WriteLine( "deposit [сумма пополнения] - пополнить баланс" );
        Console.WriteLine( "bet [сумма ставки] - поставить и запустить" );
        Console.WriteLine( "bal - вывести сумму баланса" );
        Console.WriteLine( "menu - вывести список команд" );
        Console.WriteLine( "exit - выход" );
    }

    private void RunGame( int sum )
    {
        Console.WriteLine( $"Игра началась, ваша ставка: {sum}" );

        int randomNum = Random.Shared.Next( 1, 21 );
        Console.WriteLine( $"Выпало число: {randomNum}" );

        if ( randomNum >= MinNum )
        {
            int winSum = sum * ( 1 + ( Multiplicator * ( randomNum % ( MinNum - 1 ) ) ) );
            Console.WriteLine( $"Вы выиграли!!! Сумма выигрыша: {winSum}" );
            _balance += winSum;
            HandleBalance();
        }
        else
        {
            Console.WriteLine( "Вы проиграли..." );
            _balance -= sum;
            HandleBalance();
        }
    }

    private void TryProcessBet( string? inputBetSum )
    {
        if ( int.TryParse( inputBetSum, out int betSum ) )
        {
            if ( betSum <= _balance )
            {
                if ( betSum > 0 )
                {
                    RunGame( betSum );
                }
                else
                {
                    Console.WriteLine( "Ошибка: сумма ставки должна быть > 0" );
                }
            }
            else
            {
                Console.WriteLine( "Ошибка: сумма ставки превышает баланс" );
            }
        }
        else
        {
            Console.WriteLine( "Ошибка: некорректная сумма ставки" );
        }
    }

    private void ProcessInput()
    {
        string? input = Console.ReadLine();
        if ( input != null )
        {
            string[] parts = input.Split( ' ' );
            if ( parts.Length == 1 )
            {
                switch ( parts[ 0 ] )
                {
                    case "bal":
                        {
                            HandleBalance();
                        }
                        break;
                    case "menu":
                        {
                            HandleMenu();
                        }
                        break;
                    case "exit":
                        {
                            EndGame();
                        }
                        break;
                    default:
                        Console.WriteLine( "Ошибка: некорректная команда" );
                        break;
                }
            }
            else if ( parts.Length > 1 )
            {
                switch ( parts[ 0 ] )
                {
                    case "deposit":
                        {
                            if ( ParseAndAddToBalance( parts[ 1 ] ) )
                            {
                                HandleBalance();
                            }
                        }
                        break;
                    case "bet":
                        {
                            TryProcessBet( parts[ 1 ] );
                        }
                        break;
                    default:
                        Console.WriteLine( "Ошибка: некорректная команда" );
                        break;
                }
            }
            else
            {
                Console.WriteLine( "Ошибка" );
            }
        }
        else
        {
            Console.WriteLine( "Ошибка: пустой ввод" );
        }
    }
}
