using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MauiMastery.Showcase.ECommerceMobile.Models;

namespace MauiMastery.Showcase.ECommerceMobile.ViewModels;

public class CartViewModel : INotifyPropertyChanged
{
    public ObservableCollection<CartItem> Items { get; } = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    public decimal Subtotal => Items.Sum(i => i.Subtotal);
    public decimal DiscountPercent { get; set; } = 0.10m; // 10% знижка
    public decimal Total => Subtotal * (1 - DiscountPercent);

    public void AddProduct(Product product)
    {
        var existing = Items.FirstOrDefault(i => i.Product.Id == product.Id);
        if (existing != null)
        {
            existing.Quantity++;
        }
        else
        {
            Items.Add(new CartItem(product, 1));
        }
        OnPropertyChanged(nameof(Subtotal));
        OnPropertyChanged(nameof(Total));
    }

    public void RemoveProduct(int productId)
    {
        var item = Items.FirstOrDefault(i => i.Product.Id == productId);
        if (item != null)
        {
            Items.Remove(item);
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(Total));
        }
    }

    public void Clear()
    {
        Items.Clear();
        OnPropertyChanged(nameof(Subtotal));
        OnPropertyChanged(nameof(Total));
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}