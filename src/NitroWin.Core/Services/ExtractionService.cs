using System.IO.Compression;

namespace NitroWin.Core.Services;

public sealed class ExtractionService(LogService logService) {
    internal async Task ExtractZipFile(string filePath, string outputPath,
        CancellationToken cancellationToken = default) {
        try {
            await ZipFile.ExtractToDirectoryAsync(filePath, outputPath, true, cancellationToken);
        } catch (Exception ex) {
            logService.ExtractionError(filePath, ex);
        }
    }
}
