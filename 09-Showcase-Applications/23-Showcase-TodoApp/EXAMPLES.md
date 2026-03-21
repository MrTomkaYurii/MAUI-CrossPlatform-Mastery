# Навчальні приклади: MAUI + MVVM

> Цей файл пояснює ключові концепції, які використовуються в проєкті.
> Кожен розділ містить просте пояснення, аналогію з реального життя та мінімальний приклад коду.

---

## 1. INotifyPropertyChanged

### Що це?
`INotifyPropertyChanged` — це інтерфейс у C#, який дозволяє об'єкту **сповіщати** інших (зокрема UI) про те, що значення якоїсь властивості змінилось.

### Навіщо це потрібно?
В MAUI, коли ти прив'язуєш (bind) властивість ViewModel до UI-елемента, то UI сам по собі **не знає**, коли значення змінилось. Без цього механізму кнопка залишиться з старим текстом, навіть якщо ViewModel вже має нове значення.

### Аналогія з реального життя
Уяви, що ти замовив піцу. Ресторан — це ViewModel, а ти — UI. Якщо ресторан **не телефонує тобі** (не сповіщає), ти ніколи не дізнаєшся, що піца готова. `INotifyPropertyChanged` — це саме той дзвінок.

### Приклад коду

```csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;

// Клас реалізує INotifyPropertyChanged, щоб UI міг "слухати" зміни
public class ExampleViewModel : INotifyPropertyChanged
{
    // Подія, яку "слухає" XAML-двіжок MAUI
    public event PropertyChangedEventHandler? PropertyChanged;

    // Приватне поле — реальне сховище значення
    private string _name = string.Empty;

    // Публічна властивість — те, до чого прив'язується XAML
    public string Name
    {
        get => _name; // повертаємо значення
        set
        {
            if (_name == value) return; // якщо значення не змінилось — нічого не робимо
            _name = value;              // зберігаємо нове значення
            OnPropertyChanged();        // сповіщаємо UI: "значення змінилось!"
        }
    }

    // Метод, який викликає подію PropertyChanged
    // [CallerMemberName] автоматично підставляє ім'я властивості ("Name"), яка викликала метод
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

**Що відбувається:** коли ти пишеш `viewModel.Name = "Іван"`, метод `OnPropertyChanged()` надсилає сигнал MAUI, і UI автоматично оновлює `Label`, `Entry` або інший елемент.

---

## 2. ICommand

### Що це?
`ICommand` — це інтерфейс, який дозволяє прив'язати **дію** (метод) до UI-елемента (наприклад, кнопки) прямо через XAML, без написання обробників подій у коді сторінки.

### Навіщо це потрібно?
Без `ICommand` тобі довелося б писати обробники подій (`Button.Clicked`) у code-behind файлі сторінки, що порушує принцип розділення логіки та UI в MVVM. `ICommand` дозволяє кнопці "знати" про команду у ViewModel.

### Аналогія з реального життя
`ICommand` — це як **пульт дистанційного керування**. Кнопка на пульті (кнопка в UI) — не виконує дію сама, вона лише надсилає сигнал. Телевізор (ViewModel) отримує сигнал і виконує дію. Пульт не знає, як саме телевізор це робить.

### Приклад коду

```csharp
using System.Windows.Input;

// ICommand має два методи та одну подію
public class SaveCommand : ICommand
{
    // Подія, яка спрацьовує, коли змінюється доступність команди
    public event EventHandler? CanExecuteChanged;

    // Визначає, чи може команда виконатись зараз
    // Якщо повертає false — кнопка автоматично стає неактивною (сіра)
    public bool CanExecute(object? parameter)
    {
        // Дозволяємо виконання завжди (можна додати умову)
        return true;
    }

    // Основна логіка команди — що відбувається при натисканні
    public void Execute(object? parameter)
    {
        Console.WriteLine("Збереження виконано!");
    }
}

// Використання у ViewModel:
public class FormViewModel : INotifyPropertyChanged
{
    // Команда оголошена як ICommand — до неї прив'язується XAML
    public ICommand SaveCommand { get; }

