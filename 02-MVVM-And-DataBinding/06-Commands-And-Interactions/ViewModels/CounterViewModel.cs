using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MauiMastery.CommandsAndEvents.Commands;

namespace MauiMastery.CommandsAndEvents.ViewModels;

public class CounterViewModel : INotifyPropertyChanged
{
    private int _count;
    private string _status = "Готовий до роботи";

    public event PropertyChangedEventHandler? PropertyChanged;

    public int Count
    {
        get => _count;
        set { if (_count != value) { _count = value; OnPropertyChanged(); } }
    }

    public string Status
    {
        get => _status;
        set { if (_status != value) { _status = value; OnPropertyChanged(); } }
    }

    public ICommand IncrementCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand HeavyWorkCommand { get; }

    public CounterViewModel()
    {
        IncrementCommand = new RelayCommandSimple(() => Count++);
        ResetCommand = new RelayCommandSimple(() => Count = 0, () => Count > 0);
        HeavyWorkCommand = new AsyncRelayCommand(async () =>
        {
            Status = "Виконується тривала фонова задача...";
            await Task.Delay(100);
            Count += 10;
            Status = "Задачу завершено успішно!";
        });
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public class RelayCommandSimple : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;
    public event EventHandler? CanExecuteChanged;

    public RelayCommandSimple(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;
    public void Execute(object? parameter) => _execute();
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}