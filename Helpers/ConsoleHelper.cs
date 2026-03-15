namespace NitroWin.Helpers
{
    public static class ConsoleHelper
    {
        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void WriteWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
