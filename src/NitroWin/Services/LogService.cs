using System.Resources;
using NitroWin.Models;
using NitroWin.Models.Apps;
using Serilog;
using Serilog.Events;

namespace NitroWin.Services;

public sealed class LogService(ResourceManager resourceManager) {
    internal void InstallingApp(AppBase app) => LogResource(
        LogEventLevel.Information, "Log_InstallingApp", GetAppParameters(app));

    internal void NotInstallingApp(AppBase app) => LogResource(
        LogEventLevel.Debug, "Log_NotInstallingApp", GetAppParameters(app));

    internal void AppInstallError(AppBase app, Exception exception) => LogResource(
        LogEventLevel.Error, "Log_AppInstallError", [.. GetAppParameters(app),
            exception.Message]);

    internal void ApplyingTweak(Tweak tweak) => LogResource(
        LogEventLevel.Debug, "Log_ApplyingTweak", tweak.Title);

    internal void AppliedTweak(Tweak tweak) => LogResource(
        LogEventLevel.Debug, "Log_AppliedTweak", tweak.Title);

    internal void TweakApplyError(Tweak tweak, Exception exception) => LogResource(
        LogEventLevel.Error, "Log_TweakApplyError", tweak.Title,
        exception.Message);

    internal void TweakReadError(string filePath, Exception exception) => LogResource(
        LogEventLevel.Error, "Log_TweakReadError", Path.GetFileName(filePath),
        exception.Message);

    internal void DownloadError(string url, Exception exception) => LogResource(
        LogEventLevel.Error, "Log_DownloadError", Path.GetFileName(url),
        exception.Message);

    internal void ExtractionError(string filePath, Exception exception) => LogResource(
        LogEventLevel.Error, "Log_ExtractionError", Path.GetFileName(filePath),
        exception.Message);

    internal void NoNetworkError() =>
        LogResource(LogEventLevel.Error, "Log_NoNetwork");

    internal void CommandLineArguments(string[] args) =>
        LogResource(LogEventLevel.Debug, "Log_CommandLineArguments", string.Join(", ", args));

    internal void NoConfigFound<T>() where T : ConfigBase =>
        LogResource(LogEventLevel.Warning, typeof(T) == typeof(AppInstallerConfig)
            ? "Log_NoAppInstallerConfigFound" : "Log_NoConfigFound");

    internal void InstallingApps() =>
        LogResource(LogEventLevel.Information, "Log_InstallingApps");

    internal void HelloFrom(string app, string version) =>
        LogResource(LogEventLevel.Debug, "Log_HelloFrom", app, version);

    internal void DownloadingTweaks() =>
        LogResource(LogEventLevel.Information, "Log_DownloadingTweaks");

    internal void ApplyingTweaks() =>
        LogResource(LogEventLevel.Information, "Log_ApplyingTweaks");

    internal void CriticalError(Exception exception) =>
        LogResource(LogEventLevel.Fatal, "Log_CriticalError", exception.Message);

    private string[] GetAppParameters(AppBase app) => app switch {
        AppxApp appxApp => [appxApp.Name ?? Path.GetFileName(appxApp.Path), resourceManager.GetString("AppSource_Appx")!],
        AppxWebApp appxWebApp => [appxWebApp.Name ?? Path.GetFileName(appxWebApp.Url), resourceManager.GetString("AppSource_AppxWeb")!],
        ChocolateyApp chocolateyApp => [chocolateyApp.Id, resourceManager.GetString("AppSource_Chocolatey")!],
        WebApp webApp => [webApp.Name ?? Path.GetFileName(webApp.Url), resourceManager.GetString("AppSource_Web")!],
        WingetApp wingetApp => [wingetApp.Id, resourceManager.GetString("AppSource_Winget")!],
        _ => throw new NotImplementedException()
    };

    private void LogResource(LogEventLevel level, string resourceName, params string[] parameters) {
        var message = string.Format(
            resourceManager.GetString(resourceName)!,
            parameters
        );

        switch (level) {
            case LogEventLevel.Debug:
                Log.Debug(message);
                break;
            case LogEventLevel.Error:
                Log.Error(message);
                break;
            case LogEventLevel.Fatal:
                Log.Fatal(message);
                break;
            case LogEventLevel.Information:
                Log.Information(message);
                break;
            case LogEventLevel.Verbose:
                Log.Verbose(message);
                break;
            case LogEventLevel.Warning:
                Log.Warning(message);
                break;
            default:
                throw new NotImplementedException();
        }
    }
}
