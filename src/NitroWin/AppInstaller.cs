using System.Diagnostics;

namespace NitroWin {
    public class AppInstaller {
        public async static Task InstallFromURL(string fileUrl, string name, string[] installerArgs) {
            string downloadPath = Path.GetTempPath();
            string fileName = name + ".exe";
            string savePath = Path.Combine(downloadPath, fileName);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(fileUrl);

                if (response.IsSuccessStatusCode)
                {
                    using (var fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                }
                else
                {
                    throw new Exception($"Error while downloading {name}.");
                }
            }
            Process.Start(savePath, installerArgs);
        }
        public static void InstallFromWinGet(string id, string[] wingetArgs) {
            List<string> arguments = ["--id", id, "--exact", .. wingetArgs];

            Process.Start("winget.exe", string.Join(" ", arguments));
        }
    }
}