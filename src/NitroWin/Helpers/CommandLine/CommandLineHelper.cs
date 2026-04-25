using Serilog;
using System.Reflection;

namespace NitroWin.Helpers.CommandLine
{
    internal static class CommandLineHelper
    {
        internal static void WriteBranding()
        {
            Console.Title = ResourceHelper.GetString("App_Name");

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

            Console.WriteLine(Environment.NewLine
                + ResourceHelper.GetString("App_Description") + Environment.NewLine);

            Log.Debug(string.Format(ResourceHelper.GetString("CommandLine_HelloFrom"),
                ResourceHelper.GetString("App_Name"), Assembly.GetExecutingAssembly().GetName().Version));
        }

        internal static CommandLineOptions ParseArguments(string[] args)
        {
            if (args.Contains("-h") || args.Contains("--help"))
            {
                WriteHelp();
                Environment.Exit(0);
            }

            if (args.Contains("-v") || args.Contains("--version"))
            {
                WriteVersion();
                Environment.Exit(0);
            }

            return new CommandLineOptions()
            {
                NoApps = args.Contains("-na") || args.Contains("--no-apps"),
                NoTweaks = args.Contains("-nt") || args.Contains("--no-tweaks")
            };
        }

        private static void WriteHelp()
        {
            Console.WriteLine(ResourceHelper.GetString("CommandLine_Options"));

            string[] options = [
                "-h, --help       => Print help",
                "-v, --version    => Print version",
                "-na, --no-apps   => Skip app installation",
                "-nt, --no-tweaks => Skip tweaks"
                ];

            foreach (string option in options)
            {
                Console.WriteLine(option);
            }
        }

        private static void WriteVersion()
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Version);
        }
    }
}
