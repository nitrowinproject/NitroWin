using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NitroWin.Cli.Helpers;
using NitroWin.Cli.Models;
using NitroWin.Core.Extensions;
using Spectre.Console.Cli;

var services = new ServiceCollection();
services.AddLogging(builder => {
    builder.AddConsole();
#if DEBUG
    builder.SetMinimumLevel(LogLevel.Debug);
#else
    builder.SetMinimumLevel(LogLevel.Error);
#endif
});
services.AddNitroWin();

var registrar = new TypeRegistrar(services);
var app = new CommandApp(registrar);

app.Configure(config => {
    config.SetApplicationName("nitrowin");
    config.SetApplicationVersion("3.2.0");

    config.AddCommand<ApplyCommand>("apply")
        .WithDescription("Applies all locally cached tweaks");
    config.AddCommand<UpdateCommand>("update")
        .WithDescription("Updates locally cached tweaks");
});

return await app.RunAsync(args);
