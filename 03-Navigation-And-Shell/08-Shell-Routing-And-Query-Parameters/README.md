# 08. Shell Routing & Query Parameters (Маршрутизація в AppShell)

### Що це?
`AppShell` надає потужну URI-базовану навігацію для мобільних та настільних платформ:
```csharp
// Навігація з параметрами
await Shell.Current.GoToAsync($"productdetails?productId=101&category=gadgets");

// Навігація назад
await Shell.Current.GoToAsync("..");
```

### Переваги Shell:
1. Декларативна структура вкладок (`TabBar`), бічного меню (`FlyoutItem`) та пошуку.
2. Глибокі посилання (Deep Linking) безпосередньо з Push-сповіщень чи веб-посилань.
3. Очищення стека навігації (`//main/details`).