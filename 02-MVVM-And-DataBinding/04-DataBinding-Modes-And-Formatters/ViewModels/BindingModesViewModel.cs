using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiMastery.DataBindingModes.ViewModels;

public class BindingModesViewModel : INotifyPropertyChanged
{
    private string _sourceText = "Початковий текст";
    private double _numericValue = 42.5;
    private DateTime _timestamp = DateTime.Now;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string SourceText
    {
        get => _sourceText;
        set { if (_sourceText != value) { _sourceText = value; OnPropertyChanged(); } }
    }

    public double NumericValue
    {
        get => _numericValue;
        set { if (Math.Abs(_numericValue - value) > 0.001) { _numericValue = value; OnPropertyChanged(); } }
    }

    public DateTime Timestamp
    {
        get => _timestamp;
        set { if (_timestamp != value) { _timestamp = value; OnPropertyChanged(); } }
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}