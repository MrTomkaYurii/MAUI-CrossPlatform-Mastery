namespace Microsoft.Maui.Controls;

/// <summary>
/// Shim for IValueConverter to enable isolated non-UI unit testing on net10.0
/// </summary>
public interface IValueConverter
{
    object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture);
    object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture);
}
