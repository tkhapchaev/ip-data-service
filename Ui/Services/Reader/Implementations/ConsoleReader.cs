using Ui.Services.Reader.Interfaces;

namespace Ui.Services.Reader.Implementations
{
    public class ConsoleReader : IReader
    {
        public string ReadLine()
        {
            var input = Console.ReadLine();

            if (input is not null)
            {
                return input.Trim();
            }

            return string.Empty;
        }
    }
}