using Microsoft.Win32;
using NitroWin.Helpers;
using NitroWin.Models.Tweaks.Actions.Operations;

namespace NitroWin.Models.Tweaks.Actions;

public sealed class RegistryValueAction : ActionBase {
    public required string Path { get; set; }
    public required string Value { get; set; }
    public string? Data { get; set; }
    public RegistryValueType? Type { get; set; }
    public RegistryValueOperation Operation { get; set; } = RegistryValueOperation.Modify;

    private void ApplyAsCurrentUserElevated() {
        var parts = Path.Split('\\').ToList();
        var hive = parts[0];
        parts.RemoveAt(0);

        var subKey = string.Join("\\", parts);

        if (Operation == RegistryValueOperation.Delete) {
            var baseKey = hive switch {
                "HKCC" or "HKEY_CURRENT_CONFIG" => Registry.CurrentConfig,
                "HKCR" or "HKEY_CLASSES_ROOT" => Registry.ClassesRoot,
                "HKCU" or "HKEY_CURRENT_USER" => Registry.CurrentUser,
                "HKLM" or "HKEY_LOCAL_MACHINE" => Registry.LocalMachine,
                "HKU" or "HKEY_USERS" => Registry.Users,
                _ => throw new NotImplementedException()
            };

            using var key = baseKey.OpenSubKey(subKey, writable: true);
            key?.DeleteValue(Value, false);

            return;
        }

        var baseName = hive switch {
            "HKCC" => "HKEY_CURRENT_CONFIG",
            "HKCR" => "HKEY_CLASSES_ROOT",
            "HKCU" => "HKEY_CURRENT_USER",
            "HKLM" => "HKEY_LOCAL_MACHINE",
            "HKU" => "HKEY_USERS",
            _ => hive
        };

        var keyName = baseName + "\\" + subKey;

        switch (Type) {
            case RegistryValueType.REG_SZ:
                Registry.SetValue(keyName, Value, Data ?? string.Empty, (RegistryValueKind)Type);
                break;

            case RegistryValueType.REG_MULTI_SZ:
                var data = string.IsNullOrEmpty(Data)
                    ? []
                    : Data.Split('\0');

                Registry.SetValue(keyName, Value, data, (RegistryValueKind)Type);
                break;

            case RegistryValueType.REG_DWORD:
                Registry.SetValue(keyName, Value, unchecked((int)Convert.ToUInt32(Data)), (RegistryValueKind)Type);
                break;

            case RegistryValueType.REG_QWORD:
                Registry.SetValue(keyName, Value, Convert.ToUInt64(Data), (RegistryValueKind)Type);
                break;

            case RegistryValueType.REG_BINARY:
                var binary = RegistryHelper.StringToByteArray(Data ?? string.Empty);
                Registry.SetValue(keyName, Value, binary, (RegistryValueKind)Type);
                break;

            case RegistryValueType.REG_NONE:
                Registry.SetValue(keyName, Value, Array.Empty<byte>(), (RegistryValueKind)Type);
                break;
        }
    }

    private async Task<int> ApplyAsTrustedInstallerAsync(CancellationToken cancellationToken) {
        var type = Type switch {
            RegistryValueType.REG_SZ => "REG_SZ",
            RegistryValueType.REG_MULTI_SZ => "REG_MULTI_SZ",
            RegistryValueType.REG_DWORD => "REG_DWORD",
            RegistryValueType.REG_QWORD => throw new NotSupportedException(),
            RegistryValueType.REG_BINARY => "REG_BINARY",
            RegistryValueType.REG_NONE => throw new NotSupportedException(),
            _ => Operation switch {
                RegistryValueOperation.Delete => null,
                _ => throw new NotSupportedException()
            }
        };

        var arguments = Operation switch {
            RegistryValueOperation.Modify => $"add \"{Path}\" /v \"{Value}\" /t {type} /d \"{Data}\" /f",
            RegistryValueOperation.Delete => $"delete \"{Path}\" /v \"{Value}\" /f",
            _ => throw new NotImplementedException()
        };

        return await ProcessHelper.StartProcessAsync("reg.exe", arguments, true, Privilege.TrustedInstaller, cancellationToken);
    }

    protected override async Task<int> ApplyAsyncCore(CancellationToken cancellationToken) {
        if (Operation == RegistryValueOperation.Delete && Type is not null)
            throw new NotImplementedException();

        switch (RunAs) {
            case Privilege.CurrentUserElevated:
                await Task.Run(ApplyAsCurrentUserElevated, cancellationToken);
                return 0;

            case Privilege.TrustedInstaller:
                return await ApplyAsTrustedInstallerAsync(cancellationToken);

            default:
                throw new NotImplementedException();
        }
    }
}
