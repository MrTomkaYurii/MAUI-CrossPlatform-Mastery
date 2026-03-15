using MauiMastery.CleanArchitecture.Domain.Entities;

namespace MauiMastery.CleanArchitecture.Application.UseCases;

public class DebitCustomerUseCase
{
    public bool Execute(Customer customer, decimal amount)
    {
        if (!customer.CanDebit(amount)) return false;
        customer.Debit(amount);
        return true;
    }
}