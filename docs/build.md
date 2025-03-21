# :hammer: · Building NitroWin

Currently, NitroWin supports Windows and Linux.

## :window: · Windows build instructions

1. Install the [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

2. Clone this repository and navigate into it:

    ```cmd
    git clone https://github.com/Nitro4542/NitroWin.git
    cd NitroWin
    ```

3. Compile:

    ```cmd
    dotnet publish -c Release -r win-x64
    ```

    or if you have an arm-based cpu:

    ```cmd
    dotnet publish -c Release -r win-arm64
    ```

The compiled executable can be found in:
`bin/Release/x64/publish/NitroWin.exe`

## :penguin: · Linux build instructions

1. Install the .NET 8 SDK. Here is the install command for Arch-based distributions:

    ```bash
    yay -Sy dotnet-sdk-8.0-bin
    ```

2. Clone this repository and navigate into it:

    ```bash
    git clone https://github.com/Nitro4542/NitroWin.git
    cd NitroWin
    ```

3. Compile:

    ```bash
    dotnet publish -c Release -r linux-x64
    ```

    or if you have an arm-based cpu:

    ```cmd
    dotnet publish -c Release -r linux-arm64
    ```

The compiled executable can be found in:
`bin/Release/x64/publish/NitroWin`
