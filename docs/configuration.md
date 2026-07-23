# :wrench: Configuring NitroWin

To configure NitroWin, there are two configuration files. Make sure they are named and placed **exactly** like the file structure below:

```text
    D:/
    ├── ...
    └── NitroWin/
        ├── NitroWin.exe
        └── Configuration/
            ├── Apps.yml <= Configure automatic app installation
            └── Config.yml <= Configure NitroWin itself
```

A good place to start are the default configuration files located [here](../dist/NitroWin/Configuration).

## :package: Apps.yml

This (not recommended) example configuration shows all of the options NitroWin has to offer:

```yaml
---
name: My Apps # Choose an appropriate name for your app list. This is optional.
author: nitrowinproject # It is recommended that you use your GitHub username here. This is optional.
apps:
  - !web: # Check out all the supported app types below.
    architectures: # You can block certain CPU architectures here. All app types support this.
      x64: false
    # arm64: false
    arguments: # An optional list of arguments to pass to the installer or package manager. All app types support this.
      - "/install"
      - "/passive"
      - "/norestart"
    name: Microsoft Visual C++ v14 Redistributable (arm64) # The name that will be shown inside the NitroWin console and log. This is not supported with package manager based apps.
    url: https://aka.ms/vc14/vc_redist.arm64.exe # Only supports exe files and direct download links. This is not supported with package manager based apps.

  - !winget:
    id: M2Team.NanaZip # Needs to be the exact WinGet package ID.

  - !choco: # Same syntax as WinGet apps.
    id: brave

  - !wingetBundle:
    fileName: MyAwesomeWinGetBundle.json # Located under `Configuration/Bundles`.

  - !chocoBundle: # Same syntax as WinGet bundle.
    fileName: packages.config # Located under `Configuration/Bundles`.
```

If no valid `Apps.yml` file is found, no apps will be installed.

### :white_check_mark: Supported App Types

Name | Description
---- | -----------
`!choco:` | Chocolatey App
`!winget:` | Winget App
`!chocoBundle:` | File name of a Chocolatey bundle located in the `Bundles` folder inside the `Configuration` folder.
`!wingetBundle:` | File name of a WinGet bundle located in the `Bundles` folder inside the `Configuration` folder.
`!web:` | Executes a file that will be downloaded from the internet (only supports `.exe` files).
`!webAppx:` | Same as web, but only supports files that are supported by the App Installer (e.g. `.appx` or `.msixbundle`).

## :nut_and_bolt: Config.yml

This example configuration shows all the options that are available right now:

```yaml
---
name: My Config # Choose an appropriate name for your configuration. This is optional.
author: nitrowinproject # It is recommended that you use your GitHub username here. This is optional.
options:
  installWinget: ifNeeded # Available options: ifNeeded (default), always, never.
  installChocolatey: ifNeeded # Available options: ifNeeded (default), always, never.
  tweakUrl: "https://github.com/nitrowinproject/Tweaks/archive/refs/heads/main.zip" # Feel free to fork the Tweaks repository and change this if you'd like.
```

If no valid `Config.yml` file is found, the [default configuration](../dist/NitroWin/Configuration/Config.yml) will be used.
