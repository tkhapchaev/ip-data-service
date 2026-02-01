using Core.Services.Logger.Interfaces;
using Ui.Services.Writer.Interfaces;

namespace Ui.Services.Writer.Implementations
{
    public class ConsoleWriter : IWriter, ILogger
    {
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        public void AppendLog(string message)
        {
            WriteLine(message);
        }
    }
}
