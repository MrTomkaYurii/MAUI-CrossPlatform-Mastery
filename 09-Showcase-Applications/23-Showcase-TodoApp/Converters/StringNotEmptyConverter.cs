using System.Globalization;

namespace TodoApp.Converters;

/// <summary>
/// Конвертер значень для XAML: перетворює рядок на bool (true якщо не порожній).
///
/// Конвертери (IValueConverter) використовуються в XAML, коли потрібно
/// перетворити значення властивості ViewModel перед відображенням.
///
/// Приклад використання:
///   IsVisible="{Binding Description, Converter={StaticResource StringNotEmptyConverter}}"
/// Це означає: "елемент видимий, тільки якщо Description не порожній рядок".
///
/// Без конвертера нам довелося б додати зайву bool-властивість у ViewModel.
/// </summary>
public class StringNotEmptyConverter : IValueConverter
{
    /// <summary>
    /// Convert: перетворює рядок → bool (ViewModel → UI).
    ///
    /// Викликається кожен раз, коли XAML-рушій оновлює прив'язаний елемент.
    /// </summary>
    /// <param name="value">Вхідне значення (очікується string)</param>
    /// <param name="targetType">Тип цільової властивості (зазвичай bool для IsVisible)</param>
    /// <param name="parameter">Опціональний параметр конвертера (не використовуємо)</param>
    /// <param name="culture">Поточна культура (для локалізації)</param>
    /// <returns>true якщо рядок не порожній, false якщо порожній або null</returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Перевіряємо, чи value є рядком і чи він не порожній
        // !string.IsNullOrWhiteSpace: повертає false для null, "", "   "
        return value is string str && !string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// ConvertBack: зворотне перетворення bool → string (UI → ViewModel).
    /// Нам не потрібне (тільки OneWay binding), тому повертаємо null.
    /// </summary>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Зворотна конвертація не підтримується
        return null;
    }
}
