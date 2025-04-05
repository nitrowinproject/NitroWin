namespace NitroWin {
    public class InstallMedia {
        public static void Deploy() {
            string driveLetter = PromptDriveLetter();

            Directory.CreateDirectory(Path.Combine(driveLetter, "NitroWin"));

            var folders = new[] {
                "Tools",
                "Tweaks"
            };

            foreach (string folder in folders) {
                try {
                    Directory.CreateDirectory(Path.Combine(driveLetter, "NitroWin", folder));
                }
                catch (Exception ex) {
                    throw new Exception($"Failed to create directory {Path.Combine(driveLetter, "NitroWin", folder)}", ex);
                }
            }

            var files = new (string file, string destination)[] {
                (Path.Combine(Helper.NitroWinDirectory, "autounattend.xml"), Path.Combine(driveLetter, "autounattend.xml")),
                (Path.Combine(Helper.NitroWinDirectory, "NitroWin.Installer.exe"), Path.Combine(driveLetter, "NitroWin", "NitroWin.Installer.exe")),
                (Path.Combine(Helper.NitroWinDirectory, "Apps.txt"), Path.Combine(driveLetter, "NitroWin", "Apps.txt")),
                (Path.Combine(Helper.NitroWinDirectory, "PsExec64.exe"), Path.Combine(driveLetter, "NitroWin", "Tools", "PsExec64.exe")),
                (Path.Combine(Helper.NitroWinDirectory, "NitroWin.Tweaks.User.ps1"), Path.Combine(driveLetter, "NitroWin", "Tweaks", "NitroWin.Tweaks.User.ps1")),
                (Path.Combine(Helper.NitroWinDirectory, "NitroWin.Tweaks.User.reg"), Path.Combine(driveLetter, "NitroWin", "Tweaks", "NitroWin.Tweaks.User.reg")),
                (Path.Combine(Helper.NitroWinDirectory, "NitroWin.Tweaks.System.ps1"), Path.Combine(driveLetter, "NitroWin", "Tweaks", "NitroWin.Tweaks.System.ps1")),
                (Path.Combine(Helper.NitroWinDirectory, "NitroWin.Tweaks.System.reg"), Path.Combine(driveLetter, "NitroWin", "Tweaks", "NitroWin.Tweaks.System.reg"))
            };

            foreach (var (source, destination) in files) {
                try {
                    File.Copy(source, destination, true);
                }
                catch (Exception ex) {
                    throw new FileLoadException($"Failed to copy {source} to {destination}", ex);
                }
            }
        }
        private static string PromptDriveLetter() {
            while (true) {
                Console.WriteLine("Please enter the drive letter of your installation media (e.g. d:\\): ");
                string? driveLetter = Console.ReadLine();
                if (driveLetter != null && driveLetter.Length == 3) {
                    return driveLetter;
                }
            }
        }
    }
}