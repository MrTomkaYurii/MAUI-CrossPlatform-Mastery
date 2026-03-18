using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiMastery.ViewModelTesting.ViewModels;

public interface IAuthService
{
    Task<bool> AuthenticateAsync(string email, string password);
}

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly IAuthService _authService;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _isLoading;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Email
    {
        get => _email;
        set { if (_email != value) { _email = value; OnPropertyChanged(); } }
    }

    public string Password
    {
        get => _password;
        set { if (_password != value) { _password = value; OnPropertyChanged(); } }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set { if (_errorMessage != value) { _errorMessage = value; OnPropertyChanged(); } }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set { if (_isLoading != value) { _isLoading = value; OnPropertyChanged(); } }
    }

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<bool> SubmitAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Всі поля обов'язкові до заповнення";
            return false;
        }

        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            var success = await _authService.AuthenticateAsync(Email, Password);
            if (!success)
            {
                ErrorMessage = "Невірний логін або пароль";
            }
            return success;
        }
        finally
        {
            IsLoading = false;
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}