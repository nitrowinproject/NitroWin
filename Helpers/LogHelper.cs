using NitroWin.Apps;
using Serilog;
using TweakLib.Models;

namespace NitroWin.Helpers
{
    public static class LogHelper
    {
        public static void InstallingApp(AppBase app)
        {
            string[] parameters = app switch
            {
                AppxApp appxApp => [appxApp.Name ?? Path.GetFileName(appxApp.Path), Globals.StringsResourceManager.GetString("AppSource_Appx")!],
                AppxWebApp appxWebApp => [appxWebApp.Name ?? Path.GetFileName(appxWebApp.Url), Globals.StringsResourceManager.GetString("AppSource_AppxWeb")!],
                WebApp webApp => [webApp.Name ?? Path.GetFileName(webApp.Url), Globals.StringsResourceManager.GetString("AppSource_Web")!],
                WingetApp wingetApp => [wingetApp.Id, Globals.StringsResourceManager.GetString("AppSource_Winget")!],
                _ => throw new NotImplementedException()
            };

            var message = string.Format(
                Globals.StringsResourceManager.GetString("Log_InstallingApp")!,
                parameters[0],
                parameters[1]
            );

            Log.Information(message);
        }

        public static void NotInstallingApp(AppBase app)
        {
            string[] parameters = app switch
            {
                AppxApp appxApp => [appxApp.Name ?? Path.GetFileName(appxApp.Path), Globals.StringsResourceManager.GetString("AppSource_Appx")!],
                AppxWebApp appxWebApp => [appxWebApp.Name ?? Path.GetFileName(appxWebApp.Url), Globals.StringsResourceManager.GetString("AppSource_AppxWeb")!],
                WebApp webApp => [webApp.Name ?? Path.GetFileName(webApp.Url), Globals.StringsResourceManager.GetString("AppSource_Web")!],
                WingetApp wingetApp => [wingetApp.Id, Globals.StringsResourceManager.GetString("AppSource_Winget")!],
                _ => throw new NotImplementedException()
            };

            var message = string.Format(
                Globals.StringsResourceManager.GetString("Log_NotInstallingApp")!,
                parameters[0],
                parameters[1]
            );

            Log.Debug(message);
        }

        public static void AppInstallError(AppBase app, Exception exception)
        {
            string[] parameters = app switch
            {
                AppxApp appxApp => [appxApp.Name ?? Path.GetFileName(appxApp.Path), Globals.StringsResourceManager.GetString("AppSource_Appx")!],
                AppxWebApp appxWebApp => [appxWebApp.Name ?? Path.GetFileName(appxWebApp.Url), Globals.StringsResourceManager.GetString("AppSource_AppxWeb")!],
                WebApp webApp => [webApp.Name ?? Path.GetFileName(webApp.Url), Globals.StringsResourceManager.GetString("AppSource_Web")!],
                WingetApp wingetApp => [wingetApp.Id, Globals.StringsResourceManager.GetString("AppSource_Winget")!],
                _ => throw new NotImplementedException()
            };

            var message = string.Format(
                Globals.StringsResourceManager.GetString("Log_AppInstallError")!,
                parameters[0],
                parameters[1],
                exception.Message
            );

            Log.Error(message);
        }

        public static void TweakApplyError(Tweak tweak, Exception exception)
        {
            var message = string.Format(
                Globals.StringsResourceManager.GetString("Log_TweakApplyError")!,
                tweak.Title,
                exception.Message
            );

            Log.Error(message);
        }

        public static void TweakReadError(string filePath, Exception exception)
        {
            var message = string.Format(
                Globals.StringsResourceManager.GetString("Log_TweakReadError")!,
                Path.GetFileName(filePath),
                exception.Message
            );

            Log.Error(message);
        }

        public static void DownloadError(string url, Exception exception)
        {
            var message = string.Format(
                Globals.StringsResourceManager.GetString("Log_DownloadError")!,
                Path.GetFileName(url),
                exception.Message
            );

            Log.Error(message);
        }

        public static void ExtractionError(string filePath, Exception exception)
        {
            var message = string.Format(
                Globals.StringsResourceManager.GetString("Log_ExtractionError")!,
                Path.GetFileName(filePath),
                exception.Message
            );

            Log.Error(message);
        }
    }
}
