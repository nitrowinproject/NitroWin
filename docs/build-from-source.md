# :hammer: Building NitroWin from source

## Requirements

- .NET 10 SDK

## Building

1. Clone the NitroWin repository and navigate into it:

    ```bash
    git clone https://github.com/nitrowinproject/NitroWin.git
    cd NitroWin
    ```

2. Start the building process:

    ```bash
    # x64
    dotnet publish NitroWin/NitroWin.csproj -c Release -r win-x64

    # arm64
    dotnet publish NitroWin/NitroWin.csproj -c Release -r win-arm64
    ```

The NitroWin binary will be located under `bin/Release/win-x64/publish/NitroWin.exe` or `bin/Release/win-arm64/publish/NitroWin.exe` depending on which CPU architecture NitroWin was built for.
