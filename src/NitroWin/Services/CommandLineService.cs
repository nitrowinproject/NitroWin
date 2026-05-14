using System.Reflection;
using System.Resources;
using NitroWin.Models;

namespace NitroWin.Services;

internal sealed class CommandLineService(ResourceManager resourceManager, LogService logService) {
    private readonly string _version = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
    private readonly string _name = resourceManager.GetString("App_Name")!;

    internal void WriteBranding() {
        Console.Title = string.Join(" ", _name, _version);

        string[] branding = [
            "d8b   db d888888b d888888b d8888b.  .d88b.  db   d8b   db d888888b d8b   db",
            "888o  88   `88'   `~~88~~' 88  `8D .8P  Y8. 88   I8I   88   `88'   888o  88",
            "88V8o 88    88       88    88oobY' 88    88 88   I8I   88    88    88V8o 88",
            "88 V8o88    88       88    88`8b   88    88 Y8   I8I   88    88    88 V8o88",
            "88  V888   .88.      88    88 `88. `8b  d8' `8b d8'8b d8'   .88.   88  V888",
            "VP   V8P Y888888P    YP    88   YD  `Y88P'   `8b8' `8d8'  Y888888P VP   V8P",
            Environment.NewLine,
            resourceManager.GetString("App_Description")!,
            string.Empty
            ];

        foreach (var line in branding) {
            Console.WriteLine(line);
        }

        logService.HelloFrom(_name, _version);
    }

    internal CommandLineOptions ParseArguments(string[] args) {
        if (args.Contains("-h") || args.Contains("--help")) {
            WriteHelp();
            Environment.Exit(0);
        }

        if (args.Contains("-v") || args.Contains("--version")) {
            Console.WriteLine(_version);
            Environment.Exit(0);
        }

        return new CommandLineOptions() {
            NoApps = args.Contains("-na") || args.Contains("--no-apps"),
            NoTweaks = args.Contains("-nt") || args.Contains("--no-tweaks")
        };
    }

    private void WriteHelp() {
        Console.WriteLine(resourceManager.GetString("CommandLine_Options"));

        string[] options = [
            "-h, --help       => Print help",
            "-v, --version    => Print version",
            "-na, --no-apps   => Skip app installation",
            "-nt, --no-tweaks => Skip tweaks"
            ];

        foreach (var option in options) {
            Console.WriteLine(option);
        }
    }
}
