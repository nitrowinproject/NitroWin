namespace NitroWin.Services;

public sealed class ExtractionService(LogService logService) {
    internal async Task ExtractZipFile(string filePath, string outputPath, CancellationToken cancellationToken = default) {
        try {
            await System.IO.Compression.ZipFile.ExtractToDirectoryAsync(filePath, outputPath, overwriteFiles: true, cancellationToken);
        } catch (Exception ex) {
            logService.ExtractionError(filePath, ex);
        }
    }
}
