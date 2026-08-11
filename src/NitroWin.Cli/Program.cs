using Microsoft.Extensions.DependencyInjection;
using NitroWin.Cli.Helpers;
using NitroWin.Cli.Models;
using NitroWin.Core.Extensions;
using Spectre.Console.Cli;

var services = new ServiceCollection();
services.AddNitroWin();

var registrar = new TypeRegistrar(services);

var app = new CommandApp(registrar);

app.Configure(config => {
    config.AddCommand<ApplyCommand>("apply");
    config.AddCommand<UpdateCommand>("update");
});

return await app.RunAsync(args);
