# :wrench: · Configuration

To select the apps you want to install, you need to use the `NitroWin.Apps.json` file.

When you run NitroWin, it looks for the file `NitroWin.Apps.json` in the root folder of all your storage devices. If you have multiple drives containing this file, the file from the drive with an earlier letter in the alphabet will be chosen.

## :monocle_face: · How do I configure it?

1. Create a file named `NitroWin.Apps.json` in the root directory of your installation media:

    ```json
    {
        "apps": [

        ]
    }
    ```

2. Add applications to it.

    I. For applications that require a download from the internet:

    ```json
    {
        "source": "web",
        "url": "https://link/to/file.exe",
        "args": ["/your", "/args"],
        "arch": ["x64", "x86", "arm64"]
    }
    ```

    II. For applications that can be downloaded with WinGet:

    ```json
    {
        "source": "winget",
        "id": "Your.App",
        "args": "--scope machine",
        "arch": ["x64", "x86", "arm64"]
    }
    ```

> [!WARNING]
> For a URL to work, it needs to end in .exe.

## :sunglasses: · Default/recommended configuration

Here's the default/recommended configuration:

```json
{
  "apps": [
    {
      "source": "web",
      "url": "https://aka.ms/vs/17/release/vc_redist.x64.exe",
      "args": ["/install", "/passive", "/norestart"],
      "arch": ["x64", "x86", "arm64"]
    },
    {
      "source": "web",
      "url": "https://aka.ms/vs/17/release/vc_redist.x86.exe",
      "args": ["/install", "/passive", "/norestart"],
      "arch": ["x64", "x86", "arm64"]
    },
    {
      "source": "web",
      "url": "https://aka.ms/vs/17/release/vc_redist.arm64.exe",
      "args": ["/install", "/passive", "/norestart"],
      "arch": ["arm64"]
    },
    {
      "source": "winget",
      "id": "Brave.Brave",
      "args": "--scope machine",
      "arch": ["x64", "x86", "arm64"]
    },
    {
      "source": "winget",
      "id": "MartiCliment.UniGetUI",
      "args": "--scope machine",
      "arch": ["x64", "arm64"]
    },
    {
      "source": "winget",
      "id": "Greenshot.Greenshot",
      "args": "--scope machine",
      "arch": ["x64", "arm64"]
    },
    {
      "source": "winget",
      "id": "KeePassXCTeam.KeePassXC",
      "args": "--scope machine",
      "arch": ["x64", "arm64"]
    },
    {
      "source": "winget",
      "id": "M2Team.NanaZip",
      "arch": ["x64", "arm64"]
    },
    {
      "source": "winget",
      "id": "Microsoft.WindowsTerminal",
      "arch": ["x64", "x86", "arm64"]
    },
    {
      "source": "winget",
      "id": "StartIsBack.StartAllBack",
      "args": "--scope machine",
      "arch": ["x64", "arm64"]
    },
    {
      "source": "winget",
      "id": "VideoLAN.VLC",
      "args": "--scope machine",
      "arch": ["x64", "x86", "arm64"]
    }
  ]
}
```
