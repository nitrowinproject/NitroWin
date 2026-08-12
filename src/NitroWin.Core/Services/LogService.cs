using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using NitroWin.Core.Models;
using NitroWin.Core.Models.Apps;

namespace NitroWin.Core.Services;

public sealed class LogService(IStringLocalizer<LogService> localizer, ILogger<LogService> logger) {
    internal void InstallingApp(AppBase app) => LogResource(
        LogLevel.Information, "InstallingApp", GetAppParameters(app));

#if DEBUG
    internal void NotInstallingApp(AppBase app) => LogResource(
        LogLevel.Debug, "NotInstallingApp", GetAppParameters(app));
#endif

    internal void AppInstallError(AppBase app, Exception exception) => LogResource(
        LogLevel.Error, "AppInstallError", [.. GetAppParameters(app),
            exception.Message]);

#if DEBUG
    internal void ApplyingTweak(Tweak tweak) =>
        LogResource(LogLevel.Debug, "ApplyingTweak", tweak.Title);
#endif

#if DEBUG
    internal void AppliedTweak(Tweak tweak) =>
        LogResource(LogLevel.Debug, "AppliedTweak", tweak.Title);
#endif

    internal void TweakApplyError(Tweak tweak, Exception exception) => LogResource(
        LogLevel.Error, "TweakApplyError", tweak.Title,
        exception.Message);

    internal void TweakReadError(string filePath, Exception exception) => LogResource(
        LogLevel.Error, "TweakReadError", Path.GetFileName(filePath),
        exception.Message);

    internal void DownloadError(string url, Exception exception) => LogResource(
        LogLevel.Error, "DownloadError", Path.GetFileName(url),
        exception.Message);

    internal void ExtractionError(string filePath, Exception exception) => LogResource(
        LogLevel.Error, "ExtractionError", Path.GetFileName(filePath),
        exception.Message);

    internal void NoNetworkError() =>
        LogResource(LogLevel.Error, "NoNetwork");

#if DEBUG
    internal void CommandLineArguments(string[] args) =>
        LogResource(LogLevel.Debug, "CommandLineArguments", string.Join(", ", args));
#endif

    internal void NoConfigFound<T>() where T : ConfigBase =>
        LogResource(LogLevel.Warning, typeof(T) == typeof(AppInstallerConfig)
            ? "NoAppInstallerConfigFound" : "NoConfigFound");

    internal void ConfigError<T>(Exception exception) where T : ConfigBase =>
        LogResource(LogLevel.Error, typeof(T) == typeof(AppInstallerConfig)
            ? "AppInstallerConfigError" : "ConfigError", exception.Message);

    internal void InstallingApps() =>
        LogResource(LogLevel.Information, "InstallingApps");

#if DEBUG
    internal void HelloFrom(string app, string version) =>
        LogResource(LogLevel.Debug, "HelloFrom", app, version);
#endif

    internal void DownloadingTweaks() =>
        LogResource(LogLevel.Information, "DownloadingTweaks");

    internal void ApplyingTweaks() =>
        LogResource(LogLevel.Information, "ApplyingTweaks");

    private string[] GetAppParameters(AppBase app) => app switch {
        AppxApp appxApp => [appxApp.Name ?? Path.GetFileName(appxApp.Path), localizer["AppSourceAppx"]],
        AppxWebApp appxWebApp => [appxWebApp.Name ?? Path.GetFileName(appxWebApp.Url), localizer["AppSourceAppxWeb"]],
        ChocolateyApp chocolateyApp => [chocolateyApp.Id, localizer["AppSourceChocolatey"]],
        ChocolateyBundleApp chocolateyBundleApp => [chocolateyBundleApp.FileName, localizer["AppSourceChocolateyBundle"]],
        WebApp webApp => [webApp.Name ?? Path.GetFileName(webApp.Url), localizer["AppSourceWeb"]],
        WingetApp wingetApp => [wingetApp.Id, localizer["AppSourceWinget"]],
        WingetBundleApp wingetBundleApp => [wingetBundleApp.FileName, localizer["AppSourceWingetBundle"]],
        _ => throw new NotImplementedException(localizer["AppTypeNotImplementedError"])
    };

    private void LogResource(LogLevel level, string resourceName, params string[]? parameters) {
        if (!logger.IsEnabled(level)) return;

        var template = localizer[resourceName];
        var message = parameters is not null ? string.Format(template, parameters) : template;

        logger.Log(level, "{Message}", message);
    }
}
