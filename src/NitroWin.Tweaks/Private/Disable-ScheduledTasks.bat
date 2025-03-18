SCHTASKS /Change /TN "\Microsoft\Windows\Application Experience\PcaPatchDbTask" /DISABLE
SCHTASKS /Change /TN "\Microsoft\Windows\AppxDeploymentClient\UCPD velocity" /DISABLE 2>nul
SCHTASKS /Change /TN "\Microsoft\Windows\Flighting\FeatureConfig\UsageDataReporting" /DISABLE 2>nul
REG DELETE "HKLM\System\CurrentControlSet\Control\Ubpm" /V "CriticalMaintenance_UsageDataReporting" /F