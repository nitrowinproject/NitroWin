using System.Runtime.InteropServices;

namespace NitroWin.Helpers;

internal partial class PlatformHelper {
    private enum POWER_PLATFORM_ROLE {
        PlatformRoleUnspecified = 0,
        PlatformRoleDesktop = 1,
        PlatformRoleMobile = 2,
        PlatformRoleWorkstation = 3,
        PlatformRoleEnterpriseServer = 4,
        PlatformRoleSOHOServer = 5,
        PlatformRoleAppliancePC = 6,
        PlatformRolePerformanceServer = 7,
        PlatformRoleSlate = 8,
        PlatformRoleMaximum = 9
    }

    [LibraryImport("PowrProf.dll")]
    private static partial POWER_PLATFORM_ROLE PowerDeterminePlatformRole();

    internal static bool IsDesktop() =>
        PowerDeterminePlatformRole() == POWER_PLATFORM_ROLE.PlatformRoleDesktop;

    internal static bool IsMobile() =>
        PowerDeterminePlatformRole() is POWER_PLATFORM_ROLE.PlatformRoleMobile
            or POWER_PLATFORM_ROLE.PlatformRoleSlate;
}
