# TodoApp

Мобільний та десктопний застосунок для управління завданнями, побудований на **.NET MAUI** (.NET 10).

## Про проект

TodoApp — це простий менеджер справ з підтримкою Android та Windows. Дозволяє додавати, виконувати та архівувати завдання, зберігаючи їх локально у SQLite-базі даних.

## Функціональність

- Перегляд активних справ на головній сторінці
- Додавання нових справ із назвою та описом
- Позначення справи як виконаної (свайп вправо або чекбокс)
- Видалення справи (свайп вліво)
- Перегляд виконаних справ у архіві
- Pull-to-refresh для оновлення списку
- Лічильник активних справ у заголовку

## Стек технологій

| Компонент | Технологія |
|---|---|
| UI-фреймворк | .NET MAUI 10 |
| Мова | C# 13, XAML |
| База даних | SQLite (sqlite-net-pcl) |
| Архітектура | MVVM |
| DI | Microsoft.Extensions.DependencyInjection |
| Платформи | Android 5.0+, Windows 10+ |

## Архітектура

Проект дотримується патерну **MVVM** та розділений на шари:

```
TodoApp/
├── Model/              # Модель даних (TodoItem)
├── Repository/         # Шар доступу до даних (ITodoRepository, TodoRepository)
├── ViewModel/          # Логіка UI (MainViewModel, AddTaskViewModel, ArchiveViewModel)
├── View/               # XAML-сторінки (MainPage, AddTaskPage, ArchivePage)
├── Converters/         # Value Converters для XAML
└── Platforms/          # Платформо-специфічний код (Android, Windows)
```

**ViewModel** взаємодіє виключно з інтерфейсом `ITodoRepository`, не знаючи про конкретну реалізацію SQLite — це дозволяє легко замінити сховище даних.

## Запуск

### Вимоги
- Visual Studio 2022 / 2025 з воркнавантаженням **.NET MAUI**
- .NET 10 SDK

### Кроки
1. Клонуйте репозиторій
2. Відкрийте `TodoApp.sln` у Visual Studio
3. Оберіть цільову платформу (Android або Windows)
4. Запустіть (`F5`)
