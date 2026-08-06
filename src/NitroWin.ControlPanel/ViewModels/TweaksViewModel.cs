using CommunityToolkit.Mvvm.ComponentModel;

namespace NitroWin.ControlPanel.ViewModels;

public partial class TweaksViewModel : ObservableObject {
    [ObservableProperty]
    public partial string? Status { get; set; }
}
