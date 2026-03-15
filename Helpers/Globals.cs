using System.Reflection;
using System.Resources;

namespace NitroWin.Helpers
{
    public static class Globals
    {
        public const string DownloadFolder = "Downloads";
        public static readonly ResourceManager ResourceManager = new("NitroWin.Resources", Assembly.GetExecutingAssembly());
    }
}
