namespace NitroWin.Core.Helpers;

internal static class RegistryHelper {
    internal static byte[] StringToByteArray(string hex) =>
        [.. Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))];
}
