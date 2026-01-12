using System.ComponentModel;
using System.Runtime.CompilerServices;
using MauiMastery.StartupLifecycle.Services;

namespace MauiMastery.StartupLifecycle.ViewModels;

public class StartupViewModel : INotifyPropertyChanged
{
    private readonly IGreetingService _greetingService;
    private string _status = "Ініціалізація...";

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Status
    {
        get => _status;
        set
        {
            if (_status != value)
            {
                _status = value;
                OnPropertyChanged();
            }
        }
    }

    public StartupViewModel(IGreetingService greetingService)
    {
        _greetingService = greetingService;
        Status = _greetingService.GetGreeting("Developer");
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}