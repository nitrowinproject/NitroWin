namespace NitroWin.Helpers
{
    public static class ExtractionHelper
    {
        public static async Task ExtractZipFile(string filePath, string outputPath)
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
