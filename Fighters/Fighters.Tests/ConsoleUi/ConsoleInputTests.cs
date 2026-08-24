using Fighters.ConsoleUi;
using Moq;
using Moq.Language;
using Xunit;

namespace Fighters.Tests.ConsoleUi
{
    public class ConsoleInputTests
    {
        private const string EmptyValueMessage = "Значение не может быть пустым, попробуйте ещё раз:";

        private readonly Mock<IConsole> _console = new();

        [Fact]
        public void Constructor_ConsoleIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>( () => new ConsoleInput( null! ) );
        }

        [Fact]
        public void ReadString_InputIsNotEmpty_ReturnsIt()
        {
            SetupUserInput( "Боец1" );

            string result = CreateConsoleInput().ReadString( "Введите имя:" );

            Assert.Equal( "Боец1", result );
        }

        [Fact]
        public void ReadString_InputHasExtraSpaces_ReturnsTrimmedValue()
        {
            SetupUserInput( "   Боец1   " );

            string result = CreateConsoleInput().ReadString( "Введите имя:" );

            Assert.Equal( "Боец1", result );
        }

        [Fact]
        public void ReadString_Always_PrintsTitleOnce()
        {
            SetupUserInput( "Боец1" );

            CreateConsoleInput().ReadString( "Введите имя:" );

            _console.Verify( console => console.WriteLine( "Введите имя:" ), Times.Once );
        }

        [Theory]
        [InlineData( null )]
        [InlineData( "" )]
        [InlineData( "   " )]
        public void ReadString_InputIsEmpty_AsksAgainAndReturnsNextValue( string? emptyInput )
        {
            SetupUserInput( emptyInput, "Боец1" );

            string result = CreateConsoleInput().ReadString( "Введите имя:" );

            Assert.Equal( "Боец1", result );
            _console.Verify( console => console.WriteLine( EmptyValueMessage ), Times.Once );
        }

        [Fact]
        public void ReadString_SeveralEmptyInputsInARow_AsksAgainForEachOfThem()
        {
            SetupUserInput( "", "   ", null, "Боец1" );

            string result = CreateConsoleInput().ReadString( "Введите имя:" );

            Assert.Equal( "Боец1", result );
            _console.Verify( console => console.WriteLine( EmptyValueMessage ), Times.Exactly( 3 ) );
        }

        [Fact]
        public void ChooseOption_OptionsIsNull_ThrowsArgumentNullException()
        {
            ConsoleInput consoleInput = CreateConsoleInput();

            Assert.Throws<ArgumentNullException>( () => consoleInput.ChooseOption( "Выберите:", null! ) );
        }

        [Fact]
        public void ChooseOption_OptionsAreEmpty_ThrowsArgumentException()
        {
            ConsoleInput consoleInput = CreateConsoleInput();

            Assert.Throws<ArgumentException>( () => consoleInput.ChooseOption( "Выберите:", new List<string>() ) );
        }

        [Theory]
        [InlineData( "0", 0 )]
        [InlineData( "1", 1 )]
        [InlineData( "2", 2 )]
        public void ChooseOption_InputIsValidIndex_ReturnsIt( string input, int expected )
        {
            SetupUserInput( input );

            int result = CreateConsoleInput().ChooseOption( "Выберите:", CreateOptions() );

            Assert.Equal( expected, result );
        }

        [Fact]
        public void ChooseOption_InputHasExtraSpaces_IsTrimmedAndAccepted()
        {
            SetupUserInput( "  2  " );

            int result = CreateConsoleInput().ChooseOption( "Выберите:", CreateOptions() );

            Assert.Equal( 2, result );
        }

        [Fact]
        public void ChooseOption_Always_PrintsTitleAndAllOptionsWithTheirIndexes()
        {
            SetupUserInput( "0" );

            CreateConsoleInput().ChooseOption( "Выберите:", CreateOptions() );

            _console.Verify( console => console.WriteLine( "Выберите:" ), Times.Once );
            _console.Verify( console => console.WriteLine( "  0 - Человек" ), Times.Once );
            _console.Verify( console => console.WriteLine( "  1 - Орк" ), Times.Once );
            _console.Verify( console => console.WriteLine( "  2 - Вампир" ), Times.Once );
        }

        [Theory]
        [InlineData( null )]
        [InlineData( "" )]
        [InlineData( "абв" )]
        [InlineData( "1.5" )]
        [InlineData( "-1" )]
        [InlineData( "3" )]
        [InlineData( "100" )]
        public void ChooseOption_InputIsInvalid_AsksAgainUntilValidIndexIsEntered( string? invalidInput )
        {
            SetupUserInput( invalidInput, "1" );

            int result = CreateConsoleInput().ChooseOption( "Выберите:", CreateOptions() );

            Assert.Equal( 1, result );
            _console.Verify( console => console.WriteLine( "Нужно ввести число от 0 до 2, попробуйте ещё раз:" ), Times.Once );
        }

        [Fact]
        public void ChooseOption_ThereIsOnlyOneOption_AcceptsOnlyZero()
        {
            SetupUserInput( "1", "0" );

            int result = CreateConsoleInput().ChooseOption( "Выберите:", new List<string> { "Человек" } );

            Assert.Equal( 0, result );
            _console.Verify( console => console.WriteLine( "Нужно ввести число от 0 до 0, попробуйте ещё раз:" ), Times.Once );
        }

        private ConsoleInput CreateConsoleInput()
        {
            return new ConsoleInput( _console.Object );
        }

        private void SetupUserInput( params string?[] inputs )
        {
            ISetupSequentialResult<string?> sequence = _console.SetupSequence( console => console.ReadLine() );

            foreach ( string? input in inputs )
            {
                sequence = sequence.Returns( input );
            }
        }

        private static List<string> CreateOptions()
        {
            return new List<string> { "Человек", "Орк", "Вампир" };
        }
    }
}
