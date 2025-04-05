using System.Runtime.InteropServices;

namespace NitroWin.Installer {
    public class Helper {
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
        private static readonly Guid DownloadsFolderGuid = new("374DE290-123F-4565-9164-39C4925E467B");
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
        private static extern string SHGetKnownFolderPath(
            [MarshalAs(UnmanagedType.LPStruct)] Guid rfid,
            uint dwFlags,
            nint hToken = 0);
        public static string GetDownloadsFolderPath()
        {
            return SHGetKnownFolderPath(DownloadsFolderGuid, 0);
        }
        public static void NetworkConnectionPrompt()
        {
            while (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                Console.WriteLine("No network connection available. Please connect to the internet and press any key to continue...");
                Console.ReadKey(true);
            }
        }
        public static bool Prompt(string message) {
            Console.WriteLine($"{message} (y/n)");
            string input = Console.ReadLine()?.ToLower();
            return input == "y";
        }
    }
}