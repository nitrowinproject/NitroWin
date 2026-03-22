using NitroWin.Apps;
using Serilog;

namespace NitroWin.Helpers
{
    public static class LogHelper
    {
        public static void InstallingApp(AppBase app)
        {
            string[] parameters = app switch
            {
                WebApp webApp => [webApp.Name ?? webApp.Url, Globals.StringsResourceManager.GetString("AppSource_Web")!],
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

        public static void AppInstallError(AppBase app, Exception exception)
        {
            string[] parameters = app switch
            {
                WebApp webApp => [webApp.Name ?? webApp.Url, Globals.StringsResourceManager.GetString("AppSource_Web")!],
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
    }
}
