using System.Diagnostics;

namespace NitroWin {
    public class AppInstaller {
        private async static Task DownloadFile(string fileUrl, string downloadPath, string fileName) {
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
        public async static Task InstallFromURL(string fileUrl, string name, string[] installerArgs) {
            string downloadPath = Path.GetTempPath();
            string fileName = name + ".exe";
            string savePath = Path.Combine(downloadPath, fileName);

            await DownloadFile(fileUrl, downloadPath, fileName);

            Process.Start(savePath, installerArgs);
        }
        public static void InstallFromWinGet(string id, string[] wingetArgs) {
            List<string> arguments = ["--id", id, "--exact", .. wingetArgs];

            Process.Start("winget.exe", string.Join(" ", arguments));
        }
        public async static Task InstallOOSU() {
            string oosuUrl = "https://dl5.oo-software.com/files/ooshutup10/OOSU10.exe";
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            await DownloadFile(oosuUrl, desktopPath, "OOSU10.exe");
        }
    }
}