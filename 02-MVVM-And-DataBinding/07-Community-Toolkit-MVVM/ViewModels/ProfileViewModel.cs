using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace MauiMastery.CommunityToolkitMvvm.ViewModels;

// Повідомлення для обміну між ViewModel
public record UserLoggedInMessage(string Username);

public partial class ProfileViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullName))]
    private string _firstName = "Олександр";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullName))]
    private string _lastName = "Коваленко";

    [ObservableProperty]
    private int _rating = 100;

    public string FullName => $"{FirstName} {LastName}";

    [RelayCommand]
    private void BoostRating()
    {
        Rating += 25;
        // Відправка слабкого повідомлення через Messenger
        WeakReferenceMessenger.Default.Send(new UserLoggedInMessage(FullName));
    }

    [RelayCommand]
    private async Task ResetRatingAsync()
    {
        await Task.Delay(50);
        Rating = 0;
    }
}