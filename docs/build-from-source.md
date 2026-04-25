# :wrench: Building NitroWin from source

## Requirements

- .NET 10 SDK

## Building

1. Clone the NitroWin repository:

    ```bash
    git clone --recurse-submodules https://github.com/nitrowinproject/NitroWin.git
    ```

2. Start the building process:

    ```bash
    # x64
    dotnet publish -c Release -r win-x64

    # arm64
    dotnet publish -c Release -r win-arm64
    ```

The NitroWin binary will be located under `bin/Release/win-x64/publish/NitroWin.exe` or `bin/Release/win-arm64/publish/NitroWin.exe` depending on which CPU architecture NitroWin was built for.
