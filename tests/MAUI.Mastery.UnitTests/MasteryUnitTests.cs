using FluentAssertions;
using MauiMastery.ConvertersAndTriggers.Converters;
using MauiMastery.DataBindingModes.ViewModels;
using MauiMastery.CommunityToolkitMvvm.ViewModels;
using MauiMastery.CleanArchitecture.Domain.Entities;
using MauiMastery.CleanArchitecture.Application.UseCases;
using MauiMastery.Showcase.ECommerceMobile.Models;
using MauiMastery.Showcase.ECommerceMobile.ViewModels;
using MauiMastery.ShellRouting.Routing;
using MauiMastery.ViewModelTesting.ViewModels;
using TodoApp.Model;
using TodoApp.Repository;
using TodoApp.Converters;
using TodoApp.ViewModel;
using Moq;
using Xunit;

namespace MAUI.Mastery.UnitTests;

public class TodoAppUnitTests
{
    [Fact]
    public void TodoItem_DefaultValues_ShouldBeInitializedProperly()
    {
        var item = new TodoItem
        {
            Title = "Купити хліб",
            Description = "Житній або цільнозерновий"
        };

        item.Id.Should().Be(0);
        item.Title.Should().Be("Купити хліб");
        item.Description.Should().Be("Житній або цільнозерновий");
        item.IsCompleted.Should().BeFalse();
        item.CompletedAt.Should().BeNull();
        item.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void StringNotEmptyConverter_ShouldValidateStringsCorrectly()
    {
        var converter = new StringNotEmptyConverter();

        converter.Convert("Деякий текст", typeof(bool), null!, null!).Should().Be(true);
        converter.Convert("", typeof(bool), null!, null!).Should().Be(false);
        converter.Convert("   ", typeof(bool), null!, null!).Should().Be(false);
        converter.Convert(null, typeof(bool), null!, null!).Should().Be(false);
        converter.Convert(12345, typeof(bool), null!, null!).Should().Be(false);
    }

    [Fact]
    public void RelayCommand_ExecuteAndCanExecute_ShouldWorkProperly()
    {
        bool executed = false;
        bool canExecuteCondition = false;

        var command = new RelayCommand(
            execute: () => executed = true,
            canExecute: () => canExecuteCondition
        );

        command.CanExecute(null).Should().BeFalse();
        canExecuteCondition = true;
        command.CanExecute(null).Should().BeTrue();

        command.Execute(null);
        executed.Should().BeTrue();
    }

    [Fact]
    public async Task TodoRepositoryMock_CRUDOperations_ShouldSucceed()
    {
        var mockRepo = new Mock<ITodoRepository>();
        var items = new List<TodoItem>
        {
            new() { Id = 1, Title = "Завдання 1", IsCompleted = false },
            new() { Id = 2, Title = "Завдання 2", IsCompleted = true }
        };

        mockRepo.Setup(r => r.GetActiveTasksAsync())
            .ReturnsAsync(items.Where(i => !i.IsCompleted).ToList());
        mockRepo.Setup(r => r.GetArchivedTasksAsync())
            .ReturnsAsync(items.Where(i => i.IsCompleted).ToList());

        var active = await mockRepo.Object.GetActiveTasksAsync();
        var archived = await mockRepo.Object.GetArchivedTasksAsync();

        active.Should().HaveCount(1);
        active.First().Title.Should().Be("Завдання 1");

        archived.Should().HaveCount(1);
        archived.First().Title.Should().Be("Завдання 2");
    }
}

public class CoreAndMvvmUnitTests
{
    [Fact]
    public void BoolToColorConverter_ShouldReturnAppropriateColors()
    {
        var converter = new BoolToColorConverter();
        converter.Convert(true).Should().Be("#10B981");
        converter.Convert(false).Should().Be("#EF4444");
    }

    [Fact]
    public void InverseBoolConverter_ShouldInvertValue()
    {
        var converter = new InverseBoolConverter();
        converter.Convert(true).Should().BeFalse();
        converter.Convert(false).Should().BeTrue();
    }

