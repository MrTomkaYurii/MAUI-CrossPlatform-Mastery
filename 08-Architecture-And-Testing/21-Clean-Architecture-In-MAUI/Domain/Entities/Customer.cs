namespace MauiMastery.CleanArchitecture.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Balance { get; set; }

    public bool CanDebit(decimal amount) => amount > 0 && Balance >= amount;

    public void Debit(decimal amount)
    {
        if (!CanDebit(amount)) throw new InvalidOperationException("Недостатньо коштів на рахунку");
        Balance -= amount;
    }
}