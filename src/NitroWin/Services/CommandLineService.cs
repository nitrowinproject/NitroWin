using System.Reflection;
using System.Resources;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NitroWin.Models;

namespace NitroWin.Services;

internal sealed class CommandLineService(ResourceManager resourceManager, LogService logService, IHostApplicationLifetime lifetime, ILogger<CommandLineService> logger) {
    private readonly string? _version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
    private readonly string _name = resourceManager.GetString("App_Name")!;

    internal void WriteBranding() {
        Console.Title = string.Join(" ", _name, _version);

        if (!logger.IsEnabled(LogLevel.Information)) return;

        string[] branding = [
            "d8b   db d888888b d888888b d8888b.  .d88b.  db   d8b   db d888888b d8b   db",
            "888o  88   `88'   `~~88~~' 88  `8D .8P  Y8. 88   I8I   88   `88'   888o  88",
            "88V8o 88    88       88    88oobY' 88    88 88   I8I   88    88    88V8o 88",
            "88 V8o88    88       88    88`8b   88    88 Y8   I8I   88    88    88 V8o88",
            "88  V888   .88.      88    88 `88. `8b  d8' `8b d8'8b d8'   .88.   88  V888",
            "VP   V8P Y888888P    YP    88   YD  `Y88P'   `8b8' `8d8'  Y888888P VP   V8P",
            resourceManager.GetString("App_Description")!
            ];

        foreach (var line in branding)
            logger.LogInformation("{Line}", line);

        logService.HelloFrom(_name, _version ?? resourceManager.GetString("CommandLine_UnknownVersion")!);
    }

    internal CommandLineOptions ParseArguments(string[] args) {
        if (args.Contains("-h") || args.Contains("--help")) {
            WriteHelp();
            lifetime.StopApplication();
            return new CommandLineOptions();
        }

        if (args.Contains("-v") || args.Contains("--version")) {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("{Version}", _version);
            lifetime.StopApplication();
            return new CommandLineOptions();
        }

        return new CommandLineOptions() {
            NoApps = args.Contains("-na") || args.Contains("--no-apps"),
            NoTweaks = args.Contains("-nt") || args.Contains("--no-tweaks")
        };
    }

    private void WriteHelp() {
        if (!logger.IsEnabled(LogLevel.Information)) return;

        logger.LogInformation("{Options}", resourceManager.GetString("CommandLine_Options"));

        string[] options = [
            $"-h, --help       => {resourceManager.GetString("Options_PrintHelp")}",
            $"-v, --version    => {resourceManager.GetString("Options_PrintVersion")}",
            $"-na, --no-apps   => {resourceManager.GetString("Options_NoApps")}",
            $"-nt, --no-tweaks => {resourceManager.GetString("Options_NoTweaks")}"
            ];

        foreach (var option in options) {
            logger.LogInformation("{Option}", option);
        }
    }
}
