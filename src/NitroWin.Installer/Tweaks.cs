using System.Diagnostics;

namespace NitroWin.Installer {
    public class Tweaks {
        private static string tweakPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tweaks");
        private enum TweakType {
            User,
            System
        }
        private static Dictionary<TweakType, string[]> tweakFiles = new Dictionary<TweakType, string[]>() {
            { TweakType.User, new string[] { Path.Combine(tweakPath, "NitroWin.Tweaks.User.ps1"), Path.Combine(tweakPath, "NitroWin.Tweaks.User.reg") } },
            { TweakType.System, new string[] { Path.Combine(tweakPath, "NitroWin.Tweaks.System.ps1"), Path.Combine(tweakPath, "NitroWin.Tweaks.System.reg") } }
        };
        public static void ApplyUserTweaks() {
            CheckTweaks();
            
            ApplyTweaks(tweakFiles[TweakType.User]);
        }
        public static void ApplySystemTweaks() {
            CheckTweaks();

            if (Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") == "AMD64") {
                ApplySystemTweaksPsExec();
            }
            else {
                Console.WriteLine("WARNING: PsExec doesn't support arm-based CPUs yet. The tweaks will be imported as an admin, which may not import all tweaks as expected.");
                ApplyTweaks(tweakFiles[TweakType.System]);
            }
        }
        private static void CheckTweaks() {
            if (Directory.Exists(tweakPath)) {
                string[] files = tweakFiles[TweakType.User].Concat(tweakFiles[TweakType.System]).ToArray();

                foreach (string file in files) {
                    if (!File.Exists(file)) {
                        throw new FileNotFoundException("Tweak not found, please re-run NitroWin.exe with internet access.", file);
                    }
                }
            }
            else {
                throw new FileNotFoundException("Tweaks folder not found, please re-run NitroWin.exe with internet access.", tweakPath);
            }
        }
        private static void ApplySystemTweaksPsExec() {
            string psexecPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tools", "psexec64.exe");

            if (!File.Exists(psexecPath)) {
                throw new FileNotFoundException("PsExec not found, please re-run NitroWin.exe with internet access.", psexecPath);
            }

            foreach (string file in tweakFiles[TweakType.System]) {
                if (file.EndsWith(".ps1")) {
                    Process.Start(new ProcessStartInfo {
                        FileName = psexecPath,
                        Arguments = $"-accepteula -s -i powershell.exe -ExecutionPolicy Bypass -File \"{file}\"",
                        UseShellExecute = true,
                        Verb = "runas"
                    });
                }
                else if (file.EndsWith(".reg")) {
                    Process.Start(new ProcessStartInfo {
                        FileName = psexecPath,
                        Arguments = $"-accepteula -s -i reg.exe import \"{file}\"",
                        UseShellExecute = true,
                        Verb = "runas"
                    });
                }
            }
        }
        private static void ApplyTweaks(string[] tweaks) {
            foreach (string file in tweaks) {
                if (file.EndsWith(".ps1")) {
                    Process.Start(new ProcessStartInfo {
                        FileName = "powershell.exe",
                        Arguments = $"-ExecutionPolicy Bypass -File \"{file}\"",
                        UseShellExecute = true,
                        Verb = "runas"
                    });
                }
                else if (file.EndsWith(".reg")) {
                    Process.Start(new ProcessStartInfo {
                        FileName = "reg.exe",
                        Arguments = $"import \"{file}\"",
                        UseShellExecute = true,
                        Verb = "runas"
                    });
                }
            }
        }
    }
}