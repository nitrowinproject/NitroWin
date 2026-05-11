using System.Reflection;
using System.Resources;

namespace NitroWin.Helpers;

internal static class ResourceHelper {
    private static readonly ResourceManager s_stringsResourceManager = new("NitroWin.Resources.Strings", Assembly.GetExecutingAssembly());

    internal static string GetString(string resourceName) => s_stringsResourceManager.GetString(resourceName) ?? string.Empty;
}
