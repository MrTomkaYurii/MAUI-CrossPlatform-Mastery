# .NET MAUI Testing & Quality Assurance Guide

Керівництво з тестування кросплатформених застосунків .NET MAUI без потреби у важких емуляторах та нативних SDK.

---

## 1. Стратегія тестування у MAUI

Мобільна розробка часто страждає від повільного зворотного зв'язку, якщо тести вимагають запуску емулятора Android або симулятора iOS. Завдяки правильному архітектурному розподілу 90% логіки тестується як звичайний .NET код:

```mermaid
flowchart TD
    subgraph Fast[Швидкі тести (За мілісекунди без емуляторів)]
        UnitTests[Юніт-тести ViewModels<br/>xUnit + FluentAssertions + Moq]
        DomainTests[Тести бізнес-правил та Use Cases]
        RepoTests[Тести локальних репозиторіїв та мапінгу даних]
        ConverterTests[Тести IValueConverter]
    end

    subgraph Slow[Інтеграційні тести (Запуск на пристроях)]
        UITest[Appium / UI Automator / XCUITest]
        VisualRegression[Скріншотне тестування макетів]
    end
```

---

## 2. Як запускати тести в цьому репозиторії

Усі тести налаштовані в проєкті `tests/MAUI.Mastery.UnitTests` і працюють на стандартному SDK `.NET 10`:

```bash
# Запуск усіх тестів із виведенням звіту
dotnet test tests/MAUI.Mastery.UnitTests

# Запуск із детальним логуванням
dotnet test tests/MAUI.Mastery.UnitTests --logger "console;verbosity=detailed"
```

---

## 3. Зразок тестування ViewModel з мокуванням сервісів

```csharp
[Fact]
public async Task SubmitAsync_WithValidCredentials_ShouldSucceedAndClearError()
{
    // Arrange
    var mockAuth = new Mock<IAuthService>();
    mockAuth.Setup(a => a.AuthenticateAsync("dev@test.com", "Password123!"))
        .ReturnsAsync(true);

    var viewModel = new LoginViewModel(mockAuth.Object)
    {
        Email = "dev@test.com",
        Password = "Password123!"
    };

    // Act
    var result = await viewModel.SubmitAsync();

    // Assert
    result.Should().BeTrue();
    viewModel.ErrorMessage.Should().BeEmpty();
    viewModel.IsLoading.Should().BeFalse();
    mockAuth.Verify(a => a.AuthenticateAsync("dev@test.com", "Password123!"), Times.Once);
}
```
