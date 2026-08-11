# :hammer_and_wrench: Building NitroWin from Source

## :exclamation: Requirements

- .NET 10 SDK

## :hammer: Building

1. Clone the NitroWin repository and navigate into it:

    ```bash
    git clone https://github.com/nitrowinproject/NitroWin.git
    cd NitroWin
    ```

2. Start the building process:

    ```bash
    # x64
    dotnet publish -c Release -r win-x64 -o publish/

    # arm64
    dotnet publish -c Release -r win-arm64 -o publish/
    ```

The NitroWin binary will be located under `publish/NitroWin.exe`.
