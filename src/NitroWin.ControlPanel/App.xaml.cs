using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NitroWin.ControlPanel.ViewModels;
using NitroWin.ControlPanel.Views.Windows;

namespace NitroWin.ControlPanel;

public partial class App : Application {
    public static IHost? AppHost { get; private set; }

    public App() {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) => {
                services.AddSingleton<MainWindow>();
                services.AddTransient<MainWindowViewModel>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e) {
        await AppHost!.StartAsync();

        var window = AppHost.Services.GetRequiredService<MainWindow>();
        window.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e) {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}
