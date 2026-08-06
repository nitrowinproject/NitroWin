using System.Net.Http.Headers;
using System.Reflection;
using System.Resources;
using Microsoft.Extensions.DependencyInjection;
using NitroWin.Core.Factories;
using NitroWin.Core.Models.Apps;
using NitroWin.Core.Models.Tweaks.Actions;
using NitroWin.Core.Services;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace NitroWin.Core.Extensions;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddNitroWin(this IServiceCollection services) {
        services.AddLogging();
        services.AddSingleton<LogService>();

        services.AddSingleton(_ => new ResourceManager(
            "NitroWin.Resources.Strings", Assembly.GetExecutingAssembly()));

        services.AddSingleton(_ => new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithObjectFactory(new ServiceProviderObjectFactory(services.BuildServiceProvider()))
            .WithTagMapping("!choco:", typeof(ChocolateyApp))
            .WithTagMapping("!chocoBundle", typeof(ChocolateyBundleApp))
            .WithTagMapping("!web:", typeof(WebApp))
            .WithTagMapping("!webAppx:", typeof(AppxWebApp))
            .WithTagMapping("!winget:", typeof(WingetApp))
            .WithTagMapping("!wingetBundle", typeof(WingetBundleApp))
            .WithTagMapping("!cmd:", typeof(CmdAction))
            .WithTagMapping("!powerShell:", typeof(PowerShellAction))
            .WithTagMapping("!registryValue:", typeof(RegistryValueAction))
            .WithTagMapping("!run:", typeof(RunAction))
            .WithTagMapping("!scheduledTask:", typeof(ScheduledTaskAction))
            .WithTagMapping("!service:", typeof(ServiceAction))
            .Build());

        services.AddSingleton<ConfigService>();

        services.AddHttpClient("Default", client => {
            client.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(
                    "NitroWin",
                    Assembly.GetExecutingAssembly().GetName().Version?.ToString()
                )
            );
        });
        services.AddSingleton<DownloaderService>();

        services.AddSingleton<ExtractionService>();

        services.AddSingleton<TweakService>();

        services.AddSingleton<ChocolateyService>();
        services.AddHostedService(sp => sp.GetRequiredService<ChocolateyService>());

        services.AddSingleton<WingetService>();
        services.AddHostedService(sp => sp.GetRequiredService<WingetService>());

        services.AddSingleton<NitroWinService>();

        return services;
    }
}
