namespace NitroWin {
    public class Helper {
        public static string WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public async static Task DownloadFile(string fileUrl, string downloadPath, string fileName) {
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
        public async static Task<string> DownloadFileToString(string fileUrl) {
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(fileUrl);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Error while fetching content from {fileUrl}. Status code: {response.StatusCode}.");
            }
        }
        public static void InsertIntoFile(string filePath, string textToInsert, string insertPoint) {
            string text = File.ReadAllText(filePath);
            int index = text.IndexOf(insertPoint) + insertPoint.Length;
            text = text.Insert(index, Environment.NewLine + textToInsert);
            File.WriteAllText(filePath, text);
        }
    }
}