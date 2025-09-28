# :wrench: · Configuration

To select the apps you want to install, you need to use the `NitroWin.json` file.

When you run NitroWin, it looks for the file `NitroWin.json` in the root folder of all your storage devices. If you have multiple drives containing this file, the file from the drive with an earlier letter in the alphabet will be chosen.

If the program isn't able to find the configuration file, it will use the [default/recommended configuration](https://raw.githubusercontent.com/nitrowinproject/NitroWin/refs/heads/main/assets/Configuration/NitroWin.json).

## :monocle_face: · How do I configure it?

1. Create a file named `NitroWin.json` in the root directory of your installation media:

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
