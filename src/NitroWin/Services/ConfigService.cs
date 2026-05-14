using NitroWin.Models;
using YamlDotNet.Serialization;

namespace NitroWin.Services;

internal sealed class ConfigService(IDeserializer deserializer, LogService logService) {
    private Config? _config;
    private AppInstallerConfig? _appInstallerConfig;

    internal async Task<Config> GetAsync() =>
        _config ??= await LoadAsync(Path.Combine("Configuration", "Config.yml"));

    private async Task<Config> LoadAsync(string path) {
        if (!File.Exists(path)) {
            logService.NoConfigFound<Config>();
            return new Config();
        }

        try {
            var content = await File.ReadAllTextAsync(path);
            return deserializer.Deserialize<Config>(content);
        } catch {
            logService.NoConfigFound<Config>();
            return new Config();
        }
    }

    internal async Task<AppInstallerConfig> GetAppInstallerAsync() =>
        _appInstallerConfig ??= await LoadAppInstallerAsync(Path.Combine("Configuration", "Apps.yml"));

    private async Task<AppInstallerConfig> LoadAppInstallerAsync(string path) {
        if (!File.Exists(path)) {
            logService.NoConfigFound<AppInstallerConfig>();
            return new AppInstallerConfig();
        }

        try {
            var content = await File.ReadAllTextAsync(path);
            return deserializer.Deserialize<AppInstallerConfig>(content);
        } catch {
            logService.NoConfigFound<AppInstallerConfig>();
            return new AppInstallerConfig();
        }
    }
}
