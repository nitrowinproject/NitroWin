using System.Runtime.InteropServices;

namespace NitroWin.Helpers;

internal static class PlatformHelper {
    [StructLayout(LayoutKind.Sequential)]
    private struct SYSTEM_POWER_CAPABILITIES {
        [MarshalAs(UnmanagedType.U1)] public bool PowerButtonPresent;
        [MarshalAs(UnmanagedType.U1)] public bool SleepButtonPresent;
        [MarshalAs(UnmanagedType.U1)] public bool LidPresent;
        [MarshalAs(UnmanagedType.U1)] public bool SystemS1;
        [MarshalAs(UnmanagedType.U1)] public bool SystemS2;
        [MarshalAs(UnmanagedType.U1)] public bool SystemS3;
        [MarshalAs(UnmanagedType.U1)] public bool SystemS4;
        [MarshalAs(UnmanagedType.U1)] public bool SystemS5;
        [MarshalAs(UnmanagedType.U1)] public bool HiberFilePresent;
        [MarshalAs(UnmanagedType.U1)] public bool FullWake;
        [MarshalAs(UnmanagedType.U1)] public bool VideoDimPresent;
        [MarshalAs(UnmanagedType.U1)] public bool ApmPresent;
        [MarshalAs(UnmanagedType.U1)] public bool UpsPresent;
        [MarshalAs(UnmanagedType.U1)] public bool ThermalControl;
        [MarshalAs(UnmanagedType.U1)] public bool ProcessorThrottle;

        public byte ProcessorMinThrottle;
        public byte ProcessorThrottleScale;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Spare2;

        public byte ProcessorMaxThrottle;

        [MarshalAs(UnmanagedType.U1)] public bool FastSystemS4;
        [MarshalAs(UnmanagedType.U1)] public bool Hiberboot;
        [MarshalAs(UnmanagedType.U1)] public bool WakeAlarmPresent;
        [MarshalAs(UnmanagedType.U1)] public bool AoAc;
        [MarshalAs(UnmanagedType.U1)] public bool DiskSpinDown;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Spare3;

        public byte SystemBatteriesPresent;
        public byte BatteriesAreShortTerm;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public BATTERY_REPORTING_SCALE[] BatteryScale;

        public SYSTEM_POWER_STATE AcOnLineWake;
        public SYSTEM_POWER_STATE SoftLidWake;
        public SYSTEM_POWER_STATE RtcWake;
        public SYSTEM_POWER_STATE MinDeviceWakeState;
        public SYSTEM_POWER_STATE DefaultLowLatencyWake;
    }

    private enum SYSTEM_POWER_STATE {
        PowerSystemUnspecified = 0,
        PowerSystemWorking = 1,
        PowerSystemSleeping1 = 2,
        PowerSystemSleeping2 = 3,
        PowerSystemSleeping3 = 4,
        PowerSystemHibernate = 5,
        PowerSystemShutdown = 6,
        PowerSystemMaximum = 7
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct BATTERY_REPORTING_SCALE {
        public uint Granularity;
        public uint Capacity;
    }

    [DllImport("PowrProf.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetPwrCapabilities(
        out SYSTEM_POWER_CAPABILITIES systemPowerCapabilities);

    internal static bool IsMobile() {
        try {
            return GetPwrCapabilities(out var caps) && caps.LidPresent;
        } catch {
            return true; // dont do desktop only tweaks if this fails
        }
    }
}
