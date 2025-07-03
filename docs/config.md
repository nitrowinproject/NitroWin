# :wrench: · Configuration

To select the apps you want to install, you need to use the NitroWin.Apps.txt file.

When you run NitroWin, it looks for the file NitroWin.Apps.txt in the root folder of all your storage devices. If you have multiple drives containing this file, the file from the drive with an earlier letter in the alphabet will be chosen.

## :monocle_face: · How do I configure it?

1. Create a file named `NitroWin.Apps.txt` in the root directory of your installation media.

2. Add applications to it.

    > [!WARNING]
    > For a URL to work, it needs to end in .exe.

    I. For applications that require a download from the internet:

        web;URL

    II. For applications that can be downloaded with WinGet:

        winget;ID

## :sunglasses: · Default/recommended configuration

Here's the default/recommended configuration:

```txt
web;https://aka.ms/vs/17/release/vc_redist.x64.exe
web;https://aka.ms/vs/17/release/vc_redist.x86.exe
web;https://laptop-updates.brave.com/latest/winx64.exe
web;https://github.com/marticliment/UniGetUI/releases/latest/download/UniGetUI.Installer.exe
winget;Greenshot.Greenshot
winget;KeePassXCTeam.KeePassXC
winget;M2Team.NanaZip
winget;Microsoft.WindowsTerminal
winget;StartIsBack.StartAllBack
winget;VideoLAN.VLC
```
