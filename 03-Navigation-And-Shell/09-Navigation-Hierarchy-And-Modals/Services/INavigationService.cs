namespace MauiMastery.NavigationHierarchy.Services;

public interface INavigationService
{
    IReadOnlyList<string> NavigationStack { get; }
    IReadOnlyList<string> ModalStack { get; }
    void Push(string pageName);
    string? Pop();
    void PushModal(string modalName);
    string? PopModal();
}

public class MockNavigationService : INavigationService
{
    private readonly Stack<string> _navStack = new();
    private readonly Stack<string> _modalStack = new();

    public IReadOnlyList<string> NavigationStack => _navStack.ToList();
    public IReadOnlyList<string> ModalStack => _modalStack.ToList();

    public void Push(string pageName) => _navStack.Push(pageName);
    public string? Pop() => _navStack.Count > 0 ? _navStack.Pop() : null;

    public void PushModal(string modalName) => _modalStack.Push(modalName);
    public string? PopModal() => _modalStack.Count > 0 ? _modalStack.Pop() : null;
}