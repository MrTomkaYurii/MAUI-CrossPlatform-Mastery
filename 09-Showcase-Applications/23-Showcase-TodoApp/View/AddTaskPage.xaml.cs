using TodoApp.ViewModel;

namespace TodoApp.View;

/// <summary>
/// Code-behind для AddTaskPage.xaml — сторінка додавання нової справи.
///
/// Відповідає за:
/// - Підключення AddTaskViewModel як BindingContext
/// - Скидання форми при кожному відкритті сторінки
/// - Фокус на полі "Назва" для зручності (автоматично відкривається клавіатура)
/// </summary>
public partial class AddTaskPage : ContentPage
{
    /// <summary>
    /// Посилання на ViewModel для виклику ResetForm при появі сторінки.
    /// </summary>
    private readonly AddTaskViewModel _viewModel;

    /// <summary>
    /// Конструктор: отримує AddTaskViewModel через Dependency Injection.
    /// </summary>
    /// <param name="viewModel">ViewModel від DI контейнера</param>
    public AddTaskPage(AddTaskViewModel viewModel)
    {
        // Ініціалізуємо XAML
        InitializeComponent();

        _viewModel = viewModel;

        // Підключаємо ViewModel — тепер {Binding ...} у XAML працює з AddTaskViewModel
        BindingContext = viewModel;
    }

    /// <summary>
    /// OnAppearing — викликається кожен раз при відкритті сторінки.
    ///
    /// Тут ми скидаємо форму, щоб при повторному відкритті
    /// поля були порожніми (а не з минулими даними).
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Скидаємо Title і Description у ViewModel (і автоматично у XAML через Binding)
        _viewModel.ResetForm();
    }
}
