using CommunityToolkit.Mvvm.ComponentModel;

namespace ECAD_Library.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _greeting = "Welcome to Avalonia!";
    }
}
