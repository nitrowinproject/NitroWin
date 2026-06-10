using NitroWin.Models;
using YamlDotNet.Serialization;

namespace NitroWin.Services;

public sealed class ConfigService(IDeserializer deserializer, LogService logService) {
    private Config? _config;
    private AppInstallerConfig? _appInstallerConfig;

    internal async Task<Config> GetAsync(CancellationToken cancellationToken = default) =>
        _config ??= await LoadAsync(Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Configuration", "Config.yml"), cancellationToken);

    private async Task<Config> LoadAsync(string path, CancellationToken cancellationToken = default) {
        if (!File.Exists(path)) {
            logService.NoConfigFound<Config>();
            return new Config();
        }

        try {
            var content = await File.ReadAllTextAsync(path, cancellationToken);
            return deserializer.Deserialize<Config>(content);
        } catch (Exception ex) {
            logService.ConfigError<Config>(ex);
            return new Config();
        }
    }

    internal async Task<AppInstallerConfig> GetAppInstallerAsync(CancellationToken cancellationToken = default) =>
        _appInstallerConfig ??= await LoadAppInstallerAsync(Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Configuration", "Apps.yml"), cancellationToken);

    private async Task<AppInstallerConfig> LoadAppInstallerAsync(string path, CancellationToken cancellationToken = default) {
        if (!File.Exists(path)) {
            logService.NoConfigFound<AppInstallerConfig>();
            return new AppInstallerConfig();
        }

        try {
            var content = await File.ReadAllTextAsync(path, cancellationToken);
            return deserializer.Deserialize<AppInstallerConfig>(content);
        } catch (Exception ex) {
            logService.ConfigError<AppInstallerConfig>(ex);
            return new AppInstallerConfig();
        }
    }
}
