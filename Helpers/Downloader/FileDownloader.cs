namespace NitroWin.Helpers.Downloader
{
    public static class FileDownloader
    {
        public static async Task<string> DownloadFileAsync(string url, string outputFolder)
        {
            using var response = await HttpClientProvider.Client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            string fileName = url.Split("/").Last();
            string filePath = Path.Combine(outputFolder, fileName);

            using var stream = await response.Content.ReadAsStreamAsync();
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: true);

            await stream.CopyToAsync(fileStream);

            return filePath;
        }
    }
}
