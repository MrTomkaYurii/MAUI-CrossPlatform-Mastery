using TodoApp.ViewModel;

namespace TodoApp.View;

/// <summary>
/// Code-behind для ArchivePage.xaml — сторінка архіву виконаних справ.
///
/// Відповідає за:
/// - Підключення ArchiveViewModel як BindingContext
/// - Оновлення списку при кожному відкритті сторінки
/// </summary>
public partial class ArchivePage : ContentPage
{
    /// <summary>
    /// Посилання на ViewModel для виклику LoadArchivedTasksAsync при появі.
    /// </summary>
    private readonly ArchiveViewModel _viewModel;

    /// <summary>
    /// Конструктор: отримує ArchiveViewModel через Dependency Injection.
    /// </summary>
    /// <param name="viewModel">ViewModel від DI контейнера</param>
    public ArchivePage(ArchiveViewModel viewModel)
    {
        // Ініціалізуємо XAML
        InitializeComponent();

        _viewModel = viewModel;

        // Підключаємо ViewModel до сторінки
        // Тепер всі {Binding ...} у XAML шукають у ArchiveViewModel
        BindingContext = viewModel;
    }

    /// <summary>
    /// OnAppearing — завантажуємо архівовані справи при кожному відкритті.
    ///
    /// Причина: якщо користувач виконав справу на головній і перейшов до архіву,
    /// нова справа повинна з'явитись у списку.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Завантажуємо архівовані справи з БД
        await _viewModel.LoadArchivedTasksAsync();
    }
}
