namespace MauiMastery.Showcase.ECommerceMobile.Models;

public record Product(int Id, string Name, string Category, decimal Price, double Rating, string ImageUrl);

public class CartItem
{
    public Product Product { get; }
    public int Quantity { get; set; }

    public CartItem(Product product, int quantity = 1)
    {
        Product = product;
        Quantity = quantity;
    }

    public decimal Subtotal => Product.Price * Quantity;
}