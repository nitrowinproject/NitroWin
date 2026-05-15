using System.Runtime.InteropServices;

namespace NitroWin.Helpers;

internal static class PlatformHelper {
    [StructLayout(LayoutKind.Sequential)]
    private struct SYSTEM_POWER_CAPABILITIES {
        [MarshalAs(UnmanagedType.I1)] public bool PowerButtonPresent;
        [MarshalAs(UnmanagedType.I1)] public bool SleepButtonPresent;
        [MarshalAs(UnmanagedType.I1)] public bool LidPresent;
        [MarshalAs(UnmanagedType.I1)] public bool SystemS1;
        [MarshalAs(UnmanagedType.I1)] public bool SystemS2;
        [MarshalAs(UnmanagedType.I1)] public bool SystemS3;
        [MarshalAs(UnmanagedType.I1)] public bool SystemS4;
        [MarshalAs(UnmanagedType.I1)] public bool SystemS5;
        [MarshalAs(UnmanagedType.I1)] public bool HiberFilePresent;
        [MarshalAs(UnmanagedType.I1)] public bool FullWake;
        [MarshalAs(UnmanagedType.I1)] public bool VideoDimPresent;
        [MarshalAs(UnmanagedType.I1)] public bool ApmPresent;
        [MarshalAs(UnmanagedType.I1)] public bool UpsPresent;
        [MarshalAs(UnmanagedType.I1)] public bool ThermalControl;
        [MarshalAs(UnmanagedType.I1)] public bool ProcessorThrottle;

        public byte ProcessorMinThrottle;
        public byte ProcessorThrottleScale;
        public byte[] spare2;
        public byte ProcessorMaxThrottle;

        [MarshalAs(UnmanagedType.I1)] public bool FastSystemS4;
        [MarshalAs(UnmanagedType.I1)] public bool Hiberboot;
        [MarshalAs(UnmanagedType.I1)] public bool WakeAlarmPresent;
        [MarshalAs(UnmanagedType.I1)] public bool AoAc;
        [MarshalAs(UnmanagedType.I1)] public bool DiskSpinDown;
    }

    [DllImport("PowrProf.dll", SetLastError = true)]
    private static extern bool GetPwrCapabilities(out SYSTEM_POWER_CAPABILITIES systemPowerCapabilities);

    internal static bool IsMobile() {
        if (GetPwrCapabilities(out var caps))
            return caps.LidPresent;

        return false;
    }
}
