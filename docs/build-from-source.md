# :hammer_and_wrench: Building NitroWin from Source

## :exclamation: Requirements

- .NET 10 SDK
- Inno Setup 7 (or later)

## :hammer: Building

1. Clone the NitroWin repository and navigate into it:

    ```bash
    git clone https://github.com/nitrowinproject/NitroWin.git
    cd NitroWin
    ```

2. Start the building process:

    ```bash
    # x64
    dotnet publish src/NitroWin/NitroWin.csproj -c Release -r win-x64 -o publish/NitroWin-win-x64

    # arm64
    dotnet publish src/NitroWin/NitroWin.csproj -c Release -r win-arm64 -o publish/NitroWin-win-arm64
    ```

The NitroWin binary will be located under `publish/NitroWin-win-x64/NitroWin.exe` or `publish/NitroWin-win-arm64/NitroWin.exe`.

### :cd: Compiling the Installer

Run the following command inside the project root:

```bash
# x64
iscc /DTargetArch=x64 ./deployment/NitroWin.iss

# arm64
iscc /DTargetArch=arm64 ./deployment/NitroWin.iss
```

The installer will be located under `deployment/Output/NitroWinSetup-x64.exe` or `deployment/Output/NitroWinSetup-arm64.exe`.
