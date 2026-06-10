namespace NitroWin.Services;

public sealed class DownloaderService(LogService logService, HttpClient httpClient) {
    internal async Task<string?> DownloadFileAsync(string url, string downloadPath, string? fileName = null, CancellationToken cancellationToken = default) {
        try {
            var fullPath = Path.Combine(downloadPath, fileName ?? new Uri(url).Segments.Last());

            Directory.CreateDirectory(downloadPath);

            using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            using var downloadStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: true);

            await downloadStream.CopyToAsync(fileStream, 81920, cancellationToken);

            return fullPath;
        } catch (Exception ex) {
            logService.DownloadError(url, ex);
            return null;
        }
    }
}
