# :hammer: · Building NitroWin

Currently, NitroWin supports building for or on Windows and Linux.

## :window: · Windows build instructions

1. Install the [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

2. Clone this repository and navigate into it:

    ```cmd
    git clone https://github.com/nitrowinproject/NitroWin.git
    cd NitroWin
    ```

3. Compile:

    ```cmd
    dotnet publish -c Release -r win-x64
    ```

    or if you have an arm-based CPU:

    ```cmd
    dotnet publish -c Release -r win-arm64
    ```

The compiled executables can be found in:  
`bin/Release/win-x64/publish/`

...or arm builds:  
`bin/Release/win-arm64/publish/`

## :penguin: · Linux build instructions

> [!TIP]
> It is not recommended, nor does it make sense, to compile NitroWin for Linux since it is a Windows modification. However, you can compile it for Windows on Linux.

1. Install the .NET 8 SDK. Here is the install command for Arch-based distributions:

    ```bash
    yay -Sy dotnet-sdk-8.0-bin
    ```

2. Clone this repository and navigate into it:

    ```bash
    git clone https://github.com/nitrowinproject/NitroWin.git
    cd NitroWin
    ```

3. Compile:

    ```bash
    dotnet publish -c Release -r linux-x64
    ```

    or if you have an arm-based CPU:

    ```cmd
    dotnet publish -c Release -r linux-arm64
    ```

The compiled executables can be found in:  
`bin/Release/linux-x64/publish/`

...or arm builds:  
`bin/Release/linux-arm64/publish/`
