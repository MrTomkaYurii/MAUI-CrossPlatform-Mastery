# 07. Modern MVVM with CommunityToolkit.Mvvm (Сучасний MVVM зі Source Generators)

### Що це?
Бібліотека `CommunityToolkit.Mvvm` використовує C# Source Generators, щоб усунути до 80% рутинного коду (boilerplate) при роботі з MVVM.

### Головні атрибути:
1. `[ObservableProperty]` — генерує публічну властивість з викликом `OnPropertyChanged`, підтримкою валідації та залежних властивостей `[NotifyPropertyChangedFor]`.
2. `[RelayCommand]` — генерує команди `ICommand` або `IAsyncRelayCommand` з безпечним управлінням паралельними викликами.
3. `WeakReferenceMessenger` — забезпечує слабкозв'язаний обмін подіями між різними ViewModel без ризику витоків пам'яті.