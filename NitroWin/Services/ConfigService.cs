using NitroWin.Models;
using YamlDotNet.Serialization;

namespace NitroWin.Services;

public sealed class ConfigService(IDeserializer deserializer, LogService logService) {
    private Config? _config;
    private AppInstallerConfig? _appInstallerConfig;

    internal async Task<Config> GetAsync(CancellationToken cancellationToken = default) =>
        _config ??= await LoadAsync<Config>(Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Configuration", "Config.yml"), cancellationToken);

    internal async Task<AppInstallerConfig> GetAppInstallerAsync(CancellationToken cancellationToken = default) =>
        _appInstallerConfig ??= await LoadAsync<AppInstallerConfig>(Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Configuration", "Apps.yml"), cancellationToken);

    private async Task<T> LoadAsync<T>(string path, CancellationToken cancellationToken) where T : ConfigBase, new() {
        if (!File.Exists(path)) {
            logService.NoConfigFound<T>();
            return new T();
        }

        try {
            var content = await File.ReadAllTextAsync(path, cancellationToken);
            return deserializer.Deserialize<T>(content);
        } catch (Exception ex) {
            logService.ConfigError<T>(ex);
            return new T();
        }
    }
}
