using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
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

services.AddLocalization(config => {
    config.ResourcesPath = "Resources";
});

services.AddNitroWin();

var registrar = new TypeRegistrar(services);
var app = new CommandApp(registrar);

await using var serviceProvider = services.BuildServiceProvider();
var localizerFactory = serviceProvider.GetRequiredService<IStringLocalizerFactory>();
var localizer = localizerFactory.Create("Strings", "NitroWin.Cli");

app.Configure(config => {
    config.SetApplicationName("nitrowin");
    config.SetApplicationVersion("3.2.0");

    config.AddCommand<ApplyCommand>("apply")
        .WithDescription(localizer["ApplyCommandDescription"]);
    config.AddCommand<ApplyCommand>("apps")
        .WithDescription(localizer["AppsCommandDescription"]);
    config.AddCommand<UpdateCommand>("update")
        .WithDescription(localizer["UpdateCommandDescription"]);
});

return await app.RunAsync(args);
