using TodoApp.Model;
using TodoApp.ViewModel;

namespace TodoApp.View;

/// <summary>
/// Code-behind для MainPage.xaml — головна сторінка зі списком активних справ.
///
/// Code-behind (файл .xaml.cs) відповідає за:
/// - Отримання ViewModel через DI і встановлення BindingContext
/// - Обробку подій, які не можна прив'язати до команд (наприклад, CheckBox.CheckedChanged)
/// - Виклик методів ViewModel при подіях навігації (OnAppearing)
///
/// Правило MVVM: мінімум логіки в code-behind, вся бізнес-логіка — у ViewModel.
/// </summary>
public partial class MainPage : ContentPage
{
    /// <summary>
    /// Посилання на ViewModel для зручного доступу в обробниках подій.
    /// Кастимо BindingContext до MainViewModel, щоб не кастувати кожен раз в методах.
    /// </summary>
    private readonly MainViewModel _viewModel;

    /// <summary>
    /// Конструктор сторінки.
    ///
    /// MainViewModel передається через Dependency Injection.
    /// MAUI DI контейнер автоматично знаходить правильний ViewModel.
    ///
    /// Порядок ініціалізації:
    /// 1. InitializeComponent() — завантажує XAML і створює UI-елементи
    /// 2. _viewModel = viewModel — зберігаємо посилання
    /// 3. BindingContext = viewModel — підключаємо ViewModel до сторінки (Binding працює!)
    /// </summary>
    /// <param name="viewModel">ViewModel від DI контейнера</param>
    public MainPage(MainViewModel viewModel)
    {
        // Ініціалізуємо XAML-частину сторінки (створює всі елементи з XAML)
        InitializeComponent();

        // Зберігаємо посилання для використання в обробниках подій
        _viewModel = viewModel;

        // BindingContext — ключова властивість!
        // Після цього рядка всі {Binding ...} у XAML шукають властивості у MainViewModel
        BindingContext = viewModel;
    }

    /// <summary>
    /// OnAppearing — викликається кожен раз, коли сторінка стає видимою.
    ///
    /// Чому завантажуємо тут, а не в конструкторі?
    /// Конструктор викликається один раз. OnAppearing — кожен раз при поверненні на сторінку.
    /// Тому тут ми оновлюємо список справ після повернення зі сторінки "Додати".
    ///
    /// Наприклад: користувач додав справу → повернувся → список оновився.
    /// </summary>
    protected override async void OnAppearing()
    {
        // Обов'язково викликаємо базовий метод (MAUI-логіка)
        base.OnAppearing();

        // Завантажуємо свіжий список справ з БД
        await _viewModel.LoadTasksAsync();
    }

    /// <summary>
    /// Обробник події CheckBox.CheckedChanged.
    ///
    /// Чому не через Command напряму в XAML?
    /// CheckBox не підтримує Command безпосередньо для CheckedChanged.
    /// Тому обробляємо подію тут і викликаємо команду ViewModel вручну.
    ///
    /// Це допустиме відступлення від чистого MVVM заради зручності.
    /// Вся логіка (що робити з справою) все одно знаходиться у ViewModel.
    /// </summary>
    /// <param name="sender">CheckBox, який змінився</param>
    /// <param name="e">Подія зі значенням IsChecked (true/false)</param>
    private void OnTaskCheckChanged(object sender, CheckedChangedEventArgs e)
    {
        // e.Value = true: користувач поставив галочку (виконати справу)
        // e.Value = false: зняв галочку (нас не цікавить — справа залишається активною)
        if (!e.Value) return;

        // Отримуємо TodoItem з BindingContext CheckBox (кожен елемент має свій контекст)
        if (sender is CheckBox checkBox && checkBox.BindingContext is TodoItem item)
        {
            // Викликаємо команду ViewModel — там уся логіка виконання справи
            // Execute приймає object? — передаємо TodoItem
            _viewModel.CompleteTaskCommand.Execute(item);
        }
    }
}
