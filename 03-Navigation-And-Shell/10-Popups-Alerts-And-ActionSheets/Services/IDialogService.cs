namespace MauiMastery.PopupsAndDialogs.Services;

public interface IDialogService
{
    Task ShowAlertAsync(string title, string message, string cancel);
    Task<bool> ShowConfirmationAsync(string title, string message, string accept, string cancel);
    Task<string> ShowActionSheetAsync(string title, string cancel, string? destruction, params string[] buttons);
}

public class MockDialogService : IDialogService
{
    public bool LastConfirmationResult { get; set; } = true;
    public string LastActionSheetResult { get; set; } = "Option 1";

    public Task ShowAlertAsync(string title, string message, string cancel) => Task.CompletedTask;
    public Task<bool> ShowConfirmationAsync(string title, string message, string accept, string cancel)
        => Task.FromResult(LastConfirmationResult);
    public Task<string> ShowActionSheetAsync(string title, string cancel, string? destruction, params string[] buttons)
        => Task.FromResult(LastActionSheetResult);
}