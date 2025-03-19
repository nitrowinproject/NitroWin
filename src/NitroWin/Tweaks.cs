namespace NitroWin {
    public class Tweaks {
        public static void Merge() {
            if (Directory.Exists("Tweaks")) {
                String mergeDirectory = "Tweaks";
                
                String mergedRegistryFile = "NitroWin.Tweaks.reg";
                String mergedBatchFile = "NitroWin.Tweaks.bat";
                String mergedPowerShellFile = "NitroWin.Tweaks.ps1";

                MergeRegistryFiles(mergedRegistryFile, mergeDirectory);
                MergeFiles(mergedBatchFile, mergeDirectory, "bat");
                MergeFiles(mergedPowerShellFile, mergeDirectory, "ps1");
            }
            else {
                Console.WriteLine("Download comming soon. Please download the Tweaks folder manually.");
            }
        }
        private static void MergeRegistryFiles(String mergedFile, String registryDirectory) {
            if (File.Exists(mergedFile))
            {
                File.Delete(mergedFile);
            }

            File.WriteAllText(mergedFile, "Windows Registry Editor Version 5.00" + Environment.NewLine);

            var registryFiles = Directory.GetFiles(registryDirectory, "*.reg", SearchOption.AllDirectories);

            foreach (var file in registryFiles) {
                var lines = File.ReadLines(file).Skip(1);
                File.AppendAllLines(mergedFile, lines);
            }
        }
        private static void MergeFiles(String mergedFile, String fileDirectory, string fileExtension) {
            if (File.Exists(mergedFile))
            {
                File.Delete(mergedFile);
            }

            var mergeFiles = Directory.GetFiles(fileDirectory, $"*.{fileExtension}", SearchOption.AllDirectories);

            foreach (var file in mergeFiles) {
                var lines = File.ReadLines(file);
                File.AppendAllLines(mergedFile, lines);
            }
        }
    }
}