namespace NitroWin.Helpers
{
    public static class ConsoleHelper
    {
        public static void WriteBranding()
        {
            Console.Title = "NitroWin";

            string[] lines = [
                "d8b   db d888888b d888888b d8888b.  .d88b.  db   d8b   db d888888b d8b   db",
                "888o  88   `88'   `~~88~~' 88  `8D .8P  Y8. 88   I8I   88   `88'   888o  88",
                "88V8o 88    88       88    88oobY' 88    88 88   I8I   88    88    88V8o 88",
                "88 V8o88    88       88    88`8b   88    88 Y8   I8I   88    88    88 V8o88",
                "88  V888   .88.      88    88 `88. `8b  d8' `8b d8'8b d8'   .88.   88  V888",
                "VP   V8P Y888888P    YP    88   YD  `Y88P'   `8b8' `8d8'  Y888888P VP   V8P"
            ];

            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine();
        }
    }
}
