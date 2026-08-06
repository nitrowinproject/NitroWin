using System.Resources;
using Microsoft.Extensions.Logging;
using NitroWin.Core.Models;
using NitroWin.Core.Models.Apps;

namespace NitroWin.Core.Services;

public sealed class LogService(ResourceManager resourceManager, ILogger<LogService> logger) {
    internal void InstallingApp(AppBase app) => LogResource(
        LogLevel.Information, "Log_InstallingApp", GetAppParameters(app));

#if DEBUG
    internal void NotInstallingApp(AppBase app) => LogResource(
        LogLevel.Debug, "Log_NotInstallingApp", GetAppParameters(app));
#endif

    internal void AppInstallError(AppBase app, Exception exception) => LogResource(
        LogLevel.Error, "Log_AppInstallError", [.. GetAppParameters(app),
            exception.Message]);

#if DEBUG
    internal void ApplyingTweak(Tweak tweak) =>
        LogResource(LogLevel.Debug, "Log_ApplyingTweak", tweak.Title);
#endif

#if DEBUG
    internal void AppliedTweak(Tweak tweak) =>
        LogResource(LogLevel.Debug, "Log_AppliedTweak", tweak.Title);
#endif

    internal void TweakApplyError(Tweak tweak, Exception exception) => LogResource(
        LogLevel.Error, "Log_TweakApplyError", tweak.Title,
        exception.Message);

    internal void TweakReadError(string filePath, Exception exception) => LogResource(
        LogLevel.Error, "Log_TweakReadError", Path.GetFileName(filePath),
        exception.Message);

    internal void DownloadError(string url, Exception exception) => LogResource(
        LogLevel.Error, "Log_DownloadError", Path.GetFileName(url),
        exception.Message);

    internal void ExtractionError(string filePath, Exception exception) => LogResource(
        LogLevel.Error, "Log_ExtractionError", Path.GetFileName(filePath),
        exception.Message);

    internal void NoNetworkError() =>
        LogResource(LogLevel.Error, "Log_NoNetwork");

#if DEBUG
    internal void CommandLineArguments(string[] args) =>
        LogResource(LogLevel.Debug, "Log_CommandLineArguments", string.Join(", ", args));
#endif

    internal void NoConfigFound<T>() where T : ConfigBase =>
        LogResource(LogLevel.Warning, typeof(T) == typeof(AppInstallerConfig)
            ? "Log_NoAppInstallerConfigFound" : "Log_NoConfigFound");

    internal void ConfigError<T>(Exception exception) where T : ConfigBase =>
        LogResource(LogLevel.Error, typeof(T) == typeof(AppInstallerConfig)
            ? "Log_AppInstallerConfigError" : "Log_ConfigError", exception.Message);

    internal void InstallingApps() =>
        LogResource(LogLevel.Information, "Log_InstallingApps");

#if DEBUG
    internal void HelloFrom(string app, string version) =>
        LogResource(LogLevel.Debug, "Log_HelloFrom", app, version);
#endif

    internal void DownloadingTweaks() =>
        LogResource(LogLevel.Information, "Log_DownloadingTweaks");

    internal void ApplyingTweaks() =>
        LogResource(LogLevel.Information, "Log_ApplyingTweaks");

    private string[] GetAppParameters(AppBase app) => app switch {
        AppxApp appxApp => [appxApp.Name ?? Path.GetFileName(appxApp.Path), resourceManager.GetString("AppSource_Appx")!],
        AppxWebApp appxWebApp => [appxWebApp.Name ?? Path.GetFileName(appxWebApp.Url), resourceManager.GetString("AppSource_AppxWeb")!],
        ChocolateyApp chocolateyApp => [chocolateyApp.Id, resourceManager.GetString("AppSource_Chocolatey")!],
        ChocolateyBundleApp chocolateyBundleApp => [chocolateyBundleApp.FileName, resourceManager.GetString("AppSource_ChocolateyBundle")!],
        WebApp webApp => [webApp.Name ?? Path.GetFileName(webApp.Url), resourceManager.GetString("AppSource_Web")!],
        WingetApp wingetApp => [wingetApp.Id, resourceManager.GetString("AppSource_Winget")!],
        WingetBundleApp wingetBundleApp => [wingetBundleApp.FileName, resourceManager.GetString("AppSource_WingetBundle")!],
        _ => throw new NotImplementedException()
    };

    private void LogResource(LogLevel level, string resourceName, params string[]? parameters) {
        if (!logger.IsEnabled(level)) return;

        var template = resourceManager.GetString(resourceName)!;
        var message = parameters is not null ? string.Format(template, parameters) : template;

        logger.Log(level, "{Message}", message);
    }
}
