# 📱 .NET MAUI Cross-Platform Mastery (.NET 10)

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![MAUI](https://img.shields.io/badge/.NET%20MAUI-Multi--platform-purple?logo=dotnet)](https://learn.microsoft.com/dotnet/maui/)
[![C# 13](https://img.shields.io/badge/C%23-13.0-blue?logo=csharp)](https://learn.microsoft.com/dotnet/csharp/)
[![MVVM](https://img.shields.io/badge/Architecture-Clean%20MVVM-green)](https://learn.microsoft.com/dotnet/architecture/maui/)
[![Tests](https://img.shields.io/badge/Tests-xUnit%20Passing-brightgreen?logo=xunit)](file:///tests/MAUI.Mastery.UnitTests)
[![Platforms](https://img.shields.io/badge/Platforms-Android%20%7C%20iOS%20%7C%20Windows%20%7C%20macOS-informational)](#)

Вичерпний практичний підручник-репозиторій для опанування **.NET MAUI (Multi-platform App UI)** на платформі **.NET 10**. Охоплює повний цикл сучасної розробки: від базових контейнерів розмітки XAML та реактивного Data Binding до чистки архітектури, взаємодії з SQLite, сенсорами пристроїв, гібридними Razor компонентами та юніт-тестуванням.

У репозиторій повністю інтегровано флагманський навчальний стенд **[TodoAppMAUI](file:///09-Showcase-Applications/23-Showcase-TodoApp)** із розширеною практичною документацією.

---

## 🏛️ Загальна архітектура курсу

```mermaid
flowchart TD
    subgraph M1[01-03: Базовий рівень]
        A1[01. Startup & Lifecycle] --> A2[02. Layouts Deep Dive]
        A2 --> A3[03. Common Controls & Styles]
    end

    subgraph M2[04-07: MVVM та реактивний UI]
        B1[04. Data Binding Modes] --> B2[05. Converters & Triggers]
        B2 --> B3[06. Commands & Interactions]
        B3 --> B4[07. CommunityToolkit.Mvvm]
    end

    subgraph M3[08-10: Навігація та діалоги]
        C1[08. Shell Routing] --> C2[09. Navigation Hierarchy]
        C2 --> C3[10. Popups & Dialogs]
    end

    subgraph M4[11-13: Дані та списки]
        D1[11. CollectionView Virtualization] --> D2[12. Infinite Scroll]
        D2 --> D3[13. SQLite Local DB]
    end

    subgraph M5[14-16: Апаратні API пристроїв]
        E1[14. Device Sensors] --> E2[15. Media & SecureStorage]
        E2 --> E3[16. Platform Handlers]
    end

    subgraph M6[17-20: Графіка, теми та гібридний код]
        F1[17. GraphicsView Canvas] --> F2[18. Animations & VSM]
        F2 --> F3[19. Dynamic Theming]
        F3 --> F4[20. Blazor Hybrid]
    end

    subgraph M7[21-24: Архітектура, тести та готові стенди]
        G1[21. Clean Architecture] --> G2[22. ViewModel Unit Testing]
        G2 --> G3[23. Showcase: TodoApp MAUI]
        G2 --> G4[24. Showcase: E-Commerce Mobile]
    end

    M1 --> M2 --> M3 --> M4 --> M5 --> M6 --> M7
```

---

## 📚 Навігаційна карта модулів і проєктів

| Модуль | Проєкт | Опис | Ключові технології |
|---|---|---|---|
| **01. Core Fundamentals** | `01-Maui-Startup-Lifecycle` | Точка входу, `MauiAppBuilder`, реєстрація DI | `MauiProgram`, `IServiceCollection`, `AppShell` |
| | `02-Layouts-Deep-Dive` | Порівняння `Grid`, `Stack`, `FlexLayout`, `AbsoluteLayout` | Адаптивність, сітки, пропорційні розміри |
| | `03-Common-Controls-And-Styles` | Базові контроли (`Entry`, `Slider`, `Editor`), стилі | `ResourceDictionary`, `Style`, `Setter` |
| **02. MVVM & DataBinding** | `04-DataBinding-Modes-And-Formatters` | Режими `OneWay`, `TwoWay`, форматування `StringFormat` | `INotifyPropertyChanged`, `BindingMode` |
| | `05-Converters-And-Triggers` | Кастомні конвертери та реакція UI через тригери | `IValueConverter`, `DataTrigger`, `PropertyTrigger` |
| | `06-Commands-And-Interactions` | Патерн `ICommand`, параметри, асинхронні команди | `RelayCommand`, `AsyncRelayCommand` |
| | `07-Community-Toolkit-MVVM` | Сучасний MVVM зі Source Generators | `[ObservableProperty]`, `[RelayCommand]`, `Messenger` |
| **03. Navigation & Shell** | `08-Shell-Routing-And-Query-Parameters` | URI маршрутизація, параметри запитів, вкладки | `Shell.Current.GoToAsync`, `QueryProperty` |
| | `09-Navigation-Hierarchy-And-Modals` | Ієрархічний стек та модальні сторінки | `PushAsync`, `PopAsync`, `PushModalAsync` |
| | `10-Popups-Alerts-And-ActionSheets` | Системні сповіщення, меню дій, підтвердження | `DisplayAlert`, `DisplayActionSheet` |
| **04. Collections & Data** | `11-CollectionView-And-DataTemplates` | Віртуалізація списків, вибір шаблонів карток | `CollectionView`, `DataTemplateSelector` |
| | `12-Infinite-Scroll-And-RefreshView` | Свайп для оновлення, динамічна пагінація | `RefreshView`, `RemainingItemsThreshold` |
| | `13-SQLite-Local-Database` | Офлайн збереження даних, CRUD операції | `sqlite-net-pcl`, `SQLiteAsyncConnection` |
| **05. Hardware & Platform** | `14-Device-Sensors-And-Connectivity` | Сенсори (GPS, батарея, статус підключення) | `Geolocation`, `Battery`, `Connectivity` |
| | `15-MediaPicker-And-SecureStorage` | Камера, фотогалерея, системний шифрований Keychain | `MediaPicker`, `SecureStorage` |
| | `16-Platform-Handlers-And-Customization` | Архітектура Handlers замість Custom Renderers | `EntryHandler.Mapper`, `PlatformView` |
| **06. Advanced UI** | `17-GraphicsView-And-Canvas-Drawing` | Векторне малювання на полотні, кастомні графіки | `Microsoft.Maui.Graphics`, `IDrawable` |
| | `18-Animations-And-VisualStateManager` | Системні плавні анімації, стани елементів | `TranslateTo`, `FadeTo`, `VisualStateManager` |
| | `19-Dynamic-Theming-Light-Dark` | Підтримка темної та світлої системних тем | `AppThemeBinding`, `AppTheme` |
| **07. Hybrid & Blazor** | `20-Blazor-Hybrid-Components` | Вбудовування Razor компонентів у нативний MAUI | `BlazorWebView`, спільний код C# Web/Mobile |
| **08. Architecture & Tests** | `21-Clean-Architecture-In-MAUI` | Розподіл на шари: Domain, Application, UI | Use Cases, Entities, патерн Repository |
| | `22-ViewModel-Unit-Testing-Patterns` | Ізольоване модульне тестування логіки MVVM | `xUnit`, `FluentAssertions`, `Moq` |
| **09. Showcases** | **`23-Showcase-TodoApp`** | **Освітній стендовий застосунок TodoAppMAUI** | **SQLite, Shell, Converters, MVVM, EXAMPLES.md** |
| | `24-Showcase-ECommerceMobile` | Стендовий мобільний магазин із кошиком товарів | Картки, розрахунок знижок, пошук |
| **Tests Suite** | `tests/MAUI.Mastery.UnitTests` | Централізовані юніт-тести всієї бізнес-логіки | Покриття 100% тестів без емуляторів |

---

## 🛠️ Як перевірити та протестувати

Для перевірки працездатності логіки та архітектури **не потрібно встановлювати важкі мобільні SDK або емулятори**. Всі тести виконуються на чистому `.NET 10`:

```bash
# Клонувати репозиторій
git clone https://github.com/MrTomkaYurii/MAUI-CrossPlatform-Mastery.git
cd MAUI-CrossPlatform-Mastery

# Запустити модульні тести
dotnet test tests/MAUI.Mastery.UnitTests
```

---

## 📖 Технічна документація

- [Гід по архітектурі та внутрішній будові MAUI](file:///docs/ARCHITECTURE.md)
- [Шпаргалка з розмітки XAML та контейнерів](file:///docs/XAML-CHEATSHEET.md)
- [Посібник з тестування мобільних клієнтів](file:///docs/TESTING-GUIDE.md)
- [Освітні приклади та аналогії стенду TodoAppMAUI](file:///09-Showcase-Applications/23-Showcase-TodoApp/EXAMPLES.md)
