using NitroWin.Core.Helpers;
using NitroWin.Core.Models;
using YamlDotNet.Serialization;

namespace NitroWin.Core.Services;

public sealed class ConfigService(IDeserializer deserializer, LogService logService) {
    private Config? _config;
    private AppInstallerConfig? _appInstallerConfig;

    public async Task<Config> GetAsync(CancellationToken cancellationToken = default) =>
        _config ??= await LoadAsync<Config>(GetConfigFilePath<Config>(), cancellationToken);

    public async Task<AppInstallerConfig> GetAppInstallerAsync(CancellationToken cancellationToken = default) =>
        _appInstallerConfig ??= await LoadAsync<AppInstallerConfig>(
            GetConfigFilePath<AppInstallerConfig>(), cancellationToken);

    private static string GetConfigFilePath<T>() {
        var fileName = typeof(T) switch {
            _ when typeof(T) == typeof(AppInstallerConfig) => "Apps.yml",
            _ when typeof(T) == typeof(Config) => "Config.yml",
            _ => throw new NotSupportedException()
        };

        var localPath = Path.Combine(
            AppContext.BaseDirectory, "Configuration", fileName);

        var programDataPath = Path.Combine(
            Paths.ConfigPath, fileName);

        if (File.Exists(localPath)) {
            if (!File.Exists(programDataPath)) {
                Directory.CreateDirectory(Path.GetDirectoryName(programDataPath) ?? throw new InvalidOperationException());
                File.Copy(localPath, programDataPath);
            }

            return localPath;
        } else
            return programDataPath;
    }

    private async Task<T> LoadAsync<T>(string path, CancellationToken cancellationToken) where T : ConfigBase, new() {
        if (!File.Exists(path)) {
            logService.NoConfigFound<T>();
            return new T();
        }

        try {
            var content = await File.ReadAllTextAsync(path, cancellationToken);
            return deserializer.Deserialize<T>(content) ?? new T();
        } catch (Exception ex) {
            logService.ConfigError<T>(ex);
            return new T();
        }
    }
}
