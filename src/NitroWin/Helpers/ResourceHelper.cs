using System.Reflection;
using System.Resources;

namespace NitroWin.Helpers
{
    internal static class ResourceHelper
    {
        private static readonly ResourceManager StringsResourceManager = new("NitroWin.Resources.Strings", Assembly.GetExecutingAssembly());

        internal static string GetString(string resourceName) => StringsResourceManager.GetString(resourceName) ?? string.Empty;
    }
}
