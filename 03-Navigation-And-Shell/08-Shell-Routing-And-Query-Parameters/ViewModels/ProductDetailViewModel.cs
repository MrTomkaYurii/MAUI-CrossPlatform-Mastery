using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiMastery.ShellRouting.ViewModels;

public class ProductDetailViewModel : INotifyPropertyChanged
{
    private string _productId = string.Empty;
    private string _productName = "Завантаження...";

    public event PropertyChangedEventHandler? PropertyChanged;

    public string ProductId
    {
        get => _productId;
        set
        {
            if (_productId != value)
            {
                _productId = value;
                OnPropertyChanged();
                LoadProductDetails(value);
            }
        }
    }

    public string ProductName
    {
        get => _productName;
        set { if (_productName != value) { _productName = value; OnPropertyChanged(); } }
    }

    public void LoadProductDetails(string id)
    {
        ProductName = id switch
        {
            "101" => "Смартфон Ultra Pro 15",
            "102" => "Ноутбук Developer Edition",
            _ => "Товар не знайдено"
        };
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}