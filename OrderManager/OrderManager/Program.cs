public class Program
{
    public static void Main( string[] args )
    {
        string product = RequestString( "Введите название товара: " );
        int count = RequestInt( "Введите количество: " );
        string name = RequestString( "Введите ваше имя: " );
        string address = RequestString( "Введите адрес доставки: " );

        Console.WriteLine( $"Здравствуйте, {name}, вы заказали {count} {product} на адрес {address}, все верно?" );
        string? answer = Console.ReadLine();

        if ( answer != null )
        {
            answer = answer.ToLower();
        }

        if ( answer == "да" || answer == "yes" || answer == "y" )
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
        Console.WriteLine( message );
        string? input = Console.ReadLine();

        while ( input == null )
        {
            Console.WriteLine( "Ошибка: Ввод не должен быть пустым" );
            input = Console.ReadLine();
        }

        return input;
    }

    private static int RequestInt( string message )
    {
        while ( true )
        {
            Console.WriteLine( message );
            string? input = Console.ReadLine();

            if ( int.TryParse( input, out int count ) )
            {
                return count;
            }
            else
            {
                Console.WriteLine( "Ошибка: Вы ввели не число" );
            }
        }
    }
}
