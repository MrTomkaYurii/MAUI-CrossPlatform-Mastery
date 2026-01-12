namespace MauiMastery.StartupLifecycle.Services;

public interface IGreetingService
{
    string GetGreeting(string name);
}

public class GreetingService : IGreetingService
{
    public string GetGreeting(string name) => $"Вітаємо у .NET MAUI 10, {name}! Застосунок успішно стартував.";
}