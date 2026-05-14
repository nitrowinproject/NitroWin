namespace NitroWin.Services;

public sealed class DownloaderService(LogService logService, HttpClient httpClient) {
    internal async Task<string?> DownloadFileAsync(string url, string downloadPath, string? fileName = null) {
        try {
            var fullPath = Path.Combine(downloadPath, fileName ?? new Uri(url).Segments.Last());

            Directory.CreateDirectory(downloadPath);

            using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            using var downloadStream = await response.Content.ReadAsStreamAsync();
            using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: true);

            await downloadStream.CopyToAsync(fileStream);

            return fullPath;
        } catch (Exception ex) {
            logService.DownloadError(url, ex);
            return null;
        }
    }
}
