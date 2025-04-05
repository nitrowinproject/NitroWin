namespace NitroWin {
    public class Helper {
        public static string NitroWinDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NitroWin");
        public async static Task DownloadFile(string fileUrl, string downloadPath, string fileName) {
            Directory.CreateDirectory(downloadPath);
            string savePath = Path.Combine(downloadPath, fileName);

            if (File.Exists(savePath)) {
                File.Delete(savePath);
            }

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
        public static bool Prompt(string message) {
            Console.WriteLine($"{message} (y/n)");
            string input = Console.ReadKey(false).Key switch {
                ConsoleKey.Y => "y",
                ConsoleKey.N => "n",
                _ => "n"
            };
            return input == "y";
        }
    }
}