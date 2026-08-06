using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NitroWin.ControlPanel.ViewModels;

public partial class MainWindowViewModel : ObservableObject {
    [ObservableProperty]
    public partial object? CurrentView { get; set; }

    public ICommand TweaksCommand { get; set; }
    public ICommand AppsCommand { get; set; }
    public ICommand ExtrasCommand { get; set; }
    public ICommand UpdatesCommand { get; set; }

    private void Tweaks() => CurrentView = new TweaksViewModel();
    private void Apps() => CurrentView = new AppsViewModel();
    private void Extras() => CurrentView = new ExtrasViewModel();
    private void Updates() => CurrentView = new UpdatesViewModel();

    public MainWindowViewModel() {
        TweaksCommand = new RelayCommand(Tweaks);
        AppsCommand = new RelayCommand(Apps);
        ExtrasCommand = new RelayCommand(Extras);
        UpdatesCommand = new RelayCommand(Updates);

        CurrentView = new TweaksViewModel();
    }
}
