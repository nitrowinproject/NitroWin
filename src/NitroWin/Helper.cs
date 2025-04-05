namespace NitroWin {
    public class Helper {
        public static string NitroWinDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NitroWin");
        public async static Task DownloadFile(string fileUrl, string downloadPath, string fileName) {
            Directory.CreateDirectory(downloadPath);
            string savePath = Path.Combine(downloadPath, fileName);

            using (HttpClient client = new HttpClient()) {
                HttpResponseMessage response = await client.GetAsync(fileUrl);

                if (response.IsSuccessStatusCode) {
                    using (var fs = new FileStream(savePath, FileMode.Create, FileAccess.Write)) {
                        await response.Content.CopyToAsync(fs);
                    }
                }
                else {
                    throw new Exception($"Error while downloading {fileUrl}. Status code: {response.StatusCode}.");
                }
            }
        }
    }
}