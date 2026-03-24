using NitroWin.Apps;
using Serilog;
using Serilog.Events;
using TweakLib.Models;

namespace NitroWin.Helpers
{
    internal static class LogHelper
    {
        internal static void InstallingApp(AppBase app) => LogResource(
            LogEventLevel.Information, "Log_InstallingApp", GetAppParameters(app));

        internal static void NotInstallingApp(AppBase app) => LogResource(
            LogEventLevel.Debug, "Log_NotInstallingApp", GetAppParameters(app));

        internal static void AppInstallError(AppBase app, Exception exception) => LogResource(
            LogEventLevel.Error, "Log_AppInstallError", [.. GetAppParameters(app),
                exception.Message]);

        internal static void TweakApplyError(Tweak tweak, Exception exception) => LogResource(
            LogEventLevel.Error, "Log_TweakApplyError", tweak.Title,
            exception.Message);

        internal static void TweakReadError(string filePath, Exception exception) => LogResource(
            LogEventLevel.Error, "Log_TweakReadError", Path.GetFileName(filePath),
            exception.Message);

        internal static void DownloadError(string url, Exception exception) => LogResource(
            LogEventLevel.Error, "Log_DownloadError", Path.GetFileName(url),
            exception.Message);

        internal static void ExtractionError(string filePath, Exception exception) => LogResource(
            LogEventLevel.Error, "Log_ExtractionError", Path.GetFileName(filePath),
            exception.Message);

        private static string[] GetAppParameters(AppBase app) => app switch
        {
            AppxApp appxApp => [appxApp.Name ?? Path.GetFileName(appxApp.Path), ResourceHelper.GetString("AppSource_Appx")],
            AppxWebApp appxWebApp => [appxWebApp.Name ?? Path.GetFileName(appxWebApp.Url), ResourceHelper.GetString("AppSource_AppxWeb")],
            ChocolateyApp chocolateyApp => [chocolateyApp.Id, ResourceHelper.GetString("AppSource_Chocolatey")],
            WebApp webApp => [webApp.Name ?? Path.GetFileName(webApp.Url), ResourceHelper.GetString("AppSource_Web")],
            WingetApp wingetApp => [wingetApp.Id, ResourceHelper.GetString("AppSource_Winget")],
            _ => throw new NotImplementedException()
        };

        private static void LogResource(LogEventLevel level, string resourceName, params string[] parameters)
        {
            var message = string.Format(
                ResourceHelper.GetString(resourceName),
                parameters
            );

            switch (level)
            {
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
}
