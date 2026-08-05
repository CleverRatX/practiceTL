public class Program
{
    private static readonly HashSet<string> _agreeAnswers = [ "да", "yes", "y" ];

    public static void Main( string[] args )
    {
        string product = RequestString( "Введите название товара: " );
        int count = RequestInt( "Введите количество: " );
        string name = RequestString( "Введите ваше имя: " );
        string address = RequestString( "Введите адрес доставки: " );

        Console.WriteLine( $"Здравствуйте, {name}, вы заказали {count} {product} на адрес {address}, все верно?" );
        bool isAgree = AskForAgreement();

        if ( isAgree )
        {
            DateTime deliveryDate = DateTime.Now.AddDays( 3 );
            Console.WriteLine( $"{name}! Ваш заказ {product} в количестве {count} оформлен! Ожидайте доставку по адресу {address} к {deliveryDate:dd.MM.yyyy}" );
        }
        else
        {
            Console.WriteLine( "Заказ отменён" );
        }
    }

    private static string RequestString( string message )
    {
        Console.Write( message );
        string? input = Console.ReadLine();

        while ( string.IsNullOrWhiteSpace( input ) )
        {
            Console.WriteLine( "Ошибка: Ввод не должен быть пустым" );
            Console.Write( message );
            input = Console.ReadLine();
        }

        return input;
    }

    private static int RequestInt( string message )
    {
        int result = 0;
        bool isParsed = false;
        while ( !isParsed )
        {
            Console.Write( message );
            string? input = Console.ReadLine();

            isParsed = int.TryParse( input, out result );
            if ( !isParsed )
            {
                Console.WriteLine( "Ошибка: Вы ввели не число" );
            }
        }
        return result;
    }

    private static bool AskForAgreement()
    {
        string? answer = Console.ReadLine();

        if ( !string.IsNullOrWhiteSpace( answer ) )
        {
            answer = answer.ToLower();
            if ( _agreeAnswers.Contains( answer ) )
            {
                return true;
            }
        }
        return false;
    }
}
