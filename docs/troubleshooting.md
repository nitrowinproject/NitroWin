# :wrench: · Troubleshooting

Here are some common problems that I encountered:

## :interrobang: · Some tweaks fail to import

Some tweaks (in the System folder only) require you to import them with TrustedInstaller privileges. You can do this with [ExecTI](https://winaero.com/download-execti-run-as-trustedinstaller/).

However, this shouldn't be a problem if you're using the answer file, as there, they are run in a system context there.

## :x: · The app doesn't run

Install the [.NET 8.0 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) if you haven't already. You only need the desktop or the normal runtime, not both.
