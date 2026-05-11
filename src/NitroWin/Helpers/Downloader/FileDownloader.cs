namespace NitroWin.Helpers.Downloader;

internal static class FileDownloader {
    internal static async Task<string?> DownloadFileAsync(string url, string outputFolder) {
        try {
            Directory.CreateDirectory(outputFolder);

            using var response = await HttpClientProvider.s_client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var fileName = url.Split("/").Last();
            var filePath = Path.Combine(outputFolder, fileName);

            using var stream = await response.Content.ReadAsStreamAsync();
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: true);

            await stream.CopyToAsync(fileStream);

            return filePath;
        } catch (Exception ex) {
            LogHelper.DownloadError(url, ex);
            return null;
        }
    }
}
