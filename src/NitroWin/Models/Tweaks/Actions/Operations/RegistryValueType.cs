using Microsoft.Win32;

namespace NitroWin.Models.Tweaks.Actions.Operations;

public enum RegistryValueType {
    REG_SZ = RegistryValueKind.String,
    REG_MULTI_SZ = RegistryValueKind.MultiString,
    REG_DWORD = RegistryValueKind.DWord,
    REG_QWORD = RegistryValueKind.QWord,
    REG_BINARY = RegistryValueKind.Binary,
    REG_NONE = RegistryValueKind.None
}
