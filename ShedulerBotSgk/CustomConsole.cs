using ShedulerBotSgk.App;

namespace ShedulerBotSgk
{
    internal class CustomConsole
    {

        static public void Welcome()
        {
            
            WriteWaring($"Starting...");
            WriteWaring($"Scheduler Bot v{Application.Version}");
            WriteWaring($"Debug Mode: {Application.Debug}");
        }

        static public void WriteError(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{DateTime.Now}] {text}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static public void WriteWaring(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{DateTime.Now}] {text}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static public void Write(string text)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[{DateTime.Now}] {text}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