    [Fact]
    public void BindingModesViewModel_ShouldNotifyPropertyChanged()
    {
        var vm = new BindingModesViewModel();
        string changedProperty = string.Empty;
        vm.PropertyChanged += (s, e) => changedProperty = e.PropertyName ?? "";

        vm.SourceText = "Новий текст";
        changedProperty.Should().Be(nameof(BindingModesViewModel.SourceText));
        vm.SourceText.Should().Be("Новий текст");
    }

    [Fact]
    public void CommunityToolkitMvvm_ProfileViewModel_ShouldCalculateFullNameAndBoostRating()
    {
        var vm = new ProfileViewModel();
        vm.FullName.Should().Be("Олександр Коваленко");
        vm.Rating.Should().Be(100);

        vm.BoostRatingCommand.Execute(null);
        vm.Rating.Should().Be(125);
    }

    [Fact]
    public void ShellQueryResolver_ShouldParseUriParametersCorrectly()
    {
        var route = "productdetails?productId=101&category=smartphones&promo=true";
        var query = ShellQueryResolver.ParseQueryString(route);

        query.Should().ContainKey("productId").WhoseValue.Should().Be("101");
        query.Should().ContainKey("category").WhoseValue.Should().Be("smartphones");
        query.Should().ContainKey("promo").WhoseValue.Should().Be("true");
    }
}

public class BusinessLogicAndArchitectureUnitTests
{
    [Fact]
    public void CleanArchitecture_CustomerDebit_ShouldWorkCorrectly()
    {
        var customer = new Customer
        {
            Name = "Юрій",
            Email = "yurii@example.com",
            Balance = 500m
        };

        var useCase = new DebitCustomerUseCase();

        // Успішне списання
        var success = useCase.Execute(customer, 200m);
        success.Should().BeTrue();
        customer.Balance.Should().Be(300m);

        // Відхилення через перевищення балансу
        var fail = useCase.Execute(customer, 400m);
        fail.Should().BeFalse();
        customer.Balance.Should().Be(300m);
    }

    [Fact]
    public void ECommerce_CartViewModel_ShouldCalculateTotalsAndDiscountsCorrectly()
    {
        var cart = new CartViewModel();
        var phone = new Product(1, "Смартфон", "Електроніка", 1000m, 4.8, "phone.png");
        var caseCover = new Product(2, "Чохол", "Аксесуари", 50m, 4.5, "case.png");

        cart.AddProduct(phone);
        cart.AddProduct(caseCover);
        cart.AddProduct(caseCover); // 2 чохли

        cart.Items.Should().HaveCount(2);
        cart.Subtotal.Should().Be(1100m); // 1000 + 2*50
        cart.Total.Should().Be(990m);    // 1100 - 10%

        cart.RemoveProduct(2); // видаляємо чохли
        cart.Items.Should().HaveCount(1);
        cart.Subtotal.Should().Be(1000m);
        cart.Total.Should().Be(900m);
    }

    [Fact]
    public async Task LoginViewModel_ValidationAndAuthentication_ShouldWorkProperly()
    {
        var mockAuth = new Mock<IAuthService>();
        mockAuth.Setup(a => a.AuthenticateAsync("user@test.com", "Secret123!"))
            .ReturnsAsync(true);
        mockAuth.Setup(a => a.AuthenticateAsync("user@test.com", "WrongPassword"))
            .ReturnsAsync(false);

        var vm = new LoginViewModel(mockAuth.Object);

        // 1. Порожні поля
        var emptyResult = await vm.SubmitAsync();
        emptyResult.Should().BeFalse();
        vm.ErrorMessage.Should().Contain("Всі поля обов'язкові");

        // 2. Невірний пароль
        vm.Email = "user@test.com";
        vm.Password = "WrongPassword";
        var failResult = await vm.SubmitAsync();
        failResult.Should().BeFalse();
        vm.ErrorMessage.Should().Contain("Невірний логін");

        // 3. Успішний вхід
        vm.Password = "Secret123!";
        var successResult = await vm.SubmitAsync();
        successResult.Should().BeTrue();
        vm.ErrorMessage.Should().BeEmpty();
    }
}
