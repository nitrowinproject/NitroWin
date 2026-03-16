namespace NitroWin.Helpers
{
    public static class ConsoleHelper
    {
        public static void WriteBranding()
        {
            Console.Title = Globals.StringsResourceManager.GetString("ConsoleHelper_ConsoleTitle")!;

            Console.WriteLine(
                "\nd8b   db d888888b d888888b d8888b.  .d88b.  db   d8b   db d888888b d8b   db"
                + "\n888o  88   `88'   `~~88~~' 88  `8D .8P  Y8. 88   I8I   88   `88'   888o  88"
                + "\n88V8o 88    88       88    88oobY' 88    88 88   I8I   88    88    88V8o 88"
                + "\n88 V8o88    88       88    88`8b   88    88 Y8   I8I   88    88    88 V8o88"
                + "\n88  V888   .88.      88    88 `88. `8b  d8' `8b d8'8b d8'   .88.   88  V888"
                + "\nVP   V8P Y888888P    YP    88   YD  `Y88P'   `8b8' `8d8'  Y888888P VP   V8P"
            );
        }

        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(Globals.StringsResourceManager.GetString("ConsoleHelper_ErrorPrefix") + message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void WriteWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Globals.StringsResourceManager.GetString("ConsoleHelper_WarningPrefix") + message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