    public FormViewModel()
    {
        // Ініціалізуємо команду в конструкторі
        SaveCommand = new SaveCommand();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
```

**У XAML прив'язка виглядає так:**
```xml
<Button Text="Зберегти" Command="{Binding SaveCommand}" />
```

---

## 3. RelayCommand

### Що це?
`RelayCommand` — це **власна реалізація** `ICommand`, яка дозволяє передати будь-який метод (Action) як команду, не створюючи окремий клас для кожної команди.

### Навіщо це потрібно?
Замість того, щоб писати окремий клас для кожної команди (`SaveCommand`, `DeleteCommand`, `LoadCommand`...), ти просто передаєш метод у `RelayCommand` і все — він сам реалізує `ICommand`. Це значно зменшує кількість коду.

### Аналогія з реального життя
`RelayCommand` — це **посередник-агент**. Ти кажеш агенту: "Виконай оцю функцію". Агент бере будь-яку функцію і виконує її через стандартний інтерфейс. Тобі не потрібно створювати нового агента для кожної функції.

### Приклад коду

```csharp
using System.Windows.Input;

/// <summary>
/// Універсальна реалізація ICommand, яка приймає Action замість окремого класу
/// </summary>
public class RelayCommand : ICommand
{
    // Делегат — зберігає посилання на метод, який треба виконати
    private readonly Action _execute;

    // Опціональний делегат — визначає, чи доступна команда
    private readonly Func<bool>? _canExecute;

    // Подія для повідомлення UI про зміну доступності команди
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Конструктор: приймає метод для виконання та опціональну умову
    /// </summary>
    /// <param name="execute">Метод, який виконується при натисканні</param>
    /// <param name="canExecute">Умова, за якої команда активна (null = завжди активна)</param>
    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        // Перевіряємо, що метод не є null — без нього команда безглузда
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    /// <summary>
    /// Чи може команда виконатись? MAUI автоматично сірить кнопку, якщо false
    /// </summary>
    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

    /// <summary>
    /// Виконати команду — викликає збережений метод
    /// </summary>
    public void Execute(object? parameter) => _execute();

    /// <summary>
    /// Примусово сповіщаємо UI про зміну доступності команди
    /// Виклич цей метод, коли умова CanExecute могла змінитись
    /// </summary>
    public void RaiseCanExecuteChanged() =>
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

/// <summary>
/// Версія RelayCommand з параметром — для команд, яким потрібен аргумент (наприклад, елемент списку)
/// </summary>
public class RelayCommand<T> : ICommand
{
    private readonly Action<T?> _execute;
    private readonly Func<T?, bool>? _canExecute;

    public event EventHandler? CanExecuteChanged;

    public RelayCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    // object? parameter — XAML завжди передає object, ми кастимо до потрібного типу
    public bool CanExecute(object? parameter) => _canExecute?.Invoke((T?)parameter) ?? true;
    public void Execute(object? parameter) => _execute((T?)parameter);
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

// ====== Використання у ViewModel ======
public class TaskViewModel : INotifyPropertyChanged
{
    private string _title = string.Empty;

    public string Title
    {
        get => _title;
        set { _title = value; OnPropertyChanged(); SaveCommand.RaiseCanExecuteChanged(); }
    }

    // Тепер нам не потрібен окремий клас — просто передаємо метод
    public RelayCommand SaveCommand { get; }
    public RelayCommand<string> GreetCommand { get; }

    public TaskViewModel()
    {
        // Команда активна тільки якщо Title не порожній
        SaveCommand = new RelayCommand(Save, () => !string.IsNullOrWhiteSpace(Title));

        // Команда з параметром — рядок передається з XAML через CommandParameter
        GreetCommand = new RelayCommand<string>(name => Console.WriteLine($"Привіт, {name}!"));
    }

    private void Save()
    {
        Console.WriteLine($"Збережено: {Title}");
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
```

---

## 4. ViewModel

### Що це?
`ViewModel` — це клас, який містить **логіку та дані** для однієї сторінки (View). Він є посередником між даними (Model) та інтерфейсом (View).

### Навіщо це потрібно?
Без ViewModel вся логіка опиняється у code-behind файлі сторінки, і її стає важко тестувати та повторно використовувати. ViewModel відокремлює "що відображати" від "як відображати".

### Аналогія з реального життя
Уяви кухаря (Model — дані/бізнес-логіка), офіціанта (ViewModel) та гостя (View — UI). Офіціант бере страву від кухаря, переформатовує інформацію ("стейк mid-rare") для гостя і передає замовлення назад. Гість ніколи не йде на кухню — він спілкується тільки з офіціантом.

### Приклад коду

```csharp
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

/// <summary>
/// ViewModel для сторінки списку товарів.
/// Відповідає за завантаження, відображення та фільтрацію товарів.
/// </summary>
public class ProductListViewModel : INotifyPropertyChanged
{
    // === Поля (приватні — зберігають реальні значення) ===

    private bool _isLoading;
    private string _searchText = string.Empty;

    // === Властивості (публічні — до них прив'язується XAML) ===

    /// <summary>Чи виконується завантаження даних прямо зараз?</summary>
    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    /// <summary>Текст пошуку — вводить користувач</summary>
    public string SearchText
    {
        get => _searchText;
        set { _searchText = value; OnPropertyChanged(); FilterProducts(); }
    }

    /// <summary>Список товарів — ObservableCollection автоматично оновлює CollectionView</summary>
    public ObservableCollection<string> Products { get; } = new();

    // === Команди ===

    /// <summary>Команда завантаження товарів — викликається при натисканні кнопки</summary>
    public ICommand LoadCommand { get; }

    // === Конструктор ===

    public ProductListViewModel()
    {
        // Ініціалізуємо команду — при натисканні викличеться метод LoadProducts
        LoadCommand = new RelayCommand(async () => await LoadProducts());
    }

    // === Приватні методи (логіка, прихована від UI) ===

    private async Task LoadProducts()
    {
        IsLoading = true; // показуємо індикатор завантаження

        // Імітуємо завантаження з мережі
        await Task.Delay(1000);

        Products.Clear();
        Products.Add("Ноутбук");
        Products.Add("Телефон");
        Products.Add("Планшет");

        IsLoading = false; // ховаємо індикатор
    }

    private void FilterProducts()
    {
        // Тут була б логіка фільтрації
    }

    // === INotifyPropertyChanged ===
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
```

---

## 5. Repository Pattern

### Що це?
Repository Pattern — це шаблон проєктування, який **відокремлює** логіку роботи з базою даних від бізнес-логіки. Ти створюєш інтерфейс `IRepository` і клас `Repository`, який реалізує цей інтерфейс.

### Навіщо це потрібно?
- ViewModel не знає, звідки беруться дані (SQLite, API, файл, пам'ять)
- Легко підміняти реалізацію (наприклад, для тестів — InMemoryRepository)
- Вся логіка БД зосереджена в одному місці

### Аналогія з реального життя
Repository — це **бібліотекар**. Ти кажеш "дай мені книгу про котів", а бібліотекар сам знає, де шукати: у залі, у підсобці чи в електронному каталозі. Тебе не цікавить, ЯК він шукає — тебе цікавить результат.

### Приклад коду

```csharp
// === Модель ===
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

// === Інтерфейс репозиторію ===
// Визначає КОНТРАКТ — що можна робити з продуктами
// Реалізацію (SQLite, API тощо) вирішує конкретний клас
public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();           // отримати всі
    Task<Product?> GetByIdAsync(int id);         // знайти один
    Task<int> AddAsync(Product product);         // додати
    Task<int> UpdateAsync(Product product);      // оновити
    Task<int> DeleteAsync(Product product);      // видалити
}

// === Реалізація через SQLite ===
public class SqliteProductRepository : IProductRepository
{
    private readonly SQLiteAsyncConnection _db;

    public SqliteProductRepository(string dbPath)
    {
        // Підключаємося до файлу бази даних
        _db = new SQLiteAsyncConnection(dbPath);
    }

    public async Task<List<Product>> GetAllAsync()
    {
        // Отримуємо всі записи таблиці Product
        return await _db.Table<Product>().ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        // Шукаємо один запис за первинним ключем
        return await _db.FindAsync<Product>(id);
    }

    public async Task<int> AddAsync(Product product)
    {
        // Вставляємо новий запис, повертаємо кількість змінених рядків
        return await _db.InsertAsync(product);
    }

    public async Task<int> UpdateAsync(Product product)
    {
        return await _db.UpdateAsync(product);
    }

    public async Task<int> DeleteAsync(Product product)
    {
        return await _db.DeleteAsync(product);
    }
}

// === Використання у ViewModel ===
public class ProductViewModel
{
    // ViewModel знає тільки про інтерфейс, не про SQLite
    private readonly IProductRepository _repository;

    public ProductViewModel(IProductRepository repository)
    {
        // Dependency Injection — репозиторій передається ззовні
        _repository = repository;
    }

    public async Task LoadAsync()
    {
        var products = await _repository.GetAllAsync();
        // ... оновлюємо колекцію
    }
}
```

---

## 6. Binding у XAML

### Що це?
Binding (прив'язка) — це механізм у XAML, який **синхронізує** значення властивості UI-елемента з властивістю ViewModel. Коли ViewModel змінюється — UI оновлюється автоматично, і навпаки.

### Навіщо це потрібно?
Без Binding тобі довелося б вручну в коді знаходити елемент і встановлювати його значення: `myLabel.Text = viewModel.Name`. З Binding це відбувається автоматично.

### Аналогія з реального життя
Binding — це **дзеркало**. Що б ти не зробив зі своєю зовнішністю (ViewModel), дзеркало (UI) одразу показує актуальний стан. І якщо б дзеркало могло впливати на тебе (двостороннє binding), то зміна у дзеркалі змінювала б і тебе.

### Приклад коду

**ViewModel (C#):**
```csharp
public class ProfileViewModel : INotifyPropertyChanged
{
    private string _firstName = "Іван";
    private int _age = 25;
    private bool _isAdmin = false;

    // Одностороннє прив'язування: ViewModel → UI
    public string FirstName
    {
        get => _firstName;
        set { _firstName = value; OnPropertyChanged(); }
    }

    // Двостороннє прив'язування: ViewModel ↔ UI
    public int Age
    {
        get => _age;
        set { _age = value; OnPropertyChanged(); }
    }

    public bool IsAdmin
    {
        get => _isAdmin;
        set { _isAdmin = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
```

**XAML — різні режими Binding:**
```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:MyApp.ViewModel"
             x:Class="MyApp.View.ProfilePage">

    <!-- Прив'язуємо ViewModel до сторінки в code-behind: BindingContext = new ProfileViewModel() -->

    <StackLayout Padding="20">

        <!-- OneWay: ViewModel → UI (тільки читання, за замовчуванням для Label) -->
        <Label Text="{Binding FirstName, Mode=OneWay}"
               FontSize="24"/>

        <!-- TwoWay: UI ↔ ViewModel (зміна в Entry оновлює ViewModel і навпаки) -->
        <Entry Text="{Binding FirstName, Mode=TwoWay}"
               Placeholder="Введіть ім'я"/>

        <!-- Binding з конвертером та форматуванням -->
        <Label Text="{Binding Age, StringFormat='Вік: {0} років'}"/>

        <!-- Прив'язка bool до видимості елемента -->
        <Label Text="Адміністратор" IsVisible="{Binding IsAdmin}"/>

        <!-- OneWayToSource: тільки UI → ViewModel (рідко використовується) -->
        <CheckBox IsChecked="{Binding IsAdmin, Mode=TwoWay}"/>

    </StackLayout>
</ContentPage>
```

---

## 7. Command у XAML

### Що це?
Command у XAML — це спосіб прив'язати **дію** (метод у ViewModel) до кнопки або іншого елемента інтерфейсу через властивість `Command`.

### Навіщо це потрібно?
Замість того, щоб писати `Button.Clicked += OnButtonClicked` у code-behind (що порушує MVVM), ти оголошуєш команду у ViewModel і прив'язуєш її прямо у XAML. UI нічого не знає про логіку — він просто каже "натиснуто!" і команда сама все робить.

### Аналогія з реального життя
Уяви кнопку виклику офіціанта в ресторані. Кнопка не знає, хто прийде у відповідь. Вона просто надсилає сигнал. Офіціант (ViewModel) отримує сигнал і виконує дію. Command у XAML — це та сама кнопка виклику.

### Приклад коду

**ViewModel:**
```csharp
public class LoginViewModel : INotifyPropertyChanged
{
    private string _email = string.Empty;
    private string _password = string.Empty;
    private bool _isLoading = false;

    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(); LoginCommand.RaiseCanExecuteChanged(); }
    }

    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); LoginCommand.RaiseCanExecuteChanged(); }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    // Команда входу — активна тільки якщо поля заповнені
    public RelayCommand LoginCommand { get; }

    // Команда без умови — завжди активна
    public RelayCommand NavigateToRegisterCommand { get; }

    // Команда з параметром — отримує дані з CommandParameter у XAML
    public RelayCommand<string> SelectRoleCommand { get; }

    public LoginViewModel()
    {
        // CanExecute перевіряє, що поля не порожні
        LoginCommand = new RelayCommand(
            execute: async () => await LoginAsync(),
            canExecute: () => !string.IsNullOrWhiteSpace(Email) &&
                              !string.IsNullOrWhiteSpace(Password) &&
                              !IsLoading
        );

        NavigateToRegisterCommand = new RelayCommand(
            () => Shell.Current.GoToAsync("register")
        );

        SelectRoleCommand = new RelayCommand<string>(
            role => Console.WriteLine($"Обрана роль: {role}")
        );
    }

    private async Task LoginAsync()
    {
        IsLoading = true;
        LoginCommand.RaiseCanExecuteChanged(); // оновлюємо стан кнопки
        await Task.Delay(2000); // імітація запиту до сервера
        IsLoading = false;
        LoginCommand.RaiseCanExecuteChanged();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
```

**XAML — повний приклад підключення кнопок до команд:**
```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyApp.View.LoginPage"
             Title="Вхід">

    <Grid RowDefinitions="*, Auto" Padding="24">

        <StackLayout Grid.Row="0" VerticalOptions="Center" Spacing="16">

            <!-- Entry прив'язаний до Email: зміна в полі оновлює ViewModel -->
            <Entry Text="{Binding Email, Mode=TwoWay}"
                   Placeholder="Email"
                   Keyboard="Email"/>

            <Entry Text="{Binding Password, Mode=TwoWay}"
                   Placeholder="Пароль"
                   IsPassword="True"/>

            <!--
                Command="{Binding LoginCommand}" — прив'язуємо команду
                IsEnabled автоматично керується CanExecute() у LoginCommand
            -->
            <Button Text="Увійти"
                    Command="{Binding LoginCommand}"
                    BackgroundColor="#6750A4"
                    TextColor="White"/>

            <!-- ActivityIndicator видимий тільки під час завантаження -->
            <ActivityIndicator IsRunning="{Binding IsLoading}"
                               IsVisible="{Binding IsLoading}"
                               Color="#6750A4"/>

            <!--
                CommandParameter передає рядок у SelectRoleCommand
                RelativeSource AncestorType — шукаємо ViewModel у батьківському елементі
            -->
            <Button Text="Адміністратор"
                    Command="{Binding SelectRoleCommand}"
                    CommandParameter="admin"/>

            <Button Text="Користувач"
                    Command="{Binding SelectRoleCommand}"
                    CommandParameter="user"/>

        </StackLayout>

        <!-- Кнопка внизу сторінки — навігація без параметрів -->
        <Button Grid.Row="1"
                Text="Немає акаунту? Зареєструватись"
                Command="{Binding NavigateToRegisterCommand}"
                BackgroundColor="Transparent"
                TextColor="#6750A4"/>

    </Grid>

</ContentPage>
```

---

*Ці концепції є фундаментом MVVM у .NET MAUI. Після вивчення цього файлу переходь до коду проєкту — там ти побачиш усі ці концепції в дії разом.*
