namespace NitroWin.Helpers
{
    internal static class ExtractionHelper
    {
        internal static async Task ExtractZipFile(string filePath, string outputPath)
        {
            try
            {
                await System.IO.Compression.ZipFile.ExtractToDirectoryAsync(filePath, outputPath, overwriteFiles: true);
            }
            catch (Exception ex)
            {
                LogHelper.ExtractionError(filePath, ex);
            }
        }
    }
}
