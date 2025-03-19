namespace NitroWin {
    public class Helper {
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
                    throw new Exception($"Error while downloading {fileUrl}.");
                }
            }
        }
        public static bool Prompt(string message) {
            Console.WriteLine(message);
            Console.WriteLine("[y|N]");

            string? result = Console.ReadLine();

            return result switch {
                "y" => true,
                "n" => false,
                _ => false
            };
        }
    }
}