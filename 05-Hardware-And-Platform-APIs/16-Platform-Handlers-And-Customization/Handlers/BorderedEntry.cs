namespace MauiMastery.PlatformHandlers.Handlers;

// Кастомний елемент керування для тонкої адаптації під кожну ОС
public class BorderedEntry
{
    public string BorderColor { get; set; } = "#3B82F6";
    public int CornerRadius { get; set; } = 8;
    public string Placeholder { get; set; } = string.Empty;
}