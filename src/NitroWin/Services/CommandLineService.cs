using System.Reflection;
using System.Resources;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NitroWin.Models;
using NitroWin.Core.Services;

namespace NitroWin.Services;

internal sealed class CommandLineService(ResourceManager resourceManager, IHostApplicationLifetime lifetime, ILogger<CommandLineService> logger) {
    private readonly string? _version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();

    internal CommandLineOptions ParseArguments(string[] args) {
        if (args.Contains("-h", StringComparer.OrdinalIgnoreCase) || args.Contains("--help", StringComparer.OrdinalIgnoreCase)) {
            WriteHelp();
            lifetime.StopApplication();
            return new CommandLineOptions();
        }

        if (args.Contains("-v", StringComparer.OrdinalIgnoreCase) || args.Contains("--version", StringComparer.OrdinalIgnoreCase)) {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("{Version}", _version);
            lifetime.StopApplication();
            return new CommandLineOptions();
        }

        return new CommandLineOptions() {
            NoApps = args.Contains("-na", StringComparer.OrdinalIgnoreCase) || args.Contains("--no-apps", StringComparer.OrdinalIgnoreCase),
            NoTweaks = args.Contains("-nt", StringComparer.OrdinalIgnoreCase) || args.Contains("--no-tweaks", StringComparer.OrdinalIgnoreCase)
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

        foreach (var option in options)
            logger.LogInformation("{Option}", option);
    }
}
